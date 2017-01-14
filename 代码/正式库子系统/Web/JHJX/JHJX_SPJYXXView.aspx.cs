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

public partial class Web_JHJX_JHJX_SPJYXXView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);

        if (!IsPostBack)
        {
           

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
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select ROW_NUMBER() over(partition by SPBH,JYSX order by zclsort desc,createtime) as rowNum,* from (select a.number,a.spbh,c.SPMC,c.GG,c.SPMS,a.JYSX ,a.jynr,a.jyryhm ,a.zccs+1 as 支持次数,参与总数,CONVERT(varchar(10), ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,参与总数)*100.00,2))+'%' as ZCL,(case a.jysx when '商品俗称' then 1 when '商品描述' then 2 when '验收标准' then 3 when '质量标准' then 4 end) as sort,ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,参与总数)*100.00,2) as zclsort,a.createtime from AAA_SPXXJYB as a left join (select spbh,jysx,SUM(ZCCS+1) as 参与总数 from AAA_SPXXJYB group by SPBH,jysx) as b on a.spbh=b.spbh and a.jysx=b.jysx left join AAA_PTSPXXB as c on a.SPBH=c.SPBH) as tab) as tab2 ";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 and rowNum<=5 ";  //检索条件  
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " sort,SPBH,rownum ";  //用于排序的字段      

        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        if (ddlJYSX.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and JYSX='" + ddlJYSX.SelectedValue.ToString() + "'";
        }       
        if (txtSPBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and SPBH like '%" + txtSPBH.Text.Trim() + "%'";
        }
        if (txtSPMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and SPMC like '%" + txtSPMC.Text.Trim() + "%'";
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
        Label lblSPMC = (Label)e.Item.FindControl("lblSPMC");
        if (lblSPMC.Text.Length > 10)
            lblSPMC.Text = lblSPMC.Text.Substring(0, 10) + "...";

        Label lblSPMS = (Label)e.Item.FindControl("lblSPMS");
        if (lblSPMS.Text.Length > 10)
            lblSPMS.Text = lblSPMS.Text.Substring(0, 10) + "...";

        Label lblGG = (Label)e.Item.FindControl("lblGG");
        if (lblGG.Text.Length > 10)
            lblGG.Text = lblGG.Text.Substring(0, 10) + "...";

        Label lblJYNR = (Label)e.Item.FindControl("lblJYNR");
        if (lblJYNR.Text.Length >20)
            lblJYNR.Text = lblJYNR.Text.Substring(0, 20) + "...";

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string sql = "select rownum as 序号, spbh as 商品编号,spmc as 商品名称,gg as 规格型号,spms as 商品描述,jysx as 建议事项,jynr as 建议内容,zcl as 支持率 from (select ROW_NUMBER() over(partition by SPBH,JYSX order by zclsort desc,createtime) as rowNum,* from (select a.number,a.spbh,c.SPMC,c.GG,c.SPMS,a.JYSX ,a.jynr,a.jyryhm ,a.zccs+1 as 支持次数,参与总数,CONVERT(varchar(10), ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,参与总数)*100.00,2))+'%' as ZCL,(case a.jysx when '商品俗称' then 1 when '商品描述' then 2 when '验收标准' then 3 when '质量标准' then 4 end) as sort,ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,参与总数)*100.00,2) as zclsort,a.createtime  from AAA_SPXXJYB as a left join (select spbh,jysx,SUM(ZCCS+1) as 参与总数 from AAA_SPXXJYB group by SPBH,jysx) as b on a.spbh=b.spbh and a.jysx=b.jysx left join AAA_PTSPXXB as c on a.SPBH=c.SPBH) as tab) as tab2  where  " + ViewState["where"] + " order by sort,spbh, rownum ASC";
        DataSet ds = DbHelperSQL.Query(sql);

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "商品建议信息表", "商品建议信息表", 25);
    }
}