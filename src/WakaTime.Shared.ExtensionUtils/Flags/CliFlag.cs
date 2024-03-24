using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    internal class CliFlag : ICliFlag
    {
        #region Static Fields and Const

        private const string FlagPrefix = "--";

        #endregion

        #region Properties

        /// <inheritdoc />
        public string CliFlagName { get; }

        /// <inheritdoc />
        public string JsonFlagName { get; }

        /// <inheritdoc />
        public bool ForExtraHeartbeat { get; }

        #endregion

        #region Constructors

        internal CliFlag(string cliFlagName, string jsonFlagName, bool forExtraHeartbeat = true)
        {
            if (string.IsNullOrEmpty(cliFlagName)) throw new ArgumentException("Flag name cannot be null or empty.",       nameof(cliFlagName));
            if (string.IsNullOrEmpty(jsonFlagName)) throw new ArgumentException("Json flag name cannot be null or empty.", nameof(jsonFlagName));

            if (!cliFlagName.StartsWith(FlagPrefix)) throw new ArgumentException("Flag name must start with '--'.",          nameof(cliFlagName));
            if (jsonFlagName.StartsWith(FlagPrefix)) throw new ArgumentException("Json flag name must not start with '--'.", nameof(jsonFlagName));

            CliFlagName = cliFlagName;
            JsonFlagName = jsonFlagName;
            ForExtraHeartbeat = forExtraHeartbeat;
        }

        #endregion

        #region Interfaces Implement

        /// <inheritdoc />
        public virtual string GetValue() => string.Empty;

        /// <inheritdoc cref="ICliFlag" />
        public override string ToString() => CliFlagName;

        /// <inheritdoc />
        public virtual string[] GetFlagWithValue() => new[] { CliFlagName };

        #endregion
    }

    internal class CliFlag<T> : CliFlag
    {
        #region Fields

        private readonly T _value;

        #endregion

        #region Constructors

        internal CliFlag(string cliFlagName, string jsonFlagName, T value, bool forExtraHeartbeat = true) : base(cliFlagName, jsonFlagName, forExtraHeartbeat) => _value = value;

        #endregion

        #region Overrides

        /// <inheritdoc />
        public override string[] GetFlagWithValue() => new[] { CliFlagName, _value.ToString() };

        /// <inheritdoc />
        public override string GetValue() => _value.ToString();

        /// <inheritdoc />
        public override string ToString() => $"{CliFlagName} {_value}";

        #endregion
    }
}