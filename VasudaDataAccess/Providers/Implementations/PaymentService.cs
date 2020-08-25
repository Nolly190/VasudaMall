using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;
using VasudaDataAccess.Provider;
using VasudaDataAccess.Utility;

namespace VasudaDataAccess.Providers.Implementations
{
    public class PaymentService : IPaymentService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public PaymentService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }

        public Response<FlutterPaymentDetails> GetFlutterwavePaymentInfo(string userId, decimal amount)
        {
            var response = new Response<FlutterPaymentDetails>();
            response.Status = false;
            try
            {
                var getUserDetails = _unitOfWork.AspNetUser.Get(userId);
                if (getUserDetails == null)
                {
                    response.Message = "Could not retrieve user info";
                    return response;
                }
                var result = new FlutterPaymentDetails()
                {
                    Amount = amount,
                    ApiKey = Encryption.Decrypt(_unitOfWork.SettingTable.GetSystemSetting().paystackPublicKey),
                    Email = getUserDetails.Email,
                    Name = getUserDetails.FullName,
                    Phone = getUserDetails.PhoneNumber,
                    PaymentId = Guid.NewGuid().ToString()
                };
                response.Status = true;
                response.SetResult(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<string> GetValidAccountName(string bankCode, string accountNumber)
        {
            var response = new Response<string>();
            response.Status = false;
            response.Message = "Could not resolve account";
            var getApiKey = _unitOfWork.SettingTable.GetSystemSetting();
            var paystackSecretkey = Encryption.Decrypt(getApiKey.paystackSecretKey);
            var newModel = JsonConvert.SerializeObject(new { account_number = accountNumber, account_bank = bankCode });
            var Url = "https://api.flutterwave.com/v3/";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
            var client = new RestClient(Url);
            var request = new RestRequest("accounts/resolve", Method.POST);
            request.AddHeader("authorization", "Bearer " + "FLWSECK-d105e19714ce12c81f6834c416810344-X");
            // request.AddHeader("authorization", "Bearer " + paystackSecretkey);
            request.AddParameter("application/json; charset=utf-8", newModel, ParameterType.RequestBody);
            var responseStr = client.Execute(request);
            var flutterResponse = JsonConvert.DeserializeObject<FlutterwaveResponse<Data>>(responseStr.Content);
            response.Message = flutterResponse.message;

            if (flutterResponse.status == "success" && flutterResponse.message == "Account details fetched")
            {
                response.SetResult(flutterResponse.data.account_name);
                response.Status = true;
                return response;
            }



            logger.Error("Paystack Account verification Error: " + responseStr);
            response.SetResult("false");
            return response;
        }

        public Response<string> UpdatePaymentInfo(string status, string transactionId, string tx_ref)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getPaymentInfo =
                    _unitOfWork.FundingRequestTable.Get(x => x.PaymentId.ToLower() == tx_ref.ToLower());
                if (getPaymentInfo != null && getPaymentInfo.IsCredited == false)
                {
                    getPaymentInfo.TransId = transactionId;
                    getPaymentInfo.PaymentStatus = status == "successful" ? FundRequestStatus.Payment_Successful.ToString() : FundRequestStatus.Payment_Error.ToString();
                    _unitOfWork.Complete();
                    var result = VerifyTransaction(getPaymentInfo.PaymentId);
                    if (result.Status)
                    {
                        var getUserDetails = _unitOfWork.AspNetUser.Get(getPaymentInfo.Userid);
                        var amount = Convert.ToDecimal((getPaymentInfo.NairaAmount / getPaymentInfo.Rate.Value).ToString("##.##"));
                        getUserDetails.Balance = getUserDetails.Balance + amount;
                        getPaymentInfo.IsCredited = true;
                        getPaymentInfo.IsApproved = true;
                        AddPaymentHistory("Credit", amount, getUserDetails.Id, "Wallet funding", "Payment Completed");
                        var mailModel = new Notification();
                        var model = new MailDTO()
                        {
                            Email = getUserDetails.Email,
                            Message = $"Your payment was successfully received and your wallet has been credited.",
                            Name = getUserDetails.FullName.Split(' ')[0],
                            Subject = "Vasuda Mall payment received",
                        };
                        _unitOfWork.Complete();
                        mailModel.SendEmail(model);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not update the transaction";
            }
            return response;
        }

        public Response<string> AddPaymentHistory(string transType, decimal amount, string userId, string purpose, string status)
        {
            var response = new Response<string>();
            response.Status = false;
            response.Message = "Could add payment history";
            try
            {
                var insertPayment = new PaymentHistoryTable()
                {
                    Id = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    TransactionType = transType,
                    Amount = amount,
                    UserId = userId,
                    Purpose = purpose,
                    Status = status
                };
                _unitOfWork.PaymentHistoryTable.Add(insertPayment);
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }
        public Response<string> VerifyTransaction(string paymentId)
        {
            var response = new Response<string>();
            response.Status = false;
            response.Message = "Could not verify transaction";
            try
            {
                var getPaymentInfo =
                    _unitOfWork.FundingRequestTable.Get(x => x.PaymentId.ToLower() == paymentId.ToLower());
                if (getPaymentInfo != null)
                {
                    var secretKey = Encryption.Decrypt(_unitOfWork.SettingTable.GetSystemSetting().paystackSecretKey);
                    var paymentVerification = FlutterwaveVerification(secretKey, getPaymentInfo.TransId);
                    if (paymentVerification.Status && getPaymentInfo.NairaAmount == paymentVerification._entity.data.amount && paymentVerification._entity.data.processor_response.Contains("successful"))
                    {
                        response.Status = true;

                    }
                    else
                    {
                        response.Status = false;
                        response.Message = paymentVerification._entity.message;

                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not update the transaction";
            }
            return response;
        }
        public Response<FlutterwaveResponse<VerifyPayment<Card>>> FlutterwaveVerification(string secretKey, string transId)
        {
            var response = new Response<FlutterwaveResponse<VerifyPayment<Card>>>();
            response.Status = false;
            try
            {
                var setting = Encryption.Decrypt(_unitOfWork.SettingTable.GetSystemSetting().paystackSecretKey);
                var Url = $"https://api.flutterwave.com/";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                var client = new RestClient(Url);
                var request = new RestRequest($"v3/transactions/{transId}/verify", Method.GET);
                request.AddHeader("authorization", "Bearer " + setting);
                var responseStr = client.Execute(request);
                var FlutterwaveVerifyResponse = JsonConvert.DeserializeObject<FlutterwaveResponse<VerifyPayment<Card>>>(responseStr.Content);
                response.Status = true;
                response.SetResult(FlutterwaveVerifyResponse);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not verify transaction";
            }
            return response;
        }

        public Response<FlutterwaveResponse<TransferData>> PayUser(UserPaymentDTO model)
        {
            var response = new Response<FlutterwaveResponse<TransferData>>();
            response.Status = false;
            try
            {
                var newModel = JsonConvert.SerializeObject(new
                {
                    account_bank = model.BankCode,
                    account_number = model.AccountNumber,
                    amount = model.Amount,
                    beneficiary_name = model.AccountName,
                    currency = "NGN",
                    narration = model.Narration
                });
                var setting = Encryption.Decrypt(_unitOfWork.SettingTable.GetSystemSetting().paystackSecretKey);
                var Url = $"https://api.flutterwave.com/";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                var client = new RestClient(Url);
                var request = new RestRequest($"v3/transfers", Method.POST);
                request.AddHeader("authorization", "Bearer " + setting);
                request.AddParameter("application/json; charset=utf-8", newModel, ParameterType.RequestBody);
                var responseStr = client.Execute(request);
                var FlutterwaveVerifyResponse = JsonConvert.DeserializeObject<FlutterwaveResponse<TransferData>>(responseStr.Content);
                response.Status = true;
                response.SetResult(FlutterwaveVerifyResponse);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not verify transaction";
            }
            return response;
        }

        public Response<List<PaymentHistoryTable>> GetPaymentHistory()
        {
            var response = new Response<List<PaymentHistoryTable>>();
            response.Status = false;
            try
            {
                var getPaymentInfo = _unitOfWork.PaymentHistoryTable.GetAll().ToList();
                response.SetResult(getPaymentInfo);
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }
    }
}
