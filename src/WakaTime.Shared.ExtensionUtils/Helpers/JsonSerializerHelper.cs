using System.Collections.Generic;
using System.Linq;
using System.Text;
using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils.Helpers
{
#if DEBUG
    public
#else
    internal
#endif
        static class JsonSerializerHelper
    {
        
        /// <summary>
        ///     Converts a heartbeat to a JSON string.
        /// </summary>
        /// <param name="heartbeat">A heartbeat to serialize.</param>
        /// <param name="isExtraHeartbeat">Whether to include flags that are not for extra heartbeat.</param>
        /// <param name="obfuscate"> Whether to obfuscate the values. If set to <c>true</c> only flags that have the <see cref="IFlag.CanObfuscate" /> of the <see cref="IFlag" /> will be obfuscated.</param>
        /// <returns>JSON string a single heartbeat. <c>{json}</c></returns>
#if DEBUG
        public
#else
        internal
#endif
            static string ToJson(FlagHolder heartbeat, bool isExtraHeartbeat = true, bool obfuscate = false)
        {
            string jsonArr = ToJson(new []{heartbeat}, isExtraHeartbeat, obfuscate);
            return jsonArr.Substring(1, jsonArr.Length - 2);
        }
        
        /// <summary>
        ///     Converts a list of heartbeats to a JSON string.
        /// </summary>
        /// <param name="heartbeats">The list of heartbeats to serialize.</param>
        /// <param name="isExtraHeartbeat">Whether to include flags that are not for extra heartbeat.</param>
        /// <param name="obfuscate">
        ///     Whether to obfuscate the values. If set to <c>true</c> only flags that have the
        ///     <see cref="IFlag.CanObfuscate" /> of the <see cref="IFlag" /> will be obfuscated.
        /// </param>
        /// <returns>JSON string representing an array of heartbeats. <c>[{json},{json},...]</c> </returns>
#if DEBUG 
        public
#else
        internal
#endif
            // ReSharper disable once CognitiveComplexity
            static string ToJson(IEnumerable<FlagHolder> heartbeats, bool isExtraHeartbeat = true, bool obfuscate = false)
        {
            if(heartbeats == null) return "[]";
            
            int heartbeatCount = 0;
            var b = new StringBuilder();
            b.Append("[");

            foreach (var hb in heartbeats)
            {
                if (heartbeatCount > 0) b.Append(",");

                int jsonMemberCount = 0;
                b.Append("{");

                foreach (var flag in hb.Flags)
                {
                    // skip flags that are not for extra heartbeat
                    if (!isExtraHeartbeat && !flag.Value.ForExtraHeartbeat) continue;

                    if (jsonMemberCount > 0) b.Append(",");
                    string flagJsonValue = flag.Value.GetFormattedForJson(obfuscate);

                    // skip flags that are empty
                    if (string.IsNullOrEmpty(flagJsonValue)) continue;

                    b.Append(flagJsonValue);
                    jsonMemberCount++;
                }

                b.Append("}");
                heartbeatCount++;
            }

            return b.Append("]")
                    .ToString();
        }

        /// <summary>
        ///     Escapes the string to be used in JSON.
        /// </summary>
        /// <param name="value">The string to escape.</param>
        /// <returns>Escaped string.</returns>
#if DEBUG
        public
#else
        internal
#endif
        // ReSharper disable once CognitiveComplexity
        static string JsonEscape(string value)
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