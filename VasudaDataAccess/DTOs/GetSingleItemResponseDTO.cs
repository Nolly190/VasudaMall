using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class GetSingleItemResponseDTO
    {
        public SinglePurchaseItemDTO PurchaseItem { get; set; }
        public SingleShippingItemDTO ShippingItem { get; set; }
        public SinglePurchaseAndShippingItemDTO PurchaseAndShippingItem { get; set; }
        public SingleDomesticItemDTO DomesticItem { get; set; }
    }
}
