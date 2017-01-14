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

public partial class Web_AuthenticationList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bangding();
        }
       
    }
    private void bangding()
    {
        DRAuthentication dr = new DRAuthentication();
        DataSet ds = new DataSet();
        ds = dr.getdss;

        this.RadGrid1.DataSource = ds;
        this.RadGrid1.DataBind();
    
    }

    public void ConfigureExport()
    {
       
        RadGrid1.ExportSettings.IgnorePaging =true ;
        //RadGrid1.ExportSettings.OpenInNewWindow = CheckBox3.Checked;

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        // this.RadGrid1.PagerStyle.Visible = false;
        ConfigureExport();
        RadGrid1.MasterTableView.ExportToExcel();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        // this.RadGrid1.PagerStyle.Visible = false;
        ConfigureExport();
        RadGrid1.MasterTableView.ExportToWord();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        //this.RadGrid1.PagerStyle.Visible = false;
        ConfigureExport();
        RadGrid1.MasterTableView.ExportToCSV();
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
        bangding();
    }
}
