using FMOP.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// JJRYWGL_C 的摘要说明
/// </summary>
public class JJRYWGL_C
{
	public JJRYWGL_C()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
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

        try
        {
            //进行在处理,daold的值，正常情况，不可能为null,所以不需要进行判断
            //通常这里不建议用in进行再处理，需要循环按唯一键进行在处理。
            //复杂的情况需要进行分析和取舍，可以选择分页首次处理的大部分字段留空，这里循环直接填空。 
            //也可以在这里重新生成新的数据集。
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                //累计卖出金额
                if (dsreturn.Tables[0].Rows[i]["结算账户类型"].ToString() == "经纪人交易账户")
                {
                    dsreturn.Tables[0].Rows[i]["累计卖出金额"] = "--";
                }
                else
                {
                    object objLJMC = DbHelperSQL.GetSingle("select isnull(sum(a.T_DJHKJE),0.00) from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where b.T_YSTBDDLYX = '" + dsreturn.Tables[0].Rows[i]["交易方账号"].ToString() + "'  and b.T_YSTBDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString() + "' and a.F_BUYWYYSHCZSJ is not null");
                    if (objLJMC != null && objLJMC.ToString() != "")
                    {
                        dsreturn.Tables[0].Rows[i]["累计卖出金额"] = objLJMC.ToString();
                    }
                }
                //累计买入金额
                object objLJMR = DbHelperSQL.GetSingle("select isnull(sum(a.T_DJHKJE),0.00) from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where b.Y_YSYDDDLYX = '" + dsreturn.Tables[0].Rows[i]["交易方账号"].ToString() + "'  and b.T_YSTBDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString() + "' and a.F_BUYWYYSHCZSJ is not null");
                if (objLJMR != null && objLJMR.ToString() != "")
                {
                    dsreturn.Tables[0].Rows[i]["累计买入金额"] = objLJMR.ToString();
                }
                //累计竞标次数
                object objLJTB = DbHelperSQL.GetSingle("select count(*) from AAA_TBD where DLYX = '" + dsreturn.Tables[0].Rows[i]["交易方账号"].ToString() + "' and GLJJRYX='" + dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString() + "'");
                object objLJYDD = DbHelperSQL.GetSingle("select count(*) from AAA_YDDXXB where DLYX = '" + dsreturn.Tables[0].Rows[i]["交易方账号"].ToString() + "' and GLJJRYX='" + dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString() + "'");
                dsreturn.Tables[0].Rows[i]["累计竞标次数"] = (Convert.ToInt64(objLJTB.ToString()) + Convert.ToInt64(objLJYDD.ToString())).ToString();
                
                //累计中标次数
                object objLJZB = DbHelperSQL.GetSingle("select count(*) from AAA_ZBDBXXB where (Y_YSYDDDLYX = '" + dsreturn.Tables[0].Rows[i]["交易方账号"].ToString() + "' and Y_YSYDDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString() + "') or (T_YSTBDDLYX = '" + dsreturn.Tables[0].Rows[i]["交易方账号"].ToString() + "' and T_YSTBDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString() + "')");
                dsreturn.Tables[0].Rows[i]["累计中标次数"] = objLJZB.ToString();

                //已发货待付款金额
                object objYFHDFK = DbHelperSQL.GetSingle("select isnull(sum(a.T_DJHKJE),0.00) from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where ((b.T_YSTBDDLYX ='" + dsreturn.Tables[0].Rows[i]["交易方账号"].ToString() + "' and b.T_YSTBDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString() + "') or (b.Y_YSYDDDLYX='" + dsreturn.Tables[0].Rows[i]["交易方账号"].ToString() + "' and b.Y_YSYDDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["关联经纪人账号"].ToString() + "')) and a.F_WLXXLRSJ is not null and a.F_BUYWYYSHCZSJ is null ");
                if (objYFHDFK != null && objYFHDFK.ToString() != "")
                {
                    dsreturn.Tables[0].Rows[i]["已发货待付款金额"] = objYFHDFK.ToString();
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

        try
        {
            //进行在处理,daold的值，正常情况，不可能为null,所以不需要进行判断
            //通常这里不建议用in进行再处理，需要循环按唯一键进行在处理。
            //复杂的情况需要进行分析和取舍，可以选择分页首次处理的大部分字段留空，这里循环直接填空。 
            //也可以在这里重新生成新的数据集。
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                //累计中标次数
                object objLJZB = DbHelperSQL.GetSingle("select COUNT(*) from AAA_ZBDBXXB where Z_SPBH='"+dsreturn .Tables [0].Rows [i]["商品编号"].ToString ()+"' and (T_YSTBDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["经纪人账号"].ToString() + "' or Y_YSYDDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["经纪人账号"].ToString() + "')");
                if (objLJZB != null && objLJZB.ToString() != "")
                {
                    dsreturn.Tables[0].Rows[i]["累计中标次数"] = objLJZB.ToString();
                }

                //累计定标次数
                object objLJDB = DbHelperSQL.GetSingle("select COUNT(*) from AAA_ZBDBXXB where Z_SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' and (T_YSTBDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["经纪人账号"].ToString() + "' or Y_YSYDDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["经纪人账号"].ToString() + "') and z_HTZT in ('定标','定标合同到期','定标合同终止','定标执行完成')");
                if (objLJDB != null && objLJDB.ToString() != "")
                {
                    dsreturn.Tables[0].Rows[i]["累计定标次数"] = objLJDB.ToString();
                }
                //最大卖家名称、区域
                if (dsreturn.Tables[0].Rows[i]["累计卖出金额"].ToString() != "0.00")
                {
                    DataSet ds_sale = DbHelperSQL.Query("select top 1  tab.*,c.I_JYFMC as 交易方名称,c.I_SSQYS+c.I_SSQYSHI+c.I_SSQYQ as 所属区域 from (select b.T_YSTBDDLYX as 交易方账号,b.Z_SPBH,sum(a.T_DJHKJE) as 卖出金额 from AAA_THDYFHDXXB as a join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null and b.T_YSTBDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["经纪人账号"].ToString() + "' and b.Z_SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' group by T_YSTBDDLYX,b.Z_SPBH) as tab left join AAA_DLZHXXB as c on c.B_DLYX=tab.交易方账号 order by 卖出金额 DESC");
                    if (ds_sale != null && ds_sale.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["最大卖方名称"] = ds_sale.Tables[0].Rows[0]["交易方名称"].ToString();
                        dsreturn.Tables[0].Rows[i]["最大卖方区域"] = ds_sale.Tables[0].Rows[0]["所属区域"].ToString();
                    }
                }               

                //最大买家名称、区域
                if (dsreturn.Tables[0].Rows[i]["累计买入金额"].ToString() != "0.00")
                {
                    DataSet ds_buy = DbHelperSQL.Query("select top 1  tab.*,c.I_JYFMC as 交易方名称,c.I_SSQYS+c.I_SSQYSHI+c.I_SSQYQ as 所属区域 from (select b.Y_YSYDDDLYX as 交易方账号,b.Z_SPBH,sum(a.T_DJHKJE) as 买入金额 from AAA_THDYFHDXXB as a join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null and b.Y_YSYDDGLJJRYX='" + dsreturn.Tables[0].Rows[i]["经纪人账号"].ToString() + "' and b.Z_SPBH='" + dsreturn.Tables[0].Rows[i]["商品编号"].ToString() + "' group by b.Y_YSYDDDLYX,b.Z_SPBH) as tab left join AAA_DLZHXXB as c on c.B_DLYX=tab.交易方账号 order by 买入金额 DESC");
                    if (ds_buy != null && ds_buy.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["最大买方名称"] = ds_buy.Tables[0].Rows[0]["交易方名称"].ToString();
                        dsreturn.Tables[0].Rows[i]["最大买方区域"] = ds_buy.Tables[0].Rows[0]["所属区域"].ToString();
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