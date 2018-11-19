using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Data
{
    public class RepositoryBase<T, TId> where T : class
    {
        protected internal static OtcsDbContext DbContext;

        public virtual List<T> GetAll()
        {
            DbContext = new OtcsDbContext();
            return DbContext.Set<T>().ToList();
        }
        public virtual async Task<List<T>> GetAllAsync()
        {
            DbContext = new OtcsDbContext();
            return await DbContext.Set<T>().ToListAsync();
        }
        public virtual T GetById(TId id)
        {
            DbContext = new OtcsDbContext();
            return DbContext.Set<T>().Find(id);
        }
        public virtual async Task<T> GetByIdAsync(TId id)
        {
            DbContext = new OtcsDbContext();
            return await DbContext.Set<T>().FindAsync(id);
        }
        public virtual int Insert(T entity)
        {
            try
            {
                DbContext = DbContext ?? new OtcsDbContext();
                DbContext.Set<T>().Add(entity);
                return DbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual async Task<int> InsertAsync(T entity)
        {
            try
            {
                DbContext = DbContext ?? new OtcsDbContext();
                DbContext.Set<T>().Add(entity);
                return await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public virtual int Delete(T entity)
        {
            try
            {
                DbContext = DbContext ?? new OtcsDbContext();
                DbContext.Set<T>().Remove(entity);
                return DbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            try
            {
                DbContext = DbContext ?? new OtcsDbContext();
                DbContext.Set<T>().Remove(entity);
                return await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual int Update()
        {
            try
            {
                DbContext = DbContext ?? new OtcsDbContext();
                return DbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual async Task<int> UpdateAsync()
        {
            try
            {
                DbContext = DbContext ?? new OtcsDbContext();
                return await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
