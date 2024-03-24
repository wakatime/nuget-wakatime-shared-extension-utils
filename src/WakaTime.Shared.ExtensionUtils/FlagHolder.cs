using System.Collections.Generic;
using System.Text;
using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils
{
    public class FlagHolder
    {
        #region Fields

        private readonly Dictionary<string, ICliFlag> _flags = new Dictionary<string, ICliFlag>();
        internal readonly WakaTime WakaTime;

        #endregion

        #region Properties

        public IReadOnlyDictionary<string, ICliFlag> Flags => _flags;

        #endregion

        #region Constructors

        public FlagHolder(WakaTime wakaTime) => WakaTime = wakaTime;

        #endregion

        public void AddFlag(ICliFlag flag, bool overwrite = true)
        {
            if (_flags.ContainsKey(flag.CliFlagName))
            {
                if (!overwrite)
                {
                    WakaTime.Logger.Debug($"Flag {flag.CliFlagName} already exists. But the {nameof(overwrite)} flag is set to false. Cannot overwrite.");
                    return;
                }

                WakaTime.Logger.Debug($"Flag {flag.CliFlagName} already exists. Overwriting.");
                _flags[flag.CliFlagName] = flag;
            }
            else
            {
                WakaTime.Logger.Debug($"Flag {flag.CliFlagName} does not exist. Adding.");
                _flags.Add(flag.CliFlagName, flag);
            }
        }

        public void RemoveFlag(ICliFlag flag) => RemoveFlag(flag.CliFlagName);

        public void RemoveFlag(string flag)
        {
            bool flagExists = _flags.TryGetValue(flag, out var existingFlag);
            if (!flagExists)
            {
                WakaTime.Logger.Debug($"Flag {flag} does not exist. Cannot remove.");
                return;
            }

            WakaTime.Logger.Debug($"Flag {flag} exists. Removing.");
            _flags.Remove(flag);
        }

        /// <summary>
        ///     Converts the heartbeat flags to JSON.
        /// </summary>
        /// <param name="isExtraHeartbeat">Whether to include flags that are not for extra heartbeat.</param>
        /// <returns>JSON string representation of a heartbeat and its flags.</returns>
        public string ToJson(bool isExtraHeartbeat = true)
        {
            int memberCount = 0;
            var b = new StringBuilder();
            b.Append("{");

            foreach (var flag in _flags)
            {
                // skip flags that are not for extra heartbeat
                if (!isExtraHeartbeat && !flag.Value.ForExtraHeartbeat) continue;

                if (memberCount > 0) b.Append(",");
                b.Append($"\"{flag.Value.JsonFlagName}\":\"{JsonEscape(flag.Value.GetValue())}\"");
                memberCount++;
            }

            return b.Append("}")
                    .ToString();
        }

        /// <summary>
        ///     Escapes the string to be used in JSON.
        /// </summary>
        /// <param name="value">The string to escape.</param>
        /// <returns>Escaped string.</returns>
        // ReSharper disable once CognitiveComplexity
        private static string JsonEscape(string value)
        {
            if (value == null) return null;
            var escaped = new StringBuilder();
            char[] chars = value.ToCharArray();
            foreach (char c in chars)
            {
                switch (c)
                {
                    case '\\':
                        escaped.Append("\\\\");
                        break;
                    case '\"':
                        escaped.Append("\\\"");
                        break;
                    case '\b':
                        escaped.Append("\\b");
                        break;
                    case '\f':
                        escaped.Append("\\f");
                        break;
                    case '\n':
                        escaped.Append("\\n");
                        break;
                    case '\r':
                        escaped.Append("\\r");
                        break;
                    case '\t':
                        escaped.Append("\\t");
                        break;
                    default:
                        bool isUnicode = c <= '\u001F' || (c >= '\u007F' && c <= '\u009F') || (c >= '\u2000' && c <= '\u20FF');
                        if (isUnicode)
                        {
                            escaped.Append("\\u");
                            string hex = ((int)c).ToString("X");
                            for (int k = 0; k < 4 - hex.Length; k++)
                            {
                                escaped.Append('0');
                            }

                            escaped.Append(hex.ToUpper());
                        }
                        else
                        {
                            escaped.Append(c);
                        }

                        break;
                }
            }

            return escaped.ToString();
        }
    }
}