using Ecommerce_Application.Helpers;
using Ecommerce_Application.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Ecommerce_Application.Services
{
    public class CartServices : APIHelper
    {
        public Task<bool> AddToCart(CartModel cart,string token)
        {
            return CallAPI(async client =>
            {
                var response = await client.PostAsJsonAsync("cart/addItem", cart);
                return response.IsSuccessStatusCode;
            },token);
        }

        public Task<List<CartModel>> ViewCartItems(string token, int customerId)
        {
            return CallAPI(async client =>
            {
                var body = new { customerId = customerId };
                var response = await client.PostAsJsonAsync("cart/cartItems", body);
                if (response.IsSuccessStatusCode) {
                    var json = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(json);
                    JToken cartItems = jsonObject["cartItems"];

                    return cartItems.ToObject<List<CartModel>>();
                }
                return null;
            }, token);
        }

        public Task<bool> DeleteCartItem(string token, int id)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.DeleteAsync($"cart/remove/{id}");
                    return response.IsSuccessStatusCode;
                },token);
            }
            catch (Exception ex) { 
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}