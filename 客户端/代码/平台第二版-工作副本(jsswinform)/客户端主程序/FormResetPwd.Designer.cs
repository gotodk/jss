namespace 客户端主程序
{
    partial class FormResetPwd
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
            this.lbUserEmail = new System.Windows.Forms.Label();
            this.PnlBig = new System.Windows.Forms.Panel();
            this.lbPhoneText = new System.Windows.Forms.Label();
            this.lbEmailText = new System.Windows.Forms.Label();
            this.btnNext = new Com.Seezt.Skins.BasicButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPhone = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.rbtnPhone = new System.Windows.Forms.RadioButton();
            this.rbtnEmail = new System.Windows.Forms.RadioButton();
            this.lbFS = new System.Windows.Forms.Label();
            this.txt_UserEmail = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.PnlBig.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbUserEmail
            // 
            this.lbUserEmail.AutoSize = true;
            this.lbUserEmail.BackColor = System.Drawing.Color.Transparent;
            this.lbUserEmail.Location = new System.Drawing.Point(83, 91);
            this.lbUserEmail.Name = "lbUserEmail";
            this.lbUserEmail.Size = new System.Drawing.Size(137, 12);
            this.lbUserEmail.TabIndex = 44;
            this.lbUserEmail.Text = "请输入注册填写的邮箱：";
            // 
            // PnlBig
            // 
            this.PnlBig.BackColor = System.Drawing.Color.White;
            this.PnlBig.Controls.Add(this.lbPhoneText);
            this.PnlBig.Controls.Add(this.lbEmailText);
            this.PnlBig.Controls.Add(this.btnNext);
            this.PnlBig.Controls.Add(this.label1);
            this.PnlBig.Controls.Add(this.txtPhone);
            this.PnlBig.Controls.Add(this.rbtnPhone);
            this.PnlBig.Controls.Add(this.rbtnEmail);
            this.PnlBig.Controls.Add(this.lbFS);
            this.PnlBig.Controls.Add(this.lbUserEmail);
            this.PnlBig.Controls.Add(this.txt_UserEmail);
            this.PnlBig.Location = new System.Drawing.Point(2, 30);
            this.PnlBig.Name = "PnlBig";
            this.PnlBig.Size = new System.Drawing.Size(587, 278);
            this.PnlBig.TabIndex = 46;
            // 
            // lbPhoneText
            // 
            this.lbPhoneText.AutoSize = true;
            this.lbPhoneText.ForeColor = System.Drawing.Color.Red;
            this.lbPhoneText.Location = new System.Drawing.Point(417, 120);
            this.lbPhoneText.Name = "lbPhoneText";
            this.lbPhoneText.Size = new System.Drawing.Size(59, 12);
            this.lbPhoneText.TabIndex = 55;
            this.lbPhoneText.Text = "我是label";
            this.lbPhoneText.Visible = false;
            // 
            // lbEmailText
            // 
            this.lbEmailText.AutoSize = true;
            this.lbEmailText.ForeColor = System.Drawing.Color.Red;
            this.lbEmailText.Location = new System.Drawing.Point(417, 91);
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
            this.btnNext.Location = new System.Drawing.Point(224, 130);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 22);
            this.btnNext.TabIndex = 100;
            this.btnNext.Texts = "下一步";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(119, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 52;
            this.label1.Text = "请输入手机号码：";
            this.label1.Visible = false;
            // 
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.White;
            this.txtPhone.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPhone.ForeColor = System.Drawing.Color.Black;
            this.txtPhone.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtPhone.Location = new System.Drawing.Point(226, 115);
            this.txtPhone.MaxLength = 11;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(185, 23);
            this.txtPhone.TabIndex = 99;
            this.txtPhone.Visible = false;
            this.txtPhone.WordWrap = false;
            this.txtPhone.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPhone_KeyUp);
            // 
            // rbtnPhone
            // 
            this.rbtnPhone.AutoSize = true;
            this.rbtnPhone.Location = new System.Drawing.Point(303, 60);
            this.rbtnPhone.Name = "rbtnPhone";
            this.rbtnPhone.Size = new System.Drawing.Size(71, 16);
            this.rbtnPhone.TabIndex = 97;
            this.rbtnPhone.TabStop = true;
            this.rbtnPhone.Tag = "使用手机找回密码";
            this.rbtnPhone.Text = "手机找回";
            this.rbtnPhone.UseVisualStyleBackColor = true;
            this.rbtnPhone.Visible = false;
            this.rbtnPhone.CheckedChanged += new System.EventHandler(this.rbtnPhone_CheckedChanged);
            // 
            // rbtnEmail
            // 
            this.rbtnEmail.AutoSize = true;
            this.rbtnEmail.Checked = true;
            this.rbtnEmail.Location = new System.Drawing.Point(226, 60);
            this.rbtnEmail.Name = "rbtnEmail";
            this.rbtnEmail.Size = new System.Drawing.Size(71, 16);
            this.rbtnEmail.TabIndex = 96;
            this.rbtnEmail.TabStop = true;
            this.rbtnEmail.Tag = "使用邮箱找回密码";
            this.rbtnEmail.Text = "邮箱找回";
            this.rbtnEmail.UseVisualStyleBackColor = true;
            this.rbtnEmail.CheckedChanged += new System.EventHandler(this.rbtnEmail_CheckedChanged);
            // 
            // lbFS
            // 
            this.lbFS.AutoSize = true;
            this.lbFS.Location = new System.Drawing.Point(119, 62);
            this.lbFS.Name = "lbFS";
            this.lbFS.Size = new System.Drawing.Size(101, 12);
            this.lbFS.TabIndex = 48;
            this.lbFS.Text = "请选择找回方式：";
            // 
            // txt_UserEmail
            // 
            this.txt_UserEmail.BackColor = System.Drawing.Color.White;
            this.txt_UserEmail.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txt_UserEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_UserEmail.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_UserEmail.ForeColor = System.Drawing.Color.Black;
            this.txt_UserEmail.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txt_UserEmail.Location = new System.Drawing.Point(226, 86);
            this.txt_UserEmail.MaxLength = 40;
            this.txt_UserEmail.Name = "txt_UserEmail";
            this.txt_UserEmail.Size = new System.Drawing.Size(185, 23);
            this.txt_UserEmail.TabIndex = 98;
            this.txt_UserEmail.WordWrap = false;
            this.txt_UserEmail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_UserEmail_KeyUp);
            // 
            // FormResetPwd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(591, 310);
            this.Controls.Add(this.PnlBig);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormResetPwd";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中国商品批发交易平台找回密码";
            this.Load += new System.EventHandler(this.FormResetPwd_Load);
            this.Controls.SetChildIndex(this.PnlBig, 0);
            this.PnlBig.ResumeLayout(false);
            this.PnlBig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SubForm.UCTextBox txt_UserEmail;
        private System.Windows.Forms.Label lbUserEmail;
        private System.Windows.Forms.Panel PnlBig;
        private System.Windows.Forms.Label lbFS;
        private System.Windows.Forms.RadioButton rbtnPhone;
        private System.Windows.Forms.RadioButton rbtnEmail;
        private System.Windows.Forms.Label label1;
        private SubForm.UCTextBox txtPhone;
        private Com.Seezt.Skins.BasicButton btnNext;
        private System.Windows.Forms.Label lbPhoneText;
        private System.Windows.Forms.Label lbEmailText;
    }
}