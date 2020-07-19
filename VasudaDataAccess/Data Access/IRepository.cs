using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VasudaDataAccess.Data_Access
{
   public interface IRepository<TEntity> where  TEntity:class
    {
        void Add(TEntity model);
        void AddRange(IEnumerable<TEntity> model);
        TEntity Get(int id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        void Remove(TEntity model);
        void RemoveRange(IEnumerable<TEntity> model);
    }
}
