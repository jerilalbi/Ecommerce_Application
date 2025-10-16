using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class HeaderModel
    {
        public List<string> Categories { get; set; }
        public int CartItemsCount { get; set; }
        public bool IsCart { get; set; } = false;
    }
}