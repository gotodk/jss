using FMDBHelperClass;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisReLoad.ShareDBcache
{
    public static class HomePageRedis
    {
        /// <summary>
        /// 写入大盘二进制文件缓存
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool UpdateHomePageByte(byte[] b)
        {
            //写入Redis缓存
            RedisClient RC = RedisClass.GetRedisClient(null);
            lock (RedisClass.LockObj)
            {
                RC.Set("str:HomePageByte", b);
            }
            return true;
        }

        /// <summary>
        /// 写入生成简化商品二进制文件
        /// </summary>
        /// <param name="b_shangpinS"></param>
        /// <returns></returns>
        public static bool UpdateJHXP(byte[] b_shangpinS)
        {
            //写入Redis缓存
            RedisClient RC = RedisClass.GetRedisClient(null);
            lock (RedisClass.LockObj)
            {
                RC.Set("str:JHXP", b_shangpinS);
            }
            return true;
        }

        /// <summary>
        /// 写入底部统计分析二进制文件
        /// </summary>
        /// <param name="b_tongji"></param>
        /// <returns></returns>
        public static bool UpdateHomePageByte_tongji(byte[] b_tongji)
        {
            //写入Redis缓存
            RedisClient RC = RedisClass.GetRedisClient(null);
            lock (RedisClass.LockObj)
            {
                RC.Set("str:HomePageByte_tongji", b_tongji);
            }
            return true;
        }

        /// <summary>
        /// 获取底部统计分析二进制文件
        /// </summary>
        /// <returns></returns>
        public static byte[] GetHomePageByte_tongji()
        {
            RedisClient RC = RedisClass.GetRedisClient(null);
            byte[] data = null;
            lock (RedisClass.LockObj)
            {
                data = RC.Get("str:HomePageByte_tongji");
            }
            return data;
        }

        /// <summary>
        /// 获取指定键值的二进制数据，一般就是 str:HomePageByte和str:JHXP
        /// </summary>
        /// <returns></returns>
        public static byte[] GetHomePageByteSSS(string key)
        {
            RedisClient RC = RedisClass.GetRedisClient(null);
            byte[] data = null;
            lock (RedisClass.LockObj)
            {
                data = RC.Get(key);
            }
            return data;
        }

        /// <summary>
        /// 检查是否开启了踢人功能。
        /// </summary>
        /// <returns></returns>
        public static bool CheckTiRen()
        {
            RedisClient RC = RedisClass.GetRedisClient(null);
            byte[] data = null;
            lock (RedisClass.LockObj)
            {
                data = RC.Get("str:TiRen");
            }
            if (data == null)
            {
                return  false;
            }
            else
            {
                return true;
            }
           
        }

        /// <summary>
        /// 标记某用户有新提醒要查看
        /// </summary>
        /// <param name="dlyx"></param>
        public static void AddNewmessage(string dlyx)
        {
            RedisClient RC = RedisClass.GetRedisClient(null);
            lock (RedisClass.LockObj)
            {
                RC.AddItemToSet("S:newmessage", dlyx.Trim());
            }
        }
        /// <summary>
        /// 检查某用户是否有可能有新提醒要显示出来，若发现有新的提醒，则自动会删除该邮箱元素。
        /// </summary>
        /// <param name="dlyx"></param>
        public static bool CheckNewmessage(string dlyx)
        {
            long delcount = 0;
            RedisClient RC = RedisClass.GetRedisClient(null);
            lock (RedisClass.LockObj)
            {
                delcount = RC.SRem("S:newmessage", Encoding.UTF8.GetBytes(dlyx.Trim()));
            }
            return (delcount == 1) ? true : false;
        }

    }
}
