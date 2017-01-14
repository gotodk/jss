using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_ChangePassNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

      
        }
    }
   
   
   
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string pw = txtNewPassWord.Text.ToString();
        string qrpw = txtQRNewPW.Text.ToString();
        string strUpateSQL = "UPDATE [dbo].[aspnet_Membership] SET [Password] = '" + pw
                           + "' WHERE [UserId] = (select distinct UserId from aspnet_Users  where UserName='" + User.Identity.Name.ToString() + "')";
        int i = DbHelperSQL.ExecuteSql(strUpateSQL);
        if (i > 0)
        {
            span.InnerText = "恭喜您！修改成功！";
          
        }
        else
        {
            span.InnerText = "服务器异常！修改失败！";
        }
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >document.getElementById('span').style.display = 'inline-block';</script>");
    }
}