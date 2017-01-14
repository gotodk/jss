namespace 客户端主程序.SubForm.NewCenterForm
{
    partial class PdfPrivewControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PdfPrivewControl));
            this.pdfCenterView = new O2S.Components.PDFView4NET.PDFPageView();
            this.pdfDoc = new O2S.Components.PDFView4NET.PDFDocument(this.components);
            this.tsBtnFirstPage = new System.Windows.Forms.ToolStripButton();
            this.tstrPage = new System.Windows.Forms.ToolStrip();
            this.tsBtnPPage = new System.Windows.Forms.ToolStripButton();
            this.tsLblPages = new System.Windows.Forms.ToolStripLabel();
            this.tsCboPages = new System.Windows.Forms.ToolStripComboBox();
            this.tsLblPages2 = new System.Windows.Forms.ToolStripLabel();
            this.tsBtnNPage = new System.Windows.Forms.ToolStripButton();
            this.tsBtnEndPage = new System.Windows.Forms.ToolStripButton();
            this.tsBtnRealSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsBtnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tstrSize = new System.Windows.Forms.ToolStrip();
            this.tsLblZoom = new System.Windows.Forms.ToolStripLabel();
            this.tsCboZoom = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.tstrPage.SuspendLayout();
            this.tstrSize.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pdfCenterView
            // 
            this.pdfCenterView.AutoScroll = true;
            this.pdfCenterView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pdfCenterView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfCenterView.Document = this.pdfDoc;
            this.pdfCenterView.Location = new System.Drawing.Point(0, 0);
            this.pdfCenterView.Name = "pdfCenterView";
            this.pdfCenterView.PageNumber = 0;
            this.pdfCenterView.Size = new System.Drawing.Size(675, 364);
            this.pdfCenterView.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.pdfCenterView.TabIndex = 2;
            this.pdfCenterView.WorkMode = O2S.Components.PDFView4NET.UserInteractiveWorkMode.PanAndScan;
            this.pdfCenterView.Zoom = 100F;
            // 
            // tsBtnFirstPage
            // 
            this.tsBtnFirstPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnFirstPage.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnFirstPage.Image")));
            this.tsBtnFirstPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnFirstPage.Name = "tsBtnFirstPage";
            this.tsBtnFirstPage.Size = new System.Drawing.Size(23, 22);
            this.tsBtnFirstPage.Text = "toolStripButton1";
            this.tsBtnFirstPage.Click += new System.EventHandler(this.tsBtnFirstPage_Click);
            // 
            // tstrPage
            // 
            this.tstrPage.Dock = System.Windows.Forms.DockStyle.None;
            this.tstrPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnFirstPage,
            this.tsBtnPPage,
            this.tsLblPages,
            this.tsCboPages,
            this.tsLblPages2,
            this.tsBtnNPage,
            this.tsBtnEndPage});
            this.tstrPage.Location = new System.Drawing.Point(218, 0);
            this.tstrPage.Name = "tstrPage";
            this.tstrPage.Size = new System.Drawing.Size(224, 25);
            this.tstrPage.TabIndex = 2;
            // 
            // tsBtnPPage
            // 
            this.tsBtnPPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnPPage.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnPPage.Image")));
            this.tsBtnPPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnPPage.Name = "tsBtnPPage";
            this.tsBtnPPage.Size = new System.Drawing.Size(23, 22);
            this.tsBtnPPage.Text = "toolStripButton2";
            this.tsBtnPPage.Click += new System.EventHandler(this.tsBtnPPage_Click);
            // 
            // tsLblPages
            // 
            this.tsLblPages.Name = "tsLblPages";
            this.tsLblPages.Size = new System.Drawing.Size(23, 22);
            this.tsLblPages.Text = "页:";
            // 
            // tsCboPages
            // 
            this.tsCboPages.Name = "tsCboPages";
            this.tsCboPages.Size = new System.Drawing.Size(75, 25);
            this.tsCboPages.SelectedIndexChanged += new System.EventHandler(this.tsCboPages_SelectedIndexChanged);
            this.tsCboPages.Leave += new System.EventHandler(this.tsCboPages_Leave);
            this.tsCboPages.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tsCboPages_KeyPress);
            // 
            // tsLblPages2
            // 
            this.tsLblPages2.Name = "tsLblPages2";
            this.tsLblPages2.Size = new System.Drawing.Size(20, 22);
            this.tsLblPages2.Text = "/1";
            // 
            // tsBtnNPage
            // 
            this.tsBtnNPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnNPage.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnNPage.Image")));
            this.tsBtnNPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnNPage.Name = "tsBtnNPage";
            this.tsBtnNPage.Size = new System.Drawing.Size(23, 22);
            this.tsBtnNPage.Text = "toolStripButton3";
            this.tsBtnNPage.Click += new System.EventHandler(this.tsBtnNPage_Click);
            // 
            // tsBtnEndPage
            // 
            this.tsBtnEndPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnEndPage.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnEndPage.Image")));
            this.tsBtnEndPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnEndPage.Name = "tsBtnEndPage";
            this.tsBtnEndPage.Size = new System.Drawing.Size(23, 22);
            this.tsBtnEndPage.Text = "toolStripButton4";
            this.tsBtnEndPage.Click += new System.EventHandler(this.tsBtnEndPage_Click);
            // 
            // tsBtnRealSize
            // 
            this.tsBtnRealSize.Checked = true;
            this.tsBtnRealSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsBtnRealSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnRealSize.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnRealSize.Image")));
            this.tsBtnRealSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnRealSize.Name = "tsBtnRealSize";
            this.tsBtnRealSize.Size = new System.Drawing.Size(23, 22);
            this.tsBtnRealSize.Text = "toolStripButton1";
            this.tsBtnRealSize.Click += new System.EventHandler(this.tsBtnRealSize_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnZoomOut
            // 
            this.tsBtnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnZoomOut.Image")));
            this.tsBtnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnZoomOut.Name = "tsBtnZoomOut";
            this.tsBtnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tsBtnZoomOut.Text = "toolStripButton5";
            this.tsBtnZoomOut.Click += new System.EventHandler(this.tsBtnZoomOut_Click);
            // 
            // tsBtnZoomIn
            // 
            this.tsBtnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnZoomIn.Image")));
            this.tsBtnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnZoomIn.Name = "tsBtnZoomIn";
            this.tsBtnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tsBtnZoomIn.Text = "toolStripButton6";
            this.tsBtnZoomIn.Click += new System.EventHandler(this.tsBtnZoomIn_Click);
            // 
            // tstrSize
            // 
            this.tstrSize.Dock = System.Windows.Forms.DockStyle.None;
            this.tstrSize.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnZoomIn,
            this.tsBtnZoomOut,
            this.toolStripSeparator1,
            this.tsBtnRealSize,
            this.tsLblZoom,
            this.tsCboZoom});
            this.tstrSize.Location = new System.Drawing.Point(4, 0);
            this.tstrSize.Name = "tstrSize";
            this.tstrSize.Size = new System.Drawing.Size(209, 25);
            this.tstrSize.TabIndex = 1;
            this.tstrSize.Text = "toolStrip1";
            // 
            // tsLblZoom
            // 
            this.tsLblZoom.Name = "tsLblZoom";
            this.tsLblZoom.Size = new System.Drawing.Size(45, 22);
            this.tsLblZoom.Text = "Zoom:";
            // 
            // tsCboZoom
            // 
            this.tsCboZoom.Items.AddRange(new object[] {
            "10.00%",
            "12.50%",
            "33.33%",
            "50.00%",
            "66.67%",
            "75.00%",
            "100.00%",
            "125.00%",
            "150.00%",
            "200.00%",
            "300.00%",
            "400.00%",
            "600.00%"});
            this.tsCboZoom.Name = "tsCboZoom";
            this.tsCboZoom.Size = new System.Drawing.Size(75, 25);
            this.tsCboZoom.Text = "100.00%";
            this.tsCboZoom.SelectedIndexChanged += new System.EventHandler(this.tsCboZoom_SelectedIndexChanged);
            this.tsCboZoom.Leave += new System.EventHandler(this.tsCboZoom_Leave);
            this.tsCboZoom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tsCboZoom_KeyPress);
            this.tsCboZoom.TextChanged += new System.EventHandler(this.tsCboZoom_TextChanged);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.pdfCenterView);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(675, 364);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(675, 389);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.tstrPage);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.tstrSize);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "PDF files(*.pdf)|*.pdf|All files(*.*)|*.*";
            // 
            // printDocument
            // 
            this.printDocument.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_BeginPrint);
            this.printDocument.EndPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_EndPrint);
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            this.printDocument.QueryPageSettings += new System.Drawing.Printing.QueryPageSettingsEventHandler(this.printDocument_QueryPageSettings);
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Document = this.printDocument;
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // PdfPrivewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "PdfPrivewControl";
            this.Size = new System.Drawing.Size(675, 389);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.tstrPage.ResumeLayout(false);
            this.tstrPage.PerformLayout();
            this.tstrSize.ResumeLayout(false);
            this.tstrSize.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private O2S.Components.PDFView4NET.PDFPageView pdfCenterView;
        private O2S.Components.PDFView4NET.PDFDocument pdfDoc;
        private System.Windows.Forms.ToolStripButton tsBtnFirstPage;
        private System.Windows.Forms.ToolStrip tstrPage;
        private System.Windows.Forms.ToolStripButton tsBtnPPage;
        private System.Windows.Forms.ToolStripLabel tsLblPages;
        private System.Windows.Forms.ToolStripComboBox tsCboPages;
        private System.Windows.Forms.ToolStripLabel tsLblPages2;
        private System.Windows.Forms.ToolStripButton tsBtnNPage;
        private System.Windows.Forms.ToolStripButton tsBtnEndPage;
        private System.Windows.Forms.ToolStripButton tsBtnRealSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsBtnZoomOut;
        private System.Windows.Forms.ToolStripButton tsBtnZoomIn;
        private System.Windows.Forms.ToolStrip tstrSize;
        private System.Windows.Forms.ToolStripLabel tsLblZoom;
        private System.Windows.Forms.ToolStripComboBox tsCboZoom;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
    }
}
