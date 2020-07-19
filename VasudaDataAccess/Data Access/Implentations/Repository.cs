using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace VasudaDataAccess.Data_Access.Implentations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext dbcontext { get; set; }
        public Repository(DbContext _context)
        {
            dbcontext = _context;
        }
        public void Add(TEntity model)
        {
            dbcontext.Set<TEntity>().Add(model);
        }

        public void AddRange(IEnumerable<TEntity> model)
        {
            dbcontext.Set<TEntity>().AddRange(model);
        }

        public TEntity Get(int id)
        {
            return dbcontext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbcontext.Set<TEntity>();
        }

        public void Remove(TEntity model)
        {
            dbcontext.Set<TEntity>().Remove(model);
        }

        public void RemoveRange(IEnumerable<TEntity> model)
        {
            dbcontext.Set<TEntity>().RemoveRange(model);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
           return dbcontext.Set<TEntity>().SingleOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Set<TEntity>().Where(predicate);
        }
    }
}