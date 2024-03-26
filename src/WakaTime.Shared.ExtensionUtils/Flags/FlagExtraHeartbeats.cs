using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--extra-heartbeats] flag. <br /> <br />
    ///     Add:<br /> <see cref="AddFlagExtraHeartbeats" /> <br />
    ///     Remove:<br /> <see cref="RemoveFlagExtraHeartbeats" /> <br />
    /// </summary>
    /// <remarks>
    ///     The class and methods are intentionally made internal and should not be exposed to the client. <br />
    ///     This flag, if required, is added in <see cref="WakaTime.ProcessHeartbeats()" /> method of <see cref="WakaTime" />
    ///     class.
    /// </remarks>
    internal static class FlagExtraHeartbeats
    {
        #region Static Fields and Const

        /// <inheritdoc cref="FlagNames" />
        /// <value>
        ///     CLI: <c>--extra-heartbeats</c> <br /> JSON: <c>extra_heartbeats</c>
        /// </value>
        internal static FlagNames Name = new FlagNames("--extra-heartbeats", "extra_heartbeats");

        #endregion

        #region Properties

        /// <summary>
        ///     Formats the value for JSON serialization.
        /// </summary>
        /// <remarks>
        ///     Always returns empty string as the extra-heartbeats flag should not be serialized to JSON.
        /// </remarks>
        private static Func<string, string, bool, string> JsonFormatter => (s1, s2, b) => string.Empty;

        /// <inheritdoc cref="Formatters.CliFormatter{T}" />
        private static Func<string, string, bool, string> CliFormatter => Formatters.CliFormatter;

        /// <inheritdoc cref="Formatters.ValueFormatter{T}" />
        private static Func<bool, bool, string> ValueFormatter => Formatters.ValueFormatter;

        #endregion

        /// <summary>
        ///     Adds [--extra-heartbeats] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">A JSON string array of extra heartbeats to send.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <remarks>
        ///     See <see cref="FlagExtraHeartbeats" /> docs for more information.
        /// </remarks>
        internal static FlagHolder AddFlagExtraHeartbeats(this FlagHolder flagHolder, bool value = true, bool overwrite = true)
        {
            var flag = new Flag<bool>(value, ValueFormatter, Name, CliFormatter, JsonFormatter, false, false);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--extra-heartbeats] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        internal static FlagHolder RemoveFlagExtraHeartbeats(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(Name.Cli);
            return flagHolder;
        }
    }
}