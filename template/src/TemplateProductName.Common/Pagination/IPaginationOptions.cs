namespace TemplateProductName.Common.Pagination
{
    public interface IPaginationOptions
    {
        int? Page { get; }
        int? PageSize { get; }
        int? Pages { get; }
        string SortProperty { get; }
        SortDirection? SortDirection { get; }
    }
}
