using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.Logic;

namespace VasudaMall.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService _OrderService { get; set; }

        public OrderController(IOrderService _service)
        {
            _OrderService = _service;
        } 
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
    }
}