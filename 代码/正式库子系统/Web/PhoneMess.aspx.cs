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
using Hesion.Brick.Core;
using FMOP.DB;


public partial class Web_PhoneMess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // 判断是否勾选
            InitPage();
        }

    }

    private void InitPage()
    {
        string StrSql = "select * from YWPTGRPZ where GH='"+User.Identity.Name.ToString().Trim()+"'";
        DataSet ds = DbHelperSQL.Query(StrSql);
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            if (ds.Tables[0].Rows[0]["SFJSDX"].ToString().Trim() == "接受")
            {
                CbPhone.Checked = true;
            }
        }

    }

    protected void CbPhone_CheckedChanged(object sender, EventArgs e)
    {
        string isReceive = "";
        if (CbPhone.Checked)
        {
            isReceive = "接受";

        }
        else
        {
            isReceive = "不接受";

        }
        string StrisExsist = "select * from YWPTGRPZ where GH='" + User.Identity.Name.ToString() + "'";
        DataSet ds = DbHelperSQL.Query(StrisExsist);
        string Updatestr = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Updatestr = " update YWPTGRPZ set SFJSDX ='" + isReceive + "' where GH ='" + User.Identity.Name.ToString() + "'";
        }
        else
        {

            Updatestr = " insert into  YWPTGRPZ ( SFJSDX ) values(''" + isReceive + "') where GH ='" + User.Identity.Name.ToString() + "'";
        }

        DbHelperSQL.QueryInt(Updatestr);
    }
}
