namespace 客户端主程序
{
    partial class FormLogin
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Blogin = new Com.Seezt.Skins.BasicButton();
            this.MFZC = new System.Windows.Forms.Label();
            this.WJMM = new System.Windows.Forms.Label();
            this.JZMM = new System.Windows.Forms.CheckBox();
            this.panel_login = new System.Windows.Forms.Panel();
            this.PBload = new System.Windows.Forms.PictureBox();
            this.username = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.userpassword = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_login.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(2, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 250);
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 35;
            this.label1.Text = "账号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "密码：";
            // 
            // Blogin
            // 
            this.Blogin.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Blogin.Enabled = false;
            this.Blogin.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Blogin.ForeColor = System.Drawing.Color.DarkBlue;
            this.Blogin.Location = new System.Drawing.Point(192, 128);
            this.Blogin.Name = "Blogin";
            this.Blogin.Size = new System.Drawing.Size(70, 22);
            this.Blogin.TabIndex = 1000;
            this.Blogin.Texts = "登录";
            this.Blogin.Click += new System.EventHandler(this.basicButton2_Click);
            // 
            // MFZC
            // 
            this.MFZC.AutoSize = true;
            this.MFZC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MFZC.Enabled = false;
            this.MFZC.Location = new System.Drawing.Point(383, 32);
            this.MFZC.Name = "MFZC";
            this.MFZC.Size = new System.Drawing.Size(53, 12);
            this.MFZC.TabIndex = 39;
            this.MFZC.Text = "免费注册";
            this.MFZC.Click += new System.EventHandler(this.MFZC_Click);
            // 
            // WJMM
            // 
            this.WJMM.AutoSize = true;
            this.WJMM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WJMM.Enabled = false;
            this.WJMM.Location = new System.Drawing.Point(383, 67);
            this.WJMM.Name = "WJMM";
            this.WJMM.Size = new System.Drawing.Size(53, 12);
            this.WJMM.TabIndex = 41;
            this.WJMM.Tag = "";
            this.WJMM.Text = "忘记密码";
            this.WJMM.Click += new System.EventHandler(this.WJMM_Click);
            // 
            // JZMM
            // 
            this.JZMM.AutoSize = true;
            this.JZMM.Enabled = false;
            this.JZMM.Location = new System.Drawing.Point(192, 97);
            this.JZMM.Name = "JZMM";
            this.JZMM.Size = new System.Drawing.Size(72, 16);
            this.JZMM.TabIndex = 999;
            this.JZMM.Text = "记住密码";
            this.JZMM.UseVisualStyleBackColor = true;
            this.JZMM.CheckedChanged += new System.EventHandler(this.JZMM_CheckedChanged);
            // 
            // panel_login
            // 
            this.panel_login.BackColor = System.Drawing.Color.White;
            this.panel_login.Controls.Add(this.PBload);
            this.panel_login.Controls.Add(this.username);
            this.panel_login.Controls.Add(this.JZMM);
            this.panel_login.Controls.Add(this.label1);
            this.panel_login.Controls.Add(this.WJMM);
            this.panel_login.Controls.Add(this.label2);
            this.panel_login.Controls.Add(this.MFZC);
            this.panel_login.Controls.Add(this.userpassword);
            this.panel_login.Controls.Add(this.Blogin);
            this.panel_login.Location = new System.Drawing.Point(3, 281);
            this.panel_login.Name = "panel_login";
            this.panel_login.Size = new System.Drawing.Size(598, 183);
            this.panel_login.TabIndex = 43;
            // 
            // PBload
            // 
            this.PBload.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBload.Location = new System.Drawing.Point(273, 123);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 43;
            this.PBload.TabStop = false;
            // 
            // username
            // 
            this.username.BackColor = System.Drawing.Color.White;
            this.username.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.username.CanPASTE = true;
            this.username.Enabled = false;
            this.username.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.username.ForeColor = System.Drawing.Color.Black;
            this.username.HotColor = System.Drawing.Color.CornflowerBlue;
            this.username.Location = new System.Drawing.Point(192, 27);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(185, 23);
            this.username.TabIndex = 997;
            this.username.TextNtip = "";
            this.username.WordWrap = false;
            // 
            // userpassword
            // 
            this.userpassword.BackColor = System.Drawing.Color.White;
            this.userpassword.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.userpassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userpassword.CanPASTE = true;
            this.userpassword.Enabled = false;
            this.userpassword.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userpassword.ForeColor = System.Drawing.Color.Black;
            this.userpassword.HotColor = System.Drawing.Color.CornflowerBlue;
            this.userpassword.Location = new System.Drawing.Point(192, 61);
            this.userpassword.MaxLength = 16;
            this.userpassword.Name = "userpassword";
            this.userpassword.PasswordChar = '*';
            this.userpassword.Size = new System.Drawing.Size(185, 23);
            this.userpassword.TabIndex = 998;
            this.userpassword.TextNtip = "";
            this.userpassword.WordWrap = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 467);
            this.Controls.Add(this.panel_login);
            this.Controls.Add(this.pictureBox1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中国商品批发交易平台（模拟运行）";
            this.Deactivate += new System.EventHandler(this.FormLogin_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogin_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormLogin_FormClosed);
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.panel_login, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_login.ResumeLayout(false);
            this.panel_login.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private SubForm.UCTextBox username;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private SubForm.UCTextBox userpassword;
        private Com.Seezt.Skins.BasicButton Blogin;
        private System.Windows.Forms.Label MFZC;
        private System.Windows.Forms.Label WJMM;
        private System.Windows.Forms.CheckBox JZMM;
        private System.Windows.Forms.Panel panel_login;
        private System.Windows.Forms.PictureBox PBload;
        private System.Windows.Forms.Timer timer1;
    }
}