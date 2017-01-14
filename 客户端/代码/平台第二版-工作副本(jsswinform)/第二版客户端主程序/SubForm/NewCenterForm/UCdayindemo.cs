using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class UCdayindemo : UserControl
    {
        public UCdayindemo()
        {
            InitializeComponent();
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            //获取参数
            object CS = this.Tag;



            try
            {
                richTextBox1.LoadFile("songti.rtf");
            }
            catch (Exception ex) { }
        }
    }
}
