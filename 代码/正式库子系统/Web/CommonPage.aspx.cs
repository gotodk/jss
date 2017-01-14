using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using RTXServerApi;
using RTXClient;
using RTXCAPILib;
using FMOP.DB;
using System.Data.SqlClient;
using FMOP.Module;

public partial class CommonPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //读取近三天的考勤记录，以确定是否进行提醒
        DateTime time = DateTime.Now;
        //string sql = "select top 5 ID, SJKSSJ,SJJSSJ,convert(char(4), JSSJ)+ '-' +substring( JSSJ,5,2)+ '-' +substring( JSSJ,7,2) dt from KQCQ where (convert(smalldatetime ,convert(char(4), JSSJ)+ '-' +substring( JSSJ,5,2)+ '-' " +
        //  " +substring( JSSJ,7,2)+' ' +substring( JSSJ,9,2)+': ' +substring( JSSJ,11,2)+': ' +substring( JSSJ,13,2)) BETWEEN '"
        //  + time.AddDays(-3).ToShortDateString() + "' AND '" + time.ToShortDateString() + "')";

        //从考勤统计里查询是否是正常的工作日
        string sql = "select ID,convert(char(4), JSSJ)+ '-' +substring( JSSJ,5,2)+ '-' +substring( JSSJ,7,2) dt from KQCQ where (convert(smalldatetime ,convert(char(4), JSSJ)+ '-' +substring( JSSJ,5,2)+ '-' " +
           " +substring( JSSJ,7,2)+' ' +substring( JSSJ,9,2)+': ' +substring( JSSJ,11,2)+': ' +substring( JSSJ,13,2)) BETWEEN '"
           + time.AddDays(-3).ToShortDateString() + "' AND '" + time.ToShortDateString() + "')"
           + " and EMPNo='" + User.Identity.Name + "' and convert(char(8), JSSJ) not in (select HOLDATE from HOLIDAY)";
        SqlDataReader reader = DbHelperSQL.GetKQ(sql);
        while (reader.Read())
        {           
            //从考勤记录表里查询统计每天的上下班时间

            DateTime dt = Convert.ToDateTime(reader["dt"].ToString()).AddDays(1);
            string riqi = dt.Year + "-" + dt.Month + "-" + dt.Day + " 0:00:00";
            string str = "select substring(max(sktime),9,2)a,substring(min(sktime),9,2)i,max(convert(char(4), sktime)+ '-' +substring( sktime,5,2)+ '-'" +
                " +substring( sktime,7,2)+' ' +substring( sktime,9,2)+': ' +substring( sktime,11,2))mx,min(convert(char(4), sktime)+ '-' +substring( sktime,5,2)+ '-' "+
                " +substring( sktime,7,2)+' ' +substring( sktime,9,2)+': ' +substring( sktime,11,2)) ni from YSSKKQ_xxx  where empno='"+User.Identity.Name+"' and  "+
                " convert(smalldatetime ,convert(char(4), sktime)+ '-' +substring( sktime,5,2)+ '-' "+
                " +substring( sktime,7,2)+' ' +substring( sktime,9,2)+': ' +substring( sktime,11,2)+': ' +substring( sktime,13,2))>='"+reader["dt"].ToString()+"' "+
                " and convert(smalldatetime ,convert(char(4), sktime)+ '-' +substring( sktime,5,2)+ '-'  +substring( sktime,7,2)+' ' +substring( sktime,9,2)+': ' "+
                " +substring( sktime,11,2)+': ' +substring( sktime,13,2))<'" + riqi + "'";
            SqlDataReader r = DbHelperSQL.GetKQ(str);

            //Response.Write(reader["dt"].ToString() + "</ br>" + riqi + "</ br>");

            if (r.Read())
            {
                //Response.Write(str  + "<br />");
                //if (r["mx"] == DBNull.Value)
                //{
                //    Response.Write("mx_null" + "<br />");
                //}
                //else
                //{
                //    Response.Write("aa" + r["mx"].ToString() + "bb" + "<br />");
                //}
                //if (r["ni"] == DBNull.Value)
                //{
                //    Response.Write("ni_null" + "<br />");
                //}
                //else
                //{
                //    Response.Write("aa" + r["ni"].ToString() + "bb" + "<br />");
                //}
                //Response.Write("aa" + r["i"].ToString() + "bb" + "<br />");
                //Response.Write("aa" + r["a"].ToString() + "bb" + "<br />");

                //mx为下班时间，ni为上班时间,为空说明当天未打卡，两者相等说明只打了一次卡，只是上班或者下班打卡
                if ((r["mx"] == DBNull.Value) || (r["ni"] == DBNull.Value) || (r["mx"].ToString() == r["ni"].ToString()) || (Convert.ToInt32(r["i"]) > 12) || ((Convert.ToInt32(r["a"]) < 12)))
                {
                    //该记录是否已经写到提醒表里


                    string strsql = "select Id from User_Warnings where Module_Url like '%Id=" + reader["ID"].ToString() + "'";
                    int Rcount = DbHelperSQL.QueryInt(strsql);
                    if (Rcount == 0)
                    {
                        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule("SJHZZGX");
                        //wf.authentication.InsertWarnings("有新的个人信息自助更新，工号为" + User.Identity.Name + "，请及时查看处理。", "WorkFlow_View.aspx?module=HR_Employees_modify", "1", User.Identity.Name.Trim(), touser);
                        wf.authentication.InsertWarnings(reader["dt"].ToString()+"打卡异常(仅提醒前三天)", "HR/Kaoqinguanli/KQDetail.aspx?Id=" + reader["ID"].ToString() + "", "1", "系统", User.Identity.Name.Trim());
                    }
                    r.Close();
                    continue;
                }
               
            }
            else
            {
                r.Close();
                continue;
            }
            r.Close();
            //string content = "";
            //if (reader["SJKSSJ"] == DBNull.Value || reader["SJJSSJ"] == DBNull.Value)
            //{
            //    content =reader["dt"].ToString()+"打卡异常";
            //}
           
        }
        reader.Close();

        //galaxy添加，用于自动登录邮箱。.......................................................................................
        //验证是否添加过邮箱账号与工号的对应关系
        //没有添加，跳转到添加页面

        //已经添加过，自动登录邮箱
        string userName = User.Identity.Name;
        string upsql = "select * from YXGHYSB where YGGH='" + userName + "'";
        DataSet ds = DbHelperSQL.Query(upsql);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["emaillogin"] = "yes";
            string YX_str = ds.Tables[0].Rows[0]["FMYXZHBBHHZ"].ToString();
            Session["mail_url"] = "http://192.168.0.4:8080/default_fm.asp?uname=" + YX_str + "&pword=1";
            //Response.Write("<script>window.parent.frames.f2.location.href = \"http://192.168.0.4:8080/default_fm.asp?uname=" + YX_str + "&pword=1\";</script>");
            //Response.Write("<script>window.open (\"http://192.168.0.4:8080/default_fm.asp\", \"newwindow\", \"height=0, width=0, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no\");</script>");
            //Response.Write("http://192.168.0.4:8080/default_fm.asp?uname=" + YX_str + "&pword=1");
            //Response.Write("<script>window.parent.frames.f2.location.href = \"autologinemail.html?un=" + YX_str + "&pw=1\";</script>");
            
        }
        else
        {
            Session["emaillogin"] = null;
        }


    
        //Response.Write("<script>window.parent.hideIframe0.location = \"www.google.cn\";</script>");
        //........................................................................................................................

        //Response.Write("<script><javascript:window.open('../RTXntegration/hidertxlogin.aspx?Sessionkey=" + GetSessionKey() + "')></script>");
        //GetSessionKey();

        #region RTX登陆

        /*2009.07.09  王永辉添加 RTX登陆*/
        
        //try
        //{
        //    if (Session["daililogin"] != null && Session["daililogin"].ToString() != "")
        //    {
        //        RTXSevera obiRtxSever = new RTXSevera();

        //        /*这里需要一个业务操作平台账号跟RTX账号的对应暂时没有*/
        //        if (GetRtxPass() != "")
        //        {
        //            string UserID = GetRtxPass();
        //            string severIP = ConfigurationManager.AppSettings["RtxseverIP"].ToString();
        //            if (!Page.IsStartupScriptRegistered("RTXClientLogin"))
        //            {
        //                Page.RegisterStartupScript("RTXClientLogin", obiRtxSever.GenerateClientLoginScript(UserID, severIP));
        //            }
        //        }
        //        else
        //        {
        //            FMOP.Common.MessageBox.Show(Page, "您的RTX未能登陆，可能是您还未分配RTX账号！");
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //     FMOP.Common.MessageBox.Show(Page, ex+"可能是您还未分配RTX账号！");
        //}
     
        #endregion 
    }


    //private string GetRtxPass()
    //{
    //    string StrRtx = "";
    //    string StrSql = "select RTXZH from YXGHYSB where YGGH = '" + Session["daililogin"].ToString().Trim()+"'";
    //    DataSet ds = DbHelperSQL.Query(StrSql);
    //    if (ds.Tables[0].Rows.Count >0)
    //    {
    //        StrRtx = ds.Tables[0].Rows[0]["RTXZH"].ToString();
    //    }

    //    return StrRtx.Trim();
    //}

   
}
