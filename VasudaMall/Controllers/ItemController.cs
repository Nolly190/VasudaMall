using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Logic;
using VasudaDataAccess.Utility;

namespace VasudaMall.Controllers
{
    public class ItemController : Controller
    {
        private IOrderService _orderService { get; set; }

        public ItemController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Domestic()
        {
            return View(_orderService.GetDomesticItemsPage(User.Identity.GetUserId()).Result());
        }

        public ActionResult General()
        {
            return View(_orderService.GetGeneralItemsPage(User.Identity.GetUserId()).Result());
        }

        [HttpPost]
        public JsonResult AddDomesticItem(AddDomesticItemViewModel model)
        {
            var response = new Response<string>();
            if (!ModelState.IsValid)
            {
                response.Message = ValidateModelState();
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            return Json(_orderService.AddDomesticItem(model, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddShippingItem(AddShippingItemViewModel model)
        {
            var response = new Response<string>();
            if (!ModelState.IsValid)
            {
                response.Message = ValidateModelState();
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            return Json(_orderService.AddShippingItem(model, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddPurchaseItem(AddPurchaseItemViewModel model)
        {
            var response = new Response<string>();
            if (!ModelState.IsValid)
            {
                response.Message = ValidateModelState();
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (!Util.ValidateUrl(model.ProductLink))
            {
                response.Message = "Please enter a correct product link";
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json(_orderService.AddPurchaseItem(model, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddShippingAndPurchaseItem(AddPurchaseAndShippingItemViewModel model)
        {
            var response = new Response<string>();
            if (!ModelState.IsValid)
            {
                response.Message = ValidateModelState();
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }


            if (!Util.ValidateUrl(model.ProductLink))
            {
                response.Message = "Please enter a correct product link.";
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            return Json(_orderService.AddShippingAndPurchaseItem(model, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteItem(string id)
        {
            var response = new Response<string>();
            if (string.IsNullOrEmpty(id))
            {
                response.Message = "Delete was not successful. Try again";
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json(_orderService.DeleteItem(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetSingleItem(string id)
        {
            var response = new Response<string>();
            if (string.IsNullOrEmpty(id))
            {
                response.Message = "Unable to retrieve item....";
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            return Json(_orderService.GetSingleItem(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProcessDomestic(string id, string action)
        {
            var response = new Response<string>();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(action))
            {
                response.Message = "Unable to process. Try again";
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json(_orderService.ProcessDomesticItem(id, action, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }

        public string ValidateModelState()
        {
            var message = string.Empty;
            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    message = item.Errors.FirstOrDefault().ErrorMessage.ToString();
                    break;
                }
            }
            return message;
        }

    }
}