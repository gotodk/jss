using FMPublicClass;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace RedisReLoad.ShareDBcache
{
    /// <summary>
    /// 通常情况，这个类不会操作数据库，全部是缓存的操作
    /// </summary>
    public static class demo
    {
        /// <summary>
        /// 更新“共享数据”或“内部变量”。 用在触发点
        /// 触发点在：xxx,xxx,xxxxxx,xxxx
        /// </summary>
        /// <param name="cs">自定义各种参数，根据这些数据更新缓存</param>
        /// <returns>自定义返回值</returns>
        public static bool Updatedemo(string cs)
        {

            return true;
        }

        /// <summary>
        /// 获取“各项数据”或“内部变量”。 用在读取缓存
        /// </summary>
        /// <param name="cs">自定义各种参数，根据这些数据读取缓存</param>
        /// <returns>自定义返回值</returns>
        public static string ReadDemo(string cs)
        {
            return "";
        }

         
    }
}
