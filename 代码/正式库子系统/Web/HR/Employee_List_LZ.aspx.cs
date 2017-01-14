using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Telerik.WebControls;

public partial class Web_HR_Employee_List_LZ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["name"] = '%' + TextBox1.Text.Trim() + '%';
            ViewState["bm"] = '%' + DropDownList1.SelectedValue.Trim() + '%';
            ViewState["paixu"] = DropDownList2.SelectedValue.Trim().ToString();
            bangding();
            drbangding();
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["name"] = '%' + TextBox1.Text.Trim() + '%';
        ViewState["bm"] = '%' + DropDownList1.SelectedValue.Trim() + '%';
        ViewState["paixu"] = DropDownList2.SelectedValue.Trim().ToString();
        bangding();
        TextBox1.Text = "";
    }

    private void drbangding()
    {
        DropDownList1.DataSource = DbHelperSQL.Query("SELECT distinct  DeptName FROM HR_Dept");
        DropDownList1.DataTextField = "DeptName";
        DropDownList1.DataValueField = "DeptName";
        DropDownList1.DataBind();
        DropDownList1.Items.Insert(0, new ListItem("全部部门", ""));
    }
    private void bangding()
    {
        string name = ViewState["name"].ToString();
        string bm = ViewState["bm"].ToString();
        string paixu = ViewState["paixu"].ToString();
        string sql = "SELECT Number, Employee_Name, Employee_Sex , LS , BM , GWMC FROM HR_Employees WHERE YGZT like '%离职%' and Employee_Name LIKE '" + name + "' AND BM LIKE '" + bm + "' order by '" + paixu + "' ";
        SqlDataSource1.SelectCommand = sql;
        SqlDataSource1.DataBind();
    }
    protected void RadGrid1_PageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
    }
}
