using Ecommerce_Application.Filters;
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
    [JWTAuthorize]
    public class ProfileController : Controller
    {
        protected readonly ProfileServices profileServices = new ProfileServices();
        public async Task<ActionResult> Index()
        {
            int userID = Convert.ToInt32(Session["UserId"] ?? 3);
            UserModel userData = await profileServices.GetUserDetails(userID, Request.Cookies["Token"].Value.ToString());
            return View(userData);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAddress(UserModel user)
        {
            if (ModelState.IsValid) {
                 bool result = await profileServices.UpdateAddress(user, Request.Cookies["Token"].Value.ToString());
                return Json(new { success = result });
            }
            return Json(new {success = false}); ;
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(int UserId, string OldPassword, string NewPassword)
        {
            if (ModelState.IsValid)
            {
                bool result = await profileServices.ChangePassword(UserId, OldPassword, NewPassword, Request.Cookies["Token"].Value.ToString());
                return Json(new { success = result });
            }
            return Json(new { success = false }); ;
        }

        [HttpPost]
        public async Task<JsonResult> UpdateUserImg()
        {
            try
            {
                var file = Request.Files["file"];
                int userId = Convert.ToInt32(Request.Form["userId"]);
                string imgPath = await profileServices.UpdateUserProfileImg(userId, Request.Cookies["Token"].Value, file);
                if (imgPath != null)
                {
                    return Json(new { success = true, imgPath = imgPath });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("error = " + ex.Message);
                throw;
            }
        }
    }
}