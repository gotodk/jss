using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class UCdayindemoLi : UserControl
    {
        public UCdayindemoLi()
        {
            InitializeComponent();
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            //获取参数
            object CS = this.Tag;
            //label1.Text = CS.ToString();
            this.pB.ImageLocation = CS.ToString();
        }
    }
}
