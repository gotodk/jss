using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;

namespace 客户端主程序
{
    public partial class FormZCXY : BasicForm
    {
        string Strzclx = "";
        public FormZCXY( string zczhlx)
        {
            InitializeComponent();
            Strzclx = zczhlx;
        }

        private void FormZCXY_Load(object sender, EventArgs e)
        {

            //    "经纪人账户",
            //"卖家账户",
            //"买家账户"
            switch (Strzclx)
            {
                case "经纪人账户":
                    this.webBrowser1.Url = new System.Uri("http://fwpt.fm8844.com/JHJXPT/webservices/xieyi0301.mht");
                    break;
                case "卖家账户":
                    this.webBrowser1.Url = new System.Uri("http://fwpt.fm8844.com/JHJXPT/webservices/xieyi0301.mht");
                    break;
                case "买家账户":
                    this.webBrowser1.Url = new System.Uri("http://fwpt.fm8844.com/JHJXPT/webservices/xieyi0301.mht");
                    break;
                default:
                    break;
            }
         

        }
    }
}
