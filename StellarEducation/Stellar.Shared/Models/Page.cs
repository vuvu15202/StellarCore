using System.Collections.Generic;

namespace Stellar.Shared.Models
{
    public class Page<T>
    {
        public List<T> Content { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalElements { get; set; }
        public int TotalPages { get; set; }
        public bool Last { get; set; }
        public bool First { get; set; }
        public bool Empty { get; set; }

        public Page() { }

        public Page(List<T> content, int pageNumber, int pageSize, long totalElements)
        {
            Content = content;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalElements = totalElements;
            TotalPages = pageSize > 0 ? (int)System.Math.Ceiling(totalElements / (double)pageSize) : 0;
            First = pageNumber == 0;
            Last = pageNumber >= TotalPages - 1;
            Empty = content.Count == 0;
        }

        public Page<U> Map<U>(System.Func<T, U> converter)
        {
            var newContent = new List<U>();
            foreach (var item in Content)
            {
                newContent.Add(converter(item));
            }
            return new Page<U>(newContent, PageNumber, PageSize, TotalElements);
        }
    }
}
