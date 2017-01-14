using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class UCdayindemoLiTwoTwoTwo : UserControl
    {
        public UCdayindemoLiTwoTwoTwo()
        {
          
            InitializeComponent();
            Go();
        }

        private void Go()
        {
            try
            {
                this.webBrowser.Navigate("http://www.baidu.com");
             
            }
            catch { }
        }




        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            //获取参数
            object CS = this.Tag;
            //label1.Text = CS.ToString();

        }
      
        /// <summary>
        /// 设置图片链接
        /// </summary>
        public string PB_Link
        {
            get { return this.pB.ImageLocation; }
            set { this.pB.ImageLocation = value; }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //int browserWidth = webBrowser.Width;
            //int browserHeight = webBrowser.Height;
            //Rectangle rectBody = this.webBrowser.Document.Body.ScrollRectangle;
            //int width = rectBody.Width + 20;
            //int height = rectBody.Height;
            //this.webBrowser.Width = width;
            //this.webBrowser.Height = height;
            //try
            //{
            //    SnapLibrary.Snapshot snap = new SnapLibrary.Snapshot();
            //    using (Bitmap bmp = snap.TakeSnapshot(this.webBrowser.ActiveXInstance, new Rectangle(0, 0, width, height)))
            //    {
            //        //this.webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
            //        //  bmp.Save(path, format);
            //        //using (Image img = ImageHelper.GetThumbnailImage(bmp, bmp.Width, bmp.Height))
            //        //{
            //        //    ImageHelper.SaveImage(img, path, format);
            //        //}
            //        pB.Image = bmp;
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //this.webBrowser.Width = browserWidth;
            //this.webBrowser.Height = browserHeight;
        }
      


    }
}
