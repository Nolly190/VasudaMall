using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;
using VasudaMall.Models;

namespace VasudaMall.Controllers
{
    public class 
        HomeController : Controller
    {
        private IStoreService _storeService;
        private UnitOfWork _unitOfWork;
        private INotificationService _notificationService;

        public HomeController(IStoreService storeService)
        {
            _storeService = storeService;
            _unitOfWork = new UnitOfWork(new VasudaModel());
            _notificationService = new NotificationService(_unitOfWork);
        }
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            //var roleStore = new RoleStore<IdentityRole>(context);
            //var roleManager = new RoleManager<IdentityRole>(roleStore);
            
            var userStore = new UserStore<ApplicationUser>(context);
            var UserManager =new UserManager<ApplicationUser>(userStore);
            try
            {
                if (User?.Identity?.GetUserId() != null &&
                    (UserManager.IsInRoleAsync(User?.Identity?.GetUserId(), "Admin").Result ||
                     UserManager.IsInRoleAsync(User?.Identity?.GetUserId(), "SuperAdmin").Result))
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch (Exception ex)
            {
            }
 
           //var idtest = roleManager.CreateAsync(new IdentityRole("SuperAdmin")).Result;
            //var idtest1 = roleManager.CreateAsync(new IdentityRole("Admin")).Result;
            // UserManager.AddToRoleAsync("9af7a9d5-f929-4ff1-a3b7-5403103d44ec", "SuperAdmin");

            return View(_storeService.GetHomePage().Result());
        }

        public JsonResult AddContact(ContactTable model)
        {
            return Json(_notificationService.AddContactUs(model),JsonRequestBehavior.AllowGet);
        }

        public ActionResult Services()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Portfolio()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Terms()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult FAQ()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Teams()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}