using System;
using VasudaDataAccess.Data_Access.Implementation;

namespace VasudaDataAccess.Utility
{
    public class Util
    {
      
        public static bool ValidateUrl(string urlName)
        {
            return Uri.TryCreate(urlName, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static string FormatDate(DateTime date)
        {
            return $"{date.Day} {GetMonthName(date.Month)}, {date.Year}";
        }
        
        public static string FormatNullableDate(DateTime? date)
        {
            return $"{date.Value.Day} {GetMonthName(date.Value.Month)}, {date.Value.Year}";
        }

        public static string ReduceDescriptionLength(string description)
        {
            string des;
            if (string.IsNullOrEmpty(description))
            {
                des = "N/A";
            }
            else
            {
                if (description.Length < 50)
                {
                    des = description ;
                }
                else
                {
                    des = $"{description.Substring(0, 50)} ....";
                }
            }
            return des;
        }

        public static string GetMonthName(int index)
        {
            string[] months = new string[]
            {
                "January","February","March","April","May","June","July","August","September","October","November","December"
            };

            var name = months[index - 1];
            return name;
        }

        public static string DomesticStatusEnumConverter(DomesticOrderStatus status)
        {
            switch (status)
            {
                case DomesticOrderStatus.AwaitingQuotation:
                    return "Awaiting Quotation";
                case DomesticOrderStatus.AwaitingUserAcceptance:
                    return "Awaiting User Acceptance";
                case DomesticOrderStatus.AwaitingShippingPayment:
                    return "Awaiting Shipping Payment";
                case DomesticOrderStatus.RejectedQuotation:
                    return "Rejected Quotation";
                case DomesticOrderStatus.Processing:
                    return "Processing"; 
                case DomesticOrderStatus.AwaitingArrival:
                    return "Awaiting Arrival";                 
                default:
                    return "Completed";
            }
        }

        public static string PaymentHistoryEnumConverter(PaymentHistoryPurposeEnum purpose)
        {
            switch (purpose)
            {
                case PaymentHistoryPurposeEnum.WalletFunding:
                    return "Wallet Funding";
                case PaymentHistoryPurposeEnum.DomesticOrderProcessing:
                    return "Domestic Order Processing";
                case PaymentHistoryPurposeEnum.DomesticOrderShipping:
                    return "Domestic Order Shipping";
                case PaymentHistoryPurposeEnum.PurchaseOrder:
                    return "Purchase Order";
                case PaymentHistoryPurposeEnum.PurchaseAndShippingOrderProcessing:
                    return "Purchase And Shipping Order Processing";
                case PaymentHistoryPurposeEnum.ProductOrder:
                    return "Product Order";
                default:
                    return "N/A";
            }
        }

        public static string OrderStatusEnumConverter(string status)
        {
            var word = "N/A";

            if (PurchaseOrderStatus.AwaitingPurchase.ToString() == status)
                word = "Awaiting Purchase";
            else if (PurchaseOrderStatus.Arrived.ToString() == status)
                word = "Arrived";
            else if (PurchaseOrderStatus.Completed.ToString() == status)
                word = "Completed";
            else if (PurchaseAndShippingOrderStatus.AwaitingShippingQuotation.ToString() == status)
                word = "Awaiting Shipping Quotation";
            else if (PurchaseAndShippingOrderStatus.AwaitingShippingPayment.ToString() == status)
                word = "Awaiting Shipping Payment";
            else if (PurchaseAndShippingOrderStatus.AwaitingArrival.ToString() == status)
                word = "Awaiting Arrival";
            else if (DomesticOrderStatus.AwaitingQuotation.ToString() == status)
                word = "Awaiting Quotation";
            else if (DomesticOrderStatus.AwaitingUserAcceptance.ToString() == status)
                word = "Awaiting User Acceptance";
            else if (DomesticOrderStatus.RejectedQuotation.ToString() == status)
                word = "Rejected Quotation";
            else if (DomesticOrderStatus.Processing.ToString() == status)
                word = "Processing";

            return word;
        }
    }
}
