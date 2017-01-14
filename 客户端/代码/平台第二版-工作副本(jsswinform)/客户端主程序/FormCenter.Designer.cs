namespace 客户端主程序
{
    partial class FormCenter
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ZZ_tbMenuAdmin = new System.Windows.Forms.TreeView();
            this.PBloading = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBloading)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(2, 31);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.ZZ_tbMenuAdmin);
            this.splitContainer1.Panel1.Controls.Add(this.PBloading);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(725, 441);
            this.splitContainer1.SplitterDistance = 188;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 29;
            // 
            // ZZ_tbMenuAdmin
            // 
            this.ZZ_tbMenuAdmin.BackColor = System.Drawing.Color.AliceBlue;
            this.ZZ_tbMenuAdmin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ZZ_tbMenuAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZZ_tbMenuAdmin.ItemHeight = 20;
            this.ZZ_tbMenuAdmin.Location = new System.Drawing.Point(0, 0);
            this.ZZ_tbMenuAdmin.Name = "ZZ_tbMenuAdmin";
            this.ZZ_tbMenuAdmin.PathSeparator = ">";
            this.ZZ_tbMenuAdmin.ShowNodeToolTips = true;
            this.ZZ_tbMenuAdmin.Size = new System.Drawing.Size(186, 439);
            this.ZZ_tbMenuAdmin.TabIndex = 0;
            this.ZZ_tbMenuAdmin.Visible = false;
            this.ZZ_tbMenuAdmin.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ZZ_tbMenuAdmin_AfterSelect);
            // 
            // PBloading
            // 
            this.PBloading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PBloading.BackColor = System.Drawing.Color.AliceBlue;
            this.PBloading.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBloading.Location = new System.Drawing.Point(1, 1);
            this.PBloading.Name = "PBloading";
            this.PBloading.Size = new System.Drawing.Size(184, 435);
            this.PBloading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PBloading.TabIndex = 13;
            this.PBloading.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(2, 474);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 23);
            this.panel1.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前余额：9999元";
            // 
            // FormCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(729, 500);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximumSize = new System.Drawing.Size(0, 0);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "FormCenter";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "我的账户";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCenter_FormClosing);
            this.Load += new System.EventHandler(this.FormCenter_Load);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PBloading)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView ZZ_tbMenuAdmin;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox PBloading;
    }
}