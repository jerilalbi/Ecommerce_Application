using Ecommerce_Application.Filters;
using Ecommerce_Application.Models;
using Ecommerce_Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_Application.Controllers
{
    [JWTAuthorize]
    public class OrderController : Controller
    {
        protected readonly OrderServices orderServices = new OrderServices();
        public async Task<ActionResult> Checkout()
        {
            int userID = Convert.ToInt32(Session["UserId"] ?? 3);
            ProfileServices profileServices = new ProfileServices();
            UserModel userDetails = await profileServices.GetUserDetails(userID, Request.Cookies["Token"].Value.ToString());
            return View(userDetails);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrder(string address)
        {
            int userID = Convert.ToInt32(Session["UserId"] ?? 3);
            int totalAmount = Convert.ToInt32(TempData["TotalAmountFinal"]);
            bool result = await orderServices.CheckOutOrder(customerId: userID, totalAmount, address: address, token: Request.Cookies["Token"].Value.ToString());
            return Json(new { success = result });
        }

        public ActionResult ContinueShopping()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> ViewOrders()
        {
            int userID = Convert.ToInt32(Session["UserId"] ?? 3);
            List<ViewOrdersModel> orders = await orderServices.ViewOrder(userID, Request.Cookies["Token"].Value.ToString());
            return View(orders);
        }

        public async Task<ActionResult> CancelOrder(int deliveryId)
        {
            int userID = Convert.ToInt32(Session["UserId"] ?? 3);
            bool result = await orderServices.CancelOrder(deliveryId, Request.Cookies["Token"].Value);
            List<ViewOrdersModel> orders = await orderServices.ViewOrder(userID, Request.Cookies["Token"].Value.ToString());
            return PartialView("_ViewOrdersSec",orders);
        }
    }
}