using FMDBHelperClass;
using RedisReLoad.ShareDBcache;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SqlRedisForFMDBHelper_AAA_ZBDBXXB
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
            string result = "未运行";
            if (tablename.Equals("AAA_ZBDBXXB"))
            {
                
                switch (typeS)
                {
                    case "插入":
                        //如果是插入将会影响 商品买卖概况跟中标标签
                        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                        string strydddlyx = "";
                        string strtbddlyx = "";
                        if (filed["T_YSTBDDLYX"].ToString().Trim().StartsWith("@"))
                        {
                            strtbddlyx = P_ht_in[filed["T_YSTBDDLYX"].ToString().Trim()].ToString();
                        }
                        else
                        {
                            strtbddlyx = filed["T_YSTBDDLYX"].ToString().Trim();
                        }

                        if (!dic.ContainsKey(strtbddlyx.Trim()))
                        {
                            dic.Add(strtbddlyx.Trim(), new List<string>() { "商品买卖概况", "中标", "竞标中" });
                        }

                        if (filed["Y_YSYDDDLYX"].ToString().Trim().StartsWith("@"))
                        {
                            strydddlyx = P_ht_in[filed["Y_YSYDDDLYX"].ToString().Trim()].ToString();
                        }
                        else
                        {
                            strydddlyx = filed["Y_YSYDDDLYX"].ToString().Trim();
                        }

                        if (!dic.ContainsKey(strydddlyx.Trim()))
                        {
                            dic.Add(strydddlyx, new List<string>() { "商品买卖概况", "中标", "竞标中" });
                        }

                        //string strSpbh = "";
                        //string strhtqx = "";

                        //if (filed["Z_SPBH"].ToString().StartsWith("@"))
                        //{
                        //    strSpbh = P_ht_in[filed["Z_SPBH"].ToString().Trim()].ToString();
                        //}
                        //else
                        //{
                        //    strSpbh = filed["Z_SPBH"].ToString().Trim();
                        //}

                        //if (filed["Z_HTQX"].ToString().StartsWith("@"))
                        //{
                        //    strhtqx = P_ht_in[filed["Z_HTQX"].ToString().Trim()].ToString();
                        //}
                        //else
                        //{
                        //    strhtqx = filed["Z_HTQX"].ToString().Trim();
                        //}

                        //PrdTongJi.SetPrdTongJi(strSpbh, strhtqx);
                        //
                        result= RemoveKey(dic);
                        break;
                    case "更新":
                        result= UpdateFnexi(filed, where, P_ht_in);
                        break;
                    default:
                        break;
                }

                
            }

            return result;
 
            
        }


        private string UpdateFnexi(Dictionary<string, string> filed, Dictionary<string, string> where, Hashtable P_ht_in)
        {
            string str = "更新语句尚执行redis操作";

            try
            {   
                //根据更新内容判定此次update将会影响几个标签的更新 ：现有的UpDate语句中，主要针对Z_HTZT='定标合同到期',Z_QPZT='清盘结束'，Z_YTHSL=@newythsl，这几个字段会对C区查询有影响。
                List<string> list = null;
                if (filed.ContainsKey("Z_HTZT") && filed.ContainsKey("Z_QPZT"))
                {
                    list = new List<string>() { "商品买卖概况", "中标", "定标与保证函", "废标", "清盘", "货物收发概况", "已下达提货单", "货物签收", "货物发出", "问题与处理" };
                }

                //定标时
                if (filed.ContainsKey("Z_HTZT") && !filed.ContainsKey("Z_QPZT"))
                {
                    list = new List<string>() { "商品买卖概况", "中标", "定标与保证函", "货物收发概况" };
                }

                //更改提货数量时
                if (filed.ContainsKey("Z_YTHSL"))
                {
                    list = new List<string>() { "已下达提货单" };
                }

                if (list == null || list.Count <= 0)
                {
                    return str;
                }

                //根据where条件查找需要变更的用户邮箱（投标单登录邮箱，预订单登陆邮箱）
                if (where != null && where.Count > 0)
                {
                    I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                    DataSet dsGXlist = new DataSet();
                    Hashtable input = new Hashtable();
                    Hashtable htresult = new Hashtable();
                    string strsql = " select T_YSTBDDLYX,Y_YSYDDDLYX from AAA_ZBDBXXB where 1=1  ";
                    string strwhere = "";
                    foreach (var item in where)
                    {

                        if (item.Value.Trim().StartsWith("@"))
                        {
                            input.Add("@" + item.Key.Trim(), P_ht_in[item.Value.Trim()].ToString());
                        }
                        else
                        {
                            input.Add("@" + item.Key.Trim(), item.Value.Trim());
                        }
                        strwhere += "  and " + item.Key.Trim() + "=@" + item.Key.Trim();
                    }
                    if (strwhere.Equals(""))
                    {

                        return str;
                    }
                    strsql = strsql + strwhere;

                    //获取受影响需要更新的用户登录邮箱
                    Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "dlyx", input);
                    if ((bool)(return_ht["return_float"])) //说明执行完成
                    {
                        //这里就可以用了。
                        dsGXlist = (DataSet)(return_ht["return_ds"]);
                        if (dsGXlist != null && dsGXlist.Tables["dlyx"].Rows.Count > 0)
                        {

                            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                            foreach (DataRow dr in dsGXlist.Tables[0].Rows)
                            {
                                if (!dic.ContainsKey(dr["T_YSTBDDLYX"].ToString().Trim()))
                                {
                                    dic.Add(dr["T_YSTBDDLYX"].ToString().Trim(), list);
                                }
                                if (!dic.ContainsKey(dr["Y_YSYDDDLYX"].ToString().Trim()))
                                {
                                    dic.Add(dr["Y_YSYDDDLYX"].ToString().Trim(), list);
                                }
                            }

                            return RemoveKey(dic);
                        }

                    }

                }
                return str;
            }
            catch(Exception ex)
            {
                str = ex.ToString();
                return str;
            }
            
        }

        /// <summary>
        /// 移除hash结构对应键值
        /// </summary>
        /// <param name="dic">键值集合Key,field集合（list）</param>
        /// <returns></returns>
        private string  RemoveKey(Dictionary<string, List<string>> dic)
        {
            string  mess = "";
            try
            {
                RedisClient RC = RedisClass.GetRedisClient(null);
                lock (RedisClass.LockObj)
                {
                    foreach (var item in dic)
                    {
                        foreach (string lable in item.Value)
                        {
                            RC.HDel("H:Cqu:" + item.Key.Trim(), Encoding.UTF8.GetBytes(lable.Trim()));
                        }
                    }

                    mess = "ok";

                }
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
            }
            
            return mess;
           
        }

        
    }
}
