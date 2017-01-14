namespace 客户端主程序
{
    partial class SPXZTS
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.basicButton_js = new Com.Seezt.Skins.BasicButton();
            this.basicButton_sgy = new Com.Seezt.Skins.BasicButton();
            this.basicButton_yn = new Com.Seezt.Skins.BasicButton();
            this.basicButton_cancel = new Com.Seezt.Skins.BasicButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(413, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "您当前选择的商品名称为“您当前选择的商品名称您当前选择的商品名称”，";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "如需重新选择商品，点击“取消”返回。";
            // 
            // basicButton_js
            // 
            this.basicButton_js.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton_js.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton_js.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton_js.Location = new System.Drawing.Point(116, 114);
            this.basicButton_js.Name = "basicButton_js";
            this.basicButton_js.Size = new System.Drawing.Size(50, 21);
            this.basicButton_js.TabIndex = 19;
            this.basicButton_js.Texts = "即时";
            this.basicButton_js.Click += new System.EventHandler(this.basicButton_js_Click);
            // 
            // basicButton_sgy
            // 
            this.basicButton_sgy.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton_sgy.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton_sgy.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton_sgy.Location = new System.Drawing.Point(181, 114);
            this.basicButton_sgy.Name = "basicButton_sgy";
            this.basicButton_sgy.Size = new System.Drawing.Size(50, 21);
            this.basicButton_sgy.TabIndex = 20;
            this.basicButton_sgy.Texts = "三个月";
            this.basicButton_sgy.Click += new System.EventHandler(this.basicButton_js_Click);
            // 
            // basicButton_yn
            // 
            this.basicButton_yn.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton_yn.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton_yn.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton_yn.Location = new System.Drawing.Point(246, 114);
            this.basicButton_yn.Name = "basicButton_yn";
            this.basicButton_yn.Size = new System.Drawing.Size(50, 21);
            this.basicButton_yn.TabIndex = 21;
            this.basicButton_yn.Texts = "一年";
            this.basicButton_yn.Click += new System.EventHandler(this.basicButton_js_Click);
            // 
            // basicButton_cancel
            // 
            this.basicButton_cancel.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton_cancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton_cancel.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton_cancel.Location = new System.Drawing.Point(311, 114);
            this.basicButton_cancel.Name = "basicButton_cancel";
            this.basicButton_cancel.Size = new System.Drawing.Size(50, 21);
            this.basicButton_cancel.TabIndex = 22;
            this.basicButton_cancel.Texts = "取消";
            this.basicButton_cancel.Click += new System.EventHandler(this.basicButton_cancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.basicButton_cancel);
            this.panel1.Controls.Add(this.basicButton_yn);
            this.panel1.Controls.Add(this.basicButton_sgy);
            this.panel1.Controls.Add(this.basicButton_js);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 156);
            this.panel1.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "请选择一种合同期限；";
            // 
            // SPXZTS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 190);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SPXZTS";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "友情提示";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SPXZTS_FormClosed);
            this.Load += new System.EventHandler(this.SPXZTS_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Com.Seezt.Skins.BasicButton basicButton_js;
        private Com.Seezt.Skins.BasicButton basicButton_sgy;
        private Com.Seezt.Skins.BasicButton basicButton_yn;
        private Com.Seezt.Skins.BasicButton basicButton_cancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;


    }
}