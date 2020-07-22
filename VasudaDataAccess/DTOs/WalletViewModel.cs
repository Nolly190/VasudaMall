using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class WalletViewModel
    {
        public List<WithdrawalDetailsTable> WithdrawalAccounts { get; set; }
        public List<ExchangeRateTable> ExchangeRates { get; set; }
        public List<PaymentHistoryTable> PaymentHistory { get; set; }
        public List<BankTable> Banks { get; set; }
        public List<SystemAccountTable> SystemAccounts { get; set; }
        public string NairaBal { get; set; }
        public string YuanBal { get; set; }
        public string DollarBal { get; set; }
        public string DollarToNairaRate { get; set; }
    }
}