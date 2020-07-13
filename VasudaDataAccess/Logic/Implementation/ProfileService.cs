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
    public class ProfileService : IProfileService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public ProfileService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaDataModel());
        }

        public Response<ProfileViewModel> GetProfileHomePage()
        {
            var result = new Response<ProfileViewModel>
            {
                Status = false,
                Message = "Could not retrieve info"
            };

            var model = new ProfileViewModel
            {
                RecentNotifications = new List<NotificationTable>()
            };
            try
            {
                model.RecentNotifications = _unitOfWork.NotificationTable.GetRecentNotifications(null);
                result.Status = true;
                result.SetResult(model);

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
