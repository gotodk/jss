﻿namespace Com.Seezt.Skins
{
    partial class LinkButton
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
            this.labelText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelText
            // 
            this.labelText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelText.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelText.ForeColor = System.Drawing.Color.DodgerBlue;
            this.labelText.Location = new System.Drawing.Point(0, 0);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(102, 20);
            this.labelText.TabIndex = 0;
            this.labelText.Text = "示例菜单";
            this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelText.Paint += new System.Windows.Forms.PaintEventHandler(this.labelText_Paint);
            this.labelText.MouseEnter += new System.EventHandler(this.labelText_MouseEnter);
            this.labelText.MouseLeave += new System.EventHandler(this.labelText_MouseLeave);
            // 
            // LinkButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.labelText);
            this.Name = "LinkButton";
            this.Size = new System.Drawing.Size(102, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelText;
    }
}
