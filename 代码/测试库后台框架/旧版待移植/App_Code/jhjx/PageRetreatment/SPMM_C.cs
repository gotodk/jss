using FMOP.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// SPMM_C 的摘要说明
/// </summary>
public class SPMM_C
{
	public SPMM_C()
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
                    //此种情况需要计算履约保证金扣罚金
                    object objLYBZJ = DbHelperSQL.GetSingle("select cast(sum(JE) as numeric(18,2)) as 履约保证金扣罚金额 from AAA_ZKLSMXB where SJLX='实' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息'  or XZ='电子购货合同履约保证金不足废标扣罚' or XZ='电子购货合同期满卖方未录入发货信息扣罚') and LYYWLX='AAA_ZBDBXXB' and LYDH='" + dsreturn.Tables[0].Rows[i]["中标定标编号"].ToString() + "'");
                    if (objLYBZJ != null && objLYBZJ.ToString() != "")
                    {
                        dsreturn.Tables[0].Rows[i]["履约保证金扣罚金额"] = objLYBZJ.ToString();
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
                    object obj_lybzj = DbHelperSQL.GetSingle("select sum(a.JE) from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.number where SJLX='预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') and LYYWLX='AAA_THDYFHDXXB' and b.ZBDBXXBBH='" + dsreturn.Tables[0].Rows[i]["中标定标编号"].ToString() + "'");
                    if (obj_lybzj != null && obj_lybzj.ToString() != "")
                    {
                        dsreturn.Tables[0].Rows[i]["履约保证金冻结剩余比例"] = ((1 - Convert.ToDouble(obj_lybzj.ToString()) / Convert.ToDouble(dsreturn.Tables[0].Rows[i]["履约保证金冻结金额"])) * 100.00).ToString("#0.00");
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
    /// 中标标签中的“是否拆单”的处理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    //public DataSet ZhongBiao(DataSet dsold)
    //{
    //    DataSet dsreturn = dsold;
    //    try
    //    {
    //        for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
    //        {
    //            string sql_cd = "";
    //            if (dsreturn.Tables[0].Rows[i]["单据类型"].ToString() == "投标单")
    //            {
    //                sql_cd = "select SFCD from AAA_TBD where number='" + dsreturn.Tables[0].Rows[i]["单据编号"].ToString() + "'";
    //            }
    //            else if (dsreturn.Tables[0].Rows[i]["单据类型"].ToString() == "预订单")
    //            {
    //                //获取是否拆单
    //                sql_cd = "select SFCD from AAA_YDDXXB where number='" + dsreturn.Tables[0].Rows[i]["单据编号"].ToString() + "'";
    //            }
    //            object obj_CD = DbHelperSQL.GetSingle(sql_cd);
    //            dsreturn.Tables[0].Rows[i]["是否拆单"] = obj_CD.ToString();
    //        }
    //    }
    //    catch(Exception ex)
    //    {
    //        //这里发生了错误，把错误输出
    //        DataTable objTable = new DataTable("二次处理错误");
    //        objTable.Columns.Add("执行错误", typeof(string));
    //        dsreturn.Tables.Add(objTable);
    //        dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
    //    }
    //    return dsreturn;
    //}

    /// <summary>
    /// 冷静期标签中的“当前卖家最低价”、“最低价标的经济批量”、“最低价标的达成率”的处理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    public DataSet LengJingQi(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                //获取当前商品的卖家最低价及相关数据
                DataSet ds_lowprice = DbHelperSQL.Query("select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,MJSDJJPL as 经济批量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' and HTQX='" + dsreturn.Tables[0].Rows[i]["合同期限"].ToString() + "'  order by TBJG,TJSJ");
                if (ds_lowprice != null && ds_lowprice.Tables[0].Rows.Count > 0)
                {
                    dsreturn.Tables[0].Rows[i]["当前卖方最低价"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                    dsreturn.Tables[0].Rows[i]["最低价标的经济批量"] = ds_lowprice.Tables[0].Rows[0]["经济批量"].ToString();

                    //获取最低价标的拟购买数量和
                    string sql = "select isnull(sum(NDGSL-YZBSL),0) from AAA_YDDXXB where SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' and  HTQX='" + dsreturn.Tables[0].Rows[i]["合同期限"].ToString() + "' and ZT = '竞标' and NMRJG >=convert(decimal(18,2),'" + ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString() + "') and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull('" + ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString() + "',''))";
                    object obj_NDGSL = DbHelperSQL.GetSingle(sql);

                    //算最低价的达成率，因为进冷静期的只是三个月/一年的合同，投标单不会出现拆单，所以达成率如下
                    dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = (Convert.ToDouble(obj_NDGSL.ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
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

    /// <summary>
    /// 竞标标签中的“当前卖家最低价”、“最低价标的经济批量”、“最低价标的达成率”的处理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    public DataSet JingBiao(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                //获取当前商品的卖家最低价及相关数据
                DataSet ds_lowprice = DbHelperSQL.Query("select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,MJSDJJPL as 经济批量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' and HTQX='" + dsreturn.Tables[0].Rows[i]["合同期限"].ToString() + "'  order by TBJG,TJSJ");
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
                        string sql = "select isnull(sum(NDGSL-YZBSL),0) from AAA_YDDXXB where SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' and  HTQX='" + dsreturn.Tables[0].Rows[i]["合同期限"].ToString() + "' and ZT = '竞标' and NMRJG >=convert(decimal(18,2),'" + ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString() + "') and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull('" + ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString() + "',''))";
                        object obj_NDGSL = DbHelperSQL.GetSingle(sql);
                        dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = (Convert.ToDouble(obj_NDGSL.ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
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

    /// <summary>
    /// 商品买卖概况标签中的“当前卖家最低价”、“最低价标的经济批量”、“最低价标的达成率”的处理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    public DataSet SPMMGK(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                #region 获取当前最低价、经济批量、达成率
                //获取当前商品的卖家最低价及相关数据
                DataSet ds_lowprice = DbHelperSQL.Query("select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,MJSDJJPL as 经济批量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' and HTQX='" + dsreturn.Tables[0].Rows[i]["合同期限"].ToString() + "'  order by TBJG,TJSJ");
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
                        string sql = "select isnull(sum(NDGSL-YZBSL),0) from AAA_YDDXXB where SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' and  HTQX='" + dsreturn.Tables[0].Rows[i]["合同期限"].ToString() + "' and ZT = '竞标' and NMRJG >=convert(decimal(18,2),'" + ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString() + "') and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull('" + ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString() + "',''))";
                        object obj_NDGSL = DbHelperSQL.GetSingle(sql);
                        dsreturn.Tables[0].Rows[i]["最低价标的达成率/中标率"] = (Convert.ToDouble(obj_NDGSL.ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                    }
                }
                #endregion
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
                        object objLJQ = DbHelperSQL.GetSingle("select SFJRLJQ from AAA_LJQDQZTXXB where SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' and HTQX='" + dsreturn.Tables[0].Rows[i]["合同期限"].ToString() + "'");
                        if (objLJQ != null && objLJQ.ToString() != "")
                        {
                            dsreturn.Tables[0].Rows[i]["当前状态"] = objLJQ.ToString() == "是" ? "竞标中（冷静期）" : "竞标中";
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
    /// 商品买卖b区中的投标单管理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    public DataSet TBDGL_Bqu(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                #region 获取当前最低价、达成率
                //获取当前商品的卖家最低价及相关数据
                DataSet ds_lowprice = DbHelperSQL.Query("select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH='" + dsreturn.Tables[0].Rows[i]["SPBH"].ToString() + "' and HTQX='" + dsreturn.Tables[0].Rows[i]["HTQX"].ToString() + "'  order by TBJG,TJSJ");
                if (ds_lowprice != null && ds_lowprice.Tables[0].Rows.Count > 0)
                {
                    dsreturn.Tables[0].Rows[i]["ZDTBJG"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                    //即时合同的达成率和三个月/一年的计算公式不同
                    if (dsreturn.Tables[0].Rows[i]["HTQX"].ToString() == "即时")
                    {
                        dsreturn.Tables[0].Rows[i]["XTDCL"] = (Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["已中标数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                    }
                    else
                    {
                        //获取最低价标的拟购买数量和
                        string sql = "select isnull(sum(NDGSL-YZBSL),0) from AAA_YDDXXB where SPBH='" + dsreturn.Tables[0].Rows[i]["SPBH"].ToString() + "' and  HTQX='" + dsreturn.Tables[0].Rows[i]["HTQX"].ToString() + "' and ZT = '竞标' and NMRJG >=convert(decimal(18,2),'" + ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString() + "') and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull('" + ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString() + "',''))";
                        object obj_NDGSL = DbHelperSQL.GetSingle(sql);
                        dsreturn.Tables[0].Rows[i]["XTDCL"] = (Convert.ToDouble(obj_NDGSL.ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                       
                    }
                    //即时/三个月/一年的合同，只有当前投标单为作为计算基础的最低价投标单时，才有我的标的达成率，其他的都为0.
                    if (dsreturn.Tables[0].Rows[i]["Number"].ToString() == ds_lowprice.Tables[0].Rows[0]["投标单号"].ToString())
                    {
                        dsreturn.Tables[0].Rows[i]["WDDCL"] = dsreturn.Tables[0].Rows[i]["XTDCL"].ToString();
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

}