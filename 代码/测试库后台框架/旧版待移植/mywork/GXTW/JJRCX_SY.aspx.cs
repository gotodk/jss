using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class mywork_GXTW_JJRCX_SY : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpager1.OnNeedLoadData += new pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);//分类列表的分页
        if (!IsPostBack)
        {
            ViewState["DLZH"] = "";
            HttpCookie cookie = Request.Cookies["username"];
            if (cookie != null)
            {
                ViewState["DLZH"] = HttpUtility.UrlDecode(cookie.Value);
            }
            BindYX();
            Bind();
        }
    }

    protected void BindYX()
    {
        //string str = "select YXMC from AAA_PTYXB where GXZH='" + ViewState["DLZH"].ToString().Trim() + "'";
        string str = "select distinct I_YXBH from AAA_DLZHXXB where I_PTGLJG =(select GLBMMC from AAA_PTGLJGB  where GLBMZH='" + ViewState["DLZH"].ToString().Trim() + "')";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            ddrJJRYX.Items.Clear();
            ddrJJRYX.Items.Add("请选择院系");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(row["I_YXBH"].ToString().Trim()))
                {
                    ddrJJRYX.Items.Add(row["I_YXBH"].ToString().Trim());
                }
                
            }
        }
    }

    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        rpt.DataSource = null;

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            tempty.Visible = false;
            rpt.DataSource = NewDS;
            lblJJRSYZJE.Text = NewDS.Tables[0].Rows[0]["当前经纪人收益总额"].ToString() + "元";
        }
        //控制在没有数据时显示表头和表脚
        else
        {
            tempty.Visible = true;
            Tfoot1.Visible = true;
            lblJJRSYZJE.Text = "0.00元";
        }
        rpt.DataBind();
    }

    private Hashtable SetV()
    {
        string DLZH = User.Identity.Name;

        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 5";
        ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = "( select d.Number 账款流水Number, b.J_JJRJSBH 经纪人编号,b.I_JYFMC 名称,a.YXMC 院系,d.LSCSSJ 收益产生时间,(case d.XZ when '来自买方收益' then '买家' when '来自卖方收益' then '卖家' else '' end) 收益产生来源,isnull(d.JE,0.00) 收益金额,d.YSLX ,'当前收益总额'=(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where XM='经纪人收益' and SJLX='预' and  LSCSSJ <=d.LSCSSJ and JSBH=b.J_JJRJSBH),'当前经纪人收益总额'=(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where XM='经纪人收益' and SJLX='预' and JSBH in (select b1.J_JJRJSBH from AAA_DLZHXXB b1 left join  AAA_PTYXB a1 on a1.Number=b1.I_YXBH left join AAA_MJMJJYZHYJJRZHGLB c1 on b1.B_DLYX = c1.DLYX where c1.FGSSHZT='审核通过' and b1.B_JSZHLX='经纪人交易账户' and a1.GXZH='" + ViewState["DLZH"].ToString().Trim() + "')) from AAA_DLZHXXB b left join  AAA_PTYXB a on a.Number=b.I_YXBH left join AAA_MJMJJYZHYJJRZHGLB c on b.B_DLYX = c.DLYX left join AAA_ZKLSMXB d on d.JSBH=b.J_JJRJSBH where c.FGSSHZT='审核通过' and b.B_JSZHLX='经纪人交易账户' and d.XM='经纪人收益' and d.SJLX='预' and a.GXZH='" + ViewState["DLZH"].ToString().Trim() + "' ) as tab ";
        ht_where["search_tbname"] = "( select d.Number 账款流水Number, b.J_JJRZGZSBH 经纪人编号,b.I_JYFMC 名称,b.I_YXBH 院系,d.LSCSSJ 收益产生时间,(case d.XZ when '来自买方收益' then '买方' when '来自卖方收益' then '卖方' else '' end) 收益产生来源,isnull(d.JE,0.00) 收益金额,d.YSLX ,'当前收益总额'=(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where XM='经纪人收益' and SJLX='预' and  LSCSSJ <=d.LSCSSJ and JSBH=b.J_JJRJSBH),'当前经纪人收益总额'=(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where XM='经纪人收益' and SJLX='预' and JSBH in (select b1.J_JJRJSBH from AAA_DLZHXXB b1 left join  AAA_PTGLJGB a1 on a1.GLBMMC=b1.I_PTGLJG left join AAA_MJMJJYZHYJJRZHGLB c1 on b1.B_DLYX = c1.DLYX where c1.FGSSHZT='审核通过' and b1.B_JSZHLX='经纪人交易账户' and a1.GLBMZH='" + ViewState["DLZH"].ToString().Trim() + "'))  from AAA_DLZHXXB b left join AAA_PTGLJGB a on a.GLBMMC=b.I_PTGLJG left join AAA_MJMJJYZHYJJRZHGLB c on b.B_DLYX = c.DLYX left join AAA_ZKLSMXB d on d.JSBH=b.J_JJRJSBH where c.FGSSHZT='审核通过' and b.B_JSZHLX='经纪人交易账户' and d.XM='经纪人收益' and d.SJLX='预' and a.GLBMZH='" + ViewState["DLZH"].ToString().Trim() + "' ) as tab ";
        ht_where["search_mainid"] = " 账款流水Number ";
        ht_where["search_str_where"] = " 1=1 ";
        ht_where["search_paixu"] = " DESC ";
        ht_where["search_paixuZD"] = " 收益产生时间";

        if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && !string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString();
            ht_where["search_str_where"] += " and 收益产生时间 between '" + txtBeginTime.Text.Trim() + "' and '" + endTime.Trim() + "'";
        }
        else if (string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && !string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString();
            ht_where["search_str_where"] += " and 收益产生时间 <= '" + endTime.Trim() + "'";
        }
        else if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {            
            ht_where["search_str_where"] += " and 收益产生时间 >= '" + txtBeginTime.Text.Trim() + "' ";
        }
        if (txtJJRBH.Text.Trim() != "")
        {
            ht_where["search_str_where"] += " and 经纪人编号 like '%" + txtJJRBH.Text.Trim() + "%'";
        }
        if (txtJJRXM.Text.Trim() != "")
        {
            ht_where["search_str_where"] += " and 名称 like '%" + txtJJRXM.Text.Trim() + "%'";
        }
        if (ddrJJRYX.SelectedValue.Trim() != "" && ddrJJRYX.SelectedValue.Trim() != "请选择院系")
        {
            ht_where["search_str_where"] += " and 院系='" + ddrJJRYX.SelectedValue.Trim() + "'";
        }

        return ht_where;
    }

    protected void Bind()
    {
        Hashtable HTwhere = SetV();
        commonpager1.HTwhere = HTwhere;
        commonpager1.GetFYdataAndRaiseEvent();
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        Bind();
    }

    #region//导出
    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.Charset = "";
        string filename = "GXTW_JJRXXSYCX" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
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
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        //string str = "select d.Number 账款流水Number, b.J_JJRJSBH 经纪人编号,b.I_JYFMC 名称,a.YXMC 院系,d.LSCSSJ 收益产生时间,(case d.XZ when '来自买方收益' then '买家' when '来自卖方收益' then '卖家' else '' end) 收益产生来源,isnull(d.JE,0.00) 收益金额,d.YSLX ,'当前收益总额'=(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where XM='经纪人收益' and SJLX='预' and  LSCSSJ <=d.LSCSSJ and JSBH=b.J_JJRJSBH) from AAA_DLZHXXB b left join  AAA_PTYXB a on a.Number=b.I_YXBH left join AAA_MJMJJYZHYJJRZHGLB c on b.B_DLYX = c.DLYX left join AAA_ZKLSMXB d on d.JSBH=b.J_JJRJSBH where c.FGSSHZT='审核通过' and b.B_JSZHLX='经纪人交易账户' and d.XM='经纪人收益' and a.GXZH='" + ViewState["DLZH"].ToString().Trim() + "'";

        string str = "select d.Number 账款流水Number, b.J_JJRZGZSBH 经纪人编号,b.I_JYFMC 名称,b.I_YXBH 院系,d.LSCSSJ 收益产生时间,(case d.XZ when '来自买方收益' then '买方' when '来自卖方收益' then '卖方' else '' end) 收益产生来源,isnull(d.JE,0.00) 收益金额,d.YSLX ,'当前收益总额'=(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where XM='经纪人收益' and SJLX='预' and  LSCSSJ <=d.LSCSSJ and JSBH=b.J_JJRJSBH)  from AAA_DLZHXXB b left join AAA_PTGLJGB a on a.GLBMMC=b.I_PTGLJG left join AAA_MJMJJYZHYJJRZHGLB c on b.B_DLYX = c.DLYX left join AAA_ZKLSMXB d on d.JSBH=b.J_JJRJSBH where c.FGSSHZT='审核通过' and b.B_JSZHLX='经纪人交易账户' and d.XM='经纪人收益' and d.SJLX='预' and a.GLBMZH='" + ViewState["DLZH"].ToString().Trim() + "'";

        if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && !string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString();
            str += " and d.LSCSSJ between '" + txtBeginTime.Text.Trim() + "' and '" + endTime.Trim() + "'";
        }
        else if (string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && !string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString();
            str += " and d.LSCSSJ <= '" + endTime.Trim() + "'";
        }
        else if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            str += " and d.LSCSSJ >= '" + txtBeginTime.Text.Trim() + "' ";
        }
        if (txtJJRBH.Text.Trim() != "")
        {
            str += " and b.J_JJRZGZSBH like '%" + txtJJRBH.Text.Trim() + "%'";
        }
        if (txtJJRXM.Text.Trim() != "")
        {
            str += " and b.I_JYFMC like '%" + txtJJRXM.Text.Trim() + "%'";
        }
        if (ddrJJRYX.SelectedValue.Trim() != "" && ddrJJRYX.SelectedValue.Trim() != "请选择院系")
        {
            str += " and b.I_YXBH='" + ddrJJRYX.SelectedValue.Trim() + "'";
        }
        str += " order by d.LSCSSJ desc";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Tfoot1.Visible = false;
            Repeater1.DataSource = ds.Tables[0].DefaultView;
            Repeater1.DataBind();

        }
        else
        {
            Tfoot1.Visible = true;
        }
        Bind();
        ToExcel(export);
    }

    #endregion
}