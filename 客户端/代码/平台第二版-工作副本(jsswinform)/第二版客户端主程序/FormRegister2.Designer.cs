namespace 客户端主程序
{
    partial class FormRegister2
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
            this.components = new System.ComponentModel.Container();
            this.PnlBig = new System.Windows.Forms.Panel();
            this.PBload = new System.Windows.Forms.PictureBox();
            this.lbChangeEmail = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbSendAgain = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbCheckEmail = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new Com.Seezt.Skins.BasicButton();
            this.lbUserEmail = new System.Windows.Forms.Label();
            this.txtValNum = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tips = new System.Windows.Forms.ToolTip(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.PnlBig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBig
            // 
            this.PnlBig.BackColor = System.Drawing.Color.White;
            this.PnlBig.Controls.Add(this.label10);
            this.PnlBig.Controls.Add(this.PBload);
            this.PnlBig.Controls.Add(this.lbChangeEmail);
            this.PnlBig.Controls.Add(this.label13);
            this.PnlBig.Controls.Add(this.lbSendAgain);
            this.PnlBig.Controls.Add(this.label11);
            this.PnlBig.Controls.Add(this.lbCheckEmail);
            this.PnlBig.Controls.Add(this.label9);
            this.PnlBig.Controls.Add(this.label8);
            this.PnlBig.Controls.Add(this.label1);
            this.PnlBig.Controls.Add(this.btnNext);
            this.PnlBig.Controls.Add(this.lbUserEmail);
            this.PnlBig.Controls.Add(this.txtValNum);
            this.PnlBig.Location = new System.Drawing.Point(2, 78);
            this.PnlBig.Name = "PnlBig";
            this.PnlBig.Size = new System.Drawing.Size(587, 251);
            this.PnlBig.TabIndex = 47;
            // 
            // PBload
            // 
            this.PBload.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBload.Location = new System.Drawing.Point(200, 190);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 74;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // lbChangeEmail
            // 
            this.lbChangeEmail.AutoSize = true;
            this.lbChangeEmail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbChangeEmail.ForeColor = System.Drawing.Color.Red;
            this.lbChangeEmail.Location = new System.Drawing.Point(124, 158);
            this.lbChangeEmail.Name = "lbChangeEmail";
            this.lbChangeEmail.Size = new System.Drawing.Size(53, 12);
            this.lbChangeEmail.TabIndex = 61;
            this.lbChangeEmail.Text = "更换邮箱";
            this.lbChangeEmail.Click += new System.EventHandler(this.lbChangeEmail_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(109, 158);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 12);
            this.label13.TabIndex = 60;
            this.label13.Text = "或";
            // 
            // lbSendAgain
            // 
            this.lbSendAgain.AutoSize = true;
            this.lbSendAgain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSendAgain.ForeColor = System.Drawing.Color.Red;
            this.lbSendAgain.Location = new System.Drawing.Point(57, 158);
            this.lbSendAgain.Name = "lbSendAgain";
            this.lbSendAgain.Size = new System.Drawing.Size(53, 12);
            this.lbSendAgain.TabIndex = 59;
            this.lbSendAgain.Text = "再次发送";
            this.lbSendAgain.Click += new System.EventHandler(this.lbSendAgain_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(57, 134);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(473, 12);
            this.label11.TabIndex = 58;
            this.label11.Text = "若“收件箱”没有找到验证邮件，建议检查“广告邮件”或”垃圾箱”，如果仍没有，可";
            // 
            // lbCheckEmail
            // 
            this.lbCheckEmail.AutoSize = true;
            this.lbCheckEmail.BackColor = System.Drawing.Color.Transparent;
            this.lbCheckEmail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbCheckEmail.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbCheckEmail.Location = new System.Drawing.Point(271, 56);
            this.lbCheckEmail.Name = "lbCheckEmail";
            this.lbCheckEmail.Size = new System.Drawing.Size(53, 12);
            this.lbCheckEmail.TabIndex = 57;
            this.lbCheckEmail.Text = "查看邮箱";
            this.lbCheckEmail.Click += new System.EventHandler(this.lbCheckEmail_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(58, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(215, 12);
            this.label9.TabIndex = 56;
            this.label9.Text = "您的邮箱将在数分钟内收到验证邮件,去";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(56, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(360, 14);
            this.label8.TabIndex = 55;
            this.label8.Text = "完成邮箱验证,即可使用邮箱找回密码和接收平台信息";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(587, 1);
            this.label1.TabIndex = 48;
            this.label1.Text = "label1";
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNext.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnNext.Location = new System.Drawing.Point(111, 200);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 22);
            this.btnNext.TabIndex = 53;
            this.btnNext.Texts = "下一步";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lbUserEmail
            // 
            this.lbUserEmail.AutoSize = true;
            this.lbUserEmail.BackColor = System.Drawing.Color.Transparent;
            this.lbUserEmail.Location = new System.Drawing.Point(58, 95);
            this.lbUserEmail.Name = "lbUserEmail";
            this.lbUserEmail.Size = new System.Drawing.Size(53, 12);
            this.lbUserEmail.TabIndex = 44;
            this.lbUserEmail.Text = "验证码：";
            this.lbUserEmail.Visible = false;
            // 
            // txtValNum
            // 
            this.txtValNum.BackColor = System.Drawing.Color.White;
            this.txtValNum.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtValNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValNum.CanPASTE = true;
            this.txtValNum.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtValNum.ForeColor = System.Drawing.Color.Black;
            this.txtValNum.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtValNum.Location = new System.Drawing.Point(111, 90);
            this.txtValNum.MaxLength = 20;
            this.txtValNum.Name = "txtValNum";
            this.txtValNum.Size = new System.Drawing.Size(185, 22);
            this.txtValNum.TabIndex = 42;
            this.txtValNum.Text = "123456";
            this.txtValNum.TextNtip = "";
            this.txtValNum.Visible = false;
            this.txtValNum.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(2, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 48);
            this.panel1.TabIndex = 48;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(254, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 1);
            this.label7.TabIndex = 32;
            this.label7.Text = "label7";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(174, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 1);
            this.label6.TabIndex = 33;
            this.label6.Text = "label6";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(312, 11);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(22, 22);
            this.panel4.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 20);
            this.label5.TabIndex = 26;
            this.label5.Text = "3";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Location = new System.Drawing.Point(232, 11);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(22, 22);
            this.panel3.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 19);
            this.label4.TabIndex = 28;
            this.label4.Text = "2";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(152, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(22, 22);
            this.panel2.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 20);
            this.label3.TabIndex = 30;
            this.label3.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(25, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 14);
            this.label2.TabIndex = 37;
            this.label2.Text = "30秒快速注册";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label10.Location = new System.Drawing.Point(57, 95);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(311, 12);
            this.label10.TabIndex = 75;
            this.label10.Text = " 模拟运营期间暂不验证邮箱验证码，请直接进入下一步。";
            // 
            // FormRegister2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(591, 331);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PnlBig);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegister2";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中国商品批发交易平台注册";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRegister2_FormClosing);
            this.Load += new System.EventHandler(this.FormRegister2_Load);
            this.Controls.SetChildIndex(this.PnlBig, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.PnlBig.ResumeLayout(false);
            this.PnlBig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PnlBig;
        private Com.Seezt.Skins.BasicButton btnNext;
        private System.Windows.Forms.Label lbUserEmail;
        private SubForm.UCTextBox txtValNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbCheckEmail;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbSendAgain;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbChangeEmail;
        private System.Windows.Forms.ToolTip tips;
        private System.Windows.Forms.PictureBox PBload;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
    }
}