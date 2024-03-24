using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    internal class Flag : IFlag
    {
        #region Properties

        /// <inheritdoc />
        public string FlagUniqueName { get; }

        /// <inheritdoc />
        public string ForJson { get; }

        /// <inheritdoc />
        public string ForCli { get; }

        /// <inheritdoc />
        public bool ForExtraHeartbeat { get; }

        #endregion

        #region Constructors

        internal Flag(string flagUniqueName, string forCli, string forJson, bool forExtraHeartbeat = true)
        {
            if (string.IsNullOrEmpty(flagUniqueName)) throw new ArgumentException("Flag unique name cannot be null or empty.", nameof(flagUniqueName));
            if (string.IsNullOrEmpty(forCli)) throw new ArgumentException("For CLI argument cannot be null or empty.",         nameof(forCli));
            if (string.IsNullOrEmpty(forJson)) throw new ArgumentException("For JSON argument cannot be null or empty.",       nameof(forJson));

            FlagUniqueName = flagUniqueName;
            ForCli = forCli;
            ForJson = forJson;
            ForExtraHeartbeat = forExtraHeartbeat;
        }

        #endregion

        #region Interfaces Implement

        /// <inheritdoc />
        public virtual string GetValue() => string.Empty;

        #endregion
    }

    internal class Flag<T> : Flag
    {
        #region Fields

        private readonly T _value;
        private readonly Func<T, string> _valueFormatter;

        #endregion

        #region Constructors

        internal Flag(string flagUniqueName,
                      string forCli,
                      string forJson,
                      T value,
                      Func<T, string> valueFormatter,
                      bool forExtraHeartbeat = true) : base(flagUniqueName, forCli, forJson, forExtraHeartbeat)
        {
            _value = value;
            _valueFormatter = valueFormatter;
        }

        #endregion

        #region Overrides

        /// <inheritdoc />
        public override string GetValue() => _valueFormatter.Invoke(_value);

        #endregion
    }
}