using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;
using WakaTime.Shared.ExtensionUtils.AsyncPackageHelpers;

namespace WakaTime.Shared.ExtensionUtils
{
    public class WakaTime : IDisposable
    {
        private const int HeartbeatFrequency = 2; // minutes

        private readonly Configuration _configuration;
        private readonly PythonCliParameters _pythonCliParameters;
        private readonly Timer _timer = new Timer();
        private readonly Dependencies _dependencies;
        private string _lastFile;
        private DateTime _lastHeartbeat = DateTime.UtcNow.AddMinutes(-3);

        public readonly ConcurrentQueue<Heartbeat> HeartbeatQueue = new ConcurrentQueue<Heartbeat>();
        public ILogger Logger { get; }
        public ConfigFile Config { get; }
        public bool IsAsyncLoadSupported { get; }

        public WakaTime(IServiceProvider serviceProvider, Configuration configuration, ILogger logger)
            : this(serviceProvider, configuration)
        {
            Logger = logger;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public WakaTime(IServiceProvider serviceProvider, Configuration configuration)
        {
            _configuration = configuration;
            Config = new ConfigFile();
            Config.Read();

            _pythonCliParameters = new PythonCliParameters();
            _dependencies = new Dependencies();

            if (Logger == null)
                Logger = new Logger();

            if (serviceProvider != null)
                IsAsyncLoadSupported = ServiceProviderHelper.IsAsyncPackageSupported(serviceProvider);
        }

        public void InitializeAsync()
        {
            Logger.Info($"Initializing WakaTime v{Constants.PluginVersion}");
            Logger.Debug("Using standalone wakatime-cli.");

            try
            {
                if (!_dependencies.DoesCliExist() || !_dependencies.IsCliUpToDate())
                    _dependencies.DownloadAndInstallCli();
            }
            catch (WebException ex)
            {
                Logger.Error("Are you behind a proxy? Try setting a proxy in WakaTime Settings with format https://user:pass@host:port. Exception Traceback:", ex);
            }
            catch (Exception ex)
            {
                Logger.Error("Error detecting dependencies. Exception Traceback:", ex);
            }

            _timer.Interval = _configuration.TimerInterval;
            _timer.Elapsed += ProcessHeartbeats;
            _timer.Start();

            Logger.Info($"Finished initializing WakaTime v{Constants.PluginVersion}");
        }

        public void HandleActivity(string currentFile, bool isWrite, string project)
        {
            if (currentFile == null)
                return;

            var now = DateTime.UtcNow;

            if (!isWrite && _lastFile != null && !EnoughTimePassed(now) && currentFile.Equals(_lastFile))
                return;

            _lastFile = currentFile;
            _lastHeartbeat = now;

            AppendHeartbeat(currentFile, isWrite, now, project);
        }

        private void AppendHeartbeat(string fileName, bool isWrite, DateTime time, string project)
        {
            var h = new Heartbeat
            {
                Entity = fileName,
                Timestamp = ToUnixEpoch(time),
                IsWrite = isWrite,
                Project = project
            };
            HeartbeatQueue.Enqueue(h);
        }

        private void ProcessHeartbeats(object sender, ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                // ReSharper disable once ConvertClosureToMethodGroup
                ProcessHeartbeats();
            });
        }

        private void ProcessHeartbeats()
        {
            var pythonBinary = _dependencies.CliLocation;
            
            // get first heartbeat from queue
            var gotOne = HeartbeatQueue.TryDequeue(out var heartbeat);
            if (!gotOne)
                return;

            // remove all extra heartbeats from queue
            var extraHeartbeats = new ArrayList();
            while (HeartbeatQueue.TryDequeue(out var h))
                extraHeartbeats.Add(h);
            var hasExtraHeartbeats = extraHeartbeats.Count > 0;

            _pythonCliParameters.Key = Config.ApiKey;
            _pythonCliParameters.Plugin =
                $"{_configuration.EditorName}/{_configuration.EditorVersion} {_configuration.PluginName}/{Constants.PluginVersion}";
            _pythonCliParameters.File = heartbeat.Entity;
            _pythonCliParameters.Time = heartbeat.Timestamp;
            _pythonCliParameters.IsWrite = heartbeat.IsWrite;
            _pythonCliParameters.Project = heartbeat.Project;
            _pythonCliParameters.HasExtraHeartbeats = hasExtraHeartbeats;

            string extraHeartbeatsJson = null;
            if (hasExtraHeartbeats)
                extraHeartbeatsJson = new JavaScriptSerializer().Serialize(extraHeartbeats);

            var process = new RunProcess(pythonBinary, _pythonCliParameters.ToArray());
            if (Config.Debug)
            {
                Logger.Debug(
                    $"[\"{pythonBinary}\", \"{string.Join("\", \"", _pythonCliParameters.ToArray(true))}\"]");
                process.Run(extraHeartbeatsJson);
                if (!string.IsNullOrEmpty(process.Output))
                    Logger.Debug(process.Output);
                if (!string.IsNullOrEmpty(process.Error))
                    Logger.Debug(process.Error);
            }
            else
                process.RunInBackground(extraHeartbeatsJson);

            if (process.Success) return;

            Logger.Error("Could not send heartbeat.");
            if (!string.IsNullOrEmpty(process.Output))
                Logger.Error(process.Output);
            if (!string.IsNullOrEmpty(process.Error))
                Logger.Error(process.Error);
        }

        private bool EnoughTimePassed(DateTime now)
        {
            return _lastHeartbeat < now.AddMinutes(-1 * HeartbeatFrequency);
        }

        private static string ToUnixEpoch(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = date - epoch;
            var seconds = Convert.ToInt64(Math.Floor(timestamp.TotalSeconds));
            var milliseconds = timestamp.ToString("ffffff");
            return $"{seconds}.{milliseconds}";
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= ProcessHeartbeats;
                _timer.Dispose();
            }

            // make sure the queue is empty	
            ProcessHeartbeats();
        }

        public static class CoreAssembly
        {
            private static readonly Assembly Reference = typeof(CoreAssembly).Assembly;
            public static readonly Version Version = Reference.GetName().Version;
        }
    }
}