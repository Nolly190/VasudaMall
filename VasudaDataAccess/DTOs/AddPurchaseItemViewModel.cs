using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class AddPurchaseItemViewModel
    {
        [Required(ErrorMessage = "Vendor name is required")]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Product link is required")]
        public string ProductLink { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public long Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        public decimal UnitPrice { get; set; }
    }
}
