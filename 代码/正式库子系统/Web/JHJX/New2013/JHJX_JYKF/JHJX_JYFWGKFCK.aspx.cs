using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;


public partial class Web_JHJX_New2013_JHJX_JYKF_JHJX_JYFWGKFCK : System.Web.UI.Page
{
    public static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();
           
            SetItems();
            DisGrid();

        }
    }
   

    protected void SetItems()
    {
        DataSet ds = DbHelperSQL.Query("select SXNR from AAA_WGSXB");


        ddlwgsx.DataSource = ds;
        ddlwgsx.DataTextField = "SXNR";
        ddlwgsx.DataValueField = "SXNR";
        ddlwgsx.DataBind();
        ddlwgsx.Items.Insert(0, new ListItem("请选择违规事项", ""));
    }
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {

        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            dt = NewDS.Tables[0];
            rpt.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rpt.DataBind();
    }
    protected string GetSql()
    {
        return " (SELECT [AAA_WGKFB].[Number]as 主键,[DLYX]as 交易方账号 ,[I_JYFMC] as 交易方名称,[JSZHLX] as 交易账户类型 ,[I_ZCLB]as 注册类别,[KFJE]as 交易方扣罚 ,[JJRKFJE] as 经纪人扣罚 ,[WGSX] as 扣罚原因, [GLJJRYX] as 经纪人账号,isnull([I_YWGLBMFL],'') as 部门分类,[GLJJRPTGLJG] as 业务管理部门,[KFPZ] as 扣罚凭证 ,[QKJS] as 情况简述  ,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[AAA_WGKFB].[CreateUser] as 操作人,[AAA_WGKFB].[CreateTime]as 创建时间 FROM [AAA_WGKFB] join [AAA_DLZHXXB] on [DLYX]=[B_DLYX] ) tab1 "; 
    }


    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] =GetSql();  //检索的表
        ht_where["search_mainid"] = " 主键 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " 主键 ";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%'  and 业务管理部门 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%'   and 扣罚原因 like '%" + ddlwgsx.SelectedValue.Trim() + "%'";

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string sql = "select * from" + GetSql() + " where   部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%'  and 业务管理部门 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%'   and 扣罚原因 like '%" + ddlwgsx.SelectedValue.Trim() + "%'";

        DataSet dataSet = DbHelperSQL.Query(sql);
        DataTable dtable = dataSet.Tables[0];

        if (dtable != null && dtable.Rows.Count > 0)
        {
            dtable.Columns.Remove("主键");
            dtable.Columns.Remove("部门分类");
            dtable.Columns.Remove("情况简述");
            dtable.Columns.Remove("扣罚凭证");

            // dtable.Columns["MB001"].ColumnName = "品号";   
            //dtable.Columns["批号"].SetOrdinal(4);
            // DataTableToExcel(dtable);

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "扣罚一览表", "扣罚一览表", 15);
        }
        else
        {
            MessageBox.Show(this, "没有可以导出的数据，请重新进行查询！");

        }

    }
   
    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        //Label lbgg = (Label)e.Item.FindControl("lbgg");
        Label lbjjrkf = (Label)e.Item.FindControl("lbjjrkf");
        if (lbjjrkf.Text.Trim() == "0")
        {
            lbjjrkf.Text = "---"; 
        }
        Label lbjyfmc = (Label)e.Item.FindControl("lbjyfmc");
        if (lbjyfmc.Text.Length > 15)
        {
            lbjyfmc.Text = lbjyfmc.Text.Substring(0, 15) + "...";
        }

        //if (lbgg.Text.Length > 10)
        //    lbgg.Text = lbgg.Text.Substring(0, 10) + "...";
        //if (lbspmc.Text.Length > 10)
        //    lbspmc.Text = lbspmc.Text.Substring(0, 10) + "...";
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "lck")
        {

            DataRow[] df = dt.Select("主键='" + e.CommandArgument.ToString().Trim() + "'");
            this.Context.Items.Add("Text", df);
            Server.Transfer("ViewWGKF.aspx");

        }

    }
}