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
using System.Configuration;

public partial class Web_JHJX_JHJX_BZHCX : System.Web.UI.Page
{
    protected static  DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            DisGrid();
        }
    }

         //绑定事件
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

            rptSPXX.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rptSPXX.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " ( SELECT 'BH'+ AAA_ZBDBXXB.Number AS 保证函编号,ISNULL(SELLER.I_JYFMC,'') AS 卖方名称,ISNULL(BUYER.I_JYFMC,'') AS 买方名称,Z_HTBH AS 合同编号,CASE WHEN Z_QPZT='清盘结束' THEN '是' ELSE '否' END AS 是否失效,Z_DBSJ AS 开具时间,Z_BZHJE AS 金额,Z_SPMC AS 商品名称,T_YSTBDDLYX AS 卖方邮箱 ,Y_YSYDDDLYX AS 买方邮箱 FROM  AAA_ZBDBXXB  LEFT JOIN (SELECT  B_DLYX, B_YHM,I_JYFMC  FROM AAA_DLZHXXB) SELLER ON T_YSTBDDLYX=SELLER.B_DLYX  LEFT JOIN   (SELECT  B_DLYX, B_YHM,I_JYFMC  FROM AAA_DLZHXXB) BUYER  ON  Y_YSYDDDLYX =BUYER.B_DLYX  WHERE Z_HTZT NOT IN ('中标','未定标废标')  ) tab1 ";  //检索的表
       // ht_where["search_mainid"] = " 保证函编号 ";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件  
       // ht_where["search_paixu"] = " DESC ";  //排序方式
        //ht_where["search_paixuZD"] = " 开具时间 ";  //用于排序的字段

        /*---shiyan 2013-12-17 数据获取优化---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
        ht_where["serach_Row_str"] = " 'BH'+ AAA_ZBDBXXB.Number AS 保证函编号,SELLER.I_JYFMC AS 卖方名称,BUYER.I_JYFMC AS 买方名称,Z_HTBH AS 合同编号,CASE WHEN Z_QPZT='清盘结束' THEN '是' ELSE '否' END AS 是否失效,Z_DBSJ AS 开具时间,Z_BZHJE AS 金额,Z_SPMC AS 商品名称,T_YSTBDDLYX AS 卖方邮箱 ,Y_YSYDDDLYX AS 买方邮箱 ";
        ht_where["search_tbname"] = " AAA_ZBDBXXB LEFT JOIN (SELECT B_DLYX,I_JYFMC FROM AAA_DLZHXXB) SELLER ON T_YSTBDDLYX=SELLER.B_DLYX LEFT JOIN (SELECT B_DLYX, I_JYFMC  FROM AAA_DLZHXXB) BUYER ON Y_YSYDDDLYX =BUYER.B_DLYX ";  //检索的表
        ht_where["search_mainid"] = " AAA_ZBDBXXB.Number ";  //所检索表的主键
        ht_where["search_str_where"] = " Z_HTZT NOT IN ('中标','未定标废标') ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " Z_DBSJ ";  //用于排序的字段

        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        //HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and 是否失效 like '%" + ddlsfyx.SelectedValue.ToString() + "%' and 卖方名称 like '%" + txtseller.Text.Trim() + "%' and 买方名称 like '%" + txtbuyer.Text.Trim() + "%'  and 合同编号 like '%" + txthtbh.Text.Trim() + "%'";

        /*---shiyan 2013-12-17 数据获取优化---*/
        if (ddlsfyx.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and (CASE WHEN Z_QPZT='清盘结束' THEN '是' ELSE '否' END)='" + ddlsfyx.SelectedValue.ToString() + "' ";
        }
        if (txtseller.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and SELLER.I_JYFMC like '%" + txtseller.Text.Trim() + "%' ";
        }
        if (txtbuyer.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and BUYER.I_JYFMC like '%" + txtbuyer.Text.Trim() + "%' ";
        }
        if (txthtbh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and Z_HTBH like '%" + txthtbh.Text.Trim() + "%' ";
        }
        /*---shiyan 结束---*/

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    protected void rptSPXX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {        
     
        if (e.CommandName == "lck")
        {
            string str = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/pingtaiservices/mobanrun/BZH.aspx?moban=BZH.htm&Number=" + e.CommandArgument.ToString().Trim().Replace("BH","");            
          
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", " <script type='text/JavaScript'>window.open('" + str + "'); </script>");

        }
    }

    protected void rptSPXX_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
       Label lbspmc = (Label)e.Item.FindControl("lbspmc");
       Label lbseller = (Label)e.Item.FindControl("lbseller");
       Label lbbuyer = (Label)e.Item.FindControl("lbbuyer");

       if (lbspmc.Text.Length > 10)
           lbspmc.Text = lbspmc.Text.Substring(0, 10) + "...";
       if (lbseller.Text.Length > 10)
           lbseller.Text = lbseller.Text.Substring(0, 10) + "...";
       if (lbbuyer.Text.Length > 10)
           lbbuyer.Text = lbbuyer.Text.Substring(0, 10) + "...";
    
    }

}