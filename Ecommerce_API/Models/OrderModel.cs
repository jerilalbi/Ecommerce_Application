using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Models
{
    public class OrderModel
    {
        public int CustomerId { get; set; }
        public int TotalAmount { get; set; }
    }
}