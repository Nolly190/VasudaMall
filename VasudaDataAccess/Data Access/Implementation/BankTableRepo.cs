using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;
using VasudaDataAccess.Data_Access.Implementation;

namespace VasudaDataAccess.Data_Access.Implementation
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
    public class FundingRequestTableRepo : Repository<FundingRequestTable>, IFundingRequestTable
    {
        public FundingRequestTableRepo(DbContext context) : base(context)
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
    public class SystemAccountTableRepo : Repository<SystemAccountTable>, ISystemAccountTable
    {
        public SystemAccountTableRepo(DbContext context) : base(context)
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
