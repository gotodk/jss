using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using O2S.Components.PDFView4NET;
using System.Net;
using System.IO;
using System.Drawing.Drawing2D;
using O2S.Components.PDFRender4NET;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class PdfPrivewControl : UserControl
    {
        public PdfPrivewControl()
        {
            InitializeComponent();
            pdfCenterView.MouseWheel += new MouseEventHandler(pdfCenterView_MouseWheel);
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            sizeInit();
            pageInit();
        }

        #region 属性




        Stream stream;

        /// <summary>
        /// 文件名 
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 缩放类型 
        /// </summary>
        enum ZoomType
        {
            /// <summary>
            /// 无类型 
            /// </summary>
            None = 0,
            /// <summary>
            /// 实际大小
            /// </summary>
            Real = 1,
            /// <summary>
            /// 合适宽
            /// </summary>
            FixWidth = 2,
            /// <summary>
            /// 合适大小
            /// </summary>
            AllFix = 3,
        }
        /// <summary>
        /// 原始文件大小
        /// </summary>
        Size osize = new Size();
        /// <summary>
        /// 允许个位的位数
        /// </summary>
        int digit = 3;
        /// <summary>
        /// 允许小数的位数
        /// </summary>
        int pdigit = 2;
        #endregion

        #region 基本方法
        /// <summary>
        /// 计算比例
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        float getPecent(Size size)
        {
            return (float)size.Width / (float)size.Height;
        }
        /// <summary>
        /// 相除并乘100
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        float getDivision(int num1, int num2)
        {
            float temp = (float)num1 / (float)num2;
            temp = temp * 100;
            return temp;
        }
        /// <summary>
        /// 设置缩放图标
        /// </summary>
        /// <param name="type"></param>
        void setZoomIco(ZoomType type)
        {
            tsBtnRealSize.Checked = type == ZoomType.Real;
        }
        /// <summary>
        /// 获取当前百分比
        /// </summary>
        float getNowPercent()
        {
            float result = 100;
            try
            {
                result = float.Parse(tsCboZoom.Text.Remove(tsCboZoom.Text.Length - 1));

            }
            catch { }
            return result;
        }
        /// <summary>
        /// 是否有文件 
        /// </summary>
        /// <returns></returns>
        bool isOnFile()
        {
            bool result = pdfDoc.PageCount > 0;
            tsBtnFirstPage.Enabled = result;
            tsBtnPPage.Enabled = result;
            tsCboPages.Enabled = result;
            tsBtnNPage.Enabled = result;
            tsBtnEndPage.Enabled = result;
            tsBtnZoomIn.Enabled = result;
            tsBtnZoomOut.Enabled = result;
            tsBtnRealSize.Enabled = result;
            tsCboZoom.Enabled = result;
            return result;
        }
        #endregion

        #region 图片百分比
        /// <summary>
        /// 百分比初始化
        /// </summary>
        void sizeInit()
        {
            float temp = pdfCenterView.Zoom;
            pdfCenterView.Zoom = 100;
            osize = pdfCenterView.AutoScrollMinSize;
        }

        #region 大小输入
        private void tsBtnRealSize_Click(object sender, EventArgs e)
        {
            setZoomIco(ZoomType.None);
            setZoom(100);
            setZoomIco(ZoomType.Real);
        }

        private void tsBtnSetWidth_Click(object sender, EventArgs e)
        {
            setZoomIco(ZoomType.None);
            setFixWidth();
            setZoomIco(ZoomType.FixWidth);
        }

        private void tsBtnSetFix_Click(object sender, EventArgs e)
        {
            setZoomIco(ZoomType.None);
            setAllFix();
            setZoomIco(ZoomType.AllFix);
        }

        private void tsCboZoom_TextChanged(object sender, EventArgs e)
        {
            if (tsCboZoom.Text == "%" ||
                tsCboZoom.Text == string.Empty)
            {
                tsCboZoom.Text = "0";
                return;
            }
            if (!tsCboZoom.Text.EndsWith("%"))
            {
                int old = tsCboZoom.SelectionStart;
                if (tsCboZoom.Text.Contains("%"))
                {
                    tsCboZoom.Text = tsCboZoom.Text.Replace("%", string.Empty);
                    return;
                }
                tsCboZoom.Text += "%";
                tsCboZoom.SelectionStart = old;
            }
            //小数点处理 
            if (tsCboZoom.Text.Contains("."))
            {
                if (tsCboZoom.Text.StartsWith("."))
                {
                    tsCboZoom.Text = "0" + tsCboZoom.Text;
                }
                int start = tsCboZoom.Text.IndexOf(".");
                if (tsCboZoom.Text.StartsWith("0") &&
                    start > 1)
                {
                    tsCboZoom.Text = tsCboZoom.Text.Remove(0, 1);
                }
            }
            else//无小数点处理 
            {
                if (tsCboZoom.Text.StartsWith("00"))
                {
                    tsCboZoom.Text = tsCboZoom.Text.Remove(0, 1);
                }
                if (tsCboZoom.Text.StartsWith("0") &&
                    tsCboZoom.Text.Length > digit)
                {
                    tsCboZoom.Text = tsCboZoom.Text.Remove(0, 1);
                }
            }
        }

        private void tsCboZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((Keys)e.KeyChar)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    can(e);
                    break;
                case Keys.Delete:
                    if (tsCboZoom.Text.Contains(".") ||//限制只能输入一个小数点 
                        //只能保留位数
                        tsCboZoom.SelectionStart < tsCboZoom.Text.Length - pdigit - 1)
                    {
                        e.Handled = true;
                    }
                    break;
                case Keys.Back:
                    break;
                case Keys.Enter:
                    tsCboZoom_Leave(sender, EventArgs.Empty);
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        private void tsBtnZoomIn_Click(object sender, EventArgs e)
        {
            float percent = getNowPercent();
            if (percent >= 600) return;
            foreach (string one in tsCboZoom.Items)
            {
                float temp = float.Parse(one.Remove(one.Length - 1));
                if (temp > percent)
                {
                    setZoomIco(ZoomType.None);
                    setZoom(temp);
                    break;
                }
            }
        }

        private void tsBtnZoomOut_Click(object sender, EventArgs e)
        {
            float percent = getNowPercent();
            if (percent <= (float)10.00) return;
            for (int i = tsCboZoom.Items.Count - 1; i >= 0; i--)
            {
                string one = tsCboZoom.Items[i].ToString();
                float temp = float.Parse(one.Remove(one.Length - 1));
                if (temp < percent)
                {
                    setZoomIco(ZoomType.None);
                    setZoom(temp);
                    break;
                }
            }
        }


        private void tsCboZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsCboZoom_Leave(sender, EventArgs.Empty);
        }
        #endregion

        #region 大小控制
        //确定 
        private void tsCboZoom_Leave(object sender, EventArgs e)
        {
            float temp;
            try
            {
                temp = float.Parse(
                    tsCboZoom.Text.Remove(tsCboZoom.Text.Length - 1));
            }
            catch
            {
                temp = 100;
            }
            setZoom(temp);
            setZoomIco(ZoomType.None);
        }
        /// <summary>
        /// 取消键值 
        /// </summary>
        /// <param name="e"></param>
        void can(KeyPressEventArgs e)
        {
            //限制小数点后只能输入两位  
            if (tsCboZoom.Text.Contains("."))
            {
                int start = tsCboZoom.Text.IndexOf(".") + 1;
                int now = tsCboZoom.SelectionStart;
                int end = tsCboZoom.Text.IndexOf("%") + 1;
                //简单插入 
                if (tsCboZoom.SelectionLength < 1)
                {
                    if (end - start > pdigit &&
                        start <= now && now < end)
                    {
                        e.Handled = true;
                    }
                }
                //多个替换 
                else
                {
                    if (now < start &&
                        now + tsCboZoom.SelectionLength >= start &&
                        tsCboZoom.Text.Length - tsCboZoom.SelectionLength > digit)
                    {
                        e.Handled = true;
                    }
                    else return;
                }
                //限制个位 
                if (start > digit && now < digit + 1)
                {
                    e.Handled = true;
                }
            }
            if (//无小数 
                !tsCboZoom.Text.Contains(".") &&
                ((tsCboZoom.Text.Length > digit &&
                !(tsCboZoom.SelectionLength > 0)
                ) ||
                tsCboZoom.SelectionStart == tsCboZoom.Text.Length)//禁止最后位置输入 
                )
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// 设置百分比 
        /// </summary>
        void setZoom(float percent)
        {
            double temp = Math.Max(10.00, Math.Min(600, percent));
            temp = Math.Round(temp, 2);
            pdfCenterView.Zoom = (float)temp;
            tsCboZoom.Text = pdfCenterView.Zoom.ToString("0.00") + "%";
        }
        /// <summary>
        /// 缩放到合适宽
        /// </summary>
        void setFixWidth()
        {
            float filep = getPecent(osize);
            float contp = getPecent(pdfCenterView.Size);
            if (contp > filep)
            {
                setZoom(
                    getDivision(pdfCenterView.Size.Width - 17, osize.Width));
            }
            else
            {
                setZoom(
                    getDivision(pdfCenterView.Size.Width, osize.Width));
            }
        }
        /// <summary>
        /// 缩放到合适宽
        /// </summary>
        void setAllFix()
        {
            float filep = getPecent(osize);
            float contp = getPecent(pdfCenterView.Size);
            if (contp > filep)
            {
                setZoom(
                    getDivision(pdfCenterView.Size.Height, osize.Height));
            }
            else
            {
                setZoom(
                    getDivision(pdfCenterView.Size.Width, osize.Width));
            }
        }
        #endregion
        #endregion

        #region 翻页
        /// <summary>
        /// 页数初始化 
        /// </summary>
        void pageInit()
        {
            tsCboPages.Items.Clear();
            if (!isOnFile()) return;
            for (int i = 0; i < pdfDoc.PageCount; i++)
            {
                tsCboPages.Items.Add((i + 1).ToString());
            }
            tsCboPages.SelectedIndex = 0;
            tsLblPages2.Text = "/" + pdfDoc.PageCount;
        }
        #region 页数输入
        private void tsBtnFirstPage_Click(object sender, EventArgs e)
        {
            tsCboPages.SelectedIndex = 0;
        }
        private void tsBtnPPage_Click(object sender, EventArgs e)
        {
            int temp = getNowPage();
            if (temp > 0)
            {
                tsCboPages.SelectedIndex = temp - 1;
            }
        }

        private void tsBtnNPage_Click(object sender, EventArgs e)
        {
            int temp = getNowPage();
            if (temp < pdfDoc.PageCount)
            {
                tsCboPages.SelectedIndex = temp + 1;
            }
        }

        private void tsBtnEndPage_Click(object sender, EventArgs e)
        {
            tsCboPages.SelectedIndex = pdfDoc.PageCount - 1;
        }

        private void tsCboPages_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((Keys)e.KeyChar)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.Back:
                    break;
                case Keys.Enter:
                    tsCboPages_Leave(sender, EventArgs.Empty);
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        private void tsCboPages_Leave(object sender, EventArgs e)
        {
            tsCboPages.SelectedIndex = getNowPage();
        }
        #endregion

        #region 滚轮
        int up2 = 0;
        int down2 = 0;
        private void pdfCenterView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                down2 = 0;
                if ((!pdfCenterView.VerticalScroll.Visible ||
                    pdfCenterView.VerticalScroll.Value == 0))
                {
                    if (getNowPage() != 0 && up2 == 2)
                    {
                        up2 = 0;
                        pdfCenterView.VerticalScroll.Value = pdfCenterView.VerticalScroll.Maximum;
                        tsBtnPPage_Click(sender, EventArgs.Empty);
                    }
                    else
                    {
                        up2++;
                    }
                }
            }
            else
            {
                up2 = 0;
                if ((!pdfCenterView.VerticalScroll.Visible ||
                    pdfCenterView.VerticalScroll.Value + pdfCenterView.VerticalScroll.LargeChange >=
                    pdfCenterView.VerticalScroll.Maximum))
                {
                    if (getNowPage() != pdfDoc.PageCount - 1 && down2 == 2)
                    {
                        down2 = 0;
                        pdfCenterView.VerticalScroll.Value = 0;
                        tsBtnNPage_Click(sender, EventArgs.Empty);
                    }
                    else
                    {
                        down2++;
                    }
                }
            }
        }
        #endregion

        #region 页数控制

        private void tsCboPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int temp = getNowPage();
            tsCboPages.SelectedIndex = temp;
            pdfCenterView.PageNumber = temp;

            bool result = temp == 0;
            tsBtnFirstPage.Enabled = !result;
            tsBtnPPage.Enabled = !result;
            result = temp == pdfDoc.PageCount - 1;
            tsBtnNPage.Enabled = !result;
            tsBtnEndPage.Enabled = !result;
        }
        /// <summary>
        /// 获取当前所写的page
        /// </summary>
        /// <returns></returns>
        int getNowPage()
        {
            int result = 1;
            try
            {
                result = int.Parse(tsCboPages.Text);
            }
            catch { }
            result = Math.Max(0, Math.Min(pdfDoc.PageCount, result));
            return result <= 0 ? 0 : result - 1;
        }
        #endregion

        #endregion

        #region 系统


        /// <summary>
        /// 从url加载pdf,必须是一个有效的pdf格式,要带http://
        /// </summary>
        /// <param name="url"></param>
        public void loadfromurl(string url)
        {
            WebClient webClient = new WebClient();
            //byte[] b = webClient.DownloadData("http://192.168.0.26:777/pingtaiservices/moban/getpdfHT.aspx?Number=N130801000294&g=" + Guid.NewGuid().ToString());
            webClient.DownloadFile(url, System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\HT.pdf");
            pdfDoc.Load(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\HT.pdf");
            pageInit();
        }

 

        public void savepdf()
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


        #endregion

        #region 旋转
        private void tsBtnRotate90CClockwise_Click(object sender, EventArgs e)
        {
            int nowpage = getNowPage();
            int pageRotation = (int)pdfDoc.Pages[nowpage].Rotation;
            pageRotation = pageRotation - 90;
            if (pageRotation < 0)
            {
                pageRotation = 270;
            }
            pdfDoc.Pages[nowpage].Rotation = (PDFPageRotation)pageRotation;
        }

        private void tsBtnRotate90Clockwise_Click(object sender, EventArgs e)
        {
            int nowpage = getNowPage();
            int pageRotation = (int)pdfDoc.Pages[nowpage].Rotation;
            pageRotation = pageRotation + 90;
            if (pageRotation >= 360)
            {
                pageRotation = 0;
            }
            pdfDoc.Pages[nowpage].Rotation = (PDFPageRotation)pageRotation;
        }
        #endregion


        /// <summary>
        /// The PDF file being previewed.
        /// </summary>
        private PDFFile pdfFile;
        /// <summary>
        /// The page being printed.
        /// </summary>
        private int currentPage;

        /// <summary>
        /// True if the page is displayed in print preview, false if the page is actually printed.
        /// </summary>
        private bool isPrintPreview;


        public void yulan()
        {
            // Close the previous file.
            if (pdfFile != null)
            {
                pdfFile.Dispose();
            }

            // Load the new file.
            pdfFile = PDFFile.Open(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\HT.pdf");

            isPrintPreview = true;
            printPreviewDialog.ShowDialog();
        }

        private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentPage = 0;
        }

        private void printDocument_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (1 == 1)
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
