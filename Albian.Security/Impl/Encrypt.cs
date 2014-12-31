using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Albian.Security.Impl
{
    public class Encrypt : FreeEncrypt
    {
        private readonly string _iv;
        private readonly string _key;
        private SymmetricAlgorithm _des;

        public Encrypt()
        {
            _key = "!@#$%^&*AlbianBySeapeak*&^%$#@!";
            _iv = "*&^%$#@!AlbianBySeapeak!@#$%^&*";
        }

        public Encrypt(string key, string iv)
        {
            _key = key;
            _iv = iv;
        }

        protected override void GetKey()
        {
            CreateKey();
            CreateIV();
        }

        private void CreateKey()
        {
            _des = new DESCryptoServiceProvider();
            var byt1 = new byte[8] {60, 61, 62, 63, 64, 65, 66, 67};
            byte[] byt2 = Encoding.UTF8.GetBytes(_key);

            int j = 0;
            for (int i = byt2.Length - 1; i >= 0; i--)
            {
                byt1[j] = byt2[i];
                if (j == 7)
                {
                    break;
                }
                j++;
            }

            _des.Key = byt1;
        }

        private void CreateIV()
        {
            var byt1 = new byte[8] {60, 61, 62, 63, 64, 65, 66, 67};
            byte[] byt2 = Encoding.UTF8.GetBytes(_iv);

            int j = 0;
            for (int i = byt2.Length - 1; i >= 0; i--)
            {
                byt1[j] = byt2[i];
                if (j == 7)
                {
                    break;
                }
                j++;
            }

            _des.IV = byt1;
        }

        public static string EncryptString(string key, string iv, string value)
        {
            var objEncrypt = new Encrypt(key, iv);
            return objEncrypt.EncryptString(value);
        }

        public static string DecryptString(string key, string iv, string value)
        {
            var objEncrypt = new Encrypt(key, iv);
            return objEncrypt.DecryptString(value);
        }

        public override string EncryptString(string value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            try
            {
                GetKey();

                ct = _des.CreateEncryptor(_des.Key, _des.IV);

                byt = Encoding.UTF8.GetBytes(value);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();

                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }

        public override string EncryptString(string value, int deepth)
        {
            if (deepth <= 0)
            {
                throw new ArgumentException("deepth is invalid.");
            }
            if (deepth == 1)
            {
                return EncryptString(value);
            }
            return EncryptString(EncryptString(value, deepth - 1));
        }

        public override string DecryptString(string value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            try
            {
                GetKey();

                ct = _des.CreateDecryptor(_des.Key, _des.IV);

                byt = Convert.FromBase64String(value);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();

                cs.Close();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }


        public override string DecryptString(string value, int deepth)
        {
            if (deepth <= 0)
            {
                throw new ArgumentException("deepth is invalid.");
            }
            return deepth == 1 ? DecryptString(value) : DecryptString(DecryptString(value, deepth - 1));
        }


        /// <summary>
        /// MD5加密字符串.
        /// </summary>
        /// <param name="input">需要加密的字符串.</param>
        /// <param name="bit">加密的位数.</param>
        /// <param name="isUpper">加密的结果是否需要大写.</param>
        /// <returns>经过MD5加密的字符串</returns>
        public override string MD5(string input, MD5Mode bit, bool isUpper)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(input));
            var stringBuilder = new StringBuilder();

            switch (bit)
            {
                case MD5Mode.Low:
                    {
                        for (int i = 4; i <= 11; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        break;
                    }
                case MD5Mode.High:
                    {
                        for (int i = 0; i <= 15; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        break;
                    }
            }
            if (isUpper)
                return stringBuilder.ToString().ToUpper();
            return stringBuilder.ToString().ToLower();
        }

        /// <summary>
        /// MD5加密字符串.
        /// </summary>
        /// <param name="input">需要加密的字符串.</param>
        /// <param name="bit">加密的位数.</param>
        /// <param name="isUpper">加密的结果是否需要大写.</param>
        /// <returns>经过MD5加密的字符串</returns>
        public override string MD5(string input, MD5Mode bit)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(input));
            var stringBuilder = new StringBuilder();

            switch (bit)
            {
                case MD5Mode.Low:
                    {
                        for (int i = 4; i <= 11; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        break;
                    }
                case MD5Mode.High:
                    {
                        for (int i = 0; i <= 15; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        break;
                    }
            }
            return stringBuilder.ToString();
        }

        public override string SHA1(string value)
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