using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;
using VasudaDataAccess.Provider;

namespace VasudaMall.Controllers
{
    [Authorize(Roles ="Admin,SuperAdmin")]
   
    public class AdminController : Controller
    {

        private IWalletService _walletService;
        private IProfileService _profileService;
        private INotificationService _notificationService;
        private IBankService _bankService;
        private IOrderService _orderService;
        private IPaymentService _paymentService;
        private IProductService _productService;

        public AdminController(IWalletService walletService, IProfileService profileService, IOrderService orderService, 
                                IPaymentService paymentService, IBankService bankService, IProductService productService)
        {
            _walletService = walletService;
            _profileService = profileService;
            _notificationService = new NotificationService(new UnitOfWork(new VasudaModel()));
            _orderService = orderService;
            _paymentService = paymentService;
            _bankService = bankService;
            _productService = productService;
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
        public ActionResult Products()
        {
            return View(_productService.GetProducts()._entity);
        }
        public ActionResult Wallet()
        {
            return View(_walletService.AdminGetAllWithdrawalAccounts().Result());
        }
        public ActionResult Transaction()
        {
            return View();
        }   
        public JsonResult AddProducts()
        {
            return Json(_productService.AddProducts(Request.Files, Request.Form),JsonRequestBehavior.AllowGet);
        }  
        public JsonResult Action(RequestApprovalDTO model)
        {
            return Json(_walletService.Action(model),JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddCategory(CategoryTable model)
        {
            return Json(_productService.AddCategory(model),JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddSubCategory(SubCategoryTable model)
        {
            return Json(_productService.AddSubCategory(model),JsonRequestBehavior.AllowGet);
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