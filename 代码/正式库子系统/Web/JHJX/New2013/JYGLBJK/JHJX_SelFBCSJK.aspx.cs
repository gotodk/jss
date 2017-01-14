using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.Common;
using FMOP.DB;

public partial class Web_JHJX_New2013_JYGLBJK_JHJX_SelFBCSJK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            //DisGrid();
        }
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        string startTime = txtBeginTime.Text.Trim();
        string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddMilliseconds(59.998).ToString();
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = "15"; //必须设置,每页的数据量。必须是数字。不能是0。
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " ( select *,count(*) 废标次数 from ( select T_YSTBDDLYX 账户名称,T_YSTBDJSZHLX 账户类型,b.I_ZCLB 注册类别,'关联经纪人'=(select GLJJRDLZH from AAA_MJMJJYZHYJJRZHGLB where DLYX=a.T_YSTBDDLYX and SFDQMRJJR='是'),b.I_LXRXM 联系人,b.I_LXRSJH 联系方式,'所属分公司'=(select d.I_PTGLJG from AAA_MJMJJYZHYJJRZHGLB c left join AAA_DLZHXXB d on c.GLJJRDLZH =d.B_DLYX  where c .DLYX=a.T_YSTBDDLYX and c.SFDQMRJJR='是') from AAA_ZBDBXXB a left join AAA_DLZHXXB b on a.T_YSTBDMJJSBH =b.J_SELJSBH where Z_HTZT in ('未定标废标','定标合同终止') and case Z_HTZT when '未定标废标' then Z_FBSJ else Z_QPKSSJ end between '" + startTime + "' and '" + endTime + "') as tab group by 账户名称,账户类型,注册类别,关联经纪人,联系人,联系方式,所属分公司  ) as tb ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 账户名称 ";  //所检索表的主键
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " 废标次数 ";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_CSSPZLSH/JHJX_CKXQ.aspx";
        return ht_where;
    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        // Response.Write(ERRinfo);
        //Response.End();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.Show(this, "查询超时，请重试或者修改查询条件！");
        }

        Repeater1.DataSource = null;

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
            ts.Visible = false;
        }
        else
        {
            DataSet ds = DbHelperSQL.Query("select '账户名称'='','账户类型'='','注册类别'='','关联经纪人'='','联系人'='','联系方式'='','所属分公司'='','废标次数'='' from AAA_ZBDBXXB  where 1!=1");
            Repeater1.DataSource = ds.Tables[0].DefaultView;
            ts.Visible = true;

        }
        Repeater1.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        if (!string.IsNullOrEmpty(txtFBCS.Text.Trim()))
        {
            HTwhere["search_str_where"] += " and 废标次数 >=" + txtFBCS.Text.Trim();
        }
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        if (txtBeginTime.Text.Trim() == "")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('废标起始时间不能为空！')</script>");
            return;
        }
        if (txtEndTime.Text.Trim() == "")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('废标结束时间不能为空！')</script>");
            return;
        }
        if (Convert.ToDateTime(txtBeginTime.Text.Trim()) > Convert.ToDateTime(txtEndTime.Text.Trim()))
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('废标结束时间必须大于废标起始时间！')</script>");
            return;
        }
        ViewState["BeginTime"] = txtBeginTime.Text.Trim();
        ViewState["EndTime"] = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddMilliseconds(59.998).ToString();
        DisGrid();
    }

    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.Charset = "";
        string filename = "JHJX_SelFBCSJK" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" +

        System.Web.HttpUtility.UrlEncode(filename, System.Text.Encoding.GetEncoding("GB2312")) + ".xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   

        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        #region//asp.net 导出Excel时，解决纯数字字符串变成类似这样的 2.00908E+18 形式的代码
        string strStyle = "<style>td{vnd.ms-excel.numberformat: @;}</style>";//导出的数字的样式
        tw.WriteLine(strStyle);
        #endregion

        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    }
    // 导出Exel时需重载此方法
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandArgument == "ck")
        {
            //Response.Redirect("JHJX_CKXQ.aspx?number=" + e.CommandName.Trim() + "&GoBackUrl=CSSPZLSH_WSH.aspx");
            TabStrip1.Visible = false;            
            divLB.Visible = false;
            divXQ.Visible = true;
            string str = e.CommandName;
            spanSP.InnerText = "账户名称：" + str ;
            XQ(str);
        }
    }
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        if (txtBeginTime.Text.Trim() == "")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('发布起始时间不能为空！')</script>");
            return;
        }
        if (txtEndTime.Text.Trim() == "")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('发布结束时间不能为空！')</script>");
            return;
        }       
        if (Convert.ToDateTime(txtBeginTime.Text.Trim()) > Convert.ToDateTime(txtEndTime.Text.Trim()))
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('发布结束时间必须大于发布起始时间！')</script>");
            return;
        }
        string startTime = txtBeginTime.Text.Trim();
        string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddMilliseconds(59.998).ToString();
        string str = " select *,count(*) 废标次数 from ( select T_YSTBDDLYX 账户名称,T_YSTBDJSZHLX 账户类型,b.I_ZCLB 注册类别,'关联经纪人'=(select GLJJRDLZH from AAA_MJMJJYZHYJJRZHGLB where DLYX=a.T_YSTBDDLYX and SFDQMRJJR='是'),b.I_LXRXM 联系人,b.I_LXRSJH 联系方式,'所属分公司'=(select d.I_PTGLJG from AAA_MJMJJYZHYJJRZHGLB c left join AAA_DLZHXXB d on c.GLJJRDLZH =d.B_DLYX  where c .DLYX=a.T_YSTBDDLYX and c.SFDQMRJJR='是') from AAA_ZBDBXXB a left join AAA_DLZHXXB b on a.T_YSTBDMJJSBH =b.J_SELJSBH where Z_HTZT in ('未定标废标','定标合同终止') and case Z_HTZT when '未定标废标' then Z_FBSJ else Z_QPKSSJ end between '" + startTime + "' and '" + endTime + "') as tab group by 账户名称,账户类型,注册类别,关联经纪人,联系人,联系方式,所属分公司   ";
        if(!string.IsNullOrEmpty(txtFBCS.Text.Trim()))
        {
            str += " having count(*)>=" + txtFBCS.Text.Trim();
        }
        str += "  order by count(*) desc";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Repeater2.DataSource = ds.Tables[0].DefaultView;
            exprestTD.Visible = false;
            Repeater2.DataBind();
        }
        else
        {
            exprestTD.Visible = true;
        }
        DisGrid();
        ToExcel(export);
    }

    protected void XQ(string DLYX)
    {
        Repeater3.DataSource = null;
        Repeater3.DataBind();
        string strSQL = "select Z_ZBSJ 中标时间,case Z_HTZT when '未定标废标' then Z_FBSJ else Z_QPKSSJ end 废标时间,Z_SPBH 商品编码,Z_SPMC 商品名称,TBJG 拟售价格,TBNSL 拟售数量 from AAA_ZBDBXXB a left join AAA_TBD b on a.T_YSTBDBH =b.Number where Z_HTZT in ('未定标废标','定标合同终止') and case Z_HTZT when '未定标废标' then Z_FBSJ else Z_QPKSSJ end between '" + ViewState["BeginTime"].ToString() + "' and '" + ViewState["EndTime"].ToString() + "' and a.T_YSTBDDLYX='" + DLYX.Trim() + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Repeater3.DataSource = ds.Tables[0].DefaultView;
            Repeater3.DataBind();
            Tr1.Visible = false;
        }
        else
        {
            Tr1.Visible = true;
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        TabStrip1.Visible = true;
        divLB.Visible = true;
        divXQ.Visible = false;
    }
}