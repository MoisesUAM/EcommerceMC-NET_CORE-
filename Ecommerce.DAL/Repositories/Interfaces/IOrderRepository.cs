using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<OrderModel>
    {
        void Update(OrderModel order);
    }
}
