using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int price { get; set; }
        public string imgUrl { get; set; }
        public int quantity { get; set; }
    }
}