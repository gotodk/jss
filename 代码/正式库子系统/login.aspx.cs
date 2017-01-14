using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Data.SqlClient;


public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitPage();
        }
    }
    #region //周丽 2012-08-21
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.QueryString["bl"] != null && Request.QueryString["bl"]!="")
        {
            divUpdatePassWord.Visible = true;
            lblUser.InnerText = Request.QueryString["bl"].ToString();
            txtOldPW.Focus();
        }
        else
        {
            divUpdatePassWord.Visible = false;
        }
        
    }
    #endregion

    
    /// <summary>
    /// 登陆事件 2009.06.10 王永辉
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        //GetSessionKey();
        TextBox tbUserName = Login1.FindControl("UserName") as TextBox;
        string StrGetState = "select YGZT FROM HR_Employees WHERE Number = '" + tbUserName.Text.ToString().Trim() + "'";
        object YGZT = DbHelperSQL.GetSingle(StrGetState);
        if (YGZT != null && YGZT.ToString() != "离职")
        {
            Login login = (Login)sender;
            //第一登录人员

            Session["shoulilogin"] = login.UserName.ToString();
            Session["daililogin"] = login.UserName;
            if (Session["daililogin"] != null)
            {
                FMEvengLog.SaveToLog(login.UserName, "login.aspx", GetIp(), "登录系统", "", "", Session["daililogin"].ToString());
            }
            else
            {
                FMEvengLog.SaveToLog(login.UserName, "login.aspx", GetIp(), "登录系统", "", "", "");

            }           
        }
        else
        {
            //FMOP.Common.MessageBox.Show(Page, "对不起，您没有权限登陆系统！");
            Response.Write("<script>javascript:alert('对不起，您没有权限登陆系统！');window.parent.location.reload('" + "../../loginNew.aspx" + "')</script>");
            Response.End();
        }
    }

    /// <summary>
    /// 获取客户端IP  王永辉 2009.06.10
    /// </summary>
    /// <returns></returns>
    string GetIp()
    {

        //可以透过代理服务器

        string userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        string UserTest = Request.UserHostAddress.ToString();

        if (userIP == null || userIP == "")
        {

            //没有代理服务器,如果有代理服务器获取的是代理服务器的IP

            userIP = Request.ServerVariables["REMOTE_ADDR"];

        }

        return userIP;

    }
    /// <summary>
    /// 初始化页面
    /// </summary>
    private void InitPage()
    {
        string StrUserName1 = "";
        string StrVersion = "";
        if (Request.QueryString["StrUserName"] != null && Request.QueryString["StrUserName"].ToString() != "")
        {
            if (Request.QueryString["StrVersion"] != null && Request.QueryString["StrVersion"].ToString() != "")
            {
                (Login1.FindControl("UserName") as TextBox).Text = Request.QueryString["StrUserName"].ToString();
                (Login1.FindControl("DropDownList1") as DropDownList).SelectedValue = Request.QueryString["StrVersion"].ToString();
                Loginin(Request.QueryString["StrUserName"].ToString(), "");

            }
            
        }
        else
        {
            //if (Request.QueryString["Password"] != null && Request.QueryString["StrPassWord"].ToString() != "")
            //{
            //    (Login1.FindControl("DropDownList1") as DropDownList).Items.FindByValue(Request.QueryString["StrVersion"].ToString());

            //}

            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() != "")
            {
                Session["RightiframeUrl"] = "ServiceCenter/Bussnessdaili.aspx";
            }

        }
        if ((Request.QueryString["shoulilogin"] != null && Request.QueryString["shoulilogin"].ToString() != ""))
        {
            string shoulilogin_str = DES.DESDecrypt(Request.QueryString["shoulilogin"].ToString(), "TMDkey22", "TMDiv222"); ;
            Session["shoulilogin"] = shoulilogin_str;
            Loginin(shoulilogin_str, "");
        }

    }


    /// <summary>
    /// 跳过登陆页面   2009.06.10 王永辉
    /// </summary>
    /// <param name="userID">登陆人账号</param>
    /// <param name="logUrl">登陆后转向URL</param>
    private void Loginin(string userID, string logUrl)
    {
        if (userID != "" && userID != null)
        {
            if (logUrl == "")
            {
                logUrl = "Default.aspx";
            }
            System.Web.Security.FormsAuthentication.SetAuthCookie(userID, false);

            //string yourURL = "../ServiceCenter/Bussnessdaili.aspx";
            Response.Redirect(logUrl);

            //string test = "<script language='javascript'>window.location.href=('" + logUrl + "');document.rightFrame.location.href=('" + yourURL + "');</script>";
            //Response.Write("<script language='javascript'>rightFrame.src=('" + yourURL + "');</script>");
        }
    }

    
    protected void Login1_LoginError(object sender, EventArgs e)
    {
        try
        {
            Login l = (Login)sender;
            MembershipUser u = Membership.GetUser(l.UserName);
            if (u == null)
            {
                l.FailureText = "您的登录尝试不成功，请重试。";
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >document.getElementById('FailureTextDiv').style.display = 'inline-block';</script>");
                return;
            }
            if (!u.IsApproved)
            {
                l.FailureText = "您的账户已经被冻结";
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >document.getElementById('FailureTextDiv').style.display = 'inline-block';</script>");
                return;
            }
            if (u.IsLockedOut)
            {
                l.FailureText = "您的帐户已经被锁定";
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >document.getElementById('FailureTextDiv').style.display = 'inline-block';</script>");
                return;
            }
            //RequiredFieldValidator require = (l.FindControl("UserNameRequired") as RequiredFieldValidator);
            //require.Text = "您的登录尝试失败";
            //require.Visible = true;
            // 默认错误消息提示FailureTextDiv
            l.FailureText = "登录失败，请您核实账户信息，5次失败将被锁定！";
             //Page.RegisterStartUpScript("key",@"getElementById('div').style.display = 'none';")或者=""就可以咯！
            this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >document.getElementById('FailureTextDiv').style.display = 'inline-block';</script>");
               
        }
        catch { }
       
    }

    #region //周丽 2012-08-21 确定更改密码
    protected void btnOK_Click(object sender, EventArgs e)
    {
        //string strSQL = "select a.Password from dbo.aspnet_Membership a , aspnet_Users b "+
        //        "where a.UserId=b.UserId and a.ApplicationId=b.ApplicationId and b.UserName='"+
        //        lblUser.InnerText.ToString() + "'";//根据账号查询密码
        //    object oldpw = DbHelperSQL.GetSingle(strSQL);
        //    if (oldpw != null && oldpw != "")
        //    {
        //        if (oldpw.ToString() == txtOldPW.Text.ToString())
        //        {
        //            string pw = txtNewPassWord.Text.ToString();
        //            string qrpw = txtQRNewPW.Text.ToString();
        //            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"\d+[a-zA-Z]+|[a-zA-Z]+\d+");

        //            if (pw == qrpw)
        //            {
        //                if (pw.Length > 6 && pw.Length < 12 && reg.IsMatch(pw.ToString()))
        //                {
        //                    string strUpateSQL = "UPDATE [dbo].[aspnet_Membership] SET [Password] = '" + pw
        //                        + "' WHERE [UserId] = (select distinct UserId from aspnet_Users  where UserName='" + lblUser.InnerText.ToString() + "')";
        //                    int i = DbHelperSQL.ExecuteSql(strUpateSQL);
        //                    if (i > 0)
        //                    {
        //                        span.InnerText = "修改成功！请重新登陆！";
        //                        Response.Redirect("login.aspx");
        //                    }
        //                    else
        //                    {
        //                        span.InnerText = "修改失败！";
        //                    }

        //                }
        //                else
        //                {
        //                    span.InnerText = "请按安全规则修改密码！";
        //                }
        //            }
        //            else
        //            {
        //                span.InnerText = "两次输入密码不相同，请重新设定密码！";
        //                txtQRNewPW.Text = "";
        //                txtNewPassWord.Text = "";
        //            }
        //        }
        //        else
        //        {
        //            span.InnerText = "原始密码输入错误！";
        //        }
        //    }
        //    else
        //    {
        //        span.InnerText = "账号错误！";
        //    }

        string pw = txtNewPassWord.Text.ToString();
        string strUpateSQL = "UPDATE [dbo].[aspnet_Membership] SET [Password] = '" + pw
                                + "' WHERE [UserId] = (select distinct UserId from aspnet_Users  where UserName='" + lblUser.InnerText.ToString() + "')";
        int i = DbHelperSQL.ExecuteSql(strUpateSQL);
        if (i > 0)
        {
            span.InnerText = "修改成功！请重新登陆！";
            Response.Redirect("login.aspx?ud=ok");
        }
        else
        {
            span.InnerText = "修改失败！";
        }
    }
    //取消
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divUpdatePassWord.Visible = false;
    }
    //关闭按钮
    protected void imgbtnClose_Click(object sender, ImageClickEventArgs e)
    {
        divUpdatePassWord.Visible = false;
    }
    #endregion
}