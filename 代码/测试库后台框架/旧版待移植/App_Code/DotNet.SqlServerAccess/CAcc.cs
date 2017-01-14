using System;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Data;

/// <summary>
/// 针对Access数据库的操作类
/// </summary>
public class CAcc
{
	public CAcc()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    public OleDbConnection ConnAcc;
    public string DataAcc = CLink.Source; //获取CLink类中的数据库连接字符串
    /// <summary>
    /// 判断数据库连接
    /// </summary>
    public void Close()
    {
        if (this.ConnAcc.State == ConnectionState.Open)
        {
            this.ConnAcc.Close();
            this.ConnAcc.Dispose();
            this.ConnAcc = null;
        }
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
        OleDbCommand cmd = new OleDbCommand(StoreName, this.ConnAcc);
        CParameter.AccessParameters(StoreName, para, cmd, this.ConnAcc);
        new OleDbDataAdapter(cmd).Fill(dataSet, startindex, num, dataname);
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
        OleDbCommand cmd = new OleDbCommand(sql, this.ConnAcc);
        CParameter.AccessParameters(sql, para, cmd, this.ConnAcc);
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
        OleDbCommand cmd = new OleDbCommand(SQL, this.ConnAcc);
        CParameter.AccessParameters(SQL, para, cmd, this.ConnAcc);
        object obj2 = cmd.ExecuteScalar();
        this.Close();
        return obj2;
    }
    /// <summary>
    /// 打开数据库连接
    /// </summary>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    public void Open(int ConnIndex)
    {
        string dataAcc = this.DataAcc;
        //if (dataAcc.IndexOf("|") > -1)
        //{
        //    dataAcc = dataAcc.Split(new char[] { '|' })[ConnIndex];
        //}
        //if (this.DataAcc.IndexOf(":") < 0)
        //{
        //    dataAcc = System.Web.HttpContext.Current.Server.MapPath(dataAcc);
        //}
        //dataAcc = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dataAcc + ";";
        this.ConnAcc = new OleDbConnection(dataAcc);
        if (this.ConnAcc.State == ConnectionState.Closed)
        {
            this.ConnAcc.Open();
        }
    }


}