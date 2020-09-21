using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class SingleProductItemDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public long Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string DateCreated { get; set; }
    }
}
