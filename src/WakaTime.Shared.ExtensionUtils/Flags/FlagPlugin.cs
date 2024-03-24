using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--plugin] flag.
    /// </summary>
    public static class FlagPlugin
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--plugin</value>
        /// </summary>
        internal const string CliFlagName = "--plugin";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "plugin";

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
        ///     Adds [--plugin] flag to the CLI arguments.
        /// </summary>
        /// <remarks>
        ///     Flag is optional
        /// </remarks>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Text editor plugin name and version for User-Agent header.</param>
        /// <remarks>
        ///     This flag is added by default to the <see cref="WakaTime.CommonFlags" /> instance on creation and will be
        ///     added to each new <see cref="FlagHolder" />. <br />
        ///     Specifying this flag in <see cref="WakaTime.CommonFlags" /> will override the value from the configuration
        ///     file. <br />
        ///     Specifying this flag in <see cref="FlagHolder" /> will override the value from the
        ///     <see cref="WakaTime.CommonFlags" /> only for this Heartbeat.
        /// </remarks>
        public static FlagHolder AddFlagPlugin(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new Flag<string>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter, false, false));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--plugin] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagPlugin(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}