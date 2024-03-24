using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--verbose] flag.
    /// </summary>
    public static class FlagVerbose
    {
        #region Static Fields and Const

        private const string CliFlagName = "--verbose";
        private const string JsonFlagName = "verbose";

        #endregion

        /// <summary>
        ///     Adds [--verbose] flag to the CLI arguments. <br />
        ///     Turns on debug messages in log file.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Boolean value to set the flag to. True by default.</param>
        public static FlagHolder AddFlagVerbose(this FlagHolder flagHolder, bool value = true)
        {
            flagHolder.AddFlag(new Flag<bool>(CliFlagName, FormatForCli(value), FormatForJson(value), value, ValueFormatter, false));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--verbose] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagVerbose(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
        
        private static string FormatForJson(bool value) => $"\"{JsonFlagName}\": {JsonSerializerHelper.JsonEscape(value.ToString().ToLower())}";

        private static string FormatForCli(bool value) => !value ? string.Empty : $"{CliFlagName}";

        private static Func<bool, string> ValueFormatter => value => value.ToString().ToLower();
    }
}