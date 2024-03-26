using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    /// <summary>
    ///     Represents a flag for the CLI arguments and JSON serialization.
    /// </summary>
    /// <typeparam name="T">The type of the flag value.</typeparam>
#if DEBUG
    public
#else
    internal
#endif
    class Flag<T> : IFlag
    {
        #region Fields

        private readonly Func<string, string, T, string> _cliFormatter;
        private readonly Func<string, string, T, string> _jsonFormatter;
        private readonly string _cliFlagName;
        private readonly string _jsonFlagName;
        private readonly T _value;
        private readonly Func<T, bool, string> _valueFormatter;

        #endregion

        #region Properties

        /// <inheritdoc />
        public string FlagUniqueName => _cliFlagName;

        /// <inheritdoc />
        public bool ForExtraHeartbeat { get;  }

        /// <inheritdoc />
        public bool CanObfuscate { get;  }

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
#if DEBUG
        public
#else
        internal
#endif
        Flag(T value,
             Func<T, bool, string> valueFormatter,
             string cliFlagName,
             Func<string, string, T, string> cliFormatter,
             string jsonFlagName,
             Func<string,string, T, string> jsonFormatter,
             bool canObfuscate = false,
             bool forExtraHeartbeat = true)
        {
            _cliFlagName = cliFlagName;
            _jsonFlagName = jsonFlagName;
            _value = value;
            _valueFormatter = valueFormatter;
            _cliFormatter = cliFormatter;
            _jsonFormatter = jsonFormatter;
            CanObfuscate = canObfuscate;
            ForExtraHeartbeat = forExtraHeartbeat;
        }

        #endregion

        #region Interfaces Implement

        /// <inheritdoc />
        public string GetValue(bool obfuscate) => _valueFormatter.Invoke(_value, CanObfuscate && obfuscate);

        /// <inheritdoc />
        public string GetFormattedForCli(bool obfuscate = false) => _cliFormatter.Invoke(_cliFlagName, GetValue(obfuscate), _value);

        /// <inheritdoc />
        public string GetFormattedForJson(bool obfuscate = false) => _jsonFormatter.Invoke(_jsonFlagName, GetValue(obfuscate), _value);

        #endregion
    }
}