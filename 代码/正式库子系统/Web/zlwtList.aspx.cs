using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Web_zlwtList : System.Web.UI.Page
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
            string cs = Server.UrlDecode(Request["cs"].ToString());
            sql = "SELECT Number,KHMC from KHGL_New where Number LIKE '%" + zdkhbh.Text.Trim() + "%' AND KHMC LIKE '%" + zdkhmc.Text.Trim() + "%' AND SSXSQY='" + cs + "'";
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
