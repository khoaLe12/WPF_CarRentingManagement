using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer
{
    public interface IBaseDAO<T,Tkey> where T : class
    {
        Task<T?> FindByIdAsync(Tkey id);
        IQueryable<T> FindAll();
        IQueryable<T> Get(Expression<Func<T,bool>> where);
        IQueryable<T> Get(Expression<Func<T, bool>> where, Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void UnTrack(T entity);
    }

    public class BaseDAO<T, Tkey> : IBaseDAO<T, Tkey> where T : class
    {
        protected FucarRentingManagementContext _applicationContext;
        protected DbSet<T> dbSet;

        public BaseDAO(FucarRentingManagementContext applicationContext)
        {
            _applicationContext = applicationContext;
            dbSet = _applicationContext.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);

        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual IQueryable<T> FindAll()
        {
            return dbSet.AsNoTracking();
        }

        public virtual async Task<T?> FindByIdAsync(Tkey id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> where, Expression<Func<T, object>>[] includes)
        {
            var result = dbSet.Where(where);
            foreach(var include in includes)
            {
                result = result.Include(include);
            }
            return result;
        }

        public virtual void Update(T entity)
        {
            _applicationContext.Entry<T>(entity).State = EntityState.Modified;
        }

        public virtual void UnTrack(T entity)
        {
            _applicationContext.Entry<T>(entity).State = EntityState.Detached;
        }
    }
}
