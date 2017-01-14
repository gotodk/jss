using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_XXHZX_view_mzpy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;
        //验证权限
        DefinedModule module = new DefinedModule("XXHZX_ViewMZPY");
        Authentication auth = module.authentication;
        if (auth == null)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }
        UserModuleAuth moduleAuth = auth.GetAuthByUserNumber(User.Identity.Name);
        if (!moduleAuth.CanModify)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }

        DataSet ds = DbHelperSQL.Query("select 被评人姓名,评分人姓名,计划组织与协调能力,发现问题和解决问题能力,专业知识和技能,创新能力,学习能力,沟通能力 from AAAAAAAAAAAAAAAAA order by 被评人姓名");
        GridView1.DataSource = ds.Tables[0].DefaultView;
        GridView1.DataBind();
    }

    public void ToExcel(System.Web.UI.Control ctl)
    {
        //   HttpContext.Current.Response.Charset   ="GB2312";   
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        ToExcel(GridView1);
    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //base.VerifyRenderingInServerForm(control);   
    }


    protected void Bttton_qingkong(object sender, EventArgs e)
    {
        DbHelperSQL.ExecuteSql("update AAAAAAAAAAAAAAAAA set 计划组织与协调能力='0',发现问题和解决问题能力='0',专业知识和技能='0',创新能力='0',学习能力='0',沟通能力='0'");
        MessageBox.ShowAndRedirect(this, "清空成功", "view_mzpy.aspx");
    }
}
