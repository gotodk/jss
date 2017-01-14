using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

/// <summary>
/// 参数类,用于执行sql语句中的参数转换
/// </summary>
public class CParameter
{
	public CParameter()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// Access参数转换
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="para">参数列表</param>
    /// <param name="cmd">OleDbCommand实例名</param>
    /// <param name="ConnSql">OleDbConnection连接字符串</param>
    public static void AccessParameters(string sql, string[,] para, OleDbCommand cmd, OleDbConnection ConnSql)
    {
        cmd.Connection = ConnSql;
        if (para != null)
        {
            for (int i = 0; i < para.GetLength(0); i++)
            {
                cmd.Parameters.Add(new OleDbParameter(para[i, 0], NullToStr(para[i, 1])));
            }
        }
        cmd.CommandText = sql;
    }
    /// <summary>
    /// SQL Server参数转换
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>返回结果</returns>
    private static string NullToStr(string str)
    {
        if (str == null)
        {
            return "";
        }
        return str;
    }
    /// <summary>
    /// 将SQL的参数转换成Access参数
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="para">参数列表</param>
    /// <param name="cmd">OleDbCommand实例名</param>
    /// <param name="ConnSql">OleDbConnection连接字符串</param>
    public static void SqlserverParameters(string sql, string[,] para, SqlCommand cmd, SqlConnection ConnSql)
    {
        sql = sql.Trim();
        cmd.Connection = ConnSql;
        if (sql.Trim().IndexOf(" ") < 0)
        {
            cmd.CommandType = CommandType.StoredProcedure;
        }
        if (para != null)
        {
            for (int i = 0; i < para.GetLength(0); i++)
            {
                cmd.Parameters.Add(new SqlParameter(para[i, 0], NullToStr(para[i, 1])));
            }
        }
        cmd.CommandText = sql;
    }
}