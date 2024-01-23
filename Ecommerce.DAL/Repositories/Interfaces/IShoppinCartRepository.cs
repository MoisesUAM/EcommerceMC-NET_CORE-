using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IShoppinCartRepository : IGenericRepository<ShoppingCartModel>
    {
        void Update(ShoppingCartModel shoppingCart);
    }
}
