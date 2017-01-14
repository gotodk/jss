using FMipcClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Collections;

namespace IPC配置更新工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private string tihuan()
        {
            ConfigurationManager.RefreshSection("ConnectionStrings");
            string GX_shibie = ConfigurationManager.ConnectionStrings["ThisAppPoolName"].ToString();//用进程池名称作为标识

            ArrayList mytxt = new ArrayList();
            using (System.IO.StreamReader reader = new System.IO.StreamReader(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "//IPC配置更新工具.exe.config", Encoding.UTF8)) { while (!reader.EndOfStream) { mytxt.Add(reader.ReadLine().Replace("add name=\"ThisAppPoolName\" connectionString=\"" + GX_shibie + "\"", "add name=\"ThisAppPoolName\" connectionString=\"" + textBox1.Text.Trim() + "\"")); } } StreamWriter writer = new StreamWriter(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "//IPC配置更新工具.exe.config"); for (int i = 0; i < mytxt.Count; i++) { writer.WriteLine(mytxt[i]); } writer.Close();


            ConfigurationManager.RefreshSection("connectionStrings");
            string GX_shibie2 = ConfigurationManager.ConnectionStrings["ThisAppPoolName"].ToString();//用进程池名称作为标识
            if (GX_shibie2 != textBox1.Text.Trim())
            {
                MessageBox.Show("重新APP.CONFIG配置有问题，没有得到新的名称"); return "err";
            }

            return "ok";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {


                if (tihuan() == "err")
                {
                    button1.Enabled = true;
                    return;
                }


                //string a1 = HelpConfig.huishou(); //回收应用程序池
                string a2 = HelpConfig.huoquguanxi();//获取最新关系到本地
                string a3 = HelpConfig.shengchengdaili();//重新生成代理类
                MessageBox.Show("___获取最新关系到本地" + a2 + "___重新生成代理类" + a3 + "|||");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            button1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigurationManager.RefreshSection("ConnectionStrings");
            string GX_shibie = ConfigurationManager.ConnectionStrings["ThisAppPoolName"].ToString();//用进程池名称作为标识
            textBox1.Text = GX_shibie;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string a1 = (string)(null);
                DataSet a2 = (DataSet)(null);
                DataTable a3 = (DataTable)(null);
                string[][] a4 = (string[][])(null);
             
                object a6 = (object)(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}
