using System;
using System.Diagnostics.CodeAnalysis;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--key] flag.
    /// </summary>
    [SuppressMessage("ReSharper", "CommentTypo")]
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
        private static Func<string, bool, string> ValueFormatter => (v, b) => b ? $"XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX{v.Substring(v.Length - 4)}" : v;

        #endregion

        /// <summary>
        ///     Adds [--key] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Your wakatime api key; uses api_key from ~/.wakatime.cfg by default.</param>
        /// <remarks>
        ///     This flag is added by default to the <see cref="WakaTime.CommonFlags" /> instance on creation and will be
        ///     added to each new <see cref="FlagHolder" />. <br />
        ///     Specifying this flag in <see cref="WakaTime.CommonFlags" /> will override the value from the configuration
        ///     file. <br />
        ///     Specifying this flag in <see cref="FlagHolder" /> will override the value from the
        ///     <see cref="WakaTime.CommonFlags" /> only for this Heartbeat.
        /// </remarks>
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static FlagHolder AddFlagKey(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new Flag<string>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter, true, false));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--key] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagKey(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}