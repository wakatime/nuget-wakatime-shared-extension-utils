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
        public static FlagHolder AddFlagEntityType(this FlagHolder flagHolder, EntityType value)
        {
            string entityType = value.GetDescription();
            flagHolder.AddFlag(new CliFlag<string>(CliFlagName, JsonFlagName, entityType));
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