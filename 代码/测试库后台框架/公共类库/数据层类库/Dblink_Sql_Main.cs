using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;

namespace FMDBHelperClass
{
    /// <summary>
    /// 用于华商中国主站，并且任何系统通用
    /// MS SQL数据库连接类,具体产品角色(此类为资讯系统设定，其他系统调用另外的角色)
    /// 功能列表:
    /// 1.初始化数据库连接
    /// 2.传送不同参数，执行sql语句，通过哈希表返回执行结果集合
    /// return_ds代表返回的数据集,执行失败会返回null，执行成功返回结果数据集。 
    /// return_float代表是否执行成功。 返回true/false,布尔类型。
    /// return_errmsg代表执行错误的调试信息。无错误将返回''.
    /// return_other其他的返回值。通常是数据受影响行数。但是不准确。 
    /// </summary>
    public class Dblink_Sql_Main : I_Dblink
    {

        #region 变 量 或 类 的 声 明 与 初 始 化

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string ConnStr = "";

        /// <summary>
        /// 用于返回值集合的哈希表
        /// 哈希表返回值说明:
        /// return_ht["return_ds"] = null; 返回的执行结果数据集，DataSet类型
        /// return_ht["return_float"] = null; 返回的执行结果标志，执行成功并且返回的数据大于0条，则为true
        /// return_ht["return_errmsg"] = null; 返回的错误捕获信息，string类型
        /// return_ht["return_other"] = null; 返回的特殊值类型，用途不定，类型不定
        /// </summary>
        private Hashtable return_ht = new Hashtable();

        #endregion

        /// <summary>
        /// MS SQL数据库连接类构造函数
        /// </summary>
        /// <param name="ConnConfig">web.config配置文件中connectionStrings下关于数据库连接字段的KEY</param>
        public Dblink_Sql_Main(string ConnConfig)
        {

            if (ConnConfig == "")
            {
                ConnStr = get_info("mainsqlserver"); //默认的配置
            }
            else
            {
                ConnStr = get_info(ConnConfig);
            }
 



            //初始化哈希表返回值
            return_ht.Add("return_ds", null);//需要返回的数据集。
            return_ht.Add("return_float", null);//方法执行结果，代表成功还是失败。(true/false)
            return_ht.Add("return_errmsg", null);//错误捕获消息
            return_ht.Add("return_other", null);//其他特殊返回值
        }

        ///// <summary>
        ///// 从注册表创建数据库服务器信息
        ///// </summary>
        ///// <param name="dblink">连接字符串</param>
        //public void setregedit(string dblink)
        //{
        //    RegistryKey hklm;
        //    hklm = Registry.CurrentUser;
        //    RegistryKey software;
        //    software = hklm.OpenSubKey("Software", true);
        //    RegistryKey mykey;
        //    mykey = software.CreateSubKey("银河采集系统");
        //    mykey.SetValue("dblink", dblink);
        //    hklm.Close();
        //}
        ///// <summary>
        ///// 从注册表读取数据库服务器信息
        ///// </summary>
        ///// <returns>连接字符串数组</returns>
        //public string[] getregedit()
        //{
        //    //
        //    string[] temp_key = { "", "", "", "" };
        //    RegistryKey pregkey;
        //    pregkey = Registry.CurrentUser.OpenSubKey("Software\\银河采集系统", true);
        //    if (pregkey == null)
        //    {
        //        setregedit(@"user id=sa;password=wwwcbc365comcn;initial catalog=GalaxyAutoCollect;Server=ZRSM1;Connect Timeout=30");
        //        pregkey = Registry.CurrentUser.OpenSubKey("Software\\银河采集系统", true);
        //    }
        //    temp_key.SetValue(pregkey.GetValue("dblink").ToString(), 0);
        //    return temp_key;
        //}

        /// <summary>
        /// 读取配置文件
        /// </summary>
        private string get_info(string ConnConfig)
        {
            string connString = ConfigurationManager.ConnectionStrings[ConnConfig].ToString();
            return connString;
        }


        /// <summary>
        /// 垃圾处理方法,销毁SqlConnection对象
        /// </summary>
        /// <param name="Conn">数据库连接对象</param>
        private void Dispose(SqlConnection Conn)
        {
            try
            {
                if (Conn != null)
                {
                    Conn.Close();
                }
                //GC.Collect();
            }
            catch { }
        }



        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行静态语句。 不返回数据集， 会返回受影响行数。
        /// 常用于执行静态语句进行更新、插入操作，不能用于查询操作。
        /// </summary>
        /// <param name="SQL">需要执行的sql语句</param>
        /// <returns>返回结果集合对应的哈希表(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable I_Dblink.RunProc(string SQL)
        {
            SqlConnection Conn = new SqlConnection(ConnStr);
            try
            {
                int sf = 0;
                //Conn = new SqlConnection(ConnStr);
                Conn.Open();
                SqlCommand Cmd = new SqlCommand(SQL, Conn);
                sf = Cmd.ExecuteNonQuery();
                Dispose(Conn);
                return_ht["return_ds"] = new DataSet();
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = sf;

                return return_ht;
            }
            catch (Exception exp)
            {
                Dispose(Conn);
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = exp.ToString();
                return_ht["return_other"] = 0;

                return return_ht;
            }
        }


        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行静态语句。 会返回数据集， 不返回受影响行数。
        /// 常用于执行静态语句进行查询操作，不能用于插入、更新操作。
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable I_Dblink.RunProc(string SQL, string DTname)
        {
            SqlConnection Conn = new SqlConnection(ConnStr);
            try
            {
                DataSet dtreturn = new DataSet();
                int kp = 0;
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter(SQL, Conn);
                kp = Da.Fill(dtreturn);
                Dispose(Conn);
                if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                {
                    dtreturn.Tables[0].TableName = DTname;
                }
                return_ht["return_ds"] = dtreturn;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -1;



                return return_ht;
            }
            //执行失败
            catch (Exception Err)
            {
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Err.ToString();
                return_ht["return_other"] = -1;

                return return_ht;
            }
        }

        /// <summary>
        /// 单语句执行，不支持事务。(优先读取缓存)
        /// 执行静态语句。 会返回数据集， 不返回受影响行数。
        /// 常用于执行静态语句进行查询操作，不能用于插入、更新操作。
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable I_Dblink.RunProc(string SQL, string DTname, string RedisKey, string RedisPZ)
        {
            DataSet formRedis = RedisClass.RedisTryOnlyForDBhelper(RedisKey, RedisPZ);
            if (formRedis != null)
            {
                return_ht["return_ds"] = formRedis;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -2;
                return return_ht;
            }
            else
            {
                SqlConnection Conn = new SqlConnection(ConnStr);
                try
                {
                    DataSet dtreturn = new DataSet();
                    int kp = 0;
                    Conn.Open();
                    SqlDataAdapter Da = new SqlDataAdapter(SQL, Conn);
                    kp = Da.Fill(dtreturn);
                    Dispose(Conn);
                    if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                    {
                        dtreturn.Tables[0].TableName = DTname;
                    }
                    return_ht["return_ds"] = dtreturn;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = "";
                    return_ht["return_other"] = -1;



                    return return_ht;
                }
                //执行失败
                catch (Exception Err)
                {
                    Dispose(Conn);

                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = Err.ToString();
                    return_ht["return_other"] = -1;

                    return return_ht;
                }
            }
        }

        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行动态语句。 会返回数据集， 不返回受影响行数。
        /// 常用于执行动态语句进行查询操作，不能用于插入、更新操作。
        /// </summary>
        /// <param name="P_cmd">带参数的语句</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">参数哈希表</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable I_Dblink.RunParam_SQL(string P_cmd, string DTname, Hashtable P_ht_in)
        {
            SqlConnection Conn = new SqlConnection(ConnStr);
            try
            {
                DataSet dtreturn = new DataSet();
                int kp = 0;
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.Connection = Conn;
                Da.SelectCommand.CommandText = P_cmd;
                Da.SelectCommand.CommandType = CommandType.Text;

                //循环添加存储过程参数
                IDictionaryEnumerator myEnumerator = P_ht_in.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    SqlParameter param = new SqlParameter();
                    param.Direction = ParameterDirection.Input;
                    param.ParameterName = myEnumerator.Key.ToString();
                    if (myEnumerator.Value == null)
                    {
                        // myEnumerator.Value = DBNull.Value;
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = myEnumerator.Value;
                    }
                    Da.SelectCommand.Parameters.Add(param);
                }


                kp = Da.Fill(dtreturn);
                Dispose(Conn);
                if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                {
                    dtreturn.Tables[0].TableName = DTname;
                }
                return_ht["return_ds"] = dtreturn;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -1;

                return return_ht;
            }
            catch (Exception Ex)
            {
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = -1;

                return return_ht;
            }
        }


        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行动态语句。 会返回数据集， 不返回受影响行数。
        /// 常用于执行动态语句进行查询操作，不能用于插入、更新操作。
        /// </summary>
        /// <param name="P_cmd">带参数的语句</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">参数哈希表</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable I_Dblink.RunParam_SQL(string P_cmd, string DTname, Hashtable P_ht_in, string RedisKey, string RedisPZ)
        {
             DataSet formRedis = RedisClass.RedisTryOnlyForDBhelper(RedisKey, RedisPZ);
             if (formRedis != null)
             {
                 return_ht["return_ds"] = formRedis;
                 return_ht["return_float"] = true;
                 return_ht["return_errmsg"] = "";
                 return_ht["return_other"] = -2;
                 return return_ht;
             }
             else
             {
                 SqlConnection Conn = new SqlConnection(ConnStr);
                 try
                 {
                     DataSet dtreturn = new DataSet();
                     int kp = 0;
                     Conn.Open();
                     SqlDataAdapter Da = new SqlDataAdapter();
                     Da.SelectCommand = new SqlCommand();
                     Da.SelectCommand.Connection = Conn;
                     Da.SelectCommand.CommandText = P_cmd;
                     Da.SelectCommand.CommandType = CommandType.Text;

                     //循环添加存储过程参数
                     IDictionaryEnumerator myEnumerator = P_ht_in.GetEnumerator();
                     while (myEnumerator.MoveNext())
                     {
                         SqlParameter param = new SqlParameter();
                         param.Direction = ParameterDirection.Input;
                         param.ParameterName = myEnumerator.Key.ToString();
                         if (myEnumerator.Value == null)
                         {
                             // myEnumerator.Value = DBNull.Value;
                             param.Value = DBNull.Value;
                         }
                         else
                         {
                             param.Value = myEnumerator.Value;
                         }
                         Da.SelectCommand.Parameters.Add(param);
                     }


                     kp = Da.Fill(dtreturn);
                     Dispose(Conn);
                     if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                     {
                         dtreturn.Tables[0].TableName = DTname;
                     }
                     return_ht["return_ds"] = dtreturn;
                     return_ht["return_float"] = true;
                     return_ht["return_errmsg"] = "";
                     return_ht["return_other"] = -1;

                     return return_ht;
                 }
                 catch (Exception Ex)
                 {
                     Dispose(Conn);

                     return_ht["return_ds"] = null;
                     return_ht["return_float"] = false;
                     return_ht["return_errmsg"] = Ex.ToString();
                     return_ht["return_other"] = -1;

                     return return_ht;
                 }
             }
        }




        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行动态语句。 不返回数据集， 会返回受影响行数。
        /// 常用于执行动态语句进行插入、更新操作，不能用于查询操作。
        /// </summary>
        /// <param name="P_cmd">带参数的语句</param>
        /// <param name="P_ht_in">参数哈希表</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable I_Dblink.RunParam_SQL(string P_cmd,  Hashtable P_ht_in)
        {
            SqlConnection Conn = new SqlConnection(ConnStr);
            try
            {
                int kp = 0;
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.Connection = Conn;
                Da.SelectCommand.CommandText = P_cmd;
                Da.SelectCommand.CommandType = CommandType.Text;

                //循环添加存储过程参数
                IDictionaryEnumerator myEnumerator = P_ht_in.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    SqlParameter param = new SqlParameter();
                    param.Direction = ParameterDirection.Input;
                    param.ParameterName = myEnumerator.Key.ToString();
                    if (myEnumerator.Value == null)
                    {
                        // myEnumerator.Value = DBNull.Value;
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = myEnumerator.Value;
                    }
                    Da.SelectCommand.Parameters.Add(param);
                }

              
                kp = kp + Da.SelectCommand.ExecuteNonQuery();


               // kp = Da.Fill(Ds);
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = kp;

                //===============================================by galaxy=====================
                //开启一个线程，执行日志处理 
                ThreadStart gogo = delegate { StartSaveRedis(P_cmd, P_ht_in, kp); };
                Thread thread = new Thread(gogo);
                thread.Name = "StartSaveRedis";
                thread.IsBackground = true;
                //Thread.Sleep(20);
                thread.Start();
 
 
                
            


    

                return return_ht;
            }
            catch (Exception Ex)
            {
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = 0;

                return return_ht;
            }
        }




        /// <summary>
        /// 多语句执行，支持事务。
        /// 执行动态语句。 不返回数据集， 会返回受影响行数。
        /// 常用于执行动态语句进行插入、更新操作，不能用于查询操作。
        /// </summary>
        /// <param name="SQLStringList">多个语句数组</param>
        /// <param name="P_ht_in">参数哈希表</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，不会返回结果集</returns>
        Hashtable I_Dblink.RunParam_SQL(ArrayList SQLStringList, Hashtable P_ht_in)
        {
            if (SQLStringList == null || SQLStringList.Count < 1)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = "没有要执行的语句";
                return_ht["return_other"] = 0;

                return return_ht;
            }

            SqlConnection Conn = null;
            SqlTransaction tx = null;
            try
            {

                Conn = new SqlConnection(ConnStr);
                Conn.Open();
                tx = Conn.BeginTransaction();
                int kp = 0;
                ArrayList kpkp = new ArrayList();


                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.Connection = Conn;
                Da.SelectCommand.Transaction = tx;

                //循环添加存储过程参数
                IDictionaryEnumerator myEnumerator = P_ht_in.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    SqlParameter param = new SqlParameter();
                    param.Direction = ParameterDirection.Input;
                    param.ParameterName = myEnumerator.Key.ToString();
                    if (myEnumerator.Value == null)
                    {
                        // myEnumerator.Value = DBNull.Value;
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = myEnumerator.Value;
                    }
                    Da.SelectCommand.Parameters.Add(param);
                }

     
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        Da.SelectCommand.CommandText = strsql;
                        Da.SelectCommand.CommandType = CommandType.Text;
                        int kptemp = Da.SelectCommand.ExecuteNonQuery();
                        kp = kp + kptemp;
                        kpkp.Add(kptemp);
                    }
                }
                tx.Commit();
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = kp;


                //===============================================by galaxy=====================
                //开启一个线程，执行日志处理，批量处理 
                ThreadStart gogo = delegate { StartSaveRedis_al(SQLStringList, P_ht_in, kpkp); };
                Thread thread = new Thread(gogo);
                thread.Name = "StartSaveRedis_al";
                thread.IsBackground = true;
                //Thread.Sleep(20);
                thread.Start();

                return return_ht;
            }
            catch (Exception Ex)
            {
                if (tx != null)
                {
                    tx.Rollback();
                }
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = 0;

                return return_ht;
            }
        }
        /// <summary>
        /// 多语句执行，支持事务。
        /// 执行静态语句。 不返回数据集， 会返回受影响行数。
        /// 常用于执行静态语句进行插入、更新操作，不能用于查询操作。
        /// </summary>
        /// <param name="SQLStringList">多个语句数组</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，不会返回结果集，返回受影响的行数</returns>
        Hashtable I_Dblink.RunParam_SQL(ArrayList SQLStringList)
        {
            if (SQLStringList == null || SQLStringList.Count < 1)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = "没有要执行的语句";
                return_ht["return_other"] = 0;

                return return_ht;
            }

            SqlConnection Conn = null;
            SqlTransaction tx = null;

          

            try
            {

                Conn = new SqlConnection(ConnStr);
                Conn.Open();
                tx = Conn.BeginTransaction();
                int kp = 0;
                ArrayList kpkp = new ArrayList();

                SqlCommand cmd = new SqlCommand();
               
                cmd.Transaction = tx;
                cmd.Connection = Conn;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            int kptemp = cmd.ExecuteNonQuery();
                            kp = kp + kptemp;
                            kpkp.Add(kptemp);
                        }
                    }

                    tx.Commit();
                    Dispose(Conn);

                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = "";
                    return_ht["return_other"] = kp;

                    //===============================================by galaxy=====================
                    //开启一个线程，执行日志处理，批量处理 
                    Hashtable P_ht_in = new Hashtable();
                    ThreadStart gogo = delegate { StartSaveRedis_al(SQLStringList, P_ht_in, kpkp); };
                    Thread thread = new Thread(gogo);
                    thread.Name = "StartSaveRedis_al";
                    thread.IsBackground = true;
                    //Thread.Sleep(20);
                    thread.Start();
 
                    return return_ht;
            }
            catch (Exception Ex)
            {
                if (tx != null)
                {
                    tx.Rollback();
                }
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = 0;

                return return_ht;
            }
        }




        /// <summary>
        /// 无参数存储过程执行，事务在存储过程内自行实现。
        /// 会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, string DTname)
        {
            SqlConnection Conn = new SqlConnection(ConnStr);
            try
            {
                int kp = 0;
                DataSet dtreturn = new DataSet();
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.Connection = Conn;
                Da.SelectCommand.CommandText = P_cmd;
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;

                kp = Da.Fill(dtreturn);
                Dispose(Conn);
                if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                {
                    dtreturn.Tables[0].TableName = DTname;
                }
                return_ht["return_ds"] = dtreturn;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -1;

                return return_ht;
            }
            catch (Exception Ex)
            {
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = -1;

                return return_ht;
            }

        }



         /// <summary>
        /// 无参数存储过程执行，事务在存储过程内自行实现。
        /// 会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, string DTname, string RedisKey, string RedisPZ)
        {
            DataSet formRedis = RedisClass.RedisTryOnlyForDBhelper(RedisKey, RedisPZ);
            if (formRedis != null)
            {
                return_ht["return_ds"] = formRedis;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -2;
                return return_ht;
            }
            else
            {
                SqlConnection Conn = new SqlConnection(ConnStr);
                try
                {
                    int kp = 0;
                    DataSet dtreturn = new DataSet();
                    Conn.Open();
                    SqlDataAdapter Da = new SqlDataAdapter();
                    Da.SelectCommand = new SqlCommand();
                    Da.SelectCommand.Connection = Conn;
                    Da.SelectCommand.CommandText = P_cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    kp = Da.Fill(dtreturn);
                    Dispose(Conn);
                    if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                    {
                        dtreturn.Tables[0].TableName = DTname;
                    }
                    return_ht["return_ds"] = dtreturn;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = "";
                    return_ht["return_other"] = -1;

                    return return_ht;
                }
                catch (Exception Ex)
                {
                    Dispose(Conn);

                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = Ex.ToString();
                    return_ht["return_other"] = -1;

                    return return_ht;
                }
            }
        }

        /// <summary>
        /// 带参数存储过程执行，事务在存储过程内自行实现。
        /// 带输入参数，无输出参数，会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, string DTname, Hashtable P_ht_in)
        {
            SqlConnection Conn = new SqlConnection(ConnStr);
            try
            {
                int kp = 0;
                DataSet dtreturn = new DataSet();
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.Connection = Conn;
                Da.SelectCommand.CommandText = P_cmd;
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;

                //循环添加存储过程参数
                IDictionaryEnumerator myEnumerator = P_ht_in.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    SqlParameter param = new SqlParameter();
                    param.Direction = ParameterDirection.Input;
                    param.ParameterName = myEnumerator.Key.ToString();
                   // param.Value = myEnumerator.Value;
                    if (myEnumerator.Value == null)
                    {
                        // myEnumerator.Value = DBNull.Value;
                        param.Value = DBNull.Value;
                    }
                    else
                    {
                        param.Value = myEnumerator.Value;
                    }
                    Da.SelectCommand.Parameters.Add(param);
                }


                kp = Da.Fill(dtreturn);
                Dispose(Conn);
                if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                {
                    dtreturn.Tables[0].TableName = DTname;
                }
                return_ht["return_ds"] = dtreturn;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -1;

                return return_ht;
            }
            catch (Exception Ex)
            {
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = -1;

                return return_ht;
            }
        }


        /// <summary>
        /// 带参数存储过程执行，事务在存储过程内自行实现。
        /// 带输入参数，无输出参数，会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, string DTname, Hashtable P_ht_in, string RedisKey, string RedisPZ)
        {
            DataSet formRedis = RedisClass.RedisTryOnlyForDBhelper(RedisKey, RedisPZ);
            if (formRedis != null)
            {
                return_ht["return_ds"] = formRedis;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -2;
                return return_ht;
            }
            else
            {
                SqlConnection Conn = new SqlConnection(ConnStr);
                try
                {
                    int kp = 0;
                    DataSet dtreturn = new DataSet();
                    Conn.Open();
                    SqlDataAdapter Da = new SqlDataAdapter();
                    Da.SelectCommand = new SqlCommand();
                    Da.SelectCommand.Connection = Conn;
                    Da.SelectCommand.CommandText = P_cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    //循环添加存储过程参数
                    IDictionaryEnumerator myEnumerator = P_ht_in.GetEnumerator();
                    while (myEnumerator.MoveNext())
                    {
                        SqlParameter param = new SqlParameter();
                        param.Direction = ParameterDirection.Input;
                        param.ParameterName = myEnumerator.Key.ToString();
                        // param.Value = myEnumerator.Value;
                        if (myEnumerator.Value == null)
                        {
                            // myEnumerator.Value = DBNull.Value;
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            param.Value = myEnumerator.Value;
                        }
                        Da.SelectCommand.Parameters.Add(param);
                    }


                    kp = Da.Fill(dtreturn);
                    Dispose(Conn);
                    if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                    {
                        dtreturn.Tables[0].TableName = DTname;
                    }
                    return_ht["return_ds"] = dtreturn;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = "";
                    return_ht["return_other"] = -1;

                    return return_ht;
                }
                catch (Exception Ex)
                {
                    Dispose(Conn);

                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = Ex.ToString();
                    return_ht["return_other"] = -1;

                    return return_ht;
                }
            }
        }

        /// <summary>
        /// 带参数存储过程执行，事务在存储过程内自行实现。
        /// 带输入参数，带输出参数，会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="P_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, string DTname, Hashtable P_ht_in, ref Hashtable P_ht_out)
        {
            SqlConnection Conn = new SqlConnection(ConnStr);
            try
            {
                int kp = 0;
                DataSet dtreturn = new DataSet();
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.Connection = Conn;
                Da.SelectCommand.CommandText = P_cmd;
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;

                //循环添加输入参数
                IDictionaryEnumerator myEnumerator1 = P_ht_in.GetEnumerator();
                myEnumerator1.Reset();
                while (myEnumerator1.MoveNext())
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.Direction = ParameterDirection.Input;
                    param1.ParameterName = myEnumerator1.Key.ToString();
                    //param1.Value = myEnumerator1.Value;
                    if (myEnumerator1.Value == null)
                    {
                        // myEnumerator.Value = DBNull.Value;
                        param1.Value = DBNull.Value;
                    }
                    else
                    {
                        param1.Value = myEnumerator1.Value;
                    }
                    Da.SelectCommand.Parameters.Add(param1);
                }


                //循环添加输出参数
                IDictionaryEnumerator myEnumerator2 = P_ht_out.GetEnumerator();
                myEnumerator2.Reset();
                while (myEnumerator2.MoveNext())
                {
                    SqlParameter param2 = new SqlParameter();
                    param2.Direction = ParameterDirection.Output;
                    param2.ParameterName = myEnumerator2.Key.ToString();
                    param2.Value = myEnumerator2.Value;
                    Da.SelectCommand.Parameters.Add(param2);
                }

                kp = Da.Fill(dtreturn);




                //循环赋值新的输出参数
                Hashtable P_ht_out_temp = new Hashtable();
                IDictionaryEnumerator myEnumerator3 = P_ht_out.GetEnumerator();
                myEnumerator3.Reset();
                while (myEnumerator3.MoveNext())
                {
                    P_ht_out_temp.Add(myEnumerator3.Key.ToString(), Da.SelectCommand.Parameters[myEnumerator3.Key.ToString()].Value.ToString());
                }
                P_ht_out = P_ht_out_temp;


                Dispose(Conn);
                if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                {
                    dtreturn.Tables[0].TableName = DTname;
                }
                return_ht["return_ds"] = dtreturn;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -1;

                return return_ht;
            }
            catch (Exception Ex)
            {
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = -1;

                return return_ht;
            }
        }



                /// <summary>
        /// 带参数存储过程执行，事务在存储过程内自行实现。
        /// 带输入参数，带输出参数，会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="P_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, string DTname, Hashtable P_ht_in, ref Hashtable P_ht_out, string RedisKey, string RedisPZ)
        {
            DataSet formRedis = RedisClass.RedisTryOnlyForDBhelper(RedisKey, RedisPZ);
            if (formRedis != null)
            {
                return_ht["return_ds"] = formRedis;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = "";
                return_ht["return_other"] = -2;
                return return_ht;
            }
            else
            {
                SqlConnection Conn = new SqlConnection(ConnStr);
                try
                {
                    int kp = 0;
                    DataSet dtreturn = new DataSet();
                    Conn.Open();
                    SqlDataAdapter Da = new SqlDataAdapter();
                    Da.SelectCommand = new SqlCommand();
                    Da.SelectCommand.Connection = Conn;
                    Da.SelectCommand.CommandText = P_cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    //循环添加输入参数
                    IDictionaryEnumerator myEnumerator1 = P_ht_in.GetEnumerator();
                    myEnumerator1.Reset();
                    while (myEnumerator1.MoveNext())
                    {
                        SqlParameter param1 = new SqlParameter();
                        param1.Direction = ParameterDirection.Input;
                        param1.ParameterName = myEnumerator1.Key.ToString();
                        //param1.Value = myEnumerator1.Value;
                        if (myEnumerator1.Value == null)
                        {
                            // myEnumerator.Value = DBNull.Value;
                            param1.Value = DBNull.Value;
                        }
                        else
                        {
                            param1.Value = myEnumerator1.Value;
                        }
                        Da.SelectCommand.Parameters.Add(param1);
                    }


                    //循环添加输出参数
                    IDictionaryEnumerator myEnumerator2 = P_ht_out.GetEnumerator();
                    myEnumerator2.Reset();
                    while (myEnumerator2.MoveNext())
                    {
                        SqlParameter param2 = new SqlParameter();
                        param2.Direction = ParameterDirection.Output;
                        param2.ParameterName = myEnumerator2.Key.ToString();
                        param2.Value = myEnumerator2.Value;
                        Da.SelectCommand.Parameters.Add(param2);
                    }

                    kp = Da.Fill(dtreturn);




                    //循环赋值新的输出参数
                    Hashtable P_ht_out_temp = new Hashtable();
                    IDictionaryEnumerator myEnumerator3 = P_ht_out.GetEnumerator();
                    myEnumerator3.Reset();
                    while (myEnumerator3.MoveNext())
                    {
                        P_ht_out_temp.Add(myEnumerator3.Key.ToString(), Da.SelectCommand.Parameters[myEnumerator3.Key.ToString()].Value.ToString());
                    }
                    P_ht_out = P_ht_out_temp;


                    Dispose(Conn);
                    if (dtreturn.Tables.Count > 0 && DTname != null && DTname.Trim() != "")
                    {
                        dtreturn.Tables[0].TableName = DTname;
                    }
                    return_ht["return_ds"] = dtreturn;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = "";
                    return_ht["return_other"] = -1;

                    return return_ht;
                }
                catch (Exception Ex)
                {
                    Dispose(Conn);

                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = Ex.ToString();
                    return_ht["return_other"] = -1;

                    return return_ht;
                }
            }
        }



        private void StartSaveRedis(string P_cmd, Hashtable P_ht_in, int kp)
        {
            try
            {
                //返回前，若存在受影响行数，调用Redis处理类SQLredis。 目前只支持普通的标准格式，不严格，实际使用，要具体验证一下。
                //条件只支持and,带or的会出错的。条件也只能是=号， 不等号，大于号之类的都不行。
                if (kp > 0)
                {
                    string typeS = "";
                    //获取表名同时确认语句类型
                    Regex r = new Regex(" update " + "(.*?)" + " set ", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    Match m = r.Match("    " + P_cmd);
                    string tablename = "";
                    tablename = m.Success ? m.Groups[1].Value.Replace("[", "").Replace("]", "").Trim() : "";
                    if (tablename != "") //这是更新
                    {
                        typeS = "更新";
                    }
                    else
                    {
                        Regex r2 = new Regex(" insert " + "(.*?)" + "\\(", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        Match m2 = r2.Match("    " + P_cmd);
                        tablename = m2.Success ? Regex.Replace(m2.Groups[1].Value, "into ", "", RegexOptions.IgnoreCase).Replace("[", "").Replace("]", "").Trim() : "";
                        if (tablename != "") //这是插入
                        {
                            typeS = "插入";
                        }
                    }
                    if (typeS == "更新" || typeS == "插入")
                    {
                        string dllfile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Bin\\SqlRedisForFMDBHelper_" + tablename + ".dll";
                        if (File.Exists(dllfile))
                        {

                            //更新或插入字段和值
                            Dictionary<string, string> filed = new Dictionary<string, string>();
                            //更新的条件和值
                            Dictionary<string, string> where = new Dictionary<string, string>();

                            switch (typeS)
                            {
                                case "更新":
                                    Regex r_g = new Regex(" set " + "(.*?)" + " where ", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                                    string[] temp = r_g.Match(P_cmd + " where ").Groups[1].Value.Trim().Split(',');
                                    foreach (string OneZDtemp in temp)
                                    {
                                        string[] ZD = OneZDtemp.Split('=');
                                        filed.Add(ZD[0].Replace("[", "").Replace("]", "").Trim(), ZD[1].Replace("'", "").Trim());
                                    }
                                    if (P_cmd.ToLower().IndexOf(" where ") > 0)
                                    {
                                        Regex r_g2 = new Regex(" where " + "(.*?)" + " 最后补位 ", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                                        string[] temp2 = Regex.Replace(r_g2.Match(P_cmd + " 最后补位 ").Groups[1].Value.Trim(), " and ", "★", RegexOptions.IgnoreCase).Split('★');
                                        foreach (string OneZDtemp in temp2)
                                        {
                                            string[] TJ = OneZDtemp.Split('=');
                                            where.Add(TJ[0].Replace("[", "").Replace("]", "").Trim(), TJ[1].Replace("'", "").Trim());
                                        }
                                    }
                                    break;
                                case "插入":
                                    Regex r_g3 = new Regex("\\(" + "(.*?)" + " 最后补位 ", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                                    string[] temp3 = Regex.Replace(Regex.Replace("    " + r_g3.Match("    " + P_cmd + " 最后补位 ").Groups[1].Value, " into ", "", RegexOptions.IgnoreCase).Replace(" ",""), "\\)values\\(", "★", RegexOptions.IgnoreCase).Split('★');
                                    string[] zd_name = temp3[0].Replace("(", "").Replace(")", "").Split(',');
                                    string[] zd_value = temp3[1].Replace("(", "").Replace(")", "").Split(',');
                                    for (int p = 0; p < zd_name.Length; p++)
                                    {
                                        filed.Add(zd_name[p].Replace("'", "").Replace("[", "").Replace("]", "").Trim(), zd_value[p].Replace("'", "").Trim());
                                    }

                                    break;
                                default:
                                    break;
                            }





                            //开始调用处理
                            Assembly serviceAsm = GetCache.InstanceCacheEx.InstanceCache(dllfile);
                            Type typeName = serviceAsm.GetType("SqlRedisForFMDBHelper_" + tablename + "." + "GO");
                            object instance = serviceAsm.CreateInstance("SqlRedisForFMDBHelper_" + tablename + "." + "GO");
                            object rtnObj = typeName.GetMethod("TryUpdateRedis").Invoke(instance, new object[] { typeS, P_cmd, P_ht_in, tablename, filed, where });
                            //若发生错误，写入日志
                            if (rtnObj == null || rtnObj.ToString() != "ok")
                            {
                                ;
                            }
                        }
                    }


                }
                //===============================================
            }
            catch (Exception exx)
            {
                string trytype = "Redis失败：" + exx.ToString();
                //写入日志
            }
        }

        private void StartSaveRedis_al(ArrayList SQLStringList, Hashtable P_ht_in, ArrayList kpkp)
        {
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    StartSaveRedis(SQLStringList[n].ToString(), P_ht_in, Convert.ToInt32(kpkp[n]));
                }
            }
            catch (Exception exx)
            {
                string trytype = "Redis失败失败失败：" + exx.ToString();
                //写入日志
            }
        }

    }
}
