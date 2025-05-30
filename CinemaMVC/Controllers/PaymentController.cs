using CinemaMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMVC.Controllers
{
    public class PaymentController : Controller
    {

        public IActionResult Payment(PaymentVM paymentVM)
        {
            ModelState.Remove("Showtime");
            ModelState.Remove("User");
            ModelState.Remove("PaymentMethod");
            ModelState.Remove("TransactionId");
            ModelState.Remove("BookingStatus");
            ModelState.Remove("CardName");
            ModelState.Remove("CardNumber");
            ModelState.Remove("ExpiryDate");
            ModelState.Remove("CVV");

            return View(paymentVM);
        }
        [HttpPost]
        public IActionResult ConfirmPayment(PaymentVM paymentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(paymentVM);
            }
            return View(paymentVM);
        }
    }
}
