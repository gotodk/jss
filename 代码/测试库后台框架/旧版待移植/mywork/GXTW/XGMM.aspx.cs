using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class mywork_GXTW_XGMM : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSeave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNewPassWord.Text.Trim()))
        {
            lblTS.Text = "新密码不能为空！";            
            return;
        }
        string DLZH = "";
        HttpCookie cookie = Request.Cookies["username"];
        if (cookie != null)
        {
            DLZH = HttpUtility.UrlDecode(cookie.Value);
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('登录时间过长，请重新登录！');window.open('Login.aspx','_top')</script>");
            return;
        }

        string strupdate = "Update AAA_PTGLJGB set GLBMMM='" + txtNewPassWord.Text.Trim() + "' where GLBMZH='" + DLZH.Trim() + "'";
        int i = DbHelperSQL.ExecuteSql(strupdate);
        if (i > 0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('密码修改成功！');</script>");            
            txtNewPassWord.Text = ""; 
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('密码修改失败！');</script>");
        }
    }
}