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
using FMOP.DB;
using System.Data.SqlClient;
using Telerik.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;

public partial class Web_HR_GWJG : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //验证用户权限
            DefinedModule module = new DefinedModule("HR_GWJG");
            Authentication auth = module.authentication;
            if (auth == null)
            {
                MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }
            if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
            {
                MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }

            //生成树
            InitTree();

            //绑定Grid
            RadGrid1.DataBind();

        }
        
    }

    /// <summary>
    /// 初始化树
    /// </summary>
    protected void InitTree()
    {
        //清除树中的内容
        RadTreeView1.Nodes.Clear(); 
        SqlDataReader drLS = DbHelperSQL.ExecuteReader("SELECT LS FROM HR_LS order by id");
        while (drLS.Read())
        {
            Telerik.WebControls.RadTreeNode ls= new Telerik.WebControls.RadTreeNode(drLS[0].ToString());
            RadTreeView1.Nodes.Add(ls);

            //遍历部门，并将部门名称和包含的二级部门数量写入树中的节点
            SqlDataReader drBM = DbHelperSQL.ExecuteReader("SELECT Job_Dept, COUNT(Number) FROM HR_Jobs where Job_Attribute='" + drLS[0].ToString() + "' GROUP BY Job_Dept");
            while (drBM.Read())
            {
                Telerik.WebControls.RadTreeNode rtn = new Telerik.WebControls.RadTreeNode(drBM[0].ToString() + "(" + drBM[1].ToString() + ")");
                ls.Nodes.Add(rtn);
                //遍历二级部门/组，并将二级部门名称和包含的岗位数量写入树中的节点
                SqlDataReader drGroup = DbHelperSQL.ExecuteReader("SELECT Job_Dept, Job_Group, COUNT(Number) FROM HR_Jobs where Job_Dept='" + drBM[0].ToString() + "' GROUP BY Job_Dept, Job_Group");
                while (drGroup.Read())
                {
                    Telerik.WebControls.RadTreeNode group = new RadTreeNode(drGroup[1].ToString() + "(" + drGroup[2].ToString() + ")");
                    rtn.Nodes.Add(group);
                    
                    //遍历岗位，并将岗位名称和该岗位的员工数量写入树中的节点
                    //SqlDataReader drJOB = DbHelperSQL.ExecuteReader("select JobName,Number,(select count(Number) from HR_Employees where JobNo= A.Number) as emCount from HR_Jobs A where Job_Dept='" + drGroup[0].ToString() + "' and Job_Group='" + drGroup[1].ToString() + "'");
                    //2009-12-16李又密修改，去除离职人员
                    SqlDataReader drJOB = DbHelperSQL.ExecuteReader("select JobName,Number,(select count(Number) from HR_Employees where JobNo= A.Number and ygzt<>'离职') as emCount from HR_Jobs A where Job_Dept='" + drGroup[0].ToString() + "' and Job_Group='" + drGroup[1].ToString() + "'");
                    while (drJOB.Read())
                    {
                        Telerik.WebControls.RadTreeNode child = new Telerik.WebControls.RadTreeNode(drJOB[0].ToString() + "(" + drJOB[2].ToString() + ")");
                        child.NavigateUrl = "GWJG.aspx?JobNo=" + drJOB[1].ToString();
                        //Telerik.WebControls.RadTreeNode child = new Telerik.WebControls.RadTreeNode(drJOB[0].ToString(), drJOB[0].ToString(), "HR_GWJG.aspx?job_id=" + drJOB[1].ToString());
                        group.Nodes.Add(child);
                    }
                    drJOB.Close();
                }
                drGroup.Close();
            }
            drBM.Close();
        }
        drLS.Close();
    }
}
