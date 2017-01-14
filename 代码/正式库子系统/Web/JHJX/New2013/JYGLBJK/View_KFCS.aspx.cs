using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Configuration;


public partial class Web_JHJX_New2013_JYGLBJK_View_KFCS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string str = Context.Handler.ToString();
            if (str == "ASP.web_jhjx_new2013_jyglbjk_kfcsjk_aspx")
            {
                DataTable dt = this.Context.Items["Text"] as DataTable;

                InitialData(dt);

            }     

        }
              
    }

    protected void    InitialData(DataTable dt)
    {
        if (dt != null)
        {
            Repeater1.DataSource = dt.DefaultView;
            Repeater1.DataBind();
            ts.Visible = false;
        }
        else 
        {
            ts.Visible = true;
        }
    }

   
}