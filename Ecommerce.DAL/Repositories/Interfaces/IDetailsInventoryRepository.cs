using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IDetailsInventoryRepository : IGenericRepository<DetailsInventoryModels>
    {
        void Update(DetailsInventoryModels detailsInventory);
    }
}
