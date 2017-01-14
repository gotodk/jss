using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace 客户端主程序.SubForm
{
    public partial class ForDP2013 : UserControl
    {
        string biaoti = "******";
        /// <summary>
        /// 标题
        /// </summary>
        public string SP_BiaoTi
        {
            get { return biaoti; }
            set
            {
                biaoti = value;
                Lbiaoti.Text = value;
            }
        }

        string shuzhi = "******";
        /// <summary>
        /// 数值
        /// </summary>
        public string SP_ShuZhi
        {
            get { return shuzhi; }
            set
            {
                shuzhi = value;
                Lshuzhi.Text = value;
            }
        }

        public ForDP2013()
        {
            InitializeComponent();
        }
    }
}
