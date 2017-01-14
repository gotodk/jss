using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pingtaiservices_moban_ZZCK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["Path"] != null && Request.Params["Path"].ToString() != "")
            {
                this.imgZZ.Src = "../../JHJXPT/SaveDir/" + Request.Params["Path"].ToString().Replace(@"\", "/");
            }
        
        
        }
    }
}