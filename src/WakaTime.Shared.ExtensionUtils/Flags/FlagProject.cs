using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--project] flag. <br /> <br />
    ///     Add Common Flag: <see cref="AddFlagProject(FlagHolder,string,bool)" /> <br />
    ///     Remove Common Flag: <see cref="RemoveFlagProject(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag: <see cref="AddFlagProject(Heartbeat,string,bool)" /> <br />
    ///     Remove Heartbeat Flag: <see cref="RemoveFlagProject(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagProject
    {
        #region Static Fields and Const

        /// <inheritdoc cref="FlagNames" />
        /// <value>
        ///     CLI: <c>--project</c> <br /> JSON: <c>project</c>
        /// </value>
        public static FlagNames Name = new FlagNames("--project", "project");

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
        ///     Adds [--project] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">
        ///     Override auto-detected project. Use --alternate-project to supply a fallback project if one can't
        ///     be auto-detected.
        /// </param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="FlagProjectAlternate.AddFlagProjectAlternate(FlagHolder,string,bool)" />
        public static FlagHolder AddFlagProject(this FlagHolder flagHolder, string value, bool overwrite = true)
        {
            var flag = new Flag<string>(value, ValueFormatter, Name, CliFormatter, JsonFormatter);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--project] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">
        ///     Override auto-detected project. Use --alternate-project to supply a fallback project if one can't
        ///     be auto-detected.
        /// </param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="FlagProjectAlternate.AddFlagProjectAlternate(Heartbeat,string,bool)" />
        public static Heartbeat AddFlagProject(this Heartbeat heartbeat, string value, bool overwrite = true) =>
            AddFlagProject(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--project] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagProjectAlternate.RemoveFlagProjectAlternate(FlagHolder)" />
        public static FlagHolder RemoveFlagProject(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(Name.Cli);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--project] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <seealso cref="FlagProjectAlternate.RemoveFlagProjectAlternate(Heartbeat)" />
        public static Heartbeat RemoveFlagProject(this Heartbeat heartbeat) => RemoveFlagProject(flagHolder: heartbeat) as Heartbeat;
    }
}