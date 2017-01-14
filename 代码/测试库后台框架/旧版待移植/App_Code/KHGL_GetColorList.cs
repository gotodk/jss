using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using FMOP.DB;
using System.Text;

/// <summary>
///KHGL_GetColorList 的摘要说明
/// </summary>
public class KHGL_GetColorList
{
	
    public ArrayList coloralllist;
	public KHGL_GetColorList(string number)
	{
        if (string.IsNullOrEmpty(number))
            return;
        coloralllist = getcolorlist(number);

	}
    //所有表名
    private static ArrayList getlist()
    {
        ArrayList list = new ArrayList();
        list.Add("KHGL_New");
        list.Add("KHGL_FXSJBXX");
        list.Add("KHGL_ZDYHJBXX");
        // 拓展信息
        list.Add("KHGL_CGFZRXX");
        list.Add("KHGL_CGFZRJTXX");
        list.Add("KHGL_DWFZRXX");
        list.Add("KHGL_DWFZRJTXX");
        list.Add("KHGL_CGZXRXX");
        list.Add("KHGL_CGZXRJTXX");
        list.Add("KHGL_CWFZRXX"); 
        list.Add("KHGL_CWFZRJTXX");
        list.Add("KHGL_BGSFZRXX"); 
        list.Add("KHGL_BGSFZRJTXX");
        list.Add("KHGL_YWLXRXX");
        list.Add("KHGL_YWLXRJTXX");
        list.Add("KHGL_SHRXX");
        list.Add("KHGL_SHRJTXX");
        list.Add("KHGL_DYJXX");
        list.Add("KHGL_XGSYXX");
        list.Add("KHGL_ZBXX");
        list.Add("KHGL_GYSXX");
        list.Add("KHGL_FWXQXX"); 
        return list;
    }
    private ArrayList getcolorlist(string number)
    {
        ArrayList colorlist = new ArrayList();
        ArrayList tablelist = getlist();
        for (int i = 0; i < tablelist.Count; i++)
        { 
            string color=getcheckRowNull(tablelist[i].ToString(),number);
            if ((color) != "")
                colorlist.Add(color);
            else
                colorlist.Add(getcheckColumnNull(tablelist[i].ToString(),number));

        }
        return colorlist;
    }
    /// <summary>
    /// 表中是否有记录
    /// </summary>
    /// <param name="tablename">表名</param>
    /// <param name="number">会员编号</param>
    /// <returns></returns>
    private string getcheckRowNull(string tablename, string number)
    {
        string color = "";
        string sql = "select number from " + tablename + " where number='" + number + "'";
        DataSet ds= DbHelperSQL.Query(sql);
        if (ds.Tables[0].Rows.Count <= 0)
            color = "#ff0033";
        return color;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tablename"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    private string getcheckColumnNull(string tablename, string number)
    {
        string color = "";
        string sql_column = "select   a.name   from   syscolumns   a,   systypes   b   where   a.xtype=b.xusertype   and   a.id=object_id('" + tablename + "')   and   b.name!='text'";
        DataSet ds_column = DbHelperSQL.Query(sql_column);
        StringBuilder sql = new StringBuilder();
        sql.Append("select number from " + tablename + " where number='" + number + "'  ");
        for (int i = 0; i < ds_column.Tables[0].Rows.Count;i++ )
        {
            string columnname=ds_column.Tables[0].Rows[i][0].ToString();
            if (columnname != "Number" & columnname != "NextChecker" & columnname != "CheckState" & columnname.ToLower() != "createuser" & columnname.ToLower() != "createtime" & columnname .ToLower ()!="changeuser" & columnname .ToLower ()!="changetime" & columnname !="KHMM"&columnname .ToLower ()!="fcreatetime"&columnname .ToLower ()!="fcreateuser"&columnname .ToLower ()!="lupdatetime"&columnname .ToLower ()!="lupdateuser"&columnname .ToLower ()!="ip")
            {
                sql.Append(" and ");
                sql.Append(" (" + columnname + " is not null and " + columnname + "!='') ");
            }
        }
        DataSet getds = DbHelperSQL.Query(sql.ToString());
        if (getds.Tables[0].Rows.Count > 0)
            color = "#009966";
        else
            color = "#ffcc66";
        return color;
    }

    public ArrayList getcoloralllist
    {
        get
        {
            return coloralllist;
        }
    }    
}
