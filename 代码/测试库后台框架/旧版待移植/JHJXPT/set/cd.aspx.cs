using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using System.Data.SqlClient;
using Hesion.Brick.Core;
using Key;
using System.Collections.Generic;
using Hesion.Brick.Core.WorkFlow;

public partial class JHJXPT_set_cd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //初始化权限下拉框
            DDLQX0.Items.Clear();
            DDLQX0.Items.Add(new ListItem("无限制", "a"));
            DDLQX0.Items.Add(new ListItem("隐藏", "b"));
            DDLQX0.Items.Add(new ListItem("自定义", "c"));
            for (int i = 0; i < 20; i++)
            {
                DDLQX0.Items.Add(new ListItem("权限位【" + Math.Pow(2, i).ToString() + "】", Math.Pow(2, i).ToString()));
            }




            if (Request["sortid"] != null && Request["sortid"].ToString() != "" && Request["tbname"] != null && Request["tbname"].ToString() != "")
            {
                DataSet dst = DbHelperSQL.Query("select * from " + Request["tbname"].ToString() + " where sortid = '" + Request["sortid"].ToString() + "'");
                string tname = dst.Tables[0].Rows[0]["SortName"].ToString();
                TBSortID.Text = Request["sortid"].ToString();
                ADD_TBFID.Text = Request["sortid"].ToString();
                TBname.Text = tname.ToString();
                TBTable.Text = Request["tbname"].ToString();
                ReLoadNode();

                //显示修改值
                ADD_name0.Text = dst.Tables[0].Rows[0]["SortName"].ToString();
                ADDurl0.Text = dst.Tables[0].Rows[0]["gotourl"].ToString();
                ADDurlsp0.Text = dst.Tables[0].Rows[0]["gotourlparameter"].ToString();
                if (dst.Tables[0].Rows[0]["targetgo"].ToString() != "_blank" && dst.Tables[0].Rows[0]["targetgo"].ToString() != "_self" && dst.Tables[0].Rows[0]["targetgo"].ToString() != "_parent" && dst.Tables[0].Rows[0]["targetgo"].ToString() != "_top")
                {
                    DDLURLTarget0.SelectedValue = "其他";
                    DDLURLTarget_d0.Text = dst.Tables[0].Rows[0]["targetgo"].ToString();
                }
                else
                {
                    DDLURLTarget0.SelectedValue = dst.Tables[0].Rows[0]["targetgo"].ToString();
                    DDLURLTarget_d0.Text = "";
                }
                if (dst.Tables[0].Rows[0]["showwho"].ToString() == "")
                {
                    DDLQX0.SelectedValue = "a";
                    DDLQX_d0.Text = "";
                }
                else if (dst.Tables[0].Rows[0]["showwho"].ToString() == "隐藏")
                {
                    DDLQX0.SelectedValue = "b";
                    DDLQX_d0.Text = "";
                }
                else if (IsNumeric(dst.Tables[0].Rows[0]["showwho"].ToString()))
                {
                    DDLQX0.SelectedValue = dst.Tables[0].Rows[0]["showwho"].ToString();
                    DDLQX_d0.Text = "";
                }
                else
                {
                    DDLQX0.SelectedValue = "c";
                    DDLQX_d0.Text = dst.Tables[0].Rows[0]["showwho"].ToString();
                }
                ADD_tip0.Text = dst.Tables[0].Rows[0]["tooltiptext"].ToString();



            }
        }

    }

    private bool IsNumeric(string str)
    {
        if (str == null || str.Length == 0)
            return false;
        foreach (char c in str)
        {
            if (!Char.IsNumber(c))
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// 重新加载菜单
    /// </summary>
    private void ReLoadNode()
    {
        TV.Nodes.Clear();
        string tablename = TBTable.Text;
        remotelogin rl = new remotelogin();
        DataTable dtm = rl.GetMenuData_yhtb(tablename);
        this.InitNode(dtm);
        TV.ExpandAll();
    }

    /// <summary>
    /// 初始化节点
    /// </summary>
    /// <param name="dt">要加载成树结构的数据源</param>
    private void InitNode(DataTable dt)
    {
        DataRow[] drRoot0 = dt.Select("1=1");
        if (drRoot0 != null && drRoot0.Length > 0)
        {
            DataRow[] drRoot_zi1 = dt.Select("SortParentID='0'");
            int allzi = drRoot_zi1.Length;
            for (int t = 0; t < allzi; t++)
            {
                DataRow drRoot_nowzi = drRoot_zi1[t];
                //检查菜单显示，隐藏禁止查看的菜单。

                TreeNode root = new TreeNode();
                root.Text = drRoot_nowzi["SortName"].ToString();
                root.ToolTip = drRoot_nowzi["ToolTipText"].ToString();
                root.NavigateUrl = "?sortid=" + drRoot_nowzi["SortID"].ToString() + "&tbname=" + TBTable.Text;
                root.Target = "_top";
                this.TV.Nodes.Add(root);
                this.BuildChild(drRoot_nowzi, root, dt);


            }

        }

    }

    /// <summary>
    /// 加载子节点
    /// </summary>
    /// <param name="dr">父节点对应的行</param>
    /// <param name="root">父节点</param>
    /// <param name="dt">要加载成树结构的数据源</param>
    private void BuildChild(DataRow dr, TreeNode root, DataTable dt)
    {
        if (dr == null || root == null) return;
        DataRow[] drChilds = dt.Select("SortParentID='" + dr["SortID"] + "'");
        if (drChilds != null || drChilds.Length > 0)
        {
            foreach (DataRow drChild in drChilds)
            {

                //检查菜单显示，隐藏禁止查看的菜单。

                TreeNode node = new TreeNode();
                node.Text = drChild["SortName"].ToString();
                node.ToolTip = drChild["ToolTipText"].ToString();
                node.NavigateUrl = "?sortid=" + drChild["SortID"].ToString() + "&tbname=" + TBTable.Text;
                node.Target = "_top";
                root.ChildNodes.Add(node);
                this.BuildChild(drChild, node, dt);


            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {
            DbHelperSQL.QueryInt("update " + TBTable.Text + " set showwho = '隐藏' where sortid = '" + TBSortID.Text + "'");
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {
            DbHelperSQL.QueryInt("delete " + TBTable.Text + " where sortid = '" + TBSortID.Text + "'");
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {

            string sortparentID = ADD_TBFID.Text; //父ID
            string sortname = ADD_name.Text;  //名字
            if (sortparentID == "" || sortname == "")
            {
                return;
            }

            DbHelperSQL.ExecuteSql("EXEC sp_Util_Sort_INSERT '" + TBTable.Text + "'," + sortparentID + ",'" + sortname + "'");
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {
            string sortname = ADD_name0.Text;
            string gotourl = ADDurl0.Text;
            string gotourlparameter = ADDurlsp0.Text;
            string targetgo = DDLURLTarget0.SelectedValue;
            string targetgo_d = DDLURLTarget_d0.Text;
            string showwho = DDLQX0.SelectedValue;
            string showwho_d = DDLQX_d0.Text;
            string tooltiptext = ADD_tip0.Text;
            if (sortname.Trim() == "")
            {
                return;
            }
            string targetgo_sql = "";
            if (targetgo == "其他")
            {
                targetgo_sql = targetgo_d;
            }
            else
            {
                targetgo_sql = targetgo;
            }

            string showwho_sql = "";
            if (showwho == "a")
            {
                showwho_sql = "";
            }
            else if (showwho == "b")
            {
                showwho_sql = "隐藏";
            }
            else if (showwho == "c")
            {
                showwho_sql = showwho_d;
            }
            else
            {
                showwho_sql = showwho;
            }
            string sql = "update " + TBTable.Text + " set sortname='" + sortname + "',gotourl='" + gotourl + "',gotourlparameter='" + gotourlparameter + "',showwho = '" + showwho_sql + "',targetgo='" + targetgo_sql + "',tooltiptext='" + tooltiptext + "' where sortid = '" + TBSortID.Text + "'";
            //Response.Write(sql);
            DbHelperSQL.ExecuteSql(sql);
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {
            DbHelperSQL.ExecuteSql("EXEC sp_Util_Sort_MoveOrder '" + TBTable.Text + "'," + TBSortID.Text + ",1");
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }
    }
    protected void Button8_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {
            DbHelperSQL.ExecuteSql("EXEC sp_Util_Sort_MoveOrder '" + TBTable.Text + "'," + TBSortID.Text + ",-1");
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {
            DbHelperSQL.ExecuteSql("EXEC sp_Util_Sort_MoveRevise '" + TBTable.Text + "',0");
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }
    }
    protected void Button9_Click(object sender, EventArgs e)
    {
        string fcfbz = Request.Headers["Accept"].ToString();
        if (fcfbz != "*/*")
        {
            if (tbydgs.Text.Trim() == "")
            {
                return;
            }
            DbHelperSQL.ExecuteSql("EXEC sp_Util_Sort_MoveSort '" + TBTable.Text + "'," + TBSortID.Text + "," + tbydgs.Text + "");
            ReLoadNode();
        }
        else
        {
            MessageBox.Show(this, "不要乱刷新，会重复提交。");
            return;
        }
    }
}