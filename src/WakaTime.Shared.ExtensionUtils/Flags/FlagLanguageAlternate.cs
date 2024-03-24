using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--alternate-language] flag.
    /// </summary>
    public static class FlagLanguageAlternate
    {
        #region Static Fields and Const

        internal const string CliFlagName = "--alternate-language";
        private const string JsonFlagName = "alternate_language";

        #endregion

        /// <summary>
        ///     Adds [--alternate-language] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Optional alternate language name. Auto-detected language takes priority.</param>
        /// <seealso cref="FlagLanguage.AddFlagLanguage" />
        public static FlagHolder AddFlagLanguageAlternate(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new Flag<string>(CliFlagName, FormatForCli(value), FormatForJson(value), value, ValueFormatter));
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
        
        private static string FormatForJson(string value) => $"\"{JsonFlagName}\": \"{JsonSerializerHelper.JsonEscape(value)}\"";

        private static string FormatForCli(string value) => $"{CliFlagName} {value}";
        
        private static Func<string, string> ValueFormatter => value => value;
    }
}