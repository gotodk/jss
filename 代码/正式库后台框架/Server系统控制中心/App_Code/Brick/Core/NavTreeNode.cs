using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.WebControls;
using System.Text;
using Hesion.Brick.Core;
using System.Data.SqlClient;
using FMDBHelperClass;
using System.Collections;

/// <summary>
/// 
/// </summary>
public static class NavTreeNode
{

    public static void SetSmallClassNode(int SmallClassId, string EmpNumber,RadTreeNode Node)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        string sql = GetModuleSQL(EmpNumber,SmallClassId);        
        Hashtable ht =  I_DBL.RunProc(sql,"");
        DataSet ds = (DataSet)ht["return_ds"];
        if(ds!=null && ds.Tables.Count>0&& ds.Tables[0].Rows.Count>0)
        {

            DataRow dr = ds.Tables[0].Rows[0];

            //通过模块类型，找到访问的页面
            int moduleType = int.Parse(dr["ModuleType"].ToString());
            string moduleName = dr["name"].ToString();
            string title = dr["title"].ToString();
            string Url = string.Empty;
            switch (moduleType)
            {
                case 1: Url = "WorkFlow_Add.aspx";
                    break;
                case 2: Url = "TextReport.aspx";
                    break;
                case 3: Url = "ChartReport.aspx";
                    break;
                case 4: Url = "CustormModule.aspx";
                    break;
            }
            Url += "?module=" + moduleName;

            //将模块节点加入到小类中
            RadTreeNode moduleNode = new RadTreeNode();
            moduleNode.Text = title;
            moduleNode.NavigateUrl = Url;
            moduleNode.Target = "rightFrame";
            moduleNode.ImageUrl = "images/littletitle/script_16x16.gif";
            Node.Nodes.Add(moduleNode);

        }
        

    }

    /// <summary>
    /// 通过员工号和栏目小类ID组装SQL语句
    /// </summary>
    /// <param name="EmpNumber">员工号</param>
    /// <param name="SmallClassId">栏目小类ID</param>
    /// <returns>SQL条件</returns>
    private static string GetModuleSQL(string EmpNumber, int SmallClassId)
    {
        User user = Users.GetUserByNumber(EmpNumber);
        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT name,title,ModuleType FROM system_Modules where (");
        sql.Append(GetExistSql("Authentication", user.JobName));
        sql.Append(" or ");
        sql.Append(GetExistSql("Authentication", EmpNumber));
        sql.Append(" or ");
        sql.Append(GetExistSql("Authentication", user.DeptName));
        sql.Append(" or ");
        sql.Append(GetExistSql("Check", user.JobName));
        sql.Append(" or ");
        sql.Append(GetExistSql("Check", EmpNumber));
        sql.Append(" or ");
        sql.Append(GetExistSql("Check", user.DeptName));
        sql.Append(" or AuthType=0 ) and smallclassid=");
        sql.Append(SmallClassId.ToString());
        return sql.ToString();
    }

    //组装SQL
    private static string GetExistSql(string node,string RoleName)
    {
        return "Configuration.exist('//" + node + "/*[roleName=\"" + RoleName + "\"]')=1";
    }
	
}
