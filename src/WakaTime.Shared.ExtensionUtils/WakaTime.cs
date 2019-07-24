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
        private readonly Configuration _configuration;
        private readonly PythonCliParameters _pythonCliParameters = new PythonCliParameters();
        private readonly Timer _timer = new Timer();

        public readonly ConcurrentQueue<Heartbeat> HeartbeatQueue = new ConcurrentQueue<Heartbeat>();

        public ConfigFile Config { get; }
        public bool IsAsyncLoadSupported { get; }

        public WakaTime(IServiceProvider serviceProvider, Configuration configuration)
        {
            _configuration = configuration;
            Config = new ConfigFile();
            Config.Read();

            IsAsyncLoadSupported = ServiceProviderHelper.IsAsyncPackageSupported(serviceProvider);
        }

        public void InitializeAsync()
        {
            Logger.Info($"Initializing WakaTime v{Constants.PluginVersion}");

            try
            {
                // Make sure python is installed
                if (!Dependencies.IsPythonInstalled())
                {
                    Dependencies.DownloadAndInstallPython();
                }

                if (!Dependencies.DoesCliExist() || !Dependencies.IsCliUpToDate())
                {
                    Dependencies.DownloadAndInstallCli();
                }
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

        private void ProcessHeartbeats(object sender, ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                ProcessHeartbeats();
            });
        }

        private void ProcessHeartbeats()
        {
            var pythonBinary = Dependencies.GetPython();
            if (pythonBinary != null)
            {
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
                    $"{_configuration.EditorName}/{_configuration.EditorVersion} {Constants.PluginName}/{Constants.PluginVersion}";
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
            else
                Logger.Error("Could not send heartbeat because python is not installed");
        }

        public static class CoreAssembly
        {
            private static readonly Assembly Reference = typeof(CoreAssembly).Assembly;
            public static readonly Version Version = Reference.GetName().Version;
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
    }
}