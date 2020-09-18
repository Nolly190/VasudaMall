using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;
using VasudaMall.Models;

namespace VasudaMall.Utilities
{
    public class UtilityClass
    {
        private Logger logger;
        private UnitOfWork _unitOfWork;

        public UserManager<ApplicationUser> UserManager { get; set; }

        public UtilityClass()
        {
            var conn = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(conn);
            UserManager = new UserManager<ApplicationUser>(userStore);
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }
        public List<string> GetRole(string userId)
        {
            var userRole = UserManager.GetRoles(userId);
            return userRole.ToList();
        }
        public Response<string> ManageAdmin(string userId, bool action)
        {
            var response = new Response<string>()
            {
                Status = false
            };
            try
            {
                var result = action ? UserManager.AddToRole(userId, "Admin") : UserManager.RemoveFromRole(userId, "Admin");
                response.Status = result.Succeeded;
            }
            catch (Exception e)
            {
                response.Message = action ? "Could not add admin" : "Could not remove admin";
                logger.Error(e.ToString());
            }

            return response;
        }  
        public int AdminChatCount()
        {
            return _unitOfWork.SupportTable.GetAll(x => x.SentBy == "User" && !x.IsRead).Count();
        }
        public int UserChatCount(string userId)
        {
            return _unitOfWork.SupportTable.GetAll(x => x.SentBy == "Admin" && !x.IsRead && x.UserId==userId).Count();
        }
    }

}