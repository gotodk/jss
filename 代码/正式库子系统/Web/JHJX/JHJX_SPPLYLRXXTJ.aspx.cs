using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
public partial class Web_JHJX_JHJX_SPPLYLRXXTJ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
 
        }
    }

    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtStart.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请选择预录入商品开始时间！");
        }
        if (txtEnd.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请选择预录入商品结束时间！");
        }

        DateTime  start =Convert.ToDateTime (txtStart.Text.Trim());
        DateTime end = Convert.ToDateTime(txtEnd.Text.Trim());
        spanTime.InnerText = txtStart.Text.Trim()+"至"+ txtEnd.Text.Trim();

        string sql = "select tab.CreateUser as 预录入操作人工号,b.Employee_name as 预录入操作人,SUM(添加商品品类数量) as 添加商品品类数量,SUM(复核未通过品类数量) as 复核未通过品类数量,SUM(确定上线品类数量) as 确定上线品类数量,CAST(cast(SUM(确定上线品类数量) as numeric(18,4))/cast(SUM(添加商品品类数量) as numeric(18,4))*100  as numeric(18,2))as 商品上线率 from (select CreateUser,1 as 添加商品品类数量,(case when FHWTGSJ is not null and QDSXSJ is null then 1 else 0 end ) as 复核未通过品类数量,(case when QDSXSJ is not null then 1 else 0 end) as  确定上线品类数量 from AAA_SPYLRXXB where convert(varchar(10),createtime,120)>='" + start.ToString("yyyy-MM-dd") + "' and convert(varchar(10),createtime,120)<='" + end.ToString("yyyy-MM-dd") + "' ) as tab  left join  HR_Employees as b on tab.CreateUser =b.Number group by tab.CreateUser,b.Employee_name ";
        DataSet ds = DbHelperSQL.Query(sql);
        ViewState["ds"] = ds;
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            rpt.DataSource = ds.Tables[0].DefaultView;           
            foot.Visible = true;
            lblLRSL.Text = ds.Tables[0].Compute("sum(添加商品品类数量)", "true").ToString();
            lblWTGSL.Text = ds.Tables[0].Compute("sum(复核未通过品类数量)", "true").ToString();
            lblYSXSL.Text = ds.Tables[0].Compute("sum(确定上线品类数量)", "true").ToString();
            lblSXBL.Text = (Convert.ToDouble(lblYSXSL.Text) / Convert.ToDouble(lblLRSL.Text) * 100).ToString("#0.00") + "%";
        }
        else
        {
            tdEmpty.Visible = true;
            foot.Visible = false;
        }
        rpt.DataBind();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToExcel(this.export);
    }
    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=myfiles.xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   
        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    }
    // 导出Exel时需重载此方法
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
}