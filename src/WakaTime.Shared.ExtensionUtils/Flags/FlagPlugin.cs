using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--plugin] flag. <br /> <br />
    ///     Add Common Flag: <see cref="AddFlagPlugin(FlagHolder,string,bool)" /> <br />
    ///     Remove Common Flag: <see cref="RemoveFlagPlugin(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag: <see cref="AddFlagPlugin(Heartbeat,string,bool)" /> <br />
    ///     Remove Heartbeat Flag: <see cref="RemoveFlagPlugin(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagPlugin
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--plugin</value>
        /// </summary>
        public const string CliFlagName = "--plugin";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "plugin";

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
        ///     Adds [--plugin] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <remarks>
        ///     Flag is optional
        /// </remarks>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Text editor plugin name and version for User-Agent header.</param>
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
        public static FlagHolder AddFlagPlugin(this FlagHolder flagHolder, string value, bool overwrite = true)
        {
            var flag = new Flag<string>(value, ValueFormatter, CliFlagName, CliFormatter, JsonFlagName, JsonFormatter, false, false);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--plugin] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">Text editor plugin name and version for User-Agent header.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <remarks>
        ///     This flag is added by default to the <see cref="WakaTime.CommonFlags" /> instance on creation and will be
        ///     added to each new <see cref="Heartbeat" />. <br />
        ///     Specifying this flag in <see cref="WakaTime.CommonFlags" /> will override the value from the configuration
        ///     file. <br />
        ///     Specifying this flag in <see cref="Heartbeat" /> will override the value from the
        ///     <see cref="WakaTime.CommonFlags" /> only for this Heartbeat.
        /// </remarks>
        public static Heartbeat AddFlagPlugin(this Heartbeat heartbeat, string value, bool overwrite = true) =>
            AddFlagPlugin(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--plugin] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagPlugin(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--plugin] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        public static Heartbeat RemoveFlagPlugin(this Heartbeat heartbeat) => RemoveFlagPlugin(flagHolder: heartbeat) as Heartbeat;
    }
}