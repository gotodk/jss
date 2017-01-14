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

public partial class Web_DYJXHList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bangding();
    }
    private void bangding()
    {
        if (Request["cpxh"] != null && Request["cpxh"].ToString() != "")
        {
            string sql = "";
            string cpxh = Server.UrlDecode( Request["cpxh"].ToString());
            sql = "select id, dyjpp,dyjxh from web_products_SYDYJX where parentNumber =(select top 1 number from web_products where Model ='" + cpxh + "') and DYJPP LIKE '%" + dyjpp.Text.Trim() + "%' AND DYJXH LIKE '%" + dyjxh.Text.Trim() + "%'";
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.DataBind();
            RadGrid1.DataBind();
        }
        else
        {
            FMOP.Common.MessageBox.Show(Page, "请先选择产品型号！");
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
