using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// 对外调用类,通过此类自动转换要调用的数据库访问类型
/// 如果是Access数据库,则通过Web.config中的连接字符串来判断类型,自动调用CAcc类的方法
/// </summary>
public class Data
{
	public Data()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    private CSql myCSql = new CSql();
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="SqlscriptOrStoreName">SQL语句或存储过程名称</param>
    /// <param name="dataname">数据集名称</param>
    /// <returns>返回DataSet</returns>
    public static DataSet ExecuteDataSet(string SqlscriptOrStoreName, string dataname)
    {
        return ExecuteDataSet(SqlscriptOrStoreName, null, dataname, 0, 0, 0);
    }
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="SqlscriptOrStoreName">SQL语句或存储过程名称</param>
    /// <param name="para">参数列表</param>
    /// <param name="dataname">数据集名称</param>
    /// <returns>返回DataSet</returns>
    public static DataSet ExecuteDataSet(string SqlscriptOrStoreName, string[,] para, string dataname)
    {
        return ExecuteDataSet(SqlscriptOrStoreName, para, dataname, 0, 0, 0);
    }
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="SqlscriptOrStoreName">SQL语句或存储过程名称</param>
    /// <param name="dataname">数据集名称</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回DataSet</returns>
    public static DataSet ExecuteDataSet(string SqlscriptOrStoreName, string dataname, int ConnIndex)
    {
        return ExecuteDataSet(SqlscriptOrStoreName, null, dataname, 0, 0, ConnIndex);
    }
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="SqlscriptOrStoreName">SQL语句或存储过程名称</param>
    /// <param name="dataname">数据集名称</param>
    /// <param name="startindex">记录的开始位置(默认从0开始)</param>
    /// <param name="num">要检索的最大记录数</param>
    /// <returns>返回DataSet</returns>
    public static DataSet ExecuteDataSet(string SqlscriptOrStoreName, string dataname, int startindex, int num)
    {
        return ExecuteDataSet(SqlscriptOrStoreName, null, dataname, startindex, num, 0);
    }
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="SqlscriptOrStoreName">SQL语句或存储过程名称</param>
    /// <param name="para">参数列表</param>
    /// <param name="dataname">数据集名称</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回DataSet</returns>
    public static DataSet ExecuteDataSet(string SqlscriptOrStoreName, string[,] para, string dataname, int ConnIndex)
    {
        return ExecuteDataSet(SqlscriptOrStoreName, para, dataname, 0, 0, ConnIndex);
    }
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="SqlscriptOrStoreName">SQL语句或存储过程名称</param>
    /// <param name="para">参数列表</param>
    /// <param name="dataname">数据集名称</param>
    /// <param name="startindex">记录的开始位置(默认从0开始)</param>
    /// <param name="num">要检索的最大记录数</param>
    /// <returns>返回DataSet</returns>
    public static DataSet ExecuteDataSet(string SqlscriptOrStoreName, string[,] para, string dataname, int startindex, int num)
    {
        return ExecuteDataSet(SqlscriptOrStoreName, para, dataname, startindex, num, 0);
    }
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="SqlscriptOrStoreName">SQL语句或存储过程名称</param>
    /// <param name="dataname">数据集名称</param>
    /// <param name="startindex">记录的开始位置(默认从0开始)</param>
    /// <param name="num">要检索的最大记录数</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回DataSet</returns>
    public static DataSet ExecuteDataSet(string SqlscriptOrStoreName, string dataname, int startindex, int num, int ConnIndex)
    {
        return ExecuteDataSet(SqlscriptOrStoreName, null, dataname, startindex, num, ConnIndex);
    }
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="SqlscriptOrStoreName">SQL语句或存储过程名称</param>
    /// <param name="para">参数列表</param>
    /// <param name="dataname">数据集名称</param>
    /// <param name="startindex">记录的开始位置(默认从0开始)</param>
    /// <param name="num">要检索的最大记录数</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回DataSet</returns>
    public static DataSet ExecuteDataSet(string SqlscriptOrStoreName, string[,] para, string dataname, int startindex, int num, int ConnIndex)
    {
        if (CLink.Type(ConnIndex) == 1)
        {
            CSql sql = new CSql();
            return sql.ExecuteDataSet(SqlscriptOrStoreName, startindex, num, dataname, para, ConnIndex);
        }
        CAcc acc = new CAcc();
        return acc.ExecuteDataSet(SqlscriptOrStoreName, startindex, num, dataname, para, ConnIndex);
    }
    /// <summary>
    /// 返回数据库的表,受影响的行数。
    /// </summary>
    /// <param name="SqlscriptOrStorename">SQL语句或存储过程名称</param>
    /// <returns>返回受影响的行数</returns>
    public static int ExecuteNonQuery(string SqlscriptOrStorename)
    {
        return ExecuteNonQuery(SqlscriptOrStorename, null, 0);
    }
    /// <summary>
    /// 返回数据库的表,受影响的行数。
    /// </summary>
    /// <param name="SqlscriptOrStorename">SQL语句或存储过程名称</param>
    /// <param name="para">参数列表</param>
    /// <returns>返回受影响的行数</returns>
    public static int ExecuteNonQuery(string SqlscriptOrStorename, string[,] para)
    {
        return ExecuteNonQuery(SqlscriptOrStorename, para, 0);
    }
    /// <summary>
    /// 返回数据库的表,受影响的行数。
    /// </summary>
    /// <param name="SqlscriptOrStorename">SQL语句或存储过程名称</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回受影响的行数</returns>
    public static int ExecuteNonQuery(string SqlscriptOrStorename, int ConnIndex)
    {
        return ExecuteNonQuery(SqlscriptOrStorename, null, ConnIndex);
    }
    /// <summary>
    /// 返回数据库的表,受影响的行数。
    /// </summary>
    /// <param name="StoreName">存储过程名称</param>
    /// <param name="para">参数列表</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回受影响的行数</returns>
    public static int ExecuteNonQuery(string StoreName, string[,] para, int ConnIndex)
    {
        if (CLink.Type(ConnIndex) == 1)
        {
            CSql sql = new CSql();
            return sql.ExecuteNonQuery(StoreName, para, ConnIndex);
        }
        CAcc acc = new CAcc();
        return acc.ExecuteNonQuery(StoreName, para, ConnIndex);
    }
    /// <summary>
    /// 仅返回结果集中的第一行的第一列数据，忽略其它行或列。
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <returns>返回第一行的第一列数据</returns>
    public static object ExecuteScalar(string SQL)
    {
        return ExecuteScalar(SQL, null, 0);
    }
    /// <summary>
    /// 仅返回结果集中的第一行的第一列数据，忽略其它行或列。
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <param name="para">参数列表</param>
    /// <returns>返回第一行的第一列数据</returns>
    public static object ExecuteScalar(string SQL, string[,] para)
    {
        return ExecuteScalar(SQL, para, 0);
    }
    /// <summary>
    /// 仅返回结果集中的第一行的第一列数据，忽略其它行或列。
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回第一行的第一列数据</returns>
    public static object ExecuteScalar(string SQL, int ConnIndex)
    {
        return ExecuteScalar(SQL, null, ConnIndex);
    }
    /// <summary>
    /// 仅返回结果集中的第一行的第一列数据，忽略其它行或列。
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <param name="para">参数列表</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回第一行的第一列数据</returns>
    public static object ExecuteScalar(string SQL, string[,] para, int ConnIndex)
    {
        if (CLink.Type() == 1)
        {
            CSql sql = new CSql();
            return sql.ExecuteScalar(SQL, para, ConnIndex);
        }
        CAcc acc = new CAcc();
        return acc.ExecuteScalar(SQL, para, ConnIndex);
    }
}