using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace Galaxy.ClassLib.DataBaseFactory
{
    /// <summary>
    /// 用于华商中国主站，并且任何系统通用
    /// MS SQL数据库连接类,具体产品角色(此类为资讯系统设定，其他系统调用另外的角色)
    /// 功能列表:
    /// 1.初始化数据库连接
    /// 2.传送不同参数，执行sql语句，通过哈希表返回执行结果集合
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
        /// <param name="ConnConfig">web.config配置文件中appSettings下关于数据库连接字段的KEY</param>
        public Dblink_Sql_Main(string ConnConfig)
        {
            ConnStr = get_info(ConnConfig);
            //初始化哈希表返回值
            return_ht.Add("return_ds", null);//需要返回的数据集
            return_ht.Add("return_float", null);//方法执行结果
            return_ht.Add("return_errmsg", null);//错误捕获消息
            return_ht.Add("return_other", null);//其他特殊返回值
        }

        /// <summary>
        /// 从注册表创建数据库服务器信息
        /// </summary>
        /// <param name="dblink">连接字符串</param>
        public void setregedit(string dblink)
        {
            RegistryKey hklm;
            hklm = Registry.CurrentUser;
            RegistryKey software;
            software = hklm.OpenSubKey("Software", true);
            RegistryKey mykey;
            mykey = software.CreateSubKey("小商品扫码购物");
            mykey.SetValue("dblink", dblink);
            hklm.Close();
        }
        /// <summary>
        /// 从注册表读取数据库服务器信息
        /// </summary>
        /// <returns>连接字符串数组</returns>
        public string[] getregedit()
        {
            //
            string[] temp_key = { "", "", "", "" };
            RegistryKey pregkey;
            pregkey = Registry.CurrentUser.OpenSubKey("Software\\小商品扫码购物", true);
            if (pregkey == null)
            {
                setregedit(@"user id=sa;password=100zzcom;initial catalog=sm;Server=192.168.120.20;Connect Timeout=30");
                pregkey = Registry.CurrentUser.OpenSubKey("Software\\小商品扫码购物", true);
            }
            temp_key.SetValue(pregkey.GetValue("dblink").ToString(), 0);
            return temp_key;
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        private string get_info(string ConnConfig)
        {
            if (ConnConfig == "本地")
            {
                return getregedit().GetValue(0).ToString();
            }
            else
            {
                return ConnConfig;
            }
        }


        /// <summary>
        /// 垃圾处理方法,销毁SqlConnection对象
        /// </summary>
        /// <param name="Conn">数据库连接对象</param>
        private void Dispose(SqlConnection Conn)
        {
            if (Conn != null)
            {
                Conn.Close();
                Conn.Dispose();
            }
            GC.Collect();
        }

        /// <summary>
        /// 从XML文件更新数据库
        /// </summary>
        /// <param name="oldtempcmd">用于获取数据结构的原始临时sql语句</param>
        /// <param name="XMLpath">文件路径</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功,受影响的行数</returns>
        Hashtable I_Dblink.UpdateMore(string oldtempcmd, string XMLpath)
        {
            try
            {
                SqlConnection Conn;
                Conn = new SqlConnection(ConnStr);
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter(oldtempcmd, Conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(Da);
                DataSet ds = new DataSet();
                Da.Fill(ds);
                ds.ReadXml(XMLpath, XmlReadMode.ReadSchema);
                int k = Da.Update(ds);
                Dispose(Conn);

                return_ht["return_ds"] = null;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = null;
                return_ht["return_other"] = k.ToString();

                return return_ht;

            }
            catch (Exception Ex)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = null;

                return return_ht;
            }

        }


        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="SQL">需要执行的sql语句</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功,受影响的行数</returns>
        Hashtable I_Dblink.RunProc(string SQL)
        {
            try
            {
                int sf = 0;
                SqlConnection Conn;
                Conn = new SqlConnection(ConnStr);
                Conn.Open();
                SqlCommand Cmd = new SqlCommand(SQL, Conn);
                sf = Cmd.ExecuteNonQuery();
                Dispose(Conn);
                return_ht["return_ds"] = null;
                return_ht["return_float"] = true;
                return_ht["return_errmsg"] = null;
                return_ht["return_other"] = sf;

                return return_ht;
            }
            catch (Exception exp)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = exp.ToString();
                return_ht["return_other"] = null;

                return return_ht;
            }
        }


        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <param name="Ds">DataSet对象</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable I_Dblink.RunProc(string SQL, DataSet Ds)
        {
            try
            {
                int kp = 0;
                SqlConnection Conn;
                Conn = new SqlConnection(ConnStr);
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter(SQL, Conn);
                kp = Da.Fill(Ds);
                Dispose(Conn);
                //返回的数据少于1条,不返回数据集
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "没有找到任何数据";
                    return_ht["return_other"] = null;
                }
                //返回数据集
                else
                {
                    return_ht["return_ds"] = Ds;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = null;
                    return_ht["return_other"] = kp;
                }



                return return_ht;
            }
            //执行失败
            catch (Exception Err)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Err.ToString();
                return_ht["return_other"] = null;

                return return_ht;
            }
        }

        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <param name="Ds">DataSet对象</param>
        /// <param name="tablename">表名</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable I_Dblink.RunProc(string SQL, DataSet Ds, string tablename)
        {
            try
            {
                int kp = 0;
                SqlConnection Conn;
                Conn = new SqlConnection(ConnStr);
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter(SQL, Conn);
                kp = Da.Fill(Ds, tablename);
                Dispose(Conn);
                //返回的数据少于1条,不返回数据集
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "没有找到任何数据";
                    return_ht["return_other"] = null;
                }
                //返回数据集
                else
                {
                    return_ht["return_ds"] = Ds;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = null;
                    return_ht["return_other"] = kp;
                }

                return return_ht;
            }
            catch (Exception Ex)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = null;

                return return_ht;
            }
        }

        /// <summary>
        /// 运行存储过程
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="Ds">DataSet对象</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, DataSet Ds)
        {
            try
            {
                int kp = 0;
                SqlConnection Conn;
                Conn = new SqlConnection(ConnStr);
                Conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = new SqlCommand();
                Da.SelectCommand.Connection = Conn;
                Da.SelectCommand.CommandText = P_cmd;
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;

                kp = Da.Fill(Ds);
                Dispose(Conn);
                //返回的数据少于1条,不返回数据集
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "没有找到任何数据";
                    return_ht["return_other"] = null;
                }
                //返回数据集
                else
                {
                    return_ht["return_ds"] = Ds;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = null;
                    return_ht["return_other"] = kp;
                }

                return return_ht;
            }
            catch (Exception Ex)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = null;

                return return_ht;
            }

        }

        /// <summary>
        /// 运行存储过程
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="Ds">DataSet对象</param>
        /// <param name="P_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, DataSet Ds, Hashtable P_ht_in)
        {
            try
            {
                int kp = 0;
                SqlConnection Conn;
                Conn = new SqlConnection(ConnStr);
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
                    param.Value = myEnumerator.Value;
                    Da.SelectCommand.Parameters.Add(param);
                }


                kp = Da.Fill(Ds);
                Dispose(Conn);
                //返回的数据少于1条,不返回数据集
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "没有找到任何数据";
                    return_ht["return_other"] = null;
                }
                //返回数据集
                else
                {
                    return_ht["return_ds"] = Ds;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = null;
                    return_ht["return_other"] = kp;
                }

                return return_ht;
            }
            catch (Exception Ex)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = null;

                return return_ht;
            }
        }

        /// <summary>
        /// 运行存储过程
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="Ds">DataSet对象</param>
        /// <param name="P_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="P_ht_out">哈希表，对应存储过程传出参数,传址</param>
        Hashtable I_Dblink.RunProc_CMD(string P_cmd, DataSet Ds, Hashtable P_ht_in, ref Hashtable P_ht_out)
        {
            try
            {
                int kp = 0;
                SqlConnection Conn;
                Conn = new SqlConnection(ConnStr);
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
                    param1.Value = myEnumerator1.Value;
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

                kp = Da.Fill(Ds);




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
                //返回的数据少于1条,不返回数据集
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "没有找到任何数据";
                    return_ht["return_other"] = null;
                }
                //返回数据集
                else
                {
                    return_ht["return_ds"] = Ds;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = null;
                    return_ht["return_other"] = kp;
                }

                return return_ht;
            }
            catch (Exception Ex)
            {
                return_ht["return_ds"] = null;
                return_ht["return_float"] = false;
                return_ht["return_errmsg"] = Ex.ToString();
                return_ht["return_other"] = null;

                return return_ht;
            }
        }
    }
}
