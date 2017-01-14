using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Collections;

public partial class Web_JHJX_PTKPXX_View : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        //commonpagernew1.OnNeedLoadData_all += new Web_pagerdemo_commonpagernew.OnNeedDataHandler_all(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
 
            UCFWJGDetail1.initdefault();
            DisGrid();
        }
    }
   


    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERPinfo)
    {

        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                if (NewDS.Tables[0].Rows[i]["fplx"].ToString() == "增值税专用发票")
                {
                    NewDS.Tables[0].Rows[i]["zpxx"] = "纳税人识别号：" + NewDS.Tables[0].Rows[i]["ybnsrsbh"].ToString() + "\n单位地址：" + NewDS.Tables[0].Rows[i]["dwdz"].ToString()+"\n联系电话："+NewDS .Tables [0].Rows [i]["lxdh"].ToString ()+"\n开户行："+NewDS .Tables [0].Rows [i]["khh"].ToString ()+"\n开户账号："+NewDS .Tables [0].Rows [i]["khzh"].ToString ();
                }
                NewDS.Tables[0].Rows[i]["ydxx"] = "收件单位名称：" + NewDS.Tables[0].Rows[i]["fpjsdwmc"].ToString() + "\n收件单位地址：" + NewDS.Tables[0].Rows[i]["fpjsdz"].ToString() + "\n收件人姓名：" + NewDS.Tables[0].Rows[i]["fpjslxr"].ToString() + "\n收件人电话：" + NewDS.Tables[0].Rows[i]["fpjslxrdh"].ToString();
            }
            DataTable dt = NewDS.Tables[0];
            rptKPXX.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rptKPXX.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.
        ht_where["page_size"] = " 20 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " a.*,'无' as zpxx,'' as ydxx  ";
        ht_where["search_tbname"] = "  AAA_PTKPXXB as a left join AAA_DLZHXXB as b on a.khbh=b.I_ZQZJZH   ";  //检索的表
        ht_where["search_mainid"] = " a.number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1  and a.zt='已生效'";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " a.zhgxsj ";  //用于排序的字段   
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        if (UCFWJGDetail1 .Value [1].ToString () != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.ptgljg='" + UCFWJGDetail1.Value[1].ToString() + "'";
        }
        else if (UCFWJGDetail1.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString() + "'";
        }
        if (ddlFPLX.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.fplx='" + ddlFPLX.SelectedValue.ToString() + "' ";
        }
        if (txtKHBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.khbh like '%" + txtKHBH.Text.Trim() + "%'";
        }
        if (txtDWMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.dwmc like '%" + txtDWMC.Text.Trim() + "%'";
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
        Label lblZPXX = (Label)e.Item.FindControl("lblZPXX");
        Label lblYDXX = (Label)e.Item.FindControl("lblYDXX");
        if (lblDWMC.Text.Length > 10)
            lblDWMC.Text = lblDWMC.Text.Substring(0, 10) + "...";
        if (lblZPXX.Text.Length > 10)
            lblZPXX.Text = lblZPXX.Text.Substring(0, 10) + "...";
        if (lblYDXX.Text.Length > 10)
            lblYDXX.Text = lblYDXX.Text.Substring(0, 10) + "...";

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string sql = "select ptgljg as 平台管理机构,khbh as 客户编号,jyzhlx as 交易账户类型,zclb as 注册类别,fplx as 发票类别,dwmc as 单位名称,ybnsrsbh as 一般纳税人识别号,dwdz as 单位地址,lxdh as 联系电话,khh as 开户行,khzh as 开户账号,fpjsdwmc as 发票接收单位名称,fpjsdz as 发票接收地址,fpjslxr as 发票接收联系人,fpjslxrdh as 发票接收联系人电话,zhgxsj as 最后更新时间 from AAA_PTKPXXB as a left join AAA_DLZHXXB as b on a.khbh=b.I_ZQZJZH   where  " + ViewState["where"] + " order by zhgxsj desc";
        DataSet ds = DbHelperSQL.Query(sql);

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "平台开票信息明细表", "平台开票信息明细表", 25);
    }
}