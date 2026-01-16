using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stellar.Shared.Utils
{
    /// <summary>
    /// EF Core value converter for List<string> to semicolon-delimited string.
    /// </summary>
    public class StringListConverter : ValueConverter<List<string>, string>
    {
        private const string SPLIT_CHAR = ";";

        public StringListConverter() 
            : base(
                list => ConvertToDatabaseColumn(list),
                joined => ConvertToEntityAttribute(joined))
        {
        }

        private static string ConvertToDatabaseColumn(List<string>? list)
        {
            if (list == null || !list.Any())
                return string.Empty;

            return string.Join(SPLIT_CHAR, list.Select(s => s?.Trim() ?? string.Empty));
        }

        private static List<string> ConvertToEntityAttribute(string? joined)
        {
            if (string.IsNullOrWhiteSpace(joined))
                return new List<string>();

            return joined.Split(SPLIT_CHAR, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();
        }
    }
}
