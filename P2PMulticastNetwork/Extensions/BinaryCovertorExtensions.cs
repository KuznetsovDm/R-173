using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace P2PMulticastNetwork.Extensions
{
    public static class BinaryCovertorExtensions
    {
        private static BinaryFormatter _formatter = new BinaryFormatter();

        public static bool TrySerialize<T>(this T obj, out byte[] result)
        {
            try
            {
                result = obj.Serialize();
                return true;
            }
            catch (Exception ex)
            {
                result = Enumerable.Empty<byte>().ToArray();
                return false;
            }
        }

        public static bool TryDeserialize<T>(this byte[] data, out T result)
        {
            try
            {
                result = data.Deserialize<T>();
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        public static byte[] Serialize<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(this byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                var model = (T)_formatter.Deserialize(ms);
                return model;
            }
        }
    }
}
