using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class AdminWalletViewModel
    {
        public List<WithdrawalRequestTable> WithdrawalDetailsTables { get; set; }
        public List<FundingRequestTable> FundingRequest { get; set; }
        public List<WithdrawalRequestTable> PendingWithdrawalRequest  { get; set; }
        public List<FundingRequestTable> PendingFundingRequest { get; set; }
    }
}
