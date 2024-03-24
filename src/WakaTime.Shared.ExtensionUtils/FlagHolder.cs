using System.Collections.Generic;
using System.Text;
using WakaTime.Shared.ExtensionUtils.Flags;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils
{
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

        public void RemoveFlag(IFlag flag) => RemoveFlag(flag.FlagUniqueName);

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



        
    }
}