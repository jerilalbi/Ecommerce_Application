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
    public class UserController : Controller
    {
        protected readonly UserServices userServices = new UserServices();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid) { 
                return View(login);
            }

            UserModel user = await userServices.loginUser(login, this.HttpContext);

            if (user != null)
            {
                Session["user"] = new UserModel
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    Phone = user.Phone,
                    Role = user.Role,
                };

                Session["UserId"] = user.UserId;

                if(user.Role == "admin")
                {
                    return RedirectToAction("Index","Admin");
                }else if(user.Role == "user")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.result = "Failed";
                return View(login);
            }
            return View(login);
        }

        public ActionResult Register()
        {
             return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel register, string action)
        {
            if(action == "register")
            {
                if (!ModelState.IsValid)
                {
                    return View(register);
                }

               bool isUserRegistered = await userServices.registerUser(register);

               if (isUserRegistered) {
                    ViewBag.UserRegistered = "success";
                    ViewBag.Message = "Registration successful";

                    ModelState.Clear();
                    return View(new RegisterModel());
               } else {
                    ViewBag.UserRegistered = "failed";
                    ViewBag.Message = "Registration failed. Please try again";

                    return View(register);
                }
            }
            else if (action == "login")
            {
                return RedirectToAction("Login");
            }
            return View(register);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}