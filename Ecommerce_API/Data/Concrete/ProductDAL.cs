using Ecommerce_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;

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
                                imgUrl = Convert.ToString(row["img"]),
                                quantity = Convert.ToInt32(row["quantity"])
                            });
                    }
                    return products;
                });
            }
            catch (Exception ex)
            {
                Logger.log(ex);
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
                Logger.log(ex);
                throw ex;
            }
        }

        public List<ProductModel> getProductByCategory(string category)
        {
            try
            {
                string storedProcedure = "ProductsByCategory";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    List<ProductModel> products = new List<ProductModel>();

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows) {
                        products.Add(new ProductModel
                        {
                            ProductId = Convert.ToInt32(row["product_id"]),
                            ProductName = Convert.ToString(row["product_name"]),
                            price = Convert.ToInt32(row["price"]),
                            imgUrl = Convert.ToString(row["img"]),
                            quantity = Convert.ToInt32(row["quantity"])
                        });
                    }
                    return products;
                    }, 
                    new SqlParameter("@category", category)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public ProductModel getProductByID(int id) {
            try
            {
                string storedProcedure = "ProductsByID";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ProductModel product = new ProductModel();
                        reader.Read();
                        product.ProductId = Convert.ToInt32(reader["product_id"]);
                        product.ProductName = Convert.ToString(reader["product_name"]);
                        product.price = Convert.ToInt32(reader["price"]);
                        product.imgUrl = Convert.ToString(reader["img"]);
                        product.quantity = Convert.ToInt32(reader["quantity"]);

                        return product;
                    }
                    return null;
                }, new SqlParameter("@id", id));
            }
            catch (Exception ex) {
                Logger.log(ex);
                throw ex; 
            }
        }

        public List<ProductModel> searchProduct(string search)
        {
            try
            {
                string storedProcedure = "SearchProduct";
                List<ProductModel> products = new List<ProductModel>();

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            products.Add(new ProductModel {
                                ProductId = Convert.ToInt32(reader["product_id"]),
                                ProductCategory = Convert.ToString(reader["category"]),
                                ProductName = Convert.ToString(reader["product_name"]),
                                price = Convert.ToInt32(reader["price"]),
                                imgUrl = Convert.ToString(reader["img"]),
                                quantity = Convert.ToInt32(reader["quantity"])
                            });
                        }
                    }
                    return products;
                }, 
                new SqlParameter("@searchTerm", search)
                );
            }
            catch (Exception ex) { 
                Logger.log(ex);
                throw ex;
            }
        }
    }
}