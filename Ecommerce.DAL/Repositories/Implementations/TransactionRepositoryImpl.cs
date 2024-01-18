using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class TransactionRepositoryImpl : GenericRepositoryImpl<TransactionsModel>, ITransactionsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
