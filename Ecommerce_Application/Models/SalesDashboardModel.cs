using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class SalesDashboardModel
    {
        public List<dynamic> SalesByMonth { get; set; }
        public List<dynamic> SalesByCategory { get; set; }
        public List<dynamic> TopsSellingProducts { get; set; }
    }
}