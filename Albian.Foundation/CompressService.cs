using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Aced.Compression;
using Albian.Foundation;

namespace Albian.Foundation.Compression
{
    /// <summary>
    /// 压缩类
    /// </summary>
    public class CompressService
    {
        public static MemoryStream Compress(Stream inputStream)
        {
            MemoryStream stream = new MemoryStream();
            using (GZipStream stream2 = new GZipStream(stream, CompressionMode.Compress, true))
            {
                int BufferSize = 0x1000;
                int num;
                byte[] buffer = new byte[BufferSize];
                while ((num = inputStream.Read(buffer, 0, BufferSize)) > 0)
                {
                    stream2.Write(buffer, 0, num);
                }
            }
            stream.Seek(0L, SeekOrigin.Begin);
            return stream;
        }

        public static MemoryStream Decompress(Stream inputStream)
        {
            MemoryStream stream = new MemoryStream();
            using (GZipStream stream2 = new GZipStream(inputStream, CompressionMode.Decompress, true))
            {
                int num;
                int BufferSize = 0x1000;
                byte[] buffer = new byte[BufferSize];
                while ((num = stream2.Read(buffer, 0, BufferSize)) > 0)
                {
                    stream.Write(buffer, 0, num);
                }
            }
            stream.Seek(0L, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="text">需要压缩的字符串</param>
        /// <remarks>如果压缩和解压缩不能对应，请注意编码问题，默认使用UTF-8编码</remarks>
        /// <returns></returns>
        public static string Compress(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="compressedText">需要解压的字符串</param>
        /// <remarks>如果压缩和解压缩不能对应，请注意编码问题，默认使用UTF-8编码</remarks>
        /// <returns></returns>
        public static string Decompress(string compressedText)
        {
            if (string.IsNullOrEmpty(compressedText))
                return compressedText;

            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
       
        /// <summary>
        /// 压缩对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] CompressObjectToBytes(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, obj);
                stream.Position = 0L;
                MemoryStream stream2 = Compress(stream);
                byte[] buffer = stream2.ToArray();
                stream2.Close();
                return buffer;
            }
        }

        /// <summary>
        /// 解压对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object DecompressToObject(byte[] data)
        {
            if ((data == null) || (data.Length == 0))
            {
                return null;
            }
            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream serializationStream = Decompress(stream);
                object obj2 = binaryFormatter.Deserialize(serializationStream);
                serializationStream.Close();
                return obj2;
            }
        }

        /// <summary>
        /// 将对象压缩成16进制字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string CompressObject2Hex(object obj)
        {
            return ConverService.Bytes2Hex(CompressObjectToBytes(obj));
        }
        /// <summary>
        /// 将16进制字符串解压缩成对象
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static object DecompressHex2Object(string hexString)
        {
            return DecompressToObject(ConverService.Hex2Bytes(hexString));
        }

        public static byte[] MemoryCompress(object obj, AcedCompressionLevel level)
        {
            byte[] objs = SerializerService.Serialize(obj);
            AcedDeflator instance = new AcedDeflator();
            byte[] bytes = instance.Compress(objs, 0, objs.Length, level, 0, 0);
            return bytes;
        }

        public static object MemoryDecompress(byte[] bytes)
        {
            AcedInflator instance = new AcedInflator();
            byte[] objs = instance.Decompress(bytes, 0, 0, 0);
            return SerializerService.Deserialize(objs);
        }

        public static string CompressMemory2Hex(object obj, AcedCompressionLevel level)
        {
            if (null == obj)
            {
                return string.Empty;
            }
            return ConverService.Bytes2Hex(MemoryCompress(obj, level));
        }

        public static object DecompressHex2Memory(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return MemoryDecompress(ConverService.Hex2Bytes(value));
        }
    }
}