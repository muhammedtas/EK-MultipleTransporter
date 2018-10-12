using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Data
{
    public class RepositoryBase<T, Id> where T : class
    {
        protected internal static OTCSDbContext dbContext;

        public virtual List<T> GetAll()
        {
            dbContext = new OTCSDbContext();
            return dbContext.Set<T>().ToList();
        }
        public virtual async Task<List<T>> GetAllAsync()
        {
            dbContext = new OTCSDbContext();
            return await dbContext.Set<T>().ToListAsync();
        }
        public virtual T GetById(Id id)
        {
            dbContext = new OTCSDbContext();
            return dbContext.Set<T>().Find(id);
        }
        public virtual async Task<T> GetByIdAsync(Id id)
        {
            dbContext = new OTCSDbContext();
            return await dbContext.Set<T>().FindAsync(id);
        }
        public virtual int Insert(T entity)
        {
            try
            {
                dbContext = dbContext ?? new OTCSDbContext();
                dbContext.Set<T>().Add(entity);
                return dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<int> InsertAsync(T entity)
        {
            try
            {
                dbContext = dbContext ?? new OTCSDbContext();
                dbContext.Set<T>().Add(entity);
                return await dbContext.SaveChangesAsync();
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
                dbContext = dbContext ?? new OTCSDbContext();
                dbContext.Set<T>().Remove(entity);
                return dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            try
            {
                dbContext = dbContext ?? new OTCSDbContext();
                dbContext.Set<T>().Remove(entity);
                return await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual int Update()
        {
            try
            {
                dbContext = dbContext ?? new OTCSDbContext();
                return dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<int> UpdateAsync()
        {
            try
            {
                dbContext = dbContext ?? new OTCSDbContext();
                return await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
