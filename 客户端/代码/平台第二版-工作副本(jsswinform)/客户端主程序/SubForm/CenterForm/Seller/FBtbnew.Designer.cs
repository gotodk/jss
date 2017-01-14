namespace 客户端主程序.SubForm.CenterForm.Seller
{
    partial class FBtbnew
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
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.cktbd = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelUC5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelUC5
            // 
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.cktbd);
            this.panelUC5.Controls.Add(this.label1);
            this.panelUC5.Location = new System.Drawing.Point(10, 10);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(790, 760);
            this.panelUC5.TabIndex = 16;
            this.panelUC5.Paint += new System.Windows.Forms.PaintEventHandler(this.panelUC5_Paint);
            // 
            // cktbd
            // 
            this.cktbd.BackColor = System.Drawing.Color.PowderBlue;
            this.cktbd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cktbd.ForeColor = System.Drawing.Color.Black;
            this.cktbd.Location = new System.Drawing.Point(111, 7);
            this.cktbd.Name = "cktbd";
            this.cktbd.Size = new System.Drawing.Size(102, 20);
            this.cktbd.TabIndex = 39;
            this.cktbd.Text = "查看投标单";
            this.cktbd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cktbd.Click += new System.EventHandler(this.cktbd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = "发布投标";
            // 
            // FBtbnew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUC5);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "FBtbnew";
            this.Size = new System.Drawing.Size(1065, 989);
            this.panelUC5.ResumeLayout(false);
            this.panelUC5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private publicUC.PanelUC panelUC5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label cktbd;
    
    }
}
