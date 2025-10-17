using Ecommerce_Application.Helpers;
using Ecommerce_Application.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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

        public Task<List<string>> GetPreviousAddress(int userId, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var response = await client.GetAsync($"user/previousAddress/{userId}");
                    if (response.IsSuccessStatusCode) { 
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<List<string>>(json);
                    }
                    return new List<string>();
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

        public Task<bool> ChangePassword(int userId, string oldPassword, string newPassword, string token)
        {
            try
            {
                return CallAPI(async client =>
                {
                    var body = new
                    {
                        UserId = userId,
                        OldPassword = oldPassword,
                        NewPassword = newPassword
                    };
                    var response = await client.PutAsJsonAsync("user/changePassword", body);
                    return response.IsSuccessStatusCode;
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<string> UpdateUserProfileImg(int userId, string token,HttpPostedFileBase file)
        {
            try
            {
                return CallAPI(async client =>
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        var streamContent = new StreamContent(file.InputStream);
                        content.Add(streamContent,"file",file.FileName);

                        var resonse = await client.PostAsync($"user/updateImg?userId={userId}",content);

                        if (resonse.IsSuccessStatusCode)
                        {
                            var json = await resonse.Content.ReadAsStringAsync();
                            JObject jsonObject = JObject.Parse(json);
                            return jsonObject["imgPath"].ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }, token);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}