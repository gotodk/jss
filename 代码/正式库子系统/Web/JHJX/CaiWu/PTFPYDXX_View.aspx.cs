using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
using System.Collections;

public partial class Web_JHJX_CaiWu_PTFPYDXX_View : System.Web.UI.Page
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
   


    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERPinfo)
    {

        //if (ERRinfo.IndexOf("超时") >= 0)
        //{
        //    MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        //}
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            DataTable dt = NewDS.Tables[0];
            rptKPXX.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rptKPXX.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        //获取已经录入过的
        Hashtable ht_where = new Hashtable();

        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.        
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " a.*,fpdb+'_'+fpdh as 发票单号,sjr+'/'+sjrdh as SJRJDH  ";
        ht_where["search_tbname"] = " AAA_PTFPYDXXB as a left join AAA_DLZHXXB as b on a.KHBH=b.I_ZQZJZH  ";  //检索的表
        ht_where["search_mainid"] = " a.Number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 and a.shzt='审核通过' ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " a.createtime ";  //用于排序的字段      

        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        if (UCFWJGDetail1.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and PTGLJG='" + UCFWJGDetail1.Value[1].ToString() + "'";
        }
        else if (UCFWJGDetail1.Value[0].ToString()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString() + "'";
        }
        if (ddlFPLX.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and FPLX='" + ddlFPLX.SelectedValue.ToString() + "' ";
        }
        if (txtKHBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and KHBH like '%" + txtKHBH.Text.Trim() + "%'";
        }
        if (txtDWMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and KHMC like '%" + txtDWMC.Text.Trim() + "%'";
        }       
        commonpagernew1.HTwhere = HTwhere;
        ViewState["where"] = HTwhere["search_str_where"];
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    protected void rptKPXX_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblDWMC = (Label)e.Item.FindControl("lblDWMC");
        if (lblDWMC.Text.Length > 10)
            lblDWMC.Text = lblDWMC.Text.Substring(0, 10) + "...";

        Label lblSJFDWMC = (Label)e.Item.FindControl("lblSJFDWMC");
        if (lblSJFDWMC.Text.Length > 7)
            lblSJFDWMC.Text = lblSJFDWMC.Text.Substring(0, 7) + "...";

        Label lblSJDZ = (Label)e.Item.FindControl("lblSJDZ");
        if (lblSJDZ.Text.Length > 7)
            lblSJDZ.Text = lblSJDZ.Text.Substring(0, 7) + "...";

        Label lblSJRJDH = (Label)e.Item.FindControl("lblSJRJDH");
        if (lblSJRJDH.Text.Length > 7)
            lblSJRJDH.Text = lblSJRJDH.Text.Substring(0, 7) + "...";

        Label lblWLGSMC = (Label)e.Item.FindControl("lblWLGSMC");
        if (lblWLGSMC.Text.Length > 7)
            lblWLGSMC.Text = lblWLGSMC.Text.Substring(0, 7) + "...";

        Label lblWLDH = (Label)e.Item.FindControl("lblWLDH");
        if (lblWLDH.Text.Length > 10)
            lblWLDH.Text = lblWLDH.Text.Substring(0, 10) + "...";  

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string sql = "select fpdb+'_'+fpdh as 发票单号, ptgljg as 平台管理机构,khbh as 客户编号,khmc as 客户全称,fplx as 发票类型,cast(fpje as numeric(18,2)) as 发票金额,fph as 发票号,sjfdwmc as 收件单位名称,sjdz as 收件地址,sjr as 收件人,sjrdh as 收件人电话,wlgsmc as 物流公司名称,wldh as 物流单号,shr as 审核人,shsj as 审核时间 from AAA_PTFPYDXXB as a left join AAA_DLZHXXB as b on a.KHBH=b.I_ZQZJZH where  " + ViewState["where"] + " order by fpdb,fpdh desc";
        DataSet ds = DbHelperSQL.Query(sql);

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "平台发票邮递信息表", "平台发票邮递信息表", 25);
    }
}