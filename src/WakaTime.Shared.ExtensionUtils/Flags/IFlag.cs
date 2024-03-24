namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Represents a flag that can be added to the CLI arguments or JSON serialization.
    /// </summary>
    public interface IFlag
    {
        #region Properties
        
        /// <summary>
        ///     Gets the flag name, used internally for flag management.
        /// </summary>
        /// <remarks>
        ///     The name must be unique for each flag type.
        /// </remarks>
        string FlagUniqueName { get; }

        /// <summary>
        ///     Gets the flag with its value for JSON serialization in format of key-value pair (i.e. "key": "value").
        /// </summary>
        string ForJson { get; }

        /// <summary>
        ///     Gets the flag with its value for CLI arguments in format of key-value pair (i.e. "--key value").
        /// </summary>
        string ForCli { get; }

        /// <summary>
        ///     Indicates whether the flag is included in JSON serialization for extra heartbeat.
        /// </summary>
        bool ForExtraHeartbeat { get; }

        #endregion

        #region Abstract Members

        /// <summary>
        ///     Gets the string representation of the Flag value.
        /// </summary>
        /// <remarks>
        ///     If the flag does not have a value, returns an empty string.
        /// </remarks>
        string GetValue();

        #endregion
    }
}