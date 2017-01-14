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
using FM.ManagerAccount;
public partial class Web_ChangPass : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
    {
        //修改密码
       // String userName = "";
        //String userPassword = "";
        //String sqlCmd = "UPDATE FS_ME_Users SET UserPassword=@UserPassword WHERE UserName=@UserName";

        //userName = this.User.Identity.Name;
        //userPassword = ChangePassword1.NewPassword.ToString();
        //if (!userPassword.Equals(""))
        //{ 
            //DBproperty objProperty = new DBproperty();
            //objProperty.userName = userName;
            //objProperty.userPassword = Encrypt.getCryptograph(userPassword);

            //DBConnect conn = new DBConnect();
            //conn.Update(sqlCmd, objProperty);
        //}
    }
}
