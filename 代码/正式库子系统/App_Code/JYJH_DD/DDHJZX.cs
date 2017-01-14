using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using FMOP.DB;

/// <summary>
/// DDHJZX 的摘要说明
/// </summary>
public class DDHJZX
{
	public DDHJZX()
	{

	}
    /// <summary>
    /// 取得XML文件
    /// </summary>
    /// <param name="TB_INFO_ID">订单号</param>
    /// <param name="TB_STATE">订单状态</param>
    /// <param name="TB_CHANGE_AGEN">当前用户</param>
    /// <param name="TB_CHANGE_DATE">当前时间</param>
    /// <param name="type">0:1</param>
    /// <returns></returns>
    private string getxml(string TB_INFO_ID, string TB_STATE, string TB_CHANGE_AGEN, string TB_CHANGE_DATE, string type)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"gb2312\"?>");
        sb.AppendLine("<ORDER_INFO>");
        sb.AppendLine("	<ORDER_INFO_ID>" + TB_INFO_ID + "</ORDER_INFO_ID>");  //订单号
        sb.AppendLine("	<ORDER_STATE>" + TB_STATE + "</ORDER_STATE>"); //订单状态
        sb.AppendLine("	<ORDER_CHANGE_AGENT>" + TB_CHANGE_AGEN + "</ORDER_CHANGE_AGENT>");//当前用户
        sb.AppendLine("	<ORDER_CHANGE_DATE >" + TB_CHANGE_DATE + "</ORDER_CHANGE_DATE>"); //当前时间
        sb.AppendLine("<OPERATE_TIMERS>" + type + "</OPERATE_TIMERS>");//第一次生成销售单为0，其他状态为1
        sb.AppendLine("</ORDER_INFO>");
        return sb.ToString();
    }
    /// <summary>
    /// 订单发送呼叫中心
    /// </summary>
    /// <param name="ddh">订单号</param>
    /// <param name="zt">订单状态</param>
    /// <param name="user">发送人(当前用户)</param>
    /// <param name="sj">当前时间</param>
    /// <param name="type">0:1</param>
    /// <returns></returns>
    public string ddsendhj(string ddh, string zt, string user, string sj, int type)
    {
        string ms = "";
        if (ddh != "" && zt != "" && user != "" && sj != "" && type.ToString() != "")
        {
            string sb = getxml(ddh, zt, user, sj, type.ToString());
            upGNKH_Order(ddh, zt);
            CallCenterWebService.callCenterWebService cws = new CallCenterWebService.callCenterWebService();
            try
            {
                int result = cws.updateOrder(sb, "201");
                if (result == 0)//成功，不成功也更新订单状态
                {
                    ms = "并成功发送至呼叫中心系统";
                    //return ms;
                }
                else
                {
                    ms = "未能将信息发送至呼叫中心系统";
                }
            }
            catch
            {
                ms = "未能发送至呼叫中心系统但信息已在本地保存";
            }
        }
        return ms;
    }

    /// <summary>
    /// 根据销售单发送呼叫中心
    /// </summary>
    /// <param name="xsdh">销售单号</param>
    /// <param name="zt">订单状态</param>
    /// <param name="user">发送人(当前用户)</param>
    /// <param name="sj">当前时间</param>
    /// <param name="type">0:1</param>
    /// <returns></returns>
    public string xsdsendhj(string xsdh, string zt, string user, string sj, int type)
    {
        string ms = "";
        if (xsdh != "" && zt != "" && user != "" && sj != "" && type.ToString() != "")
        {
            string sb = getxml(getddh(xsdh), zt, user, sj, type.ToString());
            upGNKH_Order(getddh(xsdh), zt);
            CallCenterWebService.callCenterWebService cws = new CallCenterWebService.callCenterWebService();
            try
            {
                int result = cws.updateOrder(sb, "201");
                if (result == 0)//成功，不成功也更新订单状态
                {
                    ms = "并成功发送至呼叫中心系统";
                    //return ms;
                }
                else
                {
                    ms = "未能将信息发送至呼叫中心系统";
                }
            }
            catch
            {
                ms = "未能发送至呼叫中心系统但信息已在本地保存";
            }
        }
        return ms;
    }
    private string getddh(string xsdh)
    {
        string ddh = "";
        DataSet ds = DbHelperSQL.Query("select ddh from yxsyb_cpxsd where number='" + xsdh + "'");
        if (ds.Tables[0].Rows.Count > 0)
            ddh = ds.Tables[0].Rows[0]["ddh"].ToString();
        return ddh;
    }
    private void upGNKH_Order(string number, string zt)
    {
        string upsql = "update GNKH_Order set DDZT='" + zt + "' where Number='" + number + "' ";
        DbHelperSQL.ExecuteSql(upsql);
    }

   
}
