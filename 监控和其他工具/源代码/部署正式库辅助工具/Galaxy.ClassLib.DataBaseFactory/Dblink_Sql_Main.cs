using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace Galaxy.ClassLib.DataBaseFactory
{
    /// <summary>
    /// ���ڻ����й���վ�������κ�ϵͳͨ��
    /// MS SQL���ݿ�������,�����Ʒ��ɫ(����Ϊ��Ѷϵͳ�趨������ϵͳ��������Ľ�ɫ)
    /// �����б�:
    /// 1.��ʼ�����ݿ�����
    /// 2.���Ͳ�ͬ������ִ��sql��䣬ͨ����ϣ����ִ�н������
    /// </summary>
    public class Dblink_Sql_Main : I_Dblink
    {

        #region �� �� �� �� �� �� �� �� �� ʼ ��

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        private string ConnStr = "";

        /// <summary>
        /// ���ڷ���ֵ���ϵĹ�ϣ��
        /// ��ϣ����ֵ˵��:
        /// return_ht["return_ds"] = null; ���ص�ִ�н�����ݼ���DataSet����
        /// return_ht["return_float"] = null; ���ص�ִ�н����־��ִ�гɹ����ҷ��ص����ݴ���0������Ϊtrue
        /// return_ht["return_errmsg"] = null; ���صĴ��󲶻���Ϣ��string����
        /// return_ht["return_other"] = null; ���ص�����ֵ���ͣ���;���������Ͳ���
        /// </summary>
        private Hashtable return_ht = new Hashtable();

        #endregion

        /// <summary>
        /// MS SQL���ݿ������๹�캯��
        /// </summary>
        /// <param name="ConnConfig">web.config�����ļ���appSettings�¹������ݿ������ֶε�KEY</param>
        public Dblink_Sql_Main(string ConnConfig)
        {
            ConnStr = get_info(ConnConfig);
            //��ʼ����ϣ����ֵ
            return_ht.Add("return_ds", null);//��Ҫ���ص����ݼ�
            return_ht.Add("return_float", null);//����ִ�н��
            return_ht.Add("return_errmsg", null);//���󲶻���Ϣ
            return_ht.Add("return_other", null);//�������ⷵ��ֵ
        }

        /// <summary>
        /// ��ע��������ݿ��������Ϣ
        /// </summary>
        /// <param name="dblink">�����ַ���</param>
        public void setregedit(string dblink)
        {
            RegistryKey hklm;
            hklm = Registry.CurrentUser;
            RegistryKey software;
            software = hklm.OpenSubKey("Software", true);
            RegistryKey mykey;
            mykey = software.CreateSubKey("С��Ʒɨ�빺��");
            mykey.SetValue("dblink", dblink);
            hklm.Close();
        }
        /// <summary>
        /// ��ע����ȡ���ݿ��������Ϣ
        /// </summary>
        /// <returns>�����ַ�������</returns>
        public string[] getregedit()
        {
            //
            string[] temp_key = { "", "", "", "" };
            RegistryKey pregkey;
            pregkey = Registry.CurrentUser.OpenSubKey("Software\\С��Ʒɨ�빺��", true);
            if (pregkey == null)
            {
                setregedit(@"user id=sa;password=100zzcom;initial catalog=sm;Server=192.168.120.20;Connect Timeout=30");
                pregkey = Registry.CurrentUser.OpenSubKey("Software\\С��Ʒɨ�빺��", true);
            }
            temp_key.SetValue(pregkey.GetValue("dblink").ToString(), 0);
            return temp_key;
        }

        /// <summary>
        /// ��ȡ�����ļ�
        /// </summary>
        private string get_info(string ConnConfig)
        {
            if (ConnConfig == "����")
            {
                return getregedit().GetValue(0).ToString();
            }
            else
            {
                return ConnConfig;
            }
        }


        /// <summary>
        /// ����������,����SqlConnection����
        /// </summary>
        /// <param name="Conn">���ݿ����Ӷ���</param>
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
        /// ��XML�ļ��������ݿ�
        /// </summary>
        /// <param name="oldtempcmd">���ڻ�ȡ���ݽṹ��ԭʼ��ʱsql���</param>
        /// <param name="XMLpath">�ļ�·��</param>
        /// <returns>���ؽ�����϶�Ӧ�Ĺ�ϣ��,����ִ���Ƿ�ɹ�,��Ӱ�������</returns>
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
        /// ����SQL���
        /// </summary>
        /// <param name="SQL">��Ҫִ�е�sql���</param>
        /// <returns>���ؽ�����϶�Ӧ�Ĺ�ϣ��,����ִ���Ƿ�ɹ�,��Ӱ�������</returns>
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
        /// ����SQL���
        /// </summary>
        /// <param name="SQL">SQL���</param>
        /// <param name="Ds">DataSet����</param>
        /// <returns>���ؽ�����϶�Ӧ�Ĺ�ϣ��,����ִ���Ƿ�ɹ���ִ�н�������ݼ�</returns>
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
                //���ص���������1��,���������ݼ�
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "û���ҵ��κ�����";
                    return_ht["return_other"] = null;
                }
                //�������ݼ�
                else
                {
                    return_ht["return_ds"] = Ds;
                    return_ht["return_float"] = true;
                    return_ht["return_errmsg"] = null;
                    return_ht["return_other"] = kp;
                }



                return return_ht;
            }
            //ִ��ʧ��
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
        /// ����SQL���
        /// </summary>
        /// <param name="SQL">SQL���</param>
        /// <param name="Ds">DataSet����</param>
        /// <param name="tablename">����</param>
        /// <returns>���ؽ�����϶�Ӧ�Ĺ�ϣ��,����ִ���Ƿ�ɹ���ִ�н�������ݼ�</returns>
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
                //���ص���������1��,���������ݼ�
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "û���ҵ��κ�����";
                    return_ht["return_other"] = null;
                }
                //�������ݼ�
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
        /// ���д洢����
        /// </summary>
        /// <param name="P_cmd">�洢��������</param>
        /// <param name="Ds">DataSet����</param>
        /// <returns>���ؽ�����϶�Ӧ�Ĺ�ϣ��,����ִ���Ƿ�ɹ���ִ�н�������ݼ�</returns>
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
                //���ص���������1��,���������ݼ�
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "û���ҵ��κ�����";
                    return_ht["return_other"] = null;
                }
                //�������ݼ�
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
        /// ���д洢����
        /// </summary>
        /// <param name="P_cmd">�洢��������</param>
        /// <param name="Ds">DataSet����</param>
        /// <param name="P_ht_in">��ϣ����Ӧ�洢���̴������,keysΪ������ǣ�valuesΪ����ֵ</param>
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

                //ѭ����Ӵ洢���̲���
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
                //���ص���������1��,���������ݼ�
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "û���ҵ��κ�����";
                    return_ht["return_other"] = null;
                }
                //�������ݼ�
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
        /// ���д洢����
        /// </summary>
        /// <param name="P_cmd">�洢��������</param>
        /// <param name="Ds">DataSet����</param>
        /// <param name="P_ht_in">��ϣ����Ӧ�洢���̴������</param>
        /// <param name="P_ht_out">��ϣ����Ӧ�洢���̴�������,��ַ</param>
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

                //ѭ������������
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


                //ѭ������������
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




                //ѭ����ֵ�µ��������
                Hashtable P_ht_out_temp = new Hashtable();
                IDictionaryEnumerator myEnumerator3 = P_ht_out.GetEnumerator();
                myEnumerator3.Reset();
                while (myEnumerator3.MoveNext())
                {
                    P_ht_out_temp.Add(myEnumerator3.Key.ToString(), Da.SelectCommand.Parameters[myEnumerator3.Key.ToString()].Value.ToString());
                }
                P_ht_out = P_ht_out_temp;


                Dispose(Conn);
                //���ص���������1��,���������ݼ�
                if (kp == 0)
                {
                    return_ht["return_ds"] = null;
                    return_ht["return_float"] = false;
                    return_ht["return_errmsg"] = "û���ҵ��κ�����";
                    return_ht["return_other"] = null;
                }
                //�������ݼ�
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
