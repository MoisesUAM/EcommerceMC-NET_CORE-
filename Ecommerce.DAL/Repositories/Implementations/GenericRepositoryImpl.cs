using Ecommerce.DAL.Data;
using Ecommerce.DAL.Repositories.Interfaces;
using Ecommerce.Models.PaginationSpecs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.DAL.Repositories.Implementations
{
    public class GenericRepositoryImpl<T> : IGenericRepository<T> where T : class
    {
        //Se injecta el servicion de dbContext de EntityFrameWork
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<T>? DbSet;

        public GenericRepositoryImpl(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            this.DbSet = _dbContext.Set<T>();
        }



#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public async Task Add(T entity) => await DbSet.AddAsync(entity);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        public void DeleteRange(IEnumerable<T> entities)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            DbSet.RemoveRange(entities);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }




        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includedProperties = null, bool isTracking = true)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IQueryable<T> query = DbSet;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (filter != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                query = query.Where(filter); //Esto se comporta como un select*from tblExample where filter = .....
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (includedProperties != null)
            {
                foreach (var includedProperty in includedProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    query = query.Include(includedProperty);//Con Esto se obtiene ademas de las lista de propiedades de la tabla consultada, los valores de las tablas relacionadas como si fuera un join
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }

            if (orderBy != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                query = orderBy(query);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (!isTracking)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                query = query.AsNoTracking();
#pragma warning restore CS8604 // Possible null reference argument.
            }

#pragma warning disable CS8604 // Possible null reference argument.
            return await query.ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.
        }




        public PaginatedList<T> GetAllPaginatedItems(PageParameters parameters, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includedProperties = null, bool isTracking = true)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IQueryable<T> query = DbSet;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (filter != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                query = query.Where(filter); //Esto se comporta como un select*from tblExample where filter = .....
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (includedProperties != null)
            {
                foreach (var includedProperty in includedProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    query = query.Include(includedProperty);//Con Esto se obtiene ademas de las lista de propiedades de la tabla consultada, los valores de las tablas relacionadas como si fuera un join
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }

            if (orderBy != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                query = orderBy(query);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (!isTracking)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                query = query.AsNoTracking();
#pragma warning restore CS8604 // Possible null reference argument.
            }

#pragma warning disable CS8604 // Possible null reference argument.
            return PaginatedList<T>.ToPaginatedList(query, parameters);
#pragma warning restore CS8604 // Possible null reference argument.
        }



#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public async Task<T> GetById(int id) => await DbSet.FindAsync(id);//esto busca solo por id o es decir Select*from TblExample where id = id;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8603 // Possible null reference return.



        public async Task<T> GetFirst(Expression<Func<T, bool>>? filter = null, string? includedProperties = null, bool isTracking = true)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IQueryable<T> query = DbSet;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (filter != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                query = query.Where(filter);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (includedProperties != null)
            {
                foreach (var includedProperty in includedProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    query = query.Include(includedProperty);
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }

            if (!isTracking)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                query = query.AsNoTracking();
#pragma warning restore CS8604 // Possible null reference argument.
            }

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
            return await query.FirstOrDefaultAsync();
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8603 // Possible null reference return.
        }



#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public void Remove(T entity) => DbSet.Remove(entity);
#pragma warning restore CS8602 // Dereference of a possibly null reference.


    }
}
