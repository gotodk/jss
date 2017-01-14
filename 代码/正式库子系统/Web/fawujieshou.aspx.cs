using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FMOP.DB;
using System.Net.Mail;
using System.Net;
using System.ComponentModel;
using Mailer.Components;
using System.IO;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;

public partial class Web_fawujieshou : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.Name == "6860696")
        {
            DbHelperSQL.ExecuteSql("update HTCDJL set HTJSR='梁连好',HTJSRQ=getdate(),HTCDRQ=getdate() where number=" + Request["number"].ToString());
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.top.frames['rightFrame'].my_closediag();</script>");
            Response.End();
        }

        else if (User.Identity.Name == "6789679")
        {
            DbHelperSQL.ExecuteSql("update HTCDJL set HTJSR='陈婷婷',HTJSRQ=getdate(),HTCDRQ=getdate() where number=" + Request["number"].ToString());
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.top.frames['rightFrame'].my_closediag();</script>");
            Response.End();
        }
        else
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.top.frames['rightFrame'].my_closediag();</script>");
            Response.End();
        }
    }
}
