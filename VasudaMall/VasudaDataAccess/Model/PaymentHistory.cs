using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class PaymentHistory
    {

        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Purpose { get; set; }
        public DateTime DateCreated { get; set; }
    }

   public enum TransactionType
    {
        Credit,
        Debit
    }
}
