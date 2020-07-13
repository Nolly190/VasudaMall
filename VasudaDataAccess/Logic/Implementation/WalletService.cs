using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic.Implementation
{
    public class WalletService : IWalletService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public WalletService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaDataModel());
        }

        public Response<WalletViewModel> GetWalletHomePage()
        {
            var result = new Response<WalletViewModel>
            {
                Status = false,
                Message = "Could not retrieve info"
            };

            var model = new WalletViewModel();
            model.WithdrawalAccounts = new List<WithdrawalDetailsTable>();
            model.ExchangeRates = new List<ExchangeRateTable>();
            model.PaymentHistory = new List<PaymentHistoryTable>();
            model.Banks = new List<BankTable>();
            try
            {
                model.ExchangeRates = _unitOfWork.ExchangeRateTable.GetExchangeRates(null);
                model.PaymentHistory = _unitOfWork.PaymentHistoryTable.GetRecentTransactionsHistory(null);
                model.WithdrawalAccounts = _unitOfWork.WithdrawalDetailsTable.GetWithdrawalAccounts(null);
                model.Banks = _unitOfWork.BankTable.GetAllActiveBanks();
                result.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            result.SetResult(model);
            return result;
        }

        public Response<TransactionViewModel> GetAllTransactionsHomePage()
        {
            var result = new Response<TransactionViewModel>
            {
                Status = false,
                Message = "Could not retrieve info"
            };

            var model = new TransactionViewModel();
            model.AllTransactions = new List<PaymentHistoryTable>();
            try
            {
                model.AllTransactions = _unitOfWork.PaymentHistoryTable.GetAll().ToList();
                result.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            result.SetResult(model);
            return result;
        }
        public Response<string> AddWithdrawalAccount()
        {
            throw new NotImplementedException();
        }
    }
}
