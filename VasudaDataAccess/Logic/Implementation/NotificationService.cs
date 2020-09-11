using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implementation;
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
            var response = new Response<string>();
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

        public Response<AdminNotificationViewModel> AdminGetAllNotifications()
        {
            var response = new Response<AdminNotificationViewModel>()
            {
                Status = false,
                Message = "Could not retrieve notifications"
            };

            var model = new AdminNotificationViewModel
            {
                Notifications = new List<NotificationTable>()
            };
            try
            {
                model.Notifications = _unitOfWork.NotificationTable.GetAll().ToList();
                response.Message = "Successfully retrieved all notifications";
                response.Status = true;
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());

            }
            return response;
        }

        public Response<string> SendMail(MailDTO model)
        {
            var response = new Response<string>()
            {
                Status = false,
                Message = "Could not send mail"
            };
            try
            {
                string mailReport = "";
                var emails = model.Email.Split(',');
                foreach (var item in emails)
                {
                    model.Email = item.ToLower();
                    var user = _unitOfWork.AspNetUser.Get(x => x.Email.ToLower() == model.Email);
                    model.Name = user?.FullName.Split(' ')[0];
                    var result = new Notification().SendEmail(model);
                    if (!result)
                    {
                        mailReport = String.Concat(mailReport, item);

                    }
                    else
                    {
                        var insertNotification = new NotificationTable()
                        {
                            DateCreated = DateTime.UtcNow.AddHours(1),
                            UserId = user.Id,
                            IsRead = false,
                            Message = model.Message,
                            Subject = model.Subject,
                            Id = Guid.NewGuid().ToString(),
                        };
                        _unitOfWork.NotificationTable.Add(insertNotification);
                    }
                }
                _unitOfWork.Complete();
                response.Status = true;
                response.SetResult(mailReport);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<List<SupportTable>> GetAllChats(string userId, string sentBy)
        {
            var response = new Response<List<SupportTable>>()
            {
                Status = false
            };
            try
            {
                _unitOfWork._dbContext.Configuration.ProxyCreationEnabled = false;
                var result = string.IsNullOrEmpty(userId) ? _unitOfWork.SupportTable.GetAll().OrderBy(x=>x.DateCreated).ToList(): _unitOfWork.SupportTable.GetAll(x=>x.UserId==userId).OrderBy(x => x.DateCreated).ToList();
                if (!string.IsNullOrEmpty(userId))
                {
                    var getRead = _unitOfWork.SupportTable.GetAll(x => x.UserId == userId && !x.IsRead && x.SentBy==sentBy).ToList();
                    foreach (var item in getRead)
                    {
                        item.IsRead = true;

                    }
                    _unitOfWork.Complete();
                }
                response.Status = true;
                response.SetResult(result);
            }
            catch (Exception ex)
            {
                response.SetResult(new List<SupportTable>());
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<string> SendChats(string userId, string message , string sentBy)
        {
            var response = new Response<string>()
            {
                Status = false
            };
            try
            {
                var sendChat = new SupportTable()
                {
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    Id = Guid.NewGuid(),
                    Message = message,
                    IsRead = false,
                    SentBy = sentBy,
                    UserId = userId,
                };
                _unitOfWork.SupportTable.Add(sendChat);
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not add chat";
            }
            return response;
        }

        public Response<NotificationTable> GetNotification(string id)
        {

            var response = new Response<NotificationTable>()
            {
                Status = false
            };
            try
            {
                _unitOfWork._dbContext.Configuration.ProxyCreationEnabled = false;
                var getNotification = _unitOfWork.NotificationTable.Get(x => x.Id == id);
                response.SetResult(getNotification);
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not retrieve notification";
            }
            return response;
        
        }
    }
}
