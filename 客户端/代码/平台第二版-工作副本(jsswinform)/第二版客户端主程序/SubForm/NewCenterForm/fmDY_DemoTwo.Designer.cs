﻿namespace 客户端主程序.SubForm.NewCenterForm
{
    partial class fmDY_DemoTwo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmDY));
            this.panel1 = new System.Windows.Forms.Panel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDialog2 = new System.Windows.Forms.PrintDialog();
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.panelDYmain = new System.Windows.Forms.FlowLayoutPanel();
            this.basicButton1 = new Com.Seezt.Skins.BasicButton();
            this.BSave = new Com.Seezt.Skins.BasicButton();
            this.panel1.SuspendLayout();
            this.panelUC5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panelUC5);
            this.panel1.Location = new System.Drawing.Point(2, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 568);
            this.panel1.TabIndex = 29;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.AllowCurrentPage = true;
            this.printDialog1.AllowSelection = true;
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.UseEXDialog = true;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDialog2
            // 
            this.printDialog2.UseEXDialog = true;
            // 
            // panelUC5
            // 
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.panelDYmain);
            this.panelUC5.Controls.Add(this.basicButton1);
            this.panelUC5.Controls.Add(this.BSave);
            this.panelUC5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUC5.Location = new System.Drawing.Point(0, 0);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(776, 568);
            this.panelUC5.TabIndex = 16;
            // 
            // panelDYmain
            // 
            this.panelDYmain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDYmain.AutoScroll = true;
            this.panelDYmain.Location = new System.Drawing.Point(4, 33);
            this.panelDYmain.Name = "panelDYmain";
            this.panelDYmain.Size = new System.Drawing.Size(769, 532);
            this.panelDYmain.TabIndex = 2;
            // 
            // basicButton1
            // 
            this.basicButton1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton1.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton1.Location = new System.Drawing.Point(3, 4);
            this.basicButton1.Name = "basicButton1";
            this.basicButton1.Size = new System.Drawing.Size(100, 22);
            this.basicButton1.TabIndex = 0;
            this.basicButton1.Texts = "预览并打印";
            this.basicButton1.Click += new System.EventHandler(this.basicButton1_Click);
            // 
            // BSave
            // 
            this.BSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.BSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BSave.ForeColor = System.Drawing.Color.DarkBlue;
            this.BSave.Location = new System.Drawing.Point(114, 4);
            this.BSave.Name = "BSave";
            this.BSave.Size = new System.Drawing.Size(100, 22);
            this.BSave.TabIndex = 1;
            this.BSave.Texts = "直接打印";
            this.BSave.Click += new System.EventHandler(this.BSave_Click);
            // 
            // fmDY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 600);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1440, 900);
            this.MinimizeBox = false;
            this.Name = "fmDY";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "打印";
            this.Load += new System.EventHandler(this.fmDY_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panelUC5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CenterForm.publicUC.PanelUC panelUC5;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private Com.Seezt.Skins.BasicButton BSave;
        private System.Windows.Forms.PrintDialog printDialog1;
        private Com.Seezt.Skins.BasicButton basicButton1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.PrintDialog printDialog2;
        private System.Windows.Forms.FlowLayoutPanel panelDYmain;
    }
}