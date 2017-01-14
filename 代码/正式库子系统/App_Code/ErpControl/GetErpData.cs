﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Collections.Generic;
using Hesion.Brick.Core;
using FMOP.DB;

/// <summary>
///GetErpData 的摘要说明
/// </summary>
public class GetErpData
{
    public static string Erpcon = ConfigurationManager.ConnectionStrings["fmConnection"].ToString();
    public static string YwCon = ConfigurationManager.ConnectionStrings["FMOPConn"].ToString(); //数据库连接业务平台系统刘杰

	public GetErpData()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 从ERP获取销货单列表
    /// </summary>
    /// <returns></returns>
    public static DataSet GetXHD(string SQLString)
    {
        DataSet ds = null;

        using (SqlConnection connection = new SqlConnection(Erpcon))
        {
            try
            {
                if (SQLString != "")
                {
                    ds = new DataSet();
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, Erpcon);
                    command.Fill(ds, "ds");
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
    }
    /// <summary>
    /// 得到ds
    /// </summary>
    /// <param name="SQLstring"></param>
    /// <returns></returns>
    public static DataSet GetDataSet(string SQLString)
    {
        DataSet ds = null;

        using (SqlConnection connection = new SqlConnection(Erpcon))
        {
            try
            {
                if (SQLString != "")
                {
                    ds = new DataSet();
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, Erpcon);
                    command.SelectCommand.CommandTimeout = 120; //延长时间为120秒
                    command.Fill(ds, "ds");
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
    }   

    public static bool Exists(string strSql)
    {
        object obj = GetErpData.GetSingle(strSql);
        int cmdresult;
        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
        {
            cmdresult = 0;
        }
        else
        {
            cmdresult = int.Parse(obj.ToString());
        }
        if (cmdresult == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 执行一条计算查询结果语句，返回查询结果（object）。
    /// </summary>
    /// <param name="SQLString">计算查询结果语句</param>
    /// <returns>查询结果（object）</returns>
    public static object GetSingle(string SQLString)
    {
        using (SqlConnection connection = new SqlConnection(Erpcon))
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }
    }

    /// <summary>
    /// 执行SQL语句，返回影响的记录数
    /// </summary>
    /// <param name="SQLString">SQL语句</param>
    /// <returns>影响的记录数</returns>
    public static int ExecuteSql(string SQLString)
    {
        using (SqlConnection connection = new SqlConnection(Erpcon))
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    connection.Close();
                    throw new Exception(E.Message);
                }
            }
        }
    }


    /// <summary>
    /// 执行多条SQL语句，实现数据库事务。
    /// </summary>
    /// <param name="SQLStringList">多条SQL语句</param>		
    public static void ExecuteSqlTran(System.Collections.ArrayList SQLStringList)
    {        
        using (SqlConnection conn = new SqlConnection(Erpcon))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                tx.Rollback();
                throw new Exception(E.Message);
            }
        }
    }   
    
    /// <summary>
    /// 实现sql事务的处理，两个数据库 刘杰 2011-05-31
    /// </summary>
    /// <param name="SQLStringList">ERP的sqllist</param>
    /// <param name="SQLStringList2">业务平台的sqlList</param>
    /// <returns>true/false</returns>
    public static bool ExecuteSqlTran(System.Collections.ArrayList SQLStringList,System.Collections.ArrayList SQLStringList2)
    {
        using (SqlConnection conn = new SqlConnection(Erpcon))  //erp数据库
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {

                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using (SqlConnection conn2 = new SqlConnection(YwCon))
                    {
                        conn2.Open();
                        using (SqlCommand cmd2 = new SqlCommand())
                        {
                            cmd2.Connection = conn2;

                            SqlTransaction tx2 = conn2.BeginTransaction();
                            cmd2.Transaction = tx2;

                            try
                            {
                                for (int m = 0; m < SQLStringList2.Count; m++)
                                {
                                    string strsql2 = SQLStringList2[m].ToString();
                                    if (strsql2.Trim().Length > 1)
                                    {
                                        cmd2.CommandText = strsql2;
                                        cmd2.ExecuteNonQuery();
                                    }
                                }


                                tx2.Commit();
                            }
                            catch (System.Data.SqlClient.SqlException E2)
                            {
                                tx2.Rollback();
                                throw E2;
                            }



                        }

                    }

                    tx.Commit();  //外层事务提交
                    return true;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    return false;
                    throw new Exception(E.Message);

                }

            }
        }
    }

    /// <summary>
    /// 执行多条SQL语句，实现数据库事务。
    /// </summary>
    /// <param name="SQLStringList">多条SQL语句</param>		
    public static int ExecuteSqlTran(List<String> SQLStringList)
    {        
        using (SqlConnection conn = new SqlConnection(Erpcon))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                int count = 0;
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n];
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        count += cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
                return count;
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                tx.Rollback();
                throw new Exception(E.Message);
            }
        }
    }

    #region 根据办事处名称，连接分公司自己的ERP系统进行数据操作


    /// <summary>
    /// 执行一条计算查询结果语句，返回查询结果（object）。2012年6月份成立分公司新增
    /// </summary>
    /// <param name="SQLString">计算查询结果语句</param>
    /// <returns>查询结果（object）</returns>
    public static object GetSingle(string SQLString,string BM)
    {       
        //string dbconn = Erpcon;
        string dbconn = "";
        object objConn = DbHelperSQL.GetSingle("select dbconn from system_city_0 where name='" + BM + "' or dyfgsmc='" + BM + "'");
        if (objConn != null && objConn.ToString() != "")
        {
            dbconn = objConn.ToString();
        }
        using (SqlConnection connection = new SqlConnection(dbconn))
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }
    }



    /// <summary>
    /// 根据办事处确定应该连哪个数据库获取数据得到ds
    /// 2012年6月份成立分公司新增
    /// </summary>
    /// <param name="SQLstring">需要执行的sql语句</param>
    ///  <param name="city">办事处名称或分公司名称</param>
    /// <returns></returns>
    public static DataSet GetDataSet(string SQLString, string BM)
    {
        DataSet ds = null;
        //string dbconn = Erpcon;
        string dbconn = "";
        object objConn = DbHelperSQL.GetSingle("select dbconn from system_city_0 where name='" + BM + "' or dyfgsmc='"+BM+"'");
        if (objConn != null && objConn.ToString() != "")
        {
            dbconn  = objConn.ToString();
        }
        using (SqlConnection connection = new SqlConnection(dbconn))
        {
            try
            {
                if (SQLString != "")
                {
                    ds = new DataSet();
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, dbconn);
                    command.SelectCommand.CommandTimeout = 120; //延长时间为120秒
                    command.Fill(ds, "ds");
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
    }

    /// <summary>
    /// 执行SQL语句，返回影响的记录数
    /// </summary>
    /// <param name="SQLString">SQL语句</param>
    /// <returns>影响的记录数</returns>
    public static int ExecuteSql(string SQLString,string BM)
    {
        //根据办事处判断应该写入哪个办事处的ERP数据库 
        string dbconn = Erpcon;
        object objConn = DbHelperSQL.GetSingle("select dbconn from system_city_0 where name='" + BM + "'  or dyfgsmc='" + BM + "'");
        if (objConn != null && objConn.ToString() != "")
        {
            dbconn  = objConn.ToString();
        }
        using (SqlConnection connection = new SqlConnection(dbconn))
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    connection.Close();
                    throw new Exception(E.Message);
                }
            }
        }
    }



    /// <summary>
    /// 执行数据库插入、更新等sql语句
    /// 2012年6月份，为分公司系统开发增加
    /// </summary>
    /// <param name="SQLStringList">需要执行的sql语句</param>
    /// <param name="BM">执行sql语句的办事处名称或分公司名称</param>
    public static void ExecuteSqlTran(System.Collections.ArrayList SQLStringList, string BM)
    {
        //根据办事处判断应该写入哪个办事处的ERP数据库
        string dbconn = Erpcon;
        object objConn = DbHelperSQL.GetSingle ("select dbconn from system_city_0 where name='" + BM + "'  or dyfgsmc='" + BM + "'");
        if (objConn != null && objConn.ToString() != "")
        {
            dbconn = objConn.ToString();
        }
        using (SqlConnection conn = new SqlConnection(dbconn))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                tx.Rollback();
                throw new Exception(E.Message);
            }
        }
    }


    /// <summary>
    /// 执行数据库插入、更新等sql语句
    /// 2012年6月份，为分公司系统开发增加
    /// </summary>
    /// <param name="SQLStringList">需要执行的sql语句</param>
    /// <param name="BM">执行sql语句的办事处名称或分公司名称</param>
    public static int ExecuteSqlTran(List<String> SQLStringList,string BM)
    {
        //根据办事处判断应该写入哪个办事处的ERP数据库 
        string dbconn = Erpcon;
        object objConn = DbHelperSQL.GetSingle ("select dbconn from system_city_0 where name='" + BM + "' or dyfgsmc='" + BM + "'");
        if (objConn != null && objConn.ToString() != "")
        {
            dbconn  = objConn.ToString();
        }
        using (SqlConnection conn = new SqlConnection(dbconn))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                int count = 0;
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n];
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        count += cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
                return count;
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                tx.Rollback();
                throw new Exception(E.Message);
            }
        }
    }

    /// <summary>
    /// 实现sql事务的处理，两个数据库 刘杰 2011-05-31
    /// </summary>
    /// <param name="SQLStringList">ERP的sqllist</param>
    /// <param name="SQLStringList2">业务平台的sqlList</param>
    /// <returns>true/false</returns>
    public static bool ExecuteSqlTran(System.Collections.ArrayList SQLStringList, System.Collections.ArrayList SQLStringList2,string BM)
    {
        //根据办事处判断应该写入哪个办事处的ERP数据库 
        string dbconn = Erpcon;
        object objConn = DbHelperSQL.GetSingle("select dbconn from system_city_0 where name='" + BM + "' or dyfgsmc='" + BM + "'");
        if (objConn != null && objConn.ToString() != "")
        {
            dbconn = objConn.ToString();
        }
        using (SqlConnection conn = new SqlConnection(dbconn))  //erp数据库
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {

                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using (SqlConnection conn2 = new SqlConnection(YwCon))
                    {
                        conn2.Open();
                        using (SqlCommand cmd2 = new SqlCommand())
                        {
                            cmd2.Connection = conn2;

                            SqlTransaction tx2 = conn2.BeginTransaction();
                            cmd2.Transaction = tx2;

                            try
                            {
                                for (int m = 0; m < SQLStringList2.Count; m++)
                                {
                                    string strsql2 = SQLStringList2[m].ToString();
                                    if (strsql2.Trim().Length > 1)
                                    {
                                        cmd2.CommandText = strsql2;
                                        cmd2.ExecuteNonQuery();
                                    }
                                }


                                tx2.Commit();
                            }
                            catch (System.Data.SqlClient.SqlException E2)
                            {
                                tx2.Rollback();
                                throw E2;
                            }
                        }

                    }

                    tx.Commit();  //外层事务提交
                    return true;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    return false;
                    throw new Exception(E.Message);

                }

            }
        }
    }
#endregion






}
