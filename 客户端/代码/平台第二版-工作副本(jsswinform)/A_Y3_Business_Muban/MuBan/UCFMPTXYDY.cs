using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Imaging;

namespace 客户端主程序.SubForm.NewCenterForm.MuBan
{
    public partial class UCFMPTXYDY : UserControl
    {
        Hashtable HTcs;
        public UCFMPTXYDY()
        {
            InitializeComponent();
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            //获取参数
            HTcs = (Hashtable)(this.Tag);
            Lpager.Text = "[第" + HTcs["纯数字索引"].ToString() + "页]";

            //这是一种用法
            //webBrowser1.Url = new Uri("http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/moban/cs1.aspx?moban=ceshi.htm&pagenumber=1&htbh=" + HTcs["要传递的参数1"].ToString() + "");
            //这是另一种用法
            webBrowser1.Url = new Uri(HTcs["数据地址"].ToString());
      
        }


        //调用远程模板，并实时分页截图。
        private void BeginShow()
        {
            //设置隐藏浏览器的高度和宽度（必须）
            this.webBrowser1.Width = pictureBox1.Width;
            this.webBrowser1.Height = pictureBox1.Height;
            //获取可滚动区域（必须）
            Rectangle rectBody = this.webBrowser1.Document.Body.ScrollRectangle;
            int width = rectBody.Width;
            int height = rectBody.Height;
            //重新设置浏览器高度和宽度（必须）
            this.webBrowser1.Width = width;
            this.webBrowser1.Height = height;
            try
            {
                //截图区域分析
                int this_y = pictureBox1.Height * (Convert.ToInt32(HTcs["纯数字索引"]) - 1);
                int this_ph = pictureBox1.Height;
                //若截图下限超过了原图高度
                if (this_y + pictureBox1.Height > height)
                {
                    this_ph = height - this_y;
                }
                //若截图上限超过了原图高度
                if (this_y > height)
                {
                    //再执行会出错
                    return;
                }
                //开始截图
                SnapLibrary.Snapshot snap = new SnapLibrary.Snapshot();
                Bitmap bmp = snap.TakeSnapshot(this.webBrowser1.ActiveXInstance, new Rectangle(0, 0, width, height));
                Bitmap bmpthis = bmp.Clone(new RectangleF(0, this_y, width, this_ph), PixelFormat.Format24bppRgb);
                pictureBox1.Image = bmpthis;
              
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            BeginShow();
        }
    }
}
