using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class login_ajax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            GetPW();
        }
        
    }
    protected void GetPW()
    {
        string username = Request.Params["sl"].ToString();
        string strSQL = "select a.Password from dbo.aspnet_Membership a , aspnet_Users b " +
               "where a.UserId=b.UserId and a.ApplicationId=b.ApplicationId and b.UserName='" +
               username.ToString() + "'";//根据账号查询密码
        object oldpw = DbHelperSQL.GetSingle(strSQL);
        Response.Write(oldpw.ToString());
        Response.End();
    }
}