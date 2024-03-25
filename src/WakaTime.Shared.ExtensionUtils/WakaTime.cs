using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WakaTime.Shared.ExtensionUtils.Extensions;
using WakaTime.Shared.ExtensionUtils.Flags;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils
{
    public class WakaTime : IDisposable
    {
        #region Fields

        private readonly Dependencies _dependencies;
        private readonly ConcurrentQueue<FlagHolder> _heartbeatQueue;
        private readonly Timer _heartbeatsProcessTimer;
        private readonly Metadata _metadata;
        private readonly Timer _totalTimeTodayUpdateTimer;

        public readonly ConfigFile Config;

        private string _lastFile;
        private DateTime _lastHeartbeat;

        #endregion

        #region Properties

        /// <summary>
        ///     Flags within this holder will be added to every heartbeat.
        /// </summary>
        public FlagHolder CommonFlags { get; }

        public ILogger Logger { get; }

        /// <summary>
        ///     A string like "3 hrs 42 mins".
        /// </summary>
        public string TotalTimeToday { get; private set; }

        /// <summary>
        ///     A string like "3 hrs 4 mins Coding, 20 mins Building, 18 mins Debugging".
        /// </summary>
        public string TotalTimeTodayDetailed { get; private set; }

        #endregion

        #region Constructors

        public WakaTime(Metadata metadata, ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));

            Config = new ConfigFile(Dependencies.GetConfigFilePath());
            _heartbeatQueue = new ConcurrentQueue<FlagHolder>();
            CommonFlags = new FlagHolder(this);

            _dependencies = new Dependencies(logger, Config);
            _heartbeatsProcessTimer = new Timer(10000);
            _totalTimeTodayUpdateTimer = new Timer(60000);
            _lastHeartbeat = DateTime.UtcNow.AddMinutes(-3);

            InitializeCommonFlags();
        }

        /// <summary>
        ///     Adds common flags for all Heartbeats to the <see cref="CommonFlags" /> holder.
        /// </summary>
        private void InitializeCommonFlags()
        {
            // add api key to cli flags
            CommonFlags.AddFlagKey(Config.GetSetting("api_key"));

            // use string builder to safely append plugin name and version
            var builder = new StringBuilder();
            if (!string.IsNullOrEmpty(_metadata.EditorName) && !string.IsNullOrEmpty(_metadata.EditorVersion))
                builder.Append($"{_metadata.EditorName}/{_metadata.EditorVersion}");

            if (!string.IsNullOrEmpty(_metadata.PluginName) && !string.IsNullOrEmpty(_metadata.PluginVersion))
            {
                if (builder.Length > 0) builder.Append(" ");
                builder.Append($"{_metadata.PluginName}/{_metadata.PluginVersion}");
            }

            if (builder.Length > 0) CommonFlags.AddFlagPlugin(builder.ToString());
        }

        #endregion

        #region Interfaces Implement

        public void Dispose()
        {
            if (_heartbeatsProcessTimer != null)
            {
                _heartbeatsProcessTimer.Stop();
                _heartbeatsProcessTimer.Elapsed -= ProcessHeartbeats;
                _heartbeatsProcessTimer.Dispose();
            }

            if (_totalTimeTodayUpdateTimer != null)
            {
                _totalTimeTodayUpdateTimer.Stop();
                _totalTimeTodayUpdateTimer.Elapsed -= UpdateTotalTimeToday;
                _totalTimeTodayUpdateTimer.Dispose();
            }

            // make sure the queue is empty	
            ProcessHeartbeats();
        }

        #endregion

        #region Events And Handlers

        /// <summary>
        ///     Fired when <see cref="TotalTimeToday" /> and <see cref="TotalTimeTodayDetailed" /> are updated.
        /// </summary>
        public event EventHandler<TotalTimeTodayUpdatedEventArgs> TotalTimeTodayUpdated;

        private void ProcessHeartbeats(object sender, ElapsedEventArgs e)
        {
            _ = Task.Run(() =>
            {
                // ReSharper disable once ConvertClosureToMethodGroup
                ProcessHeartbeats();
            });
        }

        private void UpdateTotalTimeToday(object sender, ElapsedEventArgs e)
        {
            _ = Task.Run(() =>
            {
                // ReSharper disable once ConvertClosureToMethodGroup
                UpdateTotalTimeToday();
            });
        }

        #endregion

        public async Task InitializeAsync()
        {
            Logger.Info($"Initializing WakaTime v{_metadata.PluginVersion}");

            try
            {
                await _dependencies.CheckAndInstall();

                _heartbeatsProcessTimer.Elapsed += ProcessHeartbeats;
                _heartbeatsProcessTimer.Start();

                UpdateTotalTimeToday(null, null); // Invoke the event handler immediately
                _totalTimeTodayUpdateTimer.Elapsed += UpdateTotalTimeToday;
                _totalTimeTodayUpdateTimer.Start();

                Logger.Info($"Finished initializing WakaTime v{_metadata.PluginVersion}");
            }
            catch (WebException ex)
            {
                Logger.Error("Are you behind a proxy? Try setting a proxy in WakaTime Settings with format https://user:pass@host:port", ex);
            }
            catch (Exception ex)
            {
                Logger.Error("Error installing dependencies", ex);
            }
        }

        /// <summary>
        ///     Sends a heartbeat to the WakaTime API.
        /// </summary>
        /// <param name="currentFile">CLI Arg: [--entity] = The current file being edited. </param>
        /// <param name="isWrite">CLI Arg: [--write] = Whether the file is being written to or not.</param>
        /// <param name="project">CLI Arg: [--alternate-project] = The project name. </param>
        /// <param name="category">CLI Arg: [--category] = The category of the file being edited. <br />
        /// <see cref="HeartbeatCategory" />:
        /// <see cref="HeartbeatCategory.Coding" />(default),
        /// <see cref="HeartbeatCategory.Building" />,
        /// <see cref="HeartbeatCategory.Indexing" />,
        /// <see cref="HeartbeatCategory.Debugging" />,
        /// <see cref="HeartbeatCategory.RunningTests" />,
        /// <see cref="HeartbeatCategory.WritingTests" />,
        /// <see cref="HeartbeatCategory.ManualTesting" />,
        /// <see cref="HeartbeatCategory.CodeReviewing" />,
        /// <see cref="HeartbeatCategory.Browsing" />,
        /// <see cref="HeartbeatCategory.Designing" />
        /// </param>
        /// <param name="entityType">CLI Arg: [--entity-type] = The type of entity being edited. <br />
        /// <see cref="EntityType" />:
        /// <see cref="EntityType.File" />(default),
        /// <see cref="EntityType.Domain" />,
        /// <see cref="EntityType.App" />
        /// </param>
        public void HandleActivity(string currentFile,
                                   bool isWrite,
                                   string project,
                                   HeartbeatCategory? category = null,
                                   EntityType? entityType = null)
        {
            if (string.IsNullOrEmpty(currentFile))
            {
                Logger.Debug("Skipping heartbeat because current file is null or empty.");
                return;
            }

            var now = DateTime.UtcNow;

            if (!isWrite && _lastFile != null && !EnoughTimePassed(now) && currentFile.Equals(_lastFile))
            {
                Logger.Debug($"Skipping heartbeat because enough time has not passed since last heartbeat for file {currentFile}");
                return;
            }

            _lastFile = currentFile;
            _lastHeartbeat = now;

            var heartbeat = this.CreateHeartbeat(currentFile, isWrite, project, category, entityType);
            HandleActivity(heartbeat);
        }

        /// <summary>
        ///     Sends a heartbeat to the WakaTime API.
        /// </summary>
        /// <param name="heartbeat">The heartbeat to send.</param>
        public void HandleActivity(FlagHolder heartbeat)
        {
            if (heartbeat is null)
            {
                Logger.Debug("Skipping heartbeat because it is null.");
                return;
            }

            // validate if it has all required flags set, skip if it doesn't
            if (!heartbeat.IsValidHeartbeat())
            {
                Logger.Debug("Skipping heartbeat because it is not valid.");
                return;
            }

            AppendHeartbeat(heartbeat);
        }

        private void AppendHeartbeat(FlagHolder heartbeat) => _heartbeatQueue.Enqueue(heartbeat);

        private bool EnoughTimePassed(DateTime now) => _lastHeartbeat < now.AddMinutes(Constants.HeartbeatFrequency * -1);

        // ReSharper disable once CognitiveComplexity
        private void ProcessHeartbeats()
        {
            try
            {
                // get first heartbeat from queue
                bool gotOne = _heartbeatQueue.TryDequeue(out var heartbeat);
                if (!gotOne) return;

                // pop all extra heartbeats from queue
                var extraHeartbeats = new Collection<FlagHolder>();
                while (_heartbeatQueue.TryDequeue(out var h))
                    extraHeartbeats.Add(h);

                bool hasExtraHeartbeats = extraHeartbeats.Count > 0;
                string extraHeartbeatsString = null;

                if (hasExtraHeartbeats)
                {
                    heartbeat.AddFlagExtraHeartbeats();
                    extraHeartbeatsString = JsonSerializerHelper.ToJson(extraHeartbeats);
                }

                string binary = _dependencies.GetCliLocation();
                var process = new RunProcess(binary, heartbeat.FlagsToCliArgsArray());
                
                var args = heartbeat.FlagsToCliArgsArray();
                var x = args
                   .Aggregate(string.Empty, (current, arg) => current + "\"" + arg + "\" ")
                   .TrimEnd(' ');
                
                Logger.Debug($"BINARY: {binary}");
                Logger.Debug($"FLAGS: {x}");

                Logger.Debug($"[\"{binary}\", \"{string.Join("\", \"", heartbeat.FlagsToCliArgsArray(true))}\"]");

                if (Config.GetSettingAsBoolean("debug"))
                {
                    process.Run(extraHeartbeatsString);

                    if (!string.IsNullOrEmpty(process.Output))
                        Logger.Debug(process.Output);

                    if (!string.IsNullOrEmpty(process.Error))
                        Logger.Debug(process.Error);
                }
                else
                {
                    process.RunInBackground(extraHeartbeatsString);
                }

                if (process.Success)
                {
                    Logger.Debug("Heartbeat sent successfully.");
                    return;
                }

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

        private void UpdateTotalTimeToday()
        {
            string binary = _dependencies.GetCliLocation();
            string apiKey = Config.GetSetting("api_key");

            Logger.Debug("Fetching TotalTimeToday...");
            var totalTimeTodayProcess = new RunProcess(binary, "--key", apiKey, "--today", "--today-hide-categories", "true");
            totalTimeTodayProcess.Run();
            string totalTimeToday = totalTimeTodayProcess.Output.Trim();
            Logger.Debug($"Fetched TotalTimeToday: {totalTimeToday}");

            if (!string.IsNullOrEmpty(totalTimeToday)) TotalTimeToday = totalTimeToday;

            Logger.Debug("Fetching TotalTimeTodayDetailed...");
            var totalTimeTodayDetailedProcess = new RunProcess(binary, "--key", apiKey, "--today", "--today-hide-categories", "false");
            totalTimeTodayDetailedProcess.Run();
            string totalTimeTodayDetailed = totalTimeTodayDetailedProcess.Output.Trim();
            Logger.Debug($"Fetched TotalTimeTodayDetailed: {totalTimeTodayDetailed}");

            if (!string.IsNullOrEmpty(totalTimeTodayDetailed)) TotalTimeTodayDetailed = totalTimeTodayDetailed;

            // If fetch was successful, fire "TotalTimeTodayUpdated" event
            if (!(string.IsNullOrEmpty(totalTimeToday) && string.IsNullOrEmpty(totalTimeTodayDetailed)))
                TotalTimeTodayUpdated?.Invoke(this, new TotalTimeTodayUpdatedEventArgs(TotalTimeToday, TotalTimeTodayDetailed));
        }
    }
}