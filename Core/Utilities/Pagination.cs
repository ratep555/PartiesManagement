using System.Collections.Generic;

namespace Core.Utilities
{
    public class Pagination<T> where T : class
    {
        public Pagination(int page, int pageCount, int count, IEnumerable<T> data)
        {
            Page = page;
            PageCount = pageCount;
            Count = count;
            Data = data;
        }

        public int Page { get; set; }
        public int PageCount { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
    