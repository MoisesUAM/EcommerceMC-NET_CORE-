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

        public void UpdatePaymentStateStripe(int orderId, string sessionId, string transactionId)
        {
            var orderDB = _dbContext.Orders.FirstOrDefault(o=>o.IdOrder == orderId);
            if (orderDB != null)
            {
                if(!String.IsNullOrEmpty(sessionId))
                {
                    orderDB!.SessionId = sessionId;
                }
                if(!String.IsNullOrEmpty(transactionId))
                {
                    orderDB!.IdTransaction = transactionId;
                    orderDB.PaymentDate = DateTime.Now;
                }

                _dbContext.SaveChanges();
            }
        }

        public void UpdateStateOrder(int orderId, string orderState, string paymentState)
        {
            var orderDB = _dbContext.Orders.FirstOrDefault(o=>o.IdOrder == orderId);
            if (orderDB != null)
            {
                orderDB.OrderState = orderState;
                orderDB.PaymentState = paymentState;
            }

            _dbContext.SaveChanges();
        }
    }
}
