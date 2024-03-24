using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--language] flag.
    /// </summary>
    public static class FlagLanguage
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--language</value>
        /// </summary>
        internal const string CliFlagName = "--language";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "language";

        #endregion

        #region Properties

        /// <summary>
        ///     Formats the value for JSON serialization.
        /// </summary>
        private static Func<string, bool, string> JsonFormatter => (v, b) =>
        {
            string formattedValue = ValueFormatter.Invoke(v, b);
            return string.IsNullOrEmpty(formattedValue) ? string.Empty : $"\"{JsonFlagName}\": \"{JsonSerializerHelper.JsonEscape(formattedValue)}\"";
        };

        /// <summary>
        ///     Formats the value for CLI arguments.
        /// </summary>
        private static Func<string, bool, string> CliFormatter => (v, b) =>
        {
            string formattedValue = ValueFormatter.Invoke(v, b);
            return string.IsNullOrEmpty(formattedValue) ? string.Empty : $"{CliFlagName} {formattedValue}";
        };

        /// <summary>
        ///     Formats the value for the string representation.
        /// </summary>
        private static Func<string, bool, string> ValueFormatter => (v, b) => v;

        #endregion

        /// <summary>
        ///     Adds [--language] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Optional language name. If valid, takes priority over auto-detected language.</param>
        /// <seealso cref="FlagLanguageAlternate.AddFlagLanguageAlternate" />
        public static FlagHolder AddFlagLanguage(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new Flag<string>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--language] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagLanguageAlternate.RemoveFlagLanguageAlternate" />
        public static FlagHolder RemoveFlagLanguage(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}