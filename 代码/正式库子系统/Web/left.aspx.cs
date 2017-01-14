using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using FMOP.DB;
using Telerik.WebControls;
using Telerik.RadTreeviewUtils;
using Hesion.Brick.Core;

public partial class left : System.Web.UI.Page
{
	private DataTable group;
	private DataTable modules;
	private string dept = "";
	private string station = "";
		
    /// <summary>
    /// 页面加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sql = "select BM,GWMC from HR_Employees where Number='" + User.Identity.Name + "'";
            SqlDataReader sdr = DbHelperSQL.ExecuteReader(sql);
            while (sdr.Read())
            {
                dept = sdr["BM"].ToString();
                station = sdr["GWMC"].ToString();
            }
            sdr.Close();

            ////清除所有Node;
            //this.RadTreeView1.Nodes.Clear();

            ////创建根节点
            //RadTreeNode Node = new RadTreeNode();
            //Node.Text = "系统首页";
            //Node.ImageUrl = "images/littletitle/video_(play)_16x16.gif";

            //this.RadTreeView1.Nodes.Add(Node);
            //setDataTable();
            ////调用递归方法
            //addTreeView(0,Node);
            //this.RadTreeView1.Nodes[0].Expanded = true;

            ////添加我的工作
            //AddMyWork(Node);
           
           
        }
    }

	private void setDataTable()
	{
        string where_sp = "";
        if (Request["sp"] != null && Request["sp"].ToString() == "assign")
        {
            where_sp = " where show <> '隐藏' ";
        }
        string cmdText = " SELECT * FROM system_ModuleGroup order by px asc " + where_sp;
		    group = DbHelperSQL.Query(cmdText).Tables[0];

		cmdText = "select distinct a.ModuleName,a.ModuleType,b.title,b.SmallClassId from system_auth a,system_modules b where a.ModuleName = b.name and ((type=1 and role='" + station
            + "') or (type=2 and role='" + dept + "') or (type=3 and role='" + User.Identity.Name + "') or (role='全公司') or ((select count(modulename) from absolutenessModule where modulename = b.name)>0)) order by b.title";
        Response.Write(cmdText);
		modules = DbHelperSQL.Query(cmdText).Tables[0];
	}

    /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="parentId">上层节点ID</param>
    /// <param name="Node">上层节点</param>
    private void addTreeView(int parentId,RadTreeNode Node)
    {
		DataRow[] dts = group.Select(" parentid=" + parentId.ToString());//查询组节点
		DataRow[] moduleResult = null;
        int ID;
        foreach (DataRow dr in dts)
        {
            RadTreeNode childNode = new RadTreeNode();
            childNode.Text = dr["title"].ToString();
            childNode.ImageUrl = "images/littletitle/folder_closed_16x16.gif";
            childNode.ImageExpandedUrl = "images/littletitle/folder_open_16x16.gif";
            Node.Nodes.Add(childNode);
            ID=Convert.ToInt16(dr["id"].ToString());
            moduleResult = modules.Select("SmallClassId=" + ID.ToString());
            addTreeView(ID, childNode);
			foreach (DataRow moduleRow in moduleResult)
			{
				int moduleType = int.Parse(moduleRow["ModuleType"].ToString());
				string moduleName = moduleRow["ModuleName"].ToString();
				string Url = string.Empty;

				switch (moduleType)
				{
					case 1:
						Url = "WorkFlow_Add.aspx";
						break;
					case 2: 
						Url = "TextReport.aspx";
						break;
					case 3: 
						Url = "ChartReport.aspx";
						break;
					case 4: 
						Url = "CustormModule.aspx";
						break;
				}
				Url += "?module=" + moduleName;
				RadTreeNode moduleNode = new RadTreeNode();
				moduleNode.Text = moduleRow["title"].ToString();
				moduleNode.NavigateUrl = Url;
				moduleNode.Target = "rightFrame";
				moduleNode.ImageUrl = "images/littletitle/script_16x16.gif";
				childNode.Nodes.Add(moduleNode);

			}
        }
    }



	/*
	 /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="parentId">上层节点ID</param>
    /// <param name="Node">上层节点</param>
    private void addTreeView(int parentId,RadTreeNode Node)
    {
        DataSet result = QueryNodeGroup(parentId); //查询组节点
        DataSet moduleResult = null;
        int ID;
        bool canCheck;
        foreach (DataRow dr in result.Tables[0].Rows)
        {
            RadTreeNode childNode = new RadTreeNode();
            childNode.Text = dr["title"].ToString();
            childNode.ImageUrl = "images/littletitle/folder_closed_16x16.gif";
            childNode.ImageExpandedUrl = "images/littletitle/folder_open_16x16.gif";
            Node.Nodes.Add(childNode);
            ID=Convert.ToInt16(dr["id"].ToString());
            moduleResult = QueryNode(ID);
            addTreeView(ID, childNode);
			foreach (DataRow moduleRow in moduleResult.Tables[0].Rows)
			{
				int moduleType = int.Parse(moduleRow["ModuleType"].ToString());
				string moduleName = moduleRow["ModuleName"].ToString();
				string Url = string.Empty;

				switch (moduleType)
				{
					case 1:
						Url = "WorkFlow_Add.aspx";
						break;
					case 2: 
						Url = "TextReport.aspx";
						break;
					case 3: 
						Url = "ChartReport.aspx";
						break;
					case 4: 
						Url = "CustormModule.aspx";
						break;
				}
				Url += "?module=" + moduleName;
				RadTreeNode moduleNode = new RadTreeNode();
				moduleNode.Text = moduleRow["title"].ToString();
				moduleNode.NavigateUrl = Url;
				moduleNode.Target = "rightFrame";
				moduleNode.ImageUrl = "images/littletitle/script_16x16.gif";
				childNode.Nodes.Add(moduleNode);

			}
        }
    }
	

	/// <summary>
    /// 查询子节点组
    /// </summary>
    private DataSet QueryNodeGroup(int parentID)
    {
        string cmdText = " SELECT * FROM system_ModuleGroup WHERE parentId=" + parentID;
        DataSet result = new DataSet();

        try
        {
            result = DbHelperSQL.Query(cmdText);
        }
        catch (Exception ex)
        {
            result = null;
        }

        return result;
    }

    /// <summary>
    /// 查询模块
    /// </summary>
    /// <param name="parentID">上层组</param>
    /// <returns>下层的所有节点</returns>
	private DataSet QueryNode(int parentID)
	{
		string cmdText = "select distinct a.ModuleName,a.ModuleType,b.title from system_auth a,system_modules b where a.ModuleName = b.name and SmallClassId=" + parentID.ToString() + " and ((type=1 and role='" + station
			+ "') or (type=2 and role='" + dept + "') or (type=3 and role='" + User.Identity.Name + "') or (role='全公司')) order by a.ModuleName";
		DataSet result = new DataSet();

		try
		{
			result = DbHelperSQL.Query(cmdText);
		}
		catch (Exception ex)
		{
			result = null;
		}

		return result;
	}
	 */



	/// <summary>
    /// 添加我的工作
    /// </summary>
    /// <param name="Node"></param> 
    private void AddMyWork(RadTreeNode parentNode)
    {
        RadTreeNode rtNode = new RadTreeNode();
        rtNode.Text = "我的工作";
        rtNode.ImageUrl = "images/littletitle/folder_closed_16x16.gif";
        rtNode.ImageExpandedUrl = "images/littletitle/folder_open_16x16.gif";
        rtNode.Target = "rightFrame";
        rtNode.NavigateUrl = "User_Add_Work.aspx";
        parentNode.Nodes.Add(rtNode);
        addwork(rtNode);
    }
    private void addwork(RadTreeNode rtNode)
    {
        string EmployeeNo = User.Identity.Name;
        DataTable dt = DbHelperSQL.Query("select * from dbo.User_work where EmployeeNo ='" + EmployeeNo + "'").Tables[0];
        foreach (DataRow row in dt.Rows)
        {   ///创建新节点
            RadTreeNode mynode = new RadTreeNode();
            mynode.Text = row["Title"].ToString();
            mynode.NavigateUrl = row["Module_Url"].ToString();
            mynode.Target = "rightFrame";
            mynode.ImageUrl = "images/littletitle/script_16x16.gif";
            rtNode.Nodes.Add(mynode);
        }
    }




}
