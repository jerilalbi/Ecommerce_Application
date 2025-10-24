using Ecommerce_API.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Ecommerce_API.Data.Concrete
{
    public class UserDAL : DBHelper
    {
        public int RegisterUser(RegisterModel register)
        {
            try
            {
                string storedProcedure = "RegisterUser";
                PasswordHelper passwordHelper = new PasswordHelper();
                (string passwordHash, string passwordSalt) = passwordHelper.createPasswordHash(register.Password);

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                 new SqlParameter("@email", register.Email),
                 new SqlParameter("@passwordHash", passwordHash),
                 new SqlParameter("@passwordSalt", passwordSalt),
                 new SqlParameter("@name", register.Name),
                 new SqlParameter("@phone", register.Phone),
                 new SqlParameter("@dob", register.Dob),
                 new SqlParameter("@gender", register.Gender)
                );

            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public UserModel LoginUser(LoginModel login)
        {
            try
            {
                string storedProcedure = "LoginUser";
                PasswordHelper passwordHelper = new PasswordHelper();
                JWTHelper jWTHelper = new JWTHelper();

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        int userId = Convert.ToInt32(reader["User_ID"]);
                        string role = Convert.ToString(reader["role"]);
                        string name = Convert.ToString(reader["name"]);
                        string email = Convert.ToString(reader["email"]);
                        string userStatus = Convert.ToString(reader["status"]);
                        string passwordHash = Convert.ToString(reader["PasswordHash"]);
                        string passwordSalt = Convert.ToString(reader["PasswordSalt"]);

                        bool isPasswordMatch = passwordHelper.verifyPasswordHash(login.password,passwordHash,passwordSalt);

                        if (isPasswordMatch)
                        {
                            if(string.Equals(userStatus, "active", StringComparison.OrdinalIgnoreCase))
                            {
                                UpdateUserLastLogin(userId);
                            }

                            return new UserModel
                            {
                                Token = jWTHelper.GenerateToken(name, email, role),
                                UserId = userId,
                                Role = role,
                                Status = userStatus,
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                },
                new SqlParameter("@email", login.email)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        private int UpdateUserLastLogin(int userId)
        {
            try
            {
                string storedProcedure = "UpdateLastLogin";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@UserId", userId)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public UserModel getUserDetails(UserModel user)
        {
            try
            {
                string storedProcedure = "UserDetails";

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();

                            return new UserModel
                            {
                                UserId = Convert.ToInt32(reader["User_ID"]),
                                Name = Convert.ToString(reader["name"]),
                                Email = Convert.ToString(reader["email"]),
                                Address = Convert.ToString(reader["address"]),
                                Phone = Convert.ToString(reader["phone"]),
                                ImgUrl = Convert.ToString(reader["ProfileImg"]),
                            };
                    }
                    else
                    {
                        return null;
                    }
                },
                new SqlParameter("@userId", user.UserId)
                );
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public int updateUserAddress(UserModel user)
        {
            try
            {
                string storedProcedure = "UpdateAddress";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery();
                },
                new SqlParameter("@userId", user.UserId),
                new SqlParameter("@address",user.Address)
                );
            }catch(Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public string UpdateUserProfileImg(int userId, HttpPostedFile file)
        {
            try
            {
                if(file == null || file.ContentLength == 0)
                {
                    throw new ArgumentException("No file uploaded");
                }

                string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/ProfileImages/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileExtension = Path.GetExtension(file.FileName);
                string fileName = $"user_{userId}{fileExtension}";
                string fullPath = Path.Combine(folderPath, fileName);
                string relativePath = "/Uploads/ProfileImages/" + fileName;

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                file.SaveAs(fullPath);

                string storedProcedure = "UpdateUserProfileImg";

                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    int result = cmd.ExecuteNonQuery();
                    if(result != 0)
                    {
                        return relativePath;
                    }
                    else
                    {
                        return "";
                    }
                },
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ImgPath", relativePath)
                );

            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        public List<string> GetPerviousAddress(int userId)
        {
            try
            {
                string storedProcedure = "PreviousAddress";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    List<string> previousAddress = new List<string>();
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())
                        {
                            previousAddress.Add(Convert.ToString(reader["address"]));
                        }
                        return previousAddress;
                    }
                }, new SqlParameter("@UserId", userId));
            }
            catch (Exception ex) {
                Logger.log(ex);
                throw ex;
            }
        }

        public int deleteAccount(UserModel userModel)
        {
            try
            {
                string storedProcedure = "DeleteUser";
                IPrincipal user = Thread.CurrentPrincipal;
                var identity = user.Identity as ClaimsIdentity;
                string email = identity.FindFirst(ClaimTypes.Email).Value;

                if (userModel.Email == email)
                {
                    return ExecuteSQL(storedProcedure, cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        return cmd.ExecuteNonQuery();
                    },
                    new SqlParameter("@email", userModel.Email)
                    );
                }
                else
                {
                    return 0;
                }  
            }
            catch (Exception ex)
            {
                Logger.log(ex);
                throw ex;
            }
        }

        private PasswordModel GetPasswordHash(int userId)
        {
            try
            {
                string storedProcedure = "GetPasswordHash";
                return ExecuteSQL(storedProcedure, cmd =>
                {
                cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) {
                            return new PasswordModel { 
                                PasswordHash = Convert.ToString(reader["PasswordHash"]),
                                PasswordSalt = Convert.ToString(reader["PasswordSalt"])
                            };
                        }
                    }
                    return null;
                }, new SqlParameter("@UserId", userId));
            }
            catch (Exception ex) {
                Logger.log(ex);
                throw ex;
            }
        }

        public int ChangePassword(string oldPassword, string newPassword, int userId)
        {
            PasswordHelper passwordHelper = new PasswordHelper();
            PasswordModel password = GetPasswordHash(userId);
            if(password != null)
            {
                bool isPasswordMatch = passwordHelper.verifyPasswordHash(oldPassword, password.PasswordHash, password.PasswordSalt);

                if (isPasswordMatch) {
                    (string passwordHash, string passwordSalt) = passwordHelper.createPasswordHash(newPassword);
                    string storedProcedure = "ChangePassword";

                    return ExecuteSQL(storedProcedure, cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        return cmd.ExecuteNonQuery();
                    }, 
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@PasswordHash", passwordHash),
                    new SqlParameter("@PasswordSalt", passwordSalt)
                    );
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}