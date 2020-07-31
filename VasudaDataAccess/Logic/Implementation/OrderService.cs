using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic.Implementation
{
    public class OrderService : IOrderService
    {

        private UnitOfWork _unitOfWork;
        private Logger logger;

        public OrderService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }

        public Response<OrderHistoryViewModel> GetAllOrdersHomePage()
        {
            var result = new Response<OrderHistoryViewModel>
            {
                Status = false,
                Message = "Could not retrieve info"
            };

            var model = new OrderHistoryViewModel
            {
                AllOrders = new List<OrderTable>()
            };
            try
            {
                model.AllOrders = _unitOfWork.OrderTable.GetAll().ToList();
                result.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            result.SetResult(model);
            return result;
        }

    }
}
