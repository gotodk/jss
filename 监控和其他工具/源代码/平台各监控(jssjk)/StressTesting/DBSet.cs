using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using StressTesting;

namespace StressTesting
{
    public partial class DBSet : Form
    {
        string CurrentPath = System.Environment.CurrentDirectory.ToString() + @"\"; //得到当前路径

        public DBSet()
        {
            InitializeComponent();

        }
        //窗体加载
        private void DBSet_Load(object sender, EventArgs e)
        {
            GetConfig();
        }
        //确定按钮
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SetConfig() == true)
            {
                this.timer1.Enabled = false;
                Test();
                //this.Close();

            }
        }
        //取消按钮
        private void btnQuit_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (lblTime.Visible == true)
            {
                Application.Exit();
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 获取数据库连接配置
        /// </summary>
        private void GetConfig()
        {
            if (!File.Exists(CurrentPath + "ConSet.xml"))
            {
                BasicShare.ShowMessage("没有ConSet.xml文件");
                return;
            }
            XmlClass xc = new XmlClass(CurrentPath + "ConSet.xml");
            XmlNodeList nlist = xc.GetNodeList("Appsetting").Item(0).ChildNodes;
            foreach (XmlNode n in nlist)
            {
                if (n.Name == "ServerName")
                {
                    txtServerName.Text = n.InnerText;
                }
                if (n.Name == "DbName")
                {
                    txtDBName.Text = n.InnerText;
                }
                if (n.Name == "UserName")
                {
                    txtUserName.Text = n.InnerText;
                }
                if (n.Name == "Password")
                {
                    txtPassWord.Text = n.InnerText;
                }
            }
        }
        /// <summary>
        /// 设置数据库连接配置
        /// </summary>
        private Boolean SetConfig()
        {
            if (!File.Exists(CurrentPath + "ConSet.xml"))
            {
                XmlClass.CreateXmlFile(CurrentPath + "ConSet.xml");

            }

            XmlClass xc = new XmlClass(CurrentPath + "ConSet.xml");

            XmlNodeList nlist = xc.GetNodeList("Appsetting").Item(0).ChildNodes; //得到节点列表
            foreach (XmlNode n in nlist)         //遍历节点列表
            {
                if (n.Name == "ServerName")
                {
                    n.InnerText = txtServerName.Text;

                }
                if (n.Name == "DbName")
                {
                    n.InnerText = txtDBName.Text;
                }
                if (n.Name == "UserName")
                {
                    n.InnerText = txtUserName.Text;
                }
                if (n.Name == "Password")
                {
                    n.InnerText = txtPassWord.Text;
                }

            }
            xc.Savexml();


            BasicShare.ShowMessage("保存成功");
            return true;

        }
        //倒计时重新启动
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lblTime.Text = Convert.ToString(Convert.ToInt16(lblTime.Text) - 1);
            if (lblTime.Text == "0")
            {
                BasicShare.AutoRun();
                Application.Exit();
            }
        }

        public void Test()
        {
            DBClass dc = new DBClass();
            if (dc.isCon() == false)
            {
                MessageBox.Show("数据连接不上,请重新设置！");


            }
            else
            {
                MainForm.connectionString = dc.getConStr();
                this.Close();
            }

        }
    }
}
