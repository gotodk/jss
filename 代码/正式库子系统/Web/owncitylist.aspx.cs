using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;
using FMOP.DB;

public partial class Web_owncitylist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        string bm = DbHelperSQL.GetSingle(sql).ToString();
        //if (bm.IndexOf("销售公司") > 0)
        if (bm.IndexOf("办事处") > 0)
        {
            sql = "select count(name) from system_city where name='" + bm + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                Response.Write(bm);
                Response.End();
            }
            else
            {
                Response.Write("nothing");
                Response.End();
            }
        }
        else
        {
            Response.Write("nothing");
            Response.End();
        }

        //Response.Write("天津销售公司");
        Response.Write("天津办事处");
        Response.End();
    }
}
