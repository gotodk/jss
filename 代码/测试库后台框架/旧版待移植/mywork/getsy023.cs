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

public partial class mywork_getsy023 : System.Web.UI.Page
{
    public string  str = "";
   
    protected string CutString(string SourseString, int c)
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

        DataSet ds = DbHelperSQL.Query("select TOP 10  XXNR  from   JYPT_tsinfoshow  order by TJSJ ");
        if (ds == null || ds.Tables.Count < 0)
        {
            str = "<span>暂时没有要展示的数据！</span>";
        }
        else
        {
            str = "";
              string tas="";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                tas = dr["XXNR"].ToString();
                if (tas.Length > 20)
                {
                    tas = tas.Remove(19) + "......";
                }
                str += "<span>·&nbsp;" + tas + "&nbsp;&nbsp;&nbsp;&nbsp;</span> ";
            }
        }
       
    }
}