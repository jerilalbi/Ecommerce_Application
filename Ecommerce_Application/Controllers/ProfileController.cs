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
    }
}