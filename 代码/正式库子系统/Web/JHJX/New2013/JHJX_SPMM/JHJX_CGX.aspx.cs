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

public partial class Web_JHJX_New2013_JHJX_SPMM_JHJX_CGX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();
           
            DisGrid();

        }
    }

    protected string GetSql()
    {
        //return "((select '投标单' as  '单据类型' ,  'T'+a.Number as Number,a.Number as 单据编号,a.HTQX as '合同期限',a.SPBH as  '商品编号',b.SPMC as '商品名称',b.GG as '规格',TBNSL as '数量',TBJG as '价格',TBJE as '金额',a.CreateTime as CreateTime,(select I_JYFMC from AAA_DLZHXXB where B_DLYX =a.DLYX )as 交易方名称,a.DLYX as 交易方账号, (select I_PTGLJG from AAA_DLZHXXB where B_DLYX =(select GLJJRDLZH from AAA_MJMJJYZHYJJRZHGLB where  SFDQMRJJR='是' and DLYX=a.DLYX ) ) as 所属分公司  from AAA_TBDCGB   as a left join AAA_PTSPXXB as b on a.SPBH = b.SPBH where  b.SFYX='是' ) union  (select '预订单' as  '单据类型' ,  'Y'+a.Number as Number,a.Number as 单据编号,a.HTQX as '合同期限',a.SPBH as  '商品编号',b.SPMC as '商品名称',b.GG as '规格',NDGSL as '数量',NMRJG as '价格',NDGJE as '金额',a.CreateTime as CreateTime ,(select I_JYFMC from AAA_DLZHXXB where B_DLYX =a.DLYX) as 交易方名称,a.DLYX as 交易方账号,  (select I_PTGLJG from AAA_DLZHXXB where B_DLYX =(select GLJJRDLZH from AAA_MJMJJYZHYJJRZHGLB where  SFDQMRJJR='是' and DLYX=a.DLYX ) ) as 所属分公司 from AAA_YDDCGB   as a left join AAA_PTSPXXB as b on a.SPBH = b.SPBH where  b.SFYX='是' )) as tab ";

        return "((select '投标单' as  '单据类型' ,  'T'+a.Number as Number,a.Number as 单据编号,a.HTQX as '合同期限',a.SPBH as  '商品编号',b.SPMC as '商品名称',b.GG as '规格',TBNSL as '数量',TBJG as '价格',TBJE as '金额',a.CreateTime as 创建时间,isnull([I_YWGLBMFL],'') as 部门分类,I_JYFMC as 交易方名称,a.DLYX as 交易方账号, isnull(I_PTGLJG,'') as 所属管理部门  from AAA_TBDCGB   as a left join AAA_PTSPXXB as b on a.SPBH = b.SPBH left join AAA_DLZHXXB as c on c.B_DLYX = a.DLYX where  b.SFYX='是' ) union  (select '预订单' as  '单据类型' ,  'Y'+a.Number as Number,a.Number as 单据编号,a.HTQX as '合同期限',a.SPBH as  '商品编号',b.SPMC as '商品名称',b.GG as '规格',NDGSL as '数量',NMRJG as '价格',NDGJE as '金额',a.CreateTime as CreateTime , isnull([I_YWGLBMFL],'') as 部门分类, I_JYFMC  as 交易方名称,a.DLYX as 交易方账号,   isnull(I_PTGLJG,'') as 所属管理部门 from AAA_YDDCGB   as a left join AAA_PTSPXXB as b on a.SPBH = b.SPBH left join AAA_DLZHXXB as c on c.B_DLYX = a.DLYX where  b.SFYX='是' )) as tab";
    }

    // 设定所属办事处下拉框内容
    //protected void setchengshi()
    //{

    //    DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处'");
    //    city.DataSource = ds;
    //    city.DataTextField = "DYFGSMC";
    //    city.DataValueField = "DYFGSMC";
    //    city.DataBind();
    //    city.Items.Insert(0, new ListItem("全部分公司", ""));
       
    //    string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
    //    string bm = DbHelperSQL.GetSingle(sql).ToString();
    //    if (bm.IndexOf("办事处") > 0)
    //    {
    //        sql = "select count(name) from system_city where name='" + bm + "'";
    //        //bm = bm.Replace("办事处", "分公司");
    //        if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
    //        {
    //            string strfgs = "select DYFGSMC FROM System_City where Name='" + bm + "'";
    //            string fgs = DbHelperSQL.GetSingle(strfgs).ToString();
    //            city.DataSource = null;
    //            city.DataBind();
    //            city.Items.Clear();
    //            city.Items.Add(fgs);

               
    //        }
    //    }
    //}


    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = GetSql();
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件
        //ht_where["search_mainid"] = " Number ";  //所检索表的主键
        //ht_where["search_paixu"] = " asc ";  //排序方式
        //ht_where["search_paixuZD"] = "创建时间";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_HWSF/FHD.aspx";

        /*---shiyan 2013-12-16 进行数据获取优化。---*/       
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是
        ht_where["serach_Row_str"] = " tab.单据类型, tab.单据编号,tab.合同期限,tab.商品编号,b.SPMC as 商品名称,b.GG as 规格,tab.数量,tab.价格,tab.金额,tab.创建时间,c.I_JYFMC as 交易方名称,tab.交易方账号, c.I_PTGLJG as 所属管理部门,c.I_YWGLBMFL as 部门分类 ";
        ht_where["search_tbname"] = " (select '投标单' as  '单据类型' ,Number as 单据编号,HTQX as '合同期限',SPBH as  '商品编号',TBNSL as '数量',TBJG as '价格',TBJE as '金额',CreateTime as 创建时间,DLYX as 交易方账号  from AAA_TBDCGB union all select '预订单' as  '单据类型',Number as 单据编号,HTQX as '合同期限',SPBH as  '商品编号',NDGSL as '数量',NMRJG as '价格',NDGJE as '金额',CreateTime as 创建时间,DLYX as 交易方账号 from AAA_YDDCGB) as tab left join AAA_PTSPXXB as b on tab.商品编号=b.SPBH left join AAA_DLZHXXB as c on tab.交易方账号=c.B_DLYX ";
        ht_where["search_str_where"] = " b.SFYX='是' ";  //检索条件
        ht_where["search_mainid"] = " 单据类型+单据编号 ";  //所检索表的主键
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " tab.创建时间 ";  //用于排序的字段
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
        //string sql_where = " and 部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%' and 所属管理部门 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%'  and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 单据类型 like '%" + ddldjlb.SelectedValue.Trim() + "%'";
        // HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;
       
        /*---shiyan增加 2013-12-16 进行数据获取优化。---*/
        if (UCFWJGDetail1.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString() + "' ";
        }
        if (UCFWJGDetail1.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and  I_PTGLJG='" + UCFWJGDetail1.Value[1].ToString() + "' ";
        }
        if (ddldjlb.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and tab.单据类别='" + this.ddldjlb.SelectedValue.ToString() + "' ";
        }
        if (txtjyfmc.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and c.I_JYFMC like '%" + this.txtjyfmc.Text.Trim() + "%' ";
        }
        if (txtjyfzh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and tab.交易方账户 like '%" + this.txtjyfmc.Text.Trim() + "%' ";
        }
        if (txtSPMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.商品名称 like '%" + this.txtSPMC.Text.Trim() + "%' ";
        }

        ViewState["ht_where"] = HTwhere["search_str_where"];
        
        /*---shiyan增加结束---*/
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }


    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    protected void btnToExcel_Click(object sender, EventArgs e)
    {

        //string sql = "select * from" + GetSql() + " where 部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%' and 所属管理部门 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%'   and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 单据类型 like '%" + ddldjlb.SelectedValue.Trim() + "%'";

        string sql = "select 单据类型+单据编号 as Number,tab.单据类型, tab.单据编号,tab.合同期限,tab.商品编号,b.SPMC as 商品名称,b.GG as 规格,tab.数量,tab.价格,tab.金额,tab.创建时间,c.I_JYFMC as 交易方名称,tab.交易方账号, c.I_PTGLJG as 所属管理部门,c.I_YWGLBMFL as 部门分类 from (select '投标单' as  '单据类型' ,Number as 单据编号,HTQX as '合同期限',SPBH as  '商品编号',TBNSL as '数量',TBJG as '价格',TBJE as '金额',CreateTime as 创建时间,DLYX as 交易方账号  from AAA_TBDCGB union all select '预订单' as  '单据类型',Number as 单据编号,HTQX as '合同期限',SPBH as  '商品编号',NDGSL as '数量',NMRJG as '价格',NDGJE as '金额',CreateTime as 创建时间,DLYX as 交易方账号 from AAA_YDDCGB) as tab left join AAA_PTSPXXB as b on tab.商品编号=b.SPBH left join AAA_DLZHXXB as c on tab.交易方账号=c.B_DLYX where "+ ViewState ["ht_where"].ToString ()+" order by tab.创建时间";

       DataSet dataSet=DbHelperSQL.Query(sql);
        DataTable dtable = dataSet.Tables[0];

        if (dtable != null && dtable.Rows.Count > 0)
        {
            dtable.Columns.Remove("Number");
            dtable.Columns.Remove("部门分类");
            dtable.Columns.Remove("合同期限");

            // dtable.Columns["MB001"].ColumnName = "品号";   
            
            dtable.Columns["交易方名称"].SetOrdinal(0);
            dtable.Columns["所属管理部门"].SetOrdinal(1);
            dtable.Columns["交易方账号"].SetOrdinal(2);
            dtable.Columns["单据类型"].SetOrdinal(3);

           // DataTableToExcel(dtable);

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "草稿一览表", "草稿一览表", 15);
        }
        else
        {
            MessageBox.Show(this, "没有可以导出的数据，请重新进行查询！");

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
        //if (e.CommandArgument == "CKXQ")
        //{

        //    Response.Redirect("CGXXQ.aspx?Number=" + e.CommandName + "&GoBackUrl=JHJX_WTYCL.aspx");

        //}
       

    }
}