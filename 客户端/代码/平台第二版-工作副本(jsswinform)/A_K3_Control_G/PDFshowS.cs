using Com.Seezt.Skins;
using O2S.Components.PDFRender4NET;
using O2S.Components.PDFView4NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class PDFshowS : BasicForm
    {
        private PDFFile pdfFile;
        string Title;
        string URL;
        private bool isPrintPreview;
        private int currentPage;

        string guid = "";
        public PDFshowS(string title,string url)
        {

            guid = Guid.NewGuid().ToString();

            Title = title;
            URL = url;

            this.TitleYS = new int[] { 0, 0, -50 };
 
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

        private void PDFshowS_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        int pagenumber = 0;
 

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;


            WebClient webClient = new WebClient();
            //byte[] b = webClient.DownloadData("http://192.168.0.26:777/pingtaiservices/moban/getpdfHT.aspx?Number=N130801000294&g=" + Guid.NewGuid().ToString());
            webClient.DownloadFile(URL, System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"HTTEMP\HT" + guid + ".pdf");
            pdfDoc.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"HTTEMP\HT" + guid + ".pdf");
            pdfFile = PDFFile.Open(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"HTTEMP\HT" + guid + ".pdf");
            pagenumber = pdfDoc.PageCount;

            for(int i = 0;i < pagenumber;i++)
            {
                PDFPageView pdfCenterView = new PDFPageView();
                pdfCenterView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
    | System.Windows.Forms.AnchorStyles.Right)));
                pdfCenterView.Cursor = System.Windows.Forms.Cursors.Hand;
                pdfCenterView.Document = pdfDoc;
                pdfCenterView.PageNumber = i;
                pdfCenterView.Location = new System.Drawing.Point(0, i*1150);
                pdfCenterView.Name = "pdfCenterView" + i;
                pdfCenterView.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                pdfCenterView.WorkMode = O2S.Components.PDFView4NET.UserInteractiveWorkMode.PanAndScan;
                pdfCenterView.Zoom = 100F;
                pdfCenterView.BorderStyle = BorderStyle.None;
                pdfCenterView.Size = new System.Drawing.Size(panelUC5.Size.Width, 1150);
                panelUC5.Controls.Add(pdfCenterView);
                if (i == 0)
                {
                    label1.Visible = false;
                    pdfCenterView.Focus();
                    pdfCenterView.Location = new Point(8, 0);
                }

                Thread.Sleep(5);

            }

 

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ;
        }

        private void basicButton2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                if (!path.EndsWith(".pdf"))
                {
                    path = path + ".pdf";
                }
                pdfDoc.Save(path);
            }
        }

        private void basicButton1_Click(object sender, EventArgs e)
        {
            if (pdfFile == null)
            {
                return;
            }
            isPrintPreview = true;
            printPreviewDialog.ShowDialog();
        }

        private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentPage = 0;
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
          
            // Get page size in points.
            SizeF pageSize = pdfFile.GetPageSize(currentPage);

            // Compute the aspect ratio of the page and the paper.
            double pageAspectRatio = pageSize.Width / pageSize.Height;
            float paperAspectRatio = e.PageBounds.Width / e.PageBounds.Height;

            // Convert the paper size to points.
            float paperWidth = 72 * e.PageBounds.Width / 100;
            float paperHeight = 72 * e.PageBounds.Height / 100;

            // Landscape page, portrait paper.
            if ((pageAspectRatio > 1) && (paperAspectRatio < 1))
            {
                // Translate to top right corner and rotate with 90 degrees clockwise.
                e.Graphics.TranslateTransform(paperWidth, 0, MatrixOrder.Prepend);
                e.Graphics.RotateTransform(90);
            }
            // Portrait page, landscape paper
            if ((pageAspectRatio < 1) && (paperAspectRatio > 1))
            {
                // Translate to bottom left corner and rotate with 90 degrees counterclockwise (-90)
                e.Graphics.TranslateTransform(0, paperHeight, MatrixOrder.Prepend);
                e.Graphics.RotateTransform(-90);
            }
           


            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pdfFile.RenderPage(currentPage, e.Graphics, 0, 0);
            currentPage++;

            e.HasMorePages = currentPage < pdfFile.PageCount;
            // Reset the print preview to false if all the pages have been displayed.
            if (!e.HasMorePages && isPrintPreview)
            {
                isPrintPreview = false;
            }
        }

        private void printDocument_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            // Set the page size only if the file is displayed in print preview.
            if (isPrintPreview)
            {
                // Get page size in points.
                SizeF pageSize = pdfFile.GetPageSize(currentPage);

                // Set the size of displayed page, convert the points to hundreds of inch.
                e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize();
                e.PageSettings.PaperSize.Width = (int)(100 * pageSize.Width / 72);
                e.PageSettings.PaperSize.Height = (int)(100 * pageSize.Height / 72);
            }
        }
    }
}
