using System;
using System.Collections;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace 客户端主程序.Support
{
    public static class Helper
    {
        /// <summary>
        /// 将DataSet格式化成字节数组byte[]
        /// </summary>
        /// <param name="dsOriginal">DataSet对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatData(DataSet dsOriginal)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            dsOriginal.RemotingFormat = SerializationFormat.Binary;
            brFormatter.Serialize(memStream, dsOriginal);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return binaryDataResult;
        }

        /// <summary>
        /// 将DataSet格式化成字节数组byte[]，并且已经经过压缩
        /// </summary>
        /// <param name="dsOriginal">DataSet对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatDataCompress(DataSet dsOriginal)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            dsOriginal.RemotingFormat = SerializationFormat.Binary;
            brFormatter.Serialize(memStream, dsOriginal);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return Compress(binaryDataResult);
        }

        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            byte[] bData;
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            GZipStream stream = new GZipStream(ms, CompressionMode.Decompress, true);
            byte[] buffer = new byte[1024];
            MemoryStream temp = new MemoryStream();
            int read = stream.Read(buffer, 0, buffer.Length);
            while (read > 0)
            {
                temp.Write(buffer, 0, read);
                read = stream.Read(buffer, 0, buffer.Length);
            }
            //必须把stream流关闭才能返回ms流数据,不然数据会不完整
            stream.Close();
            stream.Dispose();
            ms.Close();
            ms.Dispose();
            bData = temp.ToArray();
            temp.Close();
            temp.Dispose();
            return bData;
        }

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            byte[] bData;
            MemoryStream ms = new MemoryStream();
            GZipStream stream = new GZipStream(ms, CompressionMode.Compress, true);
            stream.Write(data, 0, data.Length);
            stream.Close();
            stream.Dispose();
            //必须把stream流关闭才能返回ms流数据,不然数据会不完整
            //并且解压缩方法stream.Read(buffer, 0, buffer.Length)时会返回0
            bData = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return bData;
        }

        /// <summary>
        /// 将字节数组反序列化成DataSet对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>DataSet对象</returns>
        public static DataSet RetrieveDataSet(byte[] binaryData)
        {
            DataSet dsOriginal = null;
            MemoryStream memStream = new MemoryStream(binaryData);
            IFormatter brFormatter = new BinaryFormatter();
            Object obj = brFormatter.Deserialize(memStream);
            dsOriginal = (DataSet)obj;
            return dsOriginal;
        }

        /// <summary>
        /// 将字节数组反解压后序列化成DataSet对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>DataSet对象</returns>
        public static DataSet RetrieveDataSetDecompress(byte[] binaryData)
        {
            DataSet dsOriginal = null;
            MemoryStream memStream = new MemoryStream(Decompress(binaryData));
            IFormatter brFormatter = new BinaryFormatter();
            Object obj = brFormatter.Deserialize(memStream);
            dsOriginal = (DataSet)obj;
            return dsOriginal;
        }

        /// <summary>
        /// 将object格式化成字节数组byte[]
        /// </summary>
        /// <param name="dsOriginal">object对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatData(object dsOriginal)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            brFormatter.Serialize(memStream, dsOriginal);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return binaryDataResult;
        }

        /// <summary>
        /// 将objec格式化成字节数组byte[]，并压缩
        /// </summary>
        /// <param name="dsOriginal">object对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatDataCompress(object dsOriginal)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            brFormatter.Serialize(memStream, dsOriginal);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return Compress(binaryDataResult);
        }

        /// <summary>
        /// 将字节数组反序列化成object对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>object对象</returns>
        public static object RetrieveObject(byte[] binaryData)
        {
            MemoryStream memStream = new MemoryStream(binaryData);
            IFormatter brFormatter = new BinaryFormatter();
            Object obj = brFormatter.Deserialize(memStream);
            return obj;
        }

        /// <summary>
        /// 将字节数组解压后反序列化成object对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>object对象</returns>
        public static object RetrieveObjectDecompress(byte[] binaryData)
        {
            MemoryStream memStream = new MemoryStream(Decompress(binaryData));
            IFormatter brFormatter = new BinaryFormatter();
            Object obj = brFormatter.Deserialize(memStream);
            return obj;
        }

        /// <summary>
        /// 将DataSet序列化成byte[]（强制压缩） 2013-09-10 guotuo
        /// </summary>
        /// <param name="ds">要序列化的DataSet</param>
        /// <returns></returns>
        public static byte[] DataSet2Byte(DataSet ds)
        {
            /* 2013-09-10 郭拓&于海滨
             * 分隔符标准：
             * 表-表：[TnTn]
             * 表-列名-数据[TnCnD]
             * 列名-列名[CnCn]
             * 行-行[RR]
             * 列-列[CC]
             */
            byte[] result = null;
            StringBuilder resultSB = new StringBuilder();
            //开始循环表名
            for (int t = 0; t < ds.Tables.Count; t++)
            {
                resultSB.Append(ds.Tables[t].TableName);
                resultSB.Append("[TnCnD]");
                //开始循环列名
                for (int c = 0; c < ds.Tables[t].Columns.Count; c++)
                {
                    if (c != 0)
                        resultSB.Append("[CnCn]");
                    resultSB.Append(ds.Tables[t].Columns[c].ColumnName);
                }
                resultSB.Append("[TnCnD]");
                //开始循环数据
                for (int i = 0; i < ds.Tables[t].Rows.Count; i++)
                {
                    if (i != 0)
                        resultSB.Append("[RR]");
                    for (int j = 0; j < ds.Tables[t].Columns.Count; j++)
                    {
                        if (j != 0)
                            resultSB.Append("[CC]");
                        resultSB.Append(ds.Tables[t].Rows[i][j].ToString());//每个单元格的数据
                    }
                }
                resultSB.Append("[TnTn]");
            }
            result = Encoding.Unicode.GetBytes(resultSB.ToString());
            result = Compress(result);
            return result;
        }
        /// <summary>
        /// 将byte[]（压缩后）序列化为DataSet 2013-09-10 guotuo
        /// </summary>
        /// <param name="b">要序列化的压缩后的byte[]</param>
        /// <returns></returns>
        public static DataSet Byte2DataSet(byte[] b)
        {
            /* 2013-09-10 郭拓&于海滨
             * 分隔符标准：
             * 表-表：[TnTn]
             * 表-列名-数据[TnCnD]
             * 列名-列名[CnCn]
             * 行-行[RR]
             * 列-列[CC]
             */
            DataSet ds = new DataSet();
            //解压Byte数组
            b = Decompress(b);

            if (b == null)
            {
                return null;
            }

            //获取源串
            string strResource = Encoding.Unicode.GetString(b);

            string[] Tables = strResource.Split(new string[] { "[TnTn]" }, StringSplitOptions.RemoveEmptyEntries);
            for (int TablesCount = 0; TablesCount < Tables.Length; TablesCount++)
            {
                DataTable dt = new DataTable();//循环出来的第一个数据
                string[] TableInfo = Tables[TablesCount].Split(new string[] { "[TnCnD]" }, StringSplitOptions.None);//单个数据表的源串
                dt.TableName = TableInfo[0];//表名

                string[] ColumsInfo = TableInfo[1].Split(new string[] { "[CnCn]" }, StringSplitOptions.None);//列名
                for (int ColumnsCount = 0; ColumnsCount < ColumsInfo.Length; ColumnsCount++)
                {
                    dt.Columns.Add(ColumsInfo[ColumnsCount]);
                }//到这里已经完成单个DataTable架构的初始化

                //开始插入表数据
                if (TableInfo.Length >= 3)
                {
                    string[] DataInfo = TableInfo[2].Split(new string[] { "[RR]" }, StringSplitOptions.None);
                    for (int RowsCount = 0; RowsCount < DataInfo.Length; RowsCount++)
                    {
                        dt.Rows.Add(DataInfo[RowsCount].Split(new string[] { "[CC]" }, StringSplitOptions.None));
                    }
                }
                ds.Tables.Add(dt);
            }



            return ds;
        }



        /// <summary>
        /// 将byte[]（压缩后）序列化为DataSet 2013-09-11 于海滨复制修改
        /// </summary>
        /// <param name="b">要序列化的压缩后的byte[]</param>
        /// <param name="HTspColumns">要特殊设置的列类型key是列名，值是类型标记（例如System.Int32）</param>
        /// <returns></returns>
        public static DataSet Byte2DataSet(byte[] b, Hashtable HTspColumns)
        {
            try
            {
                /* 2013-09-10 郭拓&于海滨
                 * 分隔符标准：
                 * 表-表：[TnTn]
                 * 表-列名-数据[TnCnD]
                 * 列名-列名[CnCn]
                 * 行-行[RR]
                 * 列-列[CC]
                 */
                DataSet ds = new DataSet();
                //解压Byte数组
                b = Decompress(b);
                //获取源串
                string strResource = Encoding.Unicode.GetString(b);

                string[] Tables = strResource.Split(new string[] { "[TnTn]" }, StringSplitOptions.RemoveEmptyEntries);
                for (int TablesCount = 0; TablesCount < Tables.Length; TablesCount++)
                {
                    DataTable dt = new DataTable();//循环出来的第一个数据
                    string[] TableInfo = Tables[TablesCount].Split(new string[] { "[TnCnD]" }, StringSplitOptions.None);//单个数据表的源串
                    dt.TableName = TableInfo[0];//表名

                    string[] ColumsInfo = TableInfo[1].Split(new string[] { "[CnCn]" }, StringSplitOptions.None);//列名
                    for (int ColumnsCount = 0; ColumnsCount < ColumsInfo.Length; ColumnsCount++)
                    {
                        dt.Columns.Add(ColumsInfo[ColumnsCount]);
                        if (HTspColumns.ContainsKey(ColumsInfo[ColumnsCount]))
                        {
                            dt.Columns[ColumnsCount].DataType = Type.GetType(HTspColumns[ColumsInfo[ColumnsCount]].ToString());
                            dt.Columns[ColumnsCount].AllowDBNull = true;
                        }
                    }//到这里已经完成单个DataTable架构的初始化

                    //开始插入表数据
                    if (TableInfo.Length >= 3)
                    {
                        if (!string.IsNullOrEmpty(TableInfo[2]))
                        {
                            string[] DataInfo = TableInfo[2].Split(new string[] { "[RR]" }, StringSplitOptions.None);
                            for (int RowsCount = 0; RowsCount < DataInfo.Length; RowsCount++)
                            {
                                //dt.Rows.Add(DataInfo[RowsCount].Split(new string[] { "[CC]" }, StringSplitOptions.None));
                                string[] ss = DataInfo[RowsCount].Split(new string[] { "[CC]" }, StringSplitOptions.None);

                                object[] ob = new object[ss.Length];
                                for (int i = 0; i < ss.Length; i++)
                                {
                                    if (string.IsNullOrEmpty(ss[i].ToString()))
                                    {
                                        ob[i] = DBNull.Value;
                                    }
                                    else
                                    {
                                        ob[i] = ss[i].ToString();
                                    }
                                }
                                dt.Rows.Add(ob);
                            }
                        }
                        
                    }
                    ds.Tables.Add(dt);
                }



                return ds;
            }
            catch(Exception e)
            {
                return null;
            }
        }


    }
}
