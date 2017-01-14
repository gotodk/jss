using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_TBZLSH_JHJX_YCTBZLSH_JYGLBSH : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            this.UCFWJGDetail1.initdefault();
            DisGrid();
        }
    }

 

    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        // Response.Write(ERRinfo);
        //Response.End();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器超时，请稍后重试！',function(){ window.location.href=window.location.href;});</script>");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {

            rptZJYEBDMX.DataSource = NewDS.Tables[0].DefaultView;
            this.tdEmpty.Visible = false;
        }
        else
        {
            rptZJYEBDMX.DataSource = DbHelperSQL.Query("select TBSH.Number,TBSH.TBDH '投标单编号',TBD.DLYX '卖方账号',DD.I_JYFMC '卖方名称',TBD.TJSJ '投标时间',(case (select COUNT(*)  from dbo.AAA_ZBDBXXB where T_YSTBDBH=TBSH.TBDH ) when 0 then TBD.ZT else  (select top 1 Z_HTZT from dbo.AAA_ZBDBXXB where T_YSTBDBH=TBSH.TBDH order by CreateTime asc) end) '投标单状态',TBSH.FWZXSHSJ '服务中心审核时间',TBSH.JYGLBSHWTGHSFXG '上次审核未通过后是否修改',isnull(convert(varchar(20),TBSH.MJZXXGSJ,120),'--') '卖方修改时间',(case TBSH.FWZXSHZT when '审核正常' then '--' else TBSH.JYGLBSHZT end) '交易管理部审核状态',TBD.SPMC '商品名称',TBD.HTQX '合同期限',TBD.GG '规格',TBD.TBNSL '投标拟售量',TBD.TBJG '投标价格',DD.I_YWGLBMFL '业务管理部门分类',TBD.GLJJRPTGLJG '平台管理机构' from dbo.AAA_TBZLSHB as TBSH left join dbo.AAA_TBD as TBD on  TBSH.TBDH=TBD.Number left join dbo.AAA_DLZHXXB as DD on  TBD.DLYX=DD.B_DLYX where TBSH.FWZXSHZT in('审核异常') and TBSH.JYGLBSHZT in('未审核','审核未通过') and TBSH.FWZXSHYCHSFXG='是'   and 1!=1");
            this.tdEmpty.Visible = true;
        }
        rptZJYEBDMX.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select TBSH.Number,TBSH.TBDH '投标单编号',TBD.DLYX '卖方账号',DD.I_JYFMC '卖方名称',TBD.TJSJ '投标时间',(case (select COUNT(*)  from dbo.AAA_ZBDBXXB where T_YSTBDBH=TBSH.TBDH ) when 0 then TBD.ZT else  (select top 1 Z_HTZT from dbo.AAA_ZBDBXXB where T_YSTBDBH=TBSH.TBDH order by CreateTime asc) end) '投标单状态',TBSH.FWZXSHSJ '服务中心审核时间',TBSH.JYGLBSHWTGHSFXG '上次审核未通过后是否修改',isnull(convert(varchar(20),TBSH.MJZXXGSJ,120),'--') '卖家修改时间',(case TBSH.FWZXSHZT when '审核正常' then '--' else TBSH.JYGLBSHZT end) '交易管理部审核状态',TBD.SPMC '商品名称',TBD.HTQX '合同期限',TBD.GG '规格',TBD.TBNSL '投标拟售量',TBD.TBJG '投标价格',DD.I_YWGLBMFL '业务管理部门分类',TBD.GLJJRPTGLJG '平台管理机构' from dbo.AAA_TBZLSHB as TBSH left join dbo.AAA_TBD as TBD on  TBSH.TBDH=TBD.Number left join dbo.AAA_DLZHXXB as DD on  TBD.DLYX=DD.B_DLYX where TBSH.FWZXSHZT in('审核异常') and TBSH.JYGLBSHZT in('未审核','审核未通过') and TBSH.FWZXSHYCHSFXG='是'  ) as tab";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = "卖家修改时间 ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/TBZLSH/JHJX_YCTBZLSH_JYGLBSH_info.aspx";
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        string sql_where = " and 业务管理部门分类 like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and 平台管理机构 like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and 合同期限 like '%" + this.ddHTQX.SelectedValue.ToString() + "%' and 卖方名称 like '%" + this.txtBuyerMC.Text.Trim().ToString() + "%' and 投标单编号 like '%" + this.txtTBDBH.Text.Trim().ToString() + "%'";


        string tbStartTime = txtTBStart.Text.Trim();
        string tbEndTime = txtTBEnd.Text.Trim();
        if (tbStartTime != "" && tbEndTime == "")
        {
            sql_where = sql_where + " and CONVERT(varchar(10),卖家修改时间,120) >= '" + Convert.ToDateTime(tbStartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (tbStartTime == "" && tbEndTime != "")
        {
            sql_where = sql_where + " and  CONVERT(varchar(10),卖家修改时间,120) <= '" + Convert.ToDateTime(txtTBEnd).ToString("yyyy-MM-dd") + "' ";
        }
        else if (tbStartTime != "" && tbEndTime != "")
        {
            sql_where = sql_where + " and CONVERT(varchar(10),卖家修改时间,120) between '" + Convert.ToDateTime(tbStartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(tbEndTime).ToString("yyyy-MM-dd") + "' ";
        }
        //账号
        else if (txtSelzh.Text.Trim() != "")
        {
            sql_where = sql_where + " and  卖方账号 like '%" + txtSelzh.Text.Trim() + "%' ";
        }


        HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }



    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    /// <summary>
    /// 生成Excel文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        string sql_where = " and 业务管理部门分类 like '%" + UCFWJGDetail1.Value[0].ToString() + "%'  and 平台管理机构 like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and 合同期限 like '%" + this.ddHTQX.SelectedValue.ToString() + "%' and 卖方名称 like '%" + this.txtBuyerMC.Text.Trim().ToString() + "%' and 投标单编号 like '%" + this.txtTBDBH.Text.Trim().ToString() + "%'";


        string tbStartTime = txtTBStart.Text.Trim();
        string tbEndTime = txtTBEnd.Text.Trim();
        if (tbStartTime != "" && tbEndTime == "")
        {
            sql_where = sql_where + " and CONVERT(varchar(10),卖方修改时间,120) >= '" + Convert.ToDateTime(tbStartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (tbStartTime == "" && tbEndTime != "")
        {
            sql_where = sql_where + " and  CONVERT(varchar(10),卖方修改时间,120) <= '" + Convert.ToDateTime(txtTBEnd).ToString("yyyy-MM-dd") + "' ";
        }
        else if (tbStartTime != "" && tbEndTime != "")
        {
            sql_where = sql_where + " and CONVERT(varchar(10),卖方修改时间,120) between '" + Convert.ToDateTime(tbStartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(tbEndTime).ToString("yyyy-MM-dd") + "' ";
        }

            //账号
        else if (txtSelzh.Text.Trim() != "")
        {
            sql_where = sql_where + " and  卖方账号 like '%" + txtSelzh.Text.Trim() + "%' ";
        }


        string sql1 = "select * from (select TBSH.Number,TBSH.TBDH '投标单编号',TBD.DLYX '卖方账号',DD.I_JYFMC '卖方名称',TBD.TJSJ '投标时间',(case (select COUNT(*)  from dbo.AAA_ZBDBXXB where T_YSTBDBH=TBSH.TBDH ) when 0 then TBD.ZT else  (select top 1 Z_HTZT from dbo.AAA_ZBDBXXB where T_YSTBDBH=TBSH.TBDH order by CreateTime asc) end) '投标单状态',TBSH.FWZXSHSJ '服务中心审核时间',TBSH.JYGLBSHWTGHSFXG '上次审核未通过后是否修改',isnull(convert(varchar(20),TBSH.MJZXXGSJ,120),'--') '卖方修改时间',(case TBSH.FWZXSHZT when '审核正常' then '--' else TBSH.JYGLBSHZT end) '交易管理部审核状态',TBD.SPMC '商品名称',TBD.HTQX '合同期限',TBD.GG '规格',TBD.TBNSL '投标拟售量',TBD.TBJG '投标价格',DD.I_YWGLBMFL '业务管理部门分类',TBD.GLJJRPTGLJG '平台管理机构' from dbo.AAA_TBZLSHB as TBSH left join dbo.AAA_TBD as TBD on  TBSH.TBDH=TBD.Number left join dbo.AAA_DLZHXXB as DD on  TBD.DLYX=DD.B_DLYX where TBSH.FWZXSHZT in('审核异常') and TBSH.JYGLBSHZT in('未审核','审核未通过') and TBSH.FWZXSHYCHSFXG='是'  ) as tab where 1=1 " + sql_where + " order by 卖方修改时间 asc ";

        DataSet NewDS = DbHelperSQL.Query(sql1);
     
        NewDS.Tables[0].Columns.Remove("Number");

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(NewDS, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "异常投标单资料", "异常投标单资料", 15);
    }
    protected void rptZJYEBDMX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "linkCKXQ"://进行审核
                string strUrl = "JHJX_YCTBZLSH_JYGLBSH_info.aspx?Number=" + e.CommandArgument.ToString();
                Response.Redirect(strUrl);
                break;
        }
    }
}