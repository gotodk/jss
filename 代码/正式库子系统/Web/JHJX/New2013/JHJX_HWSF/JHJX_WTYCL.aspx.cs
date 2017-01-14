using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.Common;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_HWSF_JHJX_WTYCL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();
            UCFWJGDetail1.LableName = new string[] { "买方" };
            UCFWJGDetail2.initdefault();
            UCFWJGDetail2.LableName = new string[] { "卖方" };
            DisGrid();

        }
    }

    //protected string GetSql()
    //{
    //    return "(select a.Number as Number, (case when ( a.F_DQZT  = '部分收货' or a.F_DQZT  = '已录入补发备注' or a.F_DQZT  = '补发货物无异议收货' ) then a.F_BUYBFSHCZSJ  when ( a.F_DQZT  = '有异议收货' or a.F_DQZT  = '有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then a.F_BUYYYYSHCZSJ  when ( a.F_DQZT  = '请重新发货' or a.F_DQZT  = '撤销' ) then a.F_BUYQZXFHCZSJ  end)  as 问题产生时间,case when (a.F_DQZT='部分收货' or a.F_DQZT='已录入补发备注' or a.F_DQZT='补发货物无异议收货' ) then '部分收货' when ( a.F_DQZT='有异议收货' or a.F_DQZT='有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then '有异议收货' when (a.F_DQZT='请重新发货' or a.F_DQZT='撤销'  ) then '请重新发货' end as 问题类别及详情 , (case when (a.F_DQZT = '补发货物无异议收货' or a.F_DQZT = '有异议收货后无异议收货' or a.F_DQZT = '卖家主动退货' or a.F_DQZT = '撤销' ) then '完成' else '未完成' end ) as 处理状态  ,'F'+a.Number as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买家名称,(select isnull(I_YWGLBMFL,'') from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买家部门分类,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDGLJJRYX) as 买家所属分公司,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖家名称, (select  isnull(I_YWGLBMFL,'') from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖家部门分类,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = b.T_YSTBDGLJJRYX) as 卖家所属分公司 , a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单编号 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  where F_DQZT in ('部分收货','已录入补发备注','补发货物无异议收货','有异议收货','有异议收货后无异议收货','卖家主动退货','请重新发货','撤销')) as tab ";
    //}
    
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = GetSql();
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件
        //ht_where["search_mainid"] = " 发货单号 ";  //所检索表的主键
        //ht_where["search_paixu"] = " asc ";  //排序方式
        //ht_where["search_paixuZD"] = " 问题产生时间 ";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_HWSF/FHD.aspx";

        /*---shiyan 2013-12-18 数据获取方式优化---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
        ht_where["serach_Row_str"] = " eb.B_DLYX as 买方账号,es.B_DLYX as 卖方账号, a.Number, (case when ( a.F_DQZT= '部分收货' or a.F_DQZT= '已录入补发备注' or a.F_DQZT= '补发货物无异议收货' ) then a.F_BUYBFSHCZSJ  when ( a.F_DQZT= '有异议收货' or a.F_DQZT = '有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then a.F_BUYYYYSHCZSJ  when ( a.F_DQZT= '请重新发货' or a.F_DQZT= '撤销' ) then a.F_BUYQZXFHCZSJ  end)  as 问题产生时间,(case when (a.F_DQZT='部分收货' or a.F_DQZT='已录入补发备注' or a.F_DQZT='补发货物无异议收货' ) then '部分收货' when ( a.F_DQZT='有异议收货' or a.F_DQZT='有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then '有异议收货' when (a.F_DQZT='请重新发货' or a.F_DQZT='撤销'  ) then '请重新发货' end) as 问题类别及详情 , (case when (a.F_DQZT = '补发货物无异议收货' or a.F_DQZT = '有异议收货后无异议收货' or a.F_DQZT = '卖家主动退货' or a.F_DQZT = '撤销' ) then '完成' else '未完成' end ) as 处理状态  ,'F'+a.Number as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,eb.I_JYFMC as 买家名称,eb.I_YWGLBMFL as 买家部门分类,eb.I_PTGLJG as 买家所属分公司,es.I_JYFMC as 卖家名称, es.I_YWGLBMFL as 卖家部门分类,es.I_PTGLJG as 卖家所属分公司 , a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单编号 ";
        ht_where["search_tbname"] = " AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as eb on b.Y_YSYDDDLYX=eb.B_DLYX left join AAA_DLZHXXB as es on b.T_YSTBDDLYX=es.B_DLYX ";
        ht_where["search_str_where"] = " F_DQZT in ('部分收货','已录入补发备注','补发货物无异议收货','有异议收货','有异议收货后无异议收货','卖家主动退货','请重新发货','撤销') ";  //检索条件
        ht_where["search_mainid"] = " a.Number ";  //所检索表的主键
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " (case when (a.F_DQZT='部分收货' or a.F_DQZT='已录入补发备注' or a.F_DQZT='补发货物无异议收货') then a.F_BUYBFSHCZSJ when (a.F_DQZT='有异议收货' or a.F_DQZT='有异议收货后无异议收货' or a.F_DQZT='卖家主动退货') then a.F_BUYYYYSHCZSJ when (a.F_DQZT='请重新发货' or a.F_DQZT= '撤销') then a.F_BUYQZXFHCZSJ end) ";  //用于排序的字段



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
            ts.Visible = true;
        }
        Repeater1.DataBind();

    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        //string sql_where = " and 买家部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%' and 卖家部门分类 like '%" + UCFWJGDetail2.Value[0].ToString().Trim() + "%'  and 买家所属分公司 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 买家名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%'  and 卖家所属分公司 like '%" + UCFWJGDetail2.Value[1].ToString().Trim() + "%' and 卖家名称 like '%" + txtSellerMC.Text.Trim() + "%' and 发货单号 like '%" + txtFHDBH.Text.Trim() + "%'";
       // HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;
       
        /*---shiyan 2013-12-18 优化数据获取方式---*/
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
        if (txtSellerMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.I_JYFMC like '%" + txtSellerMC.Text.ToString().Trim() + "%' ";
        }
        ////商品名称
        //if (txtSPMC.Text.Trim() != "")
        //{
        //    HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.Z_SPMC like '%" + txtSPMC.Text.ToString().Trim() + "%' ";
        //}
        ////电子购货合同编号
        //if (txtDZGHHTBH.Text.Trim() != "")
        //{
        //    HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.Z_HTBH like '%" + txtDZGHHTBH.Text.ToString().Trim() + "%' ";
        //}
        //发货单编号
        if (txtFHDBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 'F'+a.Number like '%" + txtFHDBH.Text.ToString().Trim() + "%' ";
        }
        // 买方账号
        if (txtBuyzh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and eb.B_DLYX like '%" + txtBuyzh.Text.ToString().Trim() + "%' ";
        }
        //卖方账号
        if (txtSelzh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.B_DLYX like '%" + txtSelzh.Text.ToString().Trim() + "%' ";
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

        //string sql = "select * from" + GetSql() + " where  买家部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%' and 卖家部门分类 like '%" + UCFWJGDetail2.Value[0].ToString().Trim() + "%'  and 买家所属分公司 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 买家名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%'  and 卖家所属分公司 like '%" + UCFWJGDetail2.Value[1].ToString().Trim() + "%' and 卖家名称 like '%" + txtSellerMC.Text.Trim() + "%' and 发货单号 like '%" + txtFHDBH.Text.Trim() + "%'";

        string sql = "select a.Number, (case when ( a.F_DQZT= '部分收货' or a.F_DQZT= '已录入补发备注' or a.F_DQZT= '补发货物无异议收货' ) then a.F_BUYBFSHCZSJ  when ( a.F_DQZT= '有异议收货' or a.F_DQZT = '有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then a.F_BUYYYYSHCZSJ  when ( a.F_DQZT= '请重新发货' or a.F_DQZT= '撤销' ) then a.F_BUYQZXFHCZSJ  end)  as 问题产生时间,(case when (a.F_DQZT='部分收货' or a.F_DQZT='已录入补发备注' or a.F_DQZT='补发货物无异议收货' ) then '部分收货' when ( a.F_DQZT='有异议收货' or a.F_DQZT='有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then '有异议收货' when (a.F_DQZT='请重新发货' or a.F_DQZT='撤销'  ) then '请重新发货' end) as 问题类别及详情 , (case when (a.F_DQZT = '补发货物无异议收货' or a.F_DQZT = '有异议收货后无异议收货' or a.F_DQZT = '卖家主动退货' or a.F_DQZT = '撤销' ) then '完成' else '未完成' end ) as 处理状态  ,'F'+a.Number as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,eb.B_DLYX as 买方账号,eb.I_JYFMC as 买方名称,eb.I_YWGLBMFL as 买家部门分类,eb.I_PTGLJG as 买方管理部门,es.B_DLYX as 买方账号,  es.I_JYFMC as 卖方名称, es.I_YWGLBMFL as 卖家部门分类,es.I_PTGLJG as 卖方管理部门 , a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单编号 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as eb on b.Y_YSYDDDLYX=eb.B_DLYX left join AAA_DLZHXXB as es on b.T_YSTBDDLYX=es.B_DLYX   where " + ViewState["ht_where"].ToString() + " order by  (case when ( a.F_DQZT  = '部分收货' or a.F_DQZT  = '已录入补发备注' or a.F_DQZT  = '补发货物无异议收货' ) then a.F_BUYBFSHCZSJ  when ( a.F_DQZT  = '有异议收货' or a.F_DQZT  = '有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then a.F_BUYYYYSHCZSJ  when ( a.F_DQZT  = '请重新发货' or a.F_DQZT  = '撤销' ) then a.F_BUYQZXFHCZSJ  end)";

        DataSet dtable = DbHelperSQL.Query(sql);

        if (dtable != null && dtable.Tables [0].Rows.Count > 0)
        {
            dtable.Tables [0].Columns.Remove("Number");

            // dtable.Columns["MB001"].ColumnName = "品号";   
            //dtable.Columns["批号"].SetOrdinal(4);
           // DataTableToExcel(dtable);

            MyXlsClass MXC = new MyXlsClass();
            MXC.goxls(dtable, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "问题与处理", "问题与处理", 15);
        }
        else
        {
            MessageBox.Show(this, "没有可以导出的数据，请重新进行查询！");

        }
    }

    //public static void DataTableToExcel(System.Data.DataTable dtData)
    //{
    //    System.Web.UI.WebControls.DataGrid dgExport = null;
    //    // 当前对话
    //    System.Web.HttpContext curContext = System.Web.HttpContext.Current;
    //    // IO用于导出并返回excel文件
    //    System.IO.StringWriter strWriter = null;
    //    System.Web.UI.HtmlTextWriter htmlWriter = null;

    //    if (dtData != null)
    //    {
    //        // 设置编码和附件格式
    //        string style = @"<style> .text { mso-number-format:\@; } </script> "; //设置格式，解决电话不显示开头是0的问题
    //        string FileName = "";
    //        FileName = "Report-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
    //        curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
    //        curContext.Response.Charset = "UTF-8";
    //        curContext.Response.ContentEncoding = System.Text.Encoding.Default;
    //        curContext.Response.ContentType = "application/ms-excel";

    //        // 导出excel文件
    //        strWriter = new System.IO.StringWriter();
    //        htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

    //        // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid
    //        dgExport = new System.Web.UI.WebControls.DataGrid();
    //        dgExport.DataSource = dtData.DefaultView;
    //        dgExport.AllowPaging = false;
    //        dgExport.DataBind();

    //        // 返回客户端
    //        dgExport.RenderControl(htmlWriter);
    //        curContext.Response.Write(style);//调用格式化字符串
    //        curContext.Response.Write(strWriter.ToString());
    //        curContext.Response.End();
    //    }
    //}

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandArgument == "FHD")
        {

            Response.Redirect("FHD.aspx?Number=" + e.CommandName + "&GoBackUrl=JHJX_WTYCL.aspx");

        }

        else if (e.CommandArgument == "WTLBJXQ")//
        {

            Response.Redirect("WTLBJXQ.aspx?Number=" + e.CommandName + "&GoBackUrl=JHJX_WTYCL.aspx");
        }

    }
}