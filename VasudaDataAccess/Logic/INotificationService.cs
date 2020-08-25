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
        Response<NotificationViewModel> GetAllNotificationsHomePage();
        Response<string> AddContactUs(ContactTable model);
        Response<AdminNotificationViewModel> AdminGetAllNotifications();
        Response<string> SendMail(MailDTO model);

    }
}
