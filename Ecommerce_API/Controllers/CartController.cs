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
    [RoutePrefix("api/cart")]
    public class CartController : ApiController
    {
        [HttpPost]
        [Route("cartItems")]
        public IHttpActionResult getCartItems([FromBody] dynamic data)
        {
            CartDAL cart = new CartDAL();
            int customerId = Convert.ToInt32(data.customerId);

            List<CartModel> cartItems = cart.getCartItems(customerId);

            if(cartItems != null)
            {
                return Ok(new { success = true, cartItems });
            }
            else
            {
                return BadRequest("Error in getting cart items");
            }   
        }

        [HttpPost]
        [Route("addItem")]
        public IHttpActionResult addCartItems([FromBody] CartModel cart) { 
            CartDAL cartDAL = new CartDAL();
            int result = cartDAL.addItemToCart(cart);

            if(result != 0)
            {
                return Ok(new { success = true, message = "Item Added to Cart", cartItemId = result });
            }
            else
            {
                return BadRequest("Item not added!");
            }
        }

        [HttpPut]
        [Route("update")]
        public IHttpActionResult changeItemQuantity([FromBody] dynamic cart)
        {
            CartDAL cartDAL = new CartDAL();
            dynamic result = cartDAL.updateItemQuantity(cart);

            if (result.RowsAffected != 0)
            {
                return Ok(new { success = true, message = "Item Updated", UpdatedQuantity = result.UpdatedQuantity });
            }
            else
            {
                return BadRequest("Item not updated!");
            }
        }

        [HttpDelete]
        [Route("remove/{id}")]
        public IHttpActionResult removeProduct(int id) { 
            CartDAL cartDal = new CartDAL();
            int result = cartDal.deleteItemCart(id);

            if (result != 0)
            {
                return Ok(new { success = true, message = "Item Removed" });
            }
            else
            {
                return BadRequest("Item not removed!");
            }
        }
    }
}
