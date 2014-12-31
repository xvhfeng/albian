using System;
using System.Collections.Generic;
using System.Text;

namespace Albian.Foundation
{
    public class ConverService
    {
        /// <summary>
        /// 16进制转换成字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Bytes2Hex(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hex = string.Empty;
            if (bytes != null)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("X2"));
                }
                hex = sb.ToString();
            }
            return hex;
        }

        /// <summary>
        /// 16进制字符串转换成byte数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] Hex2Bytes(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return null;

            byte[] returnBytes = new byte[hex.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
