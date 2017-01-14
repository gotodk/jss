using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Configuration;

public partial class Web_JHJX_New2013_JHJX_SPMM_JHJX_QP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();
            UCFWJGDetail1.LableName = new string[] { "买家" };
            UCFWJGDetail2.initdefault();
            UCFWJGDetail2.LableName = new string[] { "卖家" };
            DisGrid();

        }
    }
   

    protected string GetSql()
    {
        //return " (select Number,    isnull( CONVERT(varchar(100), Z_QPKSSJ, 20),'---') as '清盘开始时间' , case  when  Z_QPJSSJ IS NULL then '---' else CONVERT(varchar(100), Z_QPJSSJ, 20)   end  as '清盘结束时间',CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END AS 清盘原因,case when isnull(Z_QPKSSJ,'') = isnull(Z_QPJSSJ,'') then '自动清盘' else '人工清盘' end as 清盘类型,Z_HTBH as 电子购货合同编号 , Z_QPZT as 清盘状态,case when (select   SUM(ISNULL( JE ,0.00))  from AAA_ZKLSMXB where XM = '订金' and YSLX = '解冻'  and SJLX = '实'  and  LYDH = AAA_ZBDBXXB.Number) is not null then CAST(  (select   SUM(ISNULL( JE ,0.00))  from AAA_ZKLSMXB where XM = '订金' and YSLX = '解冻'  and SJLX = '实'  and  LYDH = AAA_ZBDBXXB.Number)   as varchar(200) ) else '---' end as 订金解冻金额 ,case when (select   SUM(ISNULL( JE ,0.00))  from AAA_ZKLSMXB where XM = '履约保证金' and YSLX = '解冻' and SJLX='实' and LYDH = AAA_ZBDBXXB.Number) is not null then CAST( (select   SUM(ISNULL( JE ,0.00))  from AAA_ZKLSMXB where XM = '履约保证金' and YSLX = '解冻' and SJLX='实' and LYDH = AAA_ZBDBXXB.Number)   as varchar(200) ) else '---' end  as 履约保证金解冻金额,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = Y_YSYDDDLYX) as 买家名称,Y_YSYDDDLYX as 买家账号,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = Y_YSYDDGLJJRYX) as 买家所属分公司,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = T_YSTBDDLYX) as 卖家名称 ,T_YSTBDDLYX as 卖家账号, (select I_PTGLJG from AAA_DLZHXXB where B_DLYX = T_YSTBDGLJJRYX) as 卖家所属分公司, isnull( Q_SFYQR,'---') as '是否确认'  from AAA_ZBDBXXB   where  (Z_QPZT='清盘中' or Z_QPZT='清盘结束' ) ) as tab ";

        return " ( select Number,    isnull( CONVERT(varchar(100), Z_QPKSSJ, 20),'---') as '清盘开始时间' , case  when  Z_QPJSSJ IS NULL then '---' else CONVERT(varchar(100), Z_QPJSSJ, 20)   end  as '清盘结束时间',CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END AS 清盘原因,case when isnull(Z_QPKSSJ,'') = isnull(Z_QPJSSJ,'') then '自动清盘' else '人工清盘' end as 清盘类型,Z_HTBH as 电子购货合同编号 , Z_QPZT as 清盘状态,case when c.djjdje is not null then CAST(  c.djjdje   as varchar(200) ) else '---' end as 订金解冻金额 ,case when d.bzjjdje  is not null then CAST( d.bzjjdje   as varchar(200) ) else '---' end  as 履约保证金解冻金额,e.I_JYFMC  as 买家名称,Y_YSYDDDLYX as 买家账号,e.I_PTGLJG as 买家所属分公司,f.I_JYFMC as 卖家名称 ,T_YSTBDDLYX as 卖家账号, f.I_PTGLJG  as 卖家所属分公司, isnull(e.I_YWGLBMFL,'') as 买家部门分类, isnull(f.I_YWGLBMFL,'') as 卖家部门分类,   isnull( Q_SFYQR,'---') as '是否确认'  from AAA_ZBDBXXB   left join (select LYDH, SUM(ISNULL( JE ,0.00)) as djjdje  from AAA_ZKLSMXB where XM = '订金' and YSLX = '解冻'  and SJLX = '实' group by  LYDH ) c on c.LYDH=AAA_ZBDBXXB.Number left join (select LYDH,  SUM(ISNULL( JE ,0.00)) as bzjjdje  from AAA_ZKLSMXB where XM = '履约保证金' and YSLX = '解冻' and SJLX='实' and  group by  LYDH) d on d.LYDH=AAA_ZBDBXXB.Number left join (select  B_DLYX , I_JYFMC,I_PTGLJG,I_YWGLBMFL from AAA_DLZHXXB) e on e.B_DLYX =Y_YSYDDDLYX left join (select  B_DLYX , I_JYFMC,I_PTGLJG,I_YWGLBMFL from AAA_DLZHXXB) f on f.B_DLYX=T_YSTBDDLYX where  (Z_QPZT='清盘中' or Z_QPZT='清盘结束' ) ) as tab";

    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = GetSql();
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件
        //ht_where["search_mainid"] = "Number ";  //所检索表的主键
        //ht_where["search_paixu"] = " asc ";  //排序方式
        //ht_where["search_paixuZD"] = " 清盘开始时间";  //用于排序的字段

        /*---shiyan 2013-12-16 进行数据获取优化。---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是
        ht_where["serach_Row_str"] = " a.Number,convert(varchar(30),a.Z_QPKSSJ,120) as '清盘开始时间' , (case when Z_QPJSSJ is null then '---' else convert(varchar(30),Z_QPJSSJ,120) end) as '清盘结束时间',(CASE a.Z_HTZT WHEN '未定标废标' THEN '废标' WHEN '定标合同到期' THEN '《电子购货合同》期满' WHEN '定标合同终止' THEN '废标' WHEN '定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END) AS 清盘原因,(case when Z_QPZT='清盘结束' and a.Z_QPKSSJ=a.Z_QPJSSJ then '自动清盘' else '人工清盘' end) as 清盘类型,Z_HTBH as 电子购货合同编号 , Z_QPZT as 清盘状态,'---' as 订金解冻金额 ,'---' as 履约保证金解冻金额,eb.I_JYFMC  as 买家名称,Y_YSYDDDLYX as 买家账号,eb.I_PTGLJG as 买家所属分公司,es.I_JYFMC as 卖家名称 ,T_YSTBDDLYX as 卖家账号, es.I_PTGLJG  as 卖家所属分公司, eb.I_YWGLBMFL as 买家部门分类,eb.I_YWGLBMFL as 卖家部门分类,isnull(Q_SFYQR,'---') as '是否确认' ";
        ht_where["search_tbname"] = " AAA_ZBDBXXB as a left join AAA_DLZHXXB as eb on a.Y_YSYDDDLYX=eb.B_DLYX left join AAA_DLZHXXB as es on a.T_YSTBDDLYX=es.B_DLYX ";
        ht_where["search_str_where"] = " (Z_QPZT='清盘中' or Z_QPZT='清盘结束' ) ";  //检索条件
        ht_where["search_mainid"] = "a.Number ";  //所检索表的主键
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " Z_QPKSSJ ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/JHJX_SPMM/QPDetail.aspx";
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
            /*---shiyan 2013-12-19 数据获取方式优化---*/
            object objDJ = new object();
            object objLYBZJ = new object();
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                objDJ = DbHelperSQL.GetSingle("select ISNULL(SUM(JE),0.00) as 订金解冻金额  from AAA_ZKLSMXB where XM='订金' and YSLX='解冻' and SJLX='实' and LYDH='" + NewDS.Tables[0].Rows[i]["number"].ToString() + "' and LYYWLX='AAA_ZBDBXXB'");
                NewDS.Tables[0].Rows[i]["订金解冻金额"] = objDJ.ToString();

                if (NewDS.Tables[0].Rows[i]["清盘状态"].ToString() == "清盘结束")
                {
                    objLYBZJ = DbHelperSQL.GetSingle("select ISNULL(SUM(JE),0.00) as 履约保证金解冻金额  from AAA_ZKLSMXB where XM = '履约保证金' and YSLX = '解冻' and SJLX='实' and LYDH='" + NewDS.Tables[0].Rows[i]["number"].ToString() + "' and LYYWLX='AAA_ZBDBXXB'");
                    NewDS.Tables[0].Rows[i]["履约保证金解冻金额"] = objLYBZJ.ToString();
                }
            }
            /*---shiyan 结束---*/
            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
            ts.Visible = false;
        }
        else
        {
            ts.Visible = true;
        }
        Repeater1.DataBind();
    }

    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        // string sql_where = " and 买家所属分公司 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 买家名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 清盘类型 like '%" + ddlqplx.SelectedValue.Trim() + "%' and 电子购货合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' and 卖家所属分公司 like '%" + UCFWJGDetail2.Value[1].ToString().Trim() + "%' and 卖家名称 like '%" + txtSebMC.Text.Trim() + "%' and 清盘状态 like '%" + ddlqpzt.SelectedValue.Trim() + "%' and 买家部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%' and 卖家部门分类 like '%" + UCFWJGDetail2.Value[0].ToString().Trim() + "%'";
        //  HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

        /*---shiyan 2013-12-19 优化数据获取方式---*/
        //买家业务管理机构
        if (UCFWJGDetail1.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and eb.I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString().Trim() + "' ";
        }
        if (UCFWJGDetail1.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and eb.I_PTGLJG='" + UCFWJGDetail1.Value[1].ToString().Trim() + "' ";
        }
        //卖家业务管理机构
        if (UCFWJGDetail2.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.I_YWGLBMFL='" + UCFWJGDetail2.Value[0].ToString().Trim() + "' ";
        }
        if (UCFWJGDetail2.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.I_PTGLJG='" + UCFWJGDetail2.Value[1].ToString().Trim() + "' ";
        }
        //买家名称
        if (txtBuyMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and eb.I_JYFMC like '%" + txtBuyMC.Text.ToString().Trim() + "%' ";
        }
        //卖家名称
        if (txtSebMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.I_JYFMC like '%" + txtSebMC.Text.ToString().Trim() + "%' ";
        }
        //电子购货合同编号
        if (txtDZGHHTBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.Z_HTBH like '%" + txtDZGHHTBH.Text.ToString().Trim() + "%' ";
        }
        //清盘类型
        if (ddlqplx.SelectedValue.ToString() == "自动清盘")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.Z_QPKSSJ=a.Z_QPJSSJ ";
        }
        else if (ddlqplx.SelectedValue.ToString() == "人工清盘")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and (a.Z_QPKSSJ<a.Z_QPJSSJ or a.Z_QPJSSJ is null)";
        }
        //清盘状态
        if (ddlqpzt.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.Z_QPZT='" + ddlqpzt.SelectedValue.ToString() + "'";
        }

        ViewState["ht_where"] = HTwhere["search_str_where"];
        /*---shiyan 结束---*/

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }

    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    protected void btnToExcel_Click(object sender, EventArgs e)
    {

        //string sql = "select * from" + GetSql() + " where 买家所属分公司 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 买家名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 清盘类型 like '%" + ddlqplx.SelectedValue.Trim() + "%' and 电子购货合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' and 卖家所属分公司 like '%" + UCFWJGDetail2.Value[1].ToString().Trim() + "%' and 卖家名称 like '%" + txtSebMC.Text.Trim() + "%' and 清盘状态 like '%" + ddlqpzt.SelectedValue.Trim() + "%' and 买家部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%' and 卖家部门分类 like '%" + UCFWJGDetail2.Value[0].ToString().Trim() + "%'";

        string sql = "select a.Number,convert(varchar(30),a.Z_QPKSSJ,120) as '清盘开始时间' , (case when Z_QPJSSJ is null then '---' else convert(varchar(30),Z_QPJSSJ,120) end) as '清盘结束时间',(CASE a.Z_HTZT WHEN '未定标废标' THEN '废标' WHEN '定标合同到期' THEN '《电子购货合同》期满' WHEN '定标合同终止' THEN '废标' WHEN '定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END) AS 清盘原因,(case when Z_QPZT='清盘结束' and a.Z_QPKSSJ=a.Z_QPJSSJ then '自动清盘' else '人工清盘' end) as 清盘类型,Z_HTBH as 电子购货合同编号 , Z_QPZT as 清盘状态,(select ISNULL(SUM(JE),0.00) as 订金解冻金额  from AAA_ZKLSMXB where XM='订金' and YSLX='解冻' and SJLX='实' and LYDH=a.Number and LYYWLX='AAA_ZBDBXXB') as 订金解冻金额 ,(case Z_QPZT when '清盘中' then '---' else (select cast(ISNULL(SUM(JE),0.00) as varchar(20)) as 履约保证金解冻金额  from AAA_ZKLSMXB where XM = '履约保证金' and YSLX = '解冻' and SJLX='实' and LYDH=a.Number and LYYWLX='AAA_ZBDBXXB') end) as 履约保证金解冻金额,eb.I_JYFMC  as 买家名称,Y_YSYDDDLYX as 买家账号,eb.I_PTGLJG as 买家所属分公司,es.I_JYFMC as 卖家名称 ,T_YSTBDDLYX as 卖家账号, es.I_PTGLJG  as 卖家所属分公司, eb.I_YWGLBMFL as 买家部门分类,eb.I_YWGLBMFL as 卖家部门分类,isnull(Q_SFYQR,'---') as '是否确认' from  AAA_ZBDBXXB as a left join AAA_DLZHXXB as eb on a.Y_YSYDDDLYX=eb.B_DLYX left join AAA_DLZHXXB as es on a.T_YSTBDDLYX=es.B_DLYX where " + ViewState["ht_where"].ToString() + " order by Z_QPKSSJ";

        DataSet dataSet=DbHelperSQL.Query(sql);
        DataTable dtable = dataSet.Tables[0];

        if (dtable != null && dtable.Rows.Count > 0)
        {
            dtable.Columns.Remove("Number");
            dtable.Columns.Remove("买家部门分类");
            dtable.Columns.Remove("卖家部门分类");

            dtable.Columns["买家所属分公司"].ColumnName = "买家所属部门分类";
            dtable.Columns["卖家所属分公司"].ColumnName = "卖家所属部门分类"; 
  
            dtable.Columns["清盘原因"].SetOrdinal(1);
            dtable.Columns["清盘类型"].SetOrdinal(2);
            dtable.Columns["清盘状态"].SetOrdinal(3);
            dtable.Columns["清盘结束时间"].SetOrdinal(4);
            // DataTableToExcel(dtable);

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "清盘一览表", "清盘一览表", 15);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "没有可以导出的数据，请重新进行查询！");

        }
    }

    public static void DataTableToExcel(System.Data.DataTable dtData)
    {
        System.Web.UI.WebControls.DataGrid dgExport = null;
        // 当前对话
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dtData != null)
        {
            // 设置编码和附件格式
            string style = @"<style> .text { mso-number-format:\@; } </script> "; //设置格式，解决电话不显示开头是0的问题
            string FileName = "";
            FileName = "Report-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
            curContext.Response.Charset = "UTF-8";
            curContext.Response.ContentEncoding = System.Text.Encoding.Default;
            curContext.Response.ContentType = "application/ms-excel";

            // 导出excel文件
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dtData.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            // 返回客户端
            dgExport.RenderControl(htmlWriter);
            curContext.Response.Write(style);//调用格式化字符串
            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();
        }
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandArgument == "DZGHHT")//电子购货合同
        {
            string url = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/pingtaiservices/moban/FMPTDZGHHT.aspx?Number=" + e.CommandName.ToString();
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.open('" + url + "');</script>");

        }

        else if (e.CommandArgument == "XQ")//
        {
            Label lbsfqr = (Label)e.Item.FindControl("lbsfqr");

            if (lbsfqr.Text != "是")
            {
                MessageBox.Show(this, "买卖双方尚未进行清盘确认！");
            }
            else
            {
                Response.Redirect("QPDetail.aspx?Number=" + e.CommandName + "&GoBackUrl=JHJX_QP.aspx");
 
            }
           
        }

    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lbqplx = (Label)e.Item.FindControl("lbqplx");
        Label lbsfqr = (Label)e.Item.FindControl("lbsfqr");
        LinkButton linlxq = (LinkButton)e.Item.FindControl("linlxq");

        if (lbqplx.Text == "自动清盘")
        {
            linlxq.Text = "--";
            linlxq.Enabled = false;

        }
        if (lbqplx.Text == "人工清盘")
        {
            if (lbsfqr.Text != "是")
                lbsfqr.Text = "否";
        }

    }
}