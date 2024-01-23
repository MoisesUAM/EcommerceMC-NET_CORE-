using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class OrderDetailsRepositoryImpl : GenericRepositoryImpl<OrderDetailsModel>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderDetailsRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(OrderDetailsModel orderDetails)
        {
            _dbContext.Update(orderDetails);
        }
    }
}
