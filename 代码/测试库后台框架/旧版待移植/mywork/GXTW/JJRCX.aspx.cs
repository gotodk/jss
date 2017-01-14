using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class mywork_GXTW_JJRCX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpager1.OnNeedLoadData += new pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);//分类列表的分页
        if(!IsPostBack)
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
        if (ds!=null && ds.Tables[0].Rows.Count>0)
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
            lblJJRZS.Text = NewDS.Tables[0].Rows.Count+"人";
        }
        //控制在没有数据时显示表头和表脚
        else
        {
            tempty.Visible = true;
            Tfoot1.Visible = true;
            lblJJRZS.Text = "0人";
        }
        rpt.DataBind();
    }

    private Hashtable SetV()
    {
        string DLZH = User.Identity.Name;

        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 5";
        ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = "( select a.YXMC 院系,b.J_JJRJSBH 经纪人编号,b.I_JYFMC 名称,b.I_SFZH 身份证号,c.FGSSHSJ 分公司审核时间 from AAA_DLZHXXB b left join  AAA_PTYXB a on a.Number=b.I_YXBH left join AAA_MJMJJYZHYJJRZHGLB c on b.B_DLYX = c.DLYX where c.FGSSHZT='审核通过' and b.B_JSZHLX='经纪人交易账户'  and a.GXZH='" + ViewState["DLZH"].ToString().Trim() + "' ) as tab ";
        ht_where["search_tbname"] = "( select b.I_YXBH 院系,b.J_JJRZGZSBH 经纪人编号,b.I_JYFMC 名称,b.I_SFZH 身份证号,c.FGSSHSJ 分公司审核时间 from AAA_DLZHXXB b left join  AAA_PTGLJGB a on a.GLBMMC=b.I_PTGLJG left join AAA_MJMJJYZHYJJRZHGLB c on b.B_DLYX = c.DLYX where c.FGSSHZT='审核通过' and b.B_JSZHLX='经纪人交易账户'  and a.GLBMZH='" + ViewState["DLZH"].ToString().Trim() + "' ) as tab ";
        ht_where["search_mainid"] = " 经纪人编号 ";
        ht_where["search_str_where"] = " 1=1 ";
        ht_where["search_paixu"] = " DESC ";
        ht_where["search_paixuZD"] = " 分公司审核时间";

        
        if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && !string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString();
            ht_where["search_str_where"] += " and 分公司审核时间 between '" + txtBeginTime.Text.Trim() + "' and '" + endTime.Trim() + "'";
        }
        else if (string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && !string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString();
            ht_where["search_str_where"] += " and 分公司审核时间 <= '" + endTime.Trim() + "'";
        }
        else if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            ht_where["search_str_where"] += " and 分公司审核时间 >= '" + txtBeginTime.Text.Trim() + "' ";
        }
        if(txtJJRBH.Text.Trim()!="")
        {
            ht_where["search_str_where"] += " and 经纪人编号 like '%" + txtJJRBH.Text.Trim() + "%'";
        }
        if(txtJJRXM.Text.Trim()!="")
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
        string filename = "GXTW_JJRXX" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
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
        string str = "select b.I_YXBH 院系,b.J_JJRZGZSBH 经纪人编号,b.I_JYFMC 名称,b.I_SFZH 身份证号,c.FGSSHSJ 分公司审核时间 from AAA_DLZHXXB b left join  AAA_PTGLJGB a on a.GLBMMC=b.I_PTGLJG left join AAA_MJMJJYZHYJJRZHGLB c on b.B_DLYX = c.DLYX where c.FGSSHZT='审核通过' and b.B_JSZHLX='经纪人交易账户'  and a.GLBMZH='" + ViewState["DLZH"].ToString().Trim() + "'";

        
        if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && !string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString();
            str += " and c.FGSSHSJ between '" + txtBeginTime.Text.Trim() + "' and '" + endTime.Trim() + "'";
        }
        else if (string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && !string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            str += " and c.FGSSHSJ <= '" + txtEndTime.Text.Trim() + "'";
        }
        else if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()) && string.IsNullOrEmpty(txtEndTime.Text.Trim()))
        {
            string endTime = Convert.ToDateTime(txtEndTime.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString();
            str += " and c.FGSSHSJ >= '" + endTime.Trim() + "' ";
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
        str += " order by c.FGSSHSJ DESC";
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