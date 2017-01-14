using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FMOP.DB;
//using FMOP.Common;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Collections;

public partial class Web_JHJX_SPFLGL : System.Web.UI.Page
{
    //用于无限极下拉框数据源
    DataView dv;
    //层次分隔符
    const string STR_TREENODE = "┆┄";
    //顶级父节点
    const int INT_TOPID = 0;
    //分类ID,与一下几个字段同数据库中的字段名称相同，以方便取值
    const string SortID = "SortID";
    //父分类ID
    const string SortParentID = "SortParentID";
    //分类名称
    const string SortName = "SortName";
 
 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetItems();
            RptInitial();

          
 
        }
        
    }
    /// <summary>
    /// 初始化Reapter排序列表的数据源
    /// </summary>
    protected void RptInitial()
    {
        string sqt = " SELECT [ID] ,[SortID],[SortName],[SortParentID],[SortParentPath],[SortOrder]  FROM [AAA_tbMenuSPFL] where SortID in (  select distinct SSFLBH from AAA_PTSPXXB) order by SortOrder";
        DataSet ds = DbHelperSQL.Query(sqt);
        Repeater1.DataSource = ds.Tables[0].DefaultView;
        Repeater1.DataBind();

        Control ctl = Repeater1.Controls[Repeater1.Controls.Count - 1];
        Label slhj = (Label)ctl.FindControl("slhj");
        slhj.Text ="共"+ ds.Tables[0].Compute("count(ID)", "true").ToString()+"条记录";
 
    }

    /// <summary>
    /// 递归绑定DropDownList
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="schar"></param>
    protected void RecursBind(int pid,  string schar)
    {
        DataRowView[] rows = dv.FindRows(pid);
        
        if (pid != 0)
        {
            schar += STR_TREENODE;
        }
        
        foreach (DataRowView row in rows)
        {            
            this.ddlssfl.Items.Add(new ListItem(schar + row[SortName].ToString(), row[SortID].ToString()));

            RecursBind(Convert.ToInt32(row[SortID]),  schar);

        }
       
       
    }


    protected void SetItems()
    {

        DataSet ds = DbHelperSQL.Query("SELECT * FROM [AAA_tbMenuSPFL]  ORDER BY [SortOrder] ");
        
        listsecond.DataSource = ds.Tables[0].DefaultView;
        listsecond.DataTextField = "SortName";
        listsecond.DataValueField = "SortID";
        listsecond.DataBind();

        //ddlssfl.DataSource = ds.Tables[0].DefaultView;
        //ddlssfl.DataTextField = "SortName";
        //ddlssfl.DataValueField = "SortID";
        //ddlssfl.DataBind();

        //ddlssfl.Items.Insert(0, new ListItem("无所属", "0"));

        dv = DbHelperSQL.Query("select SortID,SortName,SortParentID,SortOrder from AAA_tbMenuSPFL  ORDER BY SortOrder").Tables[0].DefaultView;
       dv.Sort = SortParentID;
 
        string schar = STR_TREENODE;
        if (dv.Table.Rows.Count > 0)
        {
            RecursBind(INT_TOPID,  schar);
        }
    }

    protected void TabStrip1_TabClick(object sender, Telerik.WebControls.TabStripEventArgs e)
    {

    }


    //上移
    protected void btntop_Click(object sender, EventArgs e)
    {
        if (listsecond.SelectedIndex > 0)
        {
            string name = listsecond.SelectedItem.Text;
            string ID = listsecond.SelectedItem.Value;
            int index = listsecond.SelectedIndex;
            listsecond.SelectedItem.Text = listsecond.Items[index - 1].Text;
            listsecond.SelectedItem.Value = listsecond.Items[index - 1].Value;
            listsecond.Items[index - 1].Text = name;
            listsecond.Items[index - 1].Value = ID;
            listsecond.SelectedIndex--;

            int currentorder = listsecond.SelectedIndex + 1;
            string strcurrent = "update  AAA_tbMenuSPFL set SortOrder="+currentorder.ToString()+" where SortID="+listsecond.SelectedValue;

            int beforeorder = listsecond.SelectedIndex + 1 + 1;
            string value = listsecond.Items[listsecond.SelectedIndex + 1].Value;
            string strbefore = "update  AAA_tbMenuSPFL set SortOrder=" + beforeorder.ToString() + " where SortID=" + value;

            List<string> list = new List<string>();
            list.Add(strcurrent);
            list.Add(strbefore);

            if (DbHelperSQL.ExecuteSqlTran(list) > 0)
            {
                ddlssfl.Items.Clear();
                SetItems();
                ddlssfl.Items.Insert(0, new ListItem("所属分类", "0"));
            }

            else
            {
                MessageBox.Show(this,"未能更新数据！");
            }

        }

    }

    //下移
    protected void btnbottom_Click(object sender, EventArgs e)
    {
        if (listsecond.SelectedIndex >= 0 && listsecond.SelectedIndex < listsecond.Items.Count - 1)
        {
            string name = listsecond.SelectedItem.Text;
            string ID = listsecond.SelectedItem.Value;
            int index = listsecond.SelectedIndex;
            listsecond.SelectedItem.Text = listsecond.Items[index + 1].Text;
            listsecond.SelectedItem.Value = listsecond.Items[index + 1].Value;
            listsecond.Items[index + 1].Text = name;
            listsecond.Items[index + 1].Value = ID;
            listsecond.SelectedIndex++;

            int currentorder = listsecond.SelectedIndex + 1;
            string strcurrent = "update  AAA_tbMenuSPFL set SortOrder=" + currentorder.ToString() + " where SortID=" + listsecond.SelectedValue;

            int beforeorder = listsecond.SelectedIndex + 1 - 1;
            string value = listsecond.Items[listsecond.SelectedIndex - 1].Value;
            string strbefore = "update  AAA_tbMenuSPFL set SortOrder=" + beforeorder.ToString() + " where SortID=" + value;

            List<string> list = new List<string>();
            list.Add(strcurrent);
            list.Add(strbefore);

            if (DbHelperSQL.ExecuteSqlTran(list) > 0)
            {
                ddlssfl.Items.Clear();
                SetItems();
                ddlssfl.Items.Insert(0, new ListItem("所属分类", "0"));
            }

            else
            {
                MessageBox.Show(this, "未能更新数据！");
            }


        }


    }

    //新增
    protected void btnqd_Click(object sender, EventArgs e)
    {
        if (txtflmc.Text.Trim() == "")
        {
            MessageBox.Show(this, "请输入要添加的类名！");
        }
        else if(DbHelperSQL.GetSingle("select COUNT(*)   from AAA_tbMenuSPFL where SortName='"+txtflmc.Text.Trim()+"'").ToString()!="0")
        {
            MessageBox.Show(this,"已经存在该分类，请勿重复添加！");
        }
        
        else
        {
            DataTable dtemp = DbHelperSQL.Query("select max(SortID)+1 as sortid,max(SortOrder)+1 as sortorder from AAA_tbMenuSPFL").Tables[0];
            string sortid = dtemp.Rows[0]["sortid"].ToString();
            string order = dtemp.Rows[0]["sortorder"].ToString();

            string path = ",";
            if (ddlssfl.SelectedValue != "0")
            {
                string s = " select SortParentPath from AAA_tbMenuSPFL where SortID=" + ddlssfl.SelectedValue;
                path = DbHelperSQL.GetSingle(s).ToString()+ ddlssfl.SelectedValue  +",";
            }


            string str = "insert into AAA_tbMenuSPFL([SortID],[SortName],[SortParentID],[SortParentPath],[SortOrder])values(" + sortid + ",'" + txtflmc.Text.Trim() + "'," + ddlssfl.SelectedValue + ",'" + path + "'," + order + ")";

            List<string> list = new List<string>();
            list.Add(str);

            if (DbHelperSQL.ExecuteSqlTran(list) > 0)
            {

                ddlssfl.Items.Clear();
                SetItems();
                ddlssfl.Items.Insert(0, new ListItem("所属分类", "0"));
                RptInitial();
                MessageBox.Show(this,"添加分类成功！");
            }

            else
            {
                MessageBox.Show(this, "未能添加分类！");
            }
 
        }
      

    }

    //修改
    protected void btnxg_Click(object sender, EventArgs e)
    {
        if (txtflmc.Text.Trim() == "")
        {
            MessageBox.Show(this, "请输入要修改的类名！");
        }
        else if (DbHelperSQL.GetSingle("select COUNT(*)   from AAA_tbMenuSPFL where SortName='" + txtflmc.Text.Trim() + "'").ToString() != "1")
        {
            MessageBox.Show(this, "没有找到要修改的分类，请输入准确的分类名！");
        }
        else
        {
            //更改后的SortParentPath
            string path = ",";
            if (ddlssfl.SelectedValue != "0")
            {
                string s = " select SortParentPath from AAA_tbMenuSPFL where SortID=" + ddlssfl.SelectedValue;
                path = DbHelperSQL.GetSingle(s).ToString() + ddlssfl.SelectedValue + ",";
            }

            //取得SortID，判断父类是否为自己
            string sd = DbHelperSQL.GetSingle("select SortID from AAA_tbMenuSPFL where SortName='" + txtflmc.Text.Trim() + "'").ToString();
            if (sd == ddlssfl.SelectedValue.Trim())
            {
                MessageBox.Show(this,"所选父类不能为自己！");
                return;
            }

            //获取该分类所有下级类的数据集
            DataTable dtest = DbHelperSQL.Query(" select SortID,SortParentID ,  SortParentPath  from  AAA_tbMenuSPFL where SortParentPath like '%,"+sd+",%'").Tables[0];

            List<string> list = new List<string>();

            if (dtest != null && dtest.Rows.Count > 0)
            {
                //第一个循环判断分类所设置的父类是否为其子类
                for (int i = 0; i < dtest.Rows.Count; i++)
                {
                                       
                    string id1 = dtest.Rows[i]["SortID"].ToString();
                    string newflid = ddlssfl.SelectedValue.Trim() ;
                    if (id1==newflid)
                    {
                        MessageBox.Show(this, "为分类所设置的父类不能为该分类的子类");
                        return;
                    }

                }
                //如果不是其子类，第二个循环用来更新其子类的新的 SortParentPath，因为分类改变了父类，其子类的 SortParentPath也会改变
                for (int i = 0; i < dtest.Rows.Count; i++)
                {
                    string parentPath = dtest.Rows[i]["SortParentPath"].ToString();
                    string id=dtest.Rows[i]["SortID"].ToString();
                    parentPath = parentPath.Substring(parentPath.IndexOf(sd), parentPath.LastIndexOf(",") - parentPath.IndexOf(sd) + 1);
                    parentPath = path+parentPath;

                    string str1 = "update AAA_tbMenuSPFL set SortParentPath='"+parentPath+"' where SortID="+id;
                    list.Add(str1);
                }
            }


            //更新分类的SortParentID，SortParentPath
            string str = "update AAA_tbMenuSPFL set SortParentID=" + ddlssfl.SelectedValue+",SortParentPath='"+path+"' where SortName='"+txtflmc.Text.Trim()+"'";
           
            list.Add(str);

            if (DbHelperSQL.ExecuteSqlTran(list) > 0)
            {
                ddlssfl.Items.Clear();
                SetItems();
                ddlssfl.Items.Insert(0, new ListItem("所属分类", "0"));
                RptInitial();
                MessageBox.Show(this, "修改成功！");
            }

            else
            {
                MessageBox.Show(this, "未能修改！");
            }
 
        }
    }
    protected void btninitial_Click(object sender, EventArgs e)
    {
        string str = "update  AAA_tbMenuSPFL set SortOrder= a.num from  (select SortID,  Row_Number() OVER( ORDER BY SortID ) as num from AAA_tbMenuSPFL) a  where AAA_tbMenuSPFL.SortID=a.SortID";

        List<string> list = new List<string>();
        list.Add(str);

        if (DbHelperSQL.ExecuteSqlTran(list) > 0)
        {
            ddlssfl.Items.Clear();
            SetItems();
            ddlssfl.Items.Insert(0, new ListItem("所属分类", "0"));
            MessageBox.Show(this, "默认成功！");
        }

        else
        {
            MessageBox.Show(this, "未能修改！");
        }
    }

   
    //排序
    protected void btnSort_Click(object sender, EventArgs e)
    {
        if (btnSort.Text == "编辑")
        {
            for (int i = 0; i < Repeater1.Items.Count; i++)
            {
                TextBox tb = (TextBox)Repeater1.Items[i].FindControl("txtsx");
               

                tb.BorderWidth = 1;
                tb.CssClass = "tj_input";
                tb.ReadOnly = false;
            }
            btnSort.Text = "保存";
        }
        else
        {
            List<string> orderList = new List<string>();
            for (int i = 0; i < Repeater1.Items.Count; i++)
            {
                TextBox tb = (TextBox)Repeater1.Items[i].FindControl("txtsx");
                Label ln = (Label)Repeater1.Items[i].FindControl("lbname");
                if (tb.Text.Trim() != "")
                {                 

                    tb.BorderWidth = 0;

                    tb.ReadOnly = true;
                    tb.Attributes["onfocus"] = "this.blur()";

                    string str = "update AAA_tbMenuSPFL set SortOrder="+tb.Text.Trim()+" where SortName='"+ln.Text.Trim()+"' ";
                    orderList.Add(str);
                }
                else
                {
                    MessageBox.Show(this, "排列顺序不可为空值！");
                    tb.Attributes["onfocus"] = "this.focus()";
                    return;
                }

            }

            if (DbHelperSQL.ExecuteSqlTran(orderList) > 0)
            {
                btnSort.Text = "编辑";
                RptInitial();
                MessageBox.Show(this, "修改成功！");
            }
          
 
        }



    }
  
    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lp = (Label)e.Item.FindControl("lbpath");
        if (lp != null)
        {
            string path = lp.Text.TrimStart(',').TrimEnd(',');

            if (path != "")
            {
                string sns = "";
                string[] strs = path.Split(',');
                for (int j = 0; j < strs.Length; j++)
                {
                    string sn = DbHelperSQL.GetSingle("SELECT SortName  FROM AAA_tbMenuSPFL where SortID=" + strs[j]).ToString();
                    sns = sns + "=>" + sn;

                }
                lp.Text = sns.TrimStart(new char[] {'=','>'});


            }
            else
            {
                lp.Text = "---";

            }
 
        }
       
    }
}