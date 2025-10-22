using Ecommerce_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO;

namespace Ecommerce_API.Data.Concrete
{
    public class AdminDAL : DBHelper
    {
        public List<SalesCategoryModel> getSalesByCategory()
        {
            try
            {
                string storedProcedure = "SalesByCategory";
                List<SalesCategoryModel> sales = new List<SalesCategoryModel>();

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                        {
                            sales.Add(new SalesCategoryModel
                            {
                                SalesCategory = Convert.ToString(reader["category"]),
                                SalesPrice = Convert.ToInt32(reader["price"])
                            });
                        }
                    }
                    return sales;
                });
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public List<SalesMonthModel> getSalesByMonth()
        {
            try
            {
                string storedProcedure = "SalesByMonth";
                List<SalesMonthModel> sales = new List<SalesMonthModel>();

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sales.Add(new SalesMonthModel
                            {
                                SalesDate = Convert.ToDateTime(reader["Order_date"]),
                                SalesAmount = Convert.ToInt32(reader["Total_amount"])
                            });
                        }
                    }
                    return sales;
                });
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public List<TopProductsModel> getTopSellingProducts()
        {
            try
            {
                string storedProcedure = "TopSellingProduct";
                List<TopProductsModel> topProducts = new List<TopProductsModel>();

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topProducts.Add(new TopProductsModel
                            {
                                ProductName = Convert.ToString(reader["product_name"]),
                                ProductCount = Convert.ToInt32(reader["product_count"])
                            });
                        }
                    }
                    return topProducts;
                });
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public List<StocksModel> getLowStockProduct()
        {
            try
            {
                string storedProcedure = "GetLowStockProducts";
                List<StocksModel> stockData = new List<StocksModel>();

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stockData.Add(new StocksModel
                            {
                                ProductName = Convert.ToString(reader["product_name"]),
                                Quantity = Convert.ToInt32(reader["quantity"])
                            });
                        }
                    }
                    return stockData;
                });
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public List<AllUserModel> getAllUsers()
        {
            try
            {
                string storedProcedure = "GetAllUsers";
                List<AllUserModel> allUsers = new List<AllUserModel>();

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allUsers.Add(new AllUserModel
                            {
                                Name = Convert.ToString(reader["name"]),
                                Email = Convert.ToString(reader["email"]),
                                Role = Convert.ToString(reader["role"]),
                            });
                        }
                    }
                    return allUsers;
                });
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int makeUserAdmin(AllUserModel userModel)
        {
            try
            {
                string storedProcedure = "MakeAdmin";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@email", userModel.Email)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int demoteAdmin(AllUserModel userModel)
        {
            try
            {
                string storedProcedure = "DemoteAdmin";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@email", userModel.Email)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int deleteUser(AllUserModel userModel)
        {
            try
            {
                string storedProcedure = "DeleteUser";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@email", userModel.Email)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int addProduct(ProductModel product, HttpPostedFile file)
        {
            try
            {

                string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/ProductImages/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileExtension = Path.GetExtension(file.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(folderPath, fileName);
                string relativePath = "/Uploads/ProductImages/" + fileName;

                if(File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                file.SaveAs(filePath);

                string storedProcedure = "AddProduct";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@category",product.ProductCategory),
                new SqlParameter("@price", product.price),
                new SqlParameter("@name", product.ProductName),
                new SqlParameter("@img",relativePath),
                new SqlParameter("@stock", product.quantity)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int updateProduct(ProductModel product)
        {
            try
            {
                string storedProcedure = "UpdateProduct";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@product_id", product.ProductId),
                new SqlParameter("@category", product.ProductCategory),
                new SqlParameter("@price", product.price),
                new SqlParameter("@name", product.ProductName),
                new SqlParameter("@img", product.imgUrl),
                new SqlParameter("@stock", product.quantity)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int deleteProduct(ProductModel product)
        {
            try
            {
                string storedProcedure = "DeleteProduct";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@product_id", product.ProductId)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public List<ViewOrdersAdminModel> GetAllOrdersAdmins()
        {
            try
            {
                string storedProcedure = "ViewOrdersAdmin";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    List<ViewOrdersAdminModel> orders = new List<ViewOrdersAdminModel>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new ViewOrdersAdminModel
                            {
                                DeliveryId = Convert.ToInt32(reader["delivery_id"]),
                                ProductName = Convert.ToString(reader["product_name"]),
                                Quantity = Convert.ToInt32(reader["Quantity_Sold"]),
                                Name = Convert.ToString(reader["Name"]),
                                Address = Convert.ToString(reader["address"]),
                                Status = Convert.ToString(reader["status"]),
                            });
                        }
                        return orders;
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw;
            }
        }

        public int ShipOrder(int DeliveryID)
        {
            try
            {
                string storedProcedure = "ShipOrderAdmin";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                }, new SqlParameter("@DeliveryItemID", DeliveryID));
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw;
            }
        }
    }
}