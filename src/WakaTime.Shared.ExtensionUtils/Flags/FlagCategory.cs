using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--category] flag.
    /// </summary>
    public static class FlagCategory
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--category</value>
        /// </summary>
        internal const string CliFlagName = "--category";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "category";

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
        ///     Adds [--category] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">
        ///     <see cref="HeartbeatCategory" /> of this heartbeat activity. <br />
        ///     Can be "coding", "building", "indexing", "debugging", "communicating", "running tests", "writing tests", "manual
        ///     testing", "code reviewing", "browsing", or "designing". Defaults to "coding".
        /// </param>
        /// <seealso cref="HeartbeatCategory" />
        /// <seealso cref="HeartbeatCategory.Coding" />
        /// <seealso cref="HeartbeatCategory.Building" />
        /// <seealso cref="HeartbeatCategory.Indexing" />
        /// <seealso cref="HeartbeatCategory.Debugging" />
        /// <seealso cref="HeartbeatCategory.RunningTests" />
        /// <seealso cref="HeartbeatCategory.WritingTests" />
        /// <seealso cref="HeartbeatCategory.ManualTesting" />
        /// <seealso cref="HeartbeatCategory.CodeReviewing" />
        /// <seealso cref="HeartbeatCategory.Browsing" />
        /// <seealso cref="HeartbeatCategory.Designing" />
        public static FlagHolder AddFlagCategory(this FlagHolder flagHolder, HeartbeatCategory value = HeartbeatCategory.Coding)
        {
            string category = value.GetDescription();
            flagHolder.AddFlag(new Flag<string>(CliFlagName, category, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--category] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagCategory(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}