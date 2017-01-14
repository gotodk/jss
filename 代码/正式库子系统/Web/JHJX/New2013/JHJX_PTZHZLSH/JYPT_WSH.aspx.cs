using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_PTZHZLSH_JYPT_WSH : System.Web.UI.Page
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
           // rptZJYEBDMX.DataSource = DbHelperSQL.Query("select DL.Number,B_DLYX '交易方账号',DL.I_ZQZJZH '交易方编号',DL.B_JSZHLX '账户类型',DL.I_JYFMC '交易方名称',DL.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',(select I_PTGLJG from dbo.AAA_DLZHXXB where J_JJRJSBH=GL.GLJJRBH) '平台管理机构',DL.I_LXRXM '联系人姓名',DL.I_LXRSJH '联系人手机号' from dbo.AAA_DLZHXXB as DL inner join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on DL.B_DLYX=GL.DLYX left join AAA_JYZHZSB as ZS on DL.I_ZQZJZH=ZS.JYFBH  where GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过' and (ZS.FWZXSHZT is null or ZS.FWZXSHZT='未审核') and 1!=1");
            rptZJYEBDMX.DataSource = null;
            this.tdEmpty.Visible = true;
        }
        rptZJYEBDMX.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        // ht_where["search_tbname"] = " (select DL.Number,B_DLYX '交易方账号',DL.I_ZQZJZH '交易方编号',DL.B_JSZHLX '账户类型',DL.I_JYFMC '交易方名称',DL.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',DL.I_YWGLBMFL '业务管理部门分类',DL.I_PTGLJG '平台管理机构',DL.I_LXRXM '联系人姓名',DL.I_LXRSJH '联系人手机号' from dbo.AAA_DLZHXXB as DL inner join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on DL.B_DLYX=GL.DLYX left join AAA_JYZHZSB as ZS on DL.I_ZQZJZH=ZS.JYFBH  where GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过' and (ZS.FWZXSHZT is null or ZS.FWZXSHZT='未审核') ) as tab";  //检索的表      
        // ht_where["search_mainid"] = " Number ";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        // ht_where["search_paixu"] = " ASC ";  //排序方式
        // ht_where["search_paixuZD"] = "分公司审核时间 ";  //用于排序的字段

        /*---shiyan 2013-12-17 优化数据获取---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.        
        ht_where["serach_Row_str"] = "  b.Number,a.DLYX as 交易方账号,b.I_ZQZJZH as 交易方编号,a.JSZHLX as 账户类型, b.I_JYFMC as 交易方名称, b.I_ZCLB as 注册类别,a.FGSSHSJ as 分公司审核时间, b.I_YWGLBMFL as 业务管理部门分类, b.I_PTGLJG as 平台管理机构,b.I_LXRXM as 联系人姓名,b.I_LXRSJH as 联系人手机号 ";
        ht_where["search_tbname"] = " AAA_MJMJJYZHYJJRZHGLB as a left join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX ";  //检索的表      
        ht_where["search_mainid"] = " b.Number ";  //所检索表的主键
        ht_where["search_str_where"] = " SFYX='是' and SFSCGLJJR='是' and FGSSHZT='审核通过'  and not exists (select 1 from AAA_JYZHZSB where DLYX=a.DLYX) ";  //检索条件  
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = "a.FGSSHSJ ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/JHJX_PTZHZLSH/JYPT_WSH_info.aspx";        
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        /*---shiyan增加 2013-12-17 进行数据获取优化。---*/
        if (UCFWJGDetail1.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString() + "' ";
        }
        if (UCFWJGDetail1.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and  b.I_PTGLJG='" + UCFWJGDetail1.Value[1].ToString() + "' ";
        }
        if (ddZHLB.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.JSZHLX='" + this.ddZHLB.SelectedValue.ToString() + "' ";
        }
        if (ddZCLB.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_ZCLB='" + this.ddZCLB.SelectedValue.ToString() + "' ";
        }
        if (txtJYFMC.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_JYFMC like '%" + this.txtJYFMC.Text.Trim() + "%' ";
        }

        if (txtJYFZH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.DLYX like '%" + this.txtJYFZH.Text.Trim() + "%' ";
        }
        /*---shiyan增加结束---*/
       //string sql_where = "and 业务管理部门分类 like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and 平台管理机构 like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and 账户类型 like '%" + this.ddZHLB.SelectedValue.ToString() + "%' and 注册类别 like '%" + this.ddZCLB.SelectedValue.ToString() + "%' and 交易方名称 like '%" + this.txtJYFMC.Text.Trim() + "%' ";
        //HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

        ViewState["ht_where"] = HTwhere["search_str_where"];
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
        //string sql_where = " and 业务管理部门分类 like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and 平台管理机构 like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and 账户类型 like '%" + this.ddZHLB.SelectedValue.ToString() + "%' and 注册类别 like '%" + this.ddZCLB.SelectedValue.ToString() + "%' and 交易方名称 like '%" + this.txtJYFMC.Text.Trim() + "%' ";


        string sql1 = "select b.Number,a.DLYX as 交易方账号,b.I_ZQZJZH as 交易方编号,JSZHLX as 账户类型, b.I_JYFMC as 交易方名称, b.I_ZCLB as 注册类别,FGSSHSJ as 分公司审核时间, b.I_YWGLBMFL as 业务管理部门分类, b.I_PTGLJG as 业务管理部门,b.I_LXRXM as 联系人姓名,b.I_LXRSJH as 联系人手机号 from AAA_MJMJJYZHYJJRZHGLB as a left join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where "+ViewState ["ht_where"]+" order by FGSSHSJ ASC";

        DataSet NewDS = DbHelperSQL.Query(sql1);

        NewDS.Tables[0].Columns.Remove("Number");
        NewDS.Tables[0].Columns.Remove("业务管理部门分类");

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(NewDS, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "平台未审核资料信息", "平台未审核资料信息", 15);
    }
    protected void rptZJYEBDMX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "linkCKXQ"://进行审核
                string strUrl = "JYPT_WSH_info.aspx?Number=" + e.CommandArgument.ToString();
                Response.Redirect(strUrl);
                break;
        }
    }
}