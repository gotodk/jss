using System;
using System.IO.Compression;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace 公用通讯协议类库.共用类库
{
    /// <summary>
    /// 序列化与反序列化消息类
    /// </summary>
    public class FormatterHelper
    {
        /// <summary>
        /// 序列化实例
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(object obj)
        {
            BinaryFormatter binaryF = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(1024 * 10);
            binaryF.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[(int)ms.Length];
            ms.Read(buffer, 0, buffer.Length);
            ms.Close();
            buffer = Compression(buffer, CompressionMode.Compress);
            return buffer;
        }
        /// <summary>
        /// 反序列化实例
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static object Deserialize(byte[] buffer)
        {
            buffer = Compression(buffer, CompressionMode.Decompress);
            BinaryFormatter binaryF = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length, false);
            object obj = binaryF.Deserialize(ms);
            ms.Close();
            return obj;
        }

        public static byte[] Compression(byte[] data, CompressionMode mode)
        {
            DeflateStream zip = null;
            try
            {
                if (mode == CompressionMode.Compress)
                {
                    MemoryStream ms = new MemoryStream();
                    zip = new DeflateStream(ms, mode, true);
                    zip.Write(data, 0, data.Length);
                    zip.Close();
                    return ms.ToArray();
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Write(data, 0, data.Length);
                    ms.Flush();
                    ms.Position = 0;
                    zip = new DeflateStream(ms, mode, true);
                    MemoryStream os = new MemoryStream();
                    int SIZE = 1024;
                    byte[] buf = new byte[SIZE];
                    int l = 0;
                    do
                    {
                        l = zip.Read(buf, 0, SIZE);
                        if (l == 0) l = zip.Read(buf, 0, SIZE);
                        os.Write(buf, 0, l);
                    }
                    while (l != 0);
                    zip.Close();
                    return os.ToArray();
                }
            }
            catch
            {
                if (zip != null) zip.Close();
                return null;
            }
            finally
            {
                if (zip != null) zip.Close();
            }
        }

    }
}
