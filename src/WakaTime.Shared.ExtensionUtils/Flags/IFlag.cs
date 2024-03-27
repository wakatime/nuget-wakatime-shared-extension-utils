namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Represents a flag that can be added to the CLI arguments or JSON serialization.
    /// </summary>
    public interface IFlag
    {
        #region Properties

        /// <inheritdoc cref="FlagNames" />
        FlagNames Names { get; }

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
        ///     Gets the flag with its value for CLI arguments in format of key-value pair (i.e. "--key value").
        /// </summary>
        string GetFormattedForCli(bool obfuscate = false);

        /// <summary>
        ///     Gets the flag with its value for JSON serialization in format of key-value pair (i.e. "key": "value").
        /// </summary>
        string GetFormattedForJson(bool obfuscate = false);

        /// <summary>
        ///     Gets the string representation of the Flag value.
        /// </summary>
        /// <param name="obfuscate">
        ///     Whether to obfuscate the value. Default is false.
        /// </param>
        /// <remarks>
        ///     If the flag does not have a value, returns an empty string.
        ///     Even if the <paramref name="obfuscate" /> is set to <c>true</c>, the value may not be obfuscated if the flag does
        ///     not support it. Check the value of <see cref="CanObfuscate" /> property.
        /// </remarks>
        string GetValue(bool obfuscate = false);

        #endregion
    }
}