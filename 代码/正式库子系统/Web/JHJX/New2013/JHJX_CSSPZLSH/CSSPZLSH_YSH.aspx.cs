using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Hesion.Brick.Core;

public partial class Web_JHJX_New2013_JHJX_CSSPZLSH_CSSPZLSH_YSH : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            DisGrid();

        }
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " ( select a.Number, a.DLYX 交易方账号,b.I_JYFMC 交易方名称,a.SQSJ 申请时间,a.SPBH 商品编号,c.SPMC 商品名称,a.SHZT 审核状态,a.SHSJ 审核时间,a.SHR 审核人,a.SFBGZZHBZ 是否变更资质或标准,c.SFYX 是否有效 from AAA_CSSPB a left join AAA_DLZHXXB b on a.DLYX=b.B_DLYX left join AAA_PTSPXXB c on a.SPBH=c.SPBH where a.SHZT in ('审核通过','驳回') ) as tab ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 商品编号 ";  //所检索表的主键
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " 审核时间";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/JHJX_CSSPZLSH/JHJX_CKXQ.aspx";
        return ht_where;
    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        // Response.Write(ERRinfo);
        //Response.End();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.Show(this, "查询超时，请重试或者修改查询条件！");
        }

        Repeater1.DataSource = null;

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
            ts.Visible = false;
        }
        else
        {
            DataSet ds = DbHelperSQL.Query("select '交易方账号'='','交易方名称'='','申请时间'='','商品编号'='','商品名称'='','审核状态'='','审核时间'='','审核人'='' from AAA_CSSPB  where 1!=1");
            Repeater1.DataSource = ds.Tables[0].DefaultView;
            ts.Visible = true;

        }
        Repeater1.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();


        string sql_where = " and 交易方账号 like '%" + txtJYFZH.Text.Trim() + "%' and 交易方名称 like '%" + txtJYFMC.Text.Trim() + "%' and 审核人 like '%" + txtSHR.Text.Trim()+ "%'";

        string StartTime = txtBeginTime.Text.Trim();
        string EndTime = txtEndTime.Text.Trim();
        if (StartTime != "" && EndTime == "")
        {
            sql_where = sql_where + " and 申请时间 >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (StartTime == "" && EndTime != "")
        {
            sql_where = sql_where + " and  申请时间 <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (StartTime != "" && EndTime != "")
        {
            sql_where = sql_where + " and 申请时间 between '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }

        string SHStartTime = SHTimeStart.Text.Trim();
        string SHEndTime = SHTimeEnd.Text.Trim();
        if (SHStartTime != "" && SHEndTime == "")
        {
            sql_where = sql_where + " and 审核时间 >= '" + Convert.ToDateTime(SHStartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (SHStartTime == "" && SHEndTime != "")
        {
            sql_where = sql_where + " and  审核时间 <= '" + Convert.ToDateTime(SHEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (SHStartTime != "" && SHEndTime != "")
        {
            sql_where = sql_where + " and 审核时间 between '" + Convert.ToDateTime(SHStartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(SHEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }

        HTwhere["search_str_where"] += sql_where;

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.Charset = "";
        string filename = "JYPT_CSSPZLSHYSH" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" +

        System.Web.HttpUtility.UrlEncode(filename, System.Text.Encoding.GetEncoding("GB2312")) + ".xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   

        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        #region//asp.net 导出Excel时，解决纯数字字符串变成类似这样的 2.00908E+18 形式的代码
        string strStyle = "<style>td{vnd.ms-excel.numberformat: @;}</style>";//导出的数字的样式
        tw.WriteLine(strStyle);
        #endregion

        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    }
    // 导出Exel时需重载此方法
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandArgument == "ck")
        {
            //string[] str = e.CommandName.Split('&');

            Response.Redirect("JHJX_CKXQ.aspx?number=" + e.CommandName.Trim() + "&GoBackUrl=CSSPZLSH_YSH.aspx");
        }        
    }
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        string str = "select * from  ( select a.DLYX 交易方账号,b.I_JYFMC 交易方名称,a.SQSJ 申请时间,a.SPBH 商品编号,c.SPMC 商品名称,a.SHZT 审核状态,a.SHSJ 审核时间,a.SHR 审核人,a.SFBGZZHBZ 是否变更资质或标准,c.SFYX 是否有效 from AAA_CSSPB a left join AAA_DLZHXXB b on a.DLYX=b.B_DLYX left join AAA_PTSPXXB c on a.SPBH=c.SPBH  where a.SHZT in ('审核通过','驳回')) as tab ";
        str += " where 交易方账号 like '%" + txtJYFZH.Text.Trim() + "%' and 交易方名称 like '%" + txtJYFMC.Text.Trim() + "%' and 审核人 like '%"+txtSHR.Text.Trim()+"%'";
        string StartTime = txtBeginTime.Text.Trim();
        string EndTime = txtEndTime.Text.Trim();
        if (StartTime != "" && EndTime == "")
        {
            str = str + " and 申请时间 >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (StartTime == "" && EndTime != "")
        {
            str = str + " and  申请时间 <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (StartTime != "" && EndTime != "")
        {
            str = str + " and 申请时间 between '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        string SHStartTime = SHTimeStart.Text.Trim();
        string SHEndTime = SHTimeEnd.Text.Trim();
        if (SHStartTime != "" && SHEndTime == "")
        {
            str = str + " and 审核时间 >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (SHStartTime == "" && SHEndTime != "")
        {
            str = str + " and  审核时间 <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (SHStartTime != "" && SHEndTime != "")
        {
            str = str + " and 审核时间 between '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }

        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Repeater2.DataSource = ds.Tables[0].DefaultView;
            exprestTD.Visible = false;
            Repeater2.DataBind();
        }
        else
        {
            exprestTD.Visible = true;
        }
        ToExcel(export);
    }
}