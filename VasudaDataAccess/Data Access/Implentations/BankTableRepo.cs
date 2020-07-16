using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implentations
{
    public class BankTableRepo : Repository<BankTable>, IBankTable
    {
        public BankTableRepo(DbContext context) : base(context)
        {

        }

        public DbContext Context
        {
            get
            {
                return dbcontext as DbContext;
            }
        }

        public List<BankTable> GetAllActiveBanks()
        {
            return dbcontext.Set<BankTable>().Where(x => x.IsActive == true).ToList();
        }
    }
}
