using System.Net;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
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
        public Response<string> GetValidAccountName(string bankCode, string accountNumber)
        {
            var response = new Response<string>();
            response.Status = false;
            response.Message = "Could not resolve account";
            var getApiKey = _unitOfWork.SettingTable.GetSystemSetting();
            var paystackSecretkey = Encryption.Decrypt(getApiKey.paystackSecretKey);
            var newModel =JsonConvert.SerializeObject(new { account_number = accountNumber, account_bank = bankCode });
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
    }
}
