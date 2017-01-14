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
using System.Net;
using System.Text;
using System.Configuration;

public partial class Web_JHJX_PTFPYJXX_LR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        commonpagernew1.OnNeedLoadData_all += new Web_pagerdemo_commonpagernew.OnNeedDataHandler_all(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
 
            bindPTGLJG();

            DisGrid();
        }
    }
    private void bindPTGLJG()
    {
        DataSet ds = DbHelperSQL.Query("select FGSname,BSCname from  AAA_CityList_FGS group by FGSname,BSCname order by FGSname");
        ddlPTGLJG.DataSource = ds.Tables[0].DefaultView;
        ddlPTGLJG.DataTextField = "FGSname";
        ddlPTGLJG.DataValueField = "FGSname";
        ddlPTGLJG.DataBind();
        ddlPTGLJG.Items.Insert(0, new ListItem("全部分公司", ""));
        //string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        //string bm = DbHelperSQL.GetSingle(sql).ToString();

        string url = ConfigurationManager.ConnectionStrings["GetBM"].ToString().Trim() + "/Web/GetBM/Handler.ashx?number=" + User.Identity.Name + "&method=GetBM";
        WebClient httpclient = new WebClient();
        //byte[] bytes = httpclient.DownloadData(url);
        httpclient.Encoding = Encoding.UTF8;
        string bm = httpclient.DownloadString(url);

        if (bm != null && bm != "")
        {
            string sql = "select count(BSCname) from AAA_CityList_FGS where BSCname='" + bm + "'";
            string fgsSql = "select FGSname from AAA_CityList_FGS where BSCname='" + bm + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                string strFGSMC = DbHelperSQL.GetSingle(fgsSql).ToString();
                ddlPTGLJG.DataSource = null;
                ddlPTGLJG.DataBind();
                ddlPTGLJG.Items.Clear();
                ddlPTGLJG.Items.Add(strFGSMC);
                ddlPTGLJG.Enabled = false;
            }
        }
    }


    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, Hashtable ALLconfig)
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

            Hashtable htTiaoJian = (Hashtable)ALLconfig["returnlastpage_spHT"];
            ddlPTGLJG.SelectedValue = htTiaoJian["平台管理机构"].ToString();
            ddlFPLX.SelectedValue = htTiaoJian["发票类别"].ToString();        
            txtKHBH.Text = htTiaoJian["客户编号"].ToString();
            txtDWMC.Text = htTiaoJian["单位名称"].ToString();
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
        ht_where["this_dblink"] = "fmConnectionWrite";//这个可以不设置,默认是FMOP,即连接业务平台数据库。如需连接ERP，则需要设置为ERP.
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = "ltrim(rtrim(TA001))+'_'+ltrim(rtrim(TA002)) as 编号, ltrim(rtrim(TA001)) as 单别,ltrim(rtrim(TA002)) as 单号,convert(varchar(10),convert(datetime,TA003),120) as 审核日期,ltrim(rtrim(TA004)) as 客户编号,ltrim(rtrim(TA008)) as 客户全称,TA070 as 平台管理机构编号,ltrim(rtrim(ME002)) as 平台管理机构,(case  ltrim(rtrim(TA011)) when 'A' then '增值税专用发票' when 'B' then '增值税普通发票'  else '其他' end) as 发票种类,cast(TA029+TA030 as numeric(18,2)) as 发票金额,(case when ltrim(rtrim(TA015))='' then '暂无' when TA015 is null then '暂无' else ltrim(rtrim(TA015)) end) as 发票号码 ";
        ht_where["search_tbname"] = " ACRTA left join CMSME on TA070=ME001 ";  //检索的表
        ht_where["search_mainid"] = " TA001+TA002 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 and TA001='613' and TA025='Y' ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " TA003 ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "PTFPYJXX_LRinfo";

        Hashtable returnlastpage_spHT = new Hashtable();
        returnlastpage_spHT["平台管理机构"] = ddlPTGLJG.SelectedValue.ToString();
        returnlastpage_spHT["发票类别"] = ddlFPLX.SelectedValue.ToString();      
        returnlastpage_spHT["客户编号"] = txtKHBH.Text.Trim();
        returnlastpage_spHT["单位名称"] = txtDWMC.Text.Trim();

        ht_where["returnlastpage_spHT"] = returnlastpage_spHT; //自动返回之前页数功能额度参数，可以设置特殊返回值，用于处理页面中条件的显示等。

        return ht_where;
    }
    public void DisGrid()
    {
        

        //开始调用自定义控件
        Hashtable HTwhere = SetV();        
       
        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and ltrim(rtrim(TA002)) not in (select TA002 from AAA_613ACRTA) ";
        if (ddlPTGLJG.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and ltrim(rtrim(ME002)) like '%" + ddlPTGLJG.SelectedValue.ToString() + "%'";
        }
        if (ddlFPLX.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and ltrim(rtrim(TA011))='" + ddlFPLX.SelectedValue.ToString() + "' ";
        }        
        if (txtKHBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and TA004 like '%" + txtKHBH.Text.Trim() + "%'";
        }
        if (txtDWMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and TA008 like '%" + txtDWMC.Text.Trim() + "%'";
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
        if (lblDWMC.Text.Length > 10)
            lblDWMC.Text = lblDWMC.Text.Substring(0, 10) + "...";      
    }

    protected void rptKPXX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        if (e.CommandName == "linkbj")
        {
            Response.Redirect("PTFPYJXX_LRinfo.aspx?type=commit&Number=" + e.CommandArgument.ToString().Trim());
        }
    }
}