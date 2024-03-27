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
        private readonly T _value;
        private readonly Func<T, bool, string> _valueFormatter;

        #endregion

        #region Properties

        /// <inheritdoc />
        public FlagNames Names { get; }

        /// <inheritdoc />
        public bool ForExtraHeartbeat { get; }

        /// <inheritdoc />
        public bool CanObfuscate { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Flag{T}" /> class.
        /// </summary>
        /// <param name="value">The value of the flag. </param>
        /// <param name="valueFormatter">The function to format the value for the string representation.</param>
        /// <param name="names">The names of the flag for the CLI arguments and JSON serialization.</param>
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
                 FlagNames names,
                 Func<string, string, T, string> cliFormatter,
                 Func<string, string, T, string> jsonFormatter,
                 bool canObfuscate = false,
                 bool forExtraHeartbeat = true)
        {
            _value = value;
            _valueFormatter = valueFormatter;
            _cliFormatter = cliFormatter;
            _jsonFormatter = jsonFormatter;
            Names = names;
            CanObfuscate = canObfuscate;
            ForExtraHeartbeat = forExtraHeartbeat;
        }

        #endregion

        #region Interfaces Implement

        /// <inheritdoc />
        public string GetValue(bool obfuscate) => _valueFormatter.Invoke(_value, CanObfuscate && obfuscate);

        /// <inheritdoc />
        public string GetFormattedForCli(bool obfuscate = false) => _cliFormatter.Invoke(Names.Cli, GetValue(obfuscate), _value);

        /// <inheritdoc />
        public string GetFormattedForJson(bool obfuscate = false) => _jsonFormatter.Invoke(Names.Json, GetValue(obfuscate), _value);

        #endregion
    }

    /// <summary>
    ///     Represents the names of the flags for the CLI arguments and JSON serialization.
    /// </summary>
    /// <remarks>
    ///     The <see cref="Cli" /> flag is also used as the key for storing the flag in <see cref="Heartbeat" /> and
    ///     <see cref="FlagHolder" /> objects.
    /// </remarks>
    public struct FlagNames
    {
        /// <summary>
        ///     The name of the flag for the CLI arguments.
        /// </summary>
        public string Cli;

        /// <summary>
        ///     The name of the flag for JSON serialization.
        /// </summary>
        public string Json;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlagNames" /> struct.
        /// </summary>
        /// <param name="cli">The name of the flag for the CLI arguments.</param>
        /// <param name="json">The name of the flag for JSON serialization.</param>
        /// <exception cref="ArgumentException">Thrown when the CLI flag name does not start with '--'.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the CLI or JSON flag name is null.</exception>
        /// <remarks>
        ///     The <see cref="Cli" /> name is also used as the key for storing the flag in <see cref="Heartbeat" /> and
        ///     <see cref="FlagHolder" /> objects.
        /// </remarks>
        public FlagNames(string cli, string json)
        {
            if (!cli.StartsWith("--"))
                throw new ArgumentException("The CLI flag name must start with '--'.", nameof(cli));

            Cli = cli ?? throw new ArgumentNullException(nameof(cli));
            Json = json ?? throw new ArgumentNullException(nameof(json));
        }
    }
}