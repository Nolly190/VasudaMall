using System.Collections.Generic;

namespace VasudaDataAccess.Data_Access
{
   public interface IRepository<TEntity> where  TEntity:class
    {
        void Add(TEntity model);
        void AddRange(IEnumerable<TEntity> model);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        void Remove(TEntity model);
        void RemoveRange(IEnumerable<TEntity> model);
    }
}
