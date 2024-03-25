using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Represents a flag for the CLI arguments and JSON serialization.
    /// </summary>
    /// <typeparam name="T">The type of the flag value.</typeparam>
    internal class Flag<T> : IFlag
    {
        #region Fields

        private readonly Func<T, bool, string> _cliFormatter;
        private readonly Func<T, bool, string> _jsonFormatter;
        private readonly T _value;
        private readonly Func<T, bool, string> _valueFormatter;

        #endregion

        #region Properties

        /// <inheritdoc />
        public string FlagUniqueName { get; set; }

        /// <inheritdoc />
        public bool ForExtraHeartbeat { get; set; }

        /// <inheritdoc />
        public bool CanObfuscate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Flag{T}" /> class.
        /// </summary>
        /// <param name="flagUniqueName">
        ///     The unique name of the flag.
        /// </param>
        /// <param name="value">The value of the flag. </param>
        /// <param name="valueFormatter">The function to format the value for the string representation.</param>
        /// <param name="cliFormatter">The function to format the value for CLI arguments.</param>
        /// <param name="jsonFormatter">The function to format the value for JSON serialization.</param>
        /// <param name="canObfuscate">Whether the flag value can be obfuscated. Default is false.</param>
        /// <param name="forExtraHeartbeat">
        ///     Whether the flag is included in JSON serialization for extra heartbeat. Default is
        ///     true.
        /// </param>
        internal Flag(string flagUniqueName,
                      T value,
                      Func<T, bool, string> valueFormatter,
                      Func<T, bool, string> cliFormatter,
                      Func<T, bool, string> jsonFormatter,
                      bool canObfuscate = false,
                      bool forExtraHeartbeat = true)
        {
            _value = value;
            _valueFormatter = valueFormatter;
            _cliFormatter = cliFormatter;
            _jsonFormatter = jsonFormatter;
            FlagUniqueName = flagUniqueName;
            CanObfuscate = canObfuscate;
            ForExtraHeartbeat = forExtraHeartbeat;
        }

        #endregion

        #region Interfaces Implement

        /// <inheritdoc />
        public string GetValue(bool obfuscate) => _valueFormatter.Invoke(_value, CanObfuscate && obfuscate);

        /// <inheritdoc />
        public string GetFormattedForCli(bool obfuscate = false) => _cliFormatter.Invoke(_value, CanObfuscate && obfuscate);

        /// <inheritdoc />
        public string GetFormattedForJson(bool obfuscate = false) => _jsonFormatter.Invoke(_value, CanObfuscate && obfuscate);

        #endregion
    }
}