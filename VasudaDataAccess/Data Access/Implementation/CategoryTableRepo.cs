using System.Data.Entity;
using VasudaDataAccess.Model;
using VasudaDataAccess.Data_Access.Implementation;

namespace VasudaDataAccess.Data_Access.Implementation
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