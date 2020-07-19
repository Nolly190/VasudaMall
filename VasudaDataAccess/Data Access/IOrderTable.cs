using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access
{
    public interface IOrderTable : IRepository<OrderTable>
    {
    }
    public interface IAspNetUser : IRepository<AspNetUser>
    {
    }
    public interface IWithdrawalRequestTable : IRepository<WithdrawalRequestTable>
    {
    }
}
