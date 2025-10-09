using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_Application.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return PartialView("_Dashboard");
        }

        public ActionResult AddProduct() { 
            return PartialView("_AddProduct");
        }

        public ActionResult Users()
        {
            return PartialView("_Users");
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }
    }
}