using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IOrderDetailRepository:IGenericRepository<OrderDetailsModel>
    {
        void Update(OrderDetailsModel orderDetails);
    }
}
