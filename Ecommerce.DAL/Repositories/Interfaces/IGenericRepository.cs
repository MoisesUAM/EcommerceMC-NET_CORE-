using Ecommerce.Models.PaginationSpecs;
using System.Linq.Expressions;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T> , IOrderedQueryable<T>>? orderBy = null,
            string? includedProperties = null,
            bool isTracking = true
            );

        PaginatedList<T> GetAllPaginatedItems(
            PageParameters parameters,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includedProperties = null,
            bool isTracking = true
          );

        Task<T> GetFirst(
            Expression<Func<T, bool>>? filter = null,
            string? includedProperties = null,
            bool isTracking = true
            );
        Task Add( T entity );
        void Remove( T entity );
        void DeleteRange( IEnumerable<T> entities );
    }
}
