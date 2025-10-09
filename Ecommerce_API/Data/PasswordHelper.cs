using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Ecommerce_API.Data
{
    public class PasswordHelper
    {
        public (string passwordHash, string passwordSalt) createPasswordHash(string password)
        {
            byte[] saltByte = new byte[16];
            using(var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltByte);
            }

            using(var pdkf = new Rfc2898DeriveBytes(password,saltByte, 10000))
            {
                byte[] hashByte = pdkf.GetBytes(20);
                return (Convert.ToBase64String(hashByte), Convert.ToBase64String(saltByte));
            }
        }

        public bool verifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            byte[] saltByte = Convert.FromBase64String(storedSalt);
            using(var pdkf = new Rfc2898DeriveBytes(password, saltByte, 10000))
            {
                byte[] hashByte = pdkf.GetBytes(20);
                return Convert.ToBase64String(hashByte) == storedHash;
            }
        }
    }
}