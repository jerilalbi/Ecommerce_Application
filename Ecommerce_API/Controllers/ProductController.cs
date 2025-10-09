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
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        [HttpGet]
        [Route("getTopSelling")]
        public IHttpActionResult GetTopSellingProduct()
        {
            ProductDAL productDAL = new ProductDAL();
            List<ProductModel> products = productDAL.getTopSellingProducts();
            if (products != null && products.Count > 0)
            {
                return Ok(products);
            }
            else
            {
                return BadRequest("Error in getting products");
            }
        }

        [HttpGet]
        [Route("getAllCategories")]
        public IHttpActionResult GetAllCategories()
        {
            ProductDAL productDAL = new ProductDAL();
            List<string> categories = productDAL.getAllCategories();
            if(categories != null && categories.Count > 0)
            {
                return Ok(categories);
            }
            else
            {
                return BadRequest("Error in getting categories");
            }
        }
    }
}
