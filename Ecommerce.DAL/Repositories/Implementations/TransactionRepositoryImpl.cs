using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class TransactionRepositoryImpl : GenericRepositoryImpl<TransactionsModel>, ITransactionsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RegisterTransaction(int productId, int storeId, string typeTransaction, string comments, int lastStock, int quantity, string userId)
        {
           
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.IdProduct == productId);

            if (typeTransaction.Equals("IN"))
            {
                TransactionsModel transaction = new TransactionsModel();
                transaction.IdProduct = productId;
                transaction.IdStore = storeId;
                transaction.Type= typeTransaction;
                transaction.Comments = comments;
                transaction.LastStock = lastStock;
                transaction.Quantity = quantity;
                transaction.Cost = product!.CostPrice;
                transaction.CurrentStock = lastStock + quantity;
                transaction.UserId = userId;
                transaction.CommitDate = DateTime.UtcNow;

                await _dbContext.Transactions.AddAsync(transaction);
                await _dbContext.SaveChangesAsync();

            }

            if (typeTransaction.Equals("OUT"))
            {
                TransactionsModel transaction = new TransactionsModel();
                transaction.IdProduct = productId;
                transaction.IdStore = storeId;
                transaction.Type = typeTransaction;
                transaction.Comments = comments;
                transaction.LastStock = lastStock;
                transaction.Quantity = quantity;
                transaction.Cost = product!.CostPrice;
                transaction.CurrentStock = lastStock - quantity;
                transaction.UserId = userId;
                transaction.CommitDate = DateTime.UtcNow;

                await _dbContext.Transactions.AddAsync(transaction);
                await _dbContext.SaveChangesAsync();

            }
        }
    }
}
