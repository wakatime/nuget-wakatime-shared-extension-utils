using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--entity] flag.
    /// </summary>
    public static class FlagEntity
    {
        #region Static Fields and Const
        
        internal const string CliFlagName = "--entity";
        private const string JsonFlagName = "entity";

        #endregion

        /// <summary>
        ///     Adds [--entity] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">
        ///     Absolute path to file for the heartbeat. Can also be a url, domain or app when --entity-type is not
        ///     file.
        /// </param>
        public static FlagHolder AddFlagEntity(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new Flag<string>(CliFlagName, FormatForCli(value), FormatForJson(value), value, ValueFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--entity] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagEntity(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
        
        private static string FormatForJson(string value) => $"\"{JsonFlagName}\": \"{JsonSerializerHelper.JsonEscape(value)}\"";

        private static string FormatForCli(string value) => $"{CliFlagName} {value}";
        
        private static Func<string, string> ValueFormatter => value => value;

    }
}