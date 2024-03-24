namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--write] flag.
    /// </summary>
    public static class FlagWrite
    {
        #region Static Fields and Const

        private const string CliFlagName = "--write";
        private const string JsonFlagName = "is_write";

        #endregion

        /// <summary>
        ///     Adds [--write] flag to the CLI arguments.
        /// </summary>
        /// <remarks>When set, tells api this heartbeat was triggered from writing to a file.</remarks>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Boolean value to set the flag to. True by default.</param>
        public static FlagHolder AddFlagWrite(this FlagHolder flagHolder, bool value = true)
        {
            flagHolder.AddFlag(new CliFlag<bool>(CliFlagName, JsonFlagName, value));
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