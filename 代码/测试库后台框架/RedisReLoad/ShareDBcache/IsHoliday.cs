using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack.Redis;
using FMDBHelperClass;
using System.Collections;
namespace RedisReLoad.ShareDBcache
{
    public static class IsHoliday
    {
        /// <summary>
        /// 更新redis中的节假日数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>true,false</returns>
        public static bool UpdateHoliday(string year)
        {
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);
                string key = "S:PTJRSDB:" + year;
                //从数据库中获取当年的所有假日数据
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                Hashtable ht_params = new Hashtable();
                ht_params.Add("@year", year);
                ht_params.Add("@sfyx", "是");
                string sql_get = "select convert(varchar(10),RQ,120) as 日期 from AAA_PTJRSDB where @year=year(RQ) and SFYX=@sfyx";
                Hashtable ht_res = I_DBL.RunParam_SQL(sql_get, "date", ht_params);
                DataTable dt = new DataTable();//用于存储返回的日期数据
                if ((bool)ht_res["return_float"])
                {//调用接口获取数据成功
                    DataSet ds_res = (DataSet)ht_res["return_ds"];
                    //获取更新redis的数据
                    List<string> L_members = new List<string>();
                    if (ds_res != null && ds_res.Tables.Contains("date"))
                    {//返回的数据集中存在名称为date的表
                        for (int i = 0; i < ds_res.Tables["date"].Rows.Count; i++)
                        {
                            L_members.Add(ds_res.Tables[0].Rows[i]["日期"].ToString());

                        }
                        //在redis中设置数据。
                        lock (RedisClass.LockObj)
                        {                           
                            RC.AddRangeToSet(key, L_members);
                        }
                        return true;
                    }
                    else
                    {//返回数据集中不存在名为date的表，返回false
                        return false;
                    }
                }
                else
                {//调用接口获取数据失败，直接返回false
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///判断传入日期是否在节假日集合中，存在在返回“是”
        /// </summary>        
        /// <param name="date">日期，格式为：yyyy-MM-dd</param>
        /// <returns>是、否</returns>
        public static string ReadHoliday(string date)
        {           
            try
            {
               string year = Convert.ToDateTime(date).Year.ToString();

                string key = "S:PTJRSDB:" + year;

                RedisClient RC = RedisClass.GetRedisClient(null);

                long existKey = 0;
                //判断redis中是否存在对应的节假日集合键值
                lock (RedisClass.LockObj)
                {
                    existKey = RC.Exists(key);
                }
                if (existKey == 0)
                { //如果key不存在，则从调用update方法更新redis数据
                    UpdateHoliday(year);
                }
                //如果判断日期是否在集合中
                long isholiday = 0;
                lock (RedisClass.LockObj)
                {
                    isholiday = RC.SIsMember(key, Encoding.UTF8.GetBytes(date));
                }
                return isholiday == 1 ? "是" : "否";
            }
            catch
            {
                return "否";
            }
        }
    }
}
