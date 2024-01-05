using Ecommerce.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Utilities.Interfaces
{
    public interface IUnitWork: IDisposable
    {
        IStoreRepository StoreRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IBrandRepository BrandRepository { get; }

        Task Save();
    }
}
