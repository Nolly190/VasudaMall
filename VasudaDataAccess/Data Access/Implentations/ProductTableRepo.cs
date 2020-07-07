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

        public List<ProductTable> GetAllPopular(int? pageNum)
        {
           return  pageNum.HasValue ?dbcontext.Set<ProductTable>().Where(x=>x.IsPopular).Skip(pageNum.Value*10).Take(10).ToList(): dbcontext.Set<ProductTable>().Where(x => x.IsPopular).Take(10).ToList();
        }

        public List<ProductTable> GetAllTrending(int? pageNum)
        {
            return pageNum.HasValue ? dbcontext.Set<ProductTable>().Where(x => !x.IsPopular).Skip(pageNum.Value * 10).Take(10).ToList() : dbcontext.Set<ProductTable>().Where(x => !x.IsPopular).Take(10).ToList();

        }
    }
}
