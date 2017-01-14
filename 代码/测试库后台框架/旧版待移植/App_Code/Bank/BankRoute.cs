using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// 多银行框架·路由类
/// </summary>
public class BankRoute
{
    #region 连接数据库进行查询，初始实体赋值
    public BankInit GetUserInformation(string umail)
    {
        BankInit BI = new BankInit();
        string sql = "SELECT *, Rt=(CASE WHEN I_KHYH='浦发银行' THEN 'BankPufa' WHEN I_KHYH='平安银行' THEN 'BankPingan' ELSE '无法找到所属银行' END) FROM AAA_DLZHXXB WHERE B_DLYX='" + umail.Trim() + "'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if ( dt==null||dt.Rows.Count < 1)
            return null;
        BI.UserEmail = dt.Rows[0]["B_DLYX"].ToString();
        BI.UserPwd = dt.Rows[0]["B_DLMM"].ToString();
        BI.UserPwd_Bank = dt.Rows[0]["B_JSZHMM"].ToString();
        BI.AccountType = dt.Rows[0]["B_JSZHLX"].ToString();
        BI.UserMoney_PT = Convert.ToDouble(dt.Rows[0]["B_ZHDQKYYE"]);
        BI.UserMoney_Bank = Convert.ToDouble(dt.Rows[0]["B_DSFCGKYYE"]);
        BI.UserType = dt.Rows[0]["I_ZCLB"].ToString();
        BI.UserDealName = dt.Rows[0]["I_JYFMC"].ToString();
        BI.IdCard = dt.Rows[0]["I_SFZH"].ToString();
        BI.OrganizationCode = dt.Rows[0]["I_ZZJGDMZDM"].ToString();
        BI.BusinessCode = dt.Rows[0]["I_YYZZZCH"].ToString();
        BI.Phone_Business = dt.Rows[0]["I_JYFLXDH"].ToString();
        BI.Phone = dt.Rows[0]["I_LXRSJH"].ToString();
        BI.UserName = dt.Rows[0]["I_LXRXM"].ToString();
        BI.Bank = dt.Rows[0]["I_KHYH"].ToString();
        BI.BankCard = dt.Rows[0]["I_YHZH"].ToString();
        BI.SecuritiesCode = dt.Rows[0]["I_ZQZJZH"].ToString();
        BI.BankState = dt.Rows[0]["I_DSFCGZT"].ToString();
        BI.Routes = dt.Rows[0]["Rt"].ToString() == "无法找到所属银行" ? null : dt.Rows[0]["Rt"].ToString();
        return BI;
    }
    #endregion
}