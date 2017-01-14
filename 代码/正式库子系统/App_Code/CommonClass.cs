using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

/// <summary>
/// 2012-02-10
/// 为上传、下载模板所用的数据库链接、提示消息和自动编号的公共类。
/// 添加者：郭拓
/// </summary>
public class CommonClass
{
    public CommonClass()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 连接数据库
    /// </summary>
    /// <returns>返回SqlmyConnection对象</returns>
    public SqlConnection GetConnection()
    {
        string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        SqlConnection myConn = new SqlConnection(myStr);
        if (myConn == null)
        {
            myConn = new SqlConnection(myStr);
            myConn.Open();
        }
        else if (myConn.State == ConnectionState.Closed)
        {
            myConn.Open();
        }
        else if (myConn.State == ConnectionState.Broken)
        {
            myConn.Close();
            myConn.Open();
        }


        return myConn;
    }
    /// <summary>
    /// 说明：MessageBox用来在客户端弹出对话框。
    /// 参数：TxtMessage 对话框中显示的内容。
    /// 参数：Url 对话框关闭后，跳转的页
    /// </summary>
    public string MessageBox(string TxtMessage, string Url)
    {
        string str;
        str = "<script language=javascript>alert('" + TxtMessage + "');location='" + Url + "'</script>";
        return str;
    }
    /// <summary>
    /// 说明：MessageBox用来在客户端弹出对话框。
    /// 参数：TxtMessage 对话框中显示的内容。
    /// </summary>
    public string MessageBox(string TxtMessage)
    {
        string str;
        str = "<script language=javascript>alert('" + TxtMessage + "')</script>";
        return str;
    }
    /// <summary>
    /// 实现自动编号
    /// </summary>
    /// <param name="FieldName">自动编号的字段名</param>
    /// <param name="TableName">表名</param>
    /// <returns>返回编号</returns>
    public int GetAutoID(string FieldName, string TableName)
    {
        SqlConnection mymyConn = GetConnection();
        SqlCommand myCmd = new SqlCommand("select Max(" + FieldName + ") as MaxID from " + TableName, mymyConn);
        SqlDataAdapter dapt = new SqlDataAdapter(myCmd);
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        if (ds.Tables[0].Rows[0][0].ToString() == "")
        {
            return 1;
        }
        else
        {
            int IntFieldID = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) + 1;
            return (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) + 1);
        }
    }
    ///// <summary>
    ///// 查看具有相同名字的文件数
    ///// </summary>
    ///// <param name="sqlStr">查询字符串</param>
    ///// <returns>返回相同文件数</returns>
    //public int GetRowCount(string sqlStr)
    //{
    //    //打开与数据库的连接
    //    SqlmyConnection mymyConn = CC.GetmyConnection();
    //    mymyConn.Open();
    //    SqlCommand myCmd = new SqlCommand(sqlStr, mymyConn);
    //    int IntRowCount = Convert.ToInt32(myCmd.ExecuteScalar().ToString());
    //    //释放资源
    //    myCmd.Dispose();
    //    mymyConn.Close();
    //    //返回行数
    //    return IntRowCount;
    //}
}