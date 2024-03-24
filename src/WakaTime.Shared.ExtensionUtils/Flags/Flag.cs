using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
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