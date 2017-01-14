using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Data;

public partial class Web_HR_RYYD_ZZRY_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DefinedModule Dfmodule = new DefinedModule("HR_RYYDB");
            Authentication auth = Dfmodule.authentication;
            if (auth == null)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }

            if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }          
            SetBM();
        }

    }

    private void SetBM()
    {
        string ls = this.ddlLS.SelectedValue.ToString();
        if (ls == "")
        {
            ddlBM.Items.Clear();
            ddlBM.Items.Add(new ListItem("全部部门", ""));
        }
        else
        {
            ddlBM.Items.Clear();
            ddlBM.DataSource = DbHelperSQL.Query("SELECT distinct  DeptName FROM HR_Dept where superior='" + ls + "'");
            ddlBM.DataTextField = "DeptName";
            ddlBM.DataValueField = "DeptName";
            ddlBM.DataBind();
            ddlBM.Items.Insert(0, new ListItem("全部部门", ""));
        }
    }

    protected void ddlLS_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetBM();
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        tbtitle.Visible = false;
        string StartTime = "";
        string EndTime = "";
        if (this.kssj.Value.Trim() != "")
        {
            StartTime = this.kssj.Value.Trim() + " 0:00:00";
        }
        if (this.jssj.Value.Trim() != "")
        {
            EndTime = this.jssj.Value.Trim() + " 23:59:59";
        }

        if ((StartTime != "" && EndTime == "") || (StartTime == "" && EndTime != ""))
        {
            MessageBox.ShowAlertAndBack(this, "请将开始时间和结束时间输入完整！");
        }
        string ls = ddlLS.SelectedValue.ToString();
        string bm = ddlBM.SelectedValue.ToString();
        lbltitle.Text = kssj.Value.ToString() + "至" + jssj.Value.ToString() + "转正人员名单";
        string sql = "select (row_number() over (order by bm,zzsj)) as 序号, ygbh as 员工工号,xm as 员工姓名,BM as 部门,gw as 岗位,isnull(zzxs,'无') as 转正形式,cast(month(zzsj) as varchar(2))+'月' as 转正月份,convert(varchar(10),zzsj,120) as 转正时间,'' as 备注 from RLZY_YGGX_YGZZGL where ls like '%" + ls + "%' and bm like '%" + bm + "%' and zzsj between '" + StartTime + "' and '" + EndTime + "' order by bm,zzsj";
        DataSet ds = DbHelperSQL.Query(sql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            tbtitle.Visible = true;
        }
        GridView1.DataSource = ds.Tables[0].DefaultView;
        GridView1.DataBind();

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        ToExcel(divDaoChu);
    }
    /// <summary>
    /// 将数据导出到excel，与下面的函数同时使用才能正常工作
    /// </summary>
    /// <param name="ctl"></param>
    public void ToExcel(System.Web.UI.Control ctl)
    {
        //   HttpContext.Current.Response.Charset   ="GB2312";   
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=ZhuanZheng.xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   
        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //base.VerifyRenderingInServerForm(control);   
    }
}