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

public partial class Web_XXHZX_OpenUser : System.Web.UI.Page
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
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        RadGrid1.CurrentPageIndex = e.NewPageIndex;
        SqlDataSource1.DataBind();
    }
}
