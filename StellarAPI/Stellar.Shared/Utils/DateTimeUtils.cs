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

        public static DateTime GetStartOfMonth(DateTime? date = null)
        {
            var d = date ?? DateTime.Now;
            return new DateTime(d.Year, d.Month, 1, 0, 0, 0);
        }

        public static DateTime GetEndOfMonth(DateTime? date = null)
        {
            var d = date ?? DateTime.Now;
            int lastDay = DateTime.DaysInMonth(d.Year, d.Month);

            return new DateTime(
                d.Year,
                d.Month,
                lastDay,
                23, 59, 59
            );
        }


        public static DateTime GetStartOfWeekDateTime(DateTime? date = null)
        {
            var d = (date ?? DateTime.Now).Date;
            return GetStartOfWeek(d);
        }

        public static DateTime GetEndOfWeekDateTime(DateTime? date = null)
        {
            var d = (date ?? DateTime.Now).Date;
            return GetEndOfWeek(d).AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public static DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }

        public static DateTime GetEndOfWeek(DateTime date)
        {
            return GetStartOfWeek(date).AddDays(6);
        }

        /// <summary>
        /// buildWeekdayLabels(new CultureInfo("vi-VN"))
        /// buildWeekdayLabels(CultureInfo.InvariantCulture)
        /// </summary>
        public static string[] BuildWeekdayLabels(CultureInfo culture)
        {
            var labels = new string[7];

            // Monday = 1 in Java, Sunday = 0 in .NET
            for (int i = 0; i < 7; i++)
            {
                var day = (DayOfWeek)(((int)DayOfWeek.Monday + i) % 7);
                labels[i] = culture.DateTimeFormat.GetAbbreviatedDayName(day);
            }

            return labels;
        }

        public static DateTime StartOfDay(DateTime? date = null)
        => (date ?? DateTime.Now).Date;

        public static DateTime EndOfDay(DateTime? date = null)
            => (date ?? DateTime.Now).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        public static DateTime EndOfDayExclusive(DateTime? date = null)
            => (date ?? DateTime.Now).Date.AddDays(1);
    }
}
