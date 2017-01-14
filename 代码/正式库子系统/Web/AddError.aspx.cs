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

public partial class Web_AddError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Labmod.Text = Request.QueryString["module"].ToString();
            LabNumber.Text = Request.QueryString["number"].ToString();
            Worning.Text = "输入的URl不正确！请检查你输入的Url";
            
        }
    }
}
