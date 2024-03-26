using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--time] flag. <br /> <br />
    ///     Add Common Flag:<br /> <see cref="AddFlagTime(FlagHolder,string,bool)" />
    ///     Or <see cref="AddFlagTime(FlagHolder,DateTime,bool)" /> <br />
    ///     Remove Common Flag:<br /> <see cref="RemoveFlagTime(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag:<br /> <see cref="AddFlagTime(Heartbeat,string,bool)" />
    ///     Or <see cref="AddFlagTime(Heartbeat,DateTime,bool)" /> <br />
    ///     Remove Heartbeat Flag:<br /> <see cref="RemoveFlagTime(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagTime
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--time</value>
        /// </summary>
        public const string CliFlagName = "--time";

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

        /// <inheritdoc cref="Formatters.JsonFormatter{T}" />
        private static Func<string, string, string, string> JsonFormatter => Formatters.JsonFormatter;

        /// <inheritdoc cref="Formatters.CliFormatter{T}" />
        private static Func<string, string, string, string> CliFormatter => Formatters.CliFormatter;

        /// <inheritdoc cref="Formatters.ValueFormatter{T}" />
        private static Func<string, bool, string> ValueFormatter => Formatters.ValueFormatter;

        #endregion

        /// <summary>
        ///     Add [--time] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Floating-point unix epoch timestamp. Uses current time by default.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <remarks>
        ///     The flag is added by default to every new instance of <see cref="FlagHolder" />. <br />
        ///     Adding this flag again will override the value set at the time of creation.
        /// </remarks>
        /// <seealso cref="AddFlagTime(FlagHolder,DateTime,bool)" />
        public static FlagHolder AddFlagTime(this FlagHolder flagHolder, string value, bool overwrite = true)
        {
            var flag = new Flag<string>(value, ValueFormatter, CliFlagName, CliFormatter, JsonFlagName, JsonFormatter);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Add [--time] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">Floating-point unix epoch timestamp. Uses current time by default.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <remarks>
        ///     The flag is added by default to every new instance of <see cref="FlagHolder" />. <br />
        ///     Adding this flag again will override the value set at the time of creation.
        /// </remarks>
        /// <seealso cref="AddFlagTime(Heartbeat,DateTime,bool)" />
        public static Heartbeat AddFlagTime(this Heartbeat heartbeat, string value, bool overwrite = true) => AddFlagTime(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Add [--time] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">DateTime instance to convert to unix epoch timestamp.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="AddFlagTime(FlagHolder,string,bool)" />
        public static FlagHolder AddFlagTime(this FlagHolder flagHolder, DateTime value, bool overwrite = true)
        {
            string unixEpoch = ToUnixEpoch(value);
            return AddFlagTime(flagHolder, unixEpoch);
        }

        /// <summary>
        ///     Add [--time] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">DateTime instance to convert to unix epoch timestamp.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="AddFlagTime(Heartbeat,DateTime,bool)" />
        public static Heartbeat AddFlagTime(this Heartbeat heartbeat, DateTime value, bool overwrite = true) => AddFlagTime(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--time] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagTime(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--time] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        public static Heartbeat RemoveFlagTime(this Heartbeat heartbeat) => RemoveFlagTime(flagHolder: heartbeat) as Heartbeat;

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