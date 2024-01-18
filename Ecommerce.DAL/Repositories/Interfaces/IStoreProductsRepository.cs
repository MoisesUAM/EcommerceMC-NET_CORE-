using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IStoreProductsRepository:IGenericRepository<StoreProductModel>
    {
        void Update(StoreProductModel storeProductModel);
    }
}
