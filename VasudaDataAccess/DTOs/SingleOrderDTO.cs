using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class SingleOrderDTO
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalServiceCharge { get; set; }
        public decimal TotalPrice { get; set; }
        public string DateCreated { get; set; }
    }
}
