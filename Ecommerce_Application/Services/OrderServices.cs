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

namespace Ecommerce_Application.Services
{
    public class OrderServices : APIHelper
    {
        public Task<bool> CheckOutOrder( int customerId, int totalAmount, string address, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var body = new
                    {
                        CustomerId = customerId,
                        TotalAmount = totalAmount,
                        Address = address
                    };
                    var response = await client.PostAsJsonAsync("order/addOrder",body);
                    return response.IsSuccessStatusCode;
                },token);
            }
            catch (Exception ex) { 
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<List<ViewOrdersModel>> ViewOrder(int id, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.GetAsync($"order/user/{id}");
                    if (response.IsSuccessStatusCode) {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<ViewOrdersModel>>(json);
                    }
                    return null;
                },token);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<bool> CancelOrder(int id, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.PutAsync($"order/cancel/{id}", new StringContent(string.Empty));
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