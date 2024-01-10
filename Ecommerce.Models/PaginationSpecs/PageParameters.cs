namespace Ecommerce.Models.PaginationSpecs
{
    public class PageParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public int TotalRecords { get; set; }
    }
}
