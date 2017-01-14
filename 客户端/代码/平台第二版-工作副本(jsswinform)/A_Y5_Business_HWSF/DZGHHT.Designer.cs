namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    partial class DZGHHT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DZGHHT));
            this.LLLNumber = new System.Windows.Forms.Label();
            this.lblHTBH = new System.Windows.Forms.Label();
            this.panelUCedit = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.PBload = new System.Windows.Forms.PictureBox();
            this.panelUCedit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.SuspendLayout();
            // 
            // LLLNumber
            // 
            this.LLLNumber.AutoSize = true;
            this.LLLNumber.ForeColor = System.Drawing.Color.Black;
            this.LLLNumber.Location = new System.Drawing.Point(114, 11);
            this.LLLNumber.Name = "LLLNumber";
            this.LLLNumber.Size = new System.Drawing.Size(77, 12);
            this.LLLNumber.TabIndex = 68;
            this.LLLNumber.Text = "隐藏定标编号";
            this.LLLNumber.Visible = false;
            // 
            // lblHTBH
            // 
            this.lblHTBH.AutoSize = true;
            this.lblHTBH.ForeColor = System.Drawing.Color.Black;
            this.lblHTBH.Location = new System.Drawing.Point(200, 11);
            this.lblHTBH.Name = "lblHTBH";
            this.lblHTBH.Size = new System.Drawing.Size(77, 12);
            this.lblHTBH.TabIndex = 69;
            this.lblHTBH.Text = "隐藏合同编号";
            this.lblHTBH.Visible = false;
            // 
            // panelUCedit
            // 
            this.panelUCedit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUCedit.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUCedit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUCedit.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUCedit.Controls.Add(this.PBload);
            this.panelUCedit.Location = new System.Drawing.Point(2, 30);
            this.panelUCedit.Margin = new System.Windows.Forms.Padding(0);
            this.panelUCedit.Name = "panelUCedit";
            this.panelUCedit.SetShowBorder = 1;
            this.panelUCedit.Size = new System.Drawing.Size(976, 598);
            this.panelUCedit.TabIndex = 29;
            // 
            // PBload
            // 
            this.PBload.Image = ((System.Drawing.Image)(resources.GetObject("PBload.Image")));
            this.PBload.Location = new System.Drawing.Point(207, 208);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 48;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // DZGHHT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 630);
            this.Controls.Add(this.lblHTBH);
            this.Controls.Add(this.panelUCedit);
            this.Controls.Add(this.LLLNumber);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DZGHHT";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "电子购货合同";
            this.Load += new System.EventHandler(this.CXweizhongbiaoEdit_Load);
            this.Controls.SetChildIndex(this.LLLNumber, 0);
            this.Controls.SetChildIndex(this.panelUCedit, 0);
            this.Controls.SetChildIndex(this.lblHTBH, 0);
            this.panelUCedit.ResumeLayout(false);
            this.panelUCedit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private 客户端主程序.SubForm.CenterForm.publicUC.PanelUC panelUCedit;
        public System.Windows.Forms.Label LLLNumber;
        public System.Windows.Forms.Label lblHTBH;
        private System.Windows.Forms.PictureBox PBload;
    }
}