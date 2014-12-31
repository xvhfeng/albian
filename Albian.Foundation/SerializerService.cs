using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Albian.Foundation
{
    public class SerializerService
    {
        public static byte[] Serialize(object obj)
        {
            if (null == obj) return null;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, obj);
                stream.Position = 0L;
                byte[] buffer = stream.ToArray();
                return buffer;
            }
        }

        public static object Deserialize(byte[] data)
        {
            if ((data == null) || (data.Length == 0))
            {
                return null;
            }
            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                object obj = binaryFormatter.Deserialize(stream);
                return obj;
            }
        }

    }
}
