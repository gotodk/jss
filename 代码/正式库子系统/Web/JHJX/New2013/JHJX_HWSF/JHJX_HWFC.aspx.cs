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

public partial class Web_JHJX_New2013_JHJX_HWSF_JHJX_HWFC : System.Web.UI.Page
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
         
            DisGrid();

        }
    }
  
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = "(select b.Number 中标定标信息表编号,a.F_FHDSCSJ as  生成时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,'T'+a.Number as 对应提货单,b.Y_YSYDDDLYX '买方账号',(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买方名称,(select I_YWGLBMFL from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) '买方业务管理部门分类',(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) '买方所属分公司',b.T_YSTBDDLYX '卖方账号',(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖方名称,(select I_YWGLBMFL from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) '卖方业务管理部门分类',(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) '卖方所属分公司',a.F_WLGSMC as 物流公司名称,a.F_WLGSLXR as 物流公司联系人,a.F_WLGSDH as 物流联系电话,a.F_WLDH as 物流单编号,a.F_FPHM as 发票编号,(case  a.F_FPSFSHTH when '是' then '随货同行' else '另外邮寄' end ) as 发票发送方式 , F_FPSFSHTH, b.Z_HTBH as 对应电子购货合同编号,a.F_FPYJXXLRSJ as 发票邮寄信息录入时间,a.F_FPYJDWMC as 发票邮寄单位名称,a.F_FPYJDBH as 发票邮寄单编号   from AAA_THDYFHDXXB as a inner join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  where a.F_FHDSCSJ is not null  and a.F_DQZT = '已录入发货信息') as tab";
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件
        //ht_where["search_mainid"] = " 发货单号 ";  //所检索表的主键
        //ht_where["search_paixu"] = " asc ";  //排序方式
        //ht_where["search_paixuZD"] = " 生成时间 ";  //用于排序的字段

        /*---shiyan 2013-12-18 数据获取方式优化---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
        ht_where["serach_Row_str"] = " b.Number 中标定标信息表编号,a.F_FHDSCSJ as  生成时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,'T'+a.Number as 对应提货单,b.Y_YSYDDDLYX '买方账号',eb.I_JYFMC as 买方名称,eb.I_YWGLBMFL as 买方业务管理部门分类,eb.I_PTGLJG as 买方所属分公司,b.T_YSTBDDLYX as 卖方账号,es.I_JYFMC as 卖方名称,es.I_YWGLBMFL as  卖方业务管理部门分类,es.I_PTGLJG as 卖方所属分公司,a.F_WLGSMC as 物流公司名称,a.F_WLGSLXR as 物流公司联系人,a.F_WLGSDH as 物流联系电话,a.F_WLDH as 物流单编号,a.F_FPHM as 发票编号,(case  a.F_FPSFSHTH when '是' then '随货同行' else '另外邮寄' end ) as 发票发送方式 , F_FPSFSHTH, b.Z_HTBH as 对应电子购货合同编号,a.F_FPYJXXLRSJ as 发票邮寄信息录入时间,a.F_FPYJDWMC as 发票邮寄单位名称,a.F_FPYJDBH as 发票邮寄单编号 ";
        ht_where["search_tbname"] = " AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as eb on b.Y_YSYDDDLYX=eb.B_DLYX left join AAA_DLZHXXB as es on b.T_YSTBDDLYX=es.B_DLYX ";
        ht_where["search_str_where"] = " a.F_DQZT='已录入发货信息' and a.F_FHDSCSJ is not null ";  //检索条件
        ht_where["search_mainid"] = " a.Number ";  //所检索表的主键
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " a.F_FHDSCSJ ";  //用于排序的字段
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
            //DataSet ds = DbHelperSQL.Query("select b.Number 中标定标信息表编号,a.F_FHDSCSJ as  生成时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,'T'+a.Number as 对应提货单,b.Y_YSYDDDLYX '买方账号',(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买方名称,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买方名称,(select I_YWGLBMFL from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) '买方业务管理部门分类',(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) '买方所属分公司',b.T_YSTBDDLYX '卖方账号',(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖方名称,(select I_YWGLBMFL from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) '卖方业务管理部门分类',(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) '卖方所属分公司',a.F_WLGSMC as 物流公司名称,a.F_WLGSLXR as 物流公司联系人,a.F_WLGSDH as 物流联系电话,a.F_WLDH as 物流单编号,a.F_FPHM as 发票编号,(case  a.F_FPSFSHTH when '是' then '随货同行' else '另外邮寄' end ) as 发票发送方式 , F_FPSFSHTH, b.Z_HTBH as 对应电子购货合同编号,a.F_FPYJXXLRSJ as 发票邮寄信息录入时间,a.F_FPYJDWMC as 发票邮寄单位名称,a.F_FPYJDBH as 发票邮寄单编号   from AAA_THDYFHDXXB as a inner join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  where a.F_FHDSCSJ is not null  and a.F_DQZT = '已录入发货信息' and 1!=1");
            //Repeater1.DataSource = ds.Tables[0].DefaultView;

            Repeater1.DataSource = null;
            ts.Visible = true;  
        }
        Repeater1.DataBind();
     
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        //string sql_where = " and 买方业务管理部门分类 like '%" + this.UCFWJGDetail3.Value[0].ToString() + "%' and 买方所属分公司 like '%" + this.UCFWJGDetail3.Value[1].ToString() + "%' and 买方名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 对应电子购货合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' and 卖方业务管理部门分类 like '%" + this.UCFWJGDetail4.Value[0].ToString() + "%'  and 卖方所属分公司 like '%" + this.UCFWJGDetail4.Value[1].ToString() + "%' and 卖方名称 like '%" + txtSellerMC.Text.Trim() + "%' and 发货单号 like '%" + txtFHDBH.Text.Trim() + "%'";
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
        if (txtSellerMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and es.I_JYFMC like '%" + txtSellerMC.Text.ToString().Trim() + "%' ";
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
        if (txtFHDBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 'F'+a.Number like '%" + txtFHDBH.Text.ToString().Trim() + "%' ";
        }
        //买方账号
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
    
   
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandArgument == "DYDZGHHT")//电子购货合同
        { 
            string url = "http://"+ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString()+"/pingtaiservices/moban/FMPTDZGHHT.aspx?Number="+e.CommandName.ToString();
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.open('" + url + "');</script>");
        }
        else if (e.CommandArgument == "FHD")//提货单编号
        {
            Response.Redirect("FHD.aspx?Number=" + e.CommandName + "&GoBackUrl=JHJX_HWFC.aspx");
        }
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDC_Click(object sender, EventArgs e)
    {

        //string sql_where = " and 买方业务管理部门分类 like '%" + this.UCFWJGDetail3.Value[0].ToString() + "%' and 买方所属分公司 like '%" + this.UCFWJGDetail3.Value[1].ToString() + "%' and 买方名称 like '%" + txtBuyMC.Text.ToString().Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 对应电子购货合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' and 卖方业务管理部门分类 like '%" + this.UCFWJGDetail4.Value[0].ToString() + "%'  and 卖方所属分公司 like '%" + this.UCFWJGDetail4.Value[1].ToString() + "%' and 卖方名称 like '%" + txtSellerMC.Text.Trim() + "%' and 发货单号 like '%" + txtFHDBH.Text.Trim() + "%'";
       // string sql1 = "select * from (select b.Number 中标定标信息表编号,a.F_FHDSCSJ as  生成时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,'T'+a.Number as 对应提货单,b.Y_YSYDDDLYX '买方账号',(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买方名称,(select I_YWGLBMFL from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) '买方业务管理部门分类',(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) '买方所属分公司',b.T_YSTBDDLYX '卖方账号',(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖方名称,(select I_YWGLBMFL from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) '卖方业务管理部门分类',(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) '卖方所属分公司',a.F_WLGSMC as 物流公司名称,a.F_WLGSLXR as 物流公司联系人,a.F_WLGSDH as 物流联系电话,a.F_WLDH as 物流单编号,a.F_FPHM as 发票编号,(case  a.F_FPSFSHTH when '是' then '随货同行' else '另外邮寄' end ) as 发票发送方式 , F_FPSFSHTH, b.Z_HTBH as 对应电子购货合同编号,a.F_FPYJXXLRSJ as 发票邮寄信息录入时间,a.F_FPYJDWMC as 发票邮寄单位名称,a.F_FPYJDBH as 发票邮寄单编号   from AAA_THDYFHDXXB as a inner join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  where a.F_FHDSCSJ is not null  and a.F_DQZT = '已录入发货信息') as tab where 1=1 " + sql_where + " order by 生成时间 asc";

        string sql1 = "select b.Number 中标定标信息表编号,a.F_FHDSCSJ as  生成时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,'T'+a.Number as 对应提货单,b.Y_YSYDDDLYX '买方账号',eb.I_JYFMC as 买方名称,eb.I_YWGLBMFL as 买方业务管理部门分类,eb.I_PTGLJG as 买方管理部门,b.T_YSTBDDLYX as 卖方账号,es.I_JYFMC as 卖方名称,es.I_YWGLBMFL as  卖方业务管理部门分类,es.I_PTGLJG as 卖方管理部门,a.F_WLGSMC as 物流公司名称,a.F_WLGSLXR as 物流公司联系人,a.F_WLGSDH as 物流联系电话,a.F_WLDH as 物流单编号,a.F_FPHM as 发票编号,(case  a.F_FPSFSHTH when '是' then '随货同行' else '另外邮寄' end ) as 发票发送方式 , F_FPSFSHTH, b.Z_HTBH as 对应电子购货合同编号,a.F_FPYJXXLRSJ as 发票邮寄信息录入时间,a.F_FPYJDWMC as 发票邮寄单位名称,a.F_FPYJDBH as 发票邮寄单编号   from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as eb on b.Y_YSYDDDLYX=eb.B_DLYX left join AAA_DLZHXXB as es on b.T_YSTBDDLYX=es.B_DLYX  where " + ViewState["ht_where"].ToString() + " order by a.F_FHDSCSJ";
        DataSet NewDS = DbHelperSQL.Query(sql1);
        NewDS.Tables[0].Columns.Remove("中标定标信息表编号");
        NewDS.Tables[0].Columns.Remove("F_FPSFSHTH");
        NewDS.Tables[0].Columns.Remove("发票邮寄信息录入时间");
        NewDS.Tables[0].Columns.Remove("发票邮寄单位名称");
        NewDS.Tables[0].Columns.Remove("发票邮寄单编号");
        NewDS.Tables[0].Columns.Remove("买方业务管理部门分类");
        NewDS.Tables[0].Columns.Remove("卖方业务管理部门分类");

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(NewDS, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "货物发出", "货物发出", 15);
    }
}
