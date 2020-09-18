using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;
using VasudaDataAccess.Provider;

namespace VasudaMall.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private IWalletService _walletService;
        private IProfileService _profileService;
        private INotificationService _notificationService;
        private IPaymentService _paymentService;

        public DashboardController(IWalletService walletService, IProfileService profileService, IPaymentService paymentService)
        {
            _walletService = walletService;
            _profileService = profileService;
            _notificationService = new NotificationService(new UnitOfWork(new VasudaModel()));
            _paymentService = paymentService;
        }

        static ScrapingBrowser _browser = new ScrapingBrowser();
        // GET: Dashboard
        public ActionResult Index()
        {
            //HtmlWeb web = new HtmlWeb();
            //HtmlDocument doc = web.Load("https://campaign.aliexpress.com/wow/gf/newarrivals20201/index?spm=a2g0o.home.16001.2.3f6b2145E9mZZX&wh_weex=true&wx_navbar_hidden=true&wx_navbar_transparent=true&ignoreNavigationBar=true&wx_statusbar_hidden=true&productIds=4001032200037");
            //HtmlNode TitleNode = doc.DocumentNode.CssSelect(".site-footer").First();

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
            return View(_profileService.GetProfileHomePage(User.Identity.GetUserId()).Result());
        }

        [HttpPost]
        public JsonResult UpdateProfile(AspNetUser model)
        {
            model.Id = User.Identity.GetUserId();
            return Json(_profileService.UpdateUserProfile(model), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendChat(string message)
        {
            return Json(_notificationService.SendChats(User.Identity.GetUserId(), message, "User"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Notification()
        {
            return View(_notificationService.GetAllNotificationsHomePage().Result());
        }
        
        public ActionResult Transaction()
        {
            return View(_walletService.GetAllTransactionsHomePage(User.Identity.GetUserId()).Result());
        }

        public ActionResult Wallet()
        {
            return View(_walletService.GetWalletHomePage(User.Identity.GetUserId()).Result());
        }
        
        public ActionResult Support()
        {
            return View(_notificationService.GetAllChats(User.Identity.GetUserId(), "Admin").Result());
        }
    }
}