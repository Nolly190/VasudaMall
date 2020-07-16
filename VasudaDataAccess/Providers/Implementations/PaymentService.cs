using Newtonsoft.Json;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;
using VasudaDataAccess.Utility;

namespace VasudaDataAccess.Provider.Implementations
{
    class PaymentService : IPaymentService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public PaymentService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaDataModel());
        }
        public Response<string> GetValidAccountName(string bankCode, string accountNumber)
        {
            var response = new Response<string>();
            var dic = new Dictionary<string, string>();
            var getApiKey = _unitOfWork.SettingTable.GetSystemSetting();
            var paystackSecretkey = Encryption.Decrypt(getApiKey.paystackSecretKey);
            var newModel =
                JsonConvert.SerializeObject(new { account_number = accountNumber, account_bank = bankCode });
            var Url = "https://api.flutterwave.com/v3/";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
            var client = new RestClient(Url);
            var request = new RestRequest("accounts/resolve", Method.GET);
            request.AddHeader("authorization", "Bearer " + paystackSecretkey);
            request.AddParameter("application/json; charset=utf-8", newModel, ParameterType.RequestBody);
            var responseStr = client.Execute(request);
            var flutterResponse = JsonConvert.DeserializeObject<FlutterwaveTransResponse>(responseStr.Content);
            if (flutterResponse.status == "success" && flutterResponse.message == "Account number resolved")
            {
                response.SetResult(flutterResponse.data.account_name);
                return response;
            }



            logger.Error("Paystack Account verification Error: " + responseStr);
            response.SetResult("false");
            return response;
        }
    }
}
