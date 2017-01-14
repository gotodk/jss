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

public partial class Web_HGJXGList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bangding();
    }
    private void bangding()
    {
        string sql = "";
        string lx = Request["lx"].ToString();
        string bh = Request["bh"].ToString();
        string kggg = txtKGGG.Text.Trim().ToString();

        KGclass KG = new KGclass();
        DataTable dt = KG.GetAllKGCanUseByWhere(bh, lx, kggg);
        //sql = "SELECT Ltrim(Rtrim(MB001)) as MB001,Ltrim(Rtrim(MB003)) as MB003 from INVMB where (MB001 like '1200%' or MB001 like '1211%') and MB001 like '%" + kgph + "%' and MB003 like '%" + kggg + "%' and MB003 not like '%作废%'";
        RadGrid1.DataSource = dt.DefaultView;
        RadGrid1.DataBind();

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
