using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implementation
{
    public class AspNetUserRepo : Repository<AspNetUser>, IAspNetUser
    {
        public AspNetUserRepo(DbContext context) : base(context)
        {

        }

        public DbContext Context
        {
            get
            {
                return dbcontext as DbContext;
            }
        }

        public AspNetUser GetUser(string userId)
        {
            var rec = dbcontext.Set<AspNetUser>().Where(x => x.Id == userId).ToList();
            if (rec.Any())
            {
                return rec.FirstOrDefault();
            }

            return null;
        }
    }
}
