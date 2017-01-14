using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using FMDBHelperClass;
using System.Data;
using FMPublicClass;

namespace RedisReLoad.ShareDBcache
{
    /// <summary>
    /// 这个类用于redis中信用积分对照表的读写
    /// edit by zzf 2014.11
    /// </summary>
    public class XYDZBRedis
    {
        /// <summary>
        /// 根据编号返回对照表中的一行数据
        /// </summary>
        /// <param name="number">对照表的主键</param>
        /// <returns></returns>
        public static DataTable Redis_GetXYBDMXDZB(string number)
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);
               
                byte[] bs=null;

                lock (RedisClass.LockObj)
                {
                    //从redis中读取
                    bs = RC.HGet("H:XYBDMXDZB", Encoding.UTF8.GetBytes(number));

                }               
                if (bs!=null && bs.Length>0)
                {
                    
                    //反序列化
                    DataTable dt = StringOP.Deserialize<DataTable>(bs); 
                    return dt;
                }
                else
                {
                    return null; 
                }            

            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 向redis中写数据
        /// </summary>
        /// <param name="number">对照表的主键</param>
        /// <param name="dt">参数数据表</param>
        /// <returns></returns>
        public static bool Redis_SetXYBDMXDZB(string number, DataTable dt)
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);

                byte[] bs=StringOP.Serialize<DataTable>(dt);

                lock (RedisClass.LockObj)
                {
                    //向redis中写入数据，返回值1表示新的值，0表示覆盖
                    long l = RC.HSet("H:XYBDMXDZB", Encoding.UTF8.GetBytes(number), bs);
                    if (l == 0 || l==1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch
            {
                return false;
            }
 
        }

        /// <summary>
        /// 获取整个对照表，存放到DataTable中
        /// </summary>
        /// <returns></returns>
        public static DataTable Redis_GetAllXYDZB()
        {
            try
            {
               RedisClient RC = RedisClass.GetRedisClient(null);

               byte[][] bes = null;
               lock (RedisClass.LockObj)
               {
                   bes = RC.HVals("H:XYBDMXDZB");
               }            

               DataTable dt = new DataTable();

               for (int i = 0; i < bes.Length; i++)
               {
                   byte[] bs = bes[i];

                   try
                   {
                       DataTable t = StringOP.Deserialize<DataTable>(bs);

                       DataRow dr = t.Rows[0];

                       if (dt.Rows.Count == 0)
                       {
                           dt = t.Clone();
                           dt.ImportRow(dr);
                       }
                       else
                       {
                           dt.ImportRow(dr); 
                       }
                      
                   }
                   catch
                   {
                       continue;
                   }  
               }                

                return dt;
 
            }
            catch
            {                
                return null; 
            } 
        }

        /// <summary>
        /// 获取多条记录
        /// </summary>
        /// <param name="numbers">对照表编号，多个用逗号分开</param>
        /// <returns></returns>
        public static DataTable Redis_GetMXYDZB(params string[] numbers)
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);
                
                List<byte[]> b = new List<byte[]>();
                               
                byte[][] bes = null;

                for (int i = 0; i < numbers.Length; i++)
                {
                    b.Add(Encoding.UTF8.GetBytes(numbers[i]));
                }
               
                lock (RedisClass.LockObj)
                {

                    bes = RC.HMGet("H:XYBDMXDZB", b.ToArray());

                }

                DataTable dt = new DataTable();

                for (int i = 0; i < bes.Length; i++)
                {
                    byte[] bs = bes[i];

                    try
                    {
                        DataTable t = StringOP.Deserialize<DataTable>(bs);

                        DataRow dr = t.Rows[0];

                        if (dt.Rows.Count == 0)
                        {
                            dt = t.Clone();
                            dt.ImportRow(dr);
                        }
                        else
                        {
                            dt.ImportRow(dr);
                        }

                    }
                    catch
                    {
                        continue;
                    }
                }

                return dt;

            }
            catch
            {
                return null;
            } 
        }
    }
}
