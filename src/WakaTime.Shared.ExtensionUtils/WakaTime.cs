using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace WakaTime.Shared.ExtensionUtils
{
    public class WakaTime : IDisposable
    {
        private readonly Metadata _metadata;
        private readonly CliParameters _cliParameters;
        private readonly Dependencies _dependencies;
        private readonly Timer _timer;
        private readonly bool _newBetaCli;

        private string _lastFile;
        private DateTime _lastHeartbeat;

        public readonly ConfigFile Config;
        public readonly ConcurrentQueue<Heartbeat> HeartbeatQueue;

        public ILogger Logger { get; }

        public WakaTime(Metadata metadata, ILogger logger)
        {
            if (metadata is null)
                throw new ArgumentNullException("metadata");

            if (logger is null)
                throw new ArgumentNullException("logger");

            Logger = logger;
            Config = new ConfigFile(Dependencies.GetConfigFilePath());
            HeartbeatQueue = new ConcurrentQueue<Heartbeat>();

            _metadata = metadata;
            _cliParameters = new CliParameters
            {
                Key = Config.GetSetting("api_key"),
                Plugin =
                $"{_metadata.EditorName}/{_metadata.EditorVersion} {_metadata.PluginName}/{_metadata.PluginVersion}"
            };
            _dependencies = new Dependencies(logger, Config);
            _timer = new Timer(10000);
            _lastHeartbeat = DateTime.UtcNow.AddMinutes(-3);

            _newBetaCli = Config.GetSettingAsBoolean("new_beta_cli", true);
        }

        public async Task InitializeAsync()
        {
            Logger.Info($"Initializing WakaTime v{_metadata.PluginVersion}");

            try
            {
                await _dependencies.CheckAndInstall();

                _timer.Elapsed += ProcessHeartbeats;
                _timer.Start();

                Logger.Info($"Finished initializing WakaTime v{_metadata.PluginVersion}");
            }
            catch (WebException ex)
            {
                Logger.Error($"Are you behind a proxy? Try setting a proxy in WakaTime Settings with format https://user:pass@host:port", ex);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error installing dependencies", ex);
            }
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
            try
            {
                var binary = _dependencies.GetCliLocation(_newBetaCli);

                // get first heartbeat from queue
                var gotOne = HeartbeatQueue.TryDequeue(out var heartbeat);
                if (!gotOne)
                    return;

                // pop all extra heartbeats from queue
                var extraHeartbeats = new Collection<Heartbeat>();
                while (HeartbeatQueue.TryDequeue(out var h))
                    extraHeartbeats.Add(h);

                var hasExtraHeartbeats = extraHeartbeats.Count > 0;

                _cliParameters.File = heartbeat.Entity;
                _cliParameters.Time = heartbeat.Timestamp;
                _cliParameters.IsWrite = heartbeat.IsWrite;
                _cliParameters.Project = heartbeat.Project;
                _cliParameters.Category = heartbeat.Category;
                _cliParameters.EntityType = heartbeat.EntityType;
                _cliParameters.HasExtraHeartbeats = hasExtraHeartbeats;

                string extraHeartbeatsString = null;
                if (hasExtraHeartbeats)
                    extraHeartbeatsString = JSONSerializer.SerializeArrayHeartbeat(extraHeartbeats);

                var process = new RunProcess(binary, _cliParameters.ToArray());

                if (Config.GetSettingAsBoolean("debug"))
                {
                    Logger.Debug(
                        $"[\"{binary}\", \"{string.Join("\", \"", _cliParameters.ToArray(true))}\"]");

                    process.Run(extraHeartbeatsString);

                    if (!string.IsNullOrEmpty(process.Output))
                        Logger.Debug(process.Output);

                    if (!string.IsNullOrEmpty(process.Error))
                        Logger.Debug(process.Error);
                }
                else
                    process.RunInBackground(extraHeartbeatsString);

                if (process.Success) return;

                Logger.Error("Could not send heartbeat.");

                if (!string.IsNullOrEmpty(process.Output))
                    Logger.Error(process.Output);

                if (!string.IsNullOrEmpty(process.Error))
                    Logger.Error(process.Error);
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing heartbeat(s).", ex);
            }
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
