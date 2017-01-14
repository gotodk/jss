using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
///YHZTC_tixing 的摘要说明
/// </summary>
public class YHZTC_tixing
{
	public YHZTC_tixing()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 用户直通车提醒发送，用户、推广、服务都用这个（字段都不能是null）。如用户端发往推广人员的，则填写两方帐号，同时服务站点留空，并标记发往对象即可。
    /// </summary>
    /// <param name="YHZH">用户帐号</param>
    /// <param name="TGRYZH">推广人员帐号</param>
    /// <param name="FWZDZH">服务站点帐号</param>
    /// <param name="SFXSG">是否显示过(插入时，默认否)</param>
    /// <param name="TXXXNR">提醒详细内容</param>
    /// <param name="FWDX">发往对象(用户,推广人员,服务站点)</param>
    /// <param name="TXLX">提醒类型(下达订单,交付旧硒鼓,出售旧硒鼓,交易中心,合格旧硒鼓调拨,其他)</param>
    public void SendForYHZTC(string YHZH, string TGRYZH, string FWZDZH, string SFXSG, string TXXXNR, string FWDX, string TXLX)
    {
        WorkFlowModule WFM = new WorkFlowModule("FM_YHZTCTX");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        string sql_insert = "insert into FM_YHZTCTX(number,YHZH,TGRYZH,FWZDZH,SFXSG,TXXXNR,FWDX,TXLX,CheckState,CreateUser) values ('" + KeyNumber + "','" + YHZH + "','" + TGRYZH + "','" + FWZDZH + "','" + SFXSG + "','" + TXXXNR + "','" + FWDX + "','" + TXLX + "',1,'admin' )";
        DbHelperSQL.ExecuteSql(sql_insert);
    }

    /// <summary>
    /// 向业务平台发送提醒。 在[用户直通车]-[提醒人设置] 中增加提醒人。分为两种情况,如果员工编号为空，则向岗位批量发送提醒
    /// </summary>
    /// <param name="bm">部门名称</param>
    /// <param name="modname">模块名称</param>
    /// <param name="msg">提醒信息内容</param>
    /// <param name="url">提醒点击查看后跳转到的URL</param>
    public void SendForFMOP(string bm, string modname, string msg, string url)
    {
        DataSet ds_touser = DbHelperSQL.Query("select * from FM_TXRGL where txmk='" + modname + "' and BM='" + bm + "'");
        if (ds_touser.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds_touser.Tables[0].Rows.Count; i++)
            {
                //分为两种情况,如果员工编号为空，则向岗位批量发送提醒。
                if (ds_touser.Tables[0].Rows[i]["ygbh"].ToString() == "")
                {
                    string ssql = "select number,employee_name,bm,gwmc from HR_employees where  ygzt not like '%离职%' and gwmc='" + ds_touser.Tables[0].Rows[i]["GW"].ToString() + "' and BM='" + ds_touser.Tables[0].Rows[i]["BM"].ToString() + "'";
                    DataSet ds_touser2 = DbHelperSQL.Query(ssql);
                    if (ds_touser2 != null && ds_touser2.Tables[0].Rows.Count > 0)
                    {
                        for (int t = 0; t < ds_touser2.Tables[0].Rows.Count; t++)
                        {
                            string touser = ds_touser2.Tables[0].Rows[t]["number"].ToString();
                            WorkFlowModule wf = new WorkFlowModule(modname);
                            wf.authentication.InsertWarnings(msg, url, "1", "", touser);
                        }
                    }

                }
                else
                {
                    string touser = ds_touser.Tables[0].Rows[i]["ygbh"].ToString();
                    WorkFlowModule wf = new WorkFlowModule(modname);
                    wf.authentication.InsertWarnings(msg, url, "1", "", touser);
                }
            }
        }
    }





    /// <summary>
    /// 插入用户直通车提醒
    /// </summary>
    /// <param name="YHZH">用户账号</param>
    /// <param name="FWSZH">服务商账号</param>
    /// <param name="TXCSRQ">提醒产生日期</param>
    /// <param name="SFXSG">是否显示过</param>
    /// <param name="TXXXNR">详细提醒内容</param>
    /// <param name="TXLX">提醒类型</param>
    public void insertTX(string YHZH, string FWSZH, string TXCSRQ, string SFXSG, string TXXXNR, string TXLX)
    {
        WorkFlowModule WFM = new WorkFlowModule("YHZTC_TIXING");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        string sql_insert = "insert into YHZTC_TIXING(number,YHZH,FWSZH,TXCSRQ,SFXSG,TXXXNR,TXLX) values ('" + KeyNumber + "','" + YHZH + "','" + FWSZH + "','" + TXCSRQ + "','" + SFXSG + "','" + TXXXNR + "','" + TXLX + "' )";
        DbHelperSQL.ExecuteSql(sql_insert);
    }
    //插入用户直通车提醒（增加提醒类型）
    public void insertTX(string YHZH, string FWSZH, string TXCSRQ, string SFXSG, string TXXXNR, string TXLX, string XXTXLX)
    {
        WorkFlowModule WFM = new WorkFlowModule("YHZTC_TIXING");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        string sql_insert = "insert into YHZTC_TIXING(number,YHZH,FWSZH,TXCSRQ,SFXSG,TXXXNR,TXLX,XXTXLX) values ('" + KeyNumber + "','" + YHZH + "','" + FWSZH + "','" + TXCSRQ + "','" + SFXSG + "','" + TXXXNR + "','" + TXLX + "','" + XXTXLX + "' )";
        DbHelperSQL.ExecuteSql(sql_insert);
    }
}