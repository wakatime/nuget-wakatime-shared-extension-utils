using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WakaTime.Shared.ExtensionUtils.Helpers;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Extension methods for managing [--extra-heartbeats] flag.
    /// </summary>
    /// <remarks>
    ///     The class and methods are intentionally made internal and should not be exposed to the client. <br />
    ///     This flag, if required, is added in <see cref="WakaTime.ProcessHeartbeats()"/> method of <see cref="WakaTime"/> class.
    /// </remarks>
    internal static class FlagExtraHeartbeats
    {
        #region Static Fields and Const

        internal const string CliFlagName = "--extra-heartbeats";
        private const string JsonFlagName = "extra_heartbeats";

        #endregion

        /// <summary>
        ///     Adds [--extra-heartbeats] flag to the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        /// <param name="value">A JSON string array of extra heartbeats to send.</param>
        /// <remarks>
        ///     See <see cref="FlagExtraHeartbeats"/> docs for more information.
        /// </remarks>
        internal static FlagHolder AddFlagExtraHeartbeats(this FlagHolder flagHolder, bool value = true)
        {
            flagHolder.AddFlag(new Flag<bool>(CliFlagName, value, ValueFormatter, CliFormatter, JsonFormatter));
            return flagHolder;
        }

        /// <summary>
        ///     Removes the [--extra-heartbeats] flag from the CLI arguments.
        /// </summary>
        /// <param name="flagHolder">The <see cref="FlagHolder" /> instance.</param>
        internal static FlagHolder RemoveFlagExtraHeartbeats(this FlagHolder flagHolder)
        {
            flagHolder.RemoveFlag(CliFlagName);
            return flagHolder;
        }

      
        
        private static Func<bool, bool, string> JsonFormatter => (v, b) =>
        {
            string formattedValue = ValueFormatter.Invoke(v, b);
            return string.IsNullOrEmpty(formattedValue) ? string.Empty : $"\"{JsonFlagName}\": {JsonSerializerHelper.JsonEscape(formattedValue)}";
        };

        private static Func<bool, bool, string> CliFormatter => (v, b) => !v ? string.Empty : $"{CliFlagName}";

        private static Func<bool, bool, string> ValueFormatter => (v, b) => v.ToString()
                                                                             .ToLower();
    }
}