using System.Diagnostics.CodeAnalysis;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--key] flag.
    /// </summary>
    [SuppressMessage("ReSharper", "CommentTypo")]
    public static class FlagKey
    {
        #region Static Fields and Const

        private const string CliFlagName = "--key";
        private const string JsonFlagName = "key";
        

        #endregion

        /// <summary>
        ///     Adds [--key] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">Your wakatime api key; uses api_key from ~/.wakatime.cfg by default.</param>
        /// <param name="obfuscate">
        ///     Whether to obfuscate the value or not. If set to <c>true</c>, the value will be obfuscated with
        ///     'XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX' + last 4 characters of the value. <br />
        ///     The default is <c>true</c>.
        /// </param>
        /// <remarks>
        ///     This flag is added by default to the <see cref="WakaTime.CommonFlagsHolder" /> instance on creation and will be
        ///     added to each new <see cref="CliHeartbeat" />. <br />
        ///     Specifying this flag in <see cref="WakaTime.CommonFlagsHolder" /> will override the value from the configuration
        ///     file. <br />
        ///     Specifying this flag in <see cref="CliHeartbeat" /> will override the value from the
        ///     <see cref="WakaTime.CommonFlagsHolder" /> only for this Heartbeat.
        /// </remarks>
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static FlagHolder AddFlagKey(this FlagHolder flagHolder, string value, bool obfuscate = true)
        {
            string cliFlagValue = obfuscate ? $"XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX{value.Substring(value.Length - 4)}" : value;
            flagHolder.AddFlag(new CliFlag<string>(CliFlagName, JsonFlagName, cliFlagValue, false));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--key] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        public static FlagHolder RemoveFlagKey(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }
    }
}