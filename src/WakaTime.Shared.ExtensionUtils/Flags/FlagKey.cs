using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--key] flag. <br /> <br />
    ///     Add Common Flag:<br /> <see cref="AddFlagKey(FlagHolder,string,bool)" /> <br />
    ///     Remove Common Flag:<br /> <see cref="RemoveFlagKey(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag:<br /> <see cref="AddFlagKey(Heartbeat,string,bool)" /> <br />
    ///     Remove Heartbeat Flag:<br /> <see cref="RemoveFlagKey(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagKey
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--key</value>
        /// </summary>
        public const string CliFlagName = "--key";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "key";

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
        ///     Adds [--key] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Your wakatime api key; uses api_key from ~/.wakatime.cfg by default.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <remarks>
        ///     This flag is added by default to the <see cref="WakaTime.CommonFlags" /> instance on creation and will be
        ///     added to each new <see cref="FlagHolder" />. <br />
        ///     Specifying this flag in <see cref="WakaTime.CommonFlags" /> will override the value from the configuration
        ///     file. <br />
        ///     Specifying this flag in <see cref="FlagHolder" /> will override the value from the
        ///     <see cref="WakaTime.CommonFlags" /> only for this Heartbeat.
        /// </remarks>
        public static FlagHolder AddFlagKey(this FlagHolder flagHolder, string value, bool overwrite = true)
        {
            var flag = new Flag<string>(value, ValueFormatter, CliFlagName, CliFormatter, JsonFlagName, JsonFormatter, true, false);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--key] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">Your wakatime api key; uses api_key from ~/.wakatime.cfg by default.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <remarks>
        ///     This flag is added by default to the <see cref="WakaTime.CommonFlags" /> instance on creation and will be
        ///     added to each new <see cref="FlagHolder" />. <br />
        ///     Specifying this flag in <see cref="WakaTime.CommonFlags" /> will override the value from the configuration
        ///     file. <br />
        ///     Specifying this flag in <see cref="FlagHolder" /> will override the value from the
        ///     <see cref="WakaTime.CommonFlags" /> only for this Heartbeat.
        /// </remarks>
        public static Heartbeat AddFlagKey(this Heartbeat heartbeat, string value, bool overwrite = true) =>
            AddFlagKey(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--key] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagKey(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--key] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        public static Heartbeat RemoveFlagKey(this Heartbeat heartbeat) => RemoveFlagKey(flagHolder: heartbeat) as Heartbeat;
    }
}