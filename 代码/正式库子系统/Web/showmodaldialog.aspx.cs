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

public partial class Web_showmodaldialog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string urltemp = Request["url"].ToString();
        urltemp = urltemp.Replace("[wh]", "?").Replace("[he]", "&").Replace("[dh]", "=");
        kuangjia.InnerHtml = "<iframe src='" + urltemp + "' height='600' width='95%' id='kuangjia0'></iframe>";
    }
}
