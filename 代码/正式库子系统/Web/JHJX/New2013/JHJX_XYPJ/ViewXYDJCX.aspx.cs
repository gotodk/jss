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

public partial class Web_JHJX_New2013_JHJX_XYPJ_ViewXYDJCXaspx : System.Web.UI.Page
{
    public static DataTable dt;
    public static string  dlyx="";
    protected void Page_Load(object sender, EventArgs e)
    {
       // commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);

        if (Context.Handler.ToString() == "ASP.web_jhjx_new2013_jhjx_xypj_jyfxydjcx_aspx")
        {
            DataRow[] drs = this.Context.Items["Text"] as DataRow[];

            InitialData(drs[0]);
        }
        DisGrid();

    }
   
    protected void InitialData(DataRow dr)
    {
        lbjf.Text = dr["信用分值"].ToString();
        lblx.Text = dr["交易账户类型"].ToString().TrimEnd(new char[]{'交','易','账','户'});
        dlyx=lbzh.Text = dr["交易方账号"].ToString().Trim();
        lbmc.Text = dr["交易方名称"].ToString();
    }

   
    
    protected string GetSql()
    {
        return " ( SELECT [AAA_JYFXYMXB].[Number]as 主键,[DLYX]as 交易方账号 ,[JYZHLX] as 交易账户类型 ,(select  top 1 DD.I_PTGLJG  from  dbo.AAA_MJMJJYZHYJJRZHGLB as GL left join dbo.AAA_DLZHXXB as DD on GL.GLJJRBH=DD.J_JJRJSBH  where DLYX= DLYX and SFDQMRJJR='是') as 所属分公司,[JFSX] as 原因,[FS]as 分数 ,YSLX as 类型 ,[AAA_JYFXYMXB].[CreateTime]as 创建时间,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别, isnull( B_ZHDQXYFZ,0) as 信用积分  FROM [AAA_JYFXYMXB] join [AAA_DLZHXXB] on [DLYX]=[B_DLYX] where [DLYX]='"+dlyx+"' ) tab1 ";
    }


    //设置初始默认值检索
    private DataTable SetV()
    {
        DataTable dt = DbHelperSQL.Query("SELECT [AAA_JYFXYMXB].[Number]as 主键,[DLYX]as 交易方账号 ,[JYZHLX] as 交易账户类型 ,(select  top 1 DD.I_PTGLJG  from  dbo.AAA_MJMJJYZHYJJRZHGLB as GL left join dbo.AAA_DLZHXXB as DD on GL.GLJJRBH=DD.J_JJRJSBH  where DLYX= DLYX and SFDQMRJJR='是') as 所属分公司,[JFSX] as 原因,[FS]as 分数 ,YSLX as 类型 ,[AAA_JYFXYMXB].[CreateTime]as 创建时间,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别, isnull( B_ZHDQXYFZ,0) as 信用积分  FROM [AAA_JYFXYMXB] join [AAA_DLZHXXB] on [DLYX]=[B_DLYX] where [DLYX]='" + dlyx + "' order by [AAA_JYFXYMXB].[Number] desc ").Tables[0];

        //for (int i = dt.Rows.Count - 1; i > 0; i--)
        //{
        //    dt.Rows[i - 1]["信用积分"] = Convert.ToDouble(dt.Rows[i]["信用积分"]) - Convert.ToDouble(dt.Rows[i]["分数"]);
        //}

        for (int i = 1; i < dt.Rows.Count; i++)
        {
              dt.Rows[i ]["信用积分"] = Convert.ToDouble(dt.Rows[i-1]["信用积分"]) - Convert.ToDouble(dt.Rows[i-1]["分数"]);
        }
        return dt;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        DataTable  dtable = SetV();

        //HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and 所属分公司 like '%" + ddlssfgs.SelectedValue.ToString() + "%' and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%' ";

        string sql_where = "1=1";

        string StartTime = txtBeginTime.Text.Trim();
        string EndTime = txtEndTime.Text.Trim();
        if (StartTime != "" && EndTime == "")
        {
            sql_where = sql_where + " and 创建时间 >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (StartTime == "" && EndTime != "")
        {
            sql_where = sql_where + " and  创建时间<= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (StartTime != "" && EndTime != "")
        {
            sql_where = sql_where + " and 创建时间 >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and 创建时间 <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }

        DataRow[] drs = dtable.Select(sql_where);

        DataTable d = new DataTable();


        if (drs != null && drs.Length > 0)
        {
            tdEmpty.Visible = false;


            rpt.DataSource = drs ;
        }
        else { tdEmpty.Visible = true; }
        rpt.DataBind();



    

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    //protected void btnExcel_Click(object sender, EventArgs e)
    //{
    //    string sql = "select * from" + GetSql() + " where  所属分公司 like '%" + ddlssfgs.SelectedValue.ToString() + "%' and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%' ";

    //    DataSet dataSet = DbHelperSQL.Query(sql);
    //    DataTable dtable = dataSet.Tables[0];

    //    if (dtable != null && dtable.Rows.Count > 0)
    //    {
    //        dtable.Columns.Remove("主键");

    //        // dtable.Columns["MB001"].ColumnName = "品号";   
    //        //dtable.Columns["批号"].SetOrdinal(4);
    //        // DataTableToExcel(dtable);

    //        MyXlsClass MXC = new MyXlsClass();

    //        MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "交易方评分变化明细表", "交易方评分变化明细表", 15);
    //    }
    //    else
    //    {
    //        MessageBox.Show(this, "没有可以导出的数据，请重新进行查询！");

    //    }

    //}

    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        //Label lbgg = (Label)e.Item.FindControl("lbgg");

        //Label lbjyfmc = (Label)e.Item.FindControl("lbjyfmc");
        //if (lbjyfmc.Text.Length > 15)
        //{
        //    lbjyfmc.Text = lbjyfmc.Text.Substring(0, 15) + "...";
        //}


    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
      
    }
    protected void btnfh_Click(object sender, EventArgs e)
    {
        string script = " <script language='javascript'>window.open('JYFXYDJCX.aspx','_self');</script>";

        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseWindow", script);
    }
}