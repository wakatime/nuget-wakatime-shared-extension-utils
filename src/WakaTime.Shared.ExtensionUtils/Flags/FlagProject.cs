using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--project] flag.
    /// </summary>
    public static class FlagProject
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--project</value>
        /// </summary>
        internal const string CliFlagName = "--project";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "project";

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
        ///     Adds [--project] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">
        ///     Override auto-detected project. Use --alternate-project to supply a fallback project if one can't
        ///     be auto-detected.
        /// </param>
        /// <seealso cref="FlagProjectAlternate.AddFlagProjectAlternate" />
        public static FlagHolder AddFlagProject(this FlagHolder flagHolder, string value)
        {
            flagHolder.AddFlag(new Flag<string>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--project] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagProjectAlternate.RemoveFlagProjectAlternate" />
        public static FlagHolder RemoveFlagProject(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}