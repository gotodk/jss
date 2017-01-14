using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using 客户端主程序.SubForm.NewCenterForm;
using 客户端主程序.SubForm.NewCenterForm.MuBan;

namespace 客户端主程序
{
    public partial class HTMLshow : BasicForm
    {
        //要打印的控件
        string[] ModName;
        string Title;
        object CS;


        public HTMLshow(string title, object cs, string[] modname)
        {

            //this.TitleYS = new int[] { 0, 0, -50 };

            CS = cs;
            Title = title;

            ModName = modname;
            InitializeComponent();

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;
            this.Text = Title;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;

        }

        private void basicButton2_Click(object sender, EventArgs e)
        {
            if (da)
            {
                webBrowser1.ShowSaveAsDialog();
            }

        }

        private void basicButton1_Click(object sender, EventArgs e)
        {
            if (da)
            {
                webBrowser1.ShowPrintPreviewDialog();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            webBrowser1.Navigate(CS.ToString());
    
        }

        bool da = false;

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            da = true;

 

            //为页面增加跳过功能
            HtmlDocument htmlDoc = webBrowser1.Document;
            HtmlElementCollection tupian = htmlDoc.GetElementsByTagName("a");
            

            for(int i = 0 ; i < tupian.Count;i++)
            {
                if (tupian[i].GetAttribute("showtupian").Length > 1)
                {
                    tupian[i].Click += new HtmlElementEventHandler(sp_showtupian);
                }
                if (tupian[i].GetAttribute("showkjdy").Length > 1)
                {
                    tupian[i].Click += new HtmlElementEventHandler(sp_showkjdy);
                }
            }

            label1.Visible = false;
 
            

        }

        public void sp_showtupian(object sender, EventArgs e)
        {
            HtmlElement he = (HtmlElement)(sender);
            string tuurl = he.GetAttribute("showtupian");


            //打开图片浏览窗体
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + tuurl.Replace(@"\", "/").Replace(@"\", "/");
            ImageShow IS = null;

            Hashtable htT = new Hashtable();
            htT["类型"] = "网址";
            htT["地址"] = url;
            if (IS == null || IS.IsDisposed == true)
            {
                IS = new ImageShow(htT);
            }
            IS.Show();
            IS.Activate();


        }

        public void sp_showkjdy(object sender, EventArgs e)
        {
            HtmlElement he = (HtmlElement)(sender);
            string number = he.GetAttribute("showkjdy");


            //打开打印控件
            if(he.Id == "tbdxz")
            {
                FormDZGHHT dy = new FormDZGHHT("查看并打印投标单", number, new string[] { "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTTBD" });
                dy.Show();
            }
            if (he.Id == "yddxz")
            {
                FormDZGHHT dy = new FormDZGHHT("查看并打印预订单", number, new string[] { "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTYDD" });
                dy.Show();
            }
            if (he.Id == "linkBZH")
            {
                int pagenumber = 1;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber];
                string[] MKarr = new string[pagenumber];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/BZH.aspx?moban=BZH.htm&Number=" + number;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.PrintUC.UCbzh";
                }
                FormDZGHHT dy = new FormDZGHHT("保证函", CSarr, MKarr);
                dy.Show();
            }

            if (he.Id.IndexOf("hl_thdbh") >= 0)
            {
                FormDZGHHT dy = new FormDZGHHT("查看并打印提货单", number, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.THDDY" });
                dy.Show();
            }
            if (he.Id.IndexOf("hl_fhdbh") >= 0)
            {
                FormDZGHHT dy = new FormDZGHHT("查看并打印发货单", number, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.FHDDY" });
                dy.Show();
            }
        }
    }
}
