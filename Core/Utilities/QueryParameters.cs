namespace Core.Utilities
{
    public class QueryParameters
    {
        private const int MaxPageCount = 50;
        public int Page { get; set; } = 1;
        private int _pageCount = MaxPageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > MaxPageCount) ? MaxPageCount : value; }
        }
        public int? ManufacturerId { get; set; }
        public int? TagId { get; set; }
        public int? CategoryId { get; set; }
        public string Sort { get; set; }
        public int? Status { get; set; }
        private string _query;
        public string Query
        {
            get => _query;
            set => _query = value.ToLower();
        }
    }
}