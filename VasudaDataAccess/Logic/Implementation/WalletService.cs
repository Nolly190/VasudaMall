using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implementation;
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
        private Notification notification;

        public WalletService(IPaymentService _paymentService)
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
            paymentService = _paymentService;
            notification = new Notification();
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
                var transactions = _unitOfWork.PaymentHistoryTable.GetRecentTransactionsHistory(null, user).ToList();
                Parallel.ForEach(transactions, transaction =>
                {
                    transaction.Purpose = Util.PaymentHistoryEnumConverter(
                                            (PaymentHistoryPurposeEnum)Enum.Parse(typeof(PaymentHistoryPurposeEnum),
                                            transaction.Purpose));
                });
                model.ExchangeRates = _unitOfWork.ExchangeRateTable.GetExchangeRates();
                model.PaymentHistory = transactions;
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
                result.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            result.SetResult(model);
            return result;
        }
        public Response<TransactionViewModel> GetAllTransactionsHomePage(string userId)
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
                var transactions = _unitOfWork.PaymentHistoryTable.GetAll(
                                        x => x.UserId.ToString() == userId)
                                        .OrderByDescending(x => x.DateCreated).ToList();
                Parallel.ForEach(transactions, transaction =>
                {
                    transaction.Purpose = Util.PaymentHistoryEnumConverter(
                                            (PaymentHistoryPurposeEnum)Enum.Parse(typeof(PaymentHistoryPurposeEnum),
                                            transaction.Purpose));
                });
                model.AllTransactions = transactions ;
                result.Status = true;
                result.Message = "Successfully retrieved all transactions";
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
                    response.Message = "Insufficient balance to complete this request";
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

        public Response<string> Action(RequestApprovalDTO model)
        {
            var response = new Response<string>()
            {
                Status = false,
                Message = "Could not retrieve process request"
            };

            try
            {
                
                if (model.Type == "Fund" && model.Action=="approve")
                {
                  response =  ApproveFundingRequest(model);
                }
                else if (model.Type == "Fund" && model.Action == "decline")
                {
                    response = DeclineFundingRequest(model);

                }
                else if (model.Type == "withdrawal" && model.Action == "approve")
                {
                    response = ApproveWithdrawalRequest(model);

                }
                else
                {
                    response = DeclineWithdrawalRequest(model);

                }
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

            var model = new AdminWalletViewModel();
            try
            {
                model.WithdrawalDetailsTables = _unitOfWork.WithdrawalRequestTable.GetAll().ToList();
                model.FundingRequest = _unitOfWork.FundingRequestTable.GetAll().ToList();
                model.PendingWithdrawalRequest = _unitOfWork.WithdrawalRequestTable.GetAll(x=>!x.IsApproved).ToList();
                model.PendingFundingRequest = _unitOfWork.FundingRequestTable.GetAll(x=>x.IsCredited == false && x.PaymentType !="Online").ToList();
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

        public Response<string> ApproveFundingRequest(RequestApprovalDTO model)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getUser = _unitOfWork.FundingRequestTable.Get(x => x.Id.ToString() == model.Id);
                if (getUser== null||getUser?.IsCredited ==true)
                {
                    response.Message = getUser == null? "Could not retrieve request" :"User has already been credited";
                    return response;
                }
                var getExchange = _unitOfWork.ExchangeRateTable.GetSingleRate("Dollar", "Naira");
                if (getExchange == null)
                {
                    response.Message =  "Could not retrieve exchange rate";
                    return response;
                }
                var dollars = Math.Round(getUser.NairaAmount / getExchange.Rate,2);
                getUser.AspNetUser.Balance = getUser.AspNetUser.Balance + dollars;
                getUser.IsCredited = true;
                getUser.IsApproved = true;
                var insertPaymentHistory = new PaymentHistoryTable()
                {
                    UserId = getUser.AspNetUser.Id,
                    Amount = dollars,
                    DateCreated = getUser.DateCreated,
                    Purpose = PaymentHistoryPurposeEnum.WalletFunding.ToString(),
                    Id = Guid.NewGuid(),
                    Status = PaymentHistoryStatus.Completed.ToString(),
                    TransactionType = PaymentHistoryType.Credit.ToString(),
                };
                _unitOfWork.PaymentHistoryTable.Add(insertPaymentHistory);
                _unitOfWork.Complete();
                var mail = new MailDTO()
                {
                    Message =$"Your payment was successful and your account has been credited with ${dollars}. \n Thank you for choosing vasuda mall " ,
                    Email = getUser.AspNetUser.Email,
                    Name = getUser.AspNetUser?.FullName.Split(' ')[0],
                    Subject = "Vasuda Mall Payment Successful"
                };
                notification.SendEmail(mail);
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }
        public Response<string> DeclineFundingRequest(RequestApprovalDTO model)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getUser = _unitOfWork.FundingRequestTable.Get(x => x.Id.ToString() == model.Id);
                if (getUser == null || getUser?.IsCredited == true)
                {
                    response.Message = getUser == null ? "Could not retrieve request" : "User has already been credited";
                    return response;
                }

                getUser.IsCredited = true;
                getUser.IsApproved = false;
                _unitOfWork.Complete();
                var mail = new MailDTO()
                {
                    Message = $"Your payment was not successful. Reason : '{model.Reason}'. Send a mail to us on Info@Vasudamall.com or chat with the admin on your profile if there are issues. \n Thank you for choosing vasuda mall ",
                    Email = getUser.AspNetUser.Email,
                    Name = getUser.AspNetUser?.FullName.Split(' ')[0],
                    Subject = "Vasuda Mall Payment Declined"
                };
                notification.SendEmail(mail);
                response.Status = true;

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }
        public Response<string> ApproveWithdrawalRequest(RequestApprovalDTO model)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getUser = _unitOfWork.WithdrawalRequestTable.Get(x => x.Id.ToString() == model.Id);
                if (getUser == null || getUser?.IsApproved == true)
                {
                    response.Message = getUser == null ? "Could not retrieve request" : "User has already been credited";
                    return response;
                }
                var getRate = _unitOfWork.ExchangeRateTable.GetSingleRate("Dollar", "Naira");
                if (getRate==null)
                {
                    response.Message = "Could not retrieve exchange rate";
                    return response;
                }

                var amount = Math.Round(getUser.Amount * getRate.Rate,2);
                getUser.AspNetUser.Balance = getUser.AspNetUser.Balance - getUser.Amount;
                getUser.IsApproved = true;
                if (getUser.AspNetUser.Balance<0)
                {
                    response.Message = "Insufficient amount";
                    return response;
                }

                var getBank =
                    _unitOfWork.BankTable.Get(x => x.BankName.ToLower() == getUser.WithdrawalDetailsTable.BankName);
                if (getBank==null)
                {
                    response.Message = "Could not retrieve bank details";
                    return response;
                }
                var userModel = new UserPaymentDTO()
                {
                    AccountNumber = getUser.WithdrawalDetailsTable.AccountNumber,
                    AccountName = getUser.WithdrawalDetailsTable.AccountName,
                    Amount = amount,
                    BankCode = getBank.BankCode,
                    Narration = "Wallet"

                };
               var makePayment= paymentService.PayUser(userModel);
               if (makePayment._entity.status=="success" && makePayment._entity.data.is_approved==1 && makePayment._entity.message== "Transfer Queued Successfully")
               {
                   getUser.IsApproved = true;
                   response.Status = true;
                   _unitOfWork.Complete();
                   var mail = new MailDTO()
                   {
                       Message = $"Your withdrawal was successful, a paymment of {amount} has been paid into your account.  \n Thank you for choosing vasuda mall ",
                       Email = getUser.AspNetUser.Email,
                       Name = getUser.AspNetUser?.FullName.Split(' ')[0],
                       Subject = "Vasuda Mall Payment Declined"
                   };
                   notification.SendEmail(mail);
                }

               else
               {
                   response.Message = makePayment._entity.message;
               }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }
        public Response<string> DeclineWithdrawalRequest(RequestApprovalDTO model)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getUser = _unitOfWork.WithdrawalRequestTable.Get(x => x.Id.ToString() == model.Id);
                if (getUser == null || getUser?.IsApproved == true)
                {
                    response.Message = getUser == null ? "Could not retrieve request" : "User has already been credited";
                    return response;
                }

                getUser.IsApproved = false;
                _unitOfWork.Complete();
                var mail = new MailDTO()
                {
                    Message = $"Your payment was not successful. Reason : '{model.Reason}'. Send a mail to us on Info@Vasudamall.com or chat with the admin on your profile if there are issues. \n Thank you for choosing vasuda mall ",
                    Email = getUser.AspNetUser.Email,
                    Name = getUser.AspNetUser?.FullName.Split(' ')[0],
                    Subject = "Vasuda Mall Payment Declined"
                };
                notification.SendEmail(mail);
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<UserDetailsDTO> GetUserInfo(string userId)
        {
            var response = new Response<UserDetailsDTO>();
            response.Status = false;
            try
            {
                var model = new UserDetailsDTO();
                model.User = _unitOfWork.AspNetUser.Get(x=>x.Id==userId);
                model.FundingRequest = _unitOfWork.FundingRequestTable.GetAll(x => x.Userid == userId && x.IsActive).ToList();
                model.WithdrawalRequest = _unitOfWork.WithdrawalRequestTable
                    .GetAll(x => x.UserId == userId && x.IsActive).ToList();
                model.OrderHistory = _unitOfWork.OrderTable.GetAll(x => x.UserId == userId && x.IsActive).ToList();
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
    }  
    
    public enum FundRequestStatus
    {
        Pending,
        Payment_Successful,
        Payment_Error,
        Payment_Credited
    }
}
