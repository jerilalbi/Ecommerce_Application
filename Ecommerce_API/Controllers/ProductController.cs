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
    [RoutePrefix("api/products")]
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
            if (categories != null && categories.Count > 0)
            {
                return Ok(categories);
            }
            else
            {
                return BadRequest("Error in getting categories");
            }
        }

        [HttpGet]
        [Route("category/{category}")]
        public IHttpActionResult GetProductsCategory(string category)
        {
            ProductDAL productDAL = new ProductDAL();
            List<ProductModel> products = productDAL.getProductByCategory(category);
            if (products.Count > 0)
            {
                return Ok(products);
            }
            else
            {
                var response = Request.CreateResponse(
                    HttpStatusCode.NotFound, new { message = "Product not found." }
                );
                return ResponseMessage(response);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetProductsId(int id)
        {
            ProductDAL productDAL = new ProductDAL();
            ProductModel products = productDAL.getProductByID(id);
            if (products !=  null)
            {
                return Ok(products);
            }
            else
            {
                var response = Request.CreateResponse(
                    HttpStatusCode.NotFound, new { message = "Product not found." }
                );
                return ResponseMessage(response);
            }
        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchProduct(string search)
        {
            ProductDAL productDAL = new ProductDAL();
            List<ProductModel> products = productDAL.searchProduct(search);

            if (products != null)
            {
                return Ok(new { success = true, products = products });
            }
            else
            {
                return BadRequest("Error in searching");
            }
        }
    }
}
