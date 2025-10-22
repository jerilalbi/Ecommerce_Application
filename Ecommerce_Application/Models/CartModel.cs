using Ecommerce_Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class CartModel : APIHelper
    {
        public int CartItemId { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductImg {  get; set; }
        public string ImgFullUrl
        {
            get
            {
                if(string.IsNullOrWhiteSpace(ProductImg))
                {
                    return "";
                }

                if (ProductImg.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    return ProductImg;
                }
                return ApiImgBaseUrl + ProductImg;
            }
        }
        public int Price { get; set; }
        public int SubTotal { get; set; }
        public int MaxStock { get; set; }
    }
}