using Ecommerce_Application.Helpers;
using Ecommerce_Application.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ecommerce_Application.Services
{
    public class ProductServices : APIHelper
    {
        public Task<List<ProductModel>> getTopSellingProducts(string token)
        {
            return CallAPI(async client =>
            {
                var response = await client.GetAsync("products/getTopSelling");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ProductModel>>(json);
                }
                return null;
            },token);
        }

        public Task<List<string>> GetAllCategories(string token)
        {
            return CallAPI(async client =>
            {
                var response = await client.GetAsync("products/getAllCategories");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<string>>(json);
                }
                return new List<string>();
            }, token);
        }

        public Task<List<ProductModel>> GetProductsFromCategory(string token, string categoryName)
        {
            try
            {
                return CallAPI(async cilent =>
                {
                    string encodedCategory = Uri.EscapeDataString(categoryName);
                    var response = await client.GetAsync($"products/category/{encodedCategory}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<ProductModel>>(json);
                    }
                    return null;
                }, token);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<ProductModel> GetProductDetails(string token, int productID)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.GetAsync($"products/{productID}");
                    if (response.IsSuccessStatusCode) {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ProductModel>(json);
                    } 
                    return null;
                }, token);
            }
            catch (Exception ex) {
               Debug.WriteLine(ex.Message);
               throw;
            }
        }

        public Task<List<ProductModel>> SearchProduct(string token, string searchTerm)
        {
            try
            {
                return CallAPI(async cilent =>
                {
                    string encodedCategory = Uri.EscapeDataString(searchTerm);
                    var response = await client.GetAsync($"products/search?search={encodedCategory}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        JObject jsonObject = JObject.Parse(json);
                        JToken products = jsonObject["products"];

                        return products.ToObject<List<ProductModel>>();
                    }
                    return null;
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}