using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class FlutterwaveResponse<TEntity>
    {
        public string status { get; set; }
            public string message { get; set; }
            public TEntity data { get; set; }
    }

    public class Data
    {
        public int id { get; set; }
        public string tx_ref { get; set; }
        [JsonProperty("flw_ref")]
        public string FlwRef { get; set; }
        public string device_fingerprint { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public int charged_amount { get; set; }
        public int app_fee { get; set; }
        public int merchant_fee { get; set; }
        public string processor_response { get; set; }
        public string auth_model { get; set; }
        public string ip { get; set; }
        public string narration { get; set; }
        public string status { get; set; }
        public string payment_type { get; set; }
        public DateTime created_at { get; set; }
        public int account_id { get; set; }
        public int amount_settled { get; set; }


        public string account_number { get; set; }
        public string bank_code { get; set; }
        public string full_name { get; set; }
        public string debit_currency { get; set; }
        public double fee { get; set; }
        public string reference { get; set; }
        public object meta { get; set; }
        public string complete_message { get; set; }
        public int requires_approval { get; set; }
        public int is_approved { get; set; }
        public string bank_name { get; set; }

        public string account_name { get; set; }

    }
    public class Card
    {
        public string first_6digits { get; set; }
        public string last_4digits { get; set; }
        public string issuer { get; set; }
        public string country { get; set; }
        public string type { get; set; }
        public string token { get; set; }
        public string expiry { get; set; }

    }

    public class VerifyPayment <TEntity>
    {
        public int id { get; set; }
        public string tx_ref { get; set; }
        public string flw_ref { get; set; }
        public string device_fingerprint { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public int charged_amount { get; set; }
        public double app_fee { get; set; }
        public int merchant_fee { get; set; }
        public string processor_response { get; set; }
        public string auth_model { get; set; }
        public string ip { get; set; }
        public string narration { get; set; }
        public string status { get; set; }
        public string payment_type { get; set; }
        public DateTime created_at { get; set; }
        public int account_id { get; set; }
        public TEntity card { get; set; }
        public double amount_settled { get; set; }

    }

    public class TransferData
    {
        public int id { get; set; }
        public string account_number { get; set; }
        public string bank_code { get; set; }
        public string full_name { get; set; }
        public DateTime created_at { get; set; }
        public string currency { get; set; }
        public string debit_currency { get; set; }
        public decimal amount { get; set; }
        public decimal fee { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public string meta { get; set; }
        public string narration { get; set; }
        public string complete_message { get; set; }
        public int requires_approval { get; set; }
        public int is_approved { get; set; }
        public string bank_name { get; set; }
    }
}
