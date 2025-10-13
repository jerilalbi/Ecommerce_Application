using Ecommerce_Application.Models;
using Ecommerce_Application.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_Application.Controllers
{
    public class CartController : Controller
    {
        protected readonly CartServices cartServices = new CartServices();
        public async Task<ActionResult> Index()
        {
            int customerId = Convert.ToInt32(Session["UserId"] ?? 3);
            List<CartModel> cartItems = await cartServices.ViewCartItems(Request.Cookies["Token"].Value.ToString(),customerId);

            return View(cartItems);
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(CartModel cartModel)
        {
            cartModel.CustomerID = Convert.ToInt32(Session["UserId"] ?? 3);
            var result = await cartServices.AddToCart(cartModel, Request.Cookies["Token"].Value.ToString());
            return Json(new {success = result});
        }

        [HttpPost]
        public async Task<ActionResult> DeleteItem(int id)
        {
            Debug.WriteLine($"cart item id: {id}");
            bool isDeleted = await cartServices.DeleteCartItem(Request.Cookies["Token"].Value.ToString(), id);
            int customerId = Convert.ToInt32(Session["UserId"] ?? 3);
            List<CartModel> cartItems = await cartServices.ViewCartItems(Request.Cookies["Token"].Value.ToString(), customerId);

            if (isDeleted)
            {
                return PartialView("_CartItems", cartItems);
            }
            return View(cartItems);
        }
    }
}