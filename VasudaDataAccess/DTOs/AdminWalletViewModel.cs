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
        public List<WithdrawalDetailsTable> AllWithdrawalAccounts { get; set; }
        //public List<BankTable> AllWithdrawalAccount { get; set; }
    }
}
