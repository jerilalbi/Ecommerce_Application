using Ecommerce_Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class ProductModel : APIHelper
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public int price { get; set; }
        public string imgUrl { get; set; }
        public int quantity { get; set; }
        public string ImgFullUrl
        {
            get
            {
                if(imgUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    return imgUrl;
                }
                return ApiImgBaseUrl + imgUrl;
            }
        }
    }
}