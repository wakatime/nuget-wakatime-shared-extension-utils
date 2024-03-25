using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WakaTime.Shared.ExtensionUtils.Exceptions;
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
        public void RemoveFlag(IFlag flag) => RemoveFlag(flag.FlagUniqueName);

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
        ///     Checks if the heartbeat has all the required flags set.
        /// </summary>
        /// <param name="throwException">Whether to throw an exception if the heartbeat is invalid. Default is false.</param>
        /// <returns><c>true</c> if the heartbeat is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="AggregateException">Thrown when one or more flags are missing for sending heartbeat.</exception>
        public bool IsValidHeartbeat(bool throwException = false)
        {
            var exceptions = new List<MissingFlagException>();
            
            bool hasKey = HasFlag(FlagKey.CliFlagName);
            bool hasPlugin = HasFlag(FlagPlugin.CliFlagName);
            bool hasEntity = HasFlag(FlagEntity.CliFlagName);
            bool hasEntityType = HasFlag(FlagEntityType.CliFlagName);
            bool hasTime = HasFlag(FlagTime.CliFlagName);
            bool hasCategory = HasFlag(FlagCategory.CliFlagName);

            if (!hasKey)
            {
                WakaTime.Logger.Error($"{FlagKey.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagKey.CliFlagName, $"Flag {FlagKey.CliFlagName} is required for sending heartbeat. Use {nameof(FlagKey.AddFlagKey)}."));
            }
            
            if (!hasPlugin)
            {
                WakaTime.Logger.Error($"{FlagPlugin.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagPlugin.CliFlagName, $"Flag {FlagPlugin.CliFlagName} is required for sending heartbeat. Use {nameof(FlagPlugin.AddFlagPlugin)}."));
            }
            
            if (!hasEntity)
            {
                WakaTime.Logger.Error($"{FlagEntity.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagEntity.CliFlagName, $"Flag {FlagEntity.CliFlagName} is required for sending heartbeat. Use {nameof(FlagEntity.AddFlagEntity)}."));
            }
            
            if (!hasEntityType)
            {
                WakaTime.Logger.Error($"{FlagEntityType.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagEntityType.CliFlagName, $"Flag {FlagEntityType.CliFlagName} is required for sending heartbeat. Use {nameof(FlagEntityType.AddFlagEntityType)}."));
            }
            
            if (!hasTime)
            {
                WakaTime.Logger.Error($"{FlagTime.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagTime.CliFlagName, $"Flag {FlagTime.CliFlagName} is required for sending heartbeat. Use {nameof(FlagTime.AddFlagTime)}."));
            }
            
            if (!hasCategory)
            {
                WakaTime.Logger.Error($"{FlagCategory.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagCategory.CliFlagName, $"Flag {FlagCategory.CliFlagName} is required for sending heartbeat. Use {nameof(FlagCategory.AddFlagCategory)}."));
            }
            
            if (exceptions.Count > 0 && throwException)
            {
                throw new AggregateException($"One or more flags are missing for sending heartbeat. See inner exceptions for details.", exceptions);
            }
            
            return hasKey && hasPlugin && hasEntity && hasEntityType && hasTime && hasCategory;
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
        internal string[] FlagsToCliArgsArray(bool obfuscate = false)
        {
            return Flags.Values.Select(flag => flag.GetFormattedForCli(obfuscate))
                        .Where(cli => !string.IsNullOrWhiteSpace(cli))
                        .ToArray();
        }
    }
}