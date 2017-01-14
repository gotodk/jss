using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_fmemail_fmemail_FYJ_2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string str_module = Request["module"].ToString();

        if (Session["emaillogin"] != null)
        {
            Session["jiuyici"] = "0";
            switch (str_module)
            {
                case "fmemail_FYJ":
                    //Response.Redirect("http://192.168.0.4:8080/wframe.asp");
                    //gogo_begin();
                    Response.Write("<script>window.parent.frames.rightFrame.location.href = \"http://192.168.0.4:8080/wframe.asp\";</script>");
                    Response.End();
                    break;
                case "fmemail_SJX":
                    //Response.Redirect("http://192.168.0.4:8080/listmail.asp?mode=in");
                    //gogo_begin();
                    Response.Write("<script>window.parent.frames.rightFrame.location.href = \"http://192.168.0.4:8080/listmail.asp?mode=in\";</script>");
                    Response.End();
                    break;
                case "fmemail_CGX":
                    //Response.Redirect("http://192.168.0.4:8080/listmail.asp?mode=out");
                    //gogo_begin();
                    Response.Write("<script>window.parent.frames.rightFrame.location.href = \"http://192.168.0.4:8080/listmail.asp?mode=out\";</script>");
                    Response.End();
                    break;
                case "fmemail_FJX":
                    //Response.Redirect("http://192.168.0.4:8080/listmail.asp?mode=sed");
                    //gogo_begin();
                    Response.Write("<script>window.parent.frames.rightFrame.location.href = \"http://192.168.0.4:8080/listmail.asp?mode=sed\";</script>");
                    Response.End();
                    break;
                case "fmemail_LJX":
                    //Response.Redirect("http://192.168.0.4:8080/listmail.asp?mode=del");
                    //gogo_begin();
                    Response.Write("<script>window.parent.frames.rightFrame.location.href = \"http://192.168.0.4:8080/listmail.asp?mode=del\";</script>");
                    Response.End();
                    break;
                case "fmemail_CZYJ":
                    //Response.Redirect("http://192.168.0.4:8080/findmail.asp");
                    //gogo_begin();
                    Response.Write("<script>window.parent.frames.rightFrame.location.href = \"http://192.168.0.4:8080/findmail.asp\";</script>");
                    Response.End();
                    break;
                case "fmemail_DZB":
                    //Response.Redirect("http://192.168.0.4:8080/ads_brow.asp");
                    //gogo_begin();
                    Response.Write("<script>window.parent.frames.rightFrame.location.href = \"http://192.168.0.4:8080/ads_brow.asp\";</script>");
                    Response.End();
                    break;
                case "fmemail_SZ":
                    //Response.Redirect("http://192.168.0.4:8080/ads_brow.asp");
                    //gogo_begin();
                    Response.Write("<script>window.parent.frames.rightFrame.location.href = \"http://192.168.0.4:8080/user_right.asp\";</script>");
                    Response.End();
                    break;
                default:

                    break;
            }
        }


    }

    public void gogo_begin()
    {
        Response.Write("<script>window.parent.frames.f1.location.href = \"http://192.168.0.4:8080/left_fm.asp\";</script>");
        Response.Write("<script>window.parent.frames.f2.location.href = \"" + Session["mail_url"] + "\";</script>");
    }
}
