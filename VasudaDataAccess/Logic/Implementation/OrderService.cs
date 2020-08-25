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

        public Response<AdminOrderDto> GetAllOrderInfo()
        {
            var response = new Response<AdminOrderDto>();
            try
            {
                var model = new AdminOrderDto();
                model.DomesticOrder = new List<ItemsTable>() ;
                model.UnfinishedOrders = _unitOfWork.OrderTable.GetAdminOrders();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
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

        public Response<List<ItemsTable>> GetOrderItems(string orderId)
        {
            var response = new Response<List<ItemsTable>>();
            response.Status = false;
            try
            {
                var getOrder = _unitOfWork.OrderTable.Get(x => x.Id == Guid.Parse(orderId));
                if (getOrder == null)
                {
                    response.Message = "Could not retrieve order";
                    return response;
                }
                response.SetResult(getOrder.ItemsTables.ToList());
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = "Could not retrieve items";
                logger.Error(ex.ToString());
            }
            return response;
        }
    }
}
