using System;

namespace WakaTime.Shared.ExtensionUtils.Flags
{
    internal class CliFlag : ICliFlag
    {
        #region Static Fields and Const

        private const string FlagPrefix = "--";

        #endregion

        #region Properties

        public string FlagName { get; }

        #endregion

        #region Constructors

        internal CliFlag(string flagName)
        {
            if (string.IsNullOrEmpty(flagName)) throw new ArgumentException("Flag name cannot be null or empty.", nameof(flagName));
            if (!flagName.StartsWith(FlagPrefix)) throw new ArgumentException("Flag name must start with '--'.",  nameof(flagName));

            FlagName = flagName;
        }

        #endregion

        #region Interfaces Implement

        /// <inheritdoc cref="ICliFlag" />
        public override string ToString() => FlagName;

        /// <inheritdoc />
        public virtual string[] GetFlagWithValue() => new[] { FlagName };

        #endregion
    }

    internal class CliFlag<T> : CliFlag
    {
        #region Fields

        private readonly T _value;

        #endregion

        #region Constructors

        internal CliFlag(string flag, T value) : base(flag) => _value = value;

        #endregion

        #region Overrides

        /// <inheritdoc />
        public override string[] GetFlagWithValue() => new[] { FlagName, _value.ToString() };

        /// <inheritdoc />
        public override string ToString() => $"{FlagName} {_value}";

        #endregion
    }
}

/*
 implemeted:
 --key string                       Your wakatime api key; uses api_key from ~/.wakatime.cfg by default.
 --plugin string                    Optional text editor plugin name and version for User-Agent header.
 --entity string                    Absolute path to file for the heartbeat. Can also be a url, domain or app when --entity-type is not file.
 --time float                       Optional floating-point unix epoch timestamp. Uses current time by default.
--category string                  Category of this heartbeat activity. Can be "coding", "building", "indexing", "debugging", "communicating", "running tests", "writing tests", "manual testing", "code reviewing", "browsing", or "designing". Defaults to "coding".
--entity-type string               Entity type for this heartbeat. Can be "file", "domain" or "app". Defaults to "file".
--alternate-project string         Optional alternate project name. Auto-detected project takes priority.
--project string                   Override auto-detected project. Use --alternate-project to supply a fallback project if one can't be auto-detected.
--language string                  Optional language name. If valid, takes priority over auto-detected language.
--alternate-language string        Optional alternate language name. Auto-detected language takes priority.
--write                            When set, tells api this heartbeat was triggered from writing to a file.

 in CliPatameters


      --extra-heartbeats                 Reads extra heartbeats from STDIN as a JSON array until EOF.

new:
      --cursorpos int                    Optional cursor position in the current file.
      --exclude strings                  Filename patterns to exclude from logging. POSIX regex syntax. Can be used more than once.
      --exclude-unknown-project          When set, any activity where the project cannot be detected will be ignored.
      --hide-branch-names string         Obfuscate branch names. Will not send revision control branch names to api.
      --hide-file-names string           Obfuscate filenames. Will not send file names to api.
      --hide-project-names string        Obfuscate project names. When a project folder is detected instead of using the folder name as the project, a .wakatime-project file is created with a random project name.
      --hostname string                  Optional name of local machine. Defaults to local machine name read from system.
      --include strings                  Filename patterns to log. When used in combination with --exclude, files matching include will still be logged. POSIX regex syntax. Can be used more than once.
      --include-only-with-project-file   Disables tracking folders unless they contain a .wakatime-project file. Defaults to false.

      --lineno int                       Optional line number. This is the current line being edited.
      --lines-in-file int                Optional lines in the file. Normally, this is detected automatically but can be provided manually for performance, accuracy, or when using --local-file.
      --local-file string                Absolute path to local file for the heartbeat. When --entity is a remote file, this local file will be used for stats and just the value of --entity is sent with the heartbeat.
      --log-file string                  Optional log file. Defaults to '~/.wakatime/wakatime.log'.
      --log-to-stdout                    If enabled, logs will go to stdout. Will overwrite logfile configs.
      --no-ssl-verify                    Disables SSL certificate verification for HTTPS requests. By default, SSL certificates are verified.

      --proxy string                     Optional proxy configuration. Supports HTTPS SOCKS and NTLM proxies. For example: 'https://user:pass@host:port' or 'socks5://user:pass@host:port' or 'domain\user:pass'
      --ssl-certs-file string            Override the bundled CA certs file. By default, uses system ca certs.
      --timeout int                      Number of seconds to wait when sending heartbeats to api. Defaults to 120 seconds. (default 120)
      --verbose                          Turns on debug messages in log file.

*/