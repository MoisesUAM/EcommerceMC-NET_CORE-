using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class StoreRepositoryImpl : GenericRepositoryImpl<StoreModel>, IStoreRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StoreRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Update(StoreModel storeModel)
        {
            var storeBD = _dbContext.Stores.FirstOrDefault(s => s.IdStore == storeModel.IdStore);
            if (storeBD != null)
            {
                storeBD.Name = storeModel.Name;
                storeBD.Description = storeModel.Description;
                storeBD.Estate = storeModel.Estate;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
