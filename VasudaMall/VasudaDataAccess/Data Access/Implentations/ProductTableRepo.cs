using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implentations
{
    public class ProductTableRepo : Repository<ProductTable>, IProductTable
    {
        public ProductTableRepo(DbContext context) : base(context)
        {
                
        }

        public DbContext Context
        {
            get
            {
                return dbcontext as DbContext;
            }
        }
    }
}
