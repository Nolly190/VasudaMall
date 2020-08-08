using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class UserPaymentDTO
    {
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string AccountName { get; set; }
        public string Narration { get; set; }
    }
}
