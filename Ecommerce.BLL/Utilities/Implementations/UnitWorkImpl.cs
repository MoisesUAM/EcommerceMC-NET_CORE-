using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Implementations;
using Ecommerce.DAL.Repositories.Interfaces;

namespace Ecommerce.BLL.Utilities.Implementations
{
    public class UnitWorkImpl : IUnitWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IStoreRepository StoreRepository {  get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IBrandRepository BrandRepository { get; private set; }

        public UnitWorkImpl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            StoreRepository = new StoreRepositoryImpl(_dbContext);
            CategoryRepository = new CategoryRepositoryImpl(_dbContext);
            BrandRepository = new BrandRepositoryImpl(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
