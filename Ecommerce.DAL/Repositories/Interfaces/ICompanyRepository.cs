using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface ICompanyRepository:IGenericRepository<CompanyModel>
    {
        void Update(CompanyModel company);
    }
}
