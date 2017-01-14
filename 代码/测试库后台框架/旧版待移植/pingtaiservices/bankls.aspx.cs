using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class pingtaiservices_bankls : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object newnumber = DbHelperSQL.GetSingle("update AAA_bankLS set LWSXH = CONVERT(varchar(50), convert(decimal(6,0),isnull(LWSXH,'0'))+1, 0)         select top 1 right ('000000'+LWSXH,6)  from AAA_bankLS");
        if (newnumber != null)
        {
            string nian = DateTime.Today.ToString("yy");
            string yue = DateTime.Today.ToString("MM");
            string ri = DateTime.Today.ToString("dd");
            Response.Write(nian + yue + ri + newnumber.ToString());
            Response.End();
        }
    }
}