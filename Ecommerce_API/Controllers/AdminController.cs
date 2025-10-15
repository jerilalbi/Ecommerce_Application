using Ecommerce_API.Data.Concrete;
using Ecommerce_API.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ecommerce_API.Controllers
{
    [Authorize(Roles = "admin")]
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
        public IHttpActionResult AddProduct([FromBody] ProductModel productModel)
        {
            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.addProduct(productModel);

            if (result != 0)
            {
                return Ok(new { success = true, message = "Product Added" });
            }
            else
            {
                return BadRequest("Product not Added");
            }
        }

        [HttpPut]
        [Route("updateProduct")]
        public IHttpActionResult UpdateProduct([FromBody] ProductModel productModel)
        {
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

        [HttpDelete]
        [Route("deleteProduct/{productId}")]
        public IHttpActionResult DeleteProduct(int productId)
        {
            AdminDAL adminDAL = new AdminDAL();
            int result = adminDAL.deleteProduct(productId);

            if (result != 0)
            {
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
