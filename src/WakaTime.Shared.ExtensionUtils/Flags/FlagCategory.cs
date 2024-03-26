using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--category] flag. <br /> <br />
    ///     Add Common Flag:<br /> <see cref="AddFlagCategory(FlagHolder,HeartbeatCategory,bool)" /> <br />
    ///     Remove Common Flag:<br /> <see cref="RemoveFlagCategory(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag:<br /> <see cref="AddFlagCategory(Heartbeat,HeartbeatCategory,bool)" /> <br />
    ///     Remove Heartbeat Flag:<br /> <see cref="RemoveFlagCategory(Heartbeat)" /> <br />
    /// </summary>
    /// <seealso cref="HeartbeatCategory" />
    public static class FlagCategory
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--category</value>
        /// </summary>
        public const string CliFlagName = "--category";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        public const string JsonFlagName = "category";

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
        ///     Adds [--category] flag to the CLI arguments for all <see cref="Heartbeat"/>s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">
        ///     <see cref="HeartbeatCategory" /> of this heartbeat activity. <br />
        ///     Can be "coding", "building", "indexing", "debugging", "communicating", "running tests", "writing tests", "manual
        ///     testing", "code reviewing", "browsing", or "designing". Defaults to "coding".
        /// </param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
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
        public static FlagHolder AddFlagCategory(this FlagHolder flagHolder, HeartbeatCategory value = HeartbeatCategory.Coding, bool overwrite = true)
        {
            string category = value.GetDescription();
            var flag = new Flag<string>(category, ValueFormatter, CliFlagName, CliFormatter, JsonFlagName, JsonFormatter);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--category] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">
        ///     <see cref="HeartbeatCategory" /> of this heartbeat activity. <br />
        ///     Can be "coding", "building", "indexing", "debugging", "communicating", "running tests", "writing tests", "manual
        ///     testing", "code reviewing", "browsing", or "designing". Defaults to "coding".
        /// </param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
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
        public static Heartbeat AddFlagCategory(this Heartbeat heartbeat, HeartbeatCategory value = HeartbeatCategory.Coding, bool overwrite = true) =>
            AddFlagCategory(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--category] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagCategory(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--category] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        public static Heartbeat RemoveFlagCategory(this Heartbeat heartbeat) =>
            RemoveFlagCategory(flagHolder: heartbeat) as Heartbeat;
    }
}