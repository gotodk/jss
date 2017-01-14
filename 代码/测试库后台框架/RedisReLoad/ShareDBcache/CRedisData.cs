using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using FMDBHelperClass;
using System.Data;
using FMPublicClass;
using System.Collections;

namespace RedisReLoad.ShareDBcache
{
    /// <summary>
    /// 用于C区Redis数据的读写
    /// </summary>
    public class CRedisData
    {       
        /// <summary>
        /// 从Hash类型中读取数据的泛型方法，可以读取存放的任何类型。
        /// 反序列化使用StringOP.Deserialize泛型方法，对应的序列化器为StringOP.Serialize泛型方法。
        /// 注意序列化与反序列化的方法必须是对应的，例如Serialize序列化string时，在泛型方法中传入类型String，首字母大写的那个。不能使用 Encoding.UTF8.GetString(bs)反序列化，因为两者不是对应的。而应使用对应的StringOP.Deserialize泛型方法。
        /// 如果需要使用序列化器Encoding.UTF8.GetBytes(str)和其对应的反序列化器Encoding.UTF8.GetString(bs)，请使用方法Redis_HGetString，Redis_HSetString。
        /// </summary>
        /// <typeparam name="TResult">返回的类型</typeparam>
        /// <param name="key">Redis中Hash类型的Key</param>
        /// <param name="field">Redis中Hash类型的Field</param>
        /// <returns>返回值</returns>
        public static TResult Redis_HGet<TResult>(string key, string field) where TResult : class
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);

                byte[] bs = null;

                lock (RedisClass.LockObj)
                {
                    //从redis中读取
                    bs = RC.HGet(key, Encoding.UTF8.GetBytes(field));

                }
                if (bs != null && bs.Length > 0)
                {
                    //反序列化
                    return StringOP.Deserialize<TResult>(bs) as TResult;
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
        /// 向Hash类型中存入数据的泛型方法，可以存放任何类型
        /// 序列化使用StringOP.Serialize泛型方法，对应的反序列化器为StringOP.Deserialize泛型方法.
        /// 注意序列化与反序列化的方法必须是对应的，当使用Serialize泛型方法序列化String时，不能使用 Encoding.UTF8.GetString(bs)反序列化，因为两者不是对应的。
        /// 如果需要使用序列化Encoding.UTF8.GetBytes(str)和其对应的反序列化Encoding.UTF8.GetString(bs)，请使用方法Redis_HGetString，Redis_HSetString.
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">Redis中Hash类型的Key</param>
        /// <param name="field">Redis中Hash类型的Field</param>
        /// <param name="t">要存放的数据</param>
        /// <returns>返回值1表示新的值，0表示覆盖，返回其他数字表示异常</returns>
        public static long Redis_HSet<T>(string key, string field, T t)
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);

                byte[] bs = StringOP.Serialize<T>(t);

                lock (RedisClass.LockObj)
                {
                    //向redis中写入数据，返回值1表示新的值，0表示覆盖
                    long l = RC.HSet(key, Encoding.UTF8.GetBytes(field), bs);
                    return l;
                }

            }
            catch
            {
                return -1;
            }

        }
        
        
        /// <summary>
        /// 从Hash类型中读取DataSet
        /// </summary>
        /// <param name="key">Redis中Hash类型的Key</param>
        /// <param name="field">Redis中Hash类型的Field</param>
        /// <returns>返回DataSet</returns>
        public static DataSet Redis_HGetData(string key,string field)
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);

                byte[] bs = null;

                lock (RedisClass.LockObj)
                {
                    //从redis中读取
                    bs = RC.HGet(key, Encoding.UTF8.GetBytes(field));

                }
                if (bs != null && bs.Length > 0)
                {

                    //反序列化
                    DataSet ds = StringOP.Deserialize<DataSet>(bs);
                    return ds;
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
        /// 向Hash类型中写入DataSet
        /// </summary>
        /// <param name="key">Redis中Hash类型的Key</param>
        /// <param name="field">Redis中Hash类型的Field</param>
        /// <param name="ds">要存入的数据集</param>
        /// <returns>true/false</returns>
        public static bool Redis_HSetData(string key,string field,DataSet ds)
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);

                byte[] bs = StringOP.Serialize<DataSet>(ds);

                lock (RedisClass.LockObj)
                {
                    //向redis中写入数据，返回值1表示新的值，0表示覆盖
                    long l = RC.HSet(key, Encoding.UTF8.GetBytes(field), bs);
                    if (l == 0 || l == 1)
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
        /// 从Hash类型中读取字符串
        /// 本方法使用的序列化是Encoding.UTF8.GetBytes(str)，其对应的反序列化是Encoding.UTF8.GetString(bs)，与上面的泛型方法不同
        /// </summary>
        /// <param name="key">Redis中Hash类型的Key</param>
        /// <param name="field">Redis中Hash类型的Field</param>
        /// <param name="RC">数据连接</param>
        /// <returns>返回值</returns>
        public static string Redis_HGetString(string key, string field, RedisClient RC)
        {
            try
            {              

                byte[] bs = null;

                lock (RedisClass.LockObj)
                {
                    //从redis中读取
                    bs = RC.HGet(key, Encoding.UTF8.GetBytes(field));

                }
                if (bs != null && bs.Length > 0)
                {

                    //反序列化                   
                    string s = Encoding.UTF8.GetString(bs);
                    return s;
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
        /// 向hash类型中存放字符串
        ///  本方法使用的序列化是Encoding.UTF8.GetBytes(str)，其对应的反序列化是Encoding.UTF8.GetString(bs)，与上面的泛型方法不同
        /// </summary>
        /// <param name="key">Redis中Hash类型的Key</param>
        /// <param name="field">Redis中Hash类型的Field</param>
        /// <param name="str">要存放的字符串</param>
        /// <param name="RC">数据连接</param>
        /// <returns>true/false</returns>
        public static bool Redis_HSetString(string key, string field, string str, RedisClient RC)
        {
            try
            {                 
             
                byte[] bs = Encoding.UTF8.GetBytes(str);

                lock (RedisClass.LockObj)
                {
                    //向redis中写入数据，返回值1表示新的值，0表示覆盖
                    long l = RC.HSet(key, Encoding.UTF8.GetBytes(field), bs);
                    if (l == 0 || l == 1)
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
        /// 从Hash类型中读取DataSet
        /// </summary>
        /// <param name="DLYX">登录邮箱，Redis中键值key</param>
        /// <param name="MKBH">模块编号，Redis中filed</param>
        /// <returns></returns>
        public static DataSet RedisZJZZ_HGetData(string DLYX,string MKBH)
        {
            try
            {
                if (string.IsNullOrEmpty(DLYX))
                {
                    return null;
                }
                string key = "H:Cqu:"+DLYX.Trim();
                string filed = "";
                switch (MKBH)
                {
                    case"":
                        filed = "资金余额变动明细";
                        break;
                    case"1":
                        filed = "资金转账";
                        break;
                    case"2":
                        filed = "资金冻结";
                        break;
                    case"3":
                        filed = "货款收付";
                        break;
                    case "4":
                        filed = "违约赔偿";
                        break;
                    case "5":
                        filed = "补偿收益";
                        break;
                    case "6":
                        filed = "违约赔偿发生记录";
                        break;
                    case "7":
                        filed = "补偿收益发生记录";
                        break;
                    default:
                        return null;
                }

                RedisClient RC = RedisClass.GetRedisClient(null);
                if (!RC.HashContainsEntry(key, filed))//验证是否存在此键值
                {
                    return null;
                }
                byte[] bs = null;

                lock (RedisClass.LockObj)
                {
                    //从redis中读取
                    bs = RC.HGet(key, Encoding.UTF8.GetBytes(filed));
                }
               return StringOP.ByteToDataset(bs);
            }
            catch(Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 向Hash类型中写入DataSet
        /// </summary>
        /// <param name="DLYX">登录邮箱，Redis中键值key【必填】</param>
        /// <param name="MKBH">模块编号，Redis中filed【MKBH值："","1","2","3","4","5","6","7"】</param>
        /// <param name="ds">数据集，Redis中的filedvalue</param>
        /// <returns></returns>
        public static bool RedisZJZZ_HSetData(string DLYX, string MKBH,DataSet ds)
        { 
            try
            {
                if (string.IsNullOrEmpty(DLYX))
                {
                    return false;
                }
                string key = "H:Cqu:" + DLYX.Trim();
                string filed = "";
                switch (MKBH)
                {
                    case "":
                        filed = "资金余额变动明细";
                        break;
                    case "1":
                        filed = "资金转账";
                        break;
                    case "2":
                        filed = "资金冻结";
                        break;
                    case "3":
                        filed = "货款收付";
                        break;
                    case "4":
                        filed = "违约赔偿";
                        break;
                    case "5":
                        filed = "补偿收益";
                        break;
                    case "6":
                        filed = "违约赔偿发生记录";
                        break;
                    case "7":
                        filed = "补偿收益发生记录";
                        break;
                    default:
                        return false;
                }

                RedisClient RC = RedisClass.GetRedisClient(null);
               
                byte[] bt = StringOP.DataToByte(ds);
                long result = 0;
                lock (RedisClass.LockObj)
                {
                    //从redis中读取
                   result = RC.HSet(key, Encoding.UTF8.GetBytes(filed), bt);
                }
                switch (result)
                {
                    case 0://1表示新的值，0表示覆盖
                    case 1:
                        return true;
                    default:
                        return false;
                }
            }
            catch(Exception)
            {
                return false;
            }            
        }

        /// <summary>
        /// 根据买家角色编号获取登录邮箱
        /// </summary>
        /// <param name="JSBH">买家角色编号</param>
        /// <returns></returns>
        private static string GetDLYXByJSBH(string JSBH)
        {
            try
            {
                //从数据库中获取登录邮箱
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                Hashtable ht_params = new Hashtable();
                ht_params.Add("@J_BUYJSBH", JSBH);
                string sql_get = "select B_DLYX from AAA_DLZHXXB where J_BUYJSBH=@J_BUYJSBH";
                Hashtable ht_res = I_DBL.RunParam_SQL(sql_get, "AAA_DLZHXXB", ht_params);
                DataTable dt = new DataTable();//用于存储返回的数据
                if ((bool)ht_res["return_float"])
                {//调用接口获取数据成功

                    DataSet ds_res = (DataSet)ht_res["return_ds"];
                    if (ds_res == null || ds_res.Tables.Count < 0 || ds_res.Tables["AAA_DLZHXXB"].Rows.Count < 0)
                    {
                        return "";
                    }
                    return ds_res.Tables["AAA_DLZHXXB"].Rows[0]["B_DLYX"].ToString().Trim();
                }
                else
                {
                    return "";
                }
            }
            catch(Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// 货物收发C区读取redis
        /// </summary>
        /// <param name="JSBH">买家角色编号</param>
        /// <param name="MKMC">模块名称</param>
        /// <returns></returns>
        public static DataSet RedisHWSF_HGetData(string JSBH,string MKMC)
        {
            try
            {
                if (string.IsNullOrEmpty(JSBH) || string.IsNullOrEmpty(MKMC))
                {
                    return null;
                }

                if (string.IsNullOrEmpty(GetDLYXByJSBH(JSBH)))
                {
                    return null;
                }

                string key = "H:Cqu:" + GetDLYXByJSBH(JSBH);

                RedisClient RC = RedisClass.GetRedisClient(null);
                if (!RC.HashContainsEntry(key, MKMC))//验证是否存在此键值
                {
                    return null;
                }
                byte[] bs = null;

                lock (RedisClass.LockObj)
                {
                    //从redis中读取
                    bs = RC.HGet(key, Encoding.UTF8.GetBytes(MKMC));
                }
                return StringOP.ByteToDataset(bs);                
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 货物收发C区更新redis
        /// </summary>
        /// <param name="JSBH">买家角色编号</param>
        /// <param name="MKMC">模块名称</param>
        /// <param name="ds">数据集</param>
        /// <returns></returns>
        public static bool RedisHWSF_HSetData(string JSBH, string MKMC, DataSet ds)
        {
            try
            {
                if (string.IsNullOrEmpty(JSBH) || string.IsNullOrEmpty(MKMC))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(GetDLYXByJSBH(JSBH)))
                {
                    return false;
                }

                string key = "H:Cqu:" + GetDLYXByJSBH(JSBH);

                RedisClient RC = RedisClass.GetRedisClient(null);

                byte[] bt = StringOP.DataToByte(ds);
                long result = 0;
                lock (RedisClass.LockObj)
                {
                    //从redis中读取
                    result = RC.HSet(key, Encoding.UTF8.GetBytes(MKMC), bt);
                }
                switch (result)
                {
                    case 0://1表示新的值，0表示覆盖
                    case 1:
                        return true;
                    default:
                        return false;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
