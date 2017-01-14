using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Data.SqlClient;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_Default : System.Web.UI.Page
{
    User user = null;
    public string gg_str = "";
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //更新用户个人配置表
        DataSet ds = DbHelperSQL.Query("select GH,SFJSDX,TXSYWJ,TXSYMC from YWPTGRPZ where GH='" + User.Identity.Name + "'");
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            this.bgshengyin.InnerHtml = "../sy/"+ds.Tables[0].Rows[0]["TXSYWJ"].ToString();
        }
        else
        {
            DbHelperSQL.ExecuteSql("insert into YWPTGRPZ (Number,GH,SFJSDX,TXSYWJ,TXSYMC,CheckState,CreateUser) values ('" + Guid.NewGuid().ToString() + "', '" + User.Identity.Name + "', '不接受','001.wav','默认音', '1', '" + User.Identity.Name + "')");
        }
        

        //更新登陆错误次数

        string strSql_ppgg1231312 = "update aspnet_Membership set  FailedPasswordAttemptCount = 0 where Email like '%" + User.Identity.Name + "%'";
        DbHelperSQL.Query(strSql_ppgg1231312);

        //显示公告
        //gg_str = "<li><a href=\"#\">用户拜访记录表已使用</a></li><li><a href=\"#\">绿装推广分析已使用</a></li><li><a href=\"#\">新版员工档案已使用</a></li><li><a href=\"#\">事务审批单正在开发</a></li><li><a href=\"#\">复方记录正在开发</a></li>";
        string strSql_ppgg = "select GGLX,GGMC,GGLJ,SFXS,LJLX,'创建时间'=Convert(varchar(10),CreateTime,120) from BBGXGGGL where SFXS = '是' order by CreateTime DESC ";
        DataSet ds_ppgg = DbHelperSQL.Query(strSql_ppgg);
        if (ds_ppgg != null && ds_ppgg.Tables[0].Rows.Count > 0)
        {
            for(int y = 0 ; y < ds_ppgg.Tables[0].Rows.Count;y++)
            {
                gg_str = gg_str + "<li><a href=\"" + ds_ppgg.Tables[0].Rows[y]["GGLJ"].ToString() + "\" target=\"" + ds_ppgg.Tables[0].Rows[y]["LJLX"].ToString() + "\" title=\"" + ds_ppgg.Tables[0].Rows[y]["GGMC"].ToString() + "(" + ds_ppgg.Tables[0].Rows[y]["创建时间"].ToString() + ")[" + ds_ppgg.Tables[0].Rows[y]["GGLX"].ToString() + "]\">" + leftx(ds_ppgg.Tables[0].Rows[y]["GGMC"].ToString(), 26) + "...</a></li>";
            }
        }
        else
        {
            gg_str = gg_str = "<li><a href=\"/Web/commonpage.aspx\" target=\"rightFrame\">暂无更新公告</a></li>";
        }


        RadAjaxTimer1.AutoStart = true;
  
      
        //ViewState["warncount"] = Hesion.Brick.Core.WorkFlow.Warning.GetWarningCount(User.Identity.Name);
        ViewState["remindcount"] = Hesion.Brick.Core.WorkFlow.Warning.GetRemindCount(User.Identity.Name);

    
        string strWeek = "";
        switch (DateTime.Now.DayOfWeek)
        { 
            case DayOfWeek.Sunday:
                strWeek = "星期日";
                break;
            case DayOfWeek.Monday:
                strWeek = "星期一";
                break;
            case DayOfWeek.Tuesday:
                strWeek = "星期二";
                break;
            case DayOfWeek.Wednesday:
                strWeek="星期三";
                break;
            case DayOfWeek.Thursday:
                strWeek = "星期四";
                break;
            case DayOfWeek.Friday:
                strWeek = "星期五";
                break;
            case DayOfWeek.Saturday:
                strWeek = "星期六";
                break;
        
        }
        this.lblServerTime.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm") + " " + strWeek;

       
        if ((int) ViewState["remindcount"] > 0)
        {
            WarningWindow.NavigateUrl = "select_warnings.aspx?type=1";
            WarningWindow.VisibleOnPageLoad = true;
        }


        this.divtitle.InnerText = " " + ConfigurationManager.AppSettings["Title"].ToString().Trim();
       this.divtitle.Style.Add("font-size","40");
       this.divtitle.Style.Add("color", "white");
       this.divtitle.Style.Add("font-family", "宋体");//text-align font-weight
       this.divtitle.Style.Add("text-align", "left");
       this.divtitle.Style.Add("font-weight", "bolder");
    }

    #region //周丽 2012-08-21
    protected void Page_init(object sender, EventArgs e)
    {
        if (User.Identity.Name != "admin" && User.Identity.Name != "")
        {
            string strSQL = "select a.Password from dbo.aspnet_Membership a , aspnet_Users b " +
                "where a.UserId=b.UserId and a.ApplicationId=b.ApplicationId and b.UserName='" +
                User.Identity.Name.ToString() + "'";//根据账号查询密码
            object pw = DbHelperSQL.GetSingle(strSQL);
            if (pw != null && pw != "")
            {
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"\d+[a-zA-Z]+|[a-zA-Z]+\d+");
                if (pw.ToString().Length <= 6 || pw.ToString().Length >20)
                {
                    Session.RemoveAll();
                    FormsAuthentication.SignOut();//真正退出代码
                    Response.Redirect("../login.aspx?bl="+User.Identity.Name);
                }
                if (!reg.IsMatch(pw.ToString()))
                {
                    Session.RemoveAll();
                    FormsAuthentication.SignOut();//真正退出代码
                    Response.Redirect("../login.aspx?bl=" + User.Identity.Name);
                }
            }

        }
    }
    #endregion

    protected void exitbutton_Click(object sender, EventArgs e)
    {
        //退出时关闭窗口
        Response.Write("<script language=javascript>window.opener=null;window.open('','_self','','');window.close();</script>");
    }
    protected void RadAjaxTimer1_Tick(object sender, Telerik.WebControls.TickEventArgs e)
    {
      
        //每隔5秒钟，比较一次，是否有新提醒报警，发现新提醒、报警弹出短信窗口
        int tmpWarn = Hesion.Brick.Core.WorkFlow.Warning.GetWarningCount(User.Identity.Name);
        int tmpRemind = Hesion.Brick.Core.WorkFlow.Warning.GetRemindCount(User.Identity.Name);
        //if (tmpWarn > (int)ViewState["warncount"])
        //{
        //    RadAjaxPanel1.ResponseScripts.Add("ShowWarnForm();");
            
        //}
        if (tmpRemind > (int)ViewState["remindcount"])
        {
            RadAjaxPanel1.ResponseScripts.Add("ShowRemindForm();");
        }

        //ViewState["warncount"] = tmpWarn;
        ViewState["remindcount"] = tmpRemind;
    }

    protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        //记录用户退出的信息到日志系统
        FMEvengLog.SaveToLog(User.Identity.Name, "login.aspx", Request.UserHostAddress, "退出系统", "", "","");
        Session.RemoveAll();
    }


    protected void LoginStatus2_LoggedOut(object sender, EventArgs e)
    {

        // 2009.06.10 王永辉 修改
        Session.RemoveAll();
        Response.Write("<script language=javascript>window.opener=null;window.open('','_self','','');window.close();</script>");
        Response.End();
    }
     

    /// <summary>
    ///  2009.06.10 王永辉 添加 为切换数据源按钮提供URL
    /// </summary>
    /// <returns></returns>
    public void  GetUrl()
    {
        string StrUrl = "../../login.aspx?shoulilogin=" + DES.DESEncrypt(Session["daililogin"].ToString(),"TMDkey22","TMDiv222"); 
        //if (this.hidUserID.Value != null)
        //{
        ////    string test = Session["daililogin"].ToString();
        //    Session["shoulilogin"] = this.hidUserID.Value.ToString();
        //    Urlstr = Urlstr + "&shoulilogin=" + this.hidUserID.Value.ToString();
           
        //}
        ////if (Session["RightiframeUrl"] != null)
        ////{
        ////    Session["RightiframeUrl"] = "ServiceCenter/Bussnessdaili.aspx";
        ////}

        Response.Write("<script>javascript:window.parent.location.reload('" + StrUrl + "')</script>");
        //if (Session["daililogin"] != null)
        //{
        //    Urlstr = Urlstr + "&shoulilogin=" + Session["daililogin"].ToString();
        //}
        //return Urlstr;
    }


    /// <summary>
    /// 判断是否显示切换数据源按钮  2009.06.10 王永辉 添加
    /// </summary>
    private void ChagUserShow()
    {
        if (Session["daililogin"] != null)
        {
            string strSql = "select * from QXSZXXXX as a left join RightFenpei as b on b.Number = a.ParentNumber where b.YGBH ='" + Session["daililogin"].ToString() + "'";
            DataSet ds = DbHelperSQL.Query(strSql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                //this.TDshouliren.Visible = true;
                //this.TDshouliren0.Visible = true;
                this.TDUserchange.Visible = true;
                this.TDUserchange0.Visible = true;
                //this.tdReturn.Visible = true;
                //this.tdReturn0.Visible = true;
            }
            else
            {
                //this.TDshouliren.Disabled = false;
                //this.TDshouliren0.Visible = false;
                this.TDUserchange.Disabled = false;
                this.TDUserchange0.Disabled = false;
                //this.tdReturn.Visible = false;
                //this.tdReturn0.Visible = false;
            }

            //if (userName.Text.ToString() == lblshouliren.Text)
            //{
            //    //this.TDshouliren.Visible = false;
            //    //this.TDshouliren0.Visible = false;
            //}
            //else
            //{
            //    //this.TDshouliren.Visible = true;
            //    //this.TDshouliren0.Visible = true;
            //}
        }
        else
        {
            Response.Write("<script>alert('连接服务器超时，请重新连接！');</script>");
            Response.Write("<script>javascript:window.parent.location.reload('" + "../../login.aspx" + "')</script>");
        }

    }

    /// <summary>
    /// 初始化 Dedault 页面
    /// </summary>
    private void InitPage()
    {

        if (Session["shoulilogin"] != null)
        {
            string logUrl = "Default.aspx";

            System.Web.Security.FormsAuthentication.SetAuthCookie(Session["shoulilogin"].ToString(), false);
            Response.Redirect(logUrl);
        }
    }

    public string GetRighUrl()
    {
        string RightiframeUrl = "commonpage.aspx";
        if (Session["RightiframeUrl"] != null)
        {
            RightiframeUrl = Session["RightiframeUrl"].ToString();
        }


        return RightiframeUrl;

    }


    ///str_value 字符
    ///str_len 要截取的字符长度
    public string leftx(string str_value, int str_len)
    {
        int p_num = 0;
        int i;
        string New_Str_value = "";

        if (str_value == "")
        {
            New_Str_value = "";
        }
        else
        {
            int Len_Num = str_value.Length;



            //if (Len_Num < str_len)
            //{
            // str_len = Len_Num;
            //}


            for (i = 0; i <= Len_Num - 1; i++)
            {
                //str_value.Substring(i,1);
                if (i > Len_Num) break;
                char c = Convert.ToChar(str_value.Substring(i, 1));
                if (((int)c > 255) || ((int)c < 0))
                {
                    p_num = p_num + 2;

                }
                else
                {
                    p_num = p_num + 1;

                }

                if (p_num >= str_len)
                {

                    New_Str_value = str_value.Substring(0, i + 1);

                    break;
                }
                else
                {
                    New_Str_value = str_value;
                }

            }

        }
        return New_Str_value;
    }


    protected void btnReturn_Click(object sender, EventArgs e)
    {
        string StrUrl = "../../login.aspx?shoulilogin=" + DES.DESEncrypt(Session["daililogin"].ToString(), "TMDkey22", "TMDiv222");
        Response.Write("<script>javascript:window.parent.location.reload('" + StrUrl + "')</script>");
    }

    public string GetRtxPass()
    {

          if(Session["niubi"] == null)
                {
  Session["niubi"] = "pass";
}
         
        if(Session["daililogin"] == null || Session["niubi"].ToString() == "nopass")
     {

return "";
}
        string StrRtx = "";
        string StrSql = "select RTXZH from YXGHYSB where YGGH = '" + Session["daililogin"].ToString().Trim() + "'";
        DataSet ds = DbHelperSQL.Query(StrSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            StrRtx = ds.Tables[0].Rows[0]["RTXZH"].ToString();
        }
        Session["niubi"] = "nopass";
        return DES.DESEncrypt(StrRtx.Trim(), "TMDkey22", "TMDiv222");

        
    }

   
}
