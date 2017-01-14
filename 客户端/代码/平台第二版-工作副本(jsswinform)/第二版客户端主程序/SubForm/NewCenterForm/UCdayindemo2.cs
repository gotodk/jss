using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class UCdayindemo2 : UserControl
    {
        public UCdayindemo2()
        {
            InitializeComponent();
        }

        private void UCdayindemo2_Load(object sender, EventArgs e)
        {
            //获取参数
            object CS = this.Tag;


            try
            {
            richTextBox1.LoadFile("ceshi.rtf");
            }
            catch(Exception ex){}

            //Image initimg = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\loginbg.jpg");
           // pictureBox1.t

         
        }
    }
}
