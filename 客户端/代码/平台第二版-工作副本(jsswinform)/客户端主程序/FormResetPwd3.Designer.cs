namespace 客户端主程序
{
    partial class FormResetPwd3
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
            this.lbSendMsgAgain = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ucTextBox1 = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.lbPhoneText = new System.Windows.Forms.Label();
            this.lbEmailText = new System.Windows.Forms.Label();
            this.btnNext = new Com.Seezt.Skins.BasicButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPhone = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.lbUserEmail = new System.Windows.Forms.Label();
            this.txt_UserEmail = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.PnlBig.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlBig
            // 
            this.PnlBig.BackColor = System.Drawing.Color.White;
            this.PnlBig.Controls.Add(this.lbSendMsgAgain);
            this.PnlBig.Controls.Add(this.label6);
            this.PnlBig.Controls.Add(this.label4);
            this.PnlBig.Controls.Add(this.label2);
            this.PnlBig.Controls.Add(this.label3);
            this.PnlBig.Controls.Add(this.ucTextBox1);
            this.PnlBig.Controls.Add(this.lbPhoneText);
            this.PnlBig.Controls.Add(this.lbEmailText);
            this.PnlBig.Controls.Add(this.btnNext);
            this.PnlBig.Controls.Add(this.label1);
            this.PnlBig.Controls.Add(this.txtPhone);
            this.PnlBig.Controls.Add(this.lbUserEmail);
            this.PnlBig.Controls.Add(this.txt_UserEmail);
            this.PnlBig.Location = new System.Drawing.Point(2, 29);
            this.PnlBig.Name = "PnlBig";
            this.PnlBig.Size = new System.Drawing.Size(587, 278);
            this.PnlBig.TabIndex = 48;
            // 
            // lbSendMsgAgain
            // 
            this.lbSendMsgAgain.AutoSize = true;
            this.lbSendMsgAgain.BackColor = System.Drawing.Color.Transparent;
            this.lbSendMsgAgain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSendMsgAgain.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbSendMsgAgain.Location = new System.Drawing.Point(255, 158);
            this.lbSendMsgAgain.Name = "lbSendMsgAgain";
            this.lbSendMsgAgain.Size = new System.Drawing.Size(113, 12);
            this.lbSendMsgAgain.TabIndex = 63;
            this.lbSendMsgAgain.Text = "重新获取短信验证码";
            this.lbSendMsgAgain.Click += new System.EventHandler(this.lbSendMsgAgain_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(121, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 12);
            this.label6.TabIndex = 61;
            this.label6.Text = "如果没有收到验证码，请 ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(121, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(263, 12);
            this.label4.TabIndex = 59;
            this.label4.Text = "您的手机将在数分钟内收到验证码,请稍等片刻。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(398, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 58;
            this.label2.Text = "我是label";
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
            // ucTextBox1
            // 
            this.ucTextBox1.BackColor = System.Drawing.Color.White;
            this.ucTextBox1.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBox1.ForeColor = System.Drawing.Color.Black;
            this.ucTextBox1.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBox1.Location = new System.Drawing.Point(207, 66);
            this.ucTextBox1.MaxLength = 20;
            this.ucTextBox1.Name = "ucTextBox1";
            this.ucTextBox1.Size = new System.Drawing.Size(185, 23);
            this.ucTextBox1.TabIndex = 56;
            this.ucTextBox1.WordWrap = false;
            // 
            // lbPhoneText
            // 
            this.lbPhoneText.AutoSize = true;
            this.lbPhoneText.ForeColor = System.Drawing.Color.Red;
            this.lbPhoneText.Location = new System.Drawing.Point(398, 131);
            this.lbPhoneText.Name = "lbPhoneText";
            this.lbPhoneText.Size = new System.Drawing.Size(59, 12);
            this.lbPhoneText.TabIndex = 55;
            this.lbPhoneText.Text = "我是label";
            // 
            // lbEmailText
            // 
            this.lbEmailText.AutoSize = true;
            this.lbEmailText.ForeColor = System.Drawing.Color.Red;
            this.lbEmailText.Location = new System.Drawing.Point(398, 102);
            this.lbEmailText.Name = "lbEmailText";
            this.lbEmailText.Size = new System.Drawing.Size(59, 12);
            this.lbEmailText.TabIndex = 54;
            this.lbEmailText.Text = "我是label";
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
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.White;
            this.txtPhone.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPhone.ForeColor = System.Drawing.Color.Black;
            this.txtPhone.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtPhone.Location = new System.Drawing.Point(207, 126);
            this.txtPhone.MaxLength = 50;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(185, 23);
            this.txtPhone.TabIndex = 51;
            this.txtPhone.WordWrap = false;
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
            // txt_UserEmail
            // 
            this.txt_UserEmail.BackColor = System.Drawing.Color.White;
            this.txt_UserEmail.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txt_UserEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_UserEmail.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_UserEmail.ForeColor = System.Drawing.Color.Black;
            this.txt_UserEmail.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txt_UserEmail.Location = new System.Drawing.Point(207, 97);
            this.txt_UserEmail.MaxLength = 50;
            this.txt_UserEmail.Name = "txt_UserEmail";
            this.txt_UserEmail.Size = new System.Drawing.Size(185, 23);
            this.txt_UserEmail.TabIndex = 42;
            this.txt_UserEmail.WordWrap = false;
            // 
            // FormResetPwd3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(591, 310);
            this.Controls.Add(this.PnlBig);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormResetPwd3";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中国商品批发交易平台找回密码";
            this.Controls.SetChildIndex(this.PnlBig, 0);
            this.PnlBig.ResumeLayout(false);
            this.PnlBig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PnlBig;
        private System.Windows.Forms.Label lbSendMsgAgain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private SubForm.UCTextBox ucTextBox1;
        private System.Windows.Forms.Label lbPhoneText;
        private System.Windows.Forms.Label lbEmailText;
        private Com.Seezt.Skins.BasicButton btnNext;
        private System.Windows.Forms.Label label1;
        private SubForm.UCTextBox txtPhone;
        private System.Windows.Forms.Label lbUserEmail;
        private SubForm.UCTextBox txt_UserEmail;
    }
}