using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VasudaMall.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public new ActionResult Profile()
        {
            return View();
        }

        public ActionResult Notification()
        {
            return View();
        }
        
        public ActionResult Transaction()
        {
            return View();
        }
        
        public ActionResult Wallet()
        {
            return View();
        }
    }
}