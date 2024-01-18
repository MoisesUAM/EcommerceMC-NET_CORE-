using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class InventoryRepositoryImpl : GenericRepositoryImpl<InventoryModel>, IInventoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InventoryRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(InventoryModel inventoryModel)
        {
            var inventoryToUpdate = _dbContext.Inventories.FirstOrDefault(i => i.IdInventory == inventoryModel.IdInventory);

            if (inventoryToUpdate != null)
            {
                inventoryToUpdate.IdStore = inventoryModel.IdStore;
                inventoryToUpdate.EndDate = inventoryModel.EndDate;
                inventoryToUpdate.Estate = inventoryModel.Estate;

                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> SelectListStores()
        {
            return _dbContext.Stores.Where(s => s.Estate == true).Select(s => new SelectListItem() { 
                Text = s.Name +" --> "+s.Description,
                Value = s.IdStore.ToString()
            });;
        }
    }
}
