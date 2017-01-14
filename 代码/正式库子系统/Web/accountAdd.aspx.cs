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
using System.Text;
using FM.ManagerAccount;
using Hesion.Brick.Core;
public partial class Web_accountAdd : System.Web.UI.Page
{
    DBConnect accountConn = null;
    DBproperty accountPro = null;
    StringBuilder sqlcmd = new StringBuilder();
  
    /// <summary>
    /// 新增账号
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        String username;
        if (Request["number"] != null && Request["number"].ToString() != "")
        {
            username = Request["number"].ToString();
            setProperty(username);
            buildSqlObj();
            accountConn = new DBConnect();
            accountConn.Insert(sqlcmd.ToString(), accountPro);
            //Response.Redirect("http://192.168.0.64:6060/lybbs1/fmjy_add.asp?number=" + username);
            //Response.End();
        }
        else
        {MessageBox.Show(this, "参数错误，保存失败！");}

        Response.Redirect("WorkFlow_Add.aspx?module=HR_Employees");

    }

    private void setProperty(String username)
    {
        accountPro = new DBproperty();
        accountPro.userName = username;
        accountPro.userNo = username;
        accountPro.EMAIL = username;
    }

    private void buildSqlObj()
    {
        sqlcmd.Append("INSERT INTO FS_ME_Users");
        sqlcmd.Append("(UserNumber,UserName,UserPassword,PassQuestion,PassAnswer,safeCode,Email,IsCorporation,Integral,FS_Money,RegTime,GroupID,isLock)");
        sqlcmd.Append("VALUES");
        sqlcmd.Append("(@UserNumber,@UserName,@UserPassword,@PassQuestion,@PassAnswer,@safeCode,@Email,@IsCorporation,@Integral,@FS_Money,@RegTime,@GroupID,@isLock)");
    }
}
