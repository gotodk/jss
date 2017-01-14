using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace 银行监控平安
{
    public partial class test : DevComponents.DotNetBar.Metro.MetroForm
    {
        public test()
        {
            InitializeComponent();
        }

        private void test_Load(object sender, EventArgs e)
        {
            string str = @"
            ";
            byte[] b = Encoding.GetEncoding("GB2312").GetBytes(str);

            MessageBox.Show(b.Length.ToString());
        }
    }
}