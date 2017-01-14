namespace StressTesting
{
    partial class MainForm
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.rtxtlsjl = new System.Windows.Forms.RichTextBox();
            this.tabLog = new System.Windows.Forms.TabControl();
            this.tabdqzt = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tablsjl = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txttotalNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textGRNum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.texteveNUm = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnbegin = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabLog.SuspendLayout();
            this.tabdqzt.SuspendLayout();
            this.tablsjl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 9);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(102, 26);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "设置数据库连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // rtxtlsjl
            // 
            this.rtxtlsjl.BackColor = System.Drawing.SystemColors.Info;
            this.rtxtlsjl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtlsjl.Location = new System.Drawing.Point(3, 3);
            this.rtxtlsjl.Name = "rtxtlsjl";
            this.rtxtlsjl.Size = new System.Drawing.Size(744, 218);
            this.rtxtlsjl.TabIndex = 1;
            this.rtxtlsjl.Text = "";
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.tabdqzt);
            this.tabLog.Controls.Add(this.tablsjl);
            this.tabLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLog.Location = new System.Drawing.Point(3, 17);
            this.tabLog.Name = "tabLog";
            this.tabLog.SelectedIndex = 0;
            this.tabLog.Size = new System.Drawing.Size(758, 250);
            this.tabLog.TabIndex = 2;
            // 
            // tabdqzt
            // 
            this.tabdqzt.Controls.Add(this.richTextBox1);
            this.tabdqzt.Location = new System.Drawing.Point(4, 22);
            this.tabdqzt.Name = "tabdqzt";
            this.tabdqzt.Padding = new System.Windows.Forms.Padding(3);
            this.tabdqzt.Size = new System.Drawing.Size(750, 224);
            this.tabdqzt.TabIndex = 0;
            this.tabdqzt.Text = "当前状态";
            this.tabdqzt.UseVisualStyleBackColor = true;
            this.tabdqzt.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(744, 218);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // tablsjl
            // 
            this.tablsjl.Controls.Add(this.rtxtlsjl);
            this.tablsjl.Location = new System.Drawing.Point(4, 22);
            this.tablsjl.Name = "tablsjl";
            this.tablsjl.Padding = new System.Windows.Forms.Padding(3);
            this.tablsjl.Size = new System.Drawing.Size(750, 224);
            this.tablsjl.TabIndex = 1;
            this.tablsjl.Text = "历史记录";
            this.tablsjl.UseVisualStyleBackColor = true;
            this.tablsjl.Enter += new System.EventHandler(this.tablsjl_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "计划插入数据条数：";
            // 
            // txttotalNum
            // 
            this.txttotalNum.Location = new System.Drawing.Point(136, 17);
            this.txttotalNum.Name = "txttotalNum";
            this.txttotalNum.Size = new System.Drawing.Size(111, 21);
            this.txttotalNum.TabIndex = 5;
            this.txttotalNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txttotalNum_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "条";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(254, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "组";
            // 
            // textGRNum
            // 
            this.textGRNum.Location = new System.Drawing.Point(136, 44);
            this.textGRNum.Name = "textGRNum";
            this.textGRNum.Size = new System.Drawing.Size(111, 21);
            this.textGRNum.TabIndex = 8;
            this.textGRNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txttotalNum_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "计划并行组数：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(254, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "条";
            // 
            // texteveNUm
            // 
            this.texteveNUm.Location = new System.Drawing.Point(136, 71);
            this.texteveNUm.Name = "texteveNUm";
            this.texteveNUm.Size = new System.Drawing.Size(111, 21);
            this.texteveNUm.TabIndex = 11;
            this.texteveNUm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txttotalNum_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "计划每组插入条数：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnbegin);
            this.groupBox1.Controls.Add(this.textGRNum);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.texteveNUm);
            this.groupBox1.Controls.Add(this.txttotalNum);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(7, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(750, 174);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(272, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(137, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "（开启的并行线程个数）";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(278, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(461, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "每组条数请确保大于2条，否则无法匹配生成投标单,当然最好是偶数，那样才能一对一";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(136, 99);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(111, 21);
            this.textBox1.TabIndex = 16;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txttotalNum_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(40, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "用户起始索引：";
            // 
            // btnbegin
            // 
            this.btnbegin.Location = new System.Drawing.Point(25, 136);
            this.btnbegin.Name = "btnbegin";
            this.btnbegin.Size = new System.Drawing.Size(102, 23);
            this.btnbegin.TabIndex = 14;
            this.btnbegin.Text = "开始压力测试";
            this.btnbegin.UseVisualStyleBackColor = true;
            this.btnbegin.Click += new System.EventHandler(this.btnbegin_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tabLog);
            this.groupBox2.Location = new System.Drawing.Point(0, 247);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(764, 270);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 11);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(682, 23);
            this.progressBar1.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(702, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "0%";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.progressBar1);
            this.groupBox3.Location = new System.Drawing.Point(3, 212);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(754, 39);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 517);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnConnect);
            this.Name = "MainForm";
            this.Text = "平台压力测试软件";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabLog.ResumeLayout(false);
            this.tabdqzt.ResumeLayout(false);
            this.tablsjl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox rtxtlsjl;
        private System.Windows.Forms.TabControl tabLog;
        private System.Windows.Forms.TabPage tabdqzt;
        private System.Windows.Forms.TabPage tablsjl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txttotalNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textGRNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox texteveNUm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnbegin;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
    }
}

