using Ecommerce_Application.Filters;
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
    [JWTAuthorize]
    public class HomeController : Controller
    {
        protected readonly ProductServices productServices = new ProductServices();
        protected readonly CartServices cartServices = new CartServices();
        public async Task<ActionResult> Index()
        {
            List<ProductModel> products = await productServices.getTopSellingProducts(Request.Cookies["Token"].Value.ToString());
            return View(products);
        }

        public async Task<ActionResult> LoadHeader(bool isCart)
        {
            try
            {
                int userID = Convert.ToInt32(Session["UserId"] ?? 3);
                List<string> categories = await productServices.GetAllCategories(Request.Cookies["Token"].Value);
                List<CartModel> cartItems = await cartServices.ViewCartItems(Request.Cookies["Token"].Value,userID);
                HeaderModel headerModel = new HeaderModel { Categories = categories, IsCart = isCart, CartItemsCount = cartItems.Count};
                return PartialView("_Header", headerModel);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}