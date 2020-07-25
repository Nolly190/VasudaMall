using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;

namespace VasudaDataAccess.Provider
{
    public interface IPaymentService
    {
        Response<string> GetValidAccountName(string bankCode, string accountNumber);
        Response<FlutterPaymentDetails> GetFlutterwavePaymentInfo (string userId, decimal amount);
        Response<string> UpdatePaymentInfo (string status,string transactionId,string tx_ref);
        Response<string> VerifyTransaction (string paymentId);
    }
}
