using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// 针对sqlserver数据库的操作类
/// </summary>
public class CSql
{
	public CSql()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    public SqlConnection ConnSql;
    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void Close()
    {
        this.ConnSql.Close();
        this.ConnSql.Dispose();
        this.ConnSql = null;
    }
    /// <summary>
    /// 返回DataSet数据集
    /// </summary>
    /// <param name="StoreName">存储过程名称</param>
    /// <param name="startindex">记录的开始位置(默认从0开始)</param>
    /// <param name="num">要检索的最大记录数</param>
    /// <param name="dataname">数据集名称</param>
    /// <param name="para">参数列表</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回DataSet</returns>
    public DataSet ExecuteDataSet(string StoreName, int startindex, int num, string dataname, string[,] para, int ConnIndex)
    {
        this.Open(ConnIndex);
        DataSet dataSet = new DataSet();
        SqlCommand cmd = new SqlCommand();
        CParameter.SqlserverParameters(StoreName, para, cmd, this.ConnSql);
        new SqlDataAdapter(cmd).Fill(dataSet, startindex, num, dataname);
        this.Close();
        return dataSet;
    }
    /// <summary>
    /// 返回数据库的表,受影响的行数。
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="para">参数列表</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回受影响的行数</returns>
    public int ExecuteNonQuery(string sql, string[,] para, int ConnIndex)
    {
        this.Open(ConnIndex);
        SqlCommand cmd = new SqlCommand();
        CParameter.SqlserverParameters(sql, para, cmd, this.ConnSql);
        int num = cmd.ExecuteNonQuery();
        this.Close();
        return num;
    }
    /// <summary>
    /// 仅返回结果集中的第一行的第一列数据，忽略其它行或列。
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <param name="para">参数列表</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回受影响的行数</returns>
    public object ExecuteScalar(string SQL, string[,] para, int ConnIndex)
    {
        this.Open(ConnIndex);
        SqlCommand cmd = new SqlCommand();
        CParameter.SqlserverParameters(SQL, para, cmd, this.ConnSql);
        object obj2 = cmd.ExecuteScalar();
        this.Close();
        return obj2;
    }
    /// <summary>
    /// 打开数据连接
    /// </summary>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    public void Open(int ConnIndex)
    {
        string source = CLink.Source;
        if (source.IndexOf("|") > -1)
        {
            source = source.Split(new char[] { '|' })[ConnIndex];
        }
        this.ConnSql = new SqlConnection(source);
        if (this.ConnSql.State == ConnectionState.Closed)
        {
            this.ConnSql.Open();
        }
    }
}