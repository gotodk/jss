﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_New2013_JHJX_XYPJ_JYFPFZJMX : System.Web.UI.Page
{
   public static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            setchengshi();
            //SetItems();
            DisGrid();

        }
    }

    protected void setchengshi()
    {
       
        DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处'");
        ddlssfgs.DataSource = ds;
        ddlssfgs.DataTextField = "DYFGSMC";
        ddlssfgs.DataValueField = "DYFGSMC";
        ddlssfgs.DataBind();
        ddlssfgs.Items.Insert(0, new ListItem("全部分公司", ""));

     
        string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        string bm = DbHelperSQL.GetSingle(sql).ToString();
        if (bm.IndexOf("办事处") > 0)
        {
            sql = "select count(name) from system_city where name='" + bm + "'";
            //bm = bm.Replace("办事处", "分公司");
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                string strfgs = "select DYFGSMC FROM System_City where Name='" + bm + "'";
                string fgs = DbHelperSQL.GetSingle(strfgs).ToString();
                ddlssfgs.DataSource = null;
                ddlssfgs.DataBind();
                ddlssfgs.Items.Clear();
                ddlssfgs.Items.Add(fgs);
             
            }
        }
    }

    protected void SetItems()
    {
       // DataSet ds = DbHelperSQL.Query("select SXNR from AAA_WGSXB");


        //ddlwgsx.DataSource = ds;
        //ddlwgsx.DataTextField = "SXNR";
        //ddlwgsx.DataValueField = "SXNR";
        //ddlwgsx.DataBind();
        //ddlwgsx.Items.Insert(0, new ListItem("请选择违规事项", ""));
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
        return " ( SELECT [AAA_JYFXYMXB].[Number]as 主键,[DLYX]as 交易方账号 ,[JYZHLX] as 交易账户类型 ,(select  top 1 DD.I_PTGLJG  from  dbo.AAA_MJMJJYZHYJJRZHGLB as GL left join dbo.AAA_DLZHXXB as DD on GL.GLJJRBH=DD.J_JJRJSBH  where DLYX= DLYX and SFDQMRJJR='是') as 所属分公司,[JFSX] as 原因,[FS]as 分数 ,YSLX as 类型 ,[AAA_JYFXYMXB].[CreateTime]as 创建时间,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别  FROM [AAA_JYFXYMXB] join [AAA_DLZHXXB] on [DLYX]=[B_DLYX] ) tab1 "; 
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

        HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and 所属分公司 like '%" + ddlssfgs.SelectedValue.ToString() + "%' and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%' ";

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string sql = "select * from" + GetSql() + " where  所属分公司 like '%" + ddlssfgs.SelectedValue.ToString() + "%' and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%' ";

        DataSet dataSet = DbHelperSQL.Query(sql);
        DataTable dtable = dataSet.Tables[0];

        if (dtable != null && dtable.Rows.Count > 0)
        {
            dtable.Columns.Remove("主键");

            // dtable.Columns["MB001"].ColumnName = "品号";   
            //dtable.Columns["批号"].SetOrdinal(4);
            // DataTableToExcel(dtable);

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "交易方评分变化明细表", "交易方评分变化明细表", 15);
        }
        else
        {
            MessageBox.Show(this, "没有可以导出的数据，请重新进行查询！");

        }

    }
   
    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        //Label lbgg = (Label)e.Item.FindControl("lbgg");
      
        Label lbjyfmc = (Label)e.Item.FindControl("lbjyfmc");
        if (lbjyfmc.Text.Length > 15)
        {
            lbjyfmc.Text = lbjyfmc.Text.Substring(0, 15) + "...";
        }

     
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //if (e.CommandName == "lck")
        //{

        //    DataRow[] df = dt.Select("主键='" + e.CommandArgument.ToString().Trim() + "'");
        //    this.Context.Items.Add("Text", df);
        //    Server.Transfer("ViewWGKF.aspx");

        //}
    }

}