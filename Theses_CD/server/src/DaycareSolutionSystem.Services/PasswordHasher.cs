using System;
using System.Security.Cryptography;
using System.Text;

namespace DaycareSolutionSystem.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            var sha256 = SHA256.Create();

            byte[] data = sha256.ComputeHash(utf8.GetBytes(password));

            return Convert.ToBase64String(data);
        }
    }
}
