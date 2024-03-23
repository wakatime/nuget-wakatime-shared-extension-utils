using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
    [SuppressMessage("ReSharper", "InvertIf")]
    [SuppressMessage("ReSharper", "CommentTypo")]
    public static class FlagKey
    {
        #region Static Fields and Const

        private const string KeyFlagName = "--key";

        #endregion

        /// <summary>
        ///     Adds [--key] flag to the CLI.
        /// </summary>
        /// <param name="wakaTime">The WakaTime instance.</param>
        /// <param name="value">Your wakatime api key; uses api_key from ~/.wakatime.cfg by default.</param>
        /// <param name="obfuscate">
        ///     Whether to obfuscate the value or not. If set to <c>true</c>, the value will be obfuscated with
        ///     'XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX' + last 4 characters of the value. <br />
        ///     The default is <c>true</c>.
        /// </param>
        /// <param name="addAlways">
        ///     Whether the flag should be included in every heartbeat or just the next one. <br />
        ///     The default is <c>true</c>.
        /// </param>
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static void AddFlagKey(this WakaTime wakaTime, string value, bool obfuscate = true, bool addAlways = true)
        {
            string cliFlagValue = obfuscate ? $"XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX{value.Substring(value.Length - 4)}" : value;

            var flag = new CliFlag<string>(KeyFlagName, cliFlagValue);

            var permanentFlag = wakaTime.CliFlagsPermanent.FirstOrDefault(f => f.Flag == KeyFlagName);
            var temporaryFlag = wakaTime.CliFlagsTemporary.FirstOrDefault(f => f.Flag == KeyFlagName);

            bool flagExistsInPermanent = permanentFlag != null;
            bool flagExistsInTemporary = temporaryFlag != null;


            if (addAlways && flagExistsInTemporary)
            {
                wakaTime.Logger.Debug($"Removing temporary flag {temporaryFlag}");
                wakaTime.CliFlagsTemporary.Remove(temporaryFlag);
            }

            if (!addAlways && flagExistsInPermanent)
            {
                wakaTime.Logger.Debug($"Removing permanent flag {permanentFlag}");
                wakaTime.CliFlagsPermanent.Remove(permanentFlag);
            }

            if (addAlways && !flagExistsInPermanent)
            {
                wakaTime.Logger.Debug($"Adding permanent flag {flag}");
                wakaTime.CliFlagsPermanent.Add(flag);
            }

            if (!addAlways && !flagExistsInTemporary)
            {
                wakaTime.Logger.Debug($"Adding temporary flag {flag}");
                wakaTime.CliFlagsTemporary.Add(flag);
            }
        }

        /// <summary>
        ///     Removes the [--key] flag from the CLI flags. <br />
        /// </summary>
        /// <param name="wakaTime">The WakaTime instance.</param>
        /// <param name="removePermanent">
        ///     Whether to remove the flag from the permanent flags or not. <br /> The permanent flags are those that are added to
        ///     every heartbeat. <br />
        ///     The default is <c>true</c>.
        /// </param>
        /// <param name="removeTemporary">
        ///     Whether to remove the flag from the temporary flags or not. <br /> The temporary flags are those that are added to
        ///     the next heartbeat only. <br />
        ///     The default is <c>true</c>.
        /// </param>
        public static void RemoveFlagKey(this WakaTime wakaTime, bool removePermanent = true, bool removeTemporary = true)
        {
            if (!removePermanent && !removeTemporary)
            {
                wakaTime.Logger.Debug("No flags to remove");
                return;
            }

            if (removePermanent)
            {
                var permanentFlag = wakaTime.CliFlagsPermanent.FirstOrDefault(f => f.Flag == KeyFlagName);
                if (permanentFlag == null)
                {
                    wakaTime.Logger.Debug($"No permanent flag {KeyFlagName} to remove");
                    return;
                }

                wakaTime.Logger.Debug($"Removing permanent flag {permanentFlag}");
                wakaTime.CliFlagsPermanent.Remove(permanentFlag);
            }

            if (removeTemporary)
            {
                var temporaryFlag = wakaTime.CliFlagsTemporary.FirstOrDefault(f => f.Flag == KeyFlagName);
                if (temporaryFlag == null)
                {
                    wakaTime.Logger.Debug($"No temporary flag {KeyFlagName} to remove");
                    return;
                }

                wakaTime.Logger.Debug($"Removing temporary flag {temporaryFlag}");
                wakaTime.CliFlagsTemporary.Remove(temporaryFlag);
            }
        }
    }
}