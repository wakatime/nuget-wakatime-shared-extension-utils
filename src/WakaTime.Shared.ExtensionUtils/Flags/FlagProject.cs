using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    public static class FlagProject
    {
        #region Static Fields and Const

        internal const string CliFlagName = "--project";
        private const string JsonFlagName = "project";

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
    }
}