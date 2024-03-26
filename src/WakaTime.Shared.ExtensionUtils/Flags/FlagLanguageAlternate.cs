using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--alternate-language] flag. <br /> <br />
    ///     Add Common Flag: <see cref="AddFlagLanguageAlternate(FlagHolder,string,bool)" /> <br />
    ///     Remove Common Flag: <see cref="RemoveFlagLanguageAlternate(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag: <see cref="AddFlagLanguageAlternate(Heartbeat,string,bool)" /> <br />
    ///     Remove Heartbeat Flag: <see cref="RemoveFlagLanguageAlternate(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagLanguageAlternate
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--alternate-language</value>
        /// </summary>
        internal const string CliFlagName = "--alternate-language";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "alternate_language";

        #endregion

        #region Properties

        /// <inheritdoc cref="Formatters.JsonFormatter{T}" />
        private static Func<string, string, string, string> JsonFormatter => Formatters.JsonFormatter;

        /// <inheritdoc cref="Formatters.CliFormatter{T}" />
        private static Func<string, string, string, string> CliFormatter => Formatters.CliFormatter;

        /// <inheritdoc cref="Formatters.ValueFormatter{T}" />
        private static Func<string, bool, string> ValueFormatter => Formatters.ValueFormatter;

        #endregion

        /// <summary>
        ///     Adds [--alternate-language] flag to the CLI arguments for all <see cref="Heartbeat"/>s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Optional alternate language name. Auto-detected language takes priority.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="FlagLanguage.AddFlagLanguage(FlagHolder,string,bool)" />
        public static FlagHolder AddFlagLanguageAlternate(this FlagHolder flagHolder, string value, bool overwrite = true)
        {
            var flag = new Flag<string>(value, ValueFormatter, CliFlagName, CliFormatter, JsonFlagName, JsonFormatter);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--alternate-language] flag to the CLI arguments for this <see cref="Heartbeat"/> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">Optional alternate language name. Auto-detected language takes priority.</param>
        /// <param name="overwrite"> Whether to overwrite the existing flag value if it already exists. Defaults to true.</param>
        /// <seealso cref="FlagLanguage.AddFlagLanguage(Heartbeat,string,bool)" />
        public static Heartbeat AddFlagLanguageAlternate(this Heartbeat heartbeat, string value, bool overwrite = true) =>
            AddFlagLanguageAlternate(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--alternate-language] flag from the CLI arguments for all <see cref="Heartbeat"/>s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagLanguage.RemoveFlagLanguage(FlagHolder)" />
        public static FlagHolder RemoveFlagLanguageAlternate(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--alternate-language] flag from the CLI arguments for this <see cref="Heartbeat"/> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <seealso cref="FlagLanguage.RemoveFlagLanguage(Heartbeat)" />
        public static Heartbeat RemoveFlagLanguageAlternate(this Heartbeat heartbeat) =>
            RemoveFlagLanguageAlternate(flagHolder: heartbeat) as Heartbeat;
    }
}