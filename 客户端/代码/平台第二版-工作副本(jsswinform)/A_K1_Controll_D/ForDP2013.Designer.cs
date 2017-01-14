namespace 客户端主程序.SubForm
{
    partial class ForDP2013
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
            this.Lshuzhi = new System.Windows.Forms.Label();
            this.Lbiaoti = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Lshuzhi
            // 
            this.Lshuzhi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lshuzhi.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lshuzhi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(238)))), ((int)(((byte)(19)))));
            this.Lshuzhi.Location = new System.Drawing.Point(62, 0);
            this.Lshuzhi.Name = "Lshuzhi";
            this.Lshuzhi.Size = new System.Drawing.Size(239, 14);
            this.Lshuzhi.TabIndex = 68;
            this.Lshuzhi.Text = "xxxxx";
            this.Lshuzhi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbiaoti
            // 
            this.Lbiaoti.AutoSize = true;
            this.Lbiaoti.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbiaoti.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lbiaoti.ForeColor = System.Drawing.Color.White;
            this.Lbiaoti.Location = new System.Drawing.Point(0, 0);
            this.Lbiaoti.Name = "Lbiaoti";
            this.Lbiaoti.Size = new System.Drawing.Size(62, 14);
            this.Lbiaoti.TabIndex = 67;
            this.Lbiaoti.Text = "xxxxxx：";
            this.Lbiaoti.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ForDP2013
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.Lshuzhi);
            this.Controls.Add(this.Lbiaoti);
            this.Name = "ForDP2013";
            this.Size = new System.Drawing.Size(301, 14);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Lshuzhi;
        private System.Windows.Forms.Label Lbiaoti;
    }
}
