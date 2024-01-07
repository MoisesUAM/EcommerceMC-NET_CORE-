using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;
using Ecommerce.Models.Records;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class ProductRepositoryImpl : GenericRepositoryImpl<ProductModel>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepositoryImpl(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Update(ProductModel product)
        {
            var productToUpdate = _dbContext.Products.FirstOrDefault(p=>p.IdProduct == product.IdProduct);

            if (productToUpdate != null)
            {
                if(product.ImageUrl != null)
                {
                    productToUpdate.ImageUrl = product.ImageUrl;
                }
                productToUpdate.SerialNumber = product.SerialNumber;
                productToUpdate.Description = product.Description;
                productToUpdate.Price = product.Price;
                productToUpdate.CostPrice = product.CostPrice;
                productToUpdate.Estate = product.Estate;
                productToUpdate.IdCategory = product.IdCategory;
                productToUpdate.IdBrand = product.IdBrand;
                productToUpdate.MasterId = product.MasterId;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetListBrandRecords()
        {
            var _brandRecords = await _dbContext.Brands.Where(b => b.Estate == true).Select(b => new BrandRecord(b.IdBrand, b.Name)).ToListAsync();

            return _brandRecords.Select(s => new SelectListItem
            {
                Value = s.IdBrand.ToString(),
                Text = s.Name
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetListCategoryRecords()
        {
            var _categoryRecords = await _dbContext.Categories.Where(c => c.Estate == true).Select(c => new CategoryRecord(c.IdCategory, c.Name)).ToListAsync();
            return _categoryRecords.Select(c => new SelectListItem
            {
                Value = c.IdCategory.ToString(),
                Text = c.Name
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetListProductRecords()
        {
            var _productRecords = await _dbContext.Products.Where(p=>p.Estate == true).Select(p => new ProductRecord(p.IdProduct, p.SerialNumber, p.Description)).ToListAsync();
            return _productRecords.Select(p => new SelectListItem
            {
                Value = p.IdProduct.ToString(),
                Text = p.SerialNumber + " " + p.Description
            });
        }


    }
}
