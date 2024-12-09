using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library_Otomation
{
    // Şifreyi hash'lemek için kullanılan yardımcı sınıf / Helper class used for hashing passwords
    public static class PasswordHelper
    {
        // Verilen şifreyi hash'ler / Hashes the given password
        public static string HashPassword(string password)
        {
            // SHA256 algoritmasını kullanarak hash oluşturma / Creating hash using SHA256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Şifreyi byte dizisine dönüştürme ve hash'leme / Convert password to byte array and hash it
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                // Her byte'ı hexadecimal formatında ekleme / Append each byte in hexadecimal format
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                // Hashlenmiş şifreyi döndürme / Return the hashed password
                return builder.ToString();
            }
        }
    }
}
