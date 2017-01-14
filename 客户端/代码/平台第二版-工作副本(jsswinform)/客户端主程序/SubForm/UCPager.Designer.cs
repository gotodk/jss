namespace 客户端主程序.SubForm
{
    partial class UCPager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCPager));
            this.wy = new System.Windows.Forms.Label();
            this.sy = new System.Windows.Forms.Label();
            this.xyy = new System.Windows.Forms.Label();
            this.syy = new System.Windows.Forms.Label();
            this.PBloading = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LGO = new System.Windows.Forms.Label();
            this.btbgo = new Com.Seezt.Skins.BasicTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PBloading)).BeginInit();
            this.SuspendLayout();
            // 
            // wy
            // 
            this.wy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.wy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.wy.ForeColor = System.Drawing.Color.Black;
            this.wy.Location = new System.Drawing.Point(155, 4);
            this.wy.Name = "wy";
            this.wy.Size = new System.Drawing.Size(29, 23);
            this.wy.TabIndex = 11;
            this.wy.Text = "尾页";
            this.wy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.wy.Click += new System.EventHandler(this.wy_Click);
            // 
            // sy
            // 
            this.sy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.sy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sy.ForeColor = System.Drawing.Color.Black;
            this.sy.Location = new System.Drawing.Point(22, 4);
            this.sy.Name = "sy";
            this.sy.Size = new System.Drawing.Size(29, 23);
            this.sy.TabIndex = 10;
            this.sy.Text = "首页";
            this.sy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sy.Click += new System.EventHandler(this.sy_Click);
            // 
            // xyy
            // 
            this.xyy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.xyy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.xyy.ForeColor = System.Drawing.Color.Black;
            this.xyy.Location = new System.Drawing.Point(102, 4);
            this.xyy.Name = "xyy";
            this.xyy.Size = new System.Drawing.Size(48, 23);
            this.xyy.TabIndex = 9;
            this.xyy.Text = "下一页";
            this.xyy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.xyy.Click += new System.EventHandler(this.xyy_Click);
            // 
            // syy
            // 
            this.syy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.syy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.syy.ForeColor = System.Drawing.Color.Black;
            this.syy.Location = new System.Drawing.Point(54, 4);
            this.syy.Name = "syy";
            this.syy.Size = new System.Drawing.Size(44, 23);
            this.syy.TabIndex = 8;
            this.syy.Text = "上一页";
            this.syy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.syy.Click += new System.EventHandler(this.syy_Click);
            // 
            // PBloading
            // 
            this.PBloading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PBloading.Image = ((System.Drawing.Image)(resources.GetObject("PBloading.Image")));
            this.PBloading.Location = new System.Drawing.Point(3, 2);
            this.PBloading.Name = "PBloading";
            this.PBloading.Size = new System.Drawing.Size(16, 25);
            this.PBloading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PBloading.TabIndex = 12;
            this.PBloading.TabStop = false;
            this.PBloading.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(301, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "统计数据";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(194, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "转到";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LGO
            // 
            this.LGO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LGO.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LGO.ForeColor = System.Drawing.Color.Black;
            this.LGO.Location = new System.Drawing.Point(268, 3);
            this.LGO.Name = "LGO";
            this.LGO.Size = new System.Drawing.Size(23, 23);
            this.LGO.TabIndex = 16;
            this.LGO.Text = "GO";
            this.LGO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LGO.Click += new System.EventHandler(this.LGO_Click);
            // 
            // btbgo
            // 
            this.btbgo.BackColor = System.Drawing.Color.Transparent;
            this.btbgo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btbgo.IsPass = false;
            this.btbgo.Location = new System.Drawing.Point(227, 3);
            this.btbgo.Multi = false;
            this.btbgo.Name = "btbgo";
            this.btbgo.ReadOn = false;
            this.btbgo.SB = System.Windows.Forms.ScrollBars.None;
            this.btbgo.Size = new System.Drawing.Size(37, 22);
            this.btbgo.SPLocation = new System.Drawing.Point(4, 5);
            this.btbgo.TabIndex = 17;
            this.btbgo.Texts = "1";
            this.btbgo.Load += new System.EventHandler(this.basicTextBox1_Load);
            // 
            // UCPager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btbgo);
            this.Controls.Add(this.LGO);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PBloading);
            this.Controls.Add(this.wy);
            this.Controls.Add(this.sy);
            this.Controls.Add(this.xyy);
            this.Controls.Add(this.syy);
            this.Name = "UCPager";
            this.Size = new System.Drawing.Size(653, 30);
            this.Load += new System.EventHandler(this.UCPager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PBloading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label wy;
        private System.Windows.Forms.Label sy;
        private System.Windows.Forms.Label xyy;
        private System.Windows.Forms.Label syy;
        private System.Windows.Forms.PictureBox PBloading;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LGO;
        private Com.Seezt.Skins.BasicTextBox btbgo;
    }
}
