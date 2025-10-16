using Ecommerce_Application.Filters;
using Ecommerce_Application.Models;
using Ecommerce_Application.Services;
using Newtonsoft.Json;
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
    public class ProductController : Controller
    {
        protected readonly ProductServices productServices = new ProductServices();

        public async Task<ActionResult> Category(string id)
        {
            ViewBag.Category = id;
            List<ProductModel> products = await productServices.GetProductsFromCategory(Request.Cookies["Token"].Value.ToString(), id);
            return View(products);
        }

        public async Task<ActionResult> Details(int id) {
            CartServices cartServices = new CartServices();
            int customerId = Convert.ToInt32(Session["UserId"] ?? 3);

            ProductModel product = await productServices.GetProductDetails(Request.Cookies["Token"].Value.ToString(), id);
            List<CartModel> cartItems = await cartServices.ViewCartItems(Request.Cookies["Token"].Value.ToString(), customerId);
            ProductDetailsModel productDetails = new ProductDetailsModel { };

            bool isInCart = cartItems.Any(c => c.ProductID == id);
            if (isInCart) {
                productDetails.CartProduct = cartItems.FirstOrDefault(item => item.ProductID == product.ProductId);
            }

            productDetails.Product = product;
            productDetails.IsProductPresent = isInCart;

            return View(productDetails);
        }

        public async Task<ActionResult> SeachProduct(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    return Json(new List<ProductModel>(), JsonRequestBehavior.AllowGet);
                }

                List<ProductModel> products = await productServices.SearchProduct(Request.Cookies["Token"].Value.ToString(), query);
                return Json(products, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                Debug.WriteLine("Error: " + ex.Message);
                throw ex;
            }
        }
    }
}