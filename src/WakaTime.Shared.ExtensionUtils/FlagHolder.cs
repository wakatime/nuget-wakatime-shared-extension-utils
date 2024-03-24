using System.Collections.Generic;
using System.Linq;
using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils
{
    /// <summary>
    ///     Represents an object holding flags with values for the CLI arguments.
    /// </summary>
    public class FlagHolder
    {
        #region Fields
        
        private readonly Dictionary<string, IFlag> _flags = new Dictionary<string, IFlag>();
        internal readonly WakaTime WakaTime;

        #endregion

        #region Properties

        public IReadOnlyDictionary<string, IFlag> Flags => _flags;

        #endregion

        #region Constructors

        public FlagHolder(WakaTime wakaTime) => WakaTime = wakaTime;

        #endregion

        public void AddFlag(IFlag flag, bool overwrite = true)
        {
            if (_flags.ContainsKey(flag.FlagUniqueName))
            {
                if (!overwrite)
                {
                    WakaTime.Logger.Debug($"Flag {flag.FlagUniqueName} already exists. But the {nameof(overwrite)} flag is set to false. Cannot overwrite.");
                    return;
                }

                WakaTime.Logger.Debug($"Flag {flag.FlagUniqueName} already exists. Overwriting.");
                _flags[flag.FlagUniqueName] = flag;
            }
            else
            {
                WakaTime.Logger.Debug($"Flag {flag.FlagUniqueName} does not exist. Adding.");
                _flags.Add(flag.FlagUniqueName, flag);
            }
        }

        internal void AddFlags(IEnumerable<IFlag> flags, bool overwrite = true)
        {
            foreach (var flag in flags) AddFlag(flag, overwrite);
        }

        public void RemoveFlag(IFlag flag) => RemoveFlag(flag.FlagUniqueName);

        internal void RemoveFlags(IEnumerable<IFlag> flags)
        {
            foreach (var flag in flags) RemoveFlag(flag);
        }

        public void RemoveFlag(string flagUniqueName)
        {
            bool flagExists = _flags.TryGetValue(flagUniqueName, out var existingFlag);
            if (!flagExists)
            {
                WakaTime.Logger.Debug($"Flag {flagUniqueName} does not exist. Cannot remove.");
                return;
            }

            WakaTime.Logger.Debug($"Flag {flagUniqueName} exists. Removing.");
            _flags.Remove(flagUniqueName);
        }

        internal void RemoveFlags(IEnumerable<string> flagUniqueNames)
        {
            foreach (string flagUniqueName in flagUniqueNames) RemoveFlag(flagUniqueName);
        }

        public bool IsValidHeartbeat()
        {
            bool hasEntity = HasFlag(FlagEntity.CliFlagName);
            bool hasEntityType = HasFlag(FlagEntityType.CliFlagName);
            bool hasTime = HasFlag(FlagTime.CliFlagName);
            bool hasCategory = HasFlag(FlagCategory.CliFlagName);

            if (!hasEntity) WakaTime.Logger.Error("Entity is required for sending heartbeat.");
            if (!hasEntityType) WakaTime.Logger.Error("Entity type is required for sending heartbeat.");
            if (!hasTime) WakaTime.Logger.Error("Time is required for sending heartbeat.");
            if (!hasCategory) WakaTime.Logger.Error("Category is required for sending heartbeat.");

            return hasEntity && hasEntityType && hasTime && hasCategory;
        }

        private bool HasFlag(string cliFlagName) => Flags.ContainsKey(cliFlagName);

        internal IFlag GetFlag(string flagUniqueName) => Flags.TryGetValue(flagUniqueName, out var flag) ? flag : null;

        internal string FlagsToCliArgs() => string.Join(" ", Flags.Values);

        internal string[] FlagsToCliArgsArray(bool obfuscate = false)
        {
            return Flags.Values.Select(flag => flag.GetFormattedForCli(obfuscate))
                        .ToArray();
        }
    }
}