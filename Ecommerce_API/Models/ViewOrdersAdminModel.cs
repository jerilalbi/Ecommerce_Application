using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Models
{
    public class ViewOrdersAdminModel
    {
        public int DeliveryId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}