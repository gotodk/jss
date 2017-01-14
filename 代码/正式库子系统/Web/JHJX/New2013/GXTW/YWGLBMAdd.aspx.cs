using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_New2013_GXTW_YWGLBMAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ViewState["Number"] = "";
            if (Request.QueryString["Number"] != null && Request.QueryString["Number"].ToString() != "")
            {
                ViewState["Number"] = Request.QueryString["Number"].ToString();
                Tab2.Text = "业务管理部门修改";
                Bind();
                btnUpdate.Visible = true;
                btnGoBack.Visible = true;
                Button7.Visible = false;
            }
            else
            {
                Tab2.Text = "业务管理部门添加";
                btnUpdate.Visible = false;
                btnGoBack.Visible = false;
                Button7.Visible = true;
            }
            
        }
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        if(txtGLBMMC.Text.Trim()==""||txtGLBMMM.Text.Trim()==""||txtGLBMZH.Text.Trim()=="")
        {
         this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('请将信息添加完整！');</script>");
         return;
        }
        if (txtGLBMMC.Text.Trim().Length > 50)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('管理部门名称过长，请重新输入！');</script>");
            return;
        }
        if (txtGLBMMM.Text.Trim().Length > 50)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('管理部门密码过长，请重新输入！');</script>");
            return;
        }
        if (txtGLBMZH.Text.Trim().Length > 50)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('管理部门账号过长，请重新输入！');</script>");
            return;
        }
        if (txtGLBMMC.Text.Trim().Contains("'"))
        {
            txtGLBMMC.Text = txtGLBMMC.Text.Replace("'", "‘");
        }
        if (txtGLBMMM.Text.Trim().Contains("'"))
        {
            txtGLBMMM.Text = txtGLBMMM.Text.Replace("'", "‘");
        }
        if (txtGLBMZH.Text.Trim().Contains("'"))
        {
            txtGLBMZH.Text = txtGLBMZH.Text.Replace("'", "‘");
        }
        string str = "select * from AAA_PTGLJGB where GLBMMC='" + txtGLBMMC.Text.Trim() + "' or GLBMZH='" + txtGLBMZH.Text.Trim() + "'";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds!=null && ds.Tables[0].Rows.Count>0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('管理部门名称或管理部门帐号已存在！');</script>");
            return;
        }
        WorkFlowModule WFM = new WorkFlowModule("AAA_PTGLJGB");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        string strInsert = "INSERT INTO [AAA_PTGLJGB] ([Number],[GLBMMC],[GLBMZH],[GLBMMM],[GLBMFLMC],[SFYX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber + "' ,'" + txtGLBMMC.Text.Trim() + "' ,'" + txtGLBMZH.Text.Trim() + "','" + txtGLBMMM.Text.Trim() + "','" + drpGLBMFL.SelectedValue.Trim() + "','" + drpSFYX.SelectedValue.Trim() + "',1,'" + User.Identity.Name.ToString().Trim() + "',getDate(),getDate())";
        int i = DbHelperSQL.ExecuteSql(strInsert);
        if (i > 0)
        {
            string url = "New2013/GXTW/YWGLBMAdd.aspx";
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('添加成功！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;  });</script>");            
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('添加失败！');</script>");
        }

    }

    protected void Bind()
    {
        string number=ViewState["Number"].ToString();
        string str = "select * from AAA_PTGLJGB where Number='" + number.Trim() + "'";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null && ds.Tables[0].Rows.Count>0)
        {
            drpSFYX.SelectedValue = ds.Tables[0].Rows[0]["GLBMFLMC"].ToString();
            txtGLBMMC.Text = ds.Tables[0].Rows[0]["GLBMMC"].ToString();
            txtGLBMZH.Text = ds.Tables[0].Rows[0]["GLBMZH"].ToString();
            txtGLBMMM.Text = ds.Tables[0].Rows[0]["GLBMMM"].ToString();
            drpSFYX.SelectedValue = ds.Tables[0].Rows[0]["SFYX"].ToString();
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string number=ViewState["Number"].ToString();
        if (txtGLBMMC.Text.Trim() == "" || txtGLBMMM.Text.Trim() == "" || txtGLBMZH.Text.Trim() == "")
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('请将信息添加完整！');</script>");
            return;
        }
        if (txtGLBMMC.Text.Trim().Length > 50)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('管理部门名称过长，请重新输入！');</script>");
            return;
        }
        if (txtGLBMMM.Text.Trim().Length > 50)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('管理部门密码过长，请重新输入！');</script>");
            return;
        }
        if (txtGLBMZH.Text.Trim().Length > 50)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('管理部门账号过长，请重新输入！');</script>");
            return;
        }
        if (txtGLBMMC.Text.Trim().Contains("'"))
        {
            txtGLBMMC.Text = txtGLBMMC.Text.Replace("'", "‘");
        }
        if (txtGLBMMM.Text.Trim().Contains("'"))
        {
            txtGLBMMM.Text = txtGLBMMM.Text.Replace("'", "‘");
        }
        if (txtGLBMZH.Text.Trim().Contains("'"))
        {
            txtGLBMZH.Text = txtGLBMZH.Text.Replace("'", "‘");
        }
        string str = "select * from AAA_PTGLJGB where (GLBMMC='" + txtGLBMMC.Text.Trim() + "' or GLBMZH='" + txtGLBMZH.Text.Trim() + "') and Number!='" + number + "'";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('管理部门名称或管理部门帐号已存在！');</script>");
            return;
        }
        string strInsert = "UPDATE [AAA_PTGLJGB] SET [GLBMMC] = '" + txtGLBMMC.Text.Trim() + "',[GLBMZH] = '" + txtGLBMZH.Text.Trim() + "' ,[GLBMMM] = '" + txtGLBMMM.Text.Trim() + "' ,[GLBMFLMC] = '" + drpGLBMFL.SelectedValue.Trim() + "',[SFYX] = '" + drpSFYX.SelectedValue.Trim() + "' ,[NextChecker] = '" + User.Identity.Name.Trim() + "' ,[CheckLimitTime] = getDate() WHERE [Number] ='" + number.Trim() + "'";
        int i = DbHelperSQL.ExecuteSql(strInsert);
        if (i > 0)
        {
            string url = "New2013/GXTW/YWGLBMCK.aspx";
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('修改成功！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;  });</script>");
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('修改失败！');</script>");
        }
    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("YWGLBMCK.aspx");
    }
}