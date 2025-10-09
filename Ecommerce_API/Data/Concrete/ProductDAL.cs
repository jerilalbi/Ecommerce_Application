using Ecommerce_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ecommerce_API.Data.Concrete
{
    public class ProductDAL : DBHelper
    {
        public List<ProductModel> getTopSellingProducts()
        {
            try
            {
                string storedProcedure = "TopSellingProduct";
                return ExecuteSQL(storedProcedure, cmd => {
                    List<ProductModel> products = new List<ProductModel>();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows) {
                        products.Add(
                            new ProductModel()
                            {
                                ProductId = Convert.ToInt32(row["product_id"]),
                                ProductName = Convert.ToString(row["product_name"]),
                                price = Convert.ToInt32(row["price"]),
                                imgUrl = Convert.ToString(row["img"])
                            });
                    }
                    return products;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> getAllCategories()
        {
            try
            {
                string storedProcedure = "AllCategories";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    List<string> categories = new List<string>();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows) {
                        categories.Add(Convert.ToString(row["category"]));
                    }

                    return categories;
                });
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}