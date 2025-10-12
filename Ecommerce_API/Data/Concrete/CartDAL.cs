using Ecommerce_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Data.Concrete
{
    public class CartDAL : DBHelper
    {
        public List<CartModel> getCartItems(int customerId)
        {
            try
            {
                string storedProcedure = "ShowCartItems";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    List<CartModel> carts = new List<CartModel>();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carts.Add(new CartModel
                            {
                                CustomerID = Convert.ToInt32(reader["customer_id"]),
                                ProductID = Convert.ToInt32(reader["product_id"]),
                                Quantity = Convert.ToInt32(reader["quantity"]),
                                Price = Convert.ToInt32(reader["price"]),
                                SubTotal = Convert.ToInt32(reader["sub_total"])
                            });
                        }
                    }
                    return carts;
                }, new SqlParameter("@customerId", customerId)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int addItemToCart(CartModel cart)
        {
            try
            {
                string storedProcedure = "AddToCart";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@customerID",cart.CustomerID),
                new SqlParameter("@productID",cart.ProductID),
                new SqlParameter("@quantity",cart.Quantity),
                new SqlParameter("@price",cart.Price)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public dynamic updateItemQuantity(dynamic cart)
        {
            try
            {
                string storedProcedure = "UpdateCart";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    object row = cmd.ExecuteScalar();
                    int result = row == null ? 0 : Convert.ToInt32(row);

                    if (result != 0)
                    {
                        return new
                        {
                            RowsAffected = 1,
                            UpdatedQuantity = result
                        };
                    }
                    return new { RowsAffected = 0, UpdatedQuantity = 0 };
                },
                new SqlParameter("@customerID", Convert.ToInt32(cart.customerId)),
                new SqlParameter("@productID", Convert.ToInt32(cart.productId)),
                new SqlParameter("@change", Convert.ToString(cart.type))
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int deleteItemCart(CartModel cart)
        {
            try
            {
                string storedProcedure = "RemoveProductCart";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@customerID", cart.CustomerID),
                new SqlParameter("@productID", cart.ProductID)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }
    }
}