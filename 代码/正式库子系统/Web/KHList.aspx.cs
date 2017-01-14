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

public partial class Web_KHList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bangding();
    }
    private void bangding()
    {
        if (Request["cs"] != null && Request["hy"] != null && Request["cs"].ToString() != "" && Request["hy"].ToString()!="")
        {
            string sql = "";
            string cs = Request["cs"].ToString();
            string hy = Request["hy"].ToString();
            sql = "SELECT Number,KHMC, SZDWXZ,DWJB,GHBL,SSHY  FROM KHGX_KHGL where Number LIKE '%" + khbh.Text.Trim() + "%' AND KHMC LIKE '%" + khmc.Text.Trim() + "%' AND city='" + cs + "' and sshy='" + hy + "' ";
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.DataBind();
            RadGrid1.DataBind();
        }

    }
    protected void EditButton_Click(object sender, EventArgs e)
    {
        bangding();
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
    }
}
