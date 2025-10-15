using Ecommerce_Application.Helpers;
using Ecommerce_Application.Models;
using Newtonsoft.Json;
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

        public Task<bool> AddNewProduct(ProductModel product, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.PutAsJsonAsync("", product);
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