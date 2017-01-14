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

public partial class Web_txlj : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string url = Request.QueryString["url"].ToString();
            string id = Request.QueryString["id"].ToString();
            string strsql = "update user_warnings set isread='true' where id=" + id;
            DbHelperSQL.ExecuteSql(strsql);
            Response.Redirect(url);
        }
    }
}
