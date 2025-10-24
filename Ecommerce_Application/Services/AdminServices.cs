using Ecommerce_Application.Helpers;
using Ecommerce_Application.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace Ecommerce_Application.Services
{
    public class AdminServices : APIHelper
    {
        public Task<dynamic> GetSalesData(string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.GetAsync("admin/getsales");
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                    return data;
                }, token);
            }
            catch (Exception ex) { 
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<dynamic> GetStocksData(string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.GetAsync("admin/stockData");
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                    return data;
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<dynamic> GetDeliveryOrders(string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.GetAsync("admin/allOrders");
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(json);

                    return data;
                }, token);
            }
            catch (Exception ex) { 
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<bool> ShipOrder(string token, int deliveryId)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.PutAsync($"admin/order/ship/{deliveryId}",new StringContent(string.Empty));
                    return response.IsSuccessStatusCode;
                },token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<bool> AddNewProduct(ProductModel product, string token, HttpPostedFileBase file)
        {
            try
            {
                return CallAPI(async client =>
                {
                    using(var content = new MultipartFormDataContent())
                    {
                        var contentStream = new StreamContent(file.InputStream);

                        content.Add(contentStream, "file", file.FileName);
                        content.Add(new StringContent(product.ProductName), "ProductName");
                        content.Add(new StringContent(product.ProductCategory), "ProductCategory");
                        content.Add(new StringContent(Convert.ToString(product.price)), "Price");
                        content.Add(new StringContent(Convert.ToString(product.quantity)), "Quantity");

                        var response = await client.PostAsync("admin/addProduct", content);
                        return response.IsSuccessStatusCode;
                    }
                        
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<bool> UpdateProduct(ProductModel product, string token, HttpPostedFileBase file)
        {
            try
            {
                return CallAPI(async client =>
                {
                    using(var content = new MultipartFormDataContent())
                    {
                        if(file != null && file.ContentLength > 0)
                        {
                            var contentStream = new StreamContent(file.InputStream);
                            content.Add(contentStream, "imgFile", file.FileName);
                        }

                        content.Add(new StringContent(Convert.ToString(product.ProductId)), "ProductId");
                        content.Add(new StringContent(product.ProductName), "ProductName");
                        content.Add(new StringContent(product.ProductCategory), "ProductCategory");
                        content.Add(new StringContent(Convert.ToString(product.price)), "Price");
                        content.Add(new StringContent(Convert.ToString(product.quantity)), "Quantity");
                        content.Add(new StringContent(product.imgUrl), "ImgUrl");

                        var response = await client.PostAsync("admin/updateProduct", content);
                        return response.IsSuccessStatusCode;
                    }
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<bool> DeleteProduct(int id, string imgUrl, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var body = new { ProductId = id, imgUrl = imgUrl };
                    var response = await client.PostAsJsonAsync($"admin/deleteProduct", body);
                    return response.IsSuccessStatusCode;
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<List<UserModel>> GetAllUsers(string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.GetAsync($"admin/getAllUsers");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        JObject jobject = JObject.Parse(json);
                        JToken userData = jobject["allUsers"];
                        return userData.ToObject<List<UserModel>>();
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

        public Task<bool> MakeAdmin(string email, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var body = new { Email = email };
                    var response = await client.PutAsJsonAsync($"admin/makeAdmin",body);
                    return response.IsSuccessStatusCode;
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<bool> DemoteAdmin(string email, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var body = new { Email = email };
                    var response = await client.PutAsJsonAsync($"admin/demoteAdmin", body);
                    return response.IsSuccessStatusCode;
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<bool> ActivateUser(int userId, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.GetAsync($"admin/activateUser/{userId}");
                    return response.IsSuccessStatusCode;
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