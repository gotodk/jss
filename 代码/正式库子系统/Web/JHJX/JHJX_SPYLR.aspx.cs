using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using FMOP.DB;
//using FMOP.Common;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;

public partial class Web_JHJX_JHJX_SPYLR : System.Web.UI.Page
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
        if (!Page.IsPostBack)
        {
 

            //用于repeater的数据源
            DataTable dt = new DataTable();
            dt.Columns.Add("ZZMC", typeof(string));
            ViewState["dt"] = dt;

            //用于下拉框ddlSSFL数据源
            dv = DbHelperSQL.Query("select SortID,SortName,SortParentID from AAA_tbMenuSPFL").Tables[0].DefaultView;
            dv.Sort = SortParentID;
            string schar = STR_TREENODE;
            if (dv.Table.Rows.Count > 0)
            {
                RecursBind(INT_TOPID, schar);
            }

        }

    }
   

    protected void RecursBind(int pid, string schar)
    {
        DataRowView[] rows = dv.FindRows(pid);

        if (pid != 0)
        {
            schar += STR_TREENODE;
        }

        foreach (DataRowView row in rows)
        {
            this.ddlSSFL.Items.Add(new ListItem(schar + row[SortName].ToString(), row[SortID].ToString()));

            RecursBind(Convert.ToInt32(row[SortID]), schar);

        }


    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        if (txtZZMC.Text.Trim() == "")
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('请先填写资质的名称！');</script>");
        }
        else if (txtZZMC.Text.Trim().Contains("|"))
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('资质的名称不能包含”|“字符！');</script>");
        }
        else if (txtZZMC.Text.Trim().Contains("*"))
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('资质的名称不能包含”*“字符！');</script>");
        }
        else
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (dt.Select("ZZMC='" + txtZZMC.Text.Trim() + "'").Length != 0)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('请勿重复添加！');</script>");
            }
            else
            {

                dt.Rows.Add(txtZZMC.Text.Trim());

                Repeater1.DataSource = dt;
                Repeater1.DataBind();
                hjspan.InnerText = dt.Rows.Count.ToString();
                PDtableRowsCount(dt.Rows.Count);
            }
            txtZZMC.Text = "";
        }
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        if (e.CommandName == "Delete")
        {

            DataTable dt = (DataTable)ViewState["dt"];
            dt.Rows.RemoveAt(e.Item.ItemIndex);

            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            hjspan.InnerText = dt.Rows.Count.ToString();
            PDtableRowsCount(dt.Rows.Count);

        }
    }

    //是否显示表尾
    protected void PDtableRowsCount(int tableRows)
    {
        if (tableRows > 0)
        {
            ts.Visible = false;
            hj.Visible = true;

        }
        else
        {
            ts.Visible = true;
            hj.Visible = false;

        }
    }
    protected void ddlSSFL_SelectedIndexChanged(object sender, EventArgs e)
    {
        RecurItem(ddlSSFL.SelectedIndex);
    }

    /// <summary>
    /// 如果所选分类还有子类，跳到第一个子类上
    /// </summary>
    /// <param name="index">索引</param>
    protected void RecurItem(int index)
    {
        DataTable dtemp = DbHelperSQL.Query("select * from AAA_tbMenuSPFL where SortParentID=" + ddlSSFL.SelectedValue).Tables[0];
        if (dtemp.Rows.Count > 0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('请选择" + ddlSSFL.SelectedItem.Text + "下面的子类！');</script>");
            index = ddlSSFL.SelectedIndex += 1;
            RecurItem(index);
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strfind = "select * from AAA_SPYLRXXB where SPMC='" + txtSPMC.Text.Trim() + "' and SPGG='" + txtSPGG.Text.Trim() + "'";
        DataTable data = (DataTable)ViewState["dt"];
        if (ddlSSFL.SelectedValue == "0")
        {
            MessageBox.Show(this, "请选择一个子类！");
        }
        else if (txtSPMC.Text.Trim() == "" || txtSPGG.Text.Trim() == "" || txtJJDW.Text.Trim() == "" || txtJJPL.Text.Trim() == "" || txtSPMS.Text.Trim() == "")
        {
            MessageBox.Show(this, "标注*号的字段不能为空！");
        }
        //else  if (!Regex.IsMatch(txtSPMC.Text.Trim(), @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$"))
        //{
        //    MessageBox.Show(this, "商品名称只可以为汉字、数字、字母或下划线的任意组合！");
        //}
        //else if (!Regex.IsMatch(txtSPGG.Text.Trim(), @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$"))
        //{
        //    MessageBox.Show(this, "商品规格只可以为汉字、数字、字母或下划线的任意组合！");
        //}
        else if(txtSPGG.Text.Trim().Contains("'") || txtSPMC.Text.Trim().Contains("'") || txtSPMS.Text.Trim().Contains("'")|| txtJJDW.Text.Trim().Contains("'"))
        {
            MessageBox.Show(this, "商品名称、规格、计价单位、商品描述中不可包含英文单引号！");
        }
        else if (DbHelperSQL.Query(strfind).Tables[0].Rows.Count > 0)
        {
            MessageBox.Show(this, "相同规格的商品信息已经存在，请勿重复添加！");
        }
        else
        {
            //获取新插入数据的number。
            WorkFlowModule WFM = new WorkFlowModule("AAA_SPYLRXXB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            //object obj = DbHelperSQL.GetSingle("select MAX( right(SPBH,4))+1 from AAA_PTSPXXB where SPBH like '" + ddlSSFL.SelectedValue.Trim() + "L%'");

            //string spbh = obj == null ? ddlSSFL.SelectedValue.Trim() + "L0001" : ddlSSFL.SelectedValue.Trim() + "L" + Convert.ToInt64(obj).ToString("D4");

            string zzxq = ZZXQ(data, "ZZMC");
            List<string> list = new List<string>();

            string str = "INSERT INTO [AAA_SPYLRXXB]([Number],[SPMC],[SPGG],[SSFLBH],[JJDW],[JJPL],[SPMS],[SCZZYQ],[SPZT],[CheckState],[CreateUser],[CreateTime]) VALUES('" + KeyNumber + "','" + txtSPMC.Text.Trim() + "','" + txtSPGG.Text.Trim() + "'," + ddlSSFL.SelectedValue.Trim() + ",'" + txtJJDW.Text.Trim() + "','" + txtJJPL.Text.Trim() + "','" + txtSPMS.Text.Trim() + "','" + zzxq + "','未复核',0,'" + User.Identity.Name.ToString() + "',GETDATE())";

            list.Add(str);
            //WorkFlowModule WNM = new WorkFlowModule("AAA_LJQDQZTXXB");
            //string number = WFM.numberFormat.GetNextNumber();
            //string num1y = number + "Y1";
            //string num3m = number + "M3";
            //string str1y = "INSERT INTO  AAA_LJQDQZTXXB (Number,SPBH,HTQX,SFJRLJQ,LJQYZBS,CheckState,CreateTime,CreateUser) VALUES('" + num1y + "','" + spbh + "','一年','否','未锁',1,GETDATE(),'" + User.Identity.Name.ToString() + "') ";
            //string str3m = "INSERT INTO  AAA_LJQDQZTXXB (Number,SPBH,HTQX,SFJRLJQ,LJQYZBS,CheckState,CreateTime,CreateUser) VALUES('" + num3m + "','" + spbh + "','三个月','否','未锁',1,GETDATE(),'" + User.Identity.Name.ToString() + "')";

            //list.Add(str1y);
            //list.Add(str3m);
            //更新数据库
            if (list.Count > 0)
            {
                if (DbHelperSQL.ExecuteSqlTran(list) > 0)
                {
                    string url = "JHJX_SPYLR.aspx";
                    MessageBox.ShowAndRedirect(this, "商品预录入成功！", url);

                }
                else
                {
                    MessageBox.Show(this, "预录入失败，请核对信息后重新提交！");
                    return;
                }
            }

        }

    }
    protected void btnCancle_Click(object sender, EventArgs e)
    {
        string url = "JHJX_SPYLR.aspx";
        Response.Redirect(url);

    }
    /// <summary>
    /// 合并DataTable的一列，用|分开
    /// </summary>
    /// <param name="data">数据表</param>
    /// <param name="colname">列名</param>
    /// <returns></returns>
    protected string ZZXQ(DataTable data, string colname)
    {
        string str = "";
        for (int i = 0; i < data.Rows.Count; i++)
        {
            str += "|" + data.Rows[i]["" + colname + ""].ToString();
            // str +=  data.Rows[i]["" + colname + ""].ToString()+"|";
        }
        if (str != "")
            str = str + "|";
        //str=str.Remove(0,1);
        return str;
    }


}