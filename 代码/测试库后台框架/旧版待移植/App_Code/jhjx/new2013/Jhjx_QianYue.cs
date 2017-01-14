/***************************************************************
 * 创建者：wyh
 * 
 * 创建时间：2014.04.14
 * 
 * 功能：客户签约功能
 * 
 * 
 * 
 * 参考文档：无
 * *************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Jhjx_QianYue 的摘要说明
/// </summary>
public class Jhjx_QianYue
{
	public Jhjx_QianYue()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public static DataSet DsSendQianYue(DataSet dsReturn, DataSet dsPara)
    {
        if (dsPara != null && dsReturn.Tables[0].Rows.Count > 0)
        {
            DataRow Drow = dsPara.Tables[0].Rows[0];
            IBank bank = new IBank(Drow["用户邮箱"].ToString(), "签约");
            string info = string.Empty;
            DataSet dsResult = bank.Invoke(dsPara, ref info);
            if (dsResult != null)
            {
                if (dsResult.Tables["str"].Rows[0]["状态信息"] == "ok")
                {
                    dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "签约成功！";
                }
                else
                {
                    dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = dsResult.Tables["str"].Rows[0]["状态信息"].ToString();
                }
            }
            else
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = info;
            }

        }
        else
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的必要参数为空，程序执行出错！";
        }
        return dsReturn;
    }
}