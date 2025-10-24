using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

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

        public void LogToEventLog(Exception ex)
        {
            string source = "EcommerceApp";
            string logName = "Application";

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
            }

            EventLog.WriteEntry(source, ex.ToString(), EventLogEntryType.Error);
        }
    }
}