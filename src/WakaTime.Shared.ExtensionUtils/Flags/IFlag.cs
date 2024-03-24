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
        ///     Indicates whether the flag is included in JSON serialization for extra heartbeat.
        /// </summary>
        bool ForExtraHeartbeat { get; }
        
        /// <summary>
        ///     Indicates whether the flag value can be obfuscated.
        /// </summary>
        bool CanObfuscate { get; }

        #endregion

        #region Abstract Members

        /// <summary>
        ///     Gets the string representation of the Flag value.
        /// </summary>
        /// <param name="obfuscate">
        ///     Whether to obfuscate the value. Default is false.
        /// </param>
        /// <remarks>
        ///     If the flag does not have a value, returns an empty string.
        /// </remarks>
        string GetValue(bool obfuscate = false);
        
        /// <summary>
        ///     Gets the flag with its value for CLI arguments in format of key-value pair (i.e. "--key value").
        /// </summary>
        string GetFormattedForCli(bool obfuscate = false);
        
        /// <summary>
        ///     Gets the flag with its value for JSON serialization in format of key-value pair (i.e. "key": "value").
        /// </summary>
        string GetFormattedForJson(bool obfuscate = false);

        #endregion
    }
}