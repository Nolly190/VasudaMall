using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class ProfileViewModel
    {
        public AspNetUser UserInfo { get; set; }
        public List<NotificationTable> RecentNotifications { get; set; }
    }
    public class StoreViewModel
    {
        public List<CategoryTable> Categories { get; set; }
        public List<ProductTable> Products { get; set; }
    }
}