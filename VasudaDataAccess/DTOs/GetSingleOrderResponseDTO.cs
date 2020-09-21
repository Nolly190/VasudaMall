using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class GetSingleOrderResponseDTO
    {
        public List<SinglePurchaseItemDTO> PurchaseOrders { get; set; }
        public List<SinglePurchaseAndShippingItemDTO> PurchaseAndShippingOrders { get; set; }
        public SingleDomesticItemDTO DomesticOrder { get; set; }
    }
}
