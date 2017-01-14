namespace 客户端主程序.SubForm.CenterForm.Buyer
{
    partial class CXshoukuanzhanghu
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer_SCCK = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.添加时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.收款人姓名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.开户银行名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.银行卡号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.默认收款账号 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.修改 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.删除 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.文件路径 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.panel1 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.B_SCSC = new System.Windows.Forms.Label();
            this.B_SC = new System.Windows.Forms.Label();
            this.B_SCCK = new System.Windows.Forms.Label();
            this.lbNumber = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.bdfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yclj = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wjm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.jsnumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scjg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNumber = new System.Windows.Forms.Label();
            this.ResetB = new Com.Seezt.Skins.BasicButton();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSave = new Com.Seezt.Skins.BasicButton();
            this.label11 = new System.Windows.Forms.Label();
            this.ucTextBoxYHZK = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.ucTextBoxSFZ = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ucTextBoxSKR = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ucTextBoxKHYH = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.PBload = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelUC2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "图片|*.jpg;*.gif;*.png|其他|*.*";
            this.openFileDialog1.Title = "选择要上传的文件";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // timer_SCCK
            // 
            this.timer_SCCK.Enabled = true;
            this.timer_SCCK.Interval = 500;
            this.timer_SCCK.Tick += new System.EventHandler(this.timer_SCCK_Tick);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.ucPager1);
            this.panel2.Location = new System.Drawing.Point(0, 250);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.SetShowBorder = 1;
            this.panel2.Size = new System.Drawing.Size(979, 183);
            this.panel2.TabIndex = 64;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(20, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 14);
            this.label4.TabIndex = 60;
            this.label4.Text = "已保存的收款账号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(137, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 14);
            this.label1.TabIndex = 61;
            this.label1.Text = "（最多设置5个账号）";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.添加时间,
            this.收款人姓名,
            this.开户银行名称,
            this.银行卡号,
            this.默认收款账号,
            this.修改,
            this.删除,
            this.编号,
            this.文件路径});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(1, 36);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(939, 96);
            this.dataGridView1.TabIndex = 52;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // 添加时间
            // 
            this.添加时间.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.添加时间.DataPropertyName = "添加时间";
            this.添加时间.FillWeight = 120F;
            this.添加时间.HeaderText = "添加时间";
            this.添加时间.Name = "添加时间";
            this.添加时间.ReadOnly = true;
            this.添加时间.Width = 83;
            // 
            // 收款人姓名
            // 
            this.收款人姓名.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.收款人姓名.DataPropertyName = "收款人姓名";
            this.收款人姓名.HeaderText = "收款人姓名";
            this.收款人姓名.Name = "收款人姓名";
            this.收款人姓名.ReadOnly = true;
            this.收款人姓名.Width = 95;
            // 
            // 开户银行名称
            // 
            this.开户银行名称.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.开户银行名称.DataPropertyName = "开户银行名称";
            this.开户银行名称.FillWeight = 200F;
            this.开户银行名称.HeaderText = "开户银行名称";
            this.开户银行名称.Name = "开户银行名称";
            this.开户银行名称.ReadOnly = true;
            this.开户银行名称.Width = 107;
            // 
            // 银行卡号
            // 
            this.银行卡号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.银行卡号.DataPropertyName = "银行卡号";
            this.银行卡号.FillWeight = 200F;
            this.银行卡号.HeaderText = "银行卡号";
            this.银行卡号.Name = "银行卡号";
            this.银行卡号.ReadOnly = true;
            this.银行卡号.Width = 83;
            // 
            // 默认收款账号
            // 
            this.默认收款账号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.默认收款账号.DataPropertyName = "默认收款账号";
            this.默认收款账号.FillWeight = 120F;
            this.默认收款账号.HeaderText = "默认收款账号";
            this.默认收款账号.LinkColor = System.Drawing.Color.RoyalBlue;
            this.默认收款账号.Name = "默认收款账号";
            this.默认收款账号.ReadOnly = true;
            this.默认收款账号.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.默认收款账号.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.默认收款账号.Width = 88;
            // 
            // 修改
            // 
            this.修改.FillWeight = 50F;
            this.修改.HeaderText = "修改";
            this.修改.LinkColor = System.Drawing.Color.RoyalBlue;
            this.修改.Name = "修改";
            this.修改.ReadOnly = true;
            this.修改.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.修改.Text = "修改";
            this.修改.UseColumnTextForLinkValue = true;
            this.修改.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.修改.Width = 50;
            // 
            // 删除
            // 
            this.删除.FillWeight = 50F;
            this.删除.HeaderText = "删除";
            this.删除.LinkColor = System.Drawing.Color.RoyalBlue;
            this.删除.Name = "删除";
            this.删除.ReadOnly = true;
            this.删除.Text = "删除";
            this.删除.UseColumnTextForLinkValue = true;
            this.删除.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.删除.Width = 50;
            // 
            // 编号
            // 
            this.编号.DataPropertyName = "Number";
            this.编号.HeaderText = "编号";
            this.编号.Name = "编号";
            this.编号.ReadOnly = true;
            this.编号.Visible = false;
            // 
            // 文件路径
            // 
            this.文件路径.DataPropertyName = "文件路径";
            this.文件路径.HeaderText = "文件路径";
            this.文件路径.Name = "文件路径";
            this.文件路径.ReadOnly = true;
            this.文件路径.Visible = false;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(7, 142);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(939, 30);
            this.ucPager1.TabIndex = 53;
            this.ucPager1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panel1.Controls.Add(this.panelUC2);
            this.panel1.Controls.Add(this.lbNumber);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.lblNumber);
            this.panel1.Controls.Add(this.ResetB);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.ucTextBoxYHZK);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.ucTextBoxSFZ);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.ucTextBoxSKR);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.ucTextBoxKHYH);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.SetShowBorder = 1;
            this.panel1.Size = new System.Drawing.Size(979, 236);
            this.panel1.TabIndex = 60;
            // 
            // panelUC2
            // 
            this.panelUC2.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC2.Controls.Add(this.B_SCSC);
            this.panelUC2.Controls.Add(this.B_SC);
            this.panelUC2.Controls.Add(this.B_SCCK);
            this.panelUC2.Location = new System.Drawing.Point(156, 133);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.SetShowBorder = 0;
            this.panelUC2.Size = new System.Drawing.Size(189, 28);
            this.panelUC2.TabIndex = 65;
            // 
            // B_SCSC
            // 
            this.B_SCSC.BackColor = System.Drawing.Color.PowderBlue;
            this.B_SCSC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.B_SCSC.ForeColor = System.Drawing.Color.Black;
            this.B_SCSC.Location = new System.Drawing.Point(122, 4);
            this.B_SCSC.Name = "B_SCSC";
            this.B_SCSC.Size = new System.Drawing.Size(50, 20);
            this.B_SCSC.TabIndex = 32;
            this.B_SCSC.Text = "删除";
            this.B_SCSC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.B_SCSC.Visible = false;
            this.B_SCSC.Click += new System.EventHandler(this.B_SCSC_Click);
            // 
            // B_SC
            // 
            this.B_SC.BackColor = System.Drawing.Color.PowderBlue;
            this.B_SC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.B_SC.ForeColor = System.Drawing.Color.Black;
            this.B_SC.Location = new System.Drawing.Point(5, 4);
            this.B_SC.Name = "B_SC";
            this.B_SC.Size = new System.Drawing.Size(50, 20);
            this.B_SC.TabIndex = 30;
            this.B_SC.Text = "上传";
            this.B_SC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.B_SC.Click += new System.EventHandler(this.B_SC_Click);
            // 
            // B_SCCK
            // 
            this.B_SCCK.BackColor = System.Drawing.Color.PowderBlue;
            this.B_SCCK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.B_SCCK.ForeColor = System.Drawing.Color.Black;
            this.B_SCCK.Location = new System.Drawing.Point(64, 4);
            this.B_SCCK.Name = "B_SCCK";
            this.B_SCCK.Size = new System.Drawing.Size(50, 20);
            this.B_SCCK.TabIndex = 31;
            this.B_SCCK.Text = "查看";
            this.B_SCCK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.B_SCCK.Visible = false;
            this.B_SCCK.Click += new System.EventHandler(this.B_SCCK_Click);
            // 
            // lbNumber
            // 
            this.lbNumber.AutoSize = true;
            this.lbNumber.Location = new System.Drawing.Point(641, 154);
            this.lbNumber.Name = "lbNumber";
            this.lbNumber.Size = new System.Drawing.Size(0, 12);
            this.lbNumber.TabIndex = 64;
            this.lbNumber.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(20, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(135, 14);
            this.label13.TabIndex = 61;
            this.label13.Text = "新增/修改收款账号";
            // 
            // listView1
            // 
            this.listView1.AutoArrange = false;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.bdfile,
            this.yclj,
            this.wjm,
            this.jsnumber,
            this.scjg});
            this.listView1.ForeColor = System.Drawing.Color.Black;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(464, 40);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(390, 85);
            this.listView1.TabIndex = 59;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Visible = false;
            // 
            // bdfile
            // 
            this.bdfile.Text = "本地路径";
            this.bdfile.Width = 70;
            // 
            // yclj
            // 
            this.yclj.Text = "远程路径";
            this.yclj.Width = 78;
            // 
            // wjm
            // 
            this.wjm.Text = "原文件名";
            this.wjm.Width = 69;
            // 
            // jsnumber
            // 
            this.jsnumber.Text = "角色编号";
            this.jsnumber.Width = 87;
            // 
            // scjg
            // 
            this.scjg.Text = "上传结果";
            // 
            // lblNumber
            // 
            this.lblNumber.ForeColor = System.Drawing.Color.Black;
            this.lblNumber.Location = new System.Drawing.Point(275, 224);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(125, 23);
            this.lblNumber.TabIndex = 63;
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblNumber.Visible = false;
            // 
            // ResetB
            // 
            this.ResetB.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ResetB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ResetB.ForeColor = System.Drawing.Color.DarkBlue;
            this.ResetB.Location = new System.Drawing.Point(255, 203);
            this.ResetB.Name = "ResetB";
            this.ResetB.Size = new System.Drawing.Size(50, 22);
            this.ResetB.TabIndex = 57;
            this.ResetB.Texts = "取消";
            this.ResetB.Click += new System.EventHandler(this.ResetB_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(80, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 12);
            this.label8.TabIndex = 46;
            this.label8.Text = "*";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSave.Location = new System.Drawing.Point(160, 203);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(50, 22);
            this.btnSave.TabIndex = 51;
            this.btnSave.Texts = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(68, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 47;
            this.label11.Text = "身份证扫描件：";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ucTextBoxYHZK
            // 
            this.ucTextBoxYHZK.BackColor = System.Drawing.Color.White;
            this.ucTextBoxYHZK.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBoxYHZK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBoxYHZK.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBoxYHZK.ForeColor = System.Drawing.Color.Black;
            this.ucTextBoxYHZK.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBoxYHZK.Location = new System.Drawing.Point(160, 103);
            this.ucTextBoxYHZK.Name = "ucTextBoxYHZK";
            this.ucTextBoxYHZK.Size = new System.Drawing.Size(258, 23);
            this.ucTextBoxYHZK.TabIndex = 45;
            this.ucTextBoxYHZK.WordWrap = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.Color.Black;
            this.checkBox1.Location = new System.Drawing.Point(160, 170);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 16);
            this.checkBox1.TabIndex = 50;
            this.checkBox1.Text = "设为默认收款账号";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ucTextBoxSFZ
            // 
            this.ucTextBoxSFZ.BackColor = System.Drawing.Color.White;
            this.ucTextBoxSFZ.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBoxSFZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBoxSFZ.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBoxSFZ.ForeColor = System.Drawing.Color.Black;
            this.ucTextBoxSFZ.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBoxSFZ.Location = new System.Drawing.Point(570, 133);
            this.ucTextBoxSFZ.Name = "ucTextBoxSFZ";
            this.ucTextBoxSFZ.Size = new System.Drawing.Size(258, 23);
            this.ucTextBoxSFZ.TabIndex = 48;
            this.ucTextBoxSFZ.Visible = false;
            this.ucTextBoxSFZ.WordWrap = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(92, 109);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 44;
            this.label9.Text = "银行账号：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(80, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "收款人姓名：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(56, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 12);
            this.label10.TabIndex = 49;
            this.label10.Text = "*";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ucTextBoxSKR
            // 
            this.ucTextBoxSKR.BackColor = System.Drawing.Color.White;
            this.ucTextBoxSKR.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBoxSKR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBoxSKR.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBoxSKR.ForeColor = System.Drawing.Color.Black;
            this.ucTextBoxSKR.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBoxSKR.Location = new System.Drawing.Point(160, 42);
            this.ucTextBoxSKR.Name = "ucTextBoxSKR";
            this.ucTextBoxSKR.Size = new System.Drawing.Size(258, 23);
            this.ucTextBoxSKR.TabIndex = 38;
            this.ucTextBoxSKR.WordWrap = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(56, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 43;
            this.label6.Text = "*";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(67, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 40;
            this.label5.Text = "*";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ucTextBoxKHYH
            // 
            this.ucTextBoxKHYH.BackColor = System.Drawing.Color.White;
            this.ucTextBoxKHYH.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBoxKHYH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBoxKHYH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBoxKHYH.ForeColor = System.Drawing.Color.Black;
            this.ucTextBoxKHYH.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBoxKHYH.Location = new System.Drawing.Point(160, 73);
            this.ucTextBoxKHYH.Name = "ucTextBoxKHYH";
            this.ucTextBoxKHYH.Size = new System.Drawing.Size(258, 23);
            this.ucTextBoxKHYH.TabIndex = 42;
            this.ucTextBoxKHYH.WordWrap = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(68, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 41;
            this.label7.Text = "开户银行名称：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PBload
            // 
            this.PBload.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBload.Location = new System.Drawing.Point(914, 14);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 58;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // CXshoukuanzhanghu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.PBload);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CXshoukuanzhanghu";
            this.Size = new System.Drawing.Size(979, 505);
            this.Load += new System.EventHandler(this.CXshoukuanzhanghu_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelUC2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCTextBox ucTextBoxSKR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private UCTextBox ucTextBoxKHYH;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private UCTextBox ucTextBoxYHZK;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private UCTextBox ucTextBoxSFZ;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBox1;
        private UCPager ucPager1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Com.Seezt.Skins.BasicButton btnSave;
        private System.Windows.Forms.PictureBox PBload;
        private Com.Seezt.Skins.BasicButton ResetB;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer_SCCK;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader bdfile;
        private System.Windows.Forms.ColumnHeader yclj;
        private System.Windows.Forms.ColumnHeader wjm;
        private System.Windows.Forms.ColumnHeader jsnumber;
        private System.Windows.Forms.ColumnHeader scjg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private publicUC.PanelUC panel1;
        private System.Windows.Forms.Label lblNumber;
        private publicUC.PanelUC panel2;
        private System.Windows.Forms.Label lbNumber;
        private publicUC.PanelUC panelUC2;
        private System.Windows.Forms.Label B_SCSC;
        private System.Windows.Forms.Label B_SC;
        private System.Windows.Forms.Label B_SCCK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 添加时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 收款人姓名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 开户银行名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 银行卡号;
        private System.Windows.Forms.DataGridViewLinkColumn 默认收款账号;
        private System.Windows.Forms.DataGridViewLinkColumn 修改;
        private System.Windows.Forms.DataGridViewLinkColumn 删除;
        private System.Windows.Forms.DataGridViewTextBoxColumn 编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 文件路径;
    }
}
