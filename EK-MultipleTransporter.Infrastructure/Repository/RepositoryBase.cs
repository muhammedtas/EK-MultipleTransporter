using EK_MultipleTransporter.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EK_MultipleTransporter.Infrastructure.Repository
{
    public abstract class RepositoryBase<T, Id> where T : class
    {
        protected internal static OTCSDbContext dbContext;

        public virtual List<T> GetAll()
        {
            dbContext = new OTCSDbContext();
            return dbContext.Set<T>().ToList();
        }
        public virtual T GetById(Id id)
        {
            dbContext = new OTCSDbContext();
            return dbContext.Set<T>().Find(id);
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
    }
}
