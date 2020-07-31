using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;
using VasudaDataAccess.Provider;
using VasudaDataAccess.Utility;

namespace VasudaDataAccess.Logic.Implementation
{
    public class WalletService : IWalletService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;
        private IPaymentService paymentService;

        public WalletService(IPaymentService _paymentService)
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
            paymentService = _paymentService;
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
                model.IsApproved = false;
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

        public Response<FlutterPaymentDetails> FundingRequest(FundingRequestTable model)
        {
            var response = new Response<FlutterPaymentDetails>()
            {
                Message = "Could not add request",
                Status = false,
            };
            try
            {
                var result = paymentService.GetFlutterwavePaymentInfo(model.Userid, model.NairaAmount);
                if (!result.Status)
                {
                    response.Message = result.Message;
                    return response;
                }
                model.PaymentStatus = FundRequestStatus.Pending.ToString();
                model.DateCreated = DateTime.UtcNow.AddHours(1);
                model.Id = Guid.NewGuid();
                model.IsActive =true;
                model.IsApproved = false;
                model.IsCredited = false;
                model.PaymentId = result._entity.PaymentId;
                model.Rate = _unitOfWork.ExchangeRateTable.GetSingleRate("Dollar", "Naira").Rate;
                _unitOfWork.FundingRequestTable.Add(model);
                _unitOfWork.Complete();
                response.Status = true;
                response.SetResult(result.Result());
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<AdminWalletViewModel> AdminGetAllWithdrawalAccounts()
        {
            var response = new Response<AdminWalletViewModel>()
            {
                Status = false,
                Message = "Could not retrieve withdrawal accounts"
            };

            var model = new AdminWalletViewModel
            {
                AllWithdrawalAccounts = new List<WithdrawalDetailsTable>()
            };
            try
            {
                model.AllWithdrawalAccounts = _unitOfWork.WithdrawalDetailsTable.GetAll().ToList();
                response.Message = "Successfully retrieved all withdrawal accounts";
                response.Status = true;
                response.SetResult(model);
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
    }  public enum FundRequestStatus
    {
        Pending,
        Payment_Successful,
        Payment_Error,
        Payment_Credited
    }
}
