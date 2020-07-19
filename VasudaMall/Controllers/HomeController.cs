using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;

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