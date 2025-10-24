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
                new SqlParameter("@TotalAmount", order.TotalAmount),
                new SqlParameter("@Address", order.Address)
                );
            }
            catch (Exception ex) {
                Logger.log(ex);
                throw ex;
            }
        }

        public List<ViewOrderModel> ViewOrders(int CustomerID)
        {
            try
            {
                string storedProcedure = "ViewOrdersCustomer";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    List<ViewOrderModel> orders = new List<ViewOrderModel>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new ViewOrderModel
                            {
                                DeliveryId = Convert.ToInt32(reader["delivery_id"]),
                                ProductName = Convert.ToString(reader["product_name"]),
                                ProductImg = Convert.ToString(reader["img"]),
                                Quantity = Convert.ToInt32(reader["Quantity_Sold"]),
                                Address = Convert.ToString(reader["address"]),
                                Price = Convert.ToInt32(reader["price"]),
                                Status = Convert.ToString(reader["status"]),
                            });
                        }
                        return orders;
                    }
                },
                new SqlParameter("@CustomerId", CustomerID)
                );
            }
            catch (Exception ex) {
                Logger.log(ex);
                throw;
            }
        }

        public int CancelOrder(int DeliveryID)
        {
            try
            {
                string storedProcedure = "CancelOrder";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                }, new SqlParameter("@DeliveryItemID", DeliveryID));
            }
            catch (Exception ex) {
                Logger.log(ex);
                throw;
            }
        }
    }
}