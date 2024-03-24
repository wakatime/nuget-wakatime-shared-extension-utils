namespace WakaTime.Shared.ExtensionUtils.Flags
{
    public interface ICliFlag
    {
        #region Properties

        /// <summary>
        ///     The flag name for the CLI.
        /// </summary>
        /// <remarks>
        ///     Must be prefixed with '--'.
        /// </remarks>
        string CliFlagName { get; }

        /// <summary>
        ///     The flag name for JSON serialization.
        /// </summary>
        string JsonFlagName { get; }

        /// <summary>
        ///     Indicates whether the flag is included in JSON serialization for extra heartbeat.
        /// </summary>
        bool ForExtraHeartbeat { get; }

        #endregion

        #region Abstract Members

        string[] GetFlagWithValue();
        string GetValue();

        string ToString();

        #endregion
    }
}