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


public partial class Web_HTowncitylist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        string bm = DbHelperSQL.GetSingle(sql).ToString();
        //if (bm.IndexOf("销售公司") > 0)
      
           
            if (1==1)
            {
                if (bm.IndexOf("办事处") < 0 )
                {
                    Response.Write(1);
                    Response.End();
                }
                else
                {
                    Response.Write(bm);
                    Response.End();
                }
            }
            else
            {
                Response.Write("nothing");
                Response.End();
            }
      
       
    }
}
