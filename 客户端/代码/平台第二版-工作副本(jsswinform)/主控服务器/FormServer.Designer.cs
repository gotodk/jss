namespace 主控服务器
{
    partial class FormServer
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RTB_errmsg = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtb_cmdlist = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DGVuserlist = new System.Windows.Forms.DataGridView();
            this.B_endserver = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.L_sservermsginfo = new System.Windows.Forms.Label();
            this.B_beginrunserver = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.infofsmb = new System.Windows.Forms.Label();
            this.fsmbb = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVuserlist)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.RTB_errmsg);
            this.groupBox3.Location = new System.Drawing.Point(12, 632);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(988, 86);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "调试信息";
            // 
            // RTB_errmsg
            // 
            this.RTB_errmsg.Location = new System.Drawing.Point(6, 20);
            this.RTB_errmsg.Name = "RTB_errmsg";
            this.RTB_errmsg.Size = new System.Drawing.Size(976, 60);
            this.RTB_errmsg.TabIndex = 2;
            this.RTB_errmsg.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtb_cmdlist);
            this.groupBox2.Location = new System.Drawing.Point(740, 33);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 330);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "服务器日志";
            // 
            // rtb_cmdlist
            // 
            this.rtb_cmdlist.Location = new System.Drawing.Point(6, 15);
            this.rtb_cmdlist.Name = "rtb_cmdlist";
            this.rtb_cmdlist.Size = new System.Drawing.Size(248, 309);
            this.rtb_cmdlist.TabIndex = 6;
            this.rtb_cmdlist.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DGVuserlist);
            this.groupBox1.Location = new System.Drawing.Point(12, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(722, 330);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "在线用户列表";
            // 
            // DGVuserlist
            // 
            this.DGVuserlist.AllowUserToAddRows = false;
            this.DGVuserlist.AllowUserToDeleteRows = false;
            this.DGVuserlist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DGVuserlist.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.DGVuserlist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVuserlist.Location = new System.Drawing.Point(6, 20);
            this.DGVuserlist.MultiSelect = false;
            this.DGVuserlist.Name = "DGVuserlist";
            this.DGVuserlist.ReadOnly = true;
            this.DGVuserlist.RowTemplate.Height = 23;
            this.DGVuserlist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVuserlist.Size = new System.Drawing.Size(710, 304);
            this.DGVuserlist.TabIndex = 0;
            this.DGVuserlist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DGVuserlist_MouseClick);
            // 
            // B_endserver
            // 
            this.B_endserver.Location = new System.Drawing.Point(280, 4);
            this.B_endserver.Name = "B_endserver";
            this.B_endserver.Size = new System.Drawing.Size(75, 23);
            this.B_endserver.TabIndex = 14;
            this.B_endserver.Text = "停止服务";
            this.B_endserver.UseVisualStyleBackColor = true;
            this.B_endserver.Click += new System.EventHandler(this.B_endserver_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "监听端口";
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(71, 6);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(100, 21);
            this.tb_port.TabIndex = 12;
            this.tb_port.Text = "58844";
            // 
            // L_sservermsginfo
            // 
            this.L_sservermsginfo.AutoSize = true;
            this.L_sservermsginfo.Location = new System.Drawing.Point(738, 9);
            this.L_sservermsginfo.Name = "L_sservermsginfo";
            this.L_sservermsginfo.Size = new System.Drawing.Size(71, 12);
            this.L_sservermsginfo.TabIndex = 11;
            this.L_sservermsginfo.Text = "状态跟踪...";
            // 
            // B_beginrunserver
            // 
            this.B_beginrunserver.Location = new System.Drawing.Point(177, 4);
            this.B_beginrunserver.Name = "B_beginrunserver";
            this.B_beginrunserver.Size = new System.Drawing.Size(85, 23);
            this.B_beginrunserver.TabIndex = 10;
            this.B_beginrunserver.Text = "开始服务";
            this.B_beginrunserver.UseVisualStyleBackColor = true;
            this.B_beginrunserver.Click += new System.EventHandler(this.B_beginrunserver_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.infofsmb);
            this.groupBox4.Controls.Add(this.fsmbb);
            this.groupBox4.Location = new System.Drawing.Point(12, 369);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(988, 257);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "发送测试消息";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 46);
            this.textBox1.Name = "textBox1";
            this.textBox1.ShowSelectionMargin = true;
            this.textBox1.Size = new System.Drawing.Size(291, 167);
            this.textBox1.TabIndex = 19;
            this.textBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 228);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "发送";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // infofsmb
            // 
            this.infofsmb.AutoSize = true;
            this.infofsmb.Location = new System.Drawing.Point(78, 20);
            this.infofsmb.Name = "infofsmb";
            this.infofsmb.Size = new System.Drawing.Size(209, 12);
            this.infofsmb.TabIndex = 2;
            this.infofsmb.Text = "..................................";
            // 
            // fsmbb
            // 
            this.fsmbb.AutoSize = true;
            this.fsmbb.Location = new System.Drawing.Point(7, 20);
            this.fsmbb.Name = "fsmbb";
            this.fsmbb.Size = new System.Drawing.Size(65, 12);
            this.fsmbb.TabIndex = 1;
            this.fsmbb.Text = "发送目标：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(573, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "监控调试专用按钮";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.B_endserver);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_port);
            this.Controls.Add(this.L_sservermsginfo);
            this.Controls.Add(this.B_beginrunserver);
            this.Name = "FormServer";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormServer_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVuserlist)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox RTB_errmsg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rtb_cmdlist;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView DGVuserlist;
        private System.Windows.Forms.Button B_endserver;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.Label L_sservermsginfo;
        private System.Windows.Forms.Button B_beginrunserver;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label infofsmb;
        private System.Windows.Forms.Label fsmbb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox textBox1;
        private System.Windows.Forms.Button button2;
    }
}

