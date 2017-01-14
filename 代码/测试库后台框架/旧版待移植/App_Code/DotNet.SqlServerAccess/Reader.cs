using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data.OleDb;

/// <summary>
/// 对外读取的通用类,主要用于快速只读的操作,同时也具有自动切换数据库功能
/// </summary>
public class Reader
{
	public Reader()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    private int _ConnIndex;
    private CAcc myAcc;
    private CSql mySql;
    private OleDbDataReader OleDR;
    private SqlDataReader SqlDR;
    /// <summary>
    /// 读取数据集合
    /// </summary>
    /// <param name="dr">OleDbDataReader的名称</param>
    public Reader(OleDbDataReader dr)
    {
        this.myAcc = new CAcc();
        this.mySql = new CSql();
        if (CLink.Type(this._ConnIndex) == 0)
        {
            this.OleDR = dr;
        }
    }
    /// <summary>
    /// 读取数据集合
    /// </summary>
    /// <param name="dr">SqlDataReader的名称</param>
    public Reader(SqlDataReader dr)
    {
        this.myAcc = new CAcc();
        this.mySql = new CSql();
        if (CLink.Type(this._ConnIndex) == 1)
        {
            this.SqlDR = dr;
        }
    }
    /// <summary>
    /// 读取数据集合
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    public Reader(string SQL)
    {
        this.myAcc = new CAcc();
        this.mySql = new CSql();
        this.ExecuteReader(SQL, null, 0);
    }
    /// <summary>
    /// 读取数据集合
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <param name="para">参数列表</param>
    public Reader(string SQL, string[,] para)
    {
        this.myAcc = new CAcc();
        this.mySql = new CSql();
        this.ExecuteReader(SQL, para, 0);
    }
    /// <summary>
    /// 读取数据集合
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    public Reader(string SQL, int ConnIndex)
    {
        this.myAcc = new CAcc();
        this.mySql = new CSql();
        this.ExecuteReader(SQL, null, ConnIndex);
    }
    /// <summary>
    /// 读取数据集合
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <param name="para">参数列表</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    public Reader(string SQL, string[,] para, int ConnIndex)
    {
        this.myAcc = new CAcc();
        this.mySql = new CSql();
        this.ExecuteReader(SQL, para, ConnIndex);
    }
    /// <summary>
    /// 关闭数据连接
    /// </summary>
    public void Close()
    {
        if (CLink.Type(this._ConnIndex) == 1)
        {
            this.SqlDR.Close();
            this.mySql.Close();
        }
        else
        {
            this.OleDR.Close();
            this.myAcc.Close();
        }
    }
    /// <summary>
    /// 执行ExecuteReader
    /// </summary>
    /// <param name="SQL">SQL语句</param>
    /// <param name="para">参数列表</param>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    private void ExecuteReader(string SQL, string[,] para, int ConnIndex)
    {
        if (CLink.Type(ConnIndex) == 1)
        {
            this.mySql.Open(ConnIndex);
            SqlCommand cmd = new SqlCommand(SQL, this.mySql.ConnSql);
            CParameter.SqlserverParameters(SQL, para, cmd, this.mySql.ConnSql);
            this.SqlDR = cmd.ExecuteReader();
        }
        else
        {
            this.myAcc.Open(ConnIndex);
            OleDbCommand command2 = new OleDbCommand(SQL, this.myAcc.ConnAcc);
            CParameter.AccessParameters(SQL, para, command2, this.myAcc.ConnAcc);
            this.OleDR = command2.ExecuteReader();
        }
        this._ConnIndex = ConnIndex;
    }
    /// <summary>
    /// 读取数据
    /// </summary>
    /// <returns></returns>
    public bool Read()
    {
        if (CLink.Type(this._ConnIndex) == 1)
        {
            return this.SqlDR.Read();
        }
        return this.OleDR.Read();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cs"></param>
    /// <returns></returns>
    public object this[string cs]
    {
        get
        {
            if (CLink.Type(this._ConnIndex) == 1)
            {
                return this.SqlDR[cs];
            }
            return this.OleDR[cs];
        }
    }
}