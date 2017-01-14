using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// 绑定数据控件
/// </summary>
public class DBControl
{
	public DBControl()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    #region 执行数据控件的绑定
    /// <summary>
    /// DataList的简单绑定
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="mydatalist">绑定的控件名</param>
    public static void BindDataList(string sql, string tablename, DataList mydatalist)
    {
        DataSet ds =Data.ExecuteDataSet(sql, tablename);
        mydatalist.DataSource = ds.Tables[0].DefaultView;
        mydatalist.DataBind();
    }

    /// <summary>
    /// Repeater的简单绑定
    /// </summary>
    /// <param name="sql"> sql语句</param>
    /// <param name="mydatalist">绑定的控件名</param>
    public static void BindRepeater(string sql, string tablename, Repeater myrepeater)
    {
        DataSet ds = Data.ExecuteDataSet(sql, tablename);
        myrepeater.DataSource = ds.Tables[0].DefaultView;
        myrepeater.DataBind();
    }

    /// <summary>
    /// 绑定DropDownList控件（注：四个函数,该函数需要一个字段名，分别绑定Value和Text两值，默认表名）
    /// </summary>
    /// <param name="str_Text">text值</param>
    /// <param name="sql">sql语句</param>
    /// <param name="myDropDownList">控件名</param>
    public static void BindDropDownList(string str_Text, string sql, string tablename, DropDownList myDropDownList)
    {
        DataSet ds = Data.ExecuteDataSet(sql, tablename);
        if (ds != null)
        {
            myDropDownList.DataSource = ds.Tables["binddropdownlist" + sql.ToString()].DefaultView;
        }
        else
        {
            myDropDownList.DataSource = ds.Tables[0].DefaultView;
        }
        myDropDownList.DataValueField = str_Text;
        myDropDownList.DataTextField = str_Text;
        myDropDownList.DataBind();
        myDropDownList.ClearSelection();
    }

    /// <summary>
    /// 绑定DropDownList控件并显示数据,DropDownList控件Value,Text值将分别等于等于str_Value,str_Text值
    /// </summary>
    /// <param name="str_Value">绑定DropDownList控件Value值相对应数据库表字段名</param>
    /// <param name="str_Text">绑定DropDownList控件Text值相对应数据库表字段名</param>
    /// <param name="sql">Select-SQL语句</param>
    /// <param name="myDropDownList">DropDownList控件id值</param>
    public static void BindDropDownList(string str_Value, string str_Text, string sql, string tablename, DropDownList myDropDownList)
    {
        DataSet ds = Data.ExecuteDataSet(sql, tablename);
        myDropDownList.DataSource = ds.Tables[0].DefaultView;
        myDropDownList.DataValueField = str_Value;
        myDropDownList.DataTextField = str_Text;
        myDropDownList.DataBind();
        myDropDownList.Items.Insert(0, "");
        myDropDownList.Items[0].Value = "";
        if (myDropDownList.Items.Count == 0)
        {
            ListItem li_null = new ListItem("无", "0");
            myDropDownList.Items.Add(li_null);
        }
    }
    /// <summary>
    /// 绑定DropDownList控件，取得选中值
    /// </summary>
    /// <param name="str_Value">数据库表示Value值字段</param>
    /// <param name="str_Text">数据库表示Text值字段</param>
    /// <param name="str_Value_Field">选中项目的值</param>
    /// <param name="str_Sql">绑定数据的SQL语句</param>
    /// <param name="myDropDownList">下拉列表框的名称</param>
    public static void SelectBindDropDownListValue(string str_Value, string str_Text, string str_Value_Field, string str_Sql, DropDownList myDropDownList)
    {
        BindDropDownList(str_Value, str_Text, str_Sql, myDropDownList);// 绑定myDropDownList控件
        myDropDownList.Items[0].Selected = false;
        for (int i = 0; i < myDropDownList.Items.Count; i++)
        {
            if (str_Value_Field == myDropDownList.Items[i].Value)
            {
                myDropDownList.Items[i].Selected = true;
                break;
            }
        }
    }
    /// <summary>
    /// 绑定ListBox控件的无限级分类需要配合BindListNode
    /// </summary>
    /// <param name="parentid">绑定的节点,从0开始或者指定绑定编号</param>
    /// <param name="str_value">>数据库表示Value值字段</param>
    /// <param name="str_text">数据库表示Text值字段</param>
    /// <param name="str_parentid">数据库表示子类id 0为跟栏目</param>
    /// <param name="str_sql">绑定数据的SQL语句</param>
    /// <param name="str_msg">提示语</param>
    /// <param name="lblist">列表框的名称</param>
    public static void BindListClass(int parentid, string str_value, string str_text, string str_parentid, string str_sql, string tablename, string str_msg, ListBox lblist)
    {
        DataTable dt = Data.ExecuteDataSet(str_sql, tablename).Tables[0];
        lblist.Items.Clear();
        // lblist.Items.Add(new ListItem(str_msg, "0"));
        DataRow[] drs = dt.Select(str_parentid + "=" + parentid);
        foreach (DataRow dr in drs)
        {
            string strvalue = dr[str_value].ToString();
            string strtext = dr[str_text].ToString();
            //
            strtext = "" + strtext;
            lblist.Items.Add(new ListItem(strtext, strvalue));
            int parenid = int.Parse(strvalue);
            string parentidmark = "　";
            BindListNode(str_value, str_text, str_parentid, parenid, dt, parentidmark, lblist);
        }
        lblist.DataBind();
    }
    /// <summary>
    /// 绑定子分类无法独立使用需要配合BindListClass
    /// </summary>
    /// <param name="str_value">数据库表示Value值字段</param>
    /// <param name="str_text">数据库表示Text值字段</param>
    /// <param name="str_parentid">数据库表示子类id 0为跟栏目</param>
    /// <param name="parentid">子ID</param>
    /// <param name="dt">datatable表</param>
    /// <param name="parentidmark">子类的表示|--</param>
    /// <param name="ltlist">列表框的名称</param>
    private static void BindListNode(string str_value, string str_text, string str_parentid, int parentid, DataTable dt, string parentidmark, ListBox ltlist)
    {
        DataRow[] drs = dt.Select(str_parentid + "=" + parentid);
        foreach (DataRow dr in drs)
        {
            string strvalue = dr[str_value].ToString();
            string strtext = dr[str_text].ToString();
            strtext = parentidmark + strtext;
            ltlist.Items.Add(new ListItem(strtext, strvalue));
            int sonparentid = int.Parse(strvalue);
            string parentidmark2 = "　" + parentidmark;
            BindListNode(str_value, str_text, str_parentid, sonparentid, dt, parentidmark2, ltlist);
        }
    }
    /// <summary>
    /// 绑定DropDownList控件的无限级分类需要配合BindDrpNode
    /// </summary>
    /// <param name="parentid">绑定的节点,从0开始或者指定绑定的城市编号</param>
    /// <param name="str_value">数据库表示Value值字段</param>
    /// <param name="str_text">数据库表示Text值字段</param>
    /// <param name="str_parentid">数据库表示子类id 0为跟栏目</param>
    /// <param name="str_sql">绑定数据的SQL语句</param>
    /// <param name="dropdownlist">下拉列表框的名称</param>
    public static void BindDrpClass(int parentid, string str_value, string str_text, string str_parentid, string str_sql, string tablename, string str_msg, DropDownList dropdownlist)
    {
        DataTable dt = Data.ExecuteDataSet(str_sql, tablename).Tables[0];
        dropdownlist.Items.Clear();
        dropdownlist.Items.Add(new ListItem(str_msg, "0"));
        DataRow[] drs = dt.Select(str_parentid + "=" + parentid);
        foreach (DataRow dr in drs)
        {
            string strvalue = dr[str_value].ToString();
            string strtext = dr[str_text].ToString();
            //
            strtext = "" + strtext;
            dropdownlist.Items.Add(new ListItem(strtext, strvalue));
            int parenid = int.Parse(strvalue);
            string parentidmark = "　|--";
            BindDrpNode(str_value, str_text, str_parentid, parenid, dt, parentidmark, dropdownlist);
        }
        dropdownlist.DataBind();
    }
    /// <summary>
    /// 绑定子分类无法独立使用需要配合BindDrpClass
    /// </summary>
    /// <param name="str_value">数据库表示Value值字段</param>
    /// <param name="str_text">数据库表示Text值字段</param>
    /// <param name="str_parentid">数据库表示子类id 0为跟栏目</param>
    /// <param name="parentid">子ID</param>
    /// <param name="dt">datatable表</param>
    /// <param name="parentidmark">子类的表示|--</param>
    /// <param name="dropdownlist">下拉列表框的名称</param>
    private static void BindDrpNode(string str_value, string str_text, string str_parentid, int parentid, DataTable dt, string parentidmark, DropDownList dropdownlist)
    {
        DataRow[] drs = dt.Select(str_parentid + "=" + parentid);
        foreach (DataRow dr in drs)
        {
            string strvalue = dr[str_value].ToString();
            string strtext = dr[str_text].ToString();
            strtext = parentidmark + strtext;
            dropdownlist.Items.Add(new ListItem(strtext, strvalue));
            int sonparentid = int.Parse(strvalue);
            string parentidmark2 = "　" + parentidmark;
            BindDrpNode(str_value, str_text, str_parentid, sonparentid, dt, parentidmark2, dropdownlist);
        }
    }
    #endregion
}