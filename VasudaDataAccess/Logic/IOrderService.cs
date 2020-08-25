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
        Response<OrderHistoryViewModel> GetAllOrdersHomePage();
        Response<List<ItemsTable>> GetOrderItems(string orderId);
        Response<AdminOrderDto> GetAllOrderInfo();
    }
}
