using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class AllUserModel
    {
        public List<UserModel> Users {  get; set; }
        public List<UserModel> Admins { get; set; }
    }
}