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
    public class AdminController : Controller
    {
        protected readonly AdminServices adminServices = new AdminServices();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return PartialView("_Dashboard");
        }

        public ActionResult Stocks()
        {
            return PartialView("_Stocks");
        }

        public async Task<ActionResult> Orders()
        {
            dynamic data = await adminServices.GetDeliveryOrders(Request.Cookies["Token"].Value);
            List<DeliveryModel> orderData = JsonConvert.DeserializeObject<List<DeliveryModel>>(Convert.ToString(data));
            return PartialView("_Orders", orderData);
        }

        public async Task<ActionResult> AddProduct() {
            ProductServices productServices = new ProductServices();
            List<string> categories = await productServices.GetAllCategories(Request.Cookies["Token"].Value.ToString());
            return PartialView("_AddProduct", categories);
        }

        public ActionResult Users()
        {
            return PartialView("_Users");
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }

        public async Task<JsonResult> GetSales()
        {
            dynamic data = await adminServices.GetSalesData(Request.Cookies["Token"].Value);

            if (data != null)
            {

                List<SalesByMonthModel> salesByMonthsData = JsonConvert.DeserializeObject<List<SalesByMonthModel>>(Convert.ToString(data.salesMonth));
                List<SalesByCategoryModel> salesByCategoryData = JsonConvert.DeserializeObject<List<SalesByCategoryModel>>(Convert.ToString(data.salesCategory));
                List<TopsSellingProductModel> topSellingProductsData = JsonConvert.DeserializeObject<List<TopsSellingProductModel>>(Convert.ToString(data.topProducts));


                var salesByMonths = salesByMonthsData.GroupBy(sal => sal.SalesDate.ToString("MMM")).Select(val => new
                {
                    month = val.Key,
                    total = val.Sum(x => x.SalesAmount),
                }).OrderBy(val => DateTime.ParseExact(val.month, "MMM", null).Month).ToList();

                var salesByCategory = salesByCategoryData.GroupBy(sal => sal.SalesCategory).Select(val => new
                {
                    category = val.Key,
                    total = val.Sum(x => x.SalesPrice),
                }).OrderBy(val => val.total).Reverse().ToList();

                var topSellingProducts = topSellingProductsData.OrderBy(val => val.ProductCount).Reverse().ToList();

                return Json(new { success = true, salesMonth = salesByMonths, salesCategory = salesByCategory, topProduct = topSellingProducts },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetStocks()
        {
            dynamic data = await adminServices.GetStocksData(Request.Cookies["Token"].Value);

            if (data != null)
            {

                List<StocksModel> stocksDataRaw = JsonConvert.DeserializeObject<List<StocksModel>>(Convert.ToString(data.stockData));

                var stocksData = stocksDataRaw.OrderBy(val => val.Quantity).ToList();

                return Json(new { success = true, stockData = stocksData}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> ShipOrder(int id)
        {
            bool isresult =await adminServices.ShipOrder(Request.Cookies["Token"].Value, id);
            dynamic data = await adminServices.GetDeliveryOrders(Request.Cookies["Token"].Value);
            List<DeliveryModel> orderData = JsonConvert.DeserializeObject<List<DeliveryModel>>(Convert.ToString(data));
            return PartialView("_Orders", orderData);
        }
    }
}