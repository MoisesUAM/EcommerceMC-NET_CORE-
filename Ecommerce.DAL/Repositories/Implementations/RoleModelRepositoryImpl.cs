using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class RoleModelRepositoryImpl:GenericRepositoryImpl<RoleModel>, IRoleModelRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleModelRepositoryImpl(ApplicationDbContext dbContext):base(dbContext) 
        { 
            _dbContext = dbContext;
        }
    }
}
