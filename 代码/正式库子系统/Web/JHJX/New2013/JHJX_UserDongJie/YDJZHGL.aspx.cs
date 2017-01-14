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

public partial class Web_JHJX_New2013_JHJX_UserDongJie_YDJZHGL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
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
            UCFWJGDetail1 .Value  =new string[]{htTiaoJian["管理机构分类"].ToString(),htTiaoJian["平台管理机构"].ToString()};
            txtJYFZH .Text = htTiaoJian["交易方账号"].ToString();
            txtJYFMC.Text  = htTiaoJian["交易方名称"].ToString();
            ddlDJGN .SelectedValue = htTiaoJian["冻结功能"].ToString();
            ViewState["ht_where"] = ALLconfig["search_str_where"];
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
        ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " (select B_DLYX as '交易方账号', B_JSZHLX as '交易帐户类型',I_ZCLB as '注册类别',I_JYFMC as '交易方名称',I_LXRXM as  '联系人姓名',I_LXRSJH as '联系人手机号',I_JYFLXDH as '交易方联系电话',(I_SSQYS+I_SSQYSHI+I_SSQYQ) as 所属区域,  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = AAA_DLZHXXB.B_DLYX and SFDQMRJJR='是') as '关联经纪人',(select I_PTGLJG  from  AAA_DLZHXXB as aa where B_DLYX=(select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = AAA_DLZHXXB.B_DLYX and SFDQMRJJR='是')) as '平台管理机构',B_SFDJ as '是否冻结',DJGNX as 冻结功能 from dbo.AAA_DLZHXXB where isnull(B_SFDJ,'')='是') as tab ";  //原始语句，2013-10-11因修改交易账户平台管理机构的获取方式改为下面的语句
        ht_where["search_tbname"] = " (select B_DLYX as '交易方账号', B_JSZHLX as '交易帐户类型',I_ZCLB as '注册类别',I_JYFMC as '交易方名称',I_LXRXM as  '联系人姓名',I_LXRSJH as '联系人手机号',I_JYFLXDH as '交易方联系电话',(I_SSQYS+I_SSQYSHI+I_SSQYQ) as 所属区域,  b.关联经纪人,I_PTGLJG as '平台管理机构',I_YWGLBMFL as 管理机构分类,B_SFDJ as '是否冻结',DJGNX as 冻结功能 from dbo.AAA_DLZHXXB as a left join (select DLYX, GLJJRDLZH as 关联经纪人 from  AAA_MJMJJYZHYJJRZHGLB where SFDQMRJJR='是') as b on b.DLYX=a.B_DLYX  where isnull(B_SFDJ,'')='是') as tab";//检索的表
        ht_where["search_mainid"] = " 交易方账号 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " 交易方账号 ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "DJZHGL_info";

        Hashtable returnlastpage_spHT = new Hashtable();
        returnlastpage_spHT["管理机构分类"] = UCFWJGDetail1.Value[0].ToString();
        returnlastpage_spHT["平台管理机构"] = UCFWJGDetail1.Value[1].ToString();
        returnlastpage_spHT["交易方账号"] = txtJYFZH.Text.Trim();
        returnlastpage_spHT["交易方名称"] = txtJYFMC.Text.Trim();
        returnlastpage_spHT["冻结功能"] = ddlDJGN.SelectedValue.ToString();
        
        ht_where["returnlastpage_spHT"] = returnlastpage_spHT; //自动返回之前页数功能额度参数，可以设置特殊返回值，用于处理页面中条件的显示等。

        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        if (UCFWJGDetail1 .Value [1].ToString () != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 平台管理机构='" + UCFWJGDetail1.Value[1].ToString() + "'";
        }
        else if (UCFWJGDetail1.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 管理机构分类='" + UCFWJGDetail1.Value[0].ToString() + "'";
        }
        if (txtJYFZH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 交易方账号 like '%" + txtJYFZH.Text.Trim() + "%'";
        }
        if (txtJYFMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 交易方名称 like '%" + txtJYFMC.Text.Trim() + "%'";
        }
        if (ddlDJGN.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 冻结功能 like '%" + ddlDJGN.SelectedValue.ToString() + "%'";
        }

        ViewState["ht_where"] = HTwhere["search_str_where"];
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    protected void rptKPXX_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblJYFZH = (Label)e.Item.FindControl("lblJYFZH");
        if (lblJYFZH.Text.Length > 12)
            lblJYFZH.Text = lblJYFZH.Text.Substring(0, 12) + "...";

        Label lblJYFMC = (Label)e.Item.FindControl("lblJYFMC");
        if (lblJYFMC.Text.Length > 8)
            lblJYFMC.Text = lblJYFMC.Text.Substring(0, 8) + "...";

        Label lblJYFLXDH = (Label)e.Item.FindControl("lblJYFLXDH");
        if (lblJYFLXDH.Text.Length > 8)
            lblJYFLXDH.Text = lblJYFLXDH.Text.Substring(0, 8) + "...";

        Label lblLXRSJH = (Label)e.Item.FindControl("lblLXRSJH");
        if (lblLXRSJH.Text.Length > 10)
            lblLXRSJH.Text = lblLXRSJH.Text.Substring(0, 10) + "...";

        Label lblSSQY = (Label)e.Item.FindControl("lblSSQY");
        if (lblSSQY.Text.Length > 6)
            lblSSQY.Text = lblSSQY.Text.Substring(0,6) + "...";

        Label lblDQGLJJR = (Label)e.Item.FindControl("lblDQGLJJR");
        if (lblDQGLJJR.Text.Length > 10)
            lblDQGLJJR.Text = lblDQGLJJR.Text.Substring(0, 10) + "...";

        Label lblDJGN = (Label)e.Item.FindControl("lblDJGN");
        if (lblDJGN.Text.Length > 8)
            lblDJGN.Text = lblDJGN.Text.Substring(0, 8) + "...";     
    }

    protected void rptKPXX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        if (e.CommandName == "linkbj")
        {
            Response.Redirect("YDJZHGL_info.aspx?dlyx=" + e.CommandArgument.ToString().Trim());
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string sql = " select * from (select B_DLYX as '交易方账号', B_JSZHLX as '交易帐户类型',I_ZCLB as '注册类别',I_JYFMC as '交易方名称',I_LXRXM as  '联系人姓名',I_LXRSJH as '联系人手机号',I_JYFLXDH as '交易方联系电话',(I_SSQYS+I_SSQYSHI+I_SSQYQ) as 所属区域,  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = AAA_DLZHXXB.B_DLYX and SFDQMRJJR='是') as '关联经纪人',(select I_PTGLJG  from  AAA_DLZHXXB as aa where B_DLYX=(select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = AAA_DLZHXXB.B_DLYX and SFDQMRJJR='是')) as '平台管理机构',I_YWGLBMFL as 管理机构分类,B_SFDJ as '是否冻结',DJGNX as 冻结功能 from dbo.AAA_DLZHXXB where isnull(B_SFDJ,'')='是') as tab where " + ViewState["ht_where"].ToString() + " order by 交易方账号 desc";
        DataSet ds = DbHelperSQL.Query(sql);

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "已冻结交易账号", "已冻结交易账户", 25);
    }
}