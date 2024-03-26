using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--alternate-project] flag. <br /> <br />
    ///     Add Common Flag: <see cref="AddFlagProjectAlternate(FlagHolder,string,bool)" /> <br />
    ///     Remove Common Flag: <see cref="RemoveFlagProjectAlternate(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag: <see cref="AddFlagProjectAlternate(Heartbeat,string,bool)" /> <br />
    ///     Remove Heartbeat Flag: <see cref="RemoveFlagProjectAlternate(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagProjectAlternate
    {
        #region Static Fields and Const

        /// <inheritdoc cref="FlagNames" />
        /// <value>
        ///     CLI: <c>--alternate-project</c> <br /> JSON: <c>alternate_project</c>
        /// </value>
        public static FlagNames Name = new FlagNames("--alternate-project", "alternate_project");

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
        ///     Adds [--alternate-project] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Optional alternate project name. Auto-detected project takes priority.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="FlagProject.AddFlagProject(FlagHolder,string,bool)" />
        public static FlagHolder AddFlagProjectAlternate(this FlagHolder flagHolder, string value, bool overwrite = true)
        {
            var flag = new Flag<string>(value, ValueFormatter, Name, CliFormatter, JsonFormatter);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--alternate-project] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">Optional alternate project name. Auto-detected project takes priority.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        /// <seealso cref="FlagProject.AddFlagProject(Heartbeat,string,bool)" />
        public static Heartbeat AddFlagProjectAlternate(this Heartbeat heartbeat, string value, bool overwrite = true) =>
            AddFlagProjectAlternate(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--alternate-project] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <seealso cref="FlagProject.RemoveFlagProject(FlagHolder)" />
        public static FlagHolder RemoveFlagProjectAlternate(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(Name.Cli);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--alternate-project] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <seealso cref="FlagProject.RemoveFlagProject(Heartbeat)" />
        public static Heartbeat RemoveFlagProjectAlternate(this Heartbeat heartbeat) => RemoveFlagProjectAlternate(flagHolder: heartbeat) as Heartbeat;
    }
}