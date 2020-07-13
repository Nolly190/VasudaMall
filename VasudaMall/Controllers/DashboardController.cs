using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Model;

namespace VasudaMall.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private IWalletService _walletService;
        private IProfileService _profileService;
        private INotificationService _notificationService;
        private IOrderService _orderService;

        public DashboardController(IWalletService walletService, IProfileService profileService, 
                                   INotificationService notificationService, IOrderService orderService)
        {
            _walletService = walletService;
            _profileService = profileService;
            _notificationService = notificationService;
            _orderService = orderService;
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddAccount(WithdrawalDetailsTable model)
        {
            if (true)
            {

            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public new ActionResult Profile()
        {
            return View(_profileService.GetProfileHomePage().Result());
        }

        public ActionResult Notification()
        {
            return View(_notificationService.GetAllNotificationsHomePage().Result());
        }
        
        public ActionResult Transaction()
        {
            return View(_walletService.GetAllTransactionsHomePage().Result());
        }
        
        public ActionResult Wallet()
        {
            return View(_walletService.GetWalletHomePage().Result());
        }

        public ActionResult OrderHistory()
        {
            return View(_orderService.GetAllOrdersHomePage().Result());
        }
    }
}