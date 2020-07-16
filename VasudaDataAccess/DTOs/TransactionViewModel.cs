using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class TransactionViewModel
    {
        public List<PaymentHistoryTable> AllTransactions { get; set; }
    }
}