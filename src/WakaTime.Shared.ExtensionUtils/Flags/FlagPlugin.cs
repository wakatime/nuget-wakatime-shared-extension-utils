namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--plugin] flag.
    /// </summary>
    public static class FlagPlugin
    {
        #region Static Fields and Const

        private const string CliFlagName = "--plugin";
        private const string JsonFlagName = "plugin";

        #endregion

        /// <summary>
        ///     Adds [--plugin] flag to the CLI arguments.
        /// </summary>
        /// <remarks>
        ///     Flag is optional
        /// </remarks>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Text editor plugin name and version for User-Agent header.</param>
        /// <remarks>
        ///     This flag is added by default to the <see cref="WakaTime.CommonFlagsHolder" /> instance on creation and will be
        ///     added to each new <see cref="CliHeartbeat" />. <br />
        ///     Specifying this flag in <see cref="WakaTime.CommonFlagsHolder" /> will override the value from the configuration
        ///     file. <br />
        ///     Specifying this flag in <see cref="CliHeartbeat" /> will override the value from the
        ///     <see cref="WakaTime.CommonFlagsHolder" /> only for this Heartbeat.
        /// </remarks>
        public static FlagHolder AddFlagPlugin(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new CliFlag<string>(CliFlagName, JsonFlagName, value, false));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--plugin] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagPlugin(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}