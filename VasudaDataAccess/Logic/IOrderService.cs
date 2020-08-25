using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic
{
  public interface IOrderService
    {
        Response<OrderHistoryViewModel> GetAllOrdersHomePage(string userId);
        Response<DomesticItemViewModel> GetDomesticItemsPage(string userId);
        Response<GeneralItemViewModel> GetGeneralItemsPage(string userId);
        Response<OrderCheckoutViewModel> GetAllItemsPage(string userId);
        Response<string> AddDomesticItem(AddDomesticItemViewModel model, string userId);
        Response<string> AddShippingItem(AddShippingItemViewModel model, string userId);
        Response<string> AddPurchaseItem(AddPurchaseItemViewModel model, string userId);
        Response<string> AddShippingAndPurchaseItem(AddPurchaseAndShippingItemViewModel model, string userId);
        Response<string> DeleteItem(string id, string userId);
        //Response<GetSinglePurchaseItemResponseDTO> GetSinglePurchaseItem(ItemsTable itemsTable, string message, bool status);
        Response<GetSingleItemResponseDTO> GetSingleItem(string id, string userId);
    }
}
