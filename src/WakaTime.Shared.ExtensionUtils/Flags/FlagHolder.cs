using System.Collections.Generic;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    public class FlagHolder
    {
        #region Fields

        private readonly Dictionary<string, ICliFlag> _flags = new Dictionary<string, ICliFlag>();
        internal readonly WakaTime WakaTime;

        #endregion

        #region Properties

        public IReadOnlyDictionary<string, ICliFlag> Flags => _flags;

        #endregion

        #region Constructors

        public FlagHolder(WakaTime wakaTime) => WakaTime = wakaTime;

        #endregion

        public void AddFlag(ICliFlag flag, bool overwrite = true)
        {
            if (_flags.ContainsKey(flag.FlagName))
            {
                if (!overwrite)
                {
                    WakaTime.Logger.Debug($"Flag {flag.FlagName} already exists. But the {nameof(overwrite)} flag is set to false. Cannot overwrite.");
                    return;
                }

                WakaTime.Logger.Debug($"Flag {flag.FlagName} already exists. Overwriting.");
                _flags[flag.FlagName] = flag;
            }
            else
            {
                WakaTime.Logger.Debug($"Flag {flag.FlagName} does not exist. Adding.");
                _flags.Add(flag.FlagName, flag);
            }
        }

        public void RemoveFlag(ICliFlag flag) => RemoveFlag(flag.FlagName);

        public void RemoveFlag(string flag)
        {
            bool flagExists = _flags.TryGetValue(flag, out var existingFlag);
            if (!flagExists)
            {
                WakaTime.Logger.Debug($"Flag {flag} does not exist. Cannot remove.");
                return;
            }

            WakaTime.Logger.Debug($"Flag {flag} exists. Removing.");
            _flags.Remove(flag);
        }
    }
}