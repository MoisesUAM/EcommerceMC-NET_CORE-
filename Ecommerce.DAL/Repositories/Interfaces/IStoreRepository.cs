using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IStoreRepository : IGenericRepository<StoreModel>
    {
        Task Update(StoreModel storeModel);
    }
}
