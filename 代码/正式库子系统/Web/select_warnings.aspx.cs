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
using System.Text;
using FMOP.DB;
using Telerik.WebControls;
using Telerik.RadGridDesign;

public partial class select_warnings : System.Web.UI.Page
{
    public string type;
    private void Page_Load(object sender, System.EventArgs e)
    {

        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        // 在此处放置用户代码以初始化页面
        //if (!IsPostBack)
        //{
            type= Request.QueryString["type"].ToString();
            ViewState["dr"] = this.RadioButtonList1.SelectedValue.ToString();
            ViewState["text"] = TextBox1.Text.Trim();
            btnFirst.Text = "首页";
            btnPrev.Text = "上一页";
            btnNext.Text = "下一页";
            btnLast.Text = "尾页";
            BindGrid();
            if (type == "1")
            {
                this.Label1.Text = "<font color=green>提醒信息</font>";
            }
            else if (type == "2")
            {
                this.Label1.Text = "<font color=red>报警信息</font>";
            }
       //}
       
            if (RadioButtonList1.SelectedIndex == 0)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>showone();</script>");
            }

        //绑定自定义声音
            if (!IsPostBack)
            {
                DataSet ds = DbHelperSQL.Query("select GH,SFJSDX,TXSYWJ,TXSYMC from YWPTGRPZ where GH='" + User.Identity.Name + "'");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int p = 0; p < ListBox_sy.Items.Count; p++)
                    {
                        if (ListBox_sy.Items[p].Value == ds.Tables[0].Rows[0]["TXSYWJ"].ToString() && ListBox_sy.Items[p].Text == ds.Tables[0].Rows[0]["TXSYMC"].ToString())
                        {
                            ListBox_sy.SelectedIndex = p;
                        }
                    }
                }
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

        BindGrid();
        ShowStats();
    }
    //邦定数据到grid
    public void BindGrid()
    
{
        btnFirst.Visible = true ;
        btnPrev.Visible = true;
        btnNext.Visible = true;
        btnLast.Visible = true;
        if (User.Identity.Name != null)
        {
            bool aa;
           
            string isRead=ViewState["dr"].ToString ();
            string FromUser=ViewState["text"].ToString ();
            string EmployeeNo = User.Identity.Name;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from (SELECT HR_Employees.Employee_Name, User_Warnings.Id, User_Warnings.Context, User_Warnings.Module_Url, User_Warnings.IsRead, User_Warnings.Grade, User_Warnings.FromUser, User_Warnings.Touser, User_Warnings.CreateTime,User_Warnings.IsCheckInfo,(datediff(second,getdate(),User_Warnings.LimitTime)) as limitSec ");
            sql.Append("FROM  User_Warnings Left JOIN HR_Employees ON User_Warnings.FromUser = HR_Employees.Number ");
            sql.Append("where ");
            //sql.Append(isRead);

            #region 2009.06.11 王永辉修改 服务中心账号特殊处理
            sql.Append(" User_Warnings.Grade='" + type + "' ");
            sql.Append(" and (((select Employee_Name from dbo.HR_Employees where Number=FromUser) like '%" + FromUser + "%') or  (User_Warnings.FromUser  like '%" + FromUser + "%'))  ");
            sql.Append(" and touser ='" + EmployeeNo + "'" + Hesion.Brick.Core.WorkFlow.Warning.SqlServiceMemcountdtr(true, EmployeeNo));
            sql.Append(") as c");
            sql.Append(" where IsRead = ");
            sql.Append(isRead);
            sql.Append(" and Grade='" + type + "' ");
            sql.Append(" order by CreateTime DESC ");
            
           

            #endregion
            #region 原始版本
            //sql.Append(" and touser ='" + EmployeeNo + "' and User_Warnings.Grade='"+type+"' ");
            //sql.Append(" and (((select Employee_Name from dbo.HR_Employees where Number=FromUser) like '%" + FromUser + "%') or  (User_Warnings.FromUser  like '%" + FromUser + "%'))  ");
            //sql.Append(" order by CreateTime DESC");
            #endregion
            DataSet ds = DbHelperSQL.Query(sql.ToString());
            Label2.Text = "共"+ds.Tables[0].Rows.Count.ToString() + "条";
            string dailiren = "无";
            if (Session["daililogin"] != null)
            {
                dailiren = Session["daililogin"].ToString();
            }
            Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, "", Request.UserHostAddress, "select_warnings.aspx", "", sql.ToString(),dailiren);

                MyDataGrid.DataSource = ds;

                //Response.Write(MyDataGrid.CurrentPageIndex);
                try
                {
                    MyDataGrid.DataBind();

                }
                catch
                {
                    MyDataGrid.CurrentPageIndex = 0;
                    MyDataGrid.DataBind();
                }
                ShowStats();
              if (ds.Tables[0].Rows.Count == 0)
            { 
                //Response.Write("<script>alert('无报警信息！')</script>");
                //Response.Write("<script language=javascript>window.history.back();</script>");
                btnFirst.Visible = false;
                btnPrev.Visible = false;
                btnNext.Visible = false;
               btnLast.Visible = false;
            }
        }
        else
        {
            Response.Write("<script>alert('用户未登录！')</script>");
        }
    }

    private string SqlServiceMemcountdtr(bool p, string EmployeeNo)
    {
        throw new NotImplementedException();
    }
    public void MyDataGrid_Page(object sender, DataGridPageChangedEventArgs e)
    {
        //int startIndex;
        //startIndex = MyDataGrid.CurrentPageIndex * MyDataGrid.PageSize;
        MyDataGrid.CurrentPageIndex = e.NewPageIndex;
        if (MyDataGrid.CurrentPageIndex > MyDataGrid.PageCount - 1)
        {
            MyDataGrid.CurrentPageIndex = 0;
        }
        BindGrid();
        ShowStats();
    }

    protected void MyDataGrid_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string id = e.Item.Cells[0].Text.Trim();
            string sql = "update User_warnings set isRead=1 where id='" + id + "'";
            DbHelperSQL.ExecuteSql(sql);

            string dailiren = "无";
            if (Session["daililogin"] != null)
            {
                dailiren = Session["daililogin"].ToString();
            }
            Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, "", Request.UserHostAddress, "select_warnings.aspx", id, sql, dailiren);
            string url = e.CommandArgument.ToString();
            Response.Redirect(url);
            BindGrid();
        }
    }
    protected void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        DataGridItem item = e.Item;
        ListItemType itemType = e.Item.ItemType;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (e.Item.Cells[4].Text.Trim() == "1")
            {
                e.Item.Cells[5].Text = "<font color=green>提醒</font>";
            }
            else if (e.Item.Cells[4].Text.Trim() == "2")
            {
                e.Item.Cells[5].Text = "<font color=red>报警</font>";
            }

        }
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["dr"] = this.RadioButtonList1.SelectedValue.ToString();
        ViewState["text"] = TextBox1.Text.Trim();
        BindGrid();
    }
    protected void Button123_Click(object sender, CommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        String SQL = "SELECT * FROM  User_Warnings where Id='" + id + "'";
        DataSet ds = DbHelperSQL.Query(SQL);
        string upsql = "update User_warnings set isRead=1 where id='" + id + "'";
        DbHelperSQL.ExecuteSql(upsql);
        BindGrid();
    }
    protected void ImageButton1_Command1(object sender, CommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        String SQL = "SELECT * FROM  User_Warnings where Id='"+id+"'";
        DataSet ds = DbHelperSQL.Query(SQL);
        string url = ds.Tables[0].Rows[0]["Module_Url"].ToString();
        string upsql = "update User_warnings set isRead=1 where id='" + id + "'";
        DbHelperSQL.ExecuteSql(upsql);
    	//Response.Clear();

       // 2009.06.12 王永辉修改
        if (Session["shoulilogin"] != null && Session["daililogin"] != null)
        {
            if (Session["shoulilogin"].ToString() == Session["daililogin"].ToString())
            {
                if (ds.Tables[0].Rows[0]["toUser"].ToString() != Session["shoulilogin"].ToString())
                {
                    Session["shoulilogin"] = ds.Tables[0].Rows[0]["Touser"].ToString();
                    Session["RightiframeUrl"] = url;
                    //Response.Redirect("login.aspx");
                    string StrUrl = "../login.aspx";
                    Response.Write("<script>javascript:window.parent.location.reload('" + StrUrl + "')</script>");
                    //ClientScript.RegisterStartupScript(GetType(), "warning", "<script language='javascript'>parent.rightFrame.location.href=('" + url + "');</script>");
                }
                else
                {
                    
                    ClientScript.RegisterStartupScript(GetType(), "warning", "<script language='javascript'>parent.rightFrame.location.href=('" + url + "');</script>");
                    BindGrid();
                    ShowStats();
                }
            }
            else
            {
                Session["shoulilogin"] = ds.Tables[0].Rows[0]["Touser"].ToString();
                Session["RightiframeUrl"] = url;
                Response.Redirect("login.aspx");
            }
        }


       
        //Response.Write("<script language=javascript>parent.rightFrame.location.href='" + url + "';alert(parent.rightFrame.location.href);</script>");
       // Response.End();
    }


    protected void ListBox_sy_SelectedIndexChanged(object sender, EventArgs e)
    {
            string sqluu = " update YWPTGRPZ set TXSYWJ='" + ListBox_sy.SelectedItem.Value + "',TXSYMC='" + ListBox_sy.SelectedItem.Text + "' where GH='" + User.Identity.Name + "'";
            int pp = DbHelperSQL.ExecuteSql(sqluu);
            if (pp > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "gh", "<script language='javascript'>top.document.getElementById('bgshengyin').innerHTML = '../sy/" + ListBox_sy.SelectedItem.Value + "';alert('您已将语音提醒音选定为【" + ListBox_sy.SelectedItem.Text + "】，请点击“确定”按钮试听！');Select_m123('../sy/" + ListBox_sy.SelectedItem.Value + "');</script>");
                //FMOP.Common.MessageBox.Show(Page, "您已将提醒音更换为[" + ListBox_sy.SelectedItem.Text + "]！");
            }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string upsql = "update User_warnings set isRead=1 where Touser = '" + User.Identity.Name + "'";
        DbHelperSQL.ExecuteSql(upsql);
        BindGrid();
    }
}