using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class DeliveryModel
    {
        public List<AdminOrdersModel> PendingOrders { get; set; }
        public List<AdminOrdersModel> AllOrders { get; set; }
    }
}