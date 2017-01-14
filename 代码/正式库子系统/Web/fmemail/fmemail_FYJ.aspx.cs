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

public partial class Web_fmemail_fmemail_FYJ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string str_module = Request["module"].ToString();

        if (Session["emaillogin"] != null)
        {
            gogo_begin();
            if (Session["jiuyici"] == null)
            {
                Response.Write("<script>setTimeout(\"test_gogo()\",1000);function test_gogo(){window.parent.frames.rightFrame.location.href = \"fmemail_FYJ_2.aspx?module=" + str_module + "\";}</script>");
            }
            else
            {
                Response.Write("<script>test_gogo();function test_gogo(){window.parent.frames.rightFrame.location.href = \"fmemail_FYJ_2.aspx?module=" + str_module + "\";}</script>");
            }

        }
        
        
    }

    public void gogo_begin()
    {
        Response.Write("<script>window.parent.frames.f1.location.href = \"http://192.168.0.7:8080/left_fm.asp\";</script>");
        Response.Write("<script>window.parent.frames.f2.location.href = \"" + Session["mail_url"] + "\";</script>");
    }
}
