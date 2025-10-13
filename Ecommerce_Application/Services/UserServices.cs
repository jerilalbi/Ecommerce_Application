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

namespace Ecommerce_Application.Services
{
    public class UserServices : APIHelper
    {
        public async Task<bool> registerUser(RegisterModel register)
        {
            return await CallAPI(async client =>
            {
                var response = await client.PostAsJsonAsync("user/register", register);
                return response.IsSuccessStatusCode;
            });
        }

        public async Task<UserModel> loginUser(LoginModel login, HttpContextBase context) {
            return await CallAPI(async client =>
            {
                var response = await client.PostAsJsonAsync("user/login", login);
                if (response.IsSuccessStatusCode) {
                    var json = await response.Content.ReadAsStringAsync();
                    JObject rootObject = JObject.Parse(json);
                    JToken user = rootObject["user"];

                    if(user != null)
                    {
                        context.Session["Token"] = user["Token"];

                        HttpCookie tokenCookie = new HttpCookie("Token", user["Token"].ToString());
                        tokenCookie.Expires = DateTime.Now.AddDays(7);
                        HttpContext.Current.Response.Cookies.Add(tokenCookie);

                        return user.ToObject<UserModel>();
                    }
                }
                return null;
            });
        }
    }
}