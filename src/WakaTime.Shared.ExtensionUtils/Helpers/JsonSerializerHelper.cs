using System.Collections.Generic;
using System.Text;

namespace WakaTime.Shared.ExtensionUtils.Helpers
{
    internal static class JsonSerializerHelper
    {
        /// <summary>
        ///     Converts the heartbeat flags to JSON.
        /// </summary>
        /// <param name="heartbeats">The list of heartbeats to serialize.</param>
        /// <param name="isExtraHeartbeat">Whether to include flags that are not for extra heartbeat.</param>
        /// <returns>JSON string representation of a heartbeat and its flags.</returns>
        // ReSharper disable once CognitiveComplexity
        internal static string ToJson(IEnumerable<CliHeartbeat> heartbeats, bool isExtraHeartbeat = true)
        {
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
                    string flagJsonValue = flag.Value.ForJson;
                    
                    // skip flags that are empty
                    if(string.IsNullOrEmpty(flagJsonValue)) continue;
                    
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
        // ReSharper disable once CognitiveComplexity
        internal static string JsonEscape(string value)
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