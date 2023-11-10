using C8.eServices.Mvc.Models;
using C8.eServices.Mvc.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace C8.eServices.Mvc.Engines
{
    public class GenericEntityBuilder<TEntity> where TEntity : class
    {
        protected readonly eServicesDbContext core;
        protected static eServicesDbContext _core;

        public GenericEntityBuilder(eServicesDbContext dbContext)
        {
            core = dbContext;
            _core = dbContext;
        }

        public void AddToTable(TEntity entity)
        {
            core.Set<TEntity>().Add(entity);
        }

        public void UpdateTable(TEntity entity)
        {
            core.Set<TEntity>().Attach(entity);
            core.Entry(entity).State = EntityState.Modified;
        }
        public void RangeAdd(List<TEntity> entity)
        {
            core.Set<TEntity>().AddRange(entity);

        }


        //public void RemoveModel<TEntity>(TEntity entity)
        //{
        //    _ = core.Set<TEntity>().Remove(entity);
        //}

        //public void RemoveModelRange<TEntity>(IEnumerable<TEntity> entity)
        //{
        //    core.Set<TEntity>().RemoveRange(entity);
        //}
        //public static List<T>  GetActionsTypes<T> (T entry) where T : class, new()
        //{
        //    return _core.Set<T>().ToList();
        //}
        public List<TEntity> GetActionsTypes<TEntity>() where TEntity : class, new()
        {
            return _core.Set<TEntity>().ToList();
        }

        public TEntity GetById(int? Id)
        {
            return core.Set<TEntity>().Find(Id) ?? null;
        }
        public TEntity GetTypeByKey<TEntity>(Expression<Func<TEntity, bool>> prediction) where TEntity : class, new()
        {
            return _core.Set<TEntity>().FirstOrDefault(prediction);
        }
        public List<TEntity> GetSubCategories<TEntity>(Expression<Func<TEntity, bool>> prediction) where TEntity : class, new()
        {
            return _core.Set<TEntity>().Where(prediction).ToList();
        }

        public void Complete()
        {
            core.SaveChanges();
        }

        public void Dispose()
        {
            core.Dispose();
        }
    }

}