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

            return Ok(new { success = true, cartItems });
        }

        [HttpPost]
        [Route("addItem")]
        public IHttpActionResult addCartItems([FromBody] CartModel cart) { 
            CartDAL cartDAL = new CartDAL();
            int result = cartDAL.addItemToCart(cart);

            if(result != 0)
            {
                return Ok(new { success = true, message = "Item Added to Cart" });
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
        [Route("remove")]
        public IHttpActionResult removeProduct([FromBody] CartModel product) { 
            CartDAL cartDal = new CartDAL();
            int result = cartDal.deleteItemCart(product);

            if (result != 0)
            {
                return Ok(new { success = true, message = "Item Removed" });
            }
            else
            {
                return BadRequest("Item not removed!");
            }
        }

        [HttpDelete]
        [Route("clear")]
        public IHttpActionResult clearCart([FromBody] CartModel cart)
        {
            CartDAL cartDal = new CartDAL();
            int result = cartDal.clearCart(cart);

            if (result != 0)
            {
                return Ok(new { success = true, message = "Cart Cleared" });
            }
            else
            {
                return BadRequest("Cart not cleared!");
            }
        }
    }
}
