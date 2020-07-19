using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;
using VasudaDataAccess.Providers.Implementations;

namespace VasudaDataAccess.Logic.Implementation
{
    public class ProfileService : IProfileService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;
        private PaymentService _paymentService;

        public ProfileService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
            _paymentService = new PaymentService();
        }

        public Response<string> AddWithdrawalAccount(WithdrawalDetailsTable model)
        {
            var response = new Response<string>();
            response.Status = false;
            response.Message = " Could not add account";
            try
            {
               
                var accountName = _paymentService.GetValidAccountName(model.BankName, model.AccountNumber);
                var getBank = _unitOfWork.BankTable.Get(x => x.IsActive && x.BankCode == model.BankName);
                if (!accountName.Status)
                {
                    response.Message = "Account name could not be resolved";
                    return response;
                }

                model.Id = Guid.NewGuid().ToString();
                model.DateCreated = DateTime.UtcNow;
                model.AccountName = accountName._entity;
                model.BankName = getBank?.BankName;
                model.IsActive = true;
                _unitOfWork.WithdrawalDetailsTable.Add(model);
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
            }
            return response;
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
