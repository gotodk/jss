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


public partial class Web_FFKH_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bangding();
    }
    private void bangding()
    {
        if (Request["cs"] != null && Request["cs"].ToString() != "")
        {
            string sql = "";
            string cs = Request["cs"].ToString();
            if (cs.IndexOf("办事处") > 0)
            {
                sql = "SELECT Number,KHMC,SSXSQY FROM KHGL_New where   Number LIKE '%" + khbh.Text.Trim() + "%' AND KHMC LIKE '%" + khmc.Text.Trim() + "%' AND SSXSQY='" + cs + "'  ";
            }
            else
            {
                sql = "SELECT Number,KHMC,SSXSQY FROM KHGL_New  where   Number LIKE '%" + khbh.Text.Trim() + "%' AND KHMC LIKE '%" + khmc.Text.Trim() + "%'";
            }
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.DataBind();
            RadGrid1.DataBind();
        }

    }
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        bangding();
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
    }
}
