namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--extra-heartbeats] flag.
    /// </summary>
    /// <remarks>
    ///     The class is internal intentionally, as this flag is handled internally within
    ///     <see cref="WakaTime.ProcessHeartbeats()" /> method of the <see cref="WakaTime" /> class
    ///     and should not be exposed to the client.
    /// </remarks>
    internal static class FlagExtraHeartbeats
    {
        #region Static Fields and Const

        private const string CliFlagName = "--extra-heartbeats";
        private const string JsonFlagName = "extra_heartbeats";

        #endregion

        /// <summary>
        ///     Adds [--extra-heartbeats] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">A JSON string array of extra heartbeats to send.</param>
        internal static FlagHolder AddFlagExtraHeartbeats(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new CliFlag<string>(CliFlagName, JsonFlagName, value, false));
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