using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--entity-type] flag.
    /// </summary>
    public static class FlagEntityType
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--entity-type</value>
        /// </summary>
        internal const string CliFlagName = "--entity-type";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "entity_type";

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
        ///     Adds [--entity-type] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value"><see cref="EntityType" /> for this heartbeat. Can be "file", "domain" or "app". Defaults to "file".</param>
        /// <seealso cref="EntityType.File" />
        /// <seealso cref="EntityType.Domain" />
        /// <seealso cref="EntityType.App" />
        public static FlagHolder AddFlagEntityType(this FlagHolder flagHolder, EntityType value = EntityType.File)
        {
            string entityType = value.GetDescription();
            flagHolder.AddFlag(new Flag<string>(CliFlagName, entityType, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--entity-type] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagEntityType(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}