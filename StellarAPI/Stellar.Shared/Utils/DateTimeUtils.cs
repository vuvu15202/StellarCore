using System;
using System.Globalization;

namespace Stellar.Shared.Utils
{
    public static class DateTimeUtils
    {
        private static readonly string[] SupportedPatterns = new[]
        {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd'T'HH:mm:ss",
            "yyyy/MM/dd HH:mm:ss",
            "dd/MM/yyyy HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-dd'T'HH:mm:ss.fff",
            "yyyyMMddHHmmss"
        };

        /// <summary>
        /// Convert String to DateTime with support for multiple formats.
        /// </summary>
        /// <param name="input">The datetime string to parse</param>
        /// <returns>Parsed DateTime or null if input is null/empty</returns>
        /// <exception cref="ArgumentException">Thrown when the format is not supported</exception>
        public static DateTime? ToDateTime(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            foreach (var pattern in SupportedPatterns)
            {
                if (DateTime.TryParseExact(input, pattern, CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out var result))
                {
                    return result;
                }
            }

            throw new ArgumentException($"Unsupported datetime format: {input}");
        }
    }
}
