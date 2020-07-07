using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.Logic;

namespace VasudaMall.Controllers
{
    public class HomeController : Controller
    {
        private IStoreService _storeService;

        public HomeController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        public ActionResult Index()
        {
            return View(_storeService.GetHomePage().Result());
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