using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Telerik.WebControls;

public partial class Web_HR_Employees_List_JB : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            
            drbangding();
            ViewState["ls"] = '%' + DropDownListLS.SelectedValue.Trim() + '%';
            ViewState["name"] = '%' + TextBox1.Text.Trim() + '%';            
            ViewState["bm"] = '%' + DropDownList1.SelectedValue.Trim() + '%';
            ViewState["paixu"] =DropDownList2.SelectedValue.Trim().ToString ();
            bangding();
           
            
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["name"] = '%' + TextBox1.Text.Trim() + '%';
        ViewState["ls"] = '%' + DropDownListLS.SelectedValue.Trim() + '%';
        ViewState["bm"] = '%' + DropDownList1.SelectedValue.Trim() + '%';
        ViewState["paixu"] = DropDownList2.SelectedValue.Trim() .ToString ();
        bangding();
        TextBox1.Text = "";
    }

    
    private void drbangding()
    {
        DropDownListLS.DataSource = DbHelperSQL.Query("select distinct ls from hr_ls");
        DropDownListLS.DataTextField = "LS";
        DropDownListLS.DataValueField = "LS";
        DropDownListLS.DataBind();
        DropDownListLS.Items.Insert(0, new ListItem("全部", ""));

        DropDownList1.DataSource = DbHelperSQL.Query("SELECT distinct  DeptName FROM HR_Dept");
        DropDownList1.DataTextField = "DeptName";
        DropDownList1.DataValueField = "DeptName";
        DropDownList1.DataBind();
        DropDownList1.Items.Insert(0, new ListItem("全部部门", ""));

        string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        string bm = DbHelperSQL.GetSingle(sql).ToString();
        if (bm.IndexOf("办事处") > 0)
        {
            sql = "select count(name) from system_city where name='" + bm + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                DropDownList1.DataSource = null;
                DropDownList1.DataBind();
                DropDownList1.Items.Clear();
                DropDownList1.Items.Add(bm);
                //Response.Write(bm);
                //Response.End();

                DropDownListLS.DataSource = null;
                DropDownListLS.DataBind();
                DropDownListLS.Items.Clear();
                DropDownListLS.Items.Add("驻外机构");
            }
        }
        if (bm.IndexOf("百仕加") > 0)           
        {
            DropDownListLS.DataSource = null;
            DropDownListLS.DataBind();
            DropDownListLS.Items.Clear();
            DropDownListLS.Items.Add("百仕加");

            DropDownList1.DataSource = DbHelperSQL.Query("SELECT distinct  DeptName FROM HR_Dept where Superior='百仕加'");
            DropDownList1.DataTextField = "DeptName";
            DropDownList1.DataValueField = "DeptName";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, new ListItem("全部部门", ""));
        }
    }
  

    private void bangding()
    {
        string name = ViewState["name"].ToString();
        string bm = ViewState["bm"].ToString();
        string paixu = ViewState["paixu"].ToString();
        string ls = ViewState["ls"].ToString();
        string sql = "SELECT Number, Employee_Name, Employee_Sex , LS , BM , GWMC FROM HR_Employees WHERE YGZT not like '%离职%' and Employee_Name LIKE '" + name + "' AND LS like '"+ls+"' and BM LIKE '" + bm + "' order by '"+paixu+"' ";
        SqlDataSource1.SelectCommand = sql;
        SqlDataSource1.DataBind();
    }
    protected void RadGrid1_PageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
    }

    //protected void setbumen()
    //{
    //    string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
    //    string bm = DbHelperSQL.GetSingle(sql).ToString();
    //    if (bm.IndexOf("办事处") > 0)
    //    {
    //        sql = "select count(name) from system_city where name='" + bm + "'";
    //        if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
    //        {
    //            DropDownList1.DataSource = null;
    //            DropDownList1.DataBind();
    //            DropDownList1.Items.Clear();
    //            DropDownList1.Items.Add(bm);                
    //            //Response.Write(bm);
    //            //Response.End();
    //        }
    //    }
    //}    
}
