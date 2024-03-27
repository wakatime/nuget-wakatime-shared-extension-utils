using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--language] flag. <br /> <br />
    ///     Add Common Flag: <see cref="AddFlagLanguage(FlagHolder,string,bool)" /> <br />
    ///     Remove Common Flag: <see cref="RemoveFlagLanguage(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag: <see cref="AddFlagLanguage(Heartbeat,string,bool)" /> <br />
    ///     Remove Heartbeat Flag: <see cref="RemoveFlagLanguage(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagLanguage
    {
        #region Static Fields and Const

        /// <inheritdoc cref="FlagNames" />
        /// <value>
        ///     CLI: <c>--language</c> <br /> JSON: <c>language</c>
        /// </value>
        public static FlagNames Name = new FlagNames("--language", "language");

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
        ///     Adds [--language] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Optional language name. If valid, takes priority over auto-detected language.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="FlagLanguageAlternate.AddFlagLanguageAlternate(FlagHolder,string,bool)" />
        public static FlagHolder AddFlagLanguage(this FlagHolder flagHolder, string value, bool overwrite = true)
        {
            var flag = new Flag<string>(value, ValueFormatter, Name, CliFormatter, JsonFormatter);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--language] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">Optional language name. If valid, takes priority over auto-detected language.</param>
        /// <param name="overwrite"> Whether to overwrite the existing flag value if it already exists. Defaults to true.</param>
        /// <seealso cref="FlagLanguageAlternate.AddFlagLanguageAlternate(Heartbeat,string,bool)" />
        public static Heartbeat AddFlagLanguage(this Heartbeat heartbeat, string value, bool overwrite = true) =>
            AddFlagLanguage(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--language] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagLanguageAlternate.RemoveFlagLanguageAlternate(FlagHolder)" />
        public static FlagHolder RemoveFlagLanguage(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(Name.Cli);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--language] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <seealso cref="FlagLanguageAlternate.RemoveFlagLanguageAlternate(Heartbeat)" />
        public static Heartbeat RemoveFlagLanguage(this Heartbeat heartbeat) => RemoveFlagLanguage(flagHolder: heartbeat) as Heartbeat;
    }
}