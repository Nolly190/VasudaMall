﻿using System;
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
}