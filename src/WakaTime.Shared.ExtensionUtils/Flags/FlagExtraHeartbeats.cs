using System;
using System.Collections.Generic;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--extra-heartbeats] flag.
    /// </summary>
    public static class FlagExtraHeartbeats
    {
        #region Static Fields and Const

        internal const string CliFlagName = "--extra-heartbeats";
        private const string JsonFlagName = "extra_heartbeats";

        #endregion

        /// <summary>
        ///     Adds [--extra-heartbeats] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">A JSON string array of extra heartbeats to send.</param>
        public static FlagHolder AddFlagExtraHeartbeats(this FlagHolder flagHolder, CliHeartbeat[] value)
        {
            flagHolder.AddFlag(new Flag<CliHeartbeat[]>(CliFlagName, FormatForCli(value), FormatForJson(value), value, ValueFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--extra-heartbeats] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagExtraHeartbeats(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        private static string ToJsonString(IEnumerable<CliHeartbeat> value) => JsonSerializerHelper.ToJson(value);
        
        private static string FormatForJson(IEnumerable<CliHeartbeat> value) => $"\"{JsonFlagName}\": {ToJsonString(value)}";

        private static string FormatForCli(IEnumerable<CliHeartbeat> value) => $"{CliFlagName} {ToJsonString(value)}";
        
        private static Func<CliHeartbeat[], string> ValueFormatter => ToJsonString;
    }
}