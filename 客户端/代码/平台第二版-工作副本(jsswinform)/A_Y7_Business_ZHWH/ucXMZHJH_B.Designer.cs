namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH
{
    partial class ucXMZHJH_B
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
            this.panelTJQ = new System.Windows.Forms.Panel();
            this.btnSave = new Com.Seezt.Skins.BasicButton();
            this.label1 = new System.Windows.Forms.Label();
            this.BSave = new Com.Seezt.Skins.BasicButton();
            this.Breset = new Com.Seezt.Skins.BasicButton();
            this.PBload = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelTJQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTJQ
            // 
            this.panelTJQ.Controls.Add(this.btnSave);
            this.panelTJQ.Controls.Add(this.label1);
            this.panelTJQ.Controls.Add(this.BSave);
            this.panelTJQ.Controls.Add(this.Breset);
            this.panelTJQ.Location = new System.Drawing.Point(10, 2);
            this.panelTJQ.Name = "panelTJQ";
            this.panelTJQ.Size = new System.Drawing.Size(566, 315);
            this.panelTJQ.TabIndex = 91;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSave.Location = new System.Drawing.Point(207, 99);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 22);
            this.btnSave.TabIndex = 52;
            this.btnSave.Texts = "确认激活";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 9F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(52, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 22);
            this.label1.TabIndex = 51;
            this.label1.Text = "须知：您的账户处于休眠状态，您向平台缴纳100元账户管理费后，账户即自动激活！";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BSave
            // 
            this.BSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.BSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BSave.ForeColor = System.Drawing.Color.DarkBlue;
            this.BSave.Location = new System.Drawing.Point(142, 556);
            this.BSave.Name = "BSave";
            this.BSave.Size = new System.Drawing.Size(60, 22);
            this.BSave.TabIndex = 49;
            this.BSave.Texts = "确认";
            // 
            // Breset
            // 
            this.Breset.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Breset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Breset.ForeColor = System.Drawing.Color.DarkBlue;
            this.Breset.Location = new System.Drawing.Point(297, 556);
            this.Breset.Name = "Breset";
            this.Breset.Size = new System.Drawing.Size(60, 22);
            this.Breset.TabIndex = 50;
            this.Breset.Texts = "重填";
            // 
            // PBload
            // 
            this.PBload.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBload.Location = new System.Drawing.Point(983, 20);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 93;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.pictureBox1.Location = new System.Drawing.Point(582, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 94;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // ucXMZHJH_B
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.PBload);
            this.Controls.Add(this.panelTJQ);
            this.Name = "ucXMZHJH_B";
            this.Size = new System.Drawing.Size(861, 580);
            this.Load += new System.EventHandler(this.ucSPMR_B_Load);
            this.panelTJQ.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTJQ;
        private Com.Seezt.Skins.BasicButton BSave;
        private Com.Seezt.Skins.BasicButton Breset;
        private System.Windows.Forms.PictureBox PBload;
        private System.Windows.Forms.Label label1;
        private Com.Seezt.Skins.BasicButton btnSave;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
