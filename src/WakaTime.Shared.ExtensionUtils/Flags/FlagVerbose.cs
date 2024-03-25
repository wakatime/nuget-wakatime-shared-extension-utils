using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--verbose] flag. <br /> <br />
    ///     Add: <see cref="AddFlagVerbose" /> <br />
    ///     Remove: <see cref="RemoveFlagVerbose" /> <br />
    /// </summary>
    public static class FlagVerbose
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--verbose</value>
        /// </summary>
        internal const string CliFlagName = "--verbose";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "verbose";

        #endregion

        #region Properties

        /// <summary>
        ///     Formats the value for the string representation.
        /// </summary>
        /// <remarks>
        ///     Always returns empty string as the verbose flag should not be serialized to JSON.
        /// </remarks>
        private static Func<bool, bool, string> JsonFormatter => (v, b) => string.Empty;

        /// <summary>
        ///     Formats the value for CLI arguments.
        /// </summary>
        private static Func<bool, bool, string> CliFormatter => (v, b) => !v ? string.Empty : $"{CliFlagName}";

        /// <summary>
        ///     Formats the value for the string representation.
        /// </summary>
        private static Func<bool, bool, string> ValueFormatter => (v, b) => v.ToString()
                                                                             .ToLower();

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
    }
}