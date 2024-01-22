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
        public IProductRepository ProductRepository { get; private set; }
        public IUserModelRepository UserModelRepository { get; private set; }
        public IStoreProductsRepository StoreProductsRepository { get; private set; }
        public IInventoryRepository InventoryRepository { get; private set; }
        public IDetailsInventoryRepository DetailsInventoryRepository { get; private set; }
        public ITransactionsRepository TransactionsRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; set; }
        public UnitWorkImpl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            StoreRepository = new StoreRepositoryImpl(_dbContext);
            CategoryRepository = new CategoryRepositoryImpl(_dbContext);
            BrandRepository = new BrandRepositoryImpl(_dbContext);
            ProductRepository = new ProductRepositoryImpl(_dbContext);
            UserModelRepository = new UserModelRepositoryImpl(_dbContext);
            StoreProductsRepository = new StoreProductsRepositoryImpl(_dbContext);
            InventoryRepository = new InventoryRepositoryImpl(_dbContext);
            DetailsInventoryRepository = new DetailsInventoryRepositoryImpl(_dbContext);
            TransactionsRepository = new TransactionRepositoryImpl(_dbContext);
            CompanyRepository = new CompanyRepositoryImpl(_dbContext);

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
