using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.Common;
using FMOP.DB;

public partial class Web_JHJX_New2013_JYGLBJK_JHJX_WZBTimeJGJK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            //DisGrid();
            TextBox1.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
        ht_where["search_tbname"] = " ( select *,cast(replace(商品编码,'L','') as int)  '排序','最近一次中标日期'=(select top 1 Z_ZBSJ from AAA_ZBDBXXB where Z_SPBH=tab.商品编码 order by Z_ZBSJ desc),'参与买家数量'=(select count(*) from AAA_YDDXXB where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'参与卖家数量'=(select count(*) from AAA_TBD where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'最高拟订购价格'=(select isnull(convert(varchar(20),max(NMRJG)),'--') from AAA_YDDXXB where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'最低拟出售价格'=(select isnull(convert(varchar(20),min(TBJG)),'--') from AAA_TBD where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'拟订购总量'=(select isnull(sum(NDGSL),0) from AAA_YDDXXB where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'拟出售总量'=(select isnull(sum(TBNSL),0) from AAA_TBD where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')) from ( select SPBH 商品编码,SPMC 商品名称,JJDW 计价单位 from AAA_TBD a where a.Number not in (select T_YSTBDBH from AAA_ZBDBXXB where Z_ZBSJ<='" + TextBox1.Text.Trim() + "') and a.TJSJ between '" + startTime + "' and '" + endTime + "' group by SPBH,SPMC,JJDW union all select SPBH 商品编码,SPMC 商品名称,JJDW 计价单位 from AAA_YDDXXB a where a.Number not in ( select Y_YSYDDBH from AAA_ZBDBXXB where Z_ZBSJ<='" + TextBox1.Text.Trim() + "') and a.TJSJ between '" + startTime + "' and '" + endTime + "' group by SPBH,SPMC,JJDW ) as tab group by 商品编码,商品名称,计价单位 ) as tb ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 商品编码 ";  //所检索表的主键
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " 排序 ";  //用于排序的字段
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
            DataSet ds = DbHelperSQL.Query("select '商品编码'='','商品名称'='','计价单位'='','最近一次中标日期'='','参与买家数量'='','参与卖家数量'='','最高拟订购价格'='','最低拟出售价格'='','拟订购总量'='','拟出售总量'='' from AAA_TBD  where 1!=1");
            Repeater1.DataSource = ds.Tables[0].DefaultView;
            ts.Visible = true;

        }
        Repeater1.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        if(txtBeginTime.Text.Trim()=="")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('发布起始时间不能为空！')</script>");
            return;
        }
        if (txtEndTime.Text.Trim() == "")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('发布结束时间不能为空！')</script>");
            return;
        }
        if (TextBox1.Text.Trim() == "")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('截止未中标时间不能为空！')</script>");
            return;
        }
        if (Convert.ToDateTime(txtBeginTime.Text.Trim()) > Convert.ToDateTime(txtEndTime.Text.Trim()))
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('发布结束时间必须大于发布起始时间！')</script>");
            return;
        }
        if (Convert.ToDateTime(txtEndTime.Text.Trim()) > Convert.ToDateTime(TextBox1.Text.Trim()))
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('截止未中标时间必须大于发布结束时间！')</script>");
            return;
        }
        ViewState["BeginTime"] = txtBeginTime.Text.Trim();
        ViewState["EndTime"] = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddMilliseconds(59.998).ToString();
        ViewState["JZTime"] = TextBox1.Text.Trim();
        DisGrid();
    }

    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.Charset = "";
        string filename = "JHJX_WZBTimeJGJK" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
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
            string[] str=e.CommandName.Split('&');
            spanSP.InnerText = "商品编号：" + str[0].Trim() + "　　　商品名称：" + str[1].Trim() + "";
            XQ(str[0].Trim());
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
        if (TextBox1.Text.Trim() == "")
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('截止未中标时间不能为空！')</script>");
            return;
        }
        if (Convert.ToDateTime(txtBeginTime.Text.Trim()) > Convert.ToDateTime(txtEndTime.Text.Trim()))
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('发布结束时间必须大于发布起始时间！')</script>");
            return;
        }
        if (Convert.ToDateTime(txtEndTime.Text.Trim()) > Convert.ToDateTime(TextBox1.Text.Trim()))
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.top.Dialog.alert('截止未中标时间必须大于发布结束时间！')</script>");
            return;
        }
        string startTime = txtBeginTime.Text.Trim();
        string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddMilliseconds(59.998).ToString();
        string str = " select *,cast(replace(商品编码,'L','') as int)  '排序','最近一次中标日期'=(select top 1 Z_ZBSJ from AAA_ZBDBXXB where Z_SPBH=tab.商品编码 order by Z_ZBSJ desc),'参与买家数量'=(select count(*) from AAA_YDDXXB where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'参与卖家数量'=(select count(*) from AAA_TBD where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'最高拟订购价格'=(select isnull(convert(varchar(20),max(NMRJG)),'--') from AAA_YDDXXB where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'最低拟出售价格'=(select isnull(convert(varchar(20),min(TBJG)),'--') from AAA_TBD where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'拟订购总量'=(select isnull(sum(NDGSL),0) from AAA_YDDXXB where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')),'拟出售总量'=(select isnull(sum(TBNSL),0) from AAA_TBD where SPBH=tab.商品编码 and ZT!='撤销' and (TJSJ between '" + startTime + "' and '" + endTime + "')) from ( select SPBH 商品编码,SPMC 商品名称,JJDW 计价单位 from AAA_TBD a where a.Number not in (select T_YSTBDBH from AAA_ZBDBXXB where Z_ZBSJ<='" + TextBox1.Text.Trim() + "') and a.TJSJ between '" + startTime + "' and '" + endTime + "' group by SPBH,SPMC,JJDW union all select SPBH 商品编码,SPMC 商品名称,JJDW 计价单位 from AAA_YDDXXB a where a.Number not in ( select Y_YSYDDBH from AAA_ZBDBXXB where Z_ZBSJ<='" + TextBox1.Text.Trim() + "') and a.TJSJ between '" + startTime + "' and '" + endTime + "' group by SPBH,SPMC,JJDW ) as tab group by 商品编码,商品名称,计价单位  order by cast(replace(商品编码,'L','') as int) asc ";        
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

    protected void XQ(string SPBH)
    {
        Repeater3.DataSource = null;
        Repeater3.DataBind();
        string strSQL = "select * from ( select a.SPBH 商品编号,a.SPMC 商品名称, b.I_ZQZJZH 交易方编号,b.I_SSQYS 所在区域,'单据类别'='投标单',a.HTQX 合同期限,a.TJSJ 发布时间,isnull(a.TBJG,0.00) 价格,isnull(a.TBNSL,0) 数量,a.PTSDZDJJPL 平台设定的经济批量,isnull(convert(varchar(20),a.MJSDJJPL),'--') 卖家设定的经济批量,a.GHQY '收货/发货区域' from AAA_TBD a left join AAA_DLZHXXB b on a.MJJSBH=b.J_SELJSBH where  a.TJSJ between '" + ViewState["BeginTime"].ToString() + "' and '" + ViewState["EndTime"].ToString() + "' and a.ZT!='撤销' union all select a.SPBH 商品编号,a.SPMC 商品名称, b.I_ZQZJZH 交易方编号,b.I_SSQYS 所在区域,'单据类别'='预订单',a.HTQX 合同期限,a.TJSJ 发布时间,isnull(a.NMRJG,0.00) 价格,isnull(a.NDGSL,0) 数量,a.PTSDZDJJPL 平台设定的经济批量,'卖家设定的经济批量'='--',a.SHQYsheng '收货/发货区域' from AAA_YDDXXB a left join AAA_DLZHXXB b on a.MJJSBH=b.J_BUYJSBH where  a.TJSJ between '" + ViewState["BeginTime"].ToString() + "' and '" + ViewState["EndTime"].ToString() + "'  and a.ZT!='撤销') as tab where 商品编号='" + SPBH.Trim() + "' order by 合同期限,单据类别 desc,发布时间";
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
        TextBox1.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }
}