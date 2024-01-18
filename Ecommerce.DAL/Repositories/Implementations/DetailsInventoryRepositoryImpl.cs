using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class DetailsInventoryRepositoryImpl : GenericRepositoryImpl<DetailsInventoryModels>, IDetailsInventoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DetailsInventoryRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(DetailsInventoryModels detailsInventory)
        {
            var detailsToUpdate = _dbContext.DetailsInventories.FirstOrDefault(d => d.IdDetailsIventory == detailsInventory.IdInventory);
            if (detailsToUpdate != null)
            {
                detailsToUpdate.LastStock = detailsInventory.LastStock;
                detailsToUpdate.Quantity = detailsInventory.Quantity;

                _dbContext.SaveChanges();
            }
        }
    }
}
