namespace 客户端主程序
{
    partial class FormRegister
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
            this.CB_TYXY = new System.Windows.Forms.CheckBox();
            this.labFMPTXY = new System.Windows.Forms.LinkLabel();
            this.TipsErrorPwdAgain = new System.Windows.Forms.Label();
            this.TipsErrorUPwd = new System.Windows.Forms.Label();
            this.TipsErrorUName = new System.Windows.Forms.Label();
            this.PBload = new System.Windows.Forms.PictureBox();
            this.TipsInputPwdAgain = new System.Windows.Forms.RichTextBox();
            this.TipsInputPwd = new System.Windows.Forms.RichTextBox();
            this.TipsInputUName = new System.Windows.Forms.RichTextBox();
            this.TipsErrorEmail = new System.Windows.Forms.Label();
            this.TipsInputEmail = new System.Windows.Forms.RichTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtUserPwdAgain = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.txtUserPwd = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.txtUserName = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new Com.Seezt.Skins.BasicButton();
            this.lbUserEmail = new System.Windows.Forms.Label();
            this.txtUserEmail = new 客户端主程序.SubForm.UCTextBox(this.components);
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
            this.PnlBig.Controls.Add(this.CB_TYXY);
            this.PnlBig.Controls.Add(this.labFMPTXY);
            this.PnlBig.Controls.Add(this.TipsErrorPwdAgain);
            this.PnlBig.Controls.Add(this.TipsErrorUPwd);
            this.PnlBig.Controls.Add(this.TipsErrorUName);
            this.PnlBig.Controls.Add(this.PBload);
            this.PnlBig.Controls.Add(this.TipsInputPwdAgain);
            this.PnlBig.Controls.Add(this.TipsInputPwd);
            this.PnlBig.Controls.Add(this.TipsInputUName);
            this.PnlBig.Controls.Add(this.TipsErrorEmail);
            this.PnlBig.Controls.Add(this.TipsInputEmail);
            this.PnlBig.Controls.Add(this.label16);
            this.PnlBig.Controls.Add(this.label15);
            this.PnlBig.Controls.Add(this.label14);
            this.PnlBig.Controls.Add(this.label13);
            this.PnlBig.Controls.Add(this.label12);
            this.PnlBig.Controls.Add(this.txtUserPwdAgain);
            this.PnlBig.Controls.Add(this.label11);
            this.PnlBig.Controls.Add(this.txtUserPwd);
            this.PnlBig.Controls.Add(this.label10);
            this.PnlBig.Controls.Add(this.txtUserName);
            this.PnlBig.Controls.Add(this.label1);
            this.PnlBig.Controls.Add(this.btnNext);
            this.PnlBig.Controls.Add(this.lbUserEmail);
            this.PnlBig.Controls.Add(this.txtUserEmail);
            this.PnlBig.Location = new System.Drawing.Point(2, 78);
            this.PnlBig.Name = "PnlBig";
            this.PnlBig.Size = new System.Drawing.Size(587, 251);
            this.PnlBig.TabIndex = 1;
            // 
            // CB_TYXY
            // 
            this.CB_TYXY.AutoSize = true;
            this.CB_TYXY.Location = new System.Drawing.Point(120, 154);
            this.CB_TYXY.Name = "CB_TYXY";
            this.CB_TYXY.Size = new System.Drawing.Size(48, 16);
            this.CB_TYXY.TabIndex = 73;
            this.CB_TYXY.Text = "同意";
            this.CB_TYXY.UseVisualStyleBackColor = true;
            this.CB_TYXY.CheckedChanged += new System.EventHandler(this.CB_TYXY_CheckedChanged);
            // 
            // labFMPTXY
            // 
            this.labFMPTXY.AutoSize = true;
            this.labFMPTXY.Location = new System.Drawing.Point(167, 155);
            this.labFMPTXY.Name = "labFMPTXY";
            this.labFMPTXY.Size = new System.Drawing.Size(101, 12);
            this.labFMPTXY.TabIndex = 72;
            this.labFMPTXY.TabStop = true;
            this.labFMPTXY.Text = "富美平台注册协议";
            this.labFMPTXY.VisitedLinkColor = System.Drawing.Color.Goldenrod;
            this.labFMPTXY.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labFMPTXY_LinkClicked);
            // 
            // TipsErrorPwdAgain
            // 
            this.TipsErrorPwdAgain.AutoSize = true;
            this.TipsErrorPwdAgain.ForeColor = System.Drawing.Color.Red;
            this.TipsErrorPwdAgain.Location = new System.Drawing.Point(312, 124);
            this.TipsErrorPwdAgain.Name = "TipsErrorPwdAgain";
            this.TipsErrorPwdAgain.Size = new System.Drawing.Size(125, 12);
            this.TipsErrorPwdAgain.TabIndex = 6;
            this.TipsErrorPwdAgain.Text = "我是再次密码错误提示";
            // 
            // TipsErrorUPwd
            // 
            this.TipsErrorUPwd.AutoSize = true;
            this.TipsErrorUPwd.ForeColor = System.Drawing.Color.Red;
            this.TipsErrorUPwd.Location = new System.Drawing.Point(312, 94);
            this.TipsErrorUPwd.Name = "TipsErrorUPwd";
            this.TipsErrorUPwd.Size = new System.Drawing.Size(101, 12);
            this.TipsErrorUPwd.TabIndex = 7;
            this.TipsErrorUPwd.Text = "我是密码错误提示";
            // 
            // TipsErrorUName
            // 
            this.TipsErrorUName.AutoSize = true;
            this.TipsErrorUName.ForeColor = System.Drawing.Color.Red;
            this.TipsErrorUName.Location = new System.Drawing.Point(312, 62);
            this.TipsErrorUName.Name = "TipsErrorUName";
            this.TipsErrorUName.Size = new System.Drawing.Size(113, 12);
            this.TipsErrorUName.TabIndex = 8;
            this.TipsErrorUName.Text = "我是用户名错误提示";
            // 
            // PBload
            // 
            this.PBload.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBload.Location = new System.Drawing.Point(236, 172);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 2;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // TipsInputPwdAgain
            // 
            this.TipsInputPwdAgain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TipsInputPwdAgain.Cursor = System.Windows.Forms.Cursors.Default;
            this.TipsInputPwdAgain.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.TipsInputPwdAgain.Location = new System.Drawing.Point(310, 122);
            this.TipsInputPwdAgain.Name = "TipsInputPwdAgain";
            this.TipsInputPwdAgain.ReadOnly = true;
            this.TipsInputPwdAgain.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TipsInputPwdAgain.Size = new System.Drawing.Size(228, 20);
            this.TipsInputPwdAgain.TabIndex = 3;
            this.TipsInputPwdAgain.Text = "再次输入密码。";
            // 
            // TipsInputPwd
            // 
            this.TipsInputPwd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TipsInputPwd.Cursor = System.Windows.Forms.Cursors.Default;
            this.TipsInputPwd.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.TipsInputPwd.Location = new System.Drawing.Point(310, 85);
            this.TipsInputPwd.Name = "TipsInputPwd";
            this.TipsInputPwd.ReadOnly = true;
            this.TipsInputPwd.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TipsInputPwd.Size = new System.Drawing.Size(228, 39);
            this.TipsInputPwd.TabIndex = 4;
            this.TipsInputPwd.Text = "密码由6-16位字母、数字组成，区分大小写。";
            // 
            // TipsInputUName
            // 
            this.TipsInputUName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TipsInputUName.Cursor = System.Windows.Forms.Cursors.Default;
            this.TipsInputUName.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.TipsInputUName.Location = new System.Drawing.Point(311, 54);
            this.TipsInputUName.Name = "TipsInputUName";
            this.TipsInputUName.ReadOnly = true;
            this.TipsInputUName.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TipsInputUName.Size = new System.Drawing.Size(228, 39);
            this.TipsInputUName.TabIndex = 5;
            this.TipsInputUName.Text = "用户名由2-16位汉字、字母（不区分大小写）、数字、下划线组成。";
            // 
            // TipsErrorEmail
            // 
            this.TipsErrorEmail.AutoSize = true;
            this.TipsErrorEmail.ForeColor = System.Drawing.Color.Red;
            this.TipsErrorEmail.Location = new System.Drawing.Point(312, 31);
            this.TipsErrorEmail.Name = "TipsErrorEmail";
            this.TipsErrorEmail.Size = new System.Drawing.Size(101, 12);
            this.TipsErrorEmail.TabIndex = 9;
            this.TipsErrorEmail.Text = "我是邮箱错误提示";
            // 
            // TipsInputEmail
            // 
            this.TipsInputEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TipsInputEmail.Cursor = System.Windows.Forms.Cursors.Default;
            this.TipsInputEmail.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.TipsInputEmail.Location = new System.Drawing.Point(312, 26);
            this.TipsInputEmail.Name = "TipsInputEmail";
            this.TipsInputEmail.ReadOnly = true;
            this.TipsInputEmail.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TipsInputEmail.Size = new System.Drawing.Size(228, 39);
            this.TipsInputEmail.TabIndex = 10;
            this.TipsInputEmail.Text = "建议您使用常用邮箱作为安全邮箱，方便账号和密码找回。";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(41, 124);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(11, 12);
            this.label16.TabIndex = 11;
            this.label16.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(63, 93);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(11, 12);
            this.label15.TabIndex = 12;
            this.label15.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(52, 62);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(11, 12);
            this.label14.TabIndex = 13;
            this.label14.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(41, 31);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(11, 12);
            this.label13.TabIndex = 14;
            this.label13.Text = "*";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(50, 124);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "确认密码：";
            // 
            // txtUserPwdAgain
            // 
            this.txtUserPwdAgain.BackColor = System.Drawing.Color.White;
            this.txtUserPwdAgain.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtUserPwdAgain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserPwdAgain.CanPASTE = true;
            this.txtUserPwdAgain.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserPwdAgain.ForeColor = System.Drawing.Color.Black;
            this.txtUserPwdAgain.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtUserPwdAgain.Location = new System.Drawing.Point(121, 119);
            this.txtUserPwdAgain.MaxLength = 16;
            this.txtUserPwdAgain.Name = "txtUserPwdAgain";
            this.txtUserPwdAgain.Size = new System.Drawing.Size(185, 23);
            this.txtUserPwdAgain.TabIndex = 70;
            this.txtUserPwdAgain.TextNtip = "";
            this.txtUserPwdAgain.UseSystemPasswordChar = true;
            this.txtUserPwdAgain.WordWrap = false;
            this.txtUserPwdAgain.Click += new System.EventHandler(this.txtUserPwdAgain_Click);
            this.txtUserPwdAgain.TextChanged += new System.EventHandler(this.txtUserPwdAgain_TextChanged);
            this.txtUserPwdAgain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserPwdAgain_KeyUp);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(72, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 16;
            this.label11.Text = "密码：";
            // 
            // txtUserPwd
            // 
            this.txtUserPwd.BackColor = System.Drawing.Color.White;
            this.txtUserPwd.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtUserPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserPwd.CanPASTE = true;
            this.txtUserPwd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserPwd.ForeColor = System.Drawing.Color.Black;
            this.txtUserPwd.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtUserPwd.Location = new System.Drawing.Point(121, 88);
            this.txtUserPwd.MaxLength = 16;
            this.txtUserPwd.Name = "txtUserPwd";
            this.txtUserPwd.Size = new System.Drawing.Size(185, 23);
            this.txtUserPwd.TabIndex = 69;
            this.txtUserPwd.TextNtip = "";
            this.txtUserPwd.UseSystemPasswordChar = true;
            this.txtUserPwd.WordWrap = false;
            this.txtUserPwd.Click += new System.EventHandler(this.txtUserPwd_Click);
            this.txtUserPwd.TextChanged += new System.EventHandler(this.txtUserPwd_TextChanged);
            this.txtUserPwd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserPwd_KeyUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(61, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "用户名：";
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.White;
            this.txtUserName.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.CanPASTE = true;
            this.txtUserName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserName.ForeColor = System.Drawing.Color.Black;
            this.txtUserName.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtUserName.Location = new System.Drawing.Point(121, 57);
            this.txtUserName.MaxLength = 16;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(185, 23);
            this.txtUserName.TabIndex = 68;
            this.txtUserName.TextNtip = "";
            this.txtUserName.WordWrap = false;
            this.txtUserName.Click += new System.EventHandler(this.txtUserName_Click);
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            this.txtUserName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserName_KeyUp);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(587, 1);
            this.label1.TabIndex = 18;
            this.label1.Text = "label1";
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.Enabled = false;
            this.btnNext.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNext.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnNext.Location = new System.Drawing.Point(121, 182);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(95, 22);
            this.btnNext.TabIndex = 71;
            this.btnNext.Texts = "下一步";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lbUserEmail
            // 
            this.lbUserEmail.AutoSize = true;
            this.lbUserEmail.BackColor = System.Drawing.Color.Transparent;
            this.lbUserEmail.Location = new System.Drawing.Point(50, 31);
            this.lbUserEmail.Name = "lbUserEmail";
            this.lbUserEmail.Size = new System.Drawing.Size(65, 12);
            this.lbUserEmail.TabIndex = 19;
            this.lbUserEmail.Text = "登录邮箱：";
            // 
            // txtUserEmail
            // 
            this.txtUserEmail.BackColor = System.Drawing.Color.White;
            this.txtUserEmail.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtUserEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserEmail.CanPASTE = true;
            this.txtUserEmail.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserEmail.ForeColor = System.Drawing.Color.Black;
            this.txtUserEmail.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtUserEmail.Location = new System.Drawing.Point(121, 26);
            this.txtUserEmail.MaxLength = 40;
            this.txtUserEmail.Name = "txtUserEmail";
            this.txtUserEmail.Size = new System.Drawing.Size(185, 23);
            this.txtUserEmail.TabIndex = 67;
            this.txtUserEmail.TextNtip = "";
            this.txtUserEmail.WordWrap = false;
            this.txtUserEmail.Click += new System.EventHandler(this.txtUserEmail_Click);
            this.txtUserEmail.TextChanged += new System.EventHandler(this.txtUserEmail_TextChanged);
            this.txtUserEmail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserEmail_KeyUp);
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
            this.panel1.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(254, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 1);
            this.label7.TabIndex = 23;
            this.label7.Text = "label7";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(174, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 1);
            this.label6.TabIndex = 24;
            this.label6.Text = "label6";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(312, 11);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(22, 22);
            this.panel4.TabIndex = 25;
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
            this.panel3.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 20);
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
            this.panel2.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 19);
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
            this.label2.TabIndex = 31;
            this.label2.Text = "30秒快速注册";
            // 
            // FormRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(591, 331);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PnlBig);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegister";
            this.ShowColorButton = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中国商品批发交易平台注册";
            this.Activated += new System.EventHandler(this.FormRegister_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRegister_FormClosing);
            this.Load += new System.EventHandler(this.FormRegister_Load);
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
        private SubForm.UCTextBox txtUserEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private SubForm.UCTextBox txtUserPwdAgain;
        private System.Windows.Forms.Label label11;
        private SubForm.UCTextBox txtUserPwd;
        private System.Windows.Forms.Label label10;
        private SubForm.UCTextBox txtUserName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RichTextBox TipsInputEmail;
        private System.Windows.Forms.Label TipsErrorEmail;
        private System.Windows.Forms.Label TipsErrorUName;
        private System.Windows.Forms.Label TipsErrorUPwd;
        private System.Windows.Forms.Label TipsErrorPwdAgain;
        private System.Windows.Forms.RichTextBox TipsInputUName;
        private System.Windows.Forms.RichTextBox TipsInputPwd;
        private System.Windows.Forms.RichTextBox TipsInputPwdAgain;
        private System.Windows.Forms.PictureBox PBload;
        private System.Windows.Forms.LinkLabel labFMPTXY;
        private System.Windows.Forms.CheckBox CB_TYXY;
    }
}