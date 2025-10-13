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
    public class HomeController : Controller
    {
        protected readonly ProductServices productServices = new ProductServices();
        public async Task<ActionResult> Index()
        {
            List<ProductModel> products = await productServices.getTopSellingProducts(Request.Cookies["Token"].Value.ToString());
            return View(products);
        }

        public async Task<ActionResult> LoadHeader()
        {
            try
            {
                List<string> categories = await productServices.GetAllCategories(Request.Cookies["Token"].Value.ToString());
                return PartialView("_Header", categories);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}