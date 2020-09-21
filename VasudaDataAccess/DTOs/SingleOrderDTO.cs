using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class SingleOrderDTO
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal? TotalServiceCharge { get; set; }
        public decimal? TotalPrice { get; set; }
        public string DateCreated { get; set; }
    }
    public class ItemDetailsDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string OrderType { get; set; }
        public long? Quantity { get; set; }
        public decimal? ItemsPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? ServicePrice { get; set; }
        public Guid Id { get; set; }

        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierAddress { get; set; }
    }
     public class OrderItemDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public SingleOrderDTO Order { get; set; }
        public List<ItemDetailsDTO> Item { get; set; }
    }
}
