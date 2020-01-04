using System;
using System.Security.Cryptography;
using System.Text;

namespace DaycareSolutionSystem.Helpers
{
    public class PasswordHasher
    {
        public string HashPassword(string password)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            var md5 = new MD5CryptoServiceProvider();

            byte[] data = md5.ComputeHash(utf8.GetBytes(password));

            return Convert.ToBase64String(data);
        }
    }
}
