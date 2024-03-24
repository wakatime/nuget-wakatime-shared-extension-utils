namespace WakaTime.Shared.ExtensionUtils.Flags
{
    public static class FlagProject
    {
        #region Static Fields and Const

        private const string CliFlagName = "--project";

        #endregion

        /// <summary>
        ///     Adds [--project] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">
        ///     Override auto-detected project. Use --alternate-project to supply a fallback project if one can't
        ///     be auto-detected.
        /// </param>
        /// <seealso cref="FlagProjectAlternate.AddFlagProjectAlternate" />
        public static FlagHolder AddFlagProject(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new CliFlag<string>(CliFlagName, value));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--project] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagProjectAlternate.RemoveFlagProjectAlternate" />
        public static FlagHolder RemoveFlagProject(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}