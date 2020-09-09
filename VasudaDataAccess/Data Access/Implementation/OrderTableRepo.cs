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
                return dbcontext;
            }
        }
    }

    public enum PurchaseOrderStatus
    {
        AwaitingPurchase = 1, Arrived, Completed
    }
    
    public enum PurchaseAndShippingOrderStatus
    {
        AwaitingPurchase = 1, AwaitingShippingQuotation, AwaitingShippingPayment, AwaitingArrival, Completed
    }

    public enum DomesticOrderStatus
    {
        AwaitingQuotation = 1, AwaitingUserAcceptance,
        Processing, AwaitingShippingPayment, AwaitingArrival, Completed, RejectedQuotation
    }
}
