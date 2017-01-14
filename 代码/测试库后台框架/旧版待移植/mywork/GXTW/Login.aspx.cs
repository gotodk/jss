using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class mywork_GXTW_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(User.Identity.Name);
        
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        
        try
        {
            lblTS.Text = "";
            string UserName = txtUserName.Text.Trim();
            string PassWord = txtPassWord.Text.Trim();
            if (string.IsNullOrEmpty(UserName))
            {
                lblTS.Text = "用户名不能为空！";
                return;
            }
            if (string.IsNullOrEmpty(PassWord))
            {
                lblTS.Text = "密码不能为空！";
                return;
            }
            string str = "select * from AAA_PTGLJGB  where GLBMZH='" + UserName.Trim() + "' and GLBMMM='" + PassWord + "'";
            DataSet ds = DbHelperSQL.Query(str);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["SFYX"].ToString() == "否")
                {
                    lblTS.Text = "请输入有效的账号！";
                    return;
                }
                HttpCookie cook = new HttpCookie("username", HttpUtility.UrlEncode(UserName));
                Response.Cookies["username"].Expires = DateTime.Now.AddDays(1); 
                HttpContext.Current.Response.Cookies.Add(cook); 

                //FormsAuthentication.RedirectFromLoginPage(UserName, false);
                //FormsAuthentication.SignOut();
                Response.Redirect("main.aspx", false);
            }
            else
            {
                lblTS.Text = "用户名或密码错误！";
                txtPassWord.Text = "";
            }
        }
        catch (Exception ex)
        {
            //throw new Exception(ex.Message);

        }
    }
}