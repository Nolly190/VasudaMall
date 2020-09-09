using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class SingleShippingItemDTO
    {
        public string SenderName { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverNumber { get; set; }
        public string ReceiverAddress { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public long Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string DateCreated { get; set; }
    }
}
