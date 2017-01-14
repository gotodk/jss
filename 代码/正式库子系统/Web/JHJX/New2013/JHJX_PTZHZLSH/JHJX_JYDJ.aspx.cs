using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_PTZHZLSH_JHJX_JYDJ : System.Web.UI.Page
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
            rptZJYEBDMX.DataSource = DbHelperSQL.Query("select  ZS.Number,ZS.DLYX '交易方账号',ZS.JYFBH '交易方编号',ZS.JSZHLX '账户类型',DD.I_JYFMC '交易方名称',DD.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',(case when ZS.FWZXXSHSJ is null then ZS.FWZXSHSJ else ZS.FWZXXSHSJ end ) '服务中心审核时间',(case when ZS.FWZXXSHR is null then ZS.FWZXSHR  else ZS.FWZXXSHR end) '服务中心审核人',isnull(ZS.FWZXSHYJ,'') '服务中心审核意见',isnull(ZS.FWZXXSHYJ,'') '服务中心新审核意见',ZS.JYGLBSHZT '交易管理部审核状态',ZS.FWZXXSHSJ,ZS.FWZXXSHR,ZS.JYGLBSHZT '交易管理部审核状态',ZS.JYGLBSHYJ '交易管理部审核意见',ZS.JYGLBSHSJ '交易管理部审核时间',ZS.JYGLBSHR '交易管理部审核人',DD.I_YWGLBMFL '业务管理部门分类',ZS.PTGLJG '平台管理机构',DD.I_LXRXM '联系人姓名',DD.I_LXRSJH '联系人手机号' from AAA_JYZHZSB as ZS left join AAA_DLZHXXB as DD on ZS.DLYX=DD.B_DLYX  left join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on ZS.DLYX=GL.DLYX where 1!=1");
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
        ht_where["search_tbname"] = "(select *,1 '列排序',(row_number() OVER (ORDER BY 服务中心审核时间 asc)) '排序ID' from(select  ZS.Number,ZS.DLYX '交易方账号',ZS.JYFBH '交易方编号',ZS.JSZHLX '账户类型',DD.I_JYFMC '交易方名称',DD.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',(case when ZS.FWZXXSHSJ is null then ZS.FWZXSHSJ else ZS.FWZXXSHSJ end ) '服务中心审核时间',(case when ZS.FWZXXSHR is null then ZS.FWZXSHR  else ZS.FWZXXSHR end) '服务中心审核人',isnull(ZS.FWZXSHYJ,'') '服务中心审核意见',isnull(ZS.FWZXXSHYJ,'') '服务中心新审核意见',ZS.JYGLBSHZT '交易管理部审核状态',isnull(ZS.JYGLBSHYJ,'- -') '交易管理部审核意见',ZS.JYGLBSHSJ '交易管理部审核时间',isnull(ZS.JYGLBSHR,'- -') '交易管理部审核人',DD.I_YWGLBMFL '业务管理部门分类',ZS.PTGLJG '平台管理机构',DD.I_LXRXM '联系人姓名',DD.I_LXRSJH '联系人手机号' from AAA_JYZHZSB as ZS left join AAA_DLZHXXB as DD on ZS.DLYX=DD.B_DLYX  left join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on ZS.DLYX=GL.DLYX where GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过'  and ZS.FWZXSHZT='建议冻结' and ZS.JYGLBSHZT='尚未处理') as tab1 union all select *,2 '列排序',(row_number() OVER (ORDER BY 交易管理部审核时间 desc)) '排序ID' from ( select  ZS.Number,ZS.DLYX '交易方账号',ZS.JYFBH '交易方编号',ZS.JSZHLX '账户类型',DD.I_JYFMC '交易方名称',DD.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',(case when ZS.FWZXXSHSJ is null then ZS.FWZXSHSJ else ZS.FWZXXSHSJ end ) '服务中心审核时间',(case when ZS.FWZXXSHR is null then ZS.FWZXSHR  else ZS.FWZXXSHR end) '服务中心审核人',isnull(ZS.FWZXSHYJ,'') '服务中心审核意见',isnull(ZS.FWZXXSHYJ,'') '服务中心新审核意见',ZS.JYGLBSHZT '交易管理部审核状态',isnull(ZS.JYGLBSHYJ,'- -') '交易管理部审核意见',ZS.JYGLBSHSJ '交易管理部审核时间',isnull(ZS.JYGLBSHR,'- -') '交易管理部审核人',DD.I_YWGLBMFL '业务管理部门分类',ZS.PTGLJG '平台管理机构',DD.I_LXRXM '联系人姓名',DD.I_LXRSJH '联系人手机号' from AAA_JYZHZSB as ZS left join AAA_DLZHXXB as DD on ZS.DLYX=DD.B_DLYX  left join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on ZS.DLYX=GL.DLYX where GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过'  and ZS.FWZXSHZT='建议冻结' and ZS.JYGLBSHZT in('同意冻结')) as tab2 union all select *,3 '列排序',(row_number() OVER (ORDER BY 交易管理部审核时间 desc)) '排序ID' from ( select  ZS.Number,ZS.DLYX '交易方账号',ZS.JYFBH '交易方编号',ZS.JSZHLX '账户类型',DD.I_JYFMC '交易方名称',DD.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',(case when ZS.FWZXXSHSJ is null then ZS.FWZXSHSJ else ZS.FWZXXSHSJ end ) '服务中心审核时间',(case when ZS.FWZXXSHR is null then ZS.FWZXSHR  else ZS.FWZXXSHR end) '服务中心审核人',isnull(ZS.FWZXSHYJ,'') '服务中心审核意见',isnull(ZS.FWZXXSHYJ,'') '服务中心新审核意见',ZS.JYGLBSHZT '交易管理部审核状态',isnull(ZS.JYGLBSHYJ,'- -') '交易管理部审核意见',ZS.JYGLBSHSJ '交易管理部审核时间',isnull(ZS.JYGLBSHR,'- -') '交易管理部审核人',DD.I_YWGLBMFL '业务管理部门分类',ZS.PTGLJG '平台管理机构',DD.I_LXRXM '联系人姓名',DD.I_LXRSJH '联系人手机号' from AAA_JYZHZSB as ZS left join AAA_DLZHXXB as DD on ZS.DLYX=DD.B_DLYX  left join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on ZS.DLYX=GL.DLYX where GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过'  and ZS.FWZXSHZT='建议冻结' and ZS.JYGLBSHZT in('不予冻结')) as tab3 ) as tab";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " 列排序 asc,排序ID  ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/JHJX_PTZHZLSH/JHJX_JYDJ_info.aspx";
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        string sql_where = " and 业务管理部门分类 like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and 平台管理机构 like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and 账户类型 like '%" + this.ddZHLB.SelectedValue.ToString() + "%' and 注册类别 like '%" + this.ddZCLB.SelectedValue.ToString() + "%' and 交易方名称 like '%" + this.txtJYFMC.Text.Trim() + "%' and 交易方账号 like '%" + this.txtJYFZH.Text.Trim() + "%'";

        string fgsStartTime = txtFGSBeginTime.Text.Trim();
        string fgsEndTime = txtFGSEndTime.Text.Trim();
        if (fgsStartTime != "" && fgsEndTime == "")
        {
            sql_where = sql_where + " and 分公司审核时间 >= '" + Convert.ToDateTime(fgsStartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (fgsStartTime == "" && fgsEndTime != "")
        {
            sql_where = sql_where + " and  分公司审核时间 <= '" + Convert.ToDateTime(fgsEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (fgsStartTime != "" && fgsEndTime != "")
        {
            sql_where = sql_where + " and 分公司审核时间 between '" + Convert.ToDateTime(fgsStartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(fgsEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }

        string fwzxStartTime = txtFWZXBeginTime.Text.Trim();
        string fwzxEndTime = txtFWZXEndTime.Text.Trim();
        if (fwzxStartTime != "" && fwzxEndTime == "")
        {
            sql_where = sql_where + " and 服务中心审核时间 >= '" + Convert.ToDateTime(fwzxStartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (fwzxStartTime == "" && fwzxEndTime != "")
        {
            sql_where = sql_where + " and  服务中心审核时间 <= '" + Convert.ToDateTime(fwzxEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (fwzxStartTime != "" && fwzxEndTime != "")
        {
            sql_where = sql_where + " and 服务中心审核时间 between '" + Convert.ToDateTime(fwzxStartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(fwzxEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        if (this.ddshzt != null && this.ddshzt.SelectedValue.Trim() != "" && this.ddshzt.SelectedValue.Trim() != "请选择")
        {
            sql_where = sql_where + " and 交易管理部审核状态 = '"+this.ddshzt.SelectedValue.Trim()+"'";
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

    /// <summary>
    /// 生成Excel文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        string sql_where = " and 业务管理部门分类 like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and 平台管理机构 like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and 账户类型 like '%" + this.ddZHLB.SelectedValue.ToString() + "%' and 注册类别 like '%" + this.ddZCLB.SelectedValue.ToString() + "%' and 交易方名称 like '%" + this.txtJYFMC.Text.Trim() + "%' and 交易方账号 like '%" + this.txtJYFZH.Text.Trim() + "%'";

        string fgsStartTime = txtFGSBeginTime.Text.Trim();
        string fgsEndTime = txtFGSEndTime.Text.Trim();
        if (fgsStartTime != "" && fgsEndTime == "")
        {
            sql_where = sql_where + " and 分公司审核时间 >= '" + Convert.ToDateTime(fgsStartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (fgsStartTime == "" && fgsEndTime != "")
        {
            sql_where = sql_where + " and  分公司审核时间 <= '" + Convert.ToDateTime(fgsEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (fgsStartTime != "" && fgsEndTime != "")
        {
            sql_where = sql_where + " and 分公司审核时间 between '" + Convert.ToDateTime(fgsStartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(fgsEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }

        string fwzxStartTime = txtFWZXBeginTime.Text.Trim();
        string fwzxEndTime = txtFWZXEndTime.Text.Trim();
        if (fwzxStartTime != "" && fwzxEndTime == "")
        {
            sql_where = sql_where + " and 服务中心审核时间 >= '" + Convert.ToDateTime(fwzxStartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (fwzxStartTime == "" && fwzxEndTime != "")
        {
            sql_where = sql_where + " and  服务中心审核时间 <= '" + Convert.ToDateTime(fwzxEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (fwzxStartTime != "" && fwzxEndTime != "")
        {
            sql_where = sql_where + " and 服务中心审核时间 between '" + Convert.ToDateTime(fwzxStartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(fwzxEndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        if (this.ddshzt != null && this.ddshzt.SelectedValue.Trim() != "" && this.ddshzt.SelectedValue.Trim() != "请选择")
        {
            sql_where = sql_where + " and 交易管理部审核状态 = '" + this.ddshzt.SelectedValue.Trim() + "'";
        }


        string sql1 = "select * from (select *,1 '列排序',(row_number() OVER (ORDER BY 服务中心审核时间 asc)) '排序ID' from(select  ZS.Number,ZS.DLYX '交易方账号',ZS.JYFBH '交易方编号',ZS.JSZHLX '账户类型',DD.I_JYFMC '交易方名称',DD.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',(case when ZS.FWZXXSHSJ is null then ZS.FWZXSHSJ else ZS.FWZXXSHSJ end ) '服务中心审核时间',(case when ZS.FWZXXSHR is null then ZS.FWZXSHR  else ZS.FWZXXSHR end) '服务中心审核人',isnull(ZS.FWZXSHYJ,'') '服务中心审核意见',isnull(ZS.FWZXXSHYJ,'') '服务中心新审核意见',ZS.JYGLBSHZT '交易管理部审核状态',isnull(ZS.JYGLBSHYJ,'- -') '交易管理部审核意见',ZS.JYGLBSHSJ '交易管理部审核时间',isnull(ZS.JYGLBSHR,'- -') '交易管理部审核人',DD.I_YWGLBMFL '业务管理部门分类',ZS.PTGLJG '平台管理机构',DD.I_LXRXM '联系人姓名',DD.I_LXRSJH '联系人手机号' from AAA_JYZHZSB as ZS left join AAA_DLZHXXB as DD on ZS.DLYX=DD.B_DLYX  left join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on ZS.DLYX=GL.DLYX where GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过'  and ZS.FWZXSHZT='建议冻结' and ZS.JYGLBSHZT='尚未处理') as tab1 union all select *,2 '列排序',(row_number() OVER (ORDER BY 交易管理部审核时间 desc)) '排序ID' from ( select  ZS.Number,ZS.DLYX '交易方账号',ZS.JYFBH '交易方编号',ZS.JSZHLX '账户类型',DD.I_JYFMC '交易方名称',DD.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',(case when ZS.FWZXXSHSJ is null then ZS.FWZXSHSJ else ZS.FWZXXSHSJ end ) '服务中心审核时间',(case when ZS.FWZXXSHR is null then ZS.FWZXSHR  else ZS.FWZXXSHR end) '服务中心审核人',isnull(ZS.FWZXSHYJ,'') '服务中心审核意见',isnull(ZS.FWZXXSHYJ,'') '服务中心新审核意见',ZS.JYGLBSHZT '交易管理部审核状态',isnull(ZS.JYGLBSHYJ,'- -') '交易管理部审核意见',ZS.JYGLBSHSJ '交易管理部审核时间',isnull(ZS.JYGLBSHR,'- -') '交易管理部审核人',DD.I_YWGLBMFL '业务管理部门分类',ZS.PTGLJG '平台管理机构',DD.I_LXRXM '联系人姓名',DD.I_LXRSJH '联系人手机号' from AAA_JYZHZSB as ZS left join AAA_DLZHXXB as DD on ZS.DLYX=DD.B_DLYX  left join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on ZS.DLYX=GL.DLYX where GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过'  and ZS.FWZXSHZT='建议冻结' and ZS.JYGLBSHZT in('同意冻结')) as tab2 union all select *,3 '列排序',(row_number() OVER (ORDER BY 交易管理部审核时间 desc)) '排序ID' from ( select  ZS.Number,ZS.DLYX '交易方账号',ZS.JYFBH '交易方编号',ZS.JSZHLX '账户类型',DD.I_JYFMC '交易方名称',DD.I_ZCLB '注册类别',GL.FGSSHSJ '分公司审核时间',(case when ZS.FWZXXSHSJ is null then ZS.FWZXSHSJ else ZS.FWZXXSHSJ end ) '服务中心审核时间',(case when ZS.FWZXXSHR is null then ZS.FWZXSHR  else ZS.FWZXXSHR end) '服务中心审核人',isnull(ZS.FWZXSHYJ,'') '服务中心审核意见',isnull(ZS.FWZXXSHYJ,'') '服务中心新审核意见',ZS.JYGLBSHZT '交易管理部审核状态',isnull(ZS.JYGLBSHYJ,'- -') '交易管理部审核意见',ZS.JYGLBSHSJ '交易管理部审核时间',isnull(ZS.JYGLBSHR,'- -') '交易管理部审核人',DD.I_YWGLBMFL '业务管理部门分类',ZS.PTGLJG '平台管理机构',DD.I_LXRXM '联系人姓名',DD.I_LXRSJH '联系人手机号' from AAA_JYZHZSB as ZS left join AAA_DLZHXXB as DD on ZS.DLYX=DD.B_DLYX  left join dbo.AAA_MJMJJYZHYJJRZHGLB as GL on ZS.DLYX=GL.DLYX where GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过'  and ZS.FWZXSHZT='建议冻结' and ZS.JYGLBSHZT in('不予冻结')) as tab3 ) as tab where 1=1 " + sql_where + " order by 列排序 asc,排序ID asc ";

        DataSet NewDS = DbHelperSQL.Query(sql1);

        NewDS.Tables[0].Columns.Remove("Number");
        NewDS.Tables[0].Columns.Remove("列排序");
        NewDS.Tables[0].Columns.Remove("排序ID");
        NewDS.Tables[0].Columns.Remove("业务管理部门分类");

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(NewDS, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "平台建议冻结账户资料信息", "平台建议冻结账户资料信息", 15);
    }
    protected void rptZJYEBDMX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "linkCKXQ"://进行审核
                string strUrl = "JHJX_JYDJ_info.aspx?Number=" + e.CommandArgument.ToString() + "&Page=NBDY";
                Response.Redirect(strUrl);
                break;
        }
    }
}