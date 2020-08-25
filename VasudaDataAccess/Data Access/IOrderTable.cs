using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access
{
    public interface IOrderTable : IRepository<OrderTable>
    {
        List<SingleAdminOrder> GetAdminOrders();
    }
    public interface IWithdrawalRequestTable : IRepository<WithdrawalRequestTable>
    {
    }
    public interface ISystemAccountTable : IRepository<SystemAccountTable>
    {
    }
    public interface IFundingRequestTable : IRepository<FundingRequestTable>
    {
    }
}
