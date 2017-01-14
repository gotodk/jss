namespace 客户端主程序.SubForm.CenterForm.Agent
{
    partial class UserListLook
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
            this.panelUC1 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.TBkey = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerBegin = new System.Windows.Forms.DateTimePicker();
            this.basicButton1 = new Com.Seezt.Skins.BasicButton();
            this.CBzcjs = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.PBload = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.XYHSH = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Lselect = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.xuanze = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.注册角色 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.注册邮箱 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.买家角色编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.卖家角色编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.panelUC1.SuspendLayout();
            this.panelUC2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelUC1
            // 
            this.panelUC1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC1.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC1.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC1.Controls.Add(this.TBkey);
            this.panelUC1.Controls.Add(this.dateTimePickerEnd);
            this.panelUC1.Controls.Add(this.dateTimePickerBegin);
            this.panelUC1.Controls.Add(this.basicButton1);
            this.panelUC1.Controls.Add(this.CBzcjs);
            this.panelUC1.Controls.Add(this.label1);
            this.panelUC1.Controls.Add(this.label2);
            this.panelUC1.Location = new System.Drawing.Point(0, 0);
            this.panelUC1.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC1.Name = "panelUC1";
            this.panelUC1.SetShowBorder = 1;
            this.panelUC1.Size = new System.Drawing.Size(979, 40);
            this.panelUC1.TabIndex = 18;
            // 
            // TBkey
            // 
            this.TBkey.BackColor = System.Drawing.Color.White;
            this.TBkey.BorderColor = System.Drawing.Color.Gray;
            this.TBkey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TBkey.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TBkey.ForeColor = System.Drawing.Color.Black;
            this.TBkey.HotColor = System.Drawing.Color.CornflowerBlue;
            this.TBkey.Location = new System.Drawing.Point(526, 8);
            this.TBkey.MaxLength = 500;
            this.TBkey.Name = "TBkey";
            this.TBkey.Size = new System.Drawing.Size(131, 22);
            this.TBkey.TabIndex = 23;
            this.TBkey.Text = "用户名、注册邮箱";
            this.TBkey.TextNtip = "";
            this.TBkey.WordWrap = false;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CalendarForeColor = System.Drawing.Color.Black;
            this.dateTimePickerEnd.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dateTimePickerEnd.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dateTimePickerEnd.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dateTimePickerEnd.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(244, 10);
            this.dateTimePickerEnd.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dateTimePickerEnd.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(131, 21);
            this.dateTimePickerEnd.TabIndex = 21;
            // 
            // dateTimePickerBegin
            // 
            this.dateTimePickerBegin.CalendarForeColor = System.Drawing.Color.Black;
            this.dateTimePickerBegin.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dateTimePickerBegin.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dateTimePickerBegin.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dateTimePickerBegin.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dateTimePickerBegin.Location = new System.Drawing.Point(82, 11);
            this.dateTimePickerBegin.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dateTimePickerBegin.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dateTimePickerBegin.Name = "dateTimePickerBegin";
            this.dateTimePickerBegin.Size = new System.Drawing.Size(131, 21);
            this.dateTimePickerBegin.TabIndex = 20;
            // 
            // basicButton1
            // 
            this.basicButton1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton1.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton1.Location = new System.Drawing.Point(674, 9);
            this.basicButton1.Name = "basicButton1";
            this.basicButton1.Size = new System.Drawing.Size(50, 22);
            this.basicButton1.TabIndex = 18;
            this.basicButton1.Texts = "查询";
            this.basicButton1.Click += new System.EventHandler(this.basicButton1_Click);
            // 
            // CBzcjs
            // 
            this.CBzcjs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CBzcjs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBzcjs.FormattingEnabled = true;
            this.CBzcjs.Items.AddRange(new object[] {
            "不限注册角色",
            "卖家账户",
            "买家账户"});
            this.CBzcjs.Location = new System.Drawing.Point(387, 10);
            this.CBzcjs.Name = "CBzcjs";
            this.CBzcjs.Size = new System.Drawing.Size(121, 22);
            this.CBzcjs.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始日期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(219, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "至";
            // 
            // panelUC2
            // 
            this.panelUC2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC2.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC2.Controls.Add(this.PBload);
            this.panelUC2.Controls.Add(this.label6);
            this.panelUC2.Controls.Add(this.XYHSH);
            this.panelUC2.Controls.Add(this.label3);
            this.panelUC2.Controls.Add(this.label4);
            this.panelUC2.Controls.Add(this.Lselect);
            this.panelUC2.Controls.Add(this.dataGridView1);
            this.panelUC2.Controls.Add(this.ucPager1);
            this.panelUC2.Location = new System.Drawing.Point(0, 52);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.Padding = new System.Windows.Forms.Padding(1);
            this.panelUC2.SetShowBorder = 1;
            this.panelUC2.Size = new System.Drawing.Size(979, 389);
            this.panelUC2.TabIndex = 4;
            // 
            // PBload
            // 
            this.PBload.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBload.Location = new System.Drawing.Point(720, 4);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 49;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            this.PBload.WaitOnLoad = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(4, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 37;
            this.label6.Text = "选中用户业务管理：";
            // 
            // XYHSH
            // 
            this.XYHSH.BackColor = System.Drawing.Color.PowderBlue;
            this.XYHSH.Cursor = System.Windows.Forms.Cursors.Hand;
            this.XYHSH.ForeColor = System.Drawing.Color.Black;
            this.XYHSH.Location = new System.Drawing.Point(586, 11);
            this.XYHSH.Name = "XYHSH";
            this.XYHSH.Size = new System.Drawing.Size(114, 20);
            this.XYHSH.TabIndex = 36;
            this.XYHSH.Text = "暂停新用户的审核";
            this.XYHSH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.XYHSH.Visible = false;
            this.XYHSH.Click += new System.EventHandler(this.label5_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(504, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "新用户管理：";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.PowderBlue;
            this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(306, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 20);
            this.label4.TabIndex = 35;
            this.label4.Text = "恢复接受选中用户的新业务";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Lselect
            // 
            this.Lselect.BackColor = System.Drawing.Color.PowderBlue;
            this.Lselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Lselect.ForeColor = System.Drawing.Color.Black;
            this.Lselect.Location = new System.Drawing.Point(119, 11);
            this.Lselect.Name = "Lselect";
            this.Lselect.Size = new System.Drawing.Size(170, 20);
            this.Lselect.TabIndex = 34;
            this.Lselect.Text = "暂停接受选中用户的新业务";
            this.Lselect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Lselect.Click += new System.EventHandler(this.Lselect_Click);
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
            this.xuanze,
            this.Column10,
            this.注册角色,
            this.Column2,
            this.注册邮箱,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.买家角色编号,
            this.卖家角色编号});
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
            this.dataGridView1.Location = new System.Drawing.Point(1, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(977, 313);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // xuanze
            // 
            this.xuanze.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.xuanze.DataPropertyName = "选中状态";
            this.xuanze.FalseValue = "否";
            this.xuanze.HeaderText = "选择";
            this.xuanze.IndeterminateValue = "";
            this.xuanze.Name = "xuanze";
            this.xuanze.TrueValue = "是";
            this.xuanze.Width = 40;
            // 
            // Column10
            // 
            this.Column10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column10.HeaderText = "查看详情";
            this.Column10.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.Column10.LinkColor = System.Drawing.Color.RoyalBlue;
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Text = "查看详情";
            this.Column10.UseColumnTextForLinkValue = true;
            this.Column10.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.Column10.Width = 64;
            // 
            // 注册角色
            // 
            this.注册角色.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.注册角色.DataPropertyName = "注册角色";
            this.注册角色.HeaderText = "注册角色";
            this.注册角色.Name = "注册角色";
            this.注册角色.ReadOnly = true;
            this.注册角色.Width = 83;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.DataPropertyName = "用户名";
            this.Column2.HeaderText = "用户名";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 71;
            // 
            // 注册邮箱
            // 
            this.注册邮箱.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.注册邮箱.DataPropertyName = "注册邮箱";
            this.注册邮箱.HeaderText = "注册邮箱";
            this.注册邮箱.Name = "注册邮箱";
            this.注册邮箱.ReadOnly = true;
            this.注册邮箱.Width = 83;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column4.DataPropertyName = "联系人姓名";
            this.Column4.HeaderText = "联系人姓名";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 95;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column5.DataPropertyName = "联系人手机号";
            this.Column5.HeaderText = "联系人手机号";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 107;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column6.DataPropertyName = "注册时间";
            this.Column6.HeaderText = "注册时间";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 83;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column7.DataPropertyName = "是否休眠";
            this.Column7.HeaderText = "是否休眠";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 83;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column8.DataPropertyName = "是否暂停新业务";
            this.Column8.HeaderText = "是否暂停新业务";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 119;
            // 
            // 买家角色编号
            // 
            this.买家角色编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.买家角色编号.DataPropertyName = "买家角色编号";
            this.买家角色编号.HeaderText = "买家角色编号";
            this.买家角色编号.Name = "买家角色编号";
            this.买家角色编号.Visible = false;
            this.买家角色编号.Width = 108;
            // 
            // 卖家角色编号
            // 
            this.卖家角色编号.DataPropertyName = "卖家角色编号";
            this.卖家角色编号.HeaderText = "卖家角色编号";
            this.卖家角色编号.Name = "卖家角色编号";
            this.卖家角色编号.Visible = false;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(3, 358);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(975, 30);
            this.ucPager1.TabIndex = 4;
            // 
            // UserListLook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUC1);
            this.Controls.Add(this.panelUC2);
            this.Name = "UserListLook";
            this.Size = new System.Drawing.Size(979, 441);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.panelUC1.ResumeLayout(false);
            this.panelUC1.PerformLayout();
            this.panelUC2.ResumeLayout(false);
            this.panelUC2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private publicUC.PanelUC panelUC2;
        private UCPager ucPager1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label3;
        private publicUC.PanelUC panelUC1;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerBegin;
        private Com.Seezt.Skins.BasicButton basicButton1;
        private System.Windows.Forms.ComboBox CBzcjs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Lselect;
        private System.Windows.Forms.Label XYHSH;
        private System.Windows.Forms.Label label6;
        private UCTextBox TBkey;
        private System.Windows.Forms.PictureBox PBload;
        private System.Windows.Forms.DataGridViewCheckBoxColumn xuanze;
        private System.Windows.Forms.DataGridViewLinkColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn 注册角色;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 注册邮箱;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn 买家角色编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 卖家角色编号;



    }
}
