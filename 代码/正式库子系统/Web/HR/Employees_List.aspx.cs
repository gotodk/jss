using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Telerik.WebControls;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_HR_Employees_List : Page
{
    public string renshu;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            DefinedModule Dfmodule = new DefinedModule("HR_Employees_List");
            Authentication auth = Dfmodule.authentication;
            if (auth == null)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }

            if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }

            ViewState["name"] = '%' + TextBoxXM.Text.Trim() + '%';
            ViewState["ls"] = '%' + DropDownListLS.SelectedValue.Trim() + '%';
            ViewState["bm"] = '%' + DropDownListBM.SelectedValue.Trim() + '%';
            ViewState["paixu"] =DropDownListPX.SelectedValue.Trim().ToString ();
            ViewState["gh"] = '%' + TextBoxGH.Text.Trim() + '%';
            ViewState["xl"] = DropDownListXL.SelectedValue.Trim().ToString();
            ViewState["kstime"] = time1.Value.Trim().ToString();
            ViewState["jztime"] = time2.Value.Trim().ToString();
            ViewState["zy"] = '%' + TextBoxZY.Text.Trim().ToString() + '%';
            bangding();
            drbangding();
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["name"] = '%' + TextBoxXM.Text.Trim() + '%';
            ViewState["ls"] = '%' + DropDownListLS.SelectedValue.Trim() + '%';
            ViewState["bm"] = '%' + DropDownListBM.SelectedValue.Trim() + '%';
            ViewState["paixu"] = DropDownListPX.SelectedValue.Trim() .ToString ();
            ViewState["gh"] = '%' + TextBoxGH.Text.Trim() + '%';
            ViewState["xl"] = DropDownListXL.SelectedValue.Trim().ToString();
            ViewState["kstime"] = time1.Value.Trim().ToString();
            ViewState["jztime"] = time2.Value.Trim().ToString();
            ViewState["zy"] = '%' + TextBoxZY.Text.Trim().ToString() + '%';
            bangding();
        TextBoxXM.Text = "";
        }

    private void drbangding()
    {


        DropDownListBM.DataSource = DbHelperSQL.Query("SELECT distinct  DeptName FROM HR_Dept");
        DropDownListBM.DataTextField = "DeptName";
        DropDownListBM.DataValueField = "DeptName";
        DropDownListBM.DataBind();
        DropDownListBM.Items.Insert(0, new ListItem("全部部门", ""));

        DropDownListLS.DataSource = DbHelperSQL.Query("select distinct ls from hr_ls");
        DropDownListLS.DataTextField = "LS";
        DropDownListLS.DataValueField = "LS";
        DropDownListLS.DataBind();
        DropDownListLS.Items.Insert(0, new ListItem("全部", ""));
    }

    private void bangding()
    {
        string name = ViewState["name"].ToString();
        string ls = ViewState["ls"].ToString();
        string bm = ViewState["bm"].ToString();
        string paixu = ViewState["paixu"].ToString();
        string gh = ViewState["gh"].ToString();
        string zy = ViewState["zy"].ToString ();
        string xl = "";
        if (ViewState["xl"].ToString() == "全部")
        {
            xl = "%%";
        }
        else
        {
            xl ='%'+ViewState["xl"].ToString()+'%';
        }
        string kstime = ViewState["kstime"].ToString();
        string jztime = ViewState["jztime"].ToString();
        string sql = "";       
        if (kstime.ToString() == "" && jztime.ToString() == "")
        {
            sql = "SELECT Number, Employee_Name, Employee_Sex , LS , BM , GWMC,convert(varchar(10),Employee_BirthDay,120) as Employee_BirthDay,XL1,ZY1 FROM HR_Employees WHERE YGZT not like '%离职%' and Employee_Name LIKE '" + name + "' AND LS like '"+ls+"' AND BM LIKE '" + bm + "' and Number like '"+gh+"' and xl1 like '"+xl+"' and zy1 like '"+zy+"' order by '" + paixu + "' ";
            renshu = DbHelperSQL.GetSingle("select count(number) FROM HR_Employees WHERE number<>'admin' and YGZT not like '%离职%' and Employee_Name LIKE '" + name + "' AND LS like '" + ls + "' AND BM LIKE '" + bm + "' and Number like '" + gh + "' and xl1 like '" + xl + "' and zy1 like '" + zy + "'").ToString ();
        }
        if (kstime.ToString() != "" && jztime.ToString() == "")
        {
            Response.Write("<script language=javascript>window.alert(\"请选择截止时间！\");history.back();</script>");
        }
        if (kstime.ToString() == "" && jztime.ToString() != "")
        {
            Response.Write("<script language=javascript>window.alert(\"请选择起始时间！\");history.back();</script>");
        }
        if (kstime.ToString() != "" && jztime.ToString() != "")
        {
            sql = "SELECT Number, Employee_Name, Employee_Sex , LS , BM , GWMC,convert(varchar(10),Employee_BirthDay,120) as Employee_BirthDay,XL1,ZY1 FROM HR_Employees WHERE YGZT not like '%离职%' and Employee_Name LIKE '" + name + "' AND LS like '" + ls + "' AND  BM LIKE '" + bm + "' and Number like '" + gh + "' and xl1 like '" + xl + "' and zy1 like '" + zy + "' and Employee_BirthDay between '" + kstime + "' and '" + jztime + "' order by '" + paixu + "' ";
            renshu = DbHelperSQL.GetSingle("select count(number) from HR_Employees WHERE number<>'admin' and  YGZT not like '%离职%' and Employee_Name LIKE '" + name + "' AND LS like '" + ls + "' AND  BM LIKE '" + bm + "' and Number like '" + gh + "' and xl1 like '" + xl + "' and zy1 like '" + zy + "' and Employee_BirthDay between '" + kstime + "' and '" + jztime + "'").ToString ();
        }

        SqlDataSource1.SelectCommand = sql;
        SqlDataSource1.DataBind();
    }
    protected void RadGrid1_PageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
    }
}
