using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--entity] flag. <br /> <br />
    ///     Add Common Flag:<br /> <see cref="AddFlagEntity(FlagHolder,string,bool)" /> <br />
    ///     Remove Common Flag:<br /> <see cref="RemoveFlagEntity(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag:<br /> <see cref="AddFlagEntity(Heartbeat,string,bool)" /> <br />
    ///     Remove Heartbeat Flag:<br /> <see cref="RemoveFlagEntity(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagEntity
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--entity</value>
        /// </summary>
        public const string CliFlagName = "--entity";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "entity";

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
        ///     Adds [--entity] flag to the CLI arguments for all <see cref="Heartbeat"/>s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">
        ///     Absolute path to file for the heartbeat. Can also be a url, domain or app when --entity-type is not
        ///     file.
        /// </param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        public static FlagHolder AddFlagEntity(this FlagHolder flagHolder, string value, bool overwrite = true)
        {
            var flag = new Flag<string>(value, ValueFormatter, CliFlagName, CliFormatter, JsonFlagName, JsonFormatter);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--entity] flag to the CLI arguments for this <see cref="Heartbeat"/> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">
        ///     Absolute path to file for the heartbeat. Can also be a url, domain or app when --entity-type is
        ///     not file.
        /// </param>
        /// <param name="overwrite"> Whether to overwrite the existing flag value if it already exists. Defaults to true.</param>
        public static Heartbeat AddFlagEntity(this Heartbeat heartbeat, string value, bool overwrite = true) => AddFlagEntity(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--entity] flag from the CLI arguments for all <see cref="Heartbeat"/>s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagEntity(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--entity] flag from the CLI arguments for this <see cref="Heartbeat"/> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        public static Heartbeat RemoveFlagEntity(this Heartbeat heartbeat) => RemoveFlagEntity(flagHolder: heartbeat) as Heartbeat;
    }
}