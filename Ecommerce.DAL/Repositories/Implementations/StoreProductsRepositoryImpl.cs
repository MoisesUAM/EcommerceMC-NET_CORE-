using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class StoreProductsRepositoryImpl : GenericRepositoryImpl<StoreProductModel>, IStoreProductsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StoreProductsRepositoryImpl(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(StoreProductModel storeProductModel)
        {
            var storeProductToUpdate = _dbContext.StoresProducts.FirstOrDefault(
                sp=>sp.IdProduct == storeProductModel.IdProduct &&
                sp.IdStore == storeProductModel.IdStore
                );

            if ( storeProductToUpdate != null )
            {
                storeProductToUpdate.OnHand = storeProductToUpdate.OnHand;
                _dbContext.SaveChanges();
            }
        }
    }
}
