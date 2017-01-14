using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FMOP.DB;

/// <summary>
/// ZJCXCqu 的摘要说明
/// </summary>
public class ZJCXCqu
{
	public ZJCXCqu()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    
    public Hashtable GetZJCXinfo(string index,string type)
    {
        Hashtable htresult = new Hashtable();
        htresult["执行结果"] = "";
        htresult["返回数据集"] = "";
        

        string sql = "select XM, XZ as XZvalue,(case when LEN (XZ)>10 then SUBSTRING (xz,0,11)+'...' else XZ end) as XZtext from AAA_moneyDZB where 1=1 ";
        if (index != "" && index != "0")
        {
            sql = sql + " and dymkbh='" + index + "'";
        }
        if (type.ToString() != "")
        {
            sql = sql + " and sjlx='" + type + "'";
        }
        sql = sql + " order by dymkbh";
        try
        {
            DataSet ds = DbHelperSQL.Query(sql);
            htresult["执行结果"] = "ok";
            htresult["返回数据集"] = ds.Tables[0];     
        }
        catch
        {
            htresult["执行结果"] = "err";
        }

        return htresult;
    }

    public Hashtable GetTongJi(string dlyx,string type)
    {
        Hashtable htresult = new Hashtable();
        htresult["执行结果"] = "ok";
        htresult["提示文本"] = "";
        htresult["当前冻结"] = "0.00";
        htresult["当前余额"] = "0.00";
        htresult["当前收益"] = "0.00";


        //获取当前冻结余额
        try
        {
            string sql_dj = "select isnull(sum(case yslx when '冻结' then je when '解冻' then -je end),0.00) as 当前冻结 from AAA_ZKLSMXB where dlyx='" + dlyx + "' and yslx in ('冻结','解冻') ";
            object objDongJie = DbHelperSQL.GetSingle(sql_dj);
       
            if (objDongJie != null)
            {
                htresult["当前冻结"] = objDongJie.ToString();
            }
        }
        catch
        {
            htresult["执行结果"] = "err";
            htresult["提示文本"] = "未获取到当前冻结金额！";
        }
        //获取当前可用资金余额
        try
        {
            ClassMoney2013 clm = new ClassMoney2013();
            double Money = clm.GetMoneyT(dlyx, "");
            htresult["当前余额"] = Money.ToString("#0.00");
        }
        catch
        {
            htresult["执行结果"] = "err";
            htresult["提示文本"] =htresult["提示文本"]+ " 未获取到当前可用资金余额！";
        }

        //获取当前经纪人收益金额
        if (type.IndexOf ("经纪人")>=0)
        {
            try
            {
                string sql_sy = "select isnull(sum(case yslx when '增加' then je when '扣减' then -je end),0.00) as 当前收益 from AAA_ZKLSMXB where dlyx='" + dlyx + "' and yslx in ('增加','扣减') and sjlx='实' and xm='经纪人收益'  ";
                object objShouYi = DbHelperSQL.GetSingle(sql_sy);
                if (objShouYi != null)
                {
                    htresult["当前收益"] = objShouYi.ToString();
                }
            }
            catch
            {
                htresult["执行结果"] = "err";
                htresult["提示文本"] = htresult["提示文本"] + " 未获取经纪人可支取收益！";
            }
        }
        return htresult;
    }
}