using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class BrandRepositoryImpl : GenericRepositoryImpl<BrandModel>, IBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandRepositoryImpl(ApplicationDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Update(BrandModel brand)
        {
            var brandToUpdate = _dbContext.Brands.FirstOrDefault(b => b.IdBrand == brand.IdBrand);
            if (brandToUpdate != null)
            {
                brandToUpdate.Name = brand.Name;
                brandToUpdate.Description = brand.Description;
                brandToUpdate.Estate = brand.Estate;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
