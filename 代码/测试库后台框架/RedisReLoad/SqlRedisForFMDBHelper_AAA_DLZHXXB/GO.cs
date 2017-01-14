using FMDBHelperClass;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlRedisForFMDBHelper_AAA_DLZHXXB
{
    public class GO
    {
        /// <summary>
        /// 开始处理，尝试更新缓存
        /// </summary>
        /// <param name="typeS">操作类型，包括“更新”、“插入”</param>
        /// <param name="P_cmd">原始带参数的sql语句</param>
        /// <param name="P_ht_in">原始传入的参数</param>
        /// <param name="tablename">被操作的表名</param>
        /// <param name="filed">被操作的字段和对应值</param>
        /// <param name="where">操作条件字段和对应值</param>
        /// <returns></returns>
        public string TryUpdateRedis(string typeS,string P_cmd, Hashtable P_ht_in, string tablename, Dictionary<string, string> filed, Dictionary<string, string> where)
        {
            //获得数据后，根据需要进行判断和处理。 注意。 字段名称一般不会有错误，但是对应值，不一定是期待的@dlyx这样的，也可能是“dj+1”这样的东西。 具体情况具体处理。


            RedisClient RC = RedisClass.GetRedisClient(null);
            lock (RedisClass.LockObj)
            {
                RC.Add("aaaaaaaaaaaa", "bbbbbbbbbbb");
            }
 
            return "ok";
        }
    }
}
