namespace Ecommerce.Models.PaginationSpecs
{
    public class PaginatedList<T> : List<T>
    {
        public PageMetaData? PageMetaData { get; set; }

        public PaginatedList(List<T> items, PageParameters pageParameters)
        {
            PageMetaData = new PageMetaData
            {
                TotalCount = pageParameters.TotalRecords,
                PageSize = pageParameters.PageSize,
                TotalPages = (int)Math.Ceiling(pageParameters.TotalRecords / (double)pageParameters.PageSize)
            };
            AddRange(items);
        }

        public static PaginatedList<T> ToPaginatedList(IEnumerable<T> entity, PageParameters parameters)
        {
            parameters.TotalRecords = entity.Count();
            var items = entity.Skip((parameters.PageNumber -1) * parameters.PageSize).Take(parameters.PageSize).ToList();
            return new PaginatedList<T>(items, parameters);
        }   
    }
}
