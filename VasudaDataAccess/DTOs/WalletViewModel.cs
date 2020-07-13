﻿using System;
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
    }
}