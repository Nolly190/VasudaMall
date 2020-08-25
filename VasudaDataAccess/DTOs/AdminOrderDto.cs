using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
  public  class AdminOrderDto
    {
        public List<ItemsTable> DomesticOrder { get; set; }
        public List<SingleAdminOrder> UnfinishedOrders { get; set; }
        public List<OrderTable> FinishedOrders { get; set; }
    }
  public  class SingleAdminOrder
    {

        public System.Guid Id { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string OrderType { get; set; }
        public Nullable<decimal> TotalServiceCharge { get; set; }
        public Nullable<decimal> ShippingFee { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemsTable> ItemsTables { get; set; }
        public string NextAction { get; set; }
    }
}
