using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--alternate-project] flag.
    /// </summary>
    public static class FlagProjectAlternate
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--alternate-project</value>
        /// </summary>
        internal const string CliFlagName = "--alternate-project";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "alternate_project";

        #endregion

        #region Properties

        /// <summary>
        ///     Formats the value for the string representation.
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
        ///     Adds [--alternate-project] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Optional alternate project name. Auto-detected project takes priority.</param>
        /// <seealso cref="FlagProject.AddFlagProject" />
        public static FlagHolder AddFlagProjectAlternate(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new Flag<string>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--alternate-project] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagProject.RemoveFlagProject" />
        public static FlagHolder RemoveFlagProjectAlternate(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}