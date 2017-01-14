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

public partial class Web_fawujiechu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (User.Identity.Name != "")
        {
            DbHelperSQL.ExecuteSql("update HTCDJL set HTJSR='',HTJSRQ=null,HTCDRQ=null,HTBH=HTBH+'JC' where number=" + Request["number"].ToString());
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('解除合同成功！');window.top.frames['rightFrame'].my_closediag();</script>");
            Response.End();
        }
        else
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.top.frames['rightFrame'].my_closediag();</script>");
            Response.End();
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('未进行操作！');window.top.frames['rightFrame'].my_closediag();</script>");
        Response.End();
    }
}
