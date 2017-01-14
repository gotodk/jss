using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_huanqian : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = Request["number"].ToString();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if(DropDownList1.SelectedValue == "更新换签编号")
        {
            DbHelperSQL.ExecuteSql("update HTCDJL set HTBH=HTBH+'H',HTJSR='',HTJSRQ=null,HTCDRQ=null  where number=" + Label1.Text);
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('换签编号重新生成成功！');window.top.frames['rightFrame'].pagesubmit('jump');window.top.frames['rightFrame'].my_closediag();</script>");
            Response.End();           
        }
        else
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.top.frames['rightFrame'].my_closediag();</script>");
            Response.End();          
        }
    }
}
