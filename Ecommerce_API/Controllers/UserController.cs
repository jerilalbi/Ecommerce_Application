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
            string token = userDAL.LoginUser(login);
            if(!String.IsNullOrEmpty(token))
            {
                return Ok(new { success = true, token = token });
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }
    }
}
