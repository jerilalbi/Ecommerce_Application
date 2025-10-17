using Ecommerce_API.Data.Concrete;
using Ecommerce_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ecommerce_API.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        protected readonly UserDAL userDAL = new UserDAL();
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register([FromBody]RegisterModel register)
        {
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

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody]LoginModel login)
        {
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

        [HttpPost]
        [Route("userDetails")]
        public IHttpActionResult UserDetails([FromBody] UserModel userModel)
        {
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

        [HttpGet]
        [Route("previousAddress/{userId}")]
        public IHttpActionResult PreviousAddress(int userId)
        {
            List<string> previousAddress = userDAL.GetPerviousAddress(userId);
            return Ok(previousAddress);
        }

        [HttpPost]
        [Route("updateImg")]
        public IHttpActionResult UpdateUserImg(int userId){
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count == 0) {
                return BadRequest("No file Uploaded");
            }

            var file = httpRequest.Files[0];

            if (file == null || file.ContentLength == 0)
            {
                return BadRequest("Empty File");
            }


            string imgPath = userDAL.UpdateUserProfileImg(userId, file);

            return Ok(new { success = true, imgPath = imgPath, Message = "Image Updated"});
        }

        [HttpPut]
        [Route("updateAddress")]
        public IHttpActionResult UpdateAddress([FromBody] UserModel userModel)
        {
            int result = userDAL.updateUserAddress(userModel);

            if(result != 0)
            {
                return Ok(new {success = true, message = "User Address Updated"});
            }
            else
            {
                return BadRequest("User Not Updated");
            }
        }

        [HttpPut]
        [Route("changePassword")]
        public IHttpActionResult ChangePassword([FromBody] dynamic body)
        {
            int userId = Convert.ToInt32(body.UserId);
            string oldPassword = Convert.ToString(body.OldPassword);
            string newPassword = Convert.ToString(body.NewPassword);

            int result = userDAL.ChangePassword(oldPassword,newPassword,userId);

            if (result != 0)
            {
                return Ok(new { success = true, message = "Password Updated" });
            }
            else
            {
                return BadRequest("Password Updation Failed");
            }
        }

        [HttpDelete]
        [Route("deleteAccount")]
        public IHttpActionResult DeleteAccount([FromBody] UserModel userModel) {
            int result = userDAL.deleteAccount(userModel);

            if (result != 0) {
                return Ok(new { success = true, message = "Account Deleted" });
            }
            else
            {
                return BadRequest("Account not deleted");
            }
        }
    }
}
