using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class Price
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string DeliveryTime { get; set; }
        public string Country { get; set; }
        public double MinKg { get; set; }
        public double MaxKg { get; set; }
        public decimal PricePerKg { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
