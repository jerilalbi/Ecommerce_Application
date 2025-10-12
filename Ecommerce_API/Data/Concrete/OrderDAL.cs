using Ecommerce_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Ecommerce_API.Data.Concrete
{
    public class OrderDAL : DBHelper
    {
        public int AddOrder(OrderModel order)
        {
            try
            {
                string storedProcedure = "AddOrder";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@CustomerID", order.CustomerId),
                new SqlParameter("@TotalAmount", order.TotalAmount)
                );
            }
            catch (Exception ex) {
                Logger.log(ex);
                throw ex;
            }
        }
    }
}