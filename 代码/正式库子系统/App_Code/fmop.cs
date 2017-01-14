using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;

/// <summary>
///fmop 的摘要说明
/// </summary>
[WebService(Namespace = "http://fm8844.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class fmop : System.Web.Services.WebService {

    public fmop () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    /// <summary>
    /// //分公司上线后计算服务商当期信用额度
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string ws_GetCreditMoney(string UserNumber)
    {
        try
        {
            KGclass KG = new KGclass();
            double re_money = KG.GetCreditMoney(UserNumber);
            return re_money.ToString("#0.00");
        }
        catch (Exception ex)
        {
            return "发生错误："+ex.ToString();
        }
    }



    /// <summary>
    /// 获取客户列表
    /// </summary>
    /// <param name="sql">要执行的sql</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet ws_GetKH(string sql)
    {
        try
        {
            DataSet ds = FMOP.DB.DbHelperSQL.Query(sql);
            return ds;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    /// <summary>
    /// 将指定用户信用额度写入记录表备查。
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string ws_XieRuShuJu(string UserNumber,string PCH,string SSBSC)
    {
        try
        {
            string RunSFZC = "正常";
            string re_money = ws_GetCreditMoney(UserNumber);
            if (re_money.IndexOf("发生错误") >= 0)
            {
                RunSFZC = "发生错误";
                re_money = "有错误";
            }
            


            int i = FMOP.DB.DbHelperSQL.ExecuteSql("insert into aaaaaaaaaa_xinyongedu (PCH,KHBH,SSBSC,RunSFZC,money) values ('" + PCH + "','" + UserNumber + "','" + SSBSC + "','" + RunSFZC + "','" + re_money + "')");
            if (i > 0)
            {
                return "ok";
            }
            else
            {
                return "err";
            }
            
        }
        catch (Exception ex)
        {
            return "xxxxx" + ex.ToString();
        }
    }

    
}
