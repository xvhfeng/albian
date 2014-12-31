using System;
using System.Security.Cryptography;
using System.Text;

namespace Albian.Security.Imp
{
    public class SHA1
    {
        public static string Encrypt(string value)
        {
            var sha = new SHA1CryptoServiceProvider();
            byte[] byt;
            string encryptString;

            try
            {
                byt = Encoding.UTF8.GetBytes(value);
                encryptString = Convert.ToBase64String(sha.ComputeHash(byt));
            }
            catch
            {
                throw new Exception("加密失败!");
            }

            return encryptString;
        }
    }
}