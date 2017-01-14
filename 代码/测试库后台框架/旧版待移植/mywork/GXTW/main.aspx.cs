using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mywork_GXTW_main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            lblDLZH.Text = HttpUtility.UrlDecode(Request.Cookies["username"].Value) + "登录成功！";
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //FormsAuthentication.SignOut();
        //Session.RemoveAll();
        HttpCookie cookie = Request.Cookies["username"];
        if (cookie != null)
        {
            cookie.Expires = DateTime.Now.AddDays(-2);
            Response.Cookies.Set(cookie);
        }
        Response.Write("<script>window.location.href='Login.aspx'</script>"); 
        //Server.Transfer("Login.aspx",false);
    }
}