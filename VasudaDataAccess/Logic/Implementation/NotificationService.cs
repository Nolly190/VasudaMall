using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;
using VasudaDataAccess.Utility;

namespace VasudaDataAccess.Logic.Implementation
{
    public class NotificationService : INotificationService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public NotificationService(UnitOfWork unitOfWork)
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = unitOfWork;
        }

        public Response<string> AddContactUs(ContactTable model)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                model.Id = Guid.NewGuid().ToString();
                model.DateAdded = DateTime.UtcNow.AddHours(1);
                _unitOfWork.ContactTable.Add(model);
                _unitOfWork.Complete();
                var mailMessage = Notification.getTemplate($"A customer with the following details contacted you. Name: {model.Fullname} \n Address: {model.Address} \n Phone: {model.Phone}  Message: {model.Message}", "Dear Admin");
                var mailModel = new MailDTO()
                {
                    Email = "info@vasudamall.com",
                    Message = mailMessage,
                    Subject = "Contact Us Filled",
                };
                var newMail = new Notification();
                newMail.SendEmail(mailModel);
                response.Status = true;
                response.Message = "Successful";
                return response;

            }
            catch (Exception e)
            {
                response.Message = "Error occured";
               logger.Error(e.ToString());
            }

            return response;
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

        public Response<string> ResolveAccount(WithdrawalDetailsTable model)
        {
            var  response = new Response<string>();
            response.Status = false;
            response.Message = "Could not resolve account";
            try
            {

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }
    }
}
