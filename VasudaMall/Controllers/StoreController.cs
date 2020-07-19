using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.Logic;

namespace VasudaMall.Controllers
{
    public class StoreController : Controller
    {
        private IStoreService _storeService;

        public StoreController(IStoreService _service)
        {
            _storeService = _service;
        }
        // GET: Store

        public ActionResult Index()
        {
            return View(_storeService.GetStorePage().Result());
        }   
        public ActionResult Cart()
        {
            return View();
        }  
        public ActionResult TrackOrder()
        {
            return View();
        } 
        public ActionResult Services()
        {
            return View();
        }  
        public ActionResult Portfolio()
        {
            return View();
        }  
        public ActionResult Clearance()
        {
            return View(_storeService.ClearanceProduct().Result());
        }  
        public ActionResult Products()
        {
            return View();
        }
    }
}