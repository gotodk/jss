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

public partial class Web_JHJX_New2013_JHJX_HWSF_JHJX_HWSFGK : System.Web.UI.Page
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
    //    return " (select  row_number() OVER (ORDER BY a.Number ASC) AS F1Order, a.Number as 投标定标单号, a.Z_SPMC as 商品名称,a.Z_SPBH as 商品编号,a.Z_GG as 规格 ,a.Z_HTBH as 对应的电子购货合同编号,a.Z_ZBSL as 定标数量,a.Z_ZBJG as 定标价格,a.Z_ZBJE as 定标金额 , sum( (case  when ( b.F_DQZT='无异议收货' or b.F_DQZT='默认无异议收货' or b.F_DQZT='补发货物无异议收货' or b.F_DQZT='有异议收货后无异议收货'  ) then isnull(b.T_THSL,0) else 0 end )) as 无异议收货数量, sum( (case when (b.F_DQZT='有异议收货' or b.F_DQZT='部分收货' ) then isnull(b.T_THSL,0) else 0 end) ) as 有异议收货数量,  case when a.Z_HTJSRQ is null then '---' else CONVERT(varchar(100), a.Z_HTJSRQ, 20) end as 对应电子购货合同结束日期,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = a.Y_YSYDDDLYX) as 买家名称,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = a.Y_YSYDDGLJJRYX) as 买家所属分公司,(select isnull(I_YWGLBMFL,'') from AAA_DLZHXXB where B_DLYX = a.Y_YSYDDDLYX) as 买家部门分类,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = a.T_YSTBDDLYX) as 卖家名称 ,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = a.T_YSTBDGLJJRYX) as 卖家所属分公司 ,(select  isnull(I_YWGLBMFL,'') from AAA_DLZHXXB where B_DLYX = a.T_YSTBDDLYX) as 卖家部门分类  from   AAA_ZBDBXXB as a left join AAA_THDYFHDXXB as b on a.Number = b.ZBDBXXBBH   where   (a.Z_HTZT <> '中标' and a.Z_HTZT <> '未定标废标' )   group by a.Number,a.Z_SPMC,a.Z_SPBH,a.Z_GG,a.Z_HTBH ,a.Z_ZBSL,a.Z_ZBJG,a.Z_ZBJE,a.Z_HTJSRQ,a.Y_YSYDDGLJJRYX,a.T_YSTBDGLJJRYX,a.T_YSTBDDLYX,a.Y_YSYDDDLYX) as tab ";
    //}

    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = GetSql();
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件
        //ht_where["search_mainid"] = " F1Order ";  //所检索表的主键
        //ht_where["search_paixu"] = " asc ";  //排序方式
        //ht_where["search_paixuZD"] = " F1Order";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_HWSF/THD.aspx";
       
        /*---shiyan 2013-12-18 数据获取方式优化---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
        ht_where["serach_Row_str"] = " a.Number as 投标定标单号, a.Z_SPMC as 商品名称,a.Z_SPBH as 商品编号,a.Z_GG as 规格 ,a.Z_HTBH as 对应的电子购货合同编号,a.Z_ZBSL as 定标数量,a.Z_ZBJG as 定标价格,a.Z_ZBJE as 定标金额 ,'' as 无异议收货数量,'' as 有异议收货数量,  (case when a.Z_HTJSRQ is null then '--' else CONVERT(varchar(100), a.Z_HTJSRQ, 20) end) as 对应电子购货合同结束日期,c.I_JYFMC as 买家名称,c.I_PTGLJG as 买家所属分公司,c.I_YWGLBMFL as 买家部门分类,b.I_JYFMC as 卖家名称 ,b.I_PTGLJG as 卖家所属分公司, b.I_YWGLBMFL as 卖家部门分类 ,a.T_YSTBDDLYX as 卖方账号, a.Y_YSYDDDLYX as 买方账号";
        ht_where["search_tbname"] = " AAA_ZBDBXXB as a left join AAA_DLZHXXB as b on a.T_YSTBDDLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.Y_YSYDDDLYX=c.B_DLYX ";
        ht_where["search_str_where"] = "  a.Z_HTZT<>'中标' and a.Z_HTZT<>'未定标废标' ";  //检索条件
        ht_where["search_mainid"] = " a.Number ";  //所检索表的主键
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " a.Number";  //用于排序的字段
        
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
            object objYYYSH = new object();
            object objWYYSH = new object();
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                objYYYSH = DbHelperSQL.GetSingle("select isnull(sum(T_THSL),0)  from AAA_THDYFHDXXB where (F_DQZT='有异议收货' or F_DQZT='部分收货') and ZBDBXXBBH='" + NewDS.Tables[0].Rows[i]["投标定标单号"].ToString() + "'");
                if (objYYYSH != null && objYYYSH.ToString() != "")
                {
                    NewDS.Tables[0].Rows[i]["有异议收货数量"] = objYYYSH.ToString();
                }
                objWYYSH = DbHelperSQL.GetSingle("select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where (F_DQZT='无异议收货' or F_DQZT='默认无异议收货' or F_DQZT='补发货物无异议收货' or F_DQZT='有异议收货后无异议收货' ) and ZBDBXXBBH='" + NewDS.Tables[0].Rows[i]["投标定标单号"].ToString() + "'");
                if (objWYYSH != null && objWYYSH.ToString() != "")
                {
                    NewDS.Tables[0].Rows[i]["无异议收货数量"] = objWYYSH.ToString();
                }
            }
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
        //string sql_where = "  and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 对应的电子购货合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' ";
       // HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;
        
        /*---shiyan 2013-12-18 优化数据获取方式---*/
        //买家业务管理机构
        if (UCFWJGDetail1.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and c.I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString().Trim() + "' ";
        }
        if (UCFWJGDetail1.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and c.I_PTGLJG='" + UCFWJGDetail1.Value[1].ToString().Trim() + "' ";
        }
        //卖家业务管理机构
        if (UCFWJGDetail2.Value[0].ToString() != "")
        { 
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_YWGLBMFL='" + UCFWJGDetail2.Value[0].ToString().Trim() + "' ";
        }
        if (UCFWJGDetail2.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_PTGLJG='" + UCFWJGDetail2.Value[1].ToString().Trim() + "' ";
        }
        //买家名称
        if (txtBuyMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and c.I_JYFMC like '%" + txtBuyMC.Text.ToString().Trim() + "%' ";
        }
        //卖家名称
        if (txtSebMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_JYFMC like '%" + txtSebMC.Text.ToString().Trim() + "%' ";
        }
        ////商品名称
        //if (txtSPMC.Text.Trim() != "")
        //{
        //    HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.Z_SPMC like '%" + txtSPMC.Text.ToString().Trim() + "%' ";
        //}
        //合同编号
        if (txtDZGHHTBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.Z_HTBH like '%" + txtDZGHHTBH.Text.ToString().Trim() + "%' ";
        }
        //买方账号
        if (txtBuyzh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and  a.Y_YSYDDDLYX  like '%" + txtBuyzh.Text.ToString().Trim() + "%' ";
        }

        //卖方账号
        if (txtSelzh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and  a.T_YSTBDDLYX  like '%" + txtSelzh.Text.ToString().Trim() + "%' ";
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

    protected void btnDC_Click(object sender, EventArgs e)
    {

       // string sql = "select * from" + GetSql() + " where 买家部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%' and 卖家部门分类 like '%"+UCFWJGDetail2.Value[0].ToString().Trim()+"%' and 买家所属分公司 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 买家名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 对应的电子购货合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' and 卖家所属分公司 like '%" + UCFWJGDetail2.Value[1].ToString().Trim() + "%' and 卖家名称 like '%" + txtSebMC.Text.Trim() + "%'";

        string sql = "select a.Number as 投标定标单号, a.Z_SPMC as 商品名称,a.Z_SPBH as 商品编号,a.Z_GG as 规格 ,a.Z_HTBH as 对应的电子购货合同编号,a.Z_ZBSL as 定标数量,a.Z_ZBJG as 定标价格,a.Z_ZBJE as 定标金额 ,(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where (F_DQZT='无异议收货' or F_DQZT='默认无异议收货' or F_DQZT='补发货物无异议收货' or F_DQZT='有异议收货后无异议收货' ) and ZBDBXXBBH=a.Number) as 无异议收货数量,(select isnull(sum(T_THSL),0)  from AAA_THDYFHDXXB where (F_DQZT='有异议收货' or F_DQZT='部分收货') and ZBDBXXBBH=a.Number) as 有异议收货数量, (case when a.Z_HTJSRQ is null then '--' else CONVERT(varchar(100), a.Z_HTJSRQ, 20) end) as 对应电子购货合同结束日期, a.Y_YSYDDDLYX as 买方账号,c.I_JYFMC as 买方名称,c.I_PTGLJG as 买方管理部门,c.I_YWGLBMFL as 买家部门分类,a.T_YSTBDDLYX as 卖方账号,b.I_JYFMC as 卖方名称 ,b.I_PTGLJG as 卖方管理部门, b.I_YWGLBMFL as 卖家部门分类 from AAA_ZBDBXXB as a left join AAA_DLZHXXB as b on a.T_YSTBDDLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.Y_YSYDDDLYX=c.B_DLYX where" + ViewState["ht_where"].ToString() + " order by a.Number ASC";

        DataSet dtable = DbHelperSQL.Query(sql);

        if (dtable != null && dtable.Tables [0].Rows.Count > 0)
        {
            //dtable.Columns.Remove("F1Order");            
            dtable.Tables [0].Columns.Remove("投标定标单号");
            dtable.Tables[0].Columns.Remove("买家部门分类");
            dtable.Tables[0].Columns.Remove("卖家部门分类");
            // dtable.Columns["MB001"].ColumnName = "品号";   
            //dtable.Columns["批号"].SetOrdinal(4);
            //DataTableToExcel(dtable);
            MyXlsClass MXC = new MyXlsClass();
            MXC.goxls(dtable, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "货物收发概况", "货物收发概况", 15);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "没有可以导出的数据，请重新进行查询！");

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
        if (e.CommandArgument == "DZGHHT")//电子购货合同
        {
            string url = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/pingtaiservices/moban/FMPTDZGHHT.aspx?Number=" + e.CommandName.ToString();
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.open('" + url + "');</script>");
            
        }
       
    }
}