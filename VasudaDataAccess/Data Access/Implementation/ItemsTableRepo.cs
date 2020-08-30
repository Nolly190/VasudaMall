using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implementation
{
    public class ItemsTableRepo : Repository<ItemsTable>, IItemsTable
    {
        public ItemsTableRepo(DbContext context) : base(context)
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

    public enum ItemType
    {
        Domestic = 1, Shipping, Purchase, PurchaseAndShipping, Product
    }

    public enum ItemStatus
    {
        Pending = 1, Completed, Cancelled
    }

    
    
    //<add key = "Purchase" value="Awaiting Purchase,Arrived,Completed,Cancel" />
    //<add key = "Domestic" value=" AwaitingQuotation,AwaitingPrice, AcceptedQuotation,Processing,Shipped,Completed,RejectedQuotation,Cancel" />
}
