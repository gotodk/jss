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
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using FMOP.DB;

public partial class Web_XXHZX_open : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DefinedModule module = new DefinedModule("Open_User");
        Authentication auth = module.authentication;
        if (auth == null)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }
        if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }
        if (string.IsNullOrEmpty(Request["Number"]))
        {
            Response.Redirect("OpenUser.aspx");
        }
        string username = Request["Number"].ToString();
        MembershipUser user = Membership.GetUser(username);
        //user.UnlockUser();
        bool b = user.UnlockUser();
        //2010年2月4日  李又密  解锁后将密码初始化为888888
        DbHelperSQL.ExecuteSql("update  aspnet_Membership set Password='888888' where userId =(SELECT u.UserId  FROM HR_Employees AS e INNER JOIN aspnet_Users AS u ON e.Number = u.UserName INNER JOIN aspnet_Membership AS m ON u.UserId = m.UserId where e.Number ='" + username + "')");
        //跳转
        Response.Write("<script laguage='javascript'>");
        Response.Write("alert('解锁成功');");
        Response.Write("window.location.href='OpenUser.aspx'");
        Response.Write("</script>");
        Response.End();
    }
}
