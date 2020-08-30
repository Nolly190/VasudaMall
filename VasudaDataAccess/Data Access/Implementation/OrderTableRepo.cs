using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implementation
{
    public class OrderTableRepo : Repository<OrderTable>, IOrderTable
    {
        

        public OrderTableRepo(DbContext context) : base(context)
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
  public class WithdrawalRequestTableRepo : Repository<WithdrawalRequestTable>, IWithdrawalRequestTable
    {
        public WithdrawalRequestTableRepo(DbContext context) : base(context)
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

    public enum OrderStatus
    {
        AwaitingPayment=1, 
    }
}
