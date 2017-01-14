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
using Telerik.WebControls;
public partial class Web_ViewLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Hesion.Brick.Core.WorkFlow.DefinedModule selfRole = new Hesion.Brick.Core.WorkFlow.DefinedModule("SystemEventView");
        Hesion.Brick.Core.WorkFlow.UserModuleAuth selfUser = new Hesion.Brick.Core.WorkFlow.UserModuleAuth();
        selfUser = selfRole.authentication.GetAuthByUserNumber(User.Identity.Name);
        if (selfUser.CanView == true)
        {

        }
        else
        {
            //转向空白页面
            Response.Write("<Script language ='javaScript'>alert('对不起,您没有操作该模块的权限!');history.back();</Script>");
            ApplicationInstance.CompleteRequest();
        }
    }


}
