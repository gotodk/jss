using FMDBHelperClass;
using FMPublicClass;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace RedisReLoad.ShareDBcache
{
    /// <summary>
    /// 公共调用类库或其他公用处理
    /// </summary>
    public static class PublicHelp
    {

        public static IDictionary<string,List<string>> Mod = null;

        /// <summary>
        /// 写入“更新通知”。用"个人数据"在触发点上
        /// </summary>
        /// <param name="NoticeObject">通知对象，一般情况就是邮箱，公共对象用"public"</param>
        /// <param name="modnameKey">通知其需要更新的模块字段的key</param>
        /// <returns>自定义返回值</returns>
        public static bool NoticeUpdate(string NoticeObject, string modnameKey)
        {
            if (Mod == null)
            {
                Mod["投标单或预订单变化"] = new List<string> { "商品买卖概况", "竞标中", "冷静期" };
                Mod["资金变化"] = new List<string> { "资金余额变动明细", "资金转账", "冻结资金", "货款收付" };
            }

            RedisClient RC = RedisClass.GetRedisClient(null);
            lock (RedisClass.LockObj)
            {
                RC.AddRangeToSet("S:CUN:" + NoticeObject, Mod[modnameKey]);
            }
            return true;
        }

        /// <summary>
        /// 删除一个通知模块并确认是否存在，已确定是否需要更新缓存(false代表可以直接从缓存读取，不用更新)。用在"个人数据"读取点上。
        /// </summary>
        /// <param name="NoticeObject">通知对象，一般情况就是邮箱</param>
        /// <param name="modname">要删除并确定是否存在的模块名称</param>
        /// <returns>是否需要更新</returns>
        public static bool LookNotice(string NoticeObject, string modname)
        {
            RedisClient RC = RedisClass.GetRedisClient(null);            
            //int delcount = RC.SRem("S:CUN:" + NoticeObject, Encoding.UTF8.GetBytes(modname));

            long delcount = 0;
            lock (RedisClass.LockObj)
            {
                delcount = RC.SRem("S:CUN:" + NoticeObject, Encoding.UTF8.GetBytes(modname));
            }
            return (delcount == 1) ? true : false;
        }

        /// <summary>
        /// 从缓存读取"个人数据"。
        /// </summary>
        /// <param name="NoticeObject">读取对象，一般情况就是邮箱</param>
        /// <param name="modname">模块名称</param>
        /// <returns>“个人数据”数据集</returns>
        public static DataSet ReadCacheGR(string NoticeObject, string modname)
        {
            RedisClient RC = RedisClass.GetRedisClient(null);
            byte[] Hvalue2 = null;
            lock (RedisClass.LockObj)
            {
                Hvalue2 = RC.HGet("H:CUC:" + NoticeObject, Encoding.UTF8.GetBytes(modname));
            }
            return (Hvalue2 == null) ? null : StringOP.ByteToDataset(Hvalue2);
        }

        /// <summary>
        /// 缓存写入"个人数据"。
        /// </summary>
        /// <param name="NoticeObject">读取对象，一般情况就是邮箱</param>
        /// <param name="modname">模块名称</param>
        /// <param name="dsGR">要写入的数据集</param>
        /// <returns>写入成功的数量</returns>
        public static long WriteCacheGR(string NoticeObject, string modname,DataSet dsGR)
        {
            RedisClient RC = RedisClass.GetRedisClient(null);
            byte[] Hvalue = StringOP.DataToByte(dsGR);
            if (Hvalue == null)
            {
                return 0;
            }
            long re = 0;
            lock (RedisClass.LockObj)
            {
                RC.HSet("H:CUC:" + NoticeObject, Encoding.UTF8.GetBytes(modname), Hvalue);
            }
            return re;
        }


        /// <summary>
        /// 这是例子，说明增加缓存读取时，代码编写模式的。
        /// </summary>
        private static DataSet DemoReidsAndDB()
        {
        
            //模拟定义两个数据
            string modnameGR = "商品买卖概况"; //
            string dlyx = "yhb888@gmail.com";
            //先确定是否需要更新缓存
            if (!PublicHelp.LookNotice(dlyx, modnameGR))//不需要更新，先从缓存读取
            {
                //读取“个人数据”
                DataSet dsCache = PublicHelp.ReadCacheGR(dlyx, modnameGR);

                //调用处理类库，获得“共享”数据，然后合并进去。
                //...............

                //直接返回，不再读取数据库
                return dsCache;
            }




            //模拟原来获取某dataset的那段代码，例如获取分页第一页的地方。
            DataSet FormDB = new DataSet("原来从数据库得到的处理后的数据");
            //...............





            //只要走了链接数据库代码，就把数据写缓存里面去。
            WriteCacheGR(dlyx, modnameGR, FormDB);

            //返回
            return FormDB;
        }

        /// <summary>
        /// 获取表的主键
        /// </summary>
        /// <param name="ModuleName">表名</param>        
        /// <returns>编号，格式为：yyyyMMddxxxxxx</returns>
        public static string GetPrimKey(string ModuleName)
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);

                string key = "str:TablePK:" + ModuleName + DateTime.Now.ToString("yyyyMM");

                DateTime dtStart = DateTime.Now;
                //获取当月最后一天的时间
                DateTime dtEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                //获取当前时间与当月最后一天相差的秒数作为键值的生命周期
                TimeSpan ts = dtEnd.Subtract(dtStart).Duration();
                string seconds = ts.TotalSeconds.ToString("0");

                //获取主键的下一个顺序号
                string id = "0";
                lock (RedisClass.LockObj)
                {
                    id = RC.ExecLuaAsString("local times=redis.call('incr',KEYS[1]) if times==1 then redis.call('expire',KEYS[1],ARGV[1]) end return times", new string[] { key }, new string[] { seconds });
                }               
                string keyNumber = DateTime.Now.ToString("yyMMdd") + id.PadLeft(6, '0');
                return keyNumber;
            }
            catch
            {
                return null;
            }
        }

    }
}
