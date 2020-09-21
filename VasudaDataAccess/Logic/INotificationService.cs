using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;
using VasudaDataAccess.Utility;

namespace VasudaDataAccess.Logic
{
    public interface INotificationService
    {
        Response<NotificationViewModel> GetAllNotificationsHomePage(string userId);
        Response<string> AddContactUs(ContactTable model);
        Response<AdminNotificationViewModel> AdminGetAllNotifications();
        Response<string> SendMail(MailDTO model);

        Response<List<SupportTable>> GetAllChats(string userId, string sentBy);
        Response<string> SendChats(string userId,string message, string sentBy);
        Response<NotificationTable> GetNotification( string id);
        Response<NotificationTable> GetUserNotification( string id, string userId);

    }
}
