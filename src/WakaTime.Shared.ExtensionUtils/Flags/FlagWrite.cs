using System;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--write] flag. <br /> <br />
    ///     Add Common Flag:<br /> <see cref="AddFlagWrite(FlagHolder,bool,bool)" /> <br />
    ///     Remove Common Flag:<br /> <see cref="RemoveFlagWrite(FlagHolder)" /> <br />
    ///     Add Heartbeat Flag:<br /> <see cref="AddFlagWrite(Heartbeat,bool,bool)" /> <br />
    ///     Remove Heartbeat Flag:<br /> <see cref="RemoveFlagWrite(Heartbeat)" /> <br />
    /// </summary>
    public static class FlagWrite
    {
        #region Static Fields and Const

        /// <summary>
        ///     The flag name for the CLI arguments. Also used for <see cref="IFlag.FlagUniqueName" /> in <see cref="IFlag" />.
        ///     <value>--write</value>
        /// </summary>
        public const string CliFlagName = "--write";

        /// <summary>
        ///     The key name for JSON serialization.
        /// </summary>
        private const string JsonFlagName = "is_write";

        #endregion

        #region Properties

        /// <summary>
        ///     Formats the value for the string representation.
        /// </summary>
        /// <remarks>
        ///     Always returns empty string as the write flag should not be serialized to JSON.
        /// </remarks>
        private static Func<string, string, bool, string> JsonFormatter => (s1, s2, b) => string.Empty;

        /// <inheritdoc cref="Formatters.CliFormatter{T}" />
        private static Func<string, string, bool, string> CliFormatter => Formatters.CliFormatter;

        /// <inheritdoc cref="Formatters.ValueFormatter{T}" />
        private static Func<bool, bool, string> ValueFormatter => Formatters.ValueFormatter;

        #endregion

        /// <summary>
        ///     Adds [--write] flag to the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <remarks>When set, tells api this heartbeat was triggered from writing to a file.</remarks>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Boolean value to set the flag to. True by default.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        public static FlagHolder AddFlagWrite(this FlagHolder flagHolder, bool value = true, bool overwrite = true)
        {
            var flag = new Flag<bool>(value, ValueFormatter, CliFlagName, CliFormatter, JsonFlagName, JsonFormatter, false, false);
            flagHolder.AddFlag(flag, overwrite);
            return flagHolder;
        }

        /// <summary>
        ///     Adds [--write] flag to the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <remarks>When set, tells api this heartbeat was triggered from writing to a file.</remarks>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        /// <param name="value">Boolean value to set the flag to. True by default.</param>
        /// <param name="overwrite">
        ///     Whether to overwrite the existing flag value if it already exists. Defaults to true.
        /// </param>
        public static Heartbeat AddFlagWrite(this Heartbeat heartbeat, bool value = true, bool overwrite = true) =>
            AddFlagWrite(flagHolder: heartbeat, value, overwrite) as Heartbeat;

        /// <summary>
        ///     Removes the [--write] flag from the CLI arguments for all <see cref="Heartbeat" />s.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagWrite(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--write] flag from the CLI arguments for this <see cref="Heartbeat" /> instance.
        /// </summary>
        /// <param name="heartbeat">The <see cref="Heartbeat" /> instance.</param>
        public static Heartbeat RemoveFlagWrite(this Heartbeat heartbeat) => RemoveFlagWrite(flagHolder: heartbeat) as Heartbeat;
    }
}