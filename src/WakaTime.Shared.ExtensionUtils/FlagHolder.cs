using System.Collections.Generic;
using System.Linq;
using WakaTime.Shared.ExtensionUtils.Flags;
using WakaTime.Shared.ExtensionUtils.Helpers;

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

        /// <summary>
        ///     A collection of flags set for this instance.
        /// </summary>
        public IReadOnlyDictionary<string, IFlag> Flags => _flags;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlagHolder" /> class.
        /// </summary>
        /// <param name="wakaTime">The <see cref="WakaTime" /> instance.</param>
        public FlagHolder(WakaTime wakaTime) => WakaTime = wakaTime;

        #endregion

        /// <summary>
        ///     Adds a flag to the collection.
        /// </summary>
        /// <param name="flag">The flag to add.</param>
        /// <param name="overwrite">Whether to overwrite the flag if it already exists. True by default.</param>
        internal void AddFlag(IFlag flag, bool overwrite = true)
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

        /// <summary>
        ///     Adds multiple flags to the collection.
        /// </summary>
        /// <param name="flags">The flags to add.</param>
        /// <param name="overwrite">Whether to overwrite the flag if it already exists. True by default.</param>
        internal void AddFlags(IEnumerable<IFlag> flags, bool overwrite = true)
        {
            foreach (var flag in flags) AddFlag(flag, overwrite);
        }

        /// <summary>
        ///     Removes a flag from the collection.
        ///     If the flag does not exist, it will not throw any exceptions.
        /// </summary>
        /// <param name="flag">The flag to remove.</param>
        internal void RemoveFlag(IFlag flag) => RemoveFlag(flag.FlagUniqueName);

        /// <summary>
        ///     Removes multiple flags from the collection.
        /// </summary>
        /// <param name="flags"></param>
        internal void RemoveFlags(IEnumerable<IFlag> flags)
        {
            foreach (var flag in flags) RemoveFlag(flag);
        }

        /// <summary>
        ///     Removes a flag from the collection by its unique name.
        /// </summary>
        /// <param name="flagUniqueName">The unique name of the flag to remove.</param>
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

        /// <summary>
        ///     Removes multiple flags from the collection by their unique names.
        /// </summary>
        /// <param name="flagUniqueNames">The unique names of the flags to remove.</param>
        internal void RemoveFlags(IEnumerable<string> flagUniqueNames)
        {
            foreach (string flagUniqueName in flagUniqueNames) RemoveFlag(flagUniqueName);
        }


        /// <summary>
        ///     Checks if the flag exists in the collection.
        /// </summary>
        /// <param name="flagUniqueName">The unique name of the flag to check.</param>
        /// <returns><c>true</c> if the flag exists; otherwise, <c>false</c>.</returns>
        public bool HasFlag(string flagUniqueName) => Flags.ContainsKey(flagUniqueName);

        /// <summary>
        ///     Gets the flag by its unique name.
        /// </summary>
        /// <param name="flagUniqueName">The unique name of the flag to get.</param>
        /// <returns>The flag if it exists; otherwise, <c>null</c>.</returns>
        public IFlag GetFlag(string flagUniqueName) => Flags.TryGetValue(flagUniqueName, out var flag) ? flag : null;

        /// <summary>
        ///     Converts the flags to CLI arguments array.
        /// </summary>
        /// <param name="obfuscate">
        ///     Whether to obfuscate the values. Default is false.
        ///     <seealso cref="IFlag.CanObfuscate" />
        /// </param>
        /// <returns>Array of CLI arguments.</returns>
#if DEBUG
        public
#else
        internal
#endif
            string[] FlagsToCliArgsArray(bool obfuscate = false)
        {
            return Flags.Values.Select(flag => flag.GetFormattedForCli(obfuscate))
                        .Where(cli => !string.IsNullOrWhiteSpace(cli))
                        .ToArray();
        }

#if DEBUG
        public
#else
        internal
#endif
            string FlagsToJson(bool isExtraHeartbeat = true, bool obfuscate = false)
        {
            return JsonSerializerHelper.ToJson(this, isExtraHeartbeat, obfuscate);
        }
    }
}