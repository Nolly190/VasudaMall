using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Logic;

namespace VasudaMall.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService _orderService { get; set; }

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        } 
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult History()
        {
            return View(_orderService.GetAllOrdersHomePage(User.Identity.GetUserId()).Result());
        }
        
        public ActionResult Checkout()
        {
            return View(_orderService.GetAllItemsPage(User.Identity.GetUserId()).Result());
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

        public bool ValidateUrl(string urlName)
        {
            return Uri.TryCreate(urlName, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}