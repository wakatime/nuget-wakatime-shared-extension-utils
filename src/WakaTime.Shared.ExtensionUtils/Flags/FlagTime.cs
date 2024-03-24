using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--time] flag.
    /// </summary>
    public static class FlagTime
    {
        #region Static Fields and Const

        internal const string CliFlagName = "--time";
        private const string JsonFlagName = "timestamp";

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        /// <summary>
        ///     Add [--time] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Floating-point unix epoch timestamp. Uses current time by default.</param>
        /// <remarks>
        ///     The flag is added by default to every new instance of <see cref="CliHeartbeat" />. <br />
        ///     Adding this flag again will override the value set at the time of creation.
        /// </remarks>
        public static FlagHolder AddFlagTime(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new Flag<string>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Add [--time] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">DateTime instance to convert to unix epoch timestamp.</param>
        public static FlagHolder AddFlagTime(this FlagHolder flagHolder, DateTime value)
        {
            string unixEpoch = ToUnixEpoch(value);
            flagHolder.AddFlag(new Flag<string>(CliFlagName, unixEpoch, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--time] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagTime(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
        
        private static Func<string, bool, string> JsonFormatter => (v, b) =>
        {
            string formattedValue = ValueFormatter.Invoke(v, b);
            return string.IsNullOrEmpty(formattedValue) ? string.Empty : $"\"{JsonFlagName}\": \"{JsonSerializerHelper.JsonEscape(formattedValue)}\"";
        };

        private static Func<string, bool, string> CliFormatter => (v, b) =>
        {
            string formattedValue = ValueFormatter.Invoke(v, b);
            return string.IsNullOrEmpty(formattedValue) ? string.Empty : $"{CliFlagName} {formattedValue}";
        };
        
        private static Func<string, bool, string> ValueFormatter => (v, b) => v;

        private static string ToUnixEpoch(DateTime date)
        {
            var timestamp = date - UnixEpoch;
            return $"{ToEpochSeconds(timestamp)}.{ToEpochMilliseconds(timestamp)}";
        }

        private static long ToEpochSeconds(TimeSpan timestamp) => Convert.ToInt64(Math.Floor(timestamp.TotalSeconds));

        // ReSharper disable once StringLiteralTypo
        private static string ToEpochMilliseconds(TimeSpan timestamp) => timestamp.ToString("ffffff");
    }
}