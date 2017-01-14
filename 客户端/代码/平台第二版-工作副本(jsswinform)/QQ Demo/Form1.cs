using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Runtime.InteropServices;

namespace QQ_Demo
{
    public partial class Form1 : BasicForm
    {






        public Form1()
        {
            InitializeComponent();
            TaskMenu.Show(this);//增加任务栏右键菜单
        }

        private void basicButton2_Click(object sender, EventArgs e)
        {
            MsgBox.Show("弹出对话框测试1.");
        }

        private void basicButton3_Click(object sender, EventArgs e)
        {
            MsgBox.Show(this,"弹出对话框测试2.");
        }

        private void basicButton4_Click(object sender, EventArgs e)
        {
            MsgBox.Show("测试", "弹出对话框测试3.");
        }

        private void basicButton5_Click(object sender, EventArgs e)
        {
            MsgBox.Show(this, "测试", "弹出对话框测试4.");
        }

        private void basicButton6_Click(object sender, EventArgs e)
        {
            MsgBox.Show(this, "测试", "弹出对话框测试5.", MessageBoxButtons.OKCancel);
        }

        private void basicButton7_Click(object sender, EventArgs e)
        {
            MsgBox.Show(this, "测试", "弹出对话框测试6.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(imageList1.Images[0]+"");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void basicCheckBox1_MouseClick(object sender, MouseEventArgs e)
        {

            
        }

        private void basicCheckBox1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
