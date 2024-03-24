using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--write] flag.
    /// </summary>
    public static class FlagWrite
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--write</value>
        /// </summary>
        internal const string CliFlagName = "--write";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "is_write";

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
        ///     Adds [--write] flag to the CLI arguments.
        /// </summary>
        /// <remarks>When set, tells api this heartbeat was triggered from writing to a file.</remarks>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Boolean value to set the flag to. True by default.</param>
        public static FlagHolder AddFlagWrite(this FlagHolder flagHolder, bool value = true)
        {
            flagHolder.AddFlag(new Flag<bool>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--write] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagWrite(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}