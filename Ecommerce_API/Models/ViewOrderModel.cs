using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Models
{
    public class ViewOrderModel
    {
        public int DeliveryId { get; set; }
        public string ProductName { get; set; }
        public string ProductImg { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
    }
}