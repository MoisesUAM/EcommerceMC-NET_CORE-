using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class OrderRepositoryImpl : GenericRepositoryImpl<OrderModel>, IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(OrderModel order)
        {
            _dbContext.Update(order);
        }
    }
}
