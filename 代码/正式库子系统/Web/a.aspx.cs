using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;


public partial class Web_a : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (User.Identity.Name != "")
        {
            DbHelperSQL.ExecuteSql("update YHZTC_KTQWBSQ set SFTG='是',SHR='" + User.Identity.Name + "',SHSJ=getdate() where number='" + Request["number"].ToString() + "'");
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('设置成功！');window.top.frames['rightFrame'].my_closediag();</script>");
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