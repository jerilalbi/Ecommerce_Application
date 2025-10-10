using Ecommerce_API.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
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
                        string name = Convert.ToString(reader["name"]);
                        string email = Convert.ToString(reader["email"]);
                        string address = Convert.ToString(reader["address"]);
                        string phone = Convert.ToString(reader["phone"]);
                        string role = Convert.ToString(reader["role"]);
                        string passwordHash = Convert.ToString(reader["PasswordHash"]);
                        string passwordSalt = Convert.ToString(reader["PasswordSalt"]);

                        bool isPasswordMatch = passwordHelper.verifyPasswordHash(login.password,passwordHash,passwordSalt);

                        if (isPasswordMatch)
                        {
                            return new UserModel { 
                                Token = jWTHelper.GenerateToken(name, email, role),
                                UserId = userId,
                                Name = name,
                                Email = email,
                                Address = address,
                                Phone = phone,
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
                        string address = Convert.ToString(reader["address"]);
                        string phone = Convert.ToString(reader["phone"]);

                            return new UserModel
                            {
                                UserId = user.UserId,
                                Name = user.Name,
                                Email = user.Email,
                                Address = address,
                                Phone = phone,
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
    }
}