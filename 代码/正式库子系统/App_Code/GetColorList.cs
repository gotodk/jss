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
/// GetColorList 的摘要说明
/// </summary>
public class GetColorList
{
    public ArrayList coloralllist;
	public GetColorList(string number)
	{
        if (string.IsNullOrEmpty(number))
            return;
        coloralllist = getcolorlist(number);

	}
    //所有表名
    private static ArrayList getlist()
    {
        ArrayList list = new ArrayList();
        list.Add("KHGX_KHGL");
        list.Add("CGFZRJBXX");
        // 第五阶段
        list.Add("CGFZRYXX");
        list.Add("DWZFZRYXX");
        list.Add("CWFZRYXX");
        list.Add("CGJCRYXX");
        list.Add("CGFZRJBXXBC1");
        list.Add("CGFZRXGAHYXGXXB");
        list.Add("CGJCRJBXX"); 
        list.Add("CGFZRJBXXBC2");
        list.Add("CGFZRXGAHXGXXBC1"); 
        list.Add("CGFZRJTJBXXB");
        list.Add("CGJCRJBXXBBC1");
        list.Add("CGJCRXGAHYXGXXB");
        list.Add("DWZFZRJBXXB");
        list.Add("CGFZRJBXXBC3");
        list.Add("CGFZRXGAHXGXXBBC2");
        list.Add("CGFZRJTJBXXBBC1");
        list.Add("CGJCRJBXXBBC2");
        list.Add("CGJCRXGAHYXGXXBBC1");
        list.Add("CGJCRJTJBXXB");
        list.Add("DWZFZRJBXXBBC1");
        list.Add("CWFZRJBXX");
        list.Add("CGFZRJBXXBC4");
        list.Add("CGFZRXGXGAHXXBBC3");
        list.Add("CGFZRJTJBXXBBC2");
        list.Add("CGJCRJBXXBC3");
        list.Add("CGJCRXGAHYXGXXBC2");
        list.Add("CGJCRJTJBXXBC1");
        list.Add("DWZFZRJBXXBC2");
        list.Add("DWZFZRXGAHYXGXXB");
        list.Add("CWFZRJBXXBC1");
        //第六阶段 
        list.Add("CGFZRJBXXBC5");
        list.Add("CGFZRXGAHYXGXXBC4");
        list.Add("CGFZRJTJBXXBC3");
        list.Add("CGJCRJBXXBC4");
        list.Add("CGJCRJBXXBC5");
        list.Add("CGJCRXGAHYXGXXBC3");
        list.Add("CGJCRXGAHYXGXXBC4");
        list.Add("CGJCRJTJBXXBC2");
        list.Add("CGJCRJTJBXXBC3");
        list.Add("DWZFZRJBXXBC3");
        list.Add("DWZFZRJBXXBC4");
        list.Add("DWZFZRJBXXBC5");
        list.Add("DWZFXGAHYXGXXBC1");
        list.Add("DWZFXGAHYXGXXBC2");
        list.Add("DWZFXGAHYXGXXBC3");
        list.Add("DWZFXGAHYXGXXBC4");
        list.Add("DWZFZRJTJBXXB");
        list.Add("DWZFZRJTJBXXBBC1");
        list.Add("DWZFZRJTJBXXBBC2");
        list.Add("DWZFZRJTJBXXBBC3");
        list.Add("CWJCRJBXXBC2");
        list.Add("CWJCRJBXXBC3");
        list.Add("CWJCRJBXXBC4");
        list.Add("CWJCRJBXXBC5");
        list.Add("CWJCRXGAHYXGXXB");
        list.Add("CWJCRXGAHYXGXXBBC1");
        list.Add("CWJCRXGAHYXGXXBBC2");
        list.Add("CWJCRXGAHYXGXXBBC3");
        list.Add("CWJCRXGAHYXGXXBBC4");
        list.Add("CWJCRJTJBXXB");
        list.Add("CWJCRJTJBXXBBC1");
        list.Add("CWJCRJTJBXXBBC2");
        list.Add("CWJCRJTJBXXBBC3");
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
            if (columnname != "Number" & columnname != "NextChecker" & columnname != "CheckState" & columnname.ToLower() != "createuser" & columnname.ToLower() != "createtime")
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
