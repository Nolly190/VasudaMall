using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;
using VasudaDataAccess.Provider;

namespace VasudaMall.Controllers
{
   // [Authorize(Roles ="Admin")]
    [Authorize]
    public class AdminController : Controller
    {

        private IWalletService _walletService;
        private IProfileService _profileService;
        private INotificationService _notificationService;
        private IBankService _bankService;
        private IOrderService _orderService;
        private IPaymentService _paymentService;

        public AdminController(IWalletService walletService, IProfileService profileService, IOrderService orderService, 
                                IPaymentService paymentService, IBankService bankService)
        {
            _walletService = walletService;
            _profileService = profileService;
            _notificationService = new NotificationService(new UnitOfWork(new VasudaModel()));
            _orderService = orderService;
            _paymentService = paymentService;
            _bankService = bankService;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Order()
        {
            return View();
        }
        public ActionResult Wallet()
        {
            return View(_walletService.AdminGetAllWithdrawalAccounts().Result());
        }
        public ActionResult Transaction()
        {
            return View();
        }
        public new ActionResult Profile()
        {

            return View(_profileService.AdminGetAllUsers().Result());
        }
        public ActionResult Notification()
        {
            return View(_notificationService.AdminGetAllNotifications().Result());
        }    
        public ActionResult Banks()
        {
            return View(_bankService.GetAllBanks().Result());
        }
        public ActionResult Support()
        {
            return View();
        }
        
        public ActionResult More()
        {
            return View();
        }
    }
}