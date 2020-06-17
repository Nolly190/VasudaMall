using System.Data.Entity;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implentations
{
    public class CategoryTableRepo : Repository<CategoryTable>, ICategoryTable
    {

        public CategoryTableRepo(DbContext context) : base(context)
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