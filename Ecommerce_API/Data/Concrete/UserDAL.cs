using Ecommerce_API.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

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
                throw ex;
            }
        }

        public string LoginUser(LoginModel login)
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
                        string name = Convert.ToString(reader["name"]);
                        string email = Convert.ToString(reader["email"]);
                        string role = Convert.ToString(reader["role"]);
                        string passwordHash = Convert.ToString(reader["PasswordHash"]);
                        string passwordSalt = Convert.ToString(reader["PasswordSalt"]);

                        bool isPasswordMatch = passwordHelper.verifyPasswordHash(login.password,passwordHash,passwordSalt);

                        if (isPasswordMatch)
                        {
                            return jWTHelper.GenerateToken(name, email, role);
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return "";
                    }
                },
                new SqlParameter("@email", login.email)
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}