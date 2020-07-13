using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class NotificationViewModel
    {
        public List<NotificationTable> AllNotifications { get; set; }
    }
}