using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--verbose] flag. <br /> <br />
    ///     Add Common Flag:<br /> <see cref="AddFlagVerbose(FlagHolder,bool,bool)" /> <br />
    ///     Remove Common Flag:<br /> <see cref="RemoveFlagVerbose(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag:<br /> <see cref="AddFlagVerbose(Heartbeat,bool,bool)" /> <br />
    ///     Remove Heartbeat Flag:<br /> <see cref="RemoveFlagVerbose(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagVerbose
    {
        #region Static Fields and Const

        /// <inheritdoc cref="FlagNames" />
        /// <value>
        ///     CLI: <c>--verbose</c> <br /> JSON: <c>verbose</c>
        /// </value>
        public static FlagNames Name = new FlagNames("--verbose", "verbose");

        #endregion

        #region Properties

        /// <summary>
        ///     Formats the value for the string representation.
        /// </summary>
        /// <remarks>
        ///     Always returns empty string as the verbose flag should not be serialized to JSON.
        /// </remarks>
        private static Func<string, string, bool, string> JsonFormatter => (s1, s2, b) => string.Empty;

        /// <inheritdoc cref="Formatters.CliFormatter{T}" />
        private static Func<string, string, bool, string> CliFormatter => Formatters.CliFormatter;

        /// <inheritdoc cref="Formatters.ValueFormatter{T}" />
        private static Func<bool, bool, string> ValueFormatter => Formatters.ValueFormatter;

        #endregion

        /// <summary>
        ///     Adds [--verbose] flag to the CLI arguments for all <see cref="Heartbeat" />s. <br />
        ///     Turns on debug messages in log file.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Boolean value to set the flag to. True by default.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        public static FlagHolder AddFlagVerbose(this FlagHolder flagHolder, bool value = true, bool overwrite = true)
        {
            var flag = new Flag<bool>(value, ValueFormatter, Name, CliFormatter, JsonFormatter, false, false);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--verbose] flag to the CLI arguments for this <see cref="Heartbeat" /> instance. <br />
        ///     Turns on debug messages in log file.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">Boolean value to set the flag to. True by default.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        public static Heartbeat AddFlagVerbose(this Heartbeat heartbeat, bool value = true, bool overwrite = true) =>
            AddFlagVerbose(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--verbose] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagVerbose(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(Name.Cli);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--verbose] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        public static Heartbeat RemoveFlagVerbose(this Heartbeat heartbeat) => RemoveFlagVerbose(flagHolder: heartbeat) as Heartbeat;
    }
}