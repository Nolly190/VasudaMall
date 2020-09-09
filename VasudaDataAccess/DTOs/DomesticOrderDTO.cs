using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{

    public  class DomesticOrderDTO
    {

        public string id { get; set; }
        public long? Quantity { get; set; }
        public decimal Weight { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverNumber { get; set; }
        public string SenderAddress { get; set; }
        public string SenderName { get; set; }
        public string DateCreated { get; set; }
        public string SenderPhoneNumber { get; set; }
    }
}
