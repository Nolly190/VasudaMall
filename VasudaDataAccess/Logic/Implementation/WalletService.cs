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
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }

        public Response<WalletViewModel> GetWalletHomePage(string user)
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
            model.SystemAccounts = new List<SystemAccountTable>();
            try
            {
                model.ExchangeRates = _unitOfWork.ExchangeRateTable.GetExchangeRates();
                model.PaymentHistory = _unitOfWork.PaymentHistoryTable.GetRecentTransactionsHistory(null);
                model.WithdrawalAccounts = _unitOfWork.WithdrawalDetailsTable.GetAll(x=>x.UserId==user && x.IsActive).ToList();
                model.Banks = _unitOfWork.BankTable.GetAllActiveBanks();
                var userInfo = _unitOfWork.AspNetUser.Get(user);
                model.DollarBal = userInfo?.Balance.ToString();
                var getRate = _unitOfWork.ExchangeRateTable.GetSingleRate("Dollar", "Yuan");
                model.YuanBal = getRate!=null? (Math.Round((userInfo.Balance*getRate.Rate),2)).ToString() :"0";
                getRate = _unitOfWork.ExchangeRateTable.GetSingleRate("Dollar", "Naira");
                model.DollarToNairaRate = getRate != null ? getRate.Rate.ToString() : "0";
                model.NairaBal = getRate != null ? (Math.Round((userInfo.Balance * getRate.Rate), 2)).ToString() : "0";
                model.SystemAccounts = _unitOfWork.SystemAccountTable.GetAll(x=>x.IsActive).ToList();
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

        public Response<string> WithdrawalRequest(WithdrawalRequestTable model)
        {
            var response = new Response<string>()
            {
                Message = "Could not add request",
                Status = false,
            };
            try
            {
                var getBalance = _unitOfWork.AspNetUser.Get(model.UserId);
                if (getBalance==null)
                {
                    response.Message = "Could not retrieve user balance";
                    return response;
                }

                if (getBalance.Balance < model.Amount)
                {
                    response.Message = "Amount is more than your balance";
                    return response;
                }

                getBalance.Balance = getBalance.Balance - model.Amount;
                model.Status = FundWithdrawalStatus.Pending.ToString();
                model.DateCreated = DateTime.UtcNow.AddHours(1);
                model.Id = Guid.NewGuid().ToString();
                model.IsActive =true;
                _unitOfWork.WithdrawalRequestTable.Add(model);
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }
    }

    public enum FundWithdrawalStatus
    {
        Pending,
        Approved,
        Declined,
    }
}
