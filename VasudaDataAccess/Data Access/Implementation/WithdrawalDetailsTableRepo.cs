using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implementation
{
    public class WithdrawalDetailsTableRepo : Repository<WithdrawalDetailsTable>, IWithdrawalDetailsTable    
    {
        public WithdrawalDetailsTableRepo(DbContext context) : base(context)
        {

        }

        public DbContext Context
        {
            get
            {
                return dbcontext as DbContext;
            }
        }

        public List<WithdrawalDetailsTable> GetWithdrawalAccounts(int? pageNum)
        {
            return pageNum.HasValue ? dbcontext.Set<WithdrawalDetailsTable>().Where(x => x.IsActive == true).Skip(pageNum.Value * 10).Take(10).ToList() : dbcontext.Set<WithdrawalDetailsTable>().Where(x => x.IsActive == true).Take(10).ToList();
        }
    }
}
