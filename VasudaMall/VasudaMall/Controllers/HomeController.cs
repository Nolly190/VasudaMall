using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VasudaMall.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        //<a href = "@Url.Action("Services", "Home")"><li><i class="fas fa-cog" aria-hidden="true"></i> &nbsp;  &nbsp; Services</li></a>
        //<a href = "@Url.Action("Portfolio", "Home")"><li><i class="fas fa-clipboard" aria-hidden="true"></i> &nbsp;  &nbsp; Portfolio</li></a>
        //<a href = "@Url.Action("Teams", "Home")"><li><i class="fas fa-users" aria-hidden="true"></i> &nbsp;  &nbsp; Team</li></a>


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