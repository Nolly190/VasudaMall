using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class ExchangeRate
    {
        public Guid Id { get; set; }
        public string RateType { get; set; }
        public decimal Rate { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
