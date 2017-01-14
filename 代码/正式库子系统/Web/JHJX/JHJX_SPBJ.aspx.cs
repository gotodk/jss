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


public partial class Web_JHJX_JHJX_SPBJ : System.Web.UI.Page
{
    protected static string yspmc = "", yspgg = "";
    protected static string connectionString = ConfigurationManager.ConnectionStrings["FMOPConn"].ToString();
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
 

            //传值
            string num = "";
            if (Request["ID"] != null && Request["ID"].ToString() != null)
            {
                num = Request["ID"].ToString();

            }
            ViewState["ID"] = num;

            //用于下拉框ddlSSFL数据源
            dv = DbHelperSQL.Query("select SortID,SortName,SortParentID from AAA_tbMenuSPFL").Tables[0].DefaultView;
            dv.Sort = SortParentID;
            string schar = STR_TREENODE;
            if (dv.Table.Rows.Count > 0)
            {
                RecursBind(INT_TOPID,  schar);
            }

            //初始化页面的数据源
            DataTable di = new DataTable();
            string sxzz = "";
            InitalData(out sxzz,num,out di);
            ViewState["di"] = di;


            //用于repeater的数据源
            DataTable dt = new DataTable();
            dt.Columns.Add("ZZMC", typeof(string));
            string [] zz= sxzz.Split('|') ;
            foreach (string  item in zz)
            {
                dt.Rows.Add(item);
            }
            if (dt.Rows.Count == 1 && dt.Rows[0]["ZZMC"].ToString() == "")
                dt.Rows.RemoveAt(0);
            ViewState["dt"] = dt;
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

        }

    }

    protected void InitalData(out string sxzz, string num,out DataTable di)
    {
        di = DbHelperSQL.Query("SELECT [Number],[SPBH],[SPMC],[GG],[SSFLBH],[JJDW],[JJPL],[SPMS],[SCZZYQ],[FBRQ],[SFYX],[ZFRQ],[CreateUser],[CreateTime] FROM [AAA_PTSPXXB] WHERE Number='"+num+"'").Tables[0];
        ddlSSFL.SelectedValue = di.Rows[0]["SSFLBH"].ToString().Trim();
        txtJJDW.Text = di.Rows[0]["JJDW"].ToString().Trim();
        yspmc= txtSPMC.Text = di.Rows[0]["SPMC"].ToString().Trim();
        yspgg = txtSPGG.Text = di.Rows[0]["GG"].ToString().Trim(); 
        txtSPMS.Text = di.Rows[0]["SPMS"].ToString().Trim();
        txtJJPL.Text = di.Rows[0]["JJPL"].ToString().Trim();
        sxzz = di.Rows[0]["SCZZYQ"].ToString().Trim().TrimStart('|').TrimEnd('|');
        ViewState["SPBH"] = di.Rows[0]["SPBH"].ToString().Trim();
    }

    /// <summary>
    /// 递归绑定DropDownList
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="schar"></param> 
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
        string strfind = "select * from AAA_PTSPXXB where SPMC='" + txtSPMC.Text.Trim() + "' and GG='" + txtSPGG.Text.Trim() + "'";
        DataTable data = (DataTable)ViewState["dt"];
        if (ddlSSFL.SelectedValue == "0")
        {
            MessageBox.Show(this, "请选择一个子类！");
        }
        else if (txtSPMC.Text.Trim() == "" || txtSPGG.Text.Trim() == "" || txtJJDW.Text.Trim() == "" || txtJJPL.Text.Trim() == "" || txtSPMS.Text.Trim() == "")
        {
            MessageBox.Show(this, "标注*号的字段不能为空！");
        }
        //else if (!Regex.IsMatch(txtSPMC.Text.Trim(), @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$"))
        //{
        //    MessageBox.Show(this, "商品名称只可以为汉字、数字、字母或下划线的任意组合！");
        //}
        //else if (!Regex.IsMatch(txtSPGG.Text.Trim(), @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$"))
        //{
        //    MessageBox.Show(this, "商品规格只可以为汉字、数字、字母或下划线的任意组合！");
        //}
        else if (txtSPGG.Text.Trim().Contains("'") || txtSPMC.Text.Trim().Contains("'") || txtSPMS.Text.Trim().Contains("'") || txtJJDW.Text.Trim().Contains("'"))
        {
            MessageBox.Show(this, "商品名称、规格、计价单位、商品描述中不可包含英文单引号！");
        }
        else if (!(yspmc == txtSPMC.Text.Trim() && yspgg == txtSPGG.Text.Trim()) && DbHelperSQL.Query(strfind).Tables[0].Rows.Count > 0)
        {
            MessageBox.Show(this, "相同规格的商品信息已经存在，请勿重复添加！");            
            
        }
        else
        {

            string zzxq = ZZXQ(data, "ZZMC");

            //更新数据库
            DataTable di = (DataTable)ViewState["di"];
            di.Rows[0]["SSFLBH"] = ddlSSFL.SelectedValue.Trim();
            di.Rows[0]["JJDW"] = txtJJDW.Text.Trim();
            di.Rows[0]["SPMC"] = txtSPMC.Text.Trim();
            di.Rows[0]["GG"] = txtSPGG.Text.Trim();
            di.Rows[0]["SPMS"] = txtSPMS.Text.Trim();
            di.Rows[0]["JJPL"] = txtJJPL.Text.Trim();
            di.Rows[0]["SCZZYQ"] = zzxq.Trim();
            di.Rows[0]["FBRQ"] = DateTime.Now.ToString();

            DataTable dc = di.GetChanges(DataRowState.Modified);

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [Number],[SPBH],[SPMC],[GG],[SSFLBH],[JJDW],[JJPL],[SPMS],[SCZZYQ],[FBRQ],[SFYX],[ZFRQ],[CreateUser],[CreateTime] FROM [AAA_PTSPXXB]", connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

            if (adapter.Update(dc) > 0)             
            {
                MessageBox.ShowAndRedirect(this, "数据更新成功", "JHJX_SPGL.aspx");
                #region//作废
                //if (ckBGZZ.Checked == true)
                //{
                //    string str = "select a.DLYX 交易方账号,b.I_JYFMC 交易方名称,b.J_SELJSBH 卖家角色编号 from AAA_CSSPB a left join AAA_DLZHXXB b on a.DLYX=b.B_DLYX where a.SFBGZZHBZ='否' and a.SHZT='审核通过' and a.SPBH='"+ViewState["SPBH"].ToString().Trim()+"' ";
                //    DataSet ds = DbHelperSQL.Query(str);
                //    List<Hashtable> list = new List<Hashtable>();
                    
                //    if (ds != null && ds.Tables[0].Rows.Count > 0)
                //    {
                //        string strUpdate = "update AAA_CSSPB set SFBGZZHBZ='是' where  SFBGZZHBZ='否' and SHZT='审核通过' and SPBH='" + ViewState["SPBH"].ToString().Trim() + "'";
                //        int i = DbHelperSQL.ExecuteSql(strUpdate);

                //        if (i > 0)
                //        {
                //            //发送提醒
                //            foreach (DataRow row1 in ds.Tables[0].Rows)
                //            {
                //                Hashtable ht = new Hashtable();
                //                ht["type"] = "集合集合经销平台";
                //                ht["提醒对象登陆邮箱"] = row1["交易方账号"].ToString();
                //                ht["提醒对象用户名"] = row1["交易方名称"].ToString();
                //                ht["提醒对象结算账户类型"] = "买家卖家交易账户";
                //                ht["提醒对象角色编号"] = row1["卖家角色编号"].ToString();
                //                ht["提醒对象角色类型"] = "卖家";
                //                ht["提醒内容文本"] = "尊敬的" + row1["交易方名称"].ToString() + "卖家， 平台现已依规对出售" + ViewState["SPBH"].ToString() + txtSPMC.Text.Trim() + "商品的资质要求作出变更，请重新申请该商品的出售资格。";//待确定
                //                ht["创建人"] = User.Identity.Name.ToString();
                //                list.Add(ht);
                //            }
                //            JHJX_SendRemindInfor.Sendmes(list);
                //            MessageBox.ShowAndRedirect(this, "数据更新成功", "JHJX_SPGL.aspx");
                //        }
                //        else
                //        {
                //            MessageBox.ShowAndRedirect(this, "数据更新失败！", "JHJX_SPGL.aspx");
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.ShowAndRedirect(this, "数据更新成功", "JHJX_SPGL.aspx");
                //    }
                    
                //}
                //else
                //{
                //    MessageBox.ShowAndRedirect(this, "数据更新成功！", "JHJX_SPGL.aspx");
                //}
                #endregion
            }
            else
            {
                MessageBox.ShowAlertAndBack(this, "数据更新失败！");
            }

        }       
        

    }
    protected void btnCancle_Click(object sender, EventArgs e)
    {
        string url = "JHJX_SPGL.aspx";
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