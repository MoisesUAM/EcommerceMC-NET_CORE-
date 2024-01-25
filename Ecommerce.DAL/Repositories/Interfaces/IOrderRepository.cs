using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<OrderModel>
    {
        void Update(OrderModel order);
        void UpdateStateOrder(int orderId, string orderState, string paymentState);
        void UpdatePaymentStateStripe(int orderId, string sessionId, string transactionId);
    }
}
