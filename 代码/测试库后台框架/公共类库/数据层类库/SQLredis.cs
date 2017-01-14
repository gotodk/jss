using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FMDBHelperClass
{

    /// <summary>
    /// 缓存dll实例的类
    /// </summary>
    public class InstanceCachesEx
    {
        public byte[] ConvertStreamToByteBuffer(System.IO.Stream theStream)
        {
            int b1;
            System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
            while ((b1 = theStream.ReadByte()) != -1)
            {
                tempStream.WriteByte(((byte)b1));
            }
            return tempStream.ToArray();
        }
        private static readonly object obj = new object();


        private Hashtable htEx = new Hashtable();

        public Assembly InstanceCache(string key)
        {


            if (htEx.ContainsKey(key))
            {
                return (Assembly)(htEx[key]);
            }
            else
            {
                Byte[] content = null;
                FileStream f = new FileStream(key, FileMode.Open, FileAccess.Read, FileShare.Read);
                int b1;
                System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
                while ((b1 = f.ReadByte()) != -1)
                {
                    tempStream.WriteByte(((byte)b1));
                }
                f.Close();

                content = tempStream.ToArray();
                Assembly valuenew = Assembly.Load(content);

                lock (obj)
                {
                    if (!htEx.ContainsKey(key))
                    {
                        htEx[key] = valuenew;
                    }

                }



                return valuenew;
            }
        }

    }

    public static class GetCache
    {
        static GetCache()
        {
            InstanceCacheEx = new InstanceCachesEx();
        }

        public static InstanceCachesEx InstanceCacheEx { get; set; }
    }

    class SQLredis
    {
    }
}
