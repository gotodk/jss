using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FMDBHelperClass;
using FMipcClass;
using FMPublicClass;
using System.Collections;
using ServiceStack.Redis;
using RedisReLoad.ShareDBcache;

/// <summary>
/// anaPageRetreat 的摘要说明
/// </summary>
public class anaPageRetreat
{
	public anaPageRetreat()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 废标标签中的履约保证金扣罚金额、是否拆单的处理
    /// </summary>
    /// <param name="dsold">首次分页得到的关键数据</param>
    /// <returns>处理后的数据</returns>
    public DataSet FeiBiao(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input=new Hashtable();
        try
        {
            //进行在处理,daold的值，正常情况，不可能为null,所以不需要进行判断
            //通常这里不建议用in进行再处理，需要循环按唯一键进行在处理。
            //复杂的情况需要进行分析和取舍，可以选择分页首次处理的大部分字段留空，这里循环直接填空。 
            //也可以在这里重新生成新的数据集。
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                //循环处理履约保证金扣罚金额
                if (dsreturn.Tables[0].Rows[i]["废标原因"].ToString() == "卖方履约保证金不足60%" && dsreturn.Tables[0].Rows[i]["单据类型"].ToString() == "投标单")
                {
                    input["@lydh"] = dsreturn.Tables[0].Rows[i]["中标定标编号"].ToString();
                    //此种情况需要计算履约保证金扣罚金
                    string sql_lybzj = "select isnull(cast(sum(JE) as numeric(18,2)),0.00) as 履约保证金扣罚金额 from AAA_ZKLSMXB where SJLX='实' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息'  or XZ='电子购货合同履约保证金不足废标扣罚' or XZ='电子购货合同期满卖方未录入发货信息扣罚') and LYYWLX='AAA_ZBDBXXB' and LYDH=@lydh";

                    Hashtable ht_lybzj = I_DBL.RunParam_SQL(sql_lybzj, "数据",input);
                    if ((bool)ht_lybzj["return_float"])
                    {
                        DataSet ds_lybzj = (DataSet)ht_lybzj["return_ds"];
                        if (ds_lybzj != null && ds_lybzj.Tables[0].Rows.Count > 0)
                        {
                            dsreturn.Tables[0].Rows[i]["履约保证金扣罚金额"] = ds_lybzj.Tables[0].Rows[0]["履约保证金扣罚金额"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }
        //二次处理后若处理结果是空值，返回原来的数据
        if (dsreturn == null)
        {
            dsreturn = dsold;
        }
        return dsreturn;
    }
    /// <summary>
    /// 定标与保证函标签中的履约保证金冻结剩余比例、时候拆单的处理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的返回数据集</returns>
    public DataSet DingBiao(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        try
        {
            //进行在处理,daold的值，正常情况，不可能为null,所以不需要进行判断
            //通常这里不建议用in进行再处理，需要循环按唯一键进行在处理。
            //复杂的情况需要进行分析和取舍，可以选择分页首次处理的大部分字段留空，这里循环直接填空。 
            //也可以在这里重新生成新的数据集。
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                if (dsreturn.Tables[0].Rows[i]["单据类型"].ToString() == "投标单")
                {
                    //获取履约保证金剩余比率
                    input["@bh"] = dsreturn.Tables[0].Rows[i]["中标定标编号"].ToString();
                    string sql_lybzj="select isnull(sum(a.JE),0.00) as 金额 from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.number where SJLX='预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') and LYYWLX='AAA_THDYFHDXXB' and b.ZBDBXXBBH=@bh";
                    Hashtable ht_res=I_DBL.RunParam_SQL(sql_lybzj,"数据",input);
                    if((bool)ht_res["return_float"])
                    {
                        DataSet ds_je=(DataSet )ht_res["return_ds"];
                        if(ds_je!=null&&ds_je.Tables [0].Rows.Count >0)
                        {
                             dsreturn.Tables[0].Rows[i]["履约保证金冻结剩余比例"] = ((1 - Convert.ToDouble(ds_je.Tables [0].Rows [0]["金额"].ToString ()) / Convert.ToDouble(dsreturn.Tables[0].Rows[i]["履约保证金冻结金额"])) * 100.00).ToString("#0.00");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }

        //二次处理后若处理结果是空值，返回原来的数据
        if (dsreturn == null)
        {
            dsreturn = dsold;
        }
        return dsreturn;
    }

    /// <summary>
    /// 冷静期标签中的“当前卖家最低价”、“最低价标的经济批量”、“最低价标的达成率”的处理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    public DataSet LengJingQi(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                //获取当前商品的卖家最低价及相关数据
                input["@htqx"] = dsreturn.Tables[0].Rows[i]["合同期限"].ToString();
                input["@spbh"] = dsreturn.Tables[0].Rows[i]["商品编号"].ToString();
                string sql_lowprice = "select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,MJSDJJPL as 经济批量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH=@spbh and HTQX=@htqx  order by TBJG,TJSJ";

                Hashtable ht_res = I_DBL.RunParam_SQL(sql_lowprice, "数据", input);
                if ((bool)ht_res["return_float"])
                {
                    DataSet ds_lowprice = (DataSet)ht_res["return_ds"];
                    if (ds_lowprice != null && ds_lowprice.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["当前卖方最低价"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                        dsreturn.Tables[0].Rows[i]["最低价标的经济批量"] = ds_lowprice.Tables[0].Rows[0]["经济批量"].ToString();

                        //获取最低价标的拟购买数量和
                        input["@tbjg"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                        input["@ghqy"] = ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString();
                        string sql = "select isnull(sum(NDGSL-YZBSL),0) as 拟订购数量 from AAA_YDDXXB where SPBH=@spbh and  HTQX=@htqx and ZT = '竞标' and NMRJG >=convert(decimal(18,2),@tbjg) and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull(@ghqy,''))";
                        Hashtable ht_ndgsl = I_DBL.RunParam_SQL(sql, "数据", input);
                        if ((bool)ht_ndgsl["return_float"])
                        {
                            DataSet ds_ndgsl = (DataSet)ht_ndgsl["return_ds"];
                            if (ds_ndgsl != null && ds_ndgsl.Tables[0].Rows.Count > 0)
                            {
                                //算最低价的达成率，因为进冷静期的只是三个月/一年的合同，投标单不会出现拆单，所以达成率如下
                                dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = (Convert.ToDouble(ds_ndgsl.Tables [0].Rows [0]["拟订购数量"].ToString ()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }
        //二次处理后若处理结果是空值，返回原来的数据
        if (dsreturn == null)
        {
            dsreturn = dsold;
        }
        return dsreturn;
    }


    /// <summary>
    /// 竞标标签中的“当前卖家最低价”、“最低价标的经济批量”、“最低价标的达成率”的处理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    public DataSet JingBiaoRedis(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();       
        RedisClient newRC = RedisClass.GetRedisClient(null);

        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                input["@spbh"] = dsreturn.Tables[0].Rows[i]["商品编号"].ToString();
                input["@htqx"] = dsreturn.Tables[0].Rows[i]["合同期限"].ToString();
                string key = "H:YeWuDB:" + input["@spbh"].ToString() + ":" + input["@htqx"].ToString();

                //从redis中读取
                string dqmjzdj = CRedisData.Redis_HGetString(key, "当前卖家最低价", newRC);
                string zdjbdjjpl = CRedisData.Redis_HGetString(key, "最低价标的经济批量", newRC);
                string zdjbbl = CRedisData.Redis_HGetString(key, "达成率/中标率", newRC);
                
                 //赋值
                dsreturn.Tables[0].Rows[i]["当前卖方最低价"] = dqmjzdj==null?"--":dqmjzdj;
                dsreturn.Tables[0].Rows[i]["最低价标的经济批量"] = zdjbdjjpl == null ? "--" : zdjbdjjpl;
                dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = zdjbbl == null ? "--" : zdjbbl;
 
            }

        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }
        return dsreturn;


    }



    /// <summary>
    /// 竞标标签中的“当前卖家最低价”、“最低价标的经济批量”、“最低价标的达成率”的处理
    /// 此方法不再使用，改用JingBiaoRedis
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    public DataSet JingBiao(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                //获取当前商品的卖家最低价及相关数据
                input["@spbh"] = dsreturn.Tables[0].Rows[i]["商品编号"].ToString();
                input["@htqx"] = dsreturn.Tables[0].Rows[i]["合同期限"].ToString();
                string sql_lowprice = "select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,MJSDJJPL as 经济批量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH=@spbh and HTQX=@htqx  order by TBJG,TJSJ";
                Hashtable ht_lowprice = I_DBL.RunParam_SQL(sql_lowprice, "数据", input);
                if ((bool)ht_lowprice["return_float"])
                {
                    DataSet ds_lowprice = (DataSet)ht_lowprice["return_ds"];
                    if (ds_lowprice != null && ds_lowprice.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["当前卖方最低价"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                        dsreturn.Tables[0].Rows[i]["最低价标的经济批量"] = ds_lowprice.Tables[0].Rows[0]["经济批量"].ToString();

                        //即时合同的达成率和三个月/一年的计算公式不同
                        if (dsreturn.Tables[0].Rows[i]["合同期限"].ToString() == "即时")
                        {
                            dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = (Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["已中标数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                        }
                        else
                        {
                            //获取最低价标的拟购买数量和
                            input["@tbjg"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                            input["@ghqy"] = ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString();

                            string sql = "select isnull(sum(NDGSL-YZBSL),0) as 拟订购数量 from AAA_YDDXXB where SPBH=@spbh and  HTQX=@htqx and ZT = '竞标' and NMRJG >=convert(decimal(18,2),@tbjg) and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull(@ghqy,''))";
                            Hashtable ht_ndgsl = I_DBL.RunParam_SQL(sql, "数据", input);
                            if ((bool)ht_ndgsl["return_float"])
                            {
                                DataSet ds_ndgsl = (DataSet)ht_ndgsl["return_ds"];
                                if (ds_ndgsl != null && ds_ndgsl.Tables[0].Rows.Count > 0)
                                {
                                    dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = (Convert.ToDouble(ds_ndgsl.Tables[0].Rows[0]["拟订购数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }
        return dsreturn;
    }

    #region
    
    /// <summary>
    /// 商品买卖C区中商品买卖概况标签中的“当前卖家最低价”、“最低价标的经济批量”、“最低价标的达成率”的处理
    /// </summary>
    /// <param name="dsold"></param>
    /// <returns></returns>
    public DataSet SPMMGKRedis(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        RedisClient newRC = RedisClass.GetRedisClient(null);
        
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                input["@spbh"] = dsreturn.Tables[0].Rows[i]["商品编号"].ToString();
                input["@htqx"] = dsreturn.Tables[0].Rows[i]["合同期限"].ToString();
                string key = "H:YeWuDB:" + input["@spbh"].ToString() + ":" + input["@htqx"].ToString();

                //先从redis中读取
                string dqmjzdj = CRedisData.Redis_HGetString(key, "当前卖家最低价", newRC);
                string zdjbdjjpl = CRedisData.Redis_HGetString(key, "最低价标的经济批量", newRC);
                string zdjbbl = CRedisData.Redis_HGetString(key, "达成率/中标率", newRC);                

                //赋值
                dsreturn.Tables[0].Rows[i]["当前卖方最低价"] = dqmjzdj==null?"--":dqmjzdj;
                dsreturn.Tables[0].Rows[i]["最低价标的经济批量"] = zdjbdjjpl == null ? "--" : zdjbdjjpl;
                dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = zdjbbl == null ? "--" : zdjbbl;

                #region 确定应该显示的当前状态
                if (dsreturn.Tables[0].Rows[i]["单据状态"].ToString() == "竞标")
                {
                    if (dsreturn.Tables[0].Rows[i]["合同期限"].ToString() == "即时")
                    {
                        dsreturn.Tables[0].Rows[i]["当前状态"] = "竞标中";
                    }
                    else
                    {
                        //三个月/一年的，判断是否进入冷静期
                        string sql_ljq = "select SFJRLJQ as 冷静期 from AAA_LJQDQZTXXB where SPBH=@spbh and HTQX=@htqx";
                        Hashtable ht_ljq = I_DBL.RunParam_SQL(sql_ljq, "data", input);
                        if ((bool)ht_ljq["return_float"])
                        {
                            DataSet ds_ljq = (DataSet)ht_ljq["return_ds"];
                            if (ds_ljq != null && ds_ljq.Tables[0].Rows.Count > 0)
                            {
                                dsreturn.Tables[0].Rows[i]["当前状态"] = ds_ljq.Tables[0].Rows[0]["冷静期"].ToString() == "是" ? "竞标中（冷静期）" : "竞标中";
                            }
                        }
                    }
                }
                else
                {
                    //从中标定标信息表中获取的数据的状态对应
                    switch (dsreturn.Tables[0].Rows[i]["单据状态"].ToString())
                    {
                        case "定标合同到期":
                            dsreturn.Tables[0].Rows[i]["当前状态"] = "定标";
                            break;
                        case "定标执行完成":
                            dsreturn.Tables[0].Rows[i]["当前状态"] = "定标";
                            break;
                        case "未定标废标":
                            dsreturn.Tables[0].Rows[i]["当前状态"] = "废标";
                            break;
                        case "定标合同终止":
                            dsreturn.Tables[0].Rows[i]["当前状态"] = "废标";
                            break;
                        default:
                            dsreturn.Tables[0].Rows[i]["当前状态"] = dsreturn.Tables[0].Rows[i]["单据状态"].ToString();
                            break;
                    }
                }
                #endregion

            }

        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }
        return dsreturn;

    }
    

    #endregion

  
    

    /// <summary>
    /// 商品买卖C区中商品买卖概况标签中的“当前卖家最低价”、“最低价标的经济批量”、“最低价标的达成率”的处理
    /// 不在使用此方法，改用对应的SPMMGKRedis方法，2014.11.27
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    public DataSet SPMMGK(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                //获取当前最低价、经济批量、达成率
                input["@spbh"] = dsreturn.Tables[0].Rows[i]["商品编号"].ToString();
                input["@htqx"] = dsreturn.Tables[0].Rows[i]["合同期限"].ToString();

                string sql_lowprice = "select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,MJSDJJPL as 经济批量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH=@spbh and HTQX=@htqx  order by TBJG,TJSJ";
                Hashtable ht_lowprice = I_DBL.RunParam_SQL(sql_lowprice, "数据", input);
                if ((bool)ht_lowprice["return_float"])
                {
                    DataSet ds_lowprice = (DataSet)ht_lowprice["return_ds"];
                    //获取当前商品的卖家最低价及相关数据               
                    if (ds_lowprice != null && ds_lowprice.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["当前卖方最低价"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                        dsreturn.Tables[0].Rows[i]["最低价标的经济批量"] = ds_lowprice.Tables[0].Rows[0]["经济批量"].ToString();

                        //即时合同的达成率和三个月/一年的计算公式不同
                        if (dsreturn.Tables[0].Rows[i]["合同期限"].ToString() == "即时")
                        {
                            dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = (Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["已中标数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                        }
                        else
                        {
                            input["@tbjg"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                            input["@ghqy"] = ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString();
                            //获取最低价标的拟购买数量和
                            string sql = "select isnull(sum(NDGSL-YZBSL),0) as 拟订购数量 from AAA_YDDXXB where SPBH=@spbh and  HTQX=@htqx and ZT = '竞标' and NMRJG >=convert(decimal(18,2),@tbjg) and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull(@ghqy,''))";
                            Hashtable ht_ndgsl = I_DBL.RunParam_SQL(sql, "data", input);
                            if ((bool)ht_ndgsl["return_float"])
                            {
                                DataSet ds_ndgsl = (DataSet)ht_ndgsl["return_ds"];
                                if (ds_ndgsl != null && ds_ndgsl.Tables[0].Rows.Count > 0)
                                {
                                    dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = (Convert.ToDouble(ds_ndgsl.Tables[0].Rows[0]["拟订购数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                                }
                            }
                        }
                    }
                }

                #region 确定应该显示的当前状态
                if (dsreturn.Tables[0].Rows[i]["单据状态"].ToString() == "竞标")
                {
                    if (dsreturn.Tables[0].Rows[i]["合同期限"].ToString() == "即时")
                    {
                        dsreturn.Tables[0].Rows[i]["当前状态"] = "竞标中";
                    }
                    else
                    {
                        //三个月/一年的，判断是否进入冷静期
                        string sql_ljq = "select SFJRLJQ as 冷静期 from AAA_LJQDQZTXXB where SPBH=@spbh and HTQX=@htqx";
                        Hashtable ht_ljq = I_DBL.RunParam_SQL(sql_ljq, "data", input);
                        if ((bool)ht_ljq["return_float"])
                        {
                            DataSet ds_ljq = (DataSet)ht_ljq["return_ds"];
                            if (ds_ljq != null && ds_ljq.Tables[0].Rows.Count > 0)
                            {
                                dsreturn.Tables[0].Rows[i]["当前状态"] = ds_ljq.Tables[0].Rows[0]["冷静期"].ToString() == "是" ? "竞标中（冷静期）" : "竞标中";
                            }
                        }
                    }
                }
                else
                {
                    //从中标定标信息表中获取的数据的状态对应
                    switch (dsreturn.Tables[0].Rows[i]["单据状态"].ToString())
                    {
                        case "定标合同到期":
                            dsreturn.Tables[0].Rows[i]["当前状态"] = "定标";
                            break;
                        case "定标执行完成":
                            dsreturn.Tables[0].Rows[i]["当前状态"] = "定标";
                            break;
                        case "未定标废标":
                            dsreturn.Tables[0].Rows[i]["当前状态"] = "废标";
                            break;
                        case "定标合同终止":
                            dsreturn.Tables[0].Rows[i]["当前状态"] = "废标";
                            break;
                        default:
                            dsreturn.Tables[0].Rows[i]["当前状态"] = dsreturn.Tables[0].Rows[i]["单据状态"].ToString();
                            break;
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }
        return dsreturn;
    }

    /// <summary>
    /// 交易方基本资料中的账户状态字段处理
    /// </summary>
    /// <param name="dsold">首次分页得到的关键数据</param>
    /// <returns>处理后的数据</returns>
    public DataSet JYFJBZL(DataSet dsold)
    {
        DataSet dsreturn = dsold;

        try
        {
            //进行在处理,daold的值，正常情况，不可能为null,所以不需要进行判断
            //通常这里不建议用in进行再处理，需要循环按唯一键进行在处理。
            //复杂的情况需要进行分析和取舍，可以选择分页首次处理的大部分字段留空，这里循环直接填空。 
            //也可以在这里重新生成新的数据集。
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                string zhzt = "";//记录账户状态信息
                //循环处理履约保证金扣罚金额
                if (dsreturn.Tables[0].Rows[i]["是否冻结"].ToString() == "是")
                {
                    zhzt = zhzt + "冻结、";
                }
                if (dsreturn.Tables[0].Rows[i]["是否休眠"].ToString() == "是")
                {
                    zhzt = zhzt + "休眠、";
                }
                if (dsreturn.Tables[0].Rows[i]["是否暂停用户新业务"].ToString() == "是")
                {
                    zhzt = zhzt + "暂停新业务、";
                }
                if (zhzt.Trim() == "")
                {
                    dsreturn.Tables[0].Rows[i]["账户状态"] = "正常";
                }
                else
                {
                    dsreturn.Tables[0].Rows[i]["账户状态"] = zhzt.Substring(0, zhzt.Length - 1);
                }
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }

        //二次处理后若处理结果是空值，返回原来的数据
        if (dsreturn == null)
        {
            dsreturn = dsold;
        }
        return dsreturn;
    }

    /// <summary>
    /// 交易方交易概况中经纪人累计收益之外的计算字段处理
    /// </summary>
    /// <param name="dsold">首次分页得到的关键数据</param>
    /// <returns>处理后的数据</returns>
    public DataSet JYFJYGK(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        try
        {
            //进行在处理,daold的值，正常情况，不可能为null,所以不需要进行判断
            //通常这里不建议用in进行再处理，需要循环按唯一键进行在处理。
            //复杂的情况需要进行分析和取舍，可以选择分页首次处理的大部分字段留空，这里循环直接填空。 
            //也可以在这里重新生成新的数据集。
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                input["@jyfzh"] = dsreturn.Tables[0].Rows[i]["交易方账号"].ToString();
                input["@gljjrzh"] = dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString();
                //累计卖出金额
                if (dsreturn.Tables[0].Rows[i]["结算账户类型"].ToString() == "经纪人交易账户")
                {
                    dsreturn.Tables[0].Rows[i]["累计卖出金额"] = "--";
                }
                else
                {
                    string sql_ljmc = "select isnull(sum(a.T_DJHKJE),0.00) as 累计卖出 from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where b.T_YSTBDDLYX = @jyfzh  and b.T_YSTBDGLJJRYX=@gljjrzh and a.F_BUYWYYSHCZSJ is not null";
                    Hashtable ht_ljmc = I_DBL.RunParam_SQL(sql_ljmc, "data", input);
                    if ((bool)ht_ljmc["return_float"])
                    {
                        DataSet ds_ljmc = (DataSet)ht_ljmc["return_ds"];
                        if (ds_ljmc != null && ds_ljmc.Tables[0].Rows.Count > 0)
                        {
                            dsreturn.Tables[0].Rows[i]["累计卖出金额"] = ds_ljmc.Tables[0].Rows[0]["累计卖出"].ToString();
                        }
                    }
                }

                //累计买入金额
                string sql_ljmr = "select isnull(sum(a.T_DJHKJE),0.00) as 累计买入 from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where b.Y_YSYDDDLYX =@jyfzh and b.Y_YSYDDGLJJRYX=@gljjrzh and a.F_BUYWYYSHCZSJ is not null";
                Hashtable ht_ljmr = I_DBL.RunParam_SQL(sql_ljmr, "data", input);
                if ((bool)ht_ljmr["return_float"])
                {
                    DataSet ds_ljmr = (DataSet)ht_ljmr["return_ds"];
                    if (ds_ljmr != null && ds_ljmr.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["累计买入金额"] = ds_ljmr.Tables[0].Rows[0]["累计买入"].ToString();
                    }
                }
               
                //累计竞标次数
                string sql_ljjb = "select (select count(*) as 累计投标单 from AAA_TBD where DLYX = @jyfzh and GLJJRYX=@gljjrzh) as 累计投标单,(select count(*) as 累计预定单 from AAA_YDDXXB where DLYX = @jyfzh and GLJJRYX=@gljjrzh) as 累计预订单";
                Hashtable ht_ljjb = I_DBL.RunParam_SQL(sql_ljjb, "data", input);

                if ((bool)ht_ljjb["return_float"])
                {
                    DataSet ds_ljjb = (DataSet)ht_ljjb["return_ds"];
                    if (ds_ljjb != null && ds_ljjb.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["累计竞标次数"] = (Convert.ToInt64(ds_ljjb.Tables [0].Rows[0]["累计投标单"]) + Convert.ToInt64(ds_ljjb.Tables [0].Rows [0]["累计预订单"])).ToString();
                    }
                }              

                //累计中标次数
                string sql_ljzb="select count(*) as 累计中标 from AAA_ZBDBXXB where (Y_YSYDDDLYX = @jyfzh and Y_YSYDDGLJJRYX=@gljjrzh) or (T_YSTBDDLYX = @jyfzh and T_YSTBDGLJJRYX=@gljjrzh)";
                Hashtable ht_ljzb=I_DBL.RunParam_SQL(sql_ljzb,"data",input );
                if((bool)ht_ljzb["return_float"])
                {
                    DataSet ds_ljzb=(DataSet)ht_ljzb["return_ds"];
                    if(ds_ljzb!=null&&ds_ljzb.Tables [0].Rows.Count >0)
                    {
                         dsreturn.Tables[0].Rows[i]["累计中标次数"] = ds_ljzb.Tables [0].Rows [0]["累计中标"].ToString ();
                    }
                }

                //已发货待付款金额= 已录入发货信息的货物金额 - 已无异议签收的货物金额(不包含“撤销”状态的单子)
                string sql_yfhdfk = "select isnull(sum(a.T_DJHKJE),0.00) as 已发货代付款 from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where ((b.T_YSTBDDLYX =@jyfzh and b.T_YSTBDGLJJRYX=@gljjrzh) or (b.Y_YSYDDDLYX=@jyfzh and b.Y_YSYDDGLJJRYX=@gljjrzh)) and a.F_WLXXLRSJ is not null and a.F_BUYWYYSHCZSJ is null and a.F_DQZT<>'撤销' and b.Z_QPZT<>'清盘结束' ";
                Hashtable ht_yfhdfu=I_DBL.RunParam_SQL(sql_yfhdfk,"data",input );
                if((bool)ht_yfhdfu["return_float"])
                {
                    DataSet ds_yfhdfu=(DataSet )ht_yfhdfu["return_ds"];
                    if(ds_yfhdfu!=null&&ds_yfhdfu.Tables[0].Rows .Count >0)
                    {
                        dsreturn.Tables[0].Rows[i]["已发货待付款金额"] = ds_yfhdfu.Tables [0].Rows [0]["已发货代付款"].ToString ();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }

        //二次处理后若处理结果是空值，返回原来的数据
        if (dsreturn == null)
        {
            dsreturn = dsold;
        }
        return dsreturn;
    }

    /// <summary>
    /// 本账户下交易方交易商品统计分析标签下除累计买入金额、累计卖出金额之外的统计字段处理
    /// </summary>
    /// <param name="dsold">首次分页得到的关键数据</param>
    /// <returns>处理后的数据</returns>
    public DataSet JYFJYSPTJ(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        try
        {
            //进行在处理,daold的值，正常情况，不可能为null,所以不需要进行判断
            //通常这里不建议用in进行再处理，需要循环按唯一键进行在处理。
            //复杂的情况需要进行分析和取舍，可以选择分页首次处理的大部分字段留空，这里循环直接填空。 
            //也可以在这里重新生成新的数据集。
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                input["@jjrzh"] = dsreturn.Tables[0].Rows[i]["经纪人账号"].ToString();
                input["@spbh"] = dsreturn.Tables[0].Rows[i]["商品编号"].ToString();
                
                //累计中标次数
                string sql_ljzb = "select COUNT(*) as 累计中标 from AAA_ZBDBXXB where Z_SPBH=@spbh and (T_YSTBDGLJJRYX=@jjrzh or Y_YSYDDGLJJRYX=@jjrzh)";
                Hashtable ht_ljzb = I_DBL.RunParam_SQL(sql_ljzb, "data", input);
                if ((bool)ht_ljzb["return_float"])
                {
                    DataSet ds_ljzb = (DataSet)ht_ljzb["return_ds"];
                    if (ds_ljzb != null && ds_ljzb.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["累计中标次数"] = ds_ljzb.Tables[0].Rows[0]["累计中标"].ToString();
                    }
                }
               

                //累计定标次数
                string sql_ljdb = "select COUNT(*) as 累计定标 from AAA_ZBDBXXB where Z_SPBH=@spbh and (T_YSTBDGLJJRYX=@jjrzh or Y_YSYDDGLJJRYX=@jjrzh) and z_HTZT in ('定标','定标合同到期','定标合同终止','定标执行完成')";
                Hashtable ht_ljdb = I_DBL.RunParam_SQL(sql_ljdb, "data", input);
                if ((bool)ht_ljdb["return_float"])
                {
                    DataSet ds_ljdb = (DataSet)ht_ljdb["return_ds"];
                    if (ds_ljdb != null && ds_ljdb.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["累计定标次数"] = ds_ljdb.Tables[0].Rows[0]["累计定标"].ToString();
                    }
                }
              
                //最大卖家名称、区域
                if (dsreturn.Tables[0].Rows[i]["累计卖出金额"].ToString() != "0.00")
                {
                    string sql_sale = "select top 1  tab.*,c.I_JYFMC as 交易方名称,c.I_SSQYS+c.I_SSQYSHI+c.I_SSQYQ as 所属区域 from (select b.T_YSTBDDLYX as 交易方账号,b.Z_SPBH,sum(a.T_DJHKJE) as 卖出金额 from AAA_THDYFHDXXB as a join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null and b.T_YSTBDGLJJRYX=@jjrzh and b.Z_SPBH=@spbh group by T_YSTBDDLYX,b.Z_SPBH) as tab left join AAA_DLZHXXB as c on c.B_DLYX=tab.交易方账号 order by 卖出金额 DESC";
                    Hashtable ht_sale = I_DBL.RunParam_SQL(sql_sale, "data", input);
                    if ((bool)ht_sale["return_float"])
                    {
                        DataSet ds_sale = (DataSet)ht_sale["return_ds"];
                        if (ds_sale != null && ds_sale.Tables[0].Rows.Count > 0)
                        {
                            dsreturn.Tables[0].Rows[i]["最大卖方名称"] = ds_sale.Tables[0].Rows[0]["交易方名称"].ToString();
                            dsreturn.Tables[0].Rows[i]["最大卖方区域"] = ds_sale.Tables[0].Rows[0]["所属区域"].ToString();
                        }
                    }
                }

                //最大买家名称、区域
                if (dsreturn.Tables[0].Rows[i]["累计买入金额"].ToString() != "0.00")
                {
                    string sql_buy = "select top 1  tab.*,c.I_JYFMC as 交易方名称,c.I_SSQYS+c.I_SSQYSHI+c.I_SSQYQ as 所属区域 from (select b.Y_YSYDDDLYX as 交易方账号,b.Z_SPBH,sum(a.T_DJHKJE) as 买入金额 from AAA_THDYFHDXXB as a join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null and b.Y_YSYDDGLJJRYX=@jjrzh and b.Z_SPBH=@spbh group by b.Y_YSYDDDLYX,b.Z_SPBH) as tab left join AAA_DLZHXXB as c on c.B_DLYX=tab.交易方账号 order by 买入金额 DESC";
                    Hashtable ht_buy = I_DBL.RunParam_SQL(sql_buy, "data", input);
                    if ((bool)ht_buy["return_float"])
                    {
                        DataSet ds_buy = (DataSet)ht_buy["return_ds"];
                        if (ds_buy != null && ds_buy.Tables[0].Rows.Count > 0)
                        {
                            dsreturn.Tables[0].Rows[i]["最大买方名称"] = ds_buy.Tables[0].Rows[0]["交易方名称"].ToString();
                            dsreturn.Tables[0].Rows[i]["最大买方区域"] = ds_buy.Tables[0].Rows[0]["所属区域"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }

        //二次处理后若处理结果是空值，返回原来的数据
        if (dsreturn == null)
        {
            dsreturn = dsold;
        }
        return dsreturn;
    }
}