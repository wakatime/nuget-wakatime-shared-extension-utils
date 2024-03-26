using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--entity-type] flag. <br /> <br />
    ///     Add Common Flag:<br /> <see cref="AddFlagEntityType(FlagHolder,EntityType,bool)" /> <br />
    ///     Remove Common Flag:<br /> <see cref="RemoveFlagEntityType(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag:<br /> <see cref="AddFlagEntityType(Heartbeat,EntityType,bool)" /> <br />
    ///     Remove Heartbeat Flag:<br /> <see cref="RemoveFlagEntityType(Heartbeat)" /> <br />
    /// </summary>
    /// <seealso cref="EntityType" />
    public static class FlagEntityType
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--entity-type</value>
        /// </summary>
        public const string CliFlagName = "--entity-type";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "entity_type";

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
        ///     Adds [--entity-type] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value"><see cref="EntityType" /> for this heartbeat. Can be "file", "domain" or "app". Defaults to "file".</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="EntityType.File" />
        /// <seealso cref="EntityType.Domain" />
        /// <seealso cref="EntityType.App" />
        public static FlagHolder AddFlagEntityType(this FlagHolder flagHolder, EntityType value = EntityType.File, bool overwrite = true)
        {
            string entityType = value.GetDescription();
            var flag = new Flag<string>(entityType, ValueFormatter, CliFlagName, CliFormatter, JsonFlagName, JsonFormatter);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--entity-type] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value"><see cref="EntityType" /> for this heartbeat. Can be "file", "domain" or "app". Defaults to "file".</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="EntityType.File" />
        /// <seealso cref="EntityType.Domain" />
        /// <seealso cref="EntityType.App" />
        public static Heartbeat AddFlagEntityType(this Heartbeat heartbeat, EntityType value = EntityType.File, bool overwrite = true) =>
            AddFlagEntityType(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--entity-type] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagEntityType(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--entity-type] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        public static Heartbeat RemoveFlagEntityType(this Heartbeat heartbeat) => RemoveFlagEntityType(flagHolder: heartbeat) as Heartbeat;
    }
}