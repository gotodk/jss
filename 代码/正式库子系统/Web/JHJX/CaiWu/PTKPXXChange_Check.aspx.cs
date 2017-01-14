using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Hesion.Brick.Core;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_PTKPXXChange_Check : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        commonpagernew1.OnNeedLoadData_all += new Web_pagerdemo_commonpagernew.OnNeedDataHandler_all(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
 
            UCFWJGDetail1.initdefault();
            DisGrid();
        }
    }  


    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, Hashtable ALLconfig)
    {
       
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            DataTable dt = NewDS.Tables[0];
            rptKPXX.DataSource = dt.DefaultView;

            Hashtable htTiaoJian = (Hashtable)ALLconfig["returnlastpage_spHT"];
            UCFWJGDetail1.Value = new string[] { htTiaoJian["管理机构分类"].ToString(), htTiaoJian["平台管理机构"].ToString() };
            ddlFPLX.SelectedValue = htTiaoJian["发票类别"].ToString();
            ddlCLZT.SelectedValue = htTiaoJian["处理状态"].ToString();
            txtKHBH.Text = htTiaoJian["客户编号"].ToString();
            txtDWMC.Text = htTiaoJian["单位名称"].ToString();
        }
        else { tdEmpty.Visible = true; }
        rptKPXX.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " a.*,b.* ";
        ht_where["search_tbname"] = " AAA_PTKPXX_BGSQ as a left join AAA_PTKPXXB as b on a.parentnumber=b.number left join AAA_DLZHXXB as c on b.khbh=c.I_ZQZJZH ";  //检索的表
        ht_where["search_mainid"] = " b.id ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " sqtjsj ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "PTKPXXChange_Checkinfo";

        Hashtable returnlastpage_spHT = new Hashtable();
        returnlastpage_spHT["管理机构分类"] = UCFWJGDetail1.Value[0].ToString();
        returnlastpage_spHT["平台管理机构"] = UCFWJGDetail1.Value[1].ToString();
        returnlastpage_spHT["发票类别"] = ddlFPLX.SelectedValue.ToString();
        returnlastpage_spHT["处理状态"] = ddlCLZT.SelectedValue.ToString();
        returnlastpage_spHT["客户编号"] = txtKHBH.Text.Trim();
        returnlastpage_spHT["单位名称"] = txtDWMC.Text.Trim();

        ht_where["returnlastpage_spHT"] = returnlastpage_spHT; //自动返回之前页数功能额度参数，可以设置特殊返回值，用于处理页面中条件的显示等。

        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        if (UCFWJGDetail1.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and ptgljg='" + UCFWJGDetail1.Value[1].ToString() + "'";
        }
        else if(UCFWJGDetail1 .Value [0].ToString ()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and c.I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString() + "'";
        }
        if (ddlFPLX.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and fplx='" + ddlFPLX.SelectedValue.ToString() + "' ";
        }
        if (ddlCLZT.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and clzt='" + ddlCLZT.SelectedValue.ToString() + "' ";
        }
        if (txtKHBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and khbh like '%" + txtKHBH.Text.Trim() + "%'";
        }
        if (txtDWMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and dwmc like '%" + txtDWMC.Text.Trim() + "%'";
        }
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    protected void rptKPXX_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
       
        Label lblDWMC = (Label)e.Item.FindControl("lblDWMC");
        Label lblCLZT = (Label)e.Item.FindControl("lblCLZT");
       
        if (lblDWMC.Text.Length > 10)
            lblDWMC.Text = lblDWMC.Text.Substring(0, 10) + "...";
        if (lblCLZT.Text.Trim() != "待处理")
        {
            LinkButton lb = (LinkButton)e.Item.FindControl("lkbtnCheck");
            lb.Text = "查看详情";
        }
    }

    protected void rptKPXX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        if (e.CommandName == "linkbj")
        {
            Response.Redirect("PTKPXXChange_Checkinfo.aspx?id=" + e.CommandArgument.ToString().Trim());
        }
    }
}