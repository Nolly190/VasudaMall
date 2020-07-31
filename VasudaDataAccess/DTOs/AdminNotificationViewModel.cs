using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class AdminNotificationViewModel
    {
        public List<NotificationTable> Notifications { get; set; }
    }
}
