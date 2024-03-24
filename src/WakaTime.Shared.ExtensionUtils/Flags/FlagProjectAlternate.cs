namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--alternate-project] flag.
    /// </summary>
    public static class FlagProjectAlternate
    {
        #region Static Fields and Const

        private const string CliFlagName = "--alternate-project";

        #endregion

        /// <summary>
        ///     Adds [--alternate-project] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Optional alternate project name. Auto-detected project takes priority.</param>
        /// <seealso cref="FlagProject.AddFlagProject" />
        public static FlagHolder AddFlagProjectAlternate(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new CliFlag<string>(CliFlagName, value));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--alternate-project] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagProject.RemoveFlagProject" />
        public static FlagHolder RemoveFlagProjectAlternate(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}