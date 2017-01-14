using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMDBHelperClass;
using System.Data;
using System.Collections;
/// <summary>
/// 多银行框架·路由类
/// 2014-07-14 shiyan 移植
/// </summary>
public class BankRoute
{
    #region 连接数据库进行查询，初始实体赋值
    public BankInit GetUserInformation(string umail)
    {
        BankInit BI = new BankInit();

        Hashtable input = new Hashtable();
        input["@dlyx"] = umail.Trim();
        string sql = "SELECT *, Rt=(CASE WHEN I_KHYH='浦发银行' THEN 'BankPufa' WHEN I_KHYH='平安银行' THEN 'BankPingan' ELSE '无法找到所属银行' END) FROM AAA_DLZHXXB WHERE B_DLYX=@dlyx";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            DataSet ds_res = (DataSet)ht_res["return_ds"];
            if (ds_res != null && ds_res.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds_res.Tables[0];
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
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    #endregion
}