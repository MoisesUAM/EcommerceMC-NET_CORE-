using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<CategoryModel>
    {
        Task Update(CategoryModel category);
    }
}
