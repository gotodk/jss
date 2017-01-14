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

public partial class Web_JHJX_New2013_JHJX_HWSF_JHJX_HWQS : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail3.initdefault();
            UCFWJGDetail3.LableName = new string[] { "买方" };
            UCFWJGDetail4.initdefault();
            UCFWJGDetail4.LableName = new string[] { "卖方" };
            //setchengshi();
            DisGrid();

        }
    }
    //// 设定所属办事处下拉框内容
    //protected void setchengshi()
    //{
    //    //string MainERP = DbHelperSQL.GetSingle("SELECT DYERP FROM SYSTEM_CITY_0 WHERE NAME='公司总部'").ToString();//总部对应的ERP名称
    //    //DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处' and DYERP <>'" + MainERP + "'");
    //    DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处'");
    //    cityBuy.DataSource = ds;
    //    cityBuy.DataTextField = "DYFGSMC";
    //    cityBuy.DataValueField = "DYFGSMC";
    //    cityBuy.DataBind();
    //    cityBuy.Items.Insert(0, new ListItem("全部分公司", ""));

    //    citySel.DataSource = ds;
    //    citySel.DataTextField = "DYFGSMC";
    //    citySel.DataValueField = "DYFGSMC";
    //    citySel.DataBind();
    //    citySel.Items.Insert(0, new ListItem("全部分公司", ""));
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
    //            cityBuy.DataSource = null;
    //            cityBuy.DataBind();
    //            cityBuy.Items.Clear();
    //            cityBuy.Items.Add(fgs);

    //            citySel.DataSource = null;
    //            citySel.DataBind();
    //            citySel.Items.Clear();
    //            citySel.Items.Add(fgs);
    //        }
    //    }
    //}
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " (select b.Number 中标定标信息表编号,F_BUYWYYSHCZSJ as 签收时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 提货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 提货金额,a.T_THSL as 无异议签收数量 ,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买家名称,'买家所属管理部门'=(select  top 1 I_YWGLBMFL  from dbo.AAA_DLZHXXB where B_DLYX= b.Y_YSYDDDLYX),'买家所属分公司'=(select  top 1 I_PTGLJG  from dbo.AAA_DLZHXXB where B_DLYX= b.Y_YSYDDDLYX),(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖家名称,'卖家所属管理部门'=(select  top 1 I_YWGLBMFL  from dbo.AAA_DLZHXXB  where B_DLYX= b.T_YSTBDDLYX) ,'卖家所属分公司'=(select  top 1 I_PTGLJG  from dbo.AAA_DLZHXXB  where B_DLYX= b.T_YSTBDDLYX) ,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号,a.F_FPHM as 发票号码, b.Z_HTBH as 电子合同编号 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  where F_DQZT in ('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货')) as tab";
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件
        //ht_where["search_mainid"] = " 发货单号 ";  //所检索表的主键
        //ht_where["search_paixu"] = " asc ";  //排序方式
        //ht_where["search_paixuZD"] = " 发货单号";  //用于排序的字段
        
        /*---shiyan 2013-12-18 数据获取方式优化---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
        ht_where["serach_Row_str"] = " eb.B_DLYX as 买方账号,es.B_DLYX as 卖方账号, b.Number 中标定标信息表编号,F_BUYWYYSHCZSJ as 签收时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 提货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 提货金额,a.T_THSL as 无异议签收数量 ,eb.I_JYFMC as 买家名称,eb.I_YWGLBMFL as 买家所属管理部门,eb.I_PTGLJG as 买家所属分公司,es.I_JYFMC as 卖家名称,es.I_YWGLBMFL as '卖家所属管理部门',es.I_PTGLJG as 卖家所属分公司,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号,a.F_FPHM as 发票号码, b.Z_HTBH as 电子合同编号 ";
        ht_where["search_tbname"] = " AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as eb on b.Y_YSYDDDLYX=eb.B_DLYX left join AAA_DLZHXXB as es on b.T_YSTBDDLYX=es.B_DLYX ";
        ht_where["search_str_where"] = " (F_DQZT='无异议收货' or F_DQZT='默认无异议收货' or F_DQZT='补发货物无异议收货' or F_DQZT='有异议收货后无异议收货') ";  //检索条件
        ht_where["search_mainid"] = " a.Number ";  //所检索表的主键
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " a.Number";  //用于排序的字段

        ht_where["returnlastpage_open"] = "New2013/JHJX_HWSF/FHD.aspx";
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
           // DataSet ds = DbHelperSQL.Query("select '标定标信息表编号'='','签收时间'='','发货单号','商品名称'='','商品编号'='','规格'='','提货数量'='','定标价格'='','提货金额'='','无异议签收数量'='' ,'买家名称'='','买家所属分公司'='','卖家名称'='','卖家所属分公司'='','物流公司名称'='','物流单号'='','发票号码'='', '电子合同编号'='' from AAA_THDYFHDXXB where 1!=1");
           // Repeater1.DataSource = ds.Tables[0].DefaultView;
            Repeater1.DataSource = null;
            ts.Visible = true;

        }
        Repeater1.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
       // string sql_where = " and 买家所属管理部门 like '%" + UCFWJGDetail3.Value[0].ToString().Trim() + "%' and 买家所属分公司 like '%" + UCFWJGDetail3.Value[1].ToString().Trim() + "%' and 买家名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 电子合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' and 卖家所属管理部门 like '%" + UCFWJGDetail4.Value[0].ToString().Trim() + "%' and 卖家所属分公司 like '%" + UCFWJGDetail4.Value[1].ToString().Trim() + "%' and 卖家名称 like '%" + txtSelMC.Text.Trim() + "%' and 发货单号 like '%" + txtTHDBH.Text.Trim() + "%'";
        //HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;
       
        /*---shiyan 2013-12-18 优化数据获取方式---*/
        //买家业务管理机构
        if (UCFWJGDetail3.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and eb.I_YWGLBMFL='" + UCFWJGDetail3.Value[0].ToString().Trim() + "' ";
        }
        if (UCFWJGDetail3.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and eb.I_PTGLJG='" + UCFWJGDetail3.Value[1].ToString().Trim() + "' ";
        }
        //卖家业务管理机构
        if (UCFWJGDetail4.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.I_YWGLBMFL='" + UCFWJGDetail4.Value[0].ToString().Trim() + "' ";
        }
        if (UCFWJGDetail4.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.I_PTGLJG='" + UCFWJGDetail4.Value[1].ToString().Trim() + "' ";
        }
        //买家名称
        if (txtBuyMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and eb.I_JYFMC like '%" + txtBuyMC.Text.ToString().Trim() + "%' ";
        }
        //卖家名称
        if (txtSelMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.I_JYFMC like '%" + txtSelMC.Text.ToString().Trim() + "%' ";
        }
        //商品名称
        if (txtSPMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.Z_SPMC like '%" + txtSPMC.Text.ToString().Trim() + "%' ";
        }
        //电子购货合同编号
        if (txtDZGHHTBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.Z_HTBH like '%" + txtDZGHHTBH.Text.ToString().Trim() + "%' ";
        }
        //发货单编号
        if (txtTHDBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 'F'+a.Number like '%" + txtTHDBH.Text.ToString().Trim() + "%' ";
        }
        //买方账号
        if (txtBuyzh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and eb.B_DLYX like '%" + txtBuyzh.Text.ToString().Trim() + "%' ";
        }
        //卖方账号es.B_DLYX
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
    protected void btnDC_Click(object sender, EventArgs e)
    {
        //string str = "select * from (select b.Number 中标定标信息表编号,F_BUYWYYSHCZSJ as 签收时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 提货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 提货金额,a.T_THSL as 无异议签收数量 ,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买家名称,'买家所属管理部门'=(select  top 1 I_YWGLBMFL  from dbo.AAA_DLZHXXB where B_DLYX= b.Y_YSYDDDLYX),'买家所属分公司'=(select  top 1 I_PTGLJG  from dbo.AAA_DLZHXXB where B_DLYX= b.Y_YSYDDDLYX),(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖家名称,'卖家所属管理部门'=(select  top 1 I_YWGLBMFL  from dbo.AAA_DLZHXXB  where B_DLYX= b.T_YSTBDDLYX) ,'卖家所属分公司'=(select  top 1 I_PTGLJG  from dbo.AAA_DLZHXXB  where B_DLYX= b.T_YSTBDDLYX) ,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号,a.F_FPHM as 发票号码, b.Z_HTBH as 电子合同编号 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  where F_DQZT in ('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货')) as tab ";
        //str += "where 买家所属管理部门 like '%" + UCFWJGDetail3.Value[0].ToString().Trim() + "%' and 买家所属分公司 like '%" + UCFWJGDetail3.Value[1].ToString().Trim() + "%' and 买家名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 电子合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' and 卖家所属管理部门 like '%" + UCFWJGDetail4.Value[0].ToString().Trim() + "%' and 卖家所属分公司 like '%" + UCFWJGDetail4.Value[1].ToString().Trim() + "%' and 卖家名称 like '%" + txtSelMC.Text.Trim() + "%' and 发货单号 like '%" + txtTHDBH.Text.Trim() + "%'";

        string str = "select F_BUYWYYSHCZSJ as 签收时间,'F'+a.Number as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,b.Number 中标定标信息表编号,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,a.T_THSL as 无异议签收数量 ,eb.B_DLYX as 买方账号, eb.I_JYFMC as 买方名称,eb.I_YWGLBMFL as 买家所属管理部门,eb.I_PTGLJG as 买方管理部门,es.B_DLYX as 卖方账号, es.I_JYFMC as 卖方名称,es.I_YWGLBMFL as 卖家所属管理部门,es.I_PTGLJG as 卖方管理部门,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号,a.F_FPHM as 发票编号, b.Z_HTBH as 对应电子购货合同编号 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as eb on b.Y_YSYDDDLYX=eb.B_DLYX left join AAA_DLZHXXB as es on b.T_YSTBDDLYX=es.B_DLYX  where " + ViewState["ht_where"].ToString() + " order by a.Number ASC";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].Columns.Remove("中标定标信息表编号");
            ds.Tables[0].Columns.Remove("卖家所属管理部门");
            ds.Tables[0].Columns.Remove("买家所属管理部门");           
            MyXlsClass MXC = new MyXlsClass();
            MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "货物签收", "货物签收", 15);
            //Repeater2.DataSource = ds.Tables[0].DefaultView;
            //exprestTD.Visible = false;            
            //Repeater2.DataBind();
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "没有数据需要导出！");
            //exprestTD.Visible = true;
        }        
       // ToExcel(export);
    }
    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.Charset = "";
        string filename = "JYPT_HWQS" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
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
        if (e.CommandArgument == "DZGHHT")//电子购货合同
        {
            string url = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/pingtaiservices/moban/FMPTDZGHHT.aspx?Number=" + e.CommandName.ToString();
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.open('" + url + "');</script>");
        }
        else if (e.CommandArgument == "FHD")//提货单编号
        {
            Response.Redirect("FHD.aspx?Number=" + e.CommandName + "&GoBackUrl=JHJX_HWQS.aspx");
        }
    }
}