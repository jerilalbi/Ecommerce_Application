using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Models
{
    public class PasswordModel
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}