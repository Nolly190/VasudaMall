using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic
{
    public interface IWalletService
    {
        Response<WalletViewModel> GetWalletHomePage(string userId);
        Response<TransactionViewModel> GetAllTransactionsHomePage();
        Response<string> WithdrawalRequest(WithdrawalRequestTable model);
        Response<FlutterPaymentDetails> FundingRequest(FundingRequestTable model);
        Response<AdminWalletViewModel> AdminGetAllWithdrawalAccounts();
        Response<string> Action(RequestApprovalDTO model);
    }
}
