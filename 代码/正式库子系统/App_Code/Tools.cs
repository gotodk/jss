using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using FMOP.DB;
/// <summary>
/// 数据库操作类
/// </summary>
public static class Tools
{
    public static DataTable GetChild(string id)//获取指定类别的子类别
    {
        return DbHelperSQL.Query("select * from table_productClass where classfather=" + id).Tables[0];

    }
    public static DataTable GetChild()//获取所有类别
    {
        //return OpenQuery("select * from table_productClass ");
        return DbHelperSQL.Query("select * from table_productClass ").Tables[0];
    }
}
