using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 配置文件管理
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void readandshow()
        {
            客户端主程序.DataControl.XMLConfig.ReadConfig();
            dataGridView1.DataSource = 客户端主程序.DataControl.XMLConfig.DSConfig.Tables[0].DefaultView;
            dataGridView2.DataSource = 客户端主程序.DataControl.XMLConfig.DSConfig.Tables[1].DefaultView;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //readandshow();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            客户端主程序.DataControl.XMLConfig.SaveConfig();
            readandshow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            客户端主程序.DataControl.XMLConfig.initConfig();
            readandshow();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            readandshow();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = 客户端主程序.Support.StringOP.encMe(textBox1.Text, "mimamima");
            }
            catch { }
            


        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
            textBox3.Text = 客户端主程序.Support.StringOP.uncMe(textBox4.Text, "mimamima");
            }
            catch { }
        }
    }
}
