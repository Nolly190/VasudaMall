using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
  public class Response<TEntity>
  {
      public TEntity _entity { get; private set; }
      public bool Status { get; set; }    
      public string Message { get; set; }

        public TEntity Result()
        {
            return _entity;
        }
        public void SetResult(TEntity entity)
        {
            _entity = entity;
        }
  }

}
