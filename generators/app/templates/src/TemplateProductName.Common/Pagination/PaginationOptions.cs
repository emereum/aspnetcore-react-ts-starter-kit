namespace TemplateProductName.Common.Pagination
{
    public class PaginationOptions : IPaginationOptions
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? Pages { get; set; }
        public string SortProperty { get; set; }
        public SortDirection? SortDirection { get; set; }

        internal PaginationOptions() { }
    }
}
