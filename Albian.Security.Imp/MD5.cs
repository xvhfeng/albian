using System.Security.Cryptography;
using System.Text;

namespace Albian.Security.Imp
{
    public class MD5
    {
        /// <summary>
        /// MD5加密字符串.
        /// </summary>
        /// <param name="input">需要加密的字符串.</param>
        /// <param name="bit">加密的位数.</param>
        /// <param name="isUpper">加密的结果是否需要大写.</param>
        /// <returns>经过MD5加密的字符串</returns>
        public static string Encrypt(string input, MD5Mode bit, bool isUpper)
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
    }
}