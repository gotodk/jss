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
using System.Data.SqlClient;
using FMOP.DB;

public partial class select_CheckInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnFirst.Text = "首页";
            btnPrev.Text = "上一页";
            btnNext.Text = "下一页";
            btnLast.Text = "尾页";
            gridbangding();
        }
    }
    private void ShowStats()
    {
        lblCurrentIndex.Text = "第 " + (MyDataGrid.CurrentPageIndex + 1).ToString() + " 页";
        lblPageCount.Text = "总共 " + MyDataGrid.PageCount.ToString() + " 页";
    }
    public void PagerButtonClick(object sender, EventArgs e)
    {
        btnPrev.Enabled = true;
        btnNext.Enabled = true;
        string arg = ((LinkButton)sender).CommandArgument.ToString();
        switch (arg)
        {
            case "next":
                if (MyDataGrid.CurrentPageIndex < (MyDataGrid.PageCount - 1))
                {
                    MyDataGrid.CurrentPageIndex += 1;
                }
                break;
            case "prev":
                if (MyDataGrid.CurrentPageIndex > 0)
                {
                    MyDataGrid.CurrentPageIndex -= 1;
                }
                break;
            case "last":
                MyDataGrid.CurrentPageIndex = (MyDataGrid.PageCount - 1);
                break;
            default:
                MyDataGrid.CurrentPageIndex = System.Convert.ToInt32(arg);
                break;
        }
        if (MyDataGrid.CurrentPageIndex == (MyDataGrid.PageCount - 1))
        {
            btnNext.Enabled = false;
        }
        else if (MyDataGrid.CurrentPageIndex == 0)
        {
            btnPrev.Enabled = false;
        }
        gridbangding();
        ShowStats();
    }
    /// <summary>
    /// 绑定datagrid
    /// </summary>
    private void gridbangding()
    {
        string name = "wutest";//获取模块名
        string sql = "SELECT * FROM system_CheckInfo WHERE  (ModuleName='" + name + "' AND IsPass=0)";
        DataSet ds = DbHelperSQL.Query(sql);

        string dailiren = "无";
        if (Session["daililogin"] != null)
        {
            dailiren = Session["daililogin"].ToString();
        }
        Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, "name", Request.UserHostAddress, "select_CheckInfo.aspx", "", sql.ToString(), dailiren);
            if (ds.Tables[0].Rows.Count != 0)
            {
                this.MyDataGrid.DataSource = ds;
                this.MyDataGrid.DataBind();
            }
            else
            {
                Response.Write("<script>alert('无审批记录！')</script>");
                Response.Write("<script language=javascript>window.history.back();</script>");
                btnFirst.Visible = false;
                btnPrev.Visible = false;
                btnNext.Visible = false;
                btnLast.Visible = false;
            }
    }
   
    public void MyDataGrid_Page(object sender, DataGridPageChangedEventArgs e)
    {
        int startIndex;
        startIndex = MyDataGrid.CurrentPageIndex * MyDataGrid.PageSize;
        MyDataGrid.CurrentPageIndex = e.NewPageIndex;
        gridbangding();
        ShowStats();
    }
    protected void MyDataGrid_ItemCommand(object source, DataGridCommandEventArgs e)
    {

    }
    protected void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        DataGridItem item = e.Item;
        ListItemType itemType = e.Item.ItemType;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (e.Item.Cells[2].Text.Trim() == "True")
            {
                e.Item.Cells[3].Text = "<font color=green>通过</font>";
            }
            else if (e.Item.Cells[2].Text.Trim() == "False")
            {
                e.Item.Cells[3].Text = "<font color=red>驳回</font>";
            }
        }
    }
}
