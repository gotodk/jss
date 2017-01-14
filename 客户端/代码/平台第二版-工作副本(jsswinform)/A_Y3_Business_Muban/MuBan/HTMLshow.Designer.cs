namespace 客户端主程序
{
    partial class HTMLshow
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
            this.components = new System.ComponentModel.Container();
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.basicButton2 = new Com.Seezt.Skins.BasicButton();
            this.basicButton1 = new Com.Seezt.Skins.BasicButton();
            this.label1 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelUC5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelUC5
            // 
            this.panelUC5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.basicButton2);
            this.panelUC5.Controls.Add(this.label1);
            this.panelUC5.Controls.Add(this.basicButton1);
            this.panelUC5.Controls.Add(this.webBrowser1);
            this.panelUC5.Location = new System.Drawing.Point(2, 31);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(829, 527);
            this.panelUC5.TabIndex = 29;
            // 
            // basicButton2
            // 
            this.basicButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.basicButton2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton2.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton2.Location = new System.Drawing.Point(706, 4);
            this.basicButton2.Name = "basicButton2";
            this.basicButton2.Size = new System.Drawing.Size(100, 22);
            this.basicButton2.TabIndex = 3;
            this.basicButton2.Texts = "下载";
            this.basicButton2.Click += new System.EventHandler(this.basicButton2_Click);
            // 
            // basicButton1
            // 
            this.basicButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.basicButton1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton1.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton1.Location = new System.Drawing.Point(597, 4);
            this.basicButton1.Name = "basicButton1";
            this.basicButton1.Size = new System.Drawing.Size(100, 22);
            this.basicButton1.TabIndex = 0;
            this.basicButton1.Texts = "预览并打印";
            this.basicButton1.Click += new System.EventHandler(this.basicButton1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(376, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "正在加载……";
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(20);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(829, 527);
            this.webBrowser1.TabIndex = 5;
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // HTMLshow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 561);
            this.Controls.Add(this.panelUC5);
            this.Name = "HTMLshow";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查看合同详情";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.Controls.SetChildIndex(this.panelUC5, 0);
            this.panelUC5.ResumeLayout(false);
            this.panelUC5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SubForm.CenterForm.publicUC.PanelUC panelUC5;
        private Com.Seezt.Skins.BasicButton basicButton2;
        private Com.Seezt.Skins.BasicButton basicButton1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}