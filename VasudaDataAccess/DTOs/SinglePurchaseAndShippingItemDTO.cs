using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class SinglePurchaseAndShippingItemDTO : SinglePurchaseItemDTO
    {
        public string ReceiverName { get; set; }
        public string ReceiverNumber { get; set; }
        public string ReceiverAddress { get; set; }
    }
}
