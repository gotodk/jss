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
public partial class Web_XXHZX_resetpwd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");

        System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        DefinedModule module = new DefinedModule("YGZHDJ");
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
            Response.Redirect("ResetUser.aspx");
        }
        string username = Request["Number"].ToString();
                
        if(Request["dj"] == null)
        {
            DbHelperSQL.ExecuteSql("update  aspnet_Membership set Password='888888' where userId =(SELECT u.UserId  FROM HR_Employees AS e INNER JOIN aspnet_Users AS u ON e.Number = u.UserName INNER JOIN aspnet_Membership AS m ON u.UserId = m.UserId where e.Number ='" + username + "')");
            //跳转
            Response.Write("<script laguage='javascript'>");
            Response.Write("alert('重置密码成功');");
            Response.Write("window.location.href='ResetUser.aspx'");
            Response.Write("</script>");
            Response.End();
        }
        else
        {
            if(Request["dj"].ToString() == "yes")
            {
                MembershipUser mu = Membership.GetUser(username);
                mu.IsApproved = false;
                Membership.UpdateUser(mu);

                //记录操作
                string sqllog = "insert into PassReLog (OP_gh, DJ_hg, DJ_name,OP_type) values ('" + username + "', '" + User.Identity.Name + "' ,'" + Users.GetUserByNumber(User.Identity.Name).Name.ToString() + "','冻结')";
                DbHelperSQL.ExecuteSql(sqllog);
                //跳转
                Response.Write("<script laguage='javascript'>");
                Response.Write("alert('用户成功冻结');");
                Response.Write("window.location.href='ResetUser.aspx'");
                Response.Write("</script>");
                Response.End();
            }
            else
            {
                MembershipUser mu = Membership.GetUser(username);
                mu.IsApproved = true;
                Membership.UpdateUser(mu);
                //记录操作
                string sqllog = "insert into PassReLog (OP_gh, UDJ_hg, UDJ_name,OP_type) values ('" + username + "', '" + User.Identity.Name + "' ,'" + Users.GetUserByNumber(User.Identity.Name).Name.ToString() + "','解冻')";
                DbHelperSQL.ExecuteSql(sqllog);
                //跳转
                Response.Write("<script laguage='javascript'>");
                Response.Write("alert('用户成功解除冻结');");
                Response.Write("window.location.href='ResetUser.aspx'");
                Response.Write("</script>");
                Response.End();
            }

        }

    }
}
