using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_Application.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Category()
        {
            return View();
        }
    }
}