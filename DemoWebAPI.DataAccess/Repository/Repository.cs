using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using DemoWebAPI.DataAccess.Data;
using DemoWebAPI.DataAccess.Repository.IRepository;


namespace DemoWebAPI.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DemoWebAPIDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(DemoWebAPIDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null,
                                      string? includeProperties = null,
                                      bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
                                               Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null,
                                               string? includeProperties = null,
                                               bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderby != null)
            {
                query = orderby(query);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            await dbSet.AddRangeAsync(entity);
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public async Task AttachAsync(T entity)
        {
            dbSet.Attach(entity).State = EntityState.Modified;
        }

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entity)
        {
            dbSet.UpdateRange(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AnyAsync();
        }
    }
}
