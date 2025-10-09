using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Data
{
    public abstract class DBHelper
    {
        protected string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        protected delegate T ExecuteDelegate<T>(SqlCommand cmd);

        protected T ExecuteSQL<T>(string query, ExecuteDelegate<T> del, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if(parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    con.Open();
                    return del(cmd);
                }
            }
        }
    }
}