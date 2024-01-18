using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IInventoryRepository : IGenericRepository<InventoryModel>
    {
        void Update(InventoryModel inventoryModel);
        IEnumerable<SelectListItem> SelectListStores();
    }
}
