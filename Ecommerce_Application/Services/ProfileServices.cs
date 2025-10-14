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
    public class ProfileServices : APIHelper
    {
        public Task<UserModel> GetUserDetails(int userId, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var body = new { UserId = userId };
                    var response = await client.PostAsJsonAsync("user/userDetails", body);
                    if (response.IsSuccessStatusCode) {
                        var json = await response.Content.ReadAsStringAsync();
                        JObject jsonObject = JObject.Parse(json);
                        JToken userData = jsonObject["user"];

                        return userData.ToObject<UserModel>();
                    }
                    return null;
                },token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<bool> UpdateAddress(UserModel user, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.PutAsJsonAsync("user/updateAddress", user);
                    return response.IsSuccessStatusCode;
                }, token);
            }
            catch (Exception ex) { 
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}