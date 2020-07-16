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
    public class NotificationService : INotificationService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public NotificationService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaDataModel());
        }

        public Response<NotificationViewModel> GetAllNotificationsHomePage()
        {
            var result = new Response<NotificationViewModel>
            {
                Status = false,
                Message = "Could not retrieve info"
            };

            var model = new NotificationViewModel
            {
                AllNotifications = new List<NotificationTable>()
            };
            try
            {
                model.AllNotifications = _unitOfWork.NotificationTable.GetAll().ToList();
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
