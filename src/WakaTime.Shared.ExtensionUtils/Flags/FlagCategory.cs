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

        private const string CliFlagName = "--category";
        private const string JsonFlagName = "category";

        #endregion

        #region Properties

        private static Func<string, string> ValueFormatter => value => value;

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
            flagHolder.AddFlag(new Flag<string>(CliFlagName,FormatForCli(category), FormatForJson(category), category, ValueFormatter));
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

        private static string FormatForJson(string value) => $"\"{JsonFlagName}\": \"{JsonSerializerHelper.JsonEscape(value)}\"";

        private static string FormatForCli(string value) => $"{CliFlagName} {value}";
    }
}