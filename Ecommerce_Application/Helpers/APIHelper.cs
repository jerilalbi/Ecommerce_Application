using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http.Headers;

namespace Ecommerce_Application.Helpers
{
    public abstract class APIHelper
    {
        protected delegate Task<T> ExecuteDelegate<T>(HttpClient client);

        protected const string ApiImgBaseUrl = "http://localhost:60";

        protected static readonly HttpClient client = new HttpClient
        {
            //BaseAddress = new Uri("https://localhost:44301/api/")
            BaseAddress = new Uri("http://localhost:60/api/")
        };

        protected async Task<T> CallAPI<T>(ExecuteDelegate<T> Execute, string token = null)
        {
            if (!string.IsNullOrEmpty(token)) {
                   client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try{
                return await Execute(client);
            }catch (Exception) {
                throw;
            }
        }
    }
}