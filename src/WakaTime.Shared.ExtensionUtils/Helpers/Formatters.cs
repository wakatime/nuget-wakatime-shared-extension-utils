using System;
using System.Linq;
using System.Text;

namespace WakaTime.Shared.ExtensionUtils.Helpers
{
    internal static class Formatters
    {
        /// <summary>
        ///     Formats the value for JSON serialization.
        /// </summary>
        /// <param name="jsonFlagName">The key name for JSON serialization.</param>
        /// <param name="value">The value to format.</param>
        /// <param name="type">The type of the value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The formatted value for JSON serialization.</returns>
        /// <exception cref="ArgumentException">Thrown when the type is not supported.</exception>
        internal static string JsonFormatter<T>(string jsonFlagName, string value, T type)
        {
            if (typeof(T) == typeof(bool))
                return string.IsNullOrEmpty(value) ? string.Empty : $"\"{jsonFlagName}\": {value.ToLower()}";

            if (typeof(T) == typeof(string))
                return string.IsNullOrEmpty(value) ? string.Empty : $"\"{jsonFlagName}\": \"{JsonSerializerHelper.JsonEscape(value)}\"";

            throw new ArgumentException($"Unsupported type: {typeof(T)}");
        }

        /// <summary>
        ///     Formats the value for CLI arguments.
        /// </summary>
        /// <param name="cliFlagName">The flag name for the CLI arguments.</param>
        /// <param name="value">
        ///     The value as string to format.  <b>Note:</b> Parameters <paramref name="value" /> and <paramref name="type" /> are
        ///     required for the method signature to allow type inference. <br />
        ///     The <paramref name="value" /> param can carry obfuscated value for sensitive data. <br />
        ///     The <paramref name="type" /> param is only used for handling <see langword="bool" /> type. <br />
        /// </param>
        /// <param name="type">
        ///     Used for type inference. Carries the original not obfuscated value of the flag.
        /// </param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>
        ///     The formatted value for CLI arguments. <br /> <br />
        ///     For <see langword="bool" /> type, returns the flag name if the value is <see langword="true" /> and empty string
        ///     otherwise. <br />
        ///     For <see langword="string" /> type, returns the flag name and value if the value is not empty and empty string
        ///     otherwise. <b>Note:</b> If required, the value should be obfuscated before passing to this method.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the type <typeparamref name="T" /> is not supported.</exception>
        /// <remarks>
        ///     <b>Note:</b> The return format for <see langword="string" /> type is <c>flagName" "value</c>.
        /// This is by design to allow correct parameters handling in the <see cref="RunProcess" /> class method of the same name.
        /// </remarks>
        internal static string CliFormatter<T>(string cliFlagName, string value, T type)
        {
            if (typeof(T) == typeof(bool))
                return type is bool b && b ? cliFlagName : string.Empty;

            if (typeof(T) == typeof(string))
                return string.IsNullOrEmpty(value) ? string.Empty : $"{cliFlagName}\" \"{value}";

            throw new ArgumentException($"Unsupported type: {typeof(T)}");
        }

        /// <summary>
        ///     Formats the value for the string representation.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="obfuscate">Whether to obfuscate the value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>
        ///     The formatted value for the string representation.
        ///     If <paramref name="obfuscate" /> is true, the value is obfuscated with 'X' characters.
        /// </returns>
        internal static string ValueFormatter<T>(T value, bool obfuscate)
        {
            string valueString = value.ToString();

            // Convert boolean values to lowercase
            if (typeof(T) == typeof(bool))
                valueString = valueString.ToLower();

            return !obfuscate ? valueString : Obfuscate(valueString, 'X', new[] { '-' });
        }

        private static string Obfuscate(string value, char obfuscationChar = 'X', char[] ignoreChars = null)
        {
            var sb = new StringBuilder(value.Length);
            char[] chars = value.ToCharArray();
            int revealLength; // number of visible characters at the end of the string
            if (chars.Length > 4)
                revealLength = 4;
            else if (chars.Length < 4 && chars.Length > 2)
                revealLength = 2;
            else if (chars.Length < 2)
                revealLength = 1;
            else
                revealLength = 0;

            for (int i = 0; i < chars.Length - revealLength; i++)
            {
                if (ignoreChars != null && ignoreChars.Contains(chars[i])) sb.Append(chars[i]);
                else sb.Append(obfuscationChar);
            }

            var visibleChars = chars.Skip(chars.Length - revealLength)
                                    .Take(revealLength);
            
            sb.Append(visibleChars.ToArray());

            return sb.ToString();
        }
    }
}