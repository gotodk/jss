using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Data;
using System.Configuration;
using FMPublicClass;
using System.Collections;
using System.Text.RegularExpressions;

namespace FMDBHelperClass
{
    static public class RedisClass
    {
        static public readonly object LockObj = new object();

        static private Hashtable htRC = new Hashtable();

        static private Hashtable DicPooledRedisPRCM = new Hashtable();
        /// <summary>
        /// 获取Redis已存在的连接实例。如果不存在则会尝试创建一个。
        /// </summary>
        /// <param name="RedisPZ">配置字符串，默认传入null</param>
        /// <returns>RedisClient连接实例</returns>
        static public RedisClient GetRedisClient(string RedisPZ)
        {
            string PZ = (RedisPZ == null || RedisPZ.Trim() == "") ? "DefaultRedis" : RedisPZ;




            /*
            if (DicPooledRedisPRCM.ContainsKey(PZ))
            {
                if (DicPooledRedisPRCM[PZ] == null)
                {
                    string connString = ConfigurationManager.ConnectionStrings[PZ].ToString();
              

                    RedisClientManagerConfig RedisConfig = new RedisClientManagerConfig();
                    RedisConfig.AutoStart = true;
                    RedisConfig.MaxReadPoolSize = 1000;
                    RedisConfig.MaxWritePoolSize = 1000;

                    PooledRedisClientManager prcm = new PooledRedisClientManager(new List<string>() { connString.Replace("|", ":") }, new List<string>() { connString.Replace("|", ":") }, RedisConfig);
                    DicPooledRedisPRCM[PZ] = prcm;





                }
            }
            else
            {
                    string connString = ConfigurationManager.ConnectionStrings[PZ].ToString();
              

                    RedisClientManagerConfig RedisConfig = new RedisClientManagerConfig();
                    RedisConfig.AutoStart = true;
                    RedisConfig.MaxReadPoolSize = 1000;
                    RedisConfig.MaxWritePoolSize = 1000;

                    PooledRedisClientManager prcm = new PooledRedisClientManager(new List<string>() { connString.Replace("|", ":") }, new List<string>() { connString.Replace("|", ":") }, RedisConfig);
                    DicPooledRedisPRCM[PZ] = prcm;

            }


            return (RedisClient)(((PooledRedisClientManager)DicPooledRedisPRCM[PZ]).GetClient());
             
            */






            /*
                       string connString = ConfigurationManager.ConnectionStrings[PZ].ToString();
                       return new RedisClient(connString.Replace("|", ":"));
            */

             




            
           
            //下面是原来的代码，有问题,没有连接池很容易冲突。所以执行命令时，需要加锁

            if (htRC.ContainsKey(PZ))
            {
                if (htRC[PZ] == null)
                {
                    string connString = ConfigurationManager.ConnectionStrings[PZ].ToString();
                    htRC[PZ] = new RedisClient(connString.Split('|')[0], Convert.ToInt32(connString.Split('|')[1]));
                }
            }
            else
            {
                string connString = ConfigurationManager.ConnectionStrings[PZ].ToString();
                htRC[PZ] = new RedisClient(connString.Split('|')[0], Convert.ToInt32(connString.Split('|')[1]));

            }


            return (RedisClient)htRC[PZ];
          



        }

        /// <summary>
        /// 从缓存读取数据(只用于FMDBHelperClass中执行语句时的那些接口实现)
        /// </summary>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置</param>
        /// <returns></returns>
        static public DataSet RedisTryOnlyForDBhelper(string RedisKey, string RedisPZ)
        {
            try
            {
                //没有设定键，直接返回空。
                if (RedisKey == null || RedisKey.Trim() == "")
                {
                    return null;
                }
                //初始化
                RedisClient RC = GetRedisClient(null);
                //读取
                byte[] re = null;
                lock (RedisClass.LockObj)
                {
                    re = RC.Get<byte[]>(RedisKey);
                }
                DataSet ds = StringOP.ByteToDataset(re);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }

            
        
        }


 


    }
}
