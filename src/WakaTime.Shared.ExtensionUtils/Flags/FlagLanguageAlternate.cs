namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--alternate-language] flag.
    /// </summary>
    public static class FlagLanguageAlternate
    {
        #region Static Fields and Const

        private const string CliFlagName = "--alternate-language";

        #endregion

        /// <summary>
        ///     Adds [--alternate-language] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Optional alternate language name. Auto-detected language takes priority.</param>
        /// <seealso cref="FlagLanguage.AddFlagLanguage" />
        public static FlagHolder AddFlagLanguageAlternate(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new CliFlag<string>(CliFlagName, value));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--alternate-language] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagLanguage.RemoveFlagLanguage" />
        public static FlagHolder RemoveFlagLanguageAlternate(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}