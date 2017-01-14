using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls;
using FMOP.DB;
using System.Data.SqlClient;

public partial class Web_FWSCK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["fwsbh"] = "";
            if (Request["number"] != null && Request["number"].ToString() != "")
            {
                string sql = "select fwsbh from YHZTC_KTQWBSQ where number='"+Request["number"].ToString()+"'";
                SqlDataReader reader = DbHelperSQL.ExecuteReader(sql);
                if (reader.Read())
                {
                    ViewState["fwsbh"] = reader["fwsbh"].ToString();
                }
                reader.Close();
            }
        }
    }
}