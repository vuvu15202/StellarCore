using System;
using System.Collections.Generic;
using System.Linq;
using Stellar.Shared.Models.Enums;

namespace Stellar.Shared.Utils
{
    public static class PageableUtils
    {
        /// <summary>
        /// Convert page, size, and sort parameters to pagination info.
        /// </summary>
        /// <param name="page">Page number (1-indexed)</param>
        /// <param name="size">Page size</param>
        /// <param name="sortStr">JSON sort specification</param>
        /// <returns>Tuple of (skip, take, sortFields)</returns>
        public static (int skip, int take, List<(string field, bool ascending)> sorts) ConvertPageable(
            int? page = null,
            int? size = null,
            string? sortStr = null)
        {
            var pageNum = page ?? 1;
            var pageSize = size ?? 20;

            // Parse sort specification
            var sorts = new List<(string field, bool ascending)>();
            
            if (!string.IsNullOrWhiteSpace(sortStr))
            {
                try
                {
                    var sortMap = JsonParserUtils.ToStringMap(sortStr);
                    
                    foreach (var entry in sortMap)
                    {
                        var sortValue = NumberUtils.ParseInt(entry.Value, 1);
                        var sortEnum = SortEnumExtensions.FromOriginal(sortValue);
                        var ascending = sortEnum == SortEnum.ASC;
                        
                        sorts.Add((entry.Key, ascending));
                    }
                }
                catch
                {
                    // If parsing fails, use default (no sorting)
                }
            }

            // Calculate skip and take
            int skip = 0;
            int take = int.MaxValue;

            if (pageNum > 0 && pageSize > 0)
            {
                skip = (pageNum - 1) * pageSize;
                take = pageSize;
            }

            return (skip, take, sorts);
        }
    }
}
