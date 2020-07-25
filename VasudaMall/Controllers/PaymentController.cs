using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VasudaDataAccess.Provider;

namespace VasudaMall.Controllers
{
    public class PaymentController : Controller
    {
        private IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        // GET: Payment
        public ActionResult Index(string transaction_id, string tx_ref, string status)
        {
            _paymentService.UpdatePaymentInfo(status, transaction_id, tx_ref);
            return RedirectToAction("Wallet","Dashboard");
        }
    }
}