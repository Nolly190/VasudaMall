using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;

namespace VasudaDataAccess.Logic
{
  public interface IOrderService
    {
        Response<OrderHistoryViewModel> GetAllOrdersHomePage();
    }
}
