using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_wuccaidan : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string IPCurl = ConfigurationManager.ConnectionStrings["IPCurl"].ToString(); //聚合中心地址
            dizhi.InnerText = IPCurl;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("default.aspx");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('后续再考虑加上，其实还不如直接从数据库查方便！');</script>");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("tongzhigengxin.aspx");
    }
    protected void Button8_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('后续实现，先人工处理！');</script>");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect("addJK.aspx");
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('后续实现，先人工处理！');</script>");
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('后续实现，先人工处理！');</script>");
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        Response.Redirect("addGX.aspx");
    }
    protected void Button2_Click1(object sender, EventArgs e)
    {
        Response.Redirect("allJKlist.aspx");
        
    }
    protected void Button6_Click1(object sender, EventArgs e)
    {
        Response.Redirect("IPlist.aspx");
    }
    protected void Button7_Click1(object sender, EventArgs e)
    {
        Response.Redirect("allFFlist.aspx");
    }
}