using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class ProductDetailsModel
    {
        public ProductModel Product { get; set; }
        public CartModel CartProduct { get; set; }
        public bool IsProductPresent { get; set; }
    }
}