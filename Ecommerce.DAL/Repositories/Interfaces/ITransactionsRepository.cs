using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface ITransactionsRepository : IGenericRepository<TransactionsModel>
    {
        Task RegisterTransaction(int productId, int storeId, string typeTransaction, string comments, int lastStock, int quantity, string userId);
    }
}
