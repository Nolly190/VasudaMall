using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;
using VasudaDataAccess.Provider;

namespace VasudaMall.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IWalletService _walletService;
        private IProfileService _profileService;
        private INotificationService _notificationService;
        private IOrderService _orderService;
        private IPaymentService _paymentService;

        public DashboardController(IWalletService walletService, IProfileService profileService, IOrderService orderService, IPaymentService paymentService)
        {
            _walletService = walletService;
            _profileService = profileService;
            _notificationService = new NotificationService(new UnitOfWork(new VasudaModel()));
            _orderService = orderService;
            _paymentService = paymentService;
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddAccount(WithdrawalDetailsTable model)
        {
            model.UserId = User.Identity.GetUserId();
            return Json(_profileService.AddWithdrawalAccount(model),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteAccount(string id)
        {
            return Json(_profileService.DisableWithdrawalAccount(id),JsonRequestBehavior.AllowGet);
        }
        public JsonResult ResolveAccount(WithdrawalDetailsTable model)
        {
           return Json(_paymentService.GetValidAccountName(model.BankName, model.AccountNumber), JsonRequestBehavior.AllowGet);
        } 
        public JsonResult WithdrawalRequest(WithdrawalRequestTable model)
        {
            model.UserId = User.Identity.GetUserId();
           return Json(_walletService.WithdrawalRequest(model), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FundRequest(FundingRequestTable model)
        {

            model.Userid = User.Identity.GetUserId();
           return Json(_walletService.FundingRequest(model), JsonRequestBehavior.AllowGet);
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
            return View(_walletService.GetWalletHomePage(User.Identity.GetUserId()).Result());
        }

        public ActionResult OrderHistory()
        {
            return View(_orderService.GetAllOrdersHomePage().Result());
        }
    }
}