using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class CompanyRepositoryImpl : GenericRepositoryImpl<CompanyModel>, ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyRepositoryImpl(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(CompanyModel company)
        {
            var companyDB = _dbContext.Companies.FirstOrDefault(c=>c.IdCompany ==  company.IdCompany);
            if (companyDB != null)
            {
                companyDB.Name = company.Name;
                companyDB.Description = company.Description;
                companyDB.Country = company.Country;
                companyDB.City = company.City;
                companyDB.Address = company.Address;
                companyDB.PhoneNumber = company.PhoneNumber;
                companyDB.IdStore = company.IdStore;
                companyDB.UpdateUserId = company.UpdateUserId;
                companyDB.UpdateDate = company.UpdateDate;
                _dbContext.SaveChanges();
            }
        }
    }
}
