using Ecommerce_API.Data.Concrete;
using Ecommerce_API.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ecommerce_API.Controllers
{
    [Authorize(Roles = "admin,super-admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        [HttpGet]
        [Route("getsales")]
        public IHttpActionResult GetSalesData()
        {
            AdminDAL adminDAL = new AdminDAL();

            List<SalesCategoryModel> salesByCategory = adminDAL.getSalesByCategory();
            List<SalesMonthModel> salesByMonth = adminDAL.getSalesByMonth();
            List<TopProductsModel> topSellingProduct = adminDAL.getTopSellingProducts();

            if(salesByMonth != null || salesByCategory != null || topSellingProduct != null )
            {
                return Ok(new {success = true, salesMonth =  salesByMonth, salesCategory = salesByCategory, topProducts = topSellingProduct});
            }
            else
            {
                return BadRequest("Error on getting Data!");
            }
        }

        [HttpGet]
        [Route("stockData")]
        public IHttpActionResult GetLowStockProducts() { 
            AdminDAL adminDAL = new AdminDAL();
            List<StocksModel> stockData= adminDAL.getLowStockProduct();

            if (stockData != null) { 
                return Ok(new {success = true, stockData=stockData});
            }
            else
            {
                return BadRequest("Unable to get stock data");
            }
        }

        [HttpGet]
        [Route("getAllUsers")]
        public IHttpActionResult GetAllUsers() { 
            AdminDAL adminDAL= new AdminDAL();
            List<AllUserModel> allUsers= adminDAL.getAllUsers();

            if (allUsers != null) { 
                return Ok(new {success = true, allUsers=allUsers});
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("makeAdmin")]
        public IHttpActionResult MakeAdmin([FromBody] AllUserModel userModel) { 
            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.makeUserAdmin(userModel);

            if (result != 0) {
                return Ok(new { success = true, message = "User Updated" });
            }
            else
            {
                return BadRequest("User not Updated");
            }
        }

        [HttpPut]
        [Authorize(Roles = "super-admin")]
        [Route("demoteAdmin")]
        public IHttpActionResult DemoteAdmin([FromBody] AllUserModel userModel)
        {
            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.demoteAdmin(userModel);

            if (result != 0)
            {
                return Ok(new { success = true, message = "Admin Demoted" });
            }
            else
            {
                return BadRequest("Admin not Demoted");
            }
        }

        [HttpDelete]
        [Route("deleteUser")]
        public IHttpActionResult DeleteUser([FromBody] AllUserModel userModel)
        {
            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.deleteUser(userModel);

            if (result != 0)
            {
                return Ok(new { success = true, message = "User Deleted" });
            }
            else
            {
                return BadRequest("User not Deleted");
            }
        }

        [HttpPost]
        [Route("addProduct")]
        public IHttpActionResult AddProduct()
        {
            var httpRequest = HttpContext.Current.Request;

            ProductModel productModel = new ProductModel
            {
                ProductName = httpRequest.Form["ProductName"],
                ProductCategory = httpRequest.Form["ProductCategory"],
                price = Convert.ToInt32(httpRequest.Form["Price"]),
                quantity = Convert.ToInt32(httpRequest.Form["Quantity"])
            };

            if (httpRequest.Files.Count == 0)
            {
                return BadRequest("No file uploaded");
            }

            var file = httpRequest.Files[0];
            if(file == null || file.ContentLength == 0)
            {
                return BadRequest("Empty File");
            }

            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.addProduct(productModel,file);

            if (result != 0)
            {
                return Ok(new { success = true, message = "Product Added" });
            }
            else
            {
                return BadRequest("Product not Added");
            }
        }

        [HttpPost]
        [Route("updateProduct")]
        public IHttpActionResult UpdateProduct()
        {
            var httpRequest = HttpContext.Current.Request;

            ProductModel productModel = new ProductModel
            {
                ProductId = Convert.ToInt32(httpRequest.Form["ProductId"]),
                ProductName = httpRequest.Form["ProductName"],
                ProductCategory = httpRequest.Form["ProductCategory"],
                price = Convert.ToInt32(httpRequest.Form["Price"]),
                quantity = Convert.ToInt32(httpRequest.Form["Quantity"]),
                imgUrl = httpRequest.Form["ImgUrl"]
            };

            string imgUrl = productModel.imgUrl;

            if (httpRequest.Files.Count > 0)
            {
                var file = httpRequest.Files["imgFile"];
                if(file != null && file.ContentLength > 0)
                {
                    string uploadsFolder = HttpContext.Current.Server.MapPath("~/Uploads/ProductImages/");

                    if (!imgUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        string fileName = Path.GetFileName(imgUrl);
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        file.SaveAs(filePath);

                        imgUrl = "/Uploads/ProductImages/" + fileName;
                    }
                    else
                    {
                        string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string newFilePath = Path.Combine(uploadsFolder, newFileName);

                        file.SaveAs(newFilePath);
                        imgUrl = "/Uploads/ProductImages/" + newFileName;
                    }
                }
            }

            productModel.imgUrl = imgUrl;

            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.updateProduct(productModel);

            if (result != 0)
            {
                return Ok(new { success = true, message = "Product Updated" });
            }
            else
            {
                return BadRequest("Product not Updated");
            }
        }

        [HttpPost]
        [Route("deleteProduct")]
        public IHttpActionResult DeleteProduct([FromBody] ProductModel product)
        {
            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.deleteProduct(product);

            if (result != 0)
            {
                if (product.imgUrl != null && !product.imgUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    string uploadsFolder = HttpContext.Current.Server.MapPath("~/Uploads/ProductImages/");
                    string fileName = Path.GetFileName(product.imgUrl);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                return Ok(new { success = true, message = "Product Deleted" });
            }
            else
            {
                return BadRequest("Product not Deleted");
            }
        }

        [HttpGet]
        [Route("allOrders")]
        public IHttpActionResult ViewAllOrder()
        {
            AdminDAL adminDAL = new AdminDAL();
            List<ViewOrdersAdminModel> orders = adminDAL.GetAllOrdersAdmins();
            if (orders.Count >= 0)
            {
                return Ok(orders);
            }
            else
            {
                return BadRequest("No Orders");
            }
        }

        [HttpPut]
        [Route("order/ship/{id}")]
        public IHttpActionResult ShipOrder(int id)
        {
            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.ShipOrder(id);
            if (result != 0)
            {
                return Ok(new { message = "Order Shipped" });
            }
            else
            {
                return BadRequest("Error");
            }
        }


    }
}
