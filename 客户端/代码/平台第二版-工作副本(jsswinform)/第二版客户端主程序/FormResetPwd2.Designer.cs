namespace 客户端主程序
{
    partial class FormResetPwd2
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
            this.lbSendAgain = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbCheckEmail = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbValNumberTips = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtValsNumber = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.lbNewPwdAgainTips = new System.Windows.Forms.Label();
            this.lbNewPwdTips = new System.Windows.Forms.Label();
            this.btnNext = new Com.Seezt.Skins.BasicButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNewPwdAgain = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.lbUserEmail = new System.Windows.Forms.Label();
            this.txtNewPwd = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.tips = new System.Windows.Forms.ToolTip(this.components);
            this.PnlBig.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBig
            // 
            this.PnlBig.BackColor = System.Drawing.Color.White;
            this.PnlBig.Controls.Add(this.lbSendAgain);
            this.PnlBig.Controls.Add(this.label5);
            this.PnlBig.Controls.Add(this.label6);
            this.PnlBig.Controls.Add(this.lbCheckEmail);
            this.PnlBig.Controls.Add(this.label4);
            this.PnlBig.Controls.Add(this.lbValNumberTips);
            this.PnlBig.Controls.Add(this.label3);
            this.PnlBig.Controls.Add(this.txtValsNumber);
            this.PnlBig.Controls.Add(this.lbNewPwdAgainTips);
            this.PnlBig.Controls.Add(this.lbNewPwdTips);
            this.PnlBig.Controls.Add(this.btnNext);
            this.PnlBig.Controls.Add(this.label1);
            this.PnlBig.Controls.Add(this.txtNewPwdAgain);
            this.PnlBig.Controls.Add(this.lbUserEmail);
            this.PnlBig.Controls.Add(this.txtNewPwd);
            this.PnlBig.Location = new System.Drawing.Point(2, 29);
            this.PnlBig.Name = "PnlBig";
            this.PnlBig.Size = new System.Drawing.Size(587, 278);
            this.PnlBig.TabIndex = 47;
            // 
            // lbSendAgain
            // 
            this.lbSendAgain.AutoSize = true;
            this.lbSendAgain.BackColor = System.Drawing.Color.Transparent;
            this.lbSendAgain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSendAgain.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSendAgain.ForeColor = System.Drawing.Color.Red;
            this.lbSendAgain.Location = new System.Drawing.Point(171, 176);
            this.lbSendAgain.Name = "lbSendAgain";
            this.lbSendAgain.Size = new System.Drawing.Size(53, 12);
            this.lbSendAgain.TabIndex = 63;
            this.lbSendAgain.Text = "再次发送";
            this.lbSendAgain.Click += new System.EventHandler(this.lbSendAgain_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(121, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 62;
            this.label5.Text = "没有，可";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(121, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(425, 12);
            this.label6.TabIndex = 61;
            this.label6.Text = "若“收件箱”没有找到验证邮件，建议检查“广告邮件”或“垃圾箱”，如果仍";
            // 
            // lbCheckEmail
            // 
            this.lbCheckEmail.AutoSize = true;
            this.lbCheckEmail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbCheckEmail.ForeColor = System.Drawing.Color.Red;
            this.lbCheckEmail.Location = new System.Drawing.Point(338, 36);
            this.lbCheckEmail.Name = "lbCheckEmail";
            this.lbCheckEmail.Size = new System.Drawing.Size(53, 12);
            this.lbCheckEmail.TabIndex = 60;
            this.lbCheckEmail.Text = "查看邮箱";
            this.lbCheckEmail.Click += new System.EventHandler(this.lbCheckEmail_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(121, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(221, 12);
            this.label4.TabIndex = 59;
            this.label4.Text = "您的邮箱将在数分钟内收到验证邮件，去";
            // 
            // lbValNumberTips
            // 
            this.lbValNumberTips.AutoSize = true;
            this.lbValNumberTips.ForeColor = System.Drawing.Color.Red;
            this.lbValNumberTips.Location = new System.Drawing.Point(398, 71);
            this.lbValNumberTips.Name = "lbValNumberTips";
            this.lbValNumberTips.Size = new System.Drawing.Size(59, 12);
            this.lbValNumberTips.TabIndex = 58;
            this.lbValNumberTips.Text = "我是label";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(145, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 57;
            this.label3.Text = "验证码：";
            // 
            // txtValsNumber
            // 
            this.txtValsNumber.BackColor = System.Drawing.Color.White;
            this.txtValsNumber.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtValsNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValsNumber.CanPASTE = true;
            this.txtValsNumber.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtValsNumber.ForeColor = System.Drawing.Color.Black;
            this.txtValsNumber.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtValsNumber.Location = new System.Drawing.Point(207, 66);
            this.txtValsNumber.MaxLength = 20;
            this.txtValsNumber.Name = "txtValsNumber";
            this.txtValsNumber.Size = new System.Drawing.Size(185, 23);
            this.txtValsNumber.TabIndex = 56;
            this.txtValsNumber.TextNtip = "";
            this.txtValsNumber.WordWrap = false;
            this.txtValsNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtValsNumber_KeyUp);
            // 
            // lbNewPwdAgainTips
            // 
            this.lbNewPwdAgainTips.AutoSize = true;
            this.lbNewPwdAgainTips.ForeColor = System.Drawing.Color.Red;
            this.lbNewPwdAgainTips.Location = new System.Drawing.Point(398, 131);
            this.lbNewPwdAgainTips.Name = "lbNewPwdAgainTips";
            this.lbNewPwdAgainTips.Size = new System.Drawing.Size(59, 12);
            this.lbNewPwdAgainTips.TabIndex = 55;
            this.lbNewPwdAgainTips.Text = "我是label";
            // 
            // lbNewPwdTips
            // 
            this.lbNewPwdTips.AutoSize = true;
            this.lbNewPwdTips.ForeColor = System.Drawing.Color.Red;
            this.lbNewPwdTips.Location = new System.Drawing.Point(398, 102);
            this.lbNewPwdTips.Name = "lbNewPwdTips";
            this.lbNewPwdTips.Size = new System.Drawing.Size(59, 12);
            this.lbNewPwdTips.TabIndex = 54;
            this.lbNewPwdTips.Text = "我是label";
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnNext.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNext.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnNext.Location = new System.Drawing.Point(209, 196);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 22);
            this.btnNext.TabIndex = 53;
            this.btnNext.Texts = "修改";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(121, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 52;
            this.label1.Text = "确认新密码：";
            // 
            // txtNewPwdAgain
            // 
            this.txtNewPwdAgain.BackColor = System.Drawing.Color.White;
            this.txtNewPwdAgain.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtNewPwdAgain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewPwdAgain.CanPASTE = true;
            this.txtNewPwdAgain.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNewPwdAgain.ForeColor = System.Drawing.Color.Black;
            this.txtNewPwdAgain.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtNewPwdAgain.Location = new System.Drawing.Point(207, 126);
            this.txtNewPwdAgain.MaxLength = 50;
            this.txtNewPwdAgain.Name = "txtNewPwdAgain";
            this.txtNewPwdAgain.Size = new System.Drawing.Size(185, 23);
            this.txtNewPwdAgain.TabIndex = 51;
            this.txtNewPwdAgain.TextNtip = "";
            this.txtNewPwdAgain.UseSystemPasswordChar = true;
            this.txtNewPwdAgain.WordWrap = false;
            this.txtNewPwdAgain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNewPwdAgain_KeyUp);
            // 
            // lbUserEmail
            // 
            this.lbUserEmail.AutoSize = true;
            this.lbUserEmail.BackColor = System.Drawing.Color.Transparent;
            this.lbUserEmail.Location = new System.Drawing.Point(121, 102);
            this.lbUserEmail.Name = "lbUserEmail";
            this.lbUserEmail.Size = new System.Drawing.Size(77, 12);
            this.lbUserEmail.TabIndex = 44;
            this.lbUserEmail.Text = "输入新密码：";
            // 
            // txtNewPwd
            // 
            this.txtNewPwd.BackColor = System.Drawing.Color.White;
            this.txtNewPwd.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtNewPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewPwd.CanPASTE = true;
            this.txtNewPwd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNewPwd.ForeColor = System.Drawing.Color.Black;
            this.txtNewPwd.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtNewPwd.Location = new System.Drawing.Point(207, 97);
            this.txtNewPwd.MaxLength = 50;
            this.txtNewPwd.Name = "txtNewPwd";
            this.txtNewPwd.Size = new System.Drawing.Size(185, 23);
            this.txtNewPwd.TabIndex = 42;
            this.txtNewPwd.TextNtip = "";
            this.txtNewPwd.UseSystemPasswordChar = true;
            this.txtNewPwd.WordWrap = false;
            this.txtNewPwd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtNewPwd_KeyUp);
            // 
            // FormResetPwd2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(591, 310);
            this.Controls.Add(this.PnlBig);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormResetPwd2";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中国商品批发交易平台找回密码";
            this.Activated += new System.EventHandler(this.FormResetPwd2_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormResetPwd2_FormClosed);
            this.Load += new System.EventHandler(this.FormResetPwd2_Load);
            this.Controls.SetChildIndex(this.PnlBig, 0);
            this.PnlBig.ResumeLayout(false);
            this.PnlBig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PnlBig;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbCheckEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbValNumberTips;
        private System.Windows.Forms.Label label3;
        private SubForm.UCTextBox txtValsNumber;
        private System.Windows.Forms.Label lbNewPwdAgainTips;
        private System.Windows.Forms.Label lbNewPwdTips;
        private Com.Seezt.Skins.BasicButton btnNext;
        private System.Windows.Forms.Label label1;
        private SubForm.UCTextBox txtNewPwdAgain;
        private System.Windows.Forms.Label lbUserEmail;
        private SubForm.UCTextBox txtNewPwd;
        private System.Windows.Forms.Label lbSendAgain;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip tips;
    }
}