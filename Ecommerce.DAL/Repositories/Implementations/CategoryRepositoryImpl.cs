using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class CategoryRepositoryImpl : GenericRepositoryImpl<CategoryModel>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task Update(CategoryModel category)
        {
            var _Category = _dbContext.Categories.FirstOrDefault(c => c.IdCategory == category.IdCategory);
            if (_Category != null)
            {
                _Category.Name = category.Name;
                _Category.Description = category.Description;
                _Category.Estate = category.Estate;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
