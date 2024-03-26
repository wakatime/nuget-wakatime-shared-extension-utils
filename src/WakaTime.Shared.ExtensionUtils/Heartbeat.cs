using System;
using System.Collections.Generic;
using WakaTime.Shared.ExtensionUtils.Exceptions;
using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils
{
    /// <summary>
    ///     Represents a heartbeat object for sending heartbeat to the WakaTime API.
    /// </summary>
    public class Heartbeat : FlagHolder
    {
        #region Constructors

        /// <inheritdoc />
        public Heartbeat(WakaTime wakaTime) : base(wakaTime)
        {
        }

        #endregion

        /// <summary>
        ///     Checks if the heartbeat has all the required flags set.
        /// </summary>
        /// <param name="throwException">Whether to throw an exception if the heartbeat is invalid. Default is false.</param>
        /// <returns><c>true</c> if the heartbeat is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="AggregateException">
        ///     Thrown when one or more required flags are missing and
        ///     <paramref name="throwException" /> is set to <c>true</c>.
        ///     Contains inner exceptions of type <see cref="MissingFlagException" /> with details of the missing flags.
        /// </exception>
        /// <remarks>
        ///     Required flags:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 <see cref="FlagKey" />
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <see cref="FlagPlugin" />
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <see cref="FlagEntity" />
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <see cref="FlagEntityType" />
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <see cref="FlagTime" />
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <see cref="FlagCategory" />
        ///             </description>
        ///         </item>
        ///     </list>
        /// </remarks>
        public bool IsValid(bool throwException = false)
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
                exceptions.Add(new MissingFlagException(FlagPlugin.CliFlagName,
                                                        $"Flag {FlagPlugin.CliFlagName} is required for sending heartbeat. Use {nameof(FlagPlugin.AddFlagPlugin)}."));
            }

            if (!hasEntity)
            {
                WakaTime.Logger.Error($"{FlagEntity.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagEntity.CliFlagName,
                                                        $"Flag {FlagEntity.CliFlagName} is required for sending heartbeat. Use {nameof(FlagEntity.AddFlagEntity)}."));
            }

            if (!hasEntityType)
            {
                WakaTime.Logger.Error($"{FlagEntityType.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagEntityType.CliFlagName,
                                                        $"Flag {FlagEntityType.CliFlagName} is required for sending heartbeat. Use {nameof(FlagEntityType.AddFlagEntityType)}."));
            }

            if (!hasTime)
            {
                WakaTime.Logger.Error($"{FlagTime.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(
                    new MissingFlagException(FlagTime.CliFlagName, $"Flag {FlagTime.CliFlagName} is required for sending heartbeat. Use {nameof(FlagTime.AddFlagTime)}."));
            }

            if (!hasCategory)
            {
                WakaTime.Logger.Error($"{FlagCategory.CliFlagName} is required for sending heartbeat.");
                exceptions.Add(new MissingFlagException(FlagCategory.CliFlagName,
                                                        $"Flag {FlagCategory.CliFlagName} is required for sending heartbeat. Use {nameof(FlagCategory.AddFlagCategory)}."));
            }

            if (exceptions.Count > 0 && throwException)
                throw new AggregateException("One or more flags are missing for sending heartbeat. See inner exceptions for details.", exceptions);

            return hasKey && hasPlugin && hasEntity && hasEntityType && hasTime && hasCategory;
        }
    }
}