using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;

public partial class mywork_getsy013_ : System.Web.UI.Page
{
    protected string CutString(string SourseString,int c)
    {
        if (SourseString.Length > c)
        {
            return SourseString.Substring(0, c) + "";
        }
        else
        {
            return SourseString;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string p = Request["p"].ToString();

        DataSet ds = DbHelperSQL.Query("select top 10 * ,(case when xuhao < 4 then 'ziti_juhong' else '' end) as 特殊样式a from AAA_HomeTopFM8844 where leixing = '" + p + "' order by xuhao asc");
        if (ds == null || ds.Tables.Count < 0)
        {
            return;
        }
        Repeater1.DataSource = ds.Tables[0].DefaultView;
        Repeater1.DataBind();
    }
}