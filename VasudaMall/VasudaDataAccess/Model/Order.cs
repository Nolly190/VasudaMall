using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class OrderTable
    {
        public Guid Id { get; set; }
        public Guid ProductTypeId { get; set; }
        public string ProductLink { get; set; }
        public Guid VendorId { get; set; }
        public string SellerPhoneNumber { get; set; }
        public decimal UnitPrice { get; set; }
        public long Quantity { get; set; }
        public decimal ItemsPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
    }
}
