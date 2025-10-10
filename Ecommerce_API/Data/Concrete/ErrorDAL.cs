using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Ecommerce_API.Data.Concrete
{
    public class ErrorDAL : DBHelper
    {
        public void LogToDB(Exception ex)
        {
            try
            {
                string storedProcedure = "LogToDB";
                ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                     return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@message",ex.Message),
                new SqlParameter("@trace",ex.StackTrace)
                );
            }
            catch (Exception ex2)
            {
                throw ex2;
            }
        }
    }
}