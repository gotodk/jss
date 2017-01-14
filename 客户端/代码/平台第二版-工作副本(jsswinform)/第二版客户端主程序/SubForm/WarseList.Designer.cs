namespace 客户端主程序.SubForm
{
    partial class WarseList
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
            this.cbSecondSPFL = new System.Windows.Forms.ComboBox();
            this.cbStartSPFL = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbSecondSPFL
            // 
            this.cbSecondSPFL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSecondSPFL.FormattingEnabled = true;
            this.cbSecondSPFL.Location = new System.Drawing.Point(130, 3);
            this.cbSecondSPFL.Name = "cbSecondSPFL";
            this.cbSecondSPFL.Size = new System.Drawing.Size(121, 20);
            this.cbSecondSPFL.TabIndex = 69;
            this.cbSecondSPFL.SelectedIndexChanged += new System.EventHandler(this.cbSecondSPFL_SelectedIndexChanged);
            // 
            // cbStartSPFL
            // 
            this.cbStartSPFL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStartSPFL.FormattingEnabled = true;
            this.cbStartSPFL.Location = new System.Drawing.Point(3, 3);
            this.cbStartSPFL.MaxDropDownItems = 10;
            this.cbStartSPFL.Name = "cbStartSPFL";
            this.cbStartSPFL.Size = new System.Drawing.Size(121, 20);
            this.cbStartSPFL.TabIndex = 68;
            this.cbStartSPFL.SelectedIndexChanged += new System.EventHandler(this.cbStartSPFL_SelectedIndexChanged);
            // 
            // WarseList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbSecondSPFL);
            this.Controls.Add(this.cbStartSPFL);
            this.Name = "WarseList";
            this.Size = new System.Drawing.Size(259, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSecondSPFL;
        private System.Windows.Forms.ComboBox cbStartSPFL;

    }
}
