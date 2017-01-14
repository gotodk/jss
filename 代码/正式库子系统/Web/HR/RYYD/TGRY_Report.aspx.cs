using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Data;

public partial class Web_HR_RYYD_TGRY_Report : System.Web.UI.Page
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
        string ls = this.ddlYLS.SelectedValue.ToString();
        if (ls == "")
        {
            ddlYBM.Items.Clear();
            ddlYBM.Items.Add(new ListItem("全部部门", ""));

            ddlXBM.Items.Clear();
            ddlXBM.Items.Add(new ListItem("全部部门", ""));
        }
        else
        {
            DataSet dsBM=DbHelperSQL.Query("SELECT distinct  DeptName FROM HR_Dept where superior='" + ls + "'");
            ddlYBM.Items.Clear();
            ddlYBM.DataSource = dsBM.Tables[0];
            ddlYBM.DataTextField = "DeptName";
            ddlYBM.DataValueField = "DeptName";
            ddlYBM.DataBind();
            ddlYBM.Items.Insert(0, new ListItem("全部部门", ""));

            ddlXBM.Items.Clear();
            ddlXBM.DataSource = dsBM.Tables[0];
            ddlXBM.DataTextField = "DeptName";
            ddlXBM.DataValueField = "DeptName";
            ddlXBM.DataBind();
            ddlXBM.Items.Insert(0, new ListItem("全部部门", ""));

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
        string yls = ddlYLS.SelectedValue.ToString();
        string ybm = ddlYBM.SelectedValue.ToString();
        string xls = ddlXLS.SelectedValue.ToString();
        string xbm = ddlXBM.SelectedValue.ToString();

        lbltitle.Text = kssj.Value.ToString() + "至" + jssj.Value.ToString() + "调岗人员名单";      
        string sql = "select (row_number() over (order by ybm,dgsj)) as 序号, ygbh as 员工工号,xm as 员工姓名,YBM as 原部门,ygw as 原岗位,tgqrzzt as 原任职状态,twbm as 部门,twgw as 岗位,tgrzzt as 任职状态,cast(month(dgsj) as varchar(2))+'月' as 调岗月份,convert(varchar(10),dgsj,120) as 调岗时间,'' as 备注 from RLZY_YGGX_TGGL where isnull(yls,'') like '%"+yls+"%' and ybm like '%" + ybm + "%' and isnull(xls,'') like '%"+xls+"%' and twbm like '%"+xbm+"%' and dgsj between '" + StartTime + "' and '" + EndTime + "' order by ybm,dgsj";
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
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=TiaoGang.xls");

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
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            TableCellCollection tcHeader = e.Row.Cells;
         

            //第二行表头 
            tcHeader[0].Attributes.Add("rowspan", "2");
            tcHeader[1].Attributes.Add("rowspan", "2");
            tcHeader[2].Attributes.Add("rowspan", "2");
            tcHeader[3].Attributes.Add("colspan", "3");
            tcHeader[3].Text = "调整前岗位";
            tcHeader[4].Attributes.Add("colspan", "3");
            tcHeader[4].Text = "调整后岗位";
            tcHeader[5].Attributes.Add("rowspan", "2");
            tcHeader[5].Text = tcHeader[9].Text;
            tcHeader[6].Attributes.Add("rowspan", "2");
            tcHeader[6].Text = tcHeader[10].Text;
            tcHeader[7].Attributes.Add("rowspan", "2");
            tcHeader[7].Text = tcHeader[11].Text + "</th></tr><tr style=\"background-color:LightGrey;font-size:10pt;height:25px;\"><th align=\"center\"><!--";
            //第二行表头
            tcHeader[8].Text = "-->原部门";
            tcHeader[9].Text = "原岗位";
            tcHeader[10].Text = "任职状态";          
            tcHeader[11].Text = "部门";
            tcHeader.Add(new TableHeaderCell());
            tcHeader[12].Text = "岗位";
            tcHeader.Add(new TableHeaderCell());
            tcHeader[13].Text = "任职状态"; 
        }

    }
}