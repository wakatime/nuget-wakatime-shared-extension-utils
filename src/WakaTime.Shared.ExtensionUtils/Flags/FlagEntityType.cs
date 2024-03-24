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
        
        private const string CliFlagName = "--entity-type";
        private const string JsonFlagName = "entity_type";

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
            flagHolder.AddFlag(new Flag<string>(CliFlagName, FormatForCli(entityType), FormatForJson(entityType), entityType, ValueFormatter));
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
        
        private static string FormatForJson(string value) => $"\"{JsonFlagName}\": \"{JsonSerializerHelper.JsonEscape(value)}\"";

        private static string FormatForCli(string value) => $"{CliFlagName} {value}";
        
        private static Func<string, string> ValueFormatter => value => value;
    }
}