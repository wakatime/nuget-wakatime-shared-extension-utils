using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;

namespace WakaTime.Shared.ExtensionUtils
{
    public class WakaTime : IDisposable
    {
        private readonly Configuration _configuration;
        private readonly CliParameters _cliParameters;
        private readonly Dependencies _dependencies;
        private readonly Timer _timer;

        private string _lastFile;
        private DateTime _lastHeartbeat;

        public readonly ConfigFile Config = new ConfigFile();
        public readonly ConcurrentQueue<Heartbeat> HeartbeatQueue = new ConcurrentQueue<Heartbeat>();

        public ILogger Logger { get; }

        public WakaTime(Configuration configuration, ILogger logger)
        {
            if (configuration is null)
                throw new ArgumentNullException("configuration");

            if (logger is null)
                throw new ArgumentNullException("logger");

            _configuration = configuration;
            _cliParameters = new CliParameters();
            _dependencies = new Dependencies(logger);
            _timer = new Timer(10000);
            _lastHeartbeat = DateTime.UtcNow.AddMinutes(-3);

            Logger = logger;

            if (string.IsNullOrEmpty(_configuration.PluginVersion))
                _configuration.PluginVersion = Constants.PluginVersion;
        }

        public void Initialize()
        {
            Logger.Info($"Initializing WakaTime v{_configuration.PluginVersion}");
            Logger.Debug("Using standalone wakatime-cli.");

            try
            {
                if (!_dependencies.DoesCliExist() || !_dependencies.IsCliUpToDate())
                    _dependencies.DownloadAndInstallCli();
            }
            catch (WebException ex)
            {
                Logger.Error($"Are you behind a proxy? Try setting a proxy in WakaTime Settings with format https://user:pass@host:port", ex);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error detecting dependencies", ex);
            }

            _timer.Elapsed += ProcessHeartbeats;
            _timer.Start();

            Logger.Info($"Finished initializing WakaTime v{_configuration.PluginVersion}");
        }

        public void HandleActivity(string currentFile, bool isWrite, string project,
            HeartbeatCategory? category = null, EntityType? entityType = null)
        {
            if (currentFile == null)
                return;

            var now = DateTime.UtcNow;

            if (!isWrite && _lastFile != null && !EnoughTimePassed(now) && currentFile.Equals(_lastFile))
                return;

            _lastFile = currentFile;
            _lastHeartbeat = now;

            AppendHeartbeat(currentFile, isWrite, now, project, category, entityType);
        }

        private void AppendHeartbeat(string fileName, bool isWrite, DateTime time, string project,
            HeartbeatCategory? category, EntityType? entityType)
        {
            var h = new Heartbeat
            {
                Entity = fileName,
                Timestamp = ToUnixEpoch(time),
                IsWrite = isWrite,
                Project = project,
                Category = category,
                EntityType = entityType
            };

            HeartbeatQueue.Enqueue(h);
        }

        private bool EnoughTimePassed(DateTime now)
        {
            return _lastHeartbeat < now.AddMinutes(Constants.HeartbeatFrequency * -1);
        }

        private static string ToUnixEpoch(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = date - epoch;
            var seconds = Convert.ToInt64(Math.Floor(timestamp.TotalSeconds));
            var milliseconds = timestamp.ToString("ffffff");
            return $"{seconds}.{milliseconds}";
        }

        private void ProcessHeartbeats(object sender, ElapsedEventArgs e)
        {
            _ = Task.Run(() =>
              {
                  // ReSharper disable once ConvertClosureToMethodGroup
                  ProcessHeartbeats();
              });
        }

        private void ProcessHeartbeats()
        {
            var binary = _dependencies.CliLocation;

            // get first heartbeat from queue
            var gotOne = HeartbeatQueue.TryDequeue(out var heartbeat);
            if (!gotOne)
                return;

            // pop all extra heartbeats from queue
            var extraHeartbeats = new ArrayList();
            while (HeartbeatQueue.TryDequeue(out var h))
                extraHeartbeats.Add(h);

            var hasExtraHeartbeats = extraHeartbeats.Count > 0;

            _cliParameters.Key = Config.ApiKey;
            _cliParameters.Plugin =
                $"{_configuration.EditorName}/{_configuration.EditorVersion} {_configuration.PluginName}/{_configuration.PluginVersion}";
            _cliParameters.File = heartbeat.Entity;
            _cliParameters.Time = heartbeat.Timestamp;
            _cliParameters.IsWrite = heartbeat.IsWrite;
            _cliParameters.Project = heartbeat.Project;
            _cliParameters.Category = heartbeat.Category;
            _cliParameters.EntityType = heartbeat.EntityType;
            _cliParameters.HasExtraHeartbeats = hasExtraHeartbeats;

            string extraHeartbeatsJson = null;
            if (hasExtraHeartbeats)
                extraHeartbeatsJson = new JavaScriptSerializer().Serialize(extraHeartbeats);

            var process = new RunProcess(binary, _cliParameters.ToArray());

            if (Config.Debug)
            {
                Logger.Debug(
                    $"[\"{binary}\", \"{string.Join("\", \"", _cliParameters.ToArray(true))}\"]");

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
