namespace TemplateProductName.Domain.Queries
{
    public interface IPaginationOptions
    {
        int Page { get; set; }
        int PageSize { get; set; }
        int Pages { get; set; }
        string SortProperty { get; set; }
        SortDirection SortDirection { get; set; }
    }
}
