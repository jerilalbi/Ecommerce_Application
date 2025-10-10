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
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register([FromBody]RegisterModel register)
        {
            UserDAL userDAL = new UserDAL();
            int rows = userDAL.RegisterUser(register);
            if(rows == -1)
            {
                return Ok(new { success = true, message = "User Registred" });
            }
            else
            {
                return BadRequest("User Registration Failed");
            }
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody]LoginModel login)
        {
            UserDAL userDAL = new UserDAL();
            UserModel user = userDAL.LoginUser(login);
            if(user != null)
            {
                return Ok(new { success = true, user });
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("userDetails")]
        public IHttpActionResult UserDetails([FromBody] UserModel userModel)
        {
            UserDAL userDAL = new UserDAL();
            UserModel user = userDAL.getUserDetails(userModel);

            if (user != null) { 
                return Ok(new { success = true, user });
            }
            else
            {
                var response = Request.CreateResponse(
                    HttpStatusCode.NotFound, new { message = "User not found." }
                );
                return ResponseMessage(response);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("updateAddress")]
        public IHttpActionResult UpdateAddress([FromBody] UserModel userModel)
        {
            UserDAL user = new UserDAL();
            int result = user.updateUserAddress(userModel);

            if(result != 0)
            {
                return Ok(new {success = true, message = "User Address Updated"});
            }
            else
            {
                return BadRequest("User Not Updated");
            }
        }
    }
}
