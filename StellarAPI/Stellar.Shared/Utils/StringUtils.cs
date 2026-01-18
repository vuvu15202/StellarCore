using System;

namespace Stellar.Shared.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Capitalize the first character of a string.
        /// </summary>
        /// <param name="input">The string to capitalize</param>
        /// <returns>String with first character capitalized, or original if null/empty</returns>
        public static string? Capitalize(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1);
        }

        /// <summary>
        /// Safely parse a string to GUID, returning null if parsing fails.
        /// </summary>
        /// <param name="input">The string to parse</param>
        /// <returns>Parsed GUID or null if invalid</returns>
        public static Guid? SafeParseGuid(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (Guid.TryParse(input, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
