using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.SubForm.NewCenterForm.GZZD;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    public partial class ucJJRSYZQ_B : UserControl
    {
        public ucJJRSYZQ_B()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 发票
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LLlishi_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/moban/FPKJYQ.htm";
            XSRM xs = new XSRM("发票开具要求", htcs);
            xs.ShowDialog();
        }
        /// <summary>
        /// 所得税计算方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/moban/SDSJSFF.htm";
            XSRM xs = new XSRM("所得税计算方法", htcs);
            xs.ShowDialog();
        }
    }
}
