using Ecommerce_API.Data.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Data
{
    public static class Logger 
    {
        public static void log(Exception ex)
        {
            ErrorDAL error = new ErrorDAL();
            error.LogToDB(ex);
        }
    }
}