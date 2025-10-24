using Ecommerce_Application.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class UserModel : APIHelper
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public List<string> PreviousAddress { get; set; }
        public string ImgUrl { get; set; }
        public string FullUrl
        {
            get
            {
                if(string.IsNullOrWhiteSpace(ImgUrl))
                {
                    return "https://www.shutterstock.com/image-vector/universal-blank-profile-picture-avatar-600nw-1654275940.jpg";
                }
                return ApiImgBaseUrl + ImgUrl;
            }
        }
    }
}