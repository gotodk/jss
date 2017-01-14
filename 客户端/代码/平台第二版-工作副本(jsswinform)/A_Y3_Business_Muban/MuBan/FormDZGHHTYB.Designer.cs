namespace 客户端主程序.SubForm.NewCenterForm.GZZD
{
    partial class FormDZGHHTYB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelUCedit = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.panel1 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panelUCedit.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelUCedit
            // 
            this.panelUCedit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUCedit.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUCedit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUCedit.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUCedit.Controls.Add(this.panel1);
            this.panelUCedit.Location = new System.Drawing.Point(2, 30);
            this.panelUCedit.Margin = new System.Windows.Forms.Padding(0);
            this.panelUCedit.Name = "panelUCedit";
            this.panelUCedit.SetShowBorder = 1;
            this.panelUCedit.Size = new System.Drawing.Size(776, 568);
            this.panelUCedit.TabIndex = 29;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.webBrowser1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 568);
            this.panel1.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(776, 568);
            this.webBrowser1.TabIndex = 76;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // XSRM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(780, 600);
            this.Controls.Add(this.panelUCedit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XSRM";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "下达提货单";
            this.Load += new System.EventHandler(this.CXweizhongbiaoEdit_Load);
            this.Controls.SetChildIndex(this.panelUCedit, 0);
            this.panelUCedit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private 客户端主程序.SubForm.CenterForm.publicUC.PanelUC panelUCedit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}