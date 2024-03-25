using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--extra-heartbeats] flag. <br /> <br />
    ///     Add: <see cref="AddFlagExtraHeartbeats" /> <br />
    ///     Remove: <see cref="RemoveFlagExtraHeartbeats" /> <br />
    /// </summary>
    /// <remarks>
    ///     The class and methods are intentionally made internal and should not be exposed to the client. <br />
    ///     This flag, if required, is added in <see cref="WakaTime.ProcessHeartbeats()" /> method of <see cref="WakaTime" />
    ///     class.
    /// </remarks>
    internal static class FlagExtraHeartbeats
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments.
        ///     <value>--extra-heartbeats</value>
        /// </summary>
        internal const string CliFlagName = "--extra-heartbeats";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "extra_heartbeats";

        #endregion

        #region Properties

        /// <summary>
        ///     Formats the value for JSON serialization.
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
        ///     Adds [--extra-heartbeats] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">A JSON string array of extra heartbeats to send.</param>
        /// <remarks>
        ///     See <see cref="FlagExtraHeartbeats" /> docs for more information.
        /// </remarks>
        internal static FlagHolder AddFlagExtraHeartbeats(this FlagHolder flagHolder, bool value = true)
        {
            flagHolder.AddFlag(new Flag<bool>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--extra-heartbeats] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        internal static FlagHolder RemoveFlagExtraHeartbeats(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}