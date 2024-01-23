using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class ShoppingCartRepositoryImpl : GenericRepositoryImpl<ShoppingCartModel>, IShoppinCartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingCartRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
           _dbContext = dbContext;
        }
        public void Update(ShoppingCartModel shoppingCart)
        {
            _dbContext.Update(shoppingCart);
        }
    }
}
