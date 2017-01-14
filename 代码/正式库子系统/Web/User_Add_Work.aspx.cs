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
using System.Xml;
using FMOP.DB;
//using FMOP.RR;
using System.Text;

public partial class User_Add_Work : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            gridbangding();
        }
    }
    //查询语句
    private void gridbangding()
    {
        if (User.Identity.Name != null)
        {
            string EmployeeNo = User.Identity.Name;
            //EmployeeNo = "01";//用户测试
            string jobno = drJobName(EmployeeNo);
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT name,Title,ModuleType, ");
            sql.Append(" (SELECT COUNT(Id) AS Expr1 ");
            sql.Append("FROM User_work ");
            sql.Append("where (name = module.name) ");
            sql.Append("AND (EmployeeNo = '");
            sql.Append(EmployeeNo);
            sql.Append("')) ");
            sql.Append("AS isSelect ");
            sql.Append("FROM system_Modules AS module ");
            sql.Append("WHERE ");
            sql.Append("( ");
            sql.Append("Configuration");
            sql.Append(".exist(");
            sql.Append("'//Authentication/*");
            sql.Append("[roleName=");
            sql.Append(jobno);
            sql.Append("]') = 1)");
            DataSet drname = DbHelperSQL.Query(sql.ToString());
            if (drname.Tables[0].Rows.Count != 0)
            {
                this.dgShow.DataSource = drname;
                this.dgShow.DataBind();
            }
            else
            {
                Response.Write("<script>alert('无工作内容！')</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('用户未登录！')</script>");
        }
    }
    //执行全选
    public void CheckAll(object sender, System.EventArgs e)
    {
        //
        CheckBox cbAll = (CheckBox)sender;
        if (cbAll.Text == "全选")
        {
            foreach (DataGridItem dgi in dgShow.Items)
            {
                CheckBox cb = (CheckBox)dgi.FindControl("cbSelect");
                cb.Checked = cbAll.Checked;

            }
        }
    }
    //确认选择事件
    protected void Button1_Click(object sender, EventArgs e)
    {
        delete();
        if (User.Identity.Name != null)
        {
            string EmployeeNo = User.Identity.Name;
            bool isbeing = false;
            foreach (DataGridItem dgi in dgShow.Items)
            {
                CheckBox cb = (CheckBox)dgi.FindControl("cbSelect");
                if (cb.Checked)
                {
                    //以下执行删除操作
                    string name = dgi.Cells[0].Text;
                    string moduletitle="";
                    string ModuleType = dgi.Cells[4].Text.ToString();
                    switch (ModuleType)
                    {
                        case "1":
                            moduletitle = "WorkFlow_Add.aspx?module=";
                            break;
                        case "2":
                            moduletitle = "TextReport.aspx?module=";
                            break;
                        case "3":
                            moduletitle = "ChartReport.aspx?module=";
                            break;
                        case "4":
                            moduletitle = "customModule.aspx?module=";
                            break;
                        default:
                            break;
                    }
                    string moduleUrl = moduletitle + name;
                    string title = dgi.Cells[1].Text;
                    //判断数据库中是否存在该模块的工作内容
                    int workNumber = DbHelperSQL.Query("select * from User_work where (name='" + name + "') and  EmployeeNo='" + EmployeeNo + "'").Tables["ds"].Rows.Count;
                    if (workNumber == 0)
                    {
                        //判断模块中是否有此岗位
                        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(name);
                        isbeing = wf.authentication.IsRoportRole(EmployeeNo);
                        //isbeing = RoportRole.IsRoportRole(name, EmployeeNo);
                        if (isbeing == true)
                        {
                            Addwork(name, EmployeeNo, moduleUrl, title);
                        }
                    } 
                }
            }
        }
        else
        {
            Response.Write("<script>alert('用户未登录！')</script>");
        }
        Response.Write("<script language=javascript>window.parent.frames('leftFrame').location.reload();</script>");
    }
    //执行插入
    public void Addwork(string name, string EmployeeNo, string Module_Url, string Title)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("insert into User_work(");
        strSql.Append("name,EmployeeNo,Module_Url,Title)");
        strSql.Append(" values (");
        strSql.Append("@name,@EmployeeNo,@Module_Url,@Title)");
        SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.VarChar,100),
					new SqlParameter("@EmployeeNo", SqlDbType.VarChar,50),
					new SqlParameter("@Module_Url", SqlDbType.VarChar,500),
					new SqlParameter("@Title", SqlDbType.VarChar,100)};
        parameters[0].Value = name;
        parameters[1].Value = EmployeeNo;
        parameters[2].Value = Module_Url;
        parameters[3].Value = Title;

        DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
    }

    protected void dgShow_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        DataGridItem item = e.Item;
        ListItemType itemType = e.Item.ItemType;
        CheckBox chkObj = new CheckBox();
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            chkObj = (CheckBox)e.Item.FindControl("cbSelect");
            if (item.Cells[2].Text.Trim() == "1")
            {
                chkObj.Checked = true;
            }
            //chkObj.CheckedChanged += new EventHandler(CheckBoxObj_CheckedChanged);
            //鼠标落在当前行的颜色
            e.Item.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#fff7ce'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
        }

    }
    /// <summary>
    /// 取得该员工编号的岗位
    /// </summary>
    /// <param name="EmployeeNo"></param>
    /// <returns></returns>
    private string drJobName(String EmployeeNo)
    {
        string JobName = null;
        SqlDataReader dr = DbHelperSQL.ExecuteReader("SELECT JobName FROM  HR_Jobs INNER JOIN HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo WHERE (HR_Employees.Number='" + EmployeeNo + "')");
        if (dr.Read())
        {
            JobName = '"' + dr["JobName"].ToString() + '"';
        }
        dr.Close();
        return JobName;
    }
    public void delete()
    {
        string sql = "DELETE FROM  User_work";
        DbHelperSQL.ExecuteSql(sql);
    }
}
