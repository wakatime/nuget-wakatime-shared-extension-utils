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

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--time</value>
        /// </summary>
        internal const string CliFlagName = "--time";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "timestamp";

        /// <summary>
        ///     Unix epoch time.
        /// </summary>
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

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
            return string.IsNullOrEmpty(formattedValue) ? string.Empty : $"{CliFlagName}\" \"{formattedValue}";
        };

        /// <summary>
        ///     Formats the value for the string representation.
        /// </summary>
        private static Func<string, bool, string> ValueFormatter => (v, b) => v;

        #endregion

        /// <summary>
        ///     Add [--time] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Floating-point unix epoch timestamp. Uses current time by default.</param>
        /// <remarks>
        ///     The flag is added by default to every new instance of <see cref="FlagHolder" />. <br />
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

        /// <summary>
        ///     Converts the DateTime instance to unix epoch timestamp.
        /// </summary>
        /// <param name="date">The DateTime instance to convert.</param>
        /// <returns>The unix epoch timestamp.</returns>
        private static string ToUnixEpoch(DateTime date)
        {
            var timestamp = date - UnixEpoch;
            return $"{ToEpochSeconds(timestamp)}.{ToEpochMilliseconds(timestamp)}";
        }

        /// <summary>
        ///     Converts the TimeSpan instance to unix epoch seconds.
        /// </summary>
        /// <param name="timestamp">The TimeSpan instance to convert.</param>
        /// <returns>The unix epoch seconds.</returns>
        private static long ToEpochSeconds(TimeSpan timestamp) => Convert.ToInt64(Math.Floor(timestamp.TotalSeconds));

        /// <summary>
        ///     Converts the TimeSpan instance to unix epoch milliseconds.
        /// </summary>
        /// <param name="timestamp">The TimeSpan instance to convert.</param>
        /// <returns>The unix epoch milliseconds.</returns>
        // ReSharper disable once StringLiteralTypo
        private static string ToEpochMilliseconds(TimeSpan timestamp) => timestamp.ToString("ffffff");
    }
}