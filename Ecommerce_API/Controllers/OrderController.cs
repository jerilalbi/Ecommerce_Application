using Ecommerce_API.Data.Concrete;
using Ecommerce_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ecommerce_API.Controllers
{
    [Authorize]
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        [HttpPost]
        [Route("addOrder")]
        public IHttpActionResult AddOrder(OrderModel order)
        {
            OrderDAL orderDAL = new OrderDAL();
            int result = orderDAL.AddOrder(order);
            if (result == 0) {
                return BadRequest("Order Not Added");
            }
            else
            {
                return Ok(new { success = true, message = "Order Added" });
            }
        }

        [HttpGet]
        [Route("user/{id}")]
        public IHttpActionResult ViewOrder(int id)
        {
            OrderDAL orderDAL = new OrderDAL();
            List<ViewOrderModel> orders = orderDAL.ViewOrders(id);
            if(orders.Count > 0)
            {
                return Ok(orders);
            }
            else
            {
                return BadRequest("No Orders");
            }
        }

        [HttpPut]
        [Route("cancel/{id}")]
        public IHttpActionResult CancelOrder(int id)
        {
            OrderDAL orderDAL = new OrderDAL();
            int result = orderDAL.CancelOrder(id);
            if (result != 0)
            {
                return Ok(new {message = "Order Cancelled"});
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
