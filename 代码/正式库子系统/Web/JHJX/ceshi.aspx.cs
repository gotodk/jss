using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_ceshi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
/*
        //判断是否有权限
        DefinedModule Dfmodule = new DefinedModule("PTRGQP");
        Authentication auth = Dfmodule.authentication;
        if (auth == null)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }

        if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }
*/
        Response.Write(User.Identity.Name);
    }
}