using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class UserModelRepositoryImpl:GenericRepositoryImpl<UserModel>, IUserModelRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserModelRepositoryImpl(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
  
    }
}
