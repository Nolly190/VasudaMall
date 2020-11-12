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
using VasudaMall.Utilities;

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
        private UnitOfWork _unitOfWork;

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
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }

        // GET: Admin
        public ActionResult Index()
        {

            return View(_orderService.GetHomeReport().Result());
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
        public ActionResult ShippingPrice()
        {
            ViewBag.ServicePrice = _unitOfWork.SettingTable.GetAll().LastOrDefault()?.ServiceCharge;
            ViewBag.ExchangeTable = _unitOfWork.ExchangeRateTable.GetExchangeRates();
            return View(_orderService.GetAllPrice().Result());
        }   
        public JsonResult AddProducts()
        {
            return Json(_productService.AddProducts(Request.Files, Request.Form),JsonRequestBehavior.AllowGet);
        }   
        public JsonResult AddServiceFee(decimal amount)
        {
            return Json(_productService.AddServicePrice(amount),JsonRequestBehavior.AllowGet);
        }   
        public JsonResult AddExchangeRate(ExchangeRateTable model)
        {
            return Json(_productService.AddExchange(model),JsonRequestBehavior.AllowGet);
        }   
        public JsonResult ProcessOrder(string orderId , string amount)
        {
            return Json(_orderService.ProcessOrder(orderId,amount),JsonRequestBehavior.AllowGet);
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
        public JsonResult ManagePrice(PriceTable model)
        {
            return Json(_walletService.ManagePrice(model),JsonRequestBehavior.AllowGet);
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
        public JsonResult ManageAdmin(string userId,bool makeAdmin)
        {
           return Json(new UtilityClass().ManageAdmin(userId, makeAdmin), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddSubCategory(SubCategoryTable model)
        {
            return Json(_productService.AddSubCategory(model),JsonRequestBehavior.AllowGet);
        }
        public JsonResult ViewMoreDomesticInfo(string id)
        {
            return Json(_orderService.GetDomesticInfo(id),JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderItems(string orderId)
        {
            return Json(_orderService.GetOrderItems(orderId),JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendPriceQuotation(string id, decimal amount)
        {
            return Json(_orderService.PriceQuote(id,amount),JsonRequestBehavior.AllowGet);
        } public JsonResult SendChat(string message,string userId)
        {
            return Json(_notificationService.SendChats(userId,message, "Admin"),JsonRequestBehavior.AllowGet);
        }
        public JsonResult RetrieveChat(string userId)
        {

            return Json(  _notificationService.GetAllChats(userId,"User"), JsonRequestBehavior.AllowGet);
        }   
        public JsonResult ViewNotification(string noteId)
        {

            return Json(  _notificationService.GetNotification(noteId), JsonRequestBehavior.AllowGet);
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
            return View(_unitOfWork.SupportTable.GetAll().ToList());
        }
        
        public ActionResult More()
        {
            return View();
        }
    }
}