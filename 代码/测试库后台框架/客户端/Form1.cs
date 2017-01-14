using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FMipcClass;
using System.DirectoryServices;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Xml;

namespace 客户端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

 

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int p = 0; p < 50; p++)
            {
                Class1 cc = new Class1();
                Thread Thread1 = new Thread(cc.runn);
                Thread1.Start();
            }
            FMWScenter wsd = new FMWScenter("http://192.168.16.8:8001/s1.asmx?wsdl");
            string suc = (string)wsd.ExecuteQuery("test", null);
        
 
        }

        public void runn()
        {
            while (DateTime.Now.Minute < 13)
            {

            }
            if (DateTime.Now.Minute > 8 && DateTime.Now.Second > 2 && DateTime.Now.Millisecond > 3)
            {
                FMWScenter wsd = new FMWScenter("http://192.168.16.8:8001/s1.asmx?wsdl");
                string suc = (string)wsd.ExecuteQuery("test", null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FMWScenter wsd = new FMWScenter();
            wsd.GetNewWSDL(new string[] { "http://192.168.16.8:8001/s1.asmx?wsdl","http://192.168.16.8:8002/s2.asmx?wsdl" });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string AppPoolName = "一组独立服务";    //应用程序池命名，当找不到该应用程序池时，系统会报找不到指定路径
            string method = "Recycle";            //启动命令， 停止：“Stop” ； 回收：“Recycle”
            try
            {
                DirectoryEntry appPool = new DirectoryEntry("IIS://127.0.0.1/W3SVC/AppPools");
                DirectoryEntry findPool = appPool.Children.Find(AppPoolName, "IIsApplicationPool");
                findPool.Invoke(method, null);
                appPool.CommitChanges();
                appPool.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime dt1 = DateTime.Now;
            FMWScenter wsd = new FMWScenter("http://192.168.16.8:8002/s2.asmx?wsdl");
            string suc = (string)wsd.ExecuteQuery("test333", new string[] { "这是参数哦.." });
            DateTime dt2 = DateTime.Now;
            richTextBox1.AppendText(suc + "\n\r");
            richTextBox1.AppendText((dt1 - dt2).Milliseconds + "\n\r");
        }

        /// <summary>
        /// 对象序列化为byte[]
        /// </summary>
        /// <param name="o">对象object</param>
        /// <returns>byte[]</returns>
        public byte[] serializeTobyte(object o)
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            serializer.Serialize(memStream, o);
            byte[] cs_byte = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return cs_byte;
        }

        /// <summary>
        /// 对象序列化为byte[]
        /// </summary>
        /// <param name="o">对象object[]</param>
        /// <returns>byte[]</returns>
        public byte[] serializeTobyte(object[] o)
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            serializer.Serialize(memStream, o);
            byte[] cs_byte = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return cs_byte;
        }

        /// <summary>
        /// 多个对象，合并后序列化为byte[]
        /// </summary>
        /// <param name="o">对象object[]</param>
        /// <returns></returns>
        public byte[] serializeTobyteDG(object[] o)
        {
            object[] temp = new object[o.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = serializeTobyte(o[i]);
            }
            return serializeTobyte(temp);
        }


        private void button5_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                DataTable dt = new DataTable();
                dt.TableName = "ss";
                object[] ccccc = new object[] { dt,1,"xx" };



                byte[] cs_byte = serializeTobyteDG(ccccc);

                //将序列化后的参数进行md5加密,不需要复杂加密，因为这是防止修改已抓到的包。
                string cs_md5_string = "";
                MD5 md5 = MD5.Create();//实例化一个md5对像
                // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                byte[] cs_md5_byte = md5.ComputeHash(cs_byte);
                for (int i = 0; i < cs_md5_byte.Length; i++)
                {
                    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                    cs_md5_string = cs_md5_string + cs_md5_byte[i].ToString("X");

                }

                MessageBox.Show(cs_md5_string);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "|" + ex.InnerException.Message);
            }
            */
            try
            {
        
                DataTable dt = new DataTable("cc");
                dt.Columns.Add("列1");
                dt.Rows.Add("值");
                object[] objCS = new object[] { "111111111111111111111111111", dt,null };


                string mainfile_str = "";
                for (int i = 0; i < objCS.Length;i++ )
                {
                    string thisfilename = "xxx_" + i + "_" + objCS[i].GetType().AssemblyQualifiedName + ".txt";
                    FileStream stream = new FileStream(thisfilename, FileMode.Create);
                    if (i == objCS.Length - 1)
                    {
                        mainfile_str = mainfile_str + thisfilename;
                    }
                    else
                    {
                        mainfile_str = mainfile_str + thisfilename + "|";
                    }
                 
                    XmlSerializer serializer = new XmlSerializer(objCS[i].GetType());
                    serializer.Serialize(stream, objCS[i]);
                    stream.Close();
                }
                File.WriteAllText("xxx.txt", mainfile_str);
 
                 
             
         

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message + "|" +ex.ToString());
            }

        }



  

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                string mainfile_str = File.ReadAllText("xxx.txt");
                string[] files = mainfile_str.Split('|');

                object[] CS = new object[files.Length];
                for(int i = 0; i < files.Length;i++)
                {
                    //把参数反序列化出来
                    FileStream stream = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                    string typename = files[i].Split('_')[2].Replace(".txt", "");
                    Type ty = Type.GetType(typename);
                    XmlSerializer serializer = new XmlSerializer(ty);
                    CS[i] = serializer.Deserialize(stream);
                    stream.Close();
                }
                MessageBox.Show(CS.ToString());

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message + "|" + ex.ToString());
            }

        }
    }
}
