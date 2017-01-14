using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace StressTesting
{
    /// <summary>
    /// 用于管理数据库的连接
    /// </summary>
    class DBClass
    {
        string currentPath = System.Environment.CurrentDirectory.ToString()+@"\"; //得到当前路径
        string connectString = "";
        public DBClass()
        {
           XmlClass xc = new XmlClass(currentPath + "ConSet.xml");
           XmlNodeList nlist=xc.GetNodeList("Appsetting").Item(0).ChildNodes; //节点列表
           string serverName = "", dbName = "", userName = "", passWord = "";    
          
            foreach (XmlNode n in nlist)         
            {
                if (n.Name == "ServerName")
                {
                    serverName = n.InnerText;
                }
                if (n.Name == "DbName")
                {
                    dbName= n.InnerText;                   
                }
                if (n.Name == "UserName")
                {
                    userName= n.InnerText;
                }
                if (n.Name == "Password")
                {
                    passWord= n.InnerText;
                }
            }
            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("data source=" + serverName + ";persist security info=false;");
            sb.Append("initial catalog=" +dbName + ";");
            sb.Append("user id=" +userName + ";");
            sb.Append("Password=" +passWord+ ";");
            connectString = sb.ToString().ToString();
        }

        public string getConStr()
        {
            return connectString;
           
        }
        /// <summary>
        /// 判断是否连接
        /// </summary>
        /// <returns></returns>
        public bool isCon()
        {
            bool IsCanConDB;
            using (SqlConnection cn = new SqlConnection(connectString))
            {
                try
                {
                    cn.Open();
                    IsCanConDB = true;
                }
                catch
                {
                    IsCanConDB = false;
                }
                finally
                {
                    cn.Close();
                }

                if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                {
                    return IsCanConDB;
                }
                else
                {
                    return IsCanConDB;
                }
            }
        }


        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, string connectionString)
        {
            DataSet ds = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (SQLString != "")
                    {
                        ds = new DataSet();
                        connection.Open();
                        SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                        command.Fill(ds, "ds");
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    //throw new Exception(ex.Message);
                    BasicShare.WriteLog(System.Environment.CurrentDirectory.ToString() + @"\", "logfile", ex.Message); 
                }
                return ds;
            }
        }
       
    }
}
