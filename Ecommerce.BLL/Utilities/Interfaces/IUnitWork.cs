using Ecommerce.DAL.Repositories.Interfaces;

namespace Ecommerce.BLL.Utilities.Interfaces
{
    public interface IUnitWork: IDisposable
    {
        IStoreRepository StoreRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IBrandRepository BrandRepository { get; }
        IProductRepository ProductRepository { get; }
        IUserModelRepository UserModelRepository { get; }
        IStoreProductsRepository StoreProductsRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        IDetailsInventoryRepository DetailsInventoryRepository { get; }
        ITransactionsRepository TransactionsRepository { get; }
        ICompanyRepository CompanyRepository { get; }

        Task Save();
    }
}
