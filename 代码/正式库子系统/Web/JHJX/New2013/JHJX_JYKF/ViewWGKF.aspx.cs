using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Configuration;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using System.Threading;
using Key;
public partial class Web_JHJX_New2013_JHJX_JYKF_ViewWGKF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Context.Handler.ToString() == "ASP.web_jhjx_new2013_jhjx_jykf_jhjx_jyfwgkfck_aspx" )
        {
            DataRow[] drs = this.Context.Items["Text"] as DataRow[];

            InitialData(drs[0]);
        }           

    }

    protected void InitialData(DataRow dr)
    {
        lbjyfzh.Text = dr["交易方账号"].ToString().Trim();
        lbjyfmc.Text = dr["交易方名称"].ToString().Trim();
        lbjyzhlx.Text = dr["交易账户类型"].ToString().Trim();
        lbzclb.Text = dr["注册类别"].ToString().Trim();
        lblxrxm.Text = dr["联系人"].ToString().Trim();
        lblxrsjh.Text= dr["联系方式"].ToString().Trim();
        lbssqy.Text = dr["所属区域"].ToString().Trim();
        lbssfgs.Text = dr["业务管理部门"].ToString().Trim();
        lbgljjrzh.Text = dr["经纪人账号"].ToString().Trim();
        ddlwgsx.Items.Add(dr["扣罚原因"].ToString().Trim());
        txtjyfkfje.Text = dr["交易方扣罚"].ToString().Trim();
        txtjjrkfje.Text = dr["经纪人扣罚"].ToString().Trim();
        txtqkjs.Text = dr["情况简述"].ToString().Trim();
        string path = keyEncryption.encMe("/Web/JHJX/New2013/JHJX_JYKF/docs/" + dr["扣罚凭证"].ToString().Trim().Replace(@"\", "/"), "mimamima");
        lck.HRef = "../../../picView/picView.aspx?path=" + path+"&jiami=y";
        
       // "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + dr["证明文件路径"].ToString().Replace(@"\", "/");
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        LoginAgain(this);
    }
    // 关闭当前页面并打开前一页面  
    public void LoginAgain(Page page)
    {
        string script = " <script language='javascript'>window.open('JHJX_JYFWGKFCK.aspx','_self');</script>";

        page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseWindow", script);

    }
}