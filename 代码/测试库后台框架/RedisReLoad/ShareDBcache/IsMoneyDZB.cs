using FMDBHelperClass;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// IsMoneyDZB 的摘要说明
/// </summary>
public static class IsMoneyDZB
{
    /// <summary>
    /// 更新redis中的资金变动明细对照表
    /// </summary>
    /// <param name="number">对照表的编号</param>
    /// <returns></returns>
    public static bool UpdateRedisMoneyDZB(string number)
    {
        try
        {
            RedisClient RC = RedisClass.GetRedisClient(null);//连接redis

            //从数据库中获取当年的所有假日数据
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ht_params = new Hashtable();
            ht_params.Add("@Number", number);
            string sql_get = "select Number ,XM ,XZ ,ZY ,SJLX ,YSLX  from AAA_moneyDZB where Number=@Number";
            Hashtable ht_res = I_DBL.RunParam_SQL(sql_get, "MoneyDZB", ht_params);
            DataTable dt = new DataTable();//用于存储返回的数据
            if ((bool)ht_res["return_float"])
            {//调用接口获取数据成功

                DataSet ds_res = (DataSet)ht_res["return_ds"];
                if (ds_res == null || ds_res.Tables.Count<0 || ds_res.Tables["MoneyDZB"].Rows.Count < 0)
                {
                    return false;
                }
                byte[] bt = FMPublicClass.StringOP.DataToByte(ds_res);//将存储的数据转换成byte数组
                //在redis中设置数据。
                long lg = 0;
                lock (RedisClass.LockObj)
                {
                    lg = RC.HSet("H:ZJBDMXDZB", Encoding.UTF8.GetBytes(number), bt);//lg=1 表示插入成功；lg=0 表示更新成功，覆盖原有键值
                }
                return lg == 1 ? true : false;
            }
            else
            {
                return false;
            }

        }
        catch { return false; }

    }
    /// <summary>
    /// 读取redis资金变动明细对照表
    /// </summary>
    /// <param name="numbers">资金变动明细对照表Number数组【必填】</param>
    /// <param name="columns">自定义列【可为null】</param>
    /// <param name="tablename">表名【参数为null或是空表名为：“MoneyDZB”；】</param>
    /// <returns></returns>
    public static DataSet ReadRedisMoneyDZB(string[] numbers,string[] columns,string tablename)
    {
        try
        {
            if (numbers == null || numbers.Length < 0)
            {
                return null;
            }

            RedisClient RC = RedisClass.GetRedisClient(null);//连接redis
            DataSet ds = new DataSet();
            DataTable tb = null;

            for (int number = 0; number < numbers.Length; number++)//遍历key,检查Hash表中是否包含指定的key
            {
                byte[] byt = null;
                lock (RedisClass.LockObj)
                {
                    if (!RC.HashContainsEntry("H:ZJBDMXDZB", numbers[number].ToString()))//如果不存在，则先更新redis
                    {
                        if (!UpdateRedisMoneyDZB(numbers[number].ToString()))//没有更新成功，返回null
                        {
                            return null;
                        }
                    }

                    byt = RC.HGet("H:ZJBDMXDZB", Encoding.UTF8.GetBytes(numbers[number]));
                    if (byt == null || byt.Length < 1)
                    {
                        return null;
                    }

                    if (number == 0)
                    {
                        tb = FMPublicClass.StringOP.ByteToDataset(byt).Tables[0].Copy();
                    }
                    else
                    {
                        tb.Rows.Add(FMPublicClass.StringOP.ByteToDataset(byt).Tables[0].Rows[0].ItemArray);
                    }
                    
                }
            }
            
            #region//zhouli作废 2014.11.05
            ////读取redis
            //for (int j = 0; j < numbers.Length; j++)
            //{
            //    byte[] byt=null;
            //    lock (RedisClass.LockObj)
            //    {
            //        byt = RC.HGet("H:ZJBDMXDZB", Encoding.UTF8.GetBytes(numbers[j]));
            //     }
            //    if (byt==null||byt.Length<1)
            //    {
            //        return null;
            //    }
                                
            //    if(j==0)
            //    {
            //         tb = FMPublicClass.StringOP.ByteToDataset(byt).Tables[0].Copy();
            //    }
            //    else
            //    {
            //        tb.Rows.Add(FMPublicClass.StringOP.ByteToDataset(byt).Tables[0].Rows[0].ItemArray);
            //    }

            //}
            #endregion

            //给table增加自定义列
            if (columns != null && columns.Length > 0)
            {
                foreach (string column in columns)
                {
                    tb.Columns.Add(column, Type.GetType("System.String"));
                }
            }

            //定义表名
            if (string.IsNullOrEmpty(tablename))
            {
                tb.TableName = "MoneyDZB";
            }
            else
            {
                tb.TableName = tablename;
            }
           
            ds.Tables.Add(tb);    

            return ds;
        }
        catch { return null; }

    }
}