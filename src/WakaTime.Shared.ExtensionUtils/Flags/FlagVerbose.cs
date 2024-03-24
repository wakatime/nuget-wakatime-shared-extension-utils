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

        internal const string CliFlagName = "--verbose";
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
            flagHolder.AddFlag(new Flag<bool>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter));
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
        
        private static Func<bool, bool, string> JsonFormatter => (v, b) =>
        {
            string formattedValue = ValueFormatter.Invoke(v, b);
            return string.IsNullOrEmpty(formattedValue) ? string.Empty : $"\"{JsonFlagName}\": {JsonSerializerHelper.JsonEscape(formattedValue)}";
        };

        private static Func<bool, bool, string> CliFormatter => (v, b) => !v ? string.Empty : $"{CliFlagName}";

        private static Func<bool, bool, string> ValueFormatter => (v, b) => v.ToString()
                                                                             .ToLower();
    }
}