using System;
using System.Collections.Generic;
using System.Web;
using FMOP.DB;
/// <summary>
///FWSZXQYSD 的摘要说明
/// </summary>
public class FWSZXQYSD
{
    public DateTime GetDealDueDate(string TimeToSetDeal)
    {
        //不管什么时间签约，到期时间都为次年的三月1号。
        DateTime SetDate = Convert.ToDateTime(TimeToSetDeal);
        string year = SetDate.AddYears(1).Year.ToString();
        string DueDate = year + "-03-01";
        return Convert.ToDateTime(DueDate);
    }

    /// <summary>
    /// 根据所属办事处和签约类型获取签约模板（续签、换签）
    /// </summary>
    /// <param name="SSBSC"></param>
    /// <param name="DealType"></param>
    public string GetDealTemplate(string SSBSC,string DealType)
    {
        string Temp = "";
        if (DealType == "续签")
        {
            Temp = DbHelperSQL.GetSingle("SELECT XQMBLJ FROM FGS_QYDYMBB WHERE SSBSC='" + SSBSC + "'").ToString().Trim();//得到当前办事处签约的模板路径
        }
        else if (DealType == "换签")
        {
            Temp = DbHelperSQL.GetSingle("SELECT HQMBLJ FROM FGS_QYDYMBB WHERE SSBSC ='" + SSBSC + "'").ToString().Trim();//同上
        }
        else
        {
            Temp = null;
        }
        return Temp;
    }
}