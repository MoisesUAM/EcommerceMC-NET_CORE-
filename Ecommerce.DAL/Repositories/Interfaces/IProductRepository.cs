using Ecommerce.Models.Catalog;
using Ecommerce.Models.Records;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<ProductModel>
    {
        Task Update(ProductModel product);
        Task<IEnumerable<SelectListItem>> GetListCategoryRecords();
        Task<IEnumerable<SelectListItem>> GetListBrandRecords();
        Task<IEnumerable<SelectListItem>> GetListProductRecords();
    }
}
