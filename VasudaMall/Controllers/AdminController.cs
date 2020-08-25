using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Logic.Implementation;
using VasudaDataAccess.Model;
using VasudaDataAccess.Provider;
using VasudaDataAccess.Utility;

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
        public ActionResult UserDetails(string userId)
        {
            return View(_walletService.GetUserInfo(userId)._entity);
        }
        public ActionResult Order()
        {
            return View(_orderService.GetAllOrderInfo().Result());
        }
        public ActionResult Products()
        {
            return View(_productService.GetProducts()._entity);
        }
        public ActionResult Wallet()
        {
            return View(_walletService.AdminGetAllWithdrawalAccounts().Result());
        }
        public ActionResult EditProduct (string id)
        {
            return View(_productService.GetProduct(id)._entity);
        }
        public ActionResult Transaction()
        {
            return View(_paymentService.GetPaymentHistory()._entity);
        }   
        public JsonResult AddProducts()
        {
            return Json(_productService.AddProducts(Request.Files, Request.Form),JsonRequestBehavior.AllowGet);
        }   
        public JsonResult BanUser(bool action , string id)
        {
            return Json(_profileService.BanUser(id,action),JsonRequestBehavior.AllowGet);
        }    
        public JsonResult EditCategory(string category, string oldCategory)
        {
            return Json(_productService.EditCategory(category, oldCategory),JsonRequestBehavior.AllowGet);
        }  
        public JsonResult EditSubCategory(string subCategory, string oldSubCategory)
        {
            return Json(_productService.EditSubCategory(subCategory, oldSubCategory),JsonRequestBehavior.AllowGet);
        }  
        public JsonResult EditVendor(string vendor, string oldVendor, string link)
        {
            return Json(_productService.EditVendor(vendor, oldVendor,link),JsonRequestBehavior.AllowGet);
        }  
        public JsonResult Action(RequestApprovalDTO model)
        {
            return Json(_walletService.Action(model),JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddCategory(CategoryTable model)
        {
            return Json(_productService.AddCategory(model),JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddVendor(VendorTable model)
        {
            return Json(_productService.AddVendor(model),JsonRequestBehavior.AllowGet);
        } 
        public JsonResult GetSubCategory(string categoryName)
        {
            return Json(_productService.GetSubCategory(categoryName),JsonRequestBehavior.AllowGet);
        } 
        public JsonResult DeleteProduct(string id)
        {
            return Json(_productService.DeleteProduct(id),JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCategory(string id)
        {
            return Json(_productService.DeleteCategory(id),JsonRequestBehavior.AllowGet);
        }  
        public JsonResult DeleteVendor(string id)
        {
            return Json(_productService.DeleteVendor(id),JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSubCategory(string id)
        {
            return Json(_productService.DeleteSubCategory(id),JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendMail(MailDTO model)
        {
            return Json(_notificationService.SendMail(model),JsonRequestBehavior.AllowGet);
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