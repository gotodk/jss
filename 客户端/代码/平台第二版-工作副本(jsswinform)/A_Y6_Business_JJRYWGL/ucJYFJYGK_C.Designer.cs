namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    partial class ucJYFJYGK_C
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
            this.btnExport = new Com.Seezt.Skins.BasicButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.ucCityList1 = new 客户端主程序.SubForm.UCCityList();
            this.txtJYF = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            this.交易方账号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.交易方名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否当前关联 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.所属区域 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.注册类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.累计卖出金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.累计买入金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.经纪人累计收益 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.累计竞标次数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.累计中标次数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.已发货待付款金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(743, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 22);
            this.btnExport.TabIndex = 127;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(177, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 126;
            this.label3.Text = "所属区域：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 125;
            this.label2.Text = "交易方名称：";
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
            this.交易方账号,
            this.交易方名称,
            this.是否当前关联,
            this.所属区域,
            this.注册类型,
            this.累计卖出金额,
            this.累计买入金额,
            this.经纪人累计收益,
            this.累计竞标次数,
            this.累计中标次数,
            this.已发货待付款金额});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(10, 35);
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
            this.dataGridView1.Size = new System.Drawing.Size(1010, 492);
            this.dataGridView1.TabIndex = 121;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(683, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 123;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click_1);
            // 
            // ucCityList1
            // 
            this.ucCityList1.EnabledItem = new bool[] {
        true,
        true,
        true};
            this.ucCityList1.Location = new System.Drawing.Point(240, 6);
            this.ucCityList1.Name = "ucCityList1";
            this.ucCityList1.SelectedItem = new string[] {
        "",
        "",
        ""};
            this.ucCityList1.Size = new System.Drawing.Size(439, 23);
            this.ucCityList1.TabIndex = 130;
            this.ucCityList1.VisibleItem = new bool[] {
        true,
        true,
        true};
            // 
            // txtJYF
            // 
            this.txtJYF.BackColor = System.Drawing.Color.White;
            this.txtJYF.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtJYF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJYF.CanPASTE = true;
            this.txtJYF.Counts = 2;
            this.txtJYF.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJYF.ForeColor = System.Drawing.Color.Black;
            this.txtJYF.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtJYF.Location = new System.Drawing.Point(88, 5);
            this.txtJYF.Name = "txtJYF";
            this.txtJYF.Size = new System.Drawing.Size(82, 23);
            this.txtJYF.TabIndex = 128;
            this.txtJYF.TextNtip = "";
            this.txtJYF.WordWrap = false;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(10, 531);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(1010, 29);
            this.ucPager1.TabIndex = 122;
            // 
            // 交易方账号
            // 
            this.交易方账号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.交易方账号.DataPropertyName = "交易方账号";
            this.交易方账号.HeaderText = "交易方账号";
            this.交易方账号.Name = "交易方账号";
            this.交易方账号.ReadOnly = true;
            this.交易方账号.Width = 95;
            // 
            // 交易方名称
            // 
            this.交易方名称.DataPropertyName = "交易方名称";
            this.交易方名称.HeaderText = "交易方名称";
            this.交易方名称.Name = "交易方名称";
            this.交易方名称.ReadOnly = true;
            this.交易方名称.Width = 160;
            // 
            // 是否当前关联
            // 
            this.是否当前关联.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.是否当前关联.DataPropertyName = "是否当前关联";
            this.是否当前关联.HeaderText = "是否当前关联";
            this.是否当前关联.Name = "是否当前关联";
            this.是否当前关联.ReadOnly = true;
            this.是否当前关联.Width = 107;
            // 
            // 所属区域
            // 
            this.所属区域.DataPropertyName = "所属区域";
            this.所属区域.HeaderText = "所属区域";
            this.所属区域.Name = "所属区域";
            this.所属区域.ReadOnly = true;
            this.所属区域.Width = 160;
            // 
            // 注册类型
            // 
            this.注册类型.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.注册类型.DataPropertyName = "注册类型";
            this.注册类型.HeaderText = "注册类型";
            this.注册类型.Name = "注册类型";
            this.注册类型.ReadOnly = true;
            this.注册类型.Width = 83;
            // 
            // 累计卖出金额
            // 
            this.累计卖出金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.累计卖出金额.DataPropertyName = "累计卖出金额";
            this.累计卖出金额.HeaderText = "累计卖出金额";
            this.累计卖出金额.Name = "累计卖出金额";
            this.累计卖出金额.ReadOnly = true;
            this.累计卖出金额.Width = 107;
            // 
            // 累计买入金额
            // 
            this.累计买入金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.累计买入金额.DataPropertyName = "累计买入金额";
            this.累计买入金额.HeaderText = "累计买入金额";
            this.累计买入金额.Name = "累计买入金额";
            this.累计买入金额.ReadOnly = true;
            this.累计买入金额.Width = 107;
            // 
            // 经纪人累计收益
            // 
            this.经纪人累计收益.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.经纪人累计收益.DataPropertyName = "经纪人累计收益";
            this.经纪人累计收益.HeaderText = "经纪人累计收益";
            this.经纪人累计收益.Name = "经纪人累计收益";
            this.经纪人累计收益.ReadOnly = true;
            this.经纪人累计收益.Width = 119;
            // 
            // 累计竞标次数
            // 
            this.累计竞标次数.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.累计竞标次数.DataPropertyName = "累计竞标次数";
            this.累计竞标次数.HeaderText = "累计竞标次数";
            this.累计竞标次数.Name = "累计竞标次数";
            this.累计竞标次数.ReadOnly = true;
            this.累计竞标次数.Width = 107;
            // 
            // 累计中标次数
            // 
            this.累计中标次数.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.累计中标次数.DataPropertyName = "累计中标次数";
            this.累计中标次数.HeaderText = "累计中标次数";
            this.累计中标次数.Name = "累计中标次数";
            this.累计中标次数.ReadOnly = true;
            this.累计中标次数.Width = 107;
            // 
            // 已发货待付款金额
            // 
            this.已发货待付款金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.已发货待付款金额.DataPropertyName = "已发货待付款金额";
            this.已发货待付款金额.HeaderText = "已发货待付款金额";
            this.已发货待付款金额.Name = "已发货待付款金额";
            this.已发货待付款金额.ReadOnly = true;
            this.已发货待付款金额.Width = 131;
            // 
            // ucJYFJYGK_C
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.ucCityList1);
            this.Controls.Add(this.txtJYF);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucJYFJYGK_C";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucJYFJYGK_C_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCTextBox txtJYF;
        private Com.Seezt.Skins.BasicButton btnExport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private UCCityList ucCityList1;
        private CMyXls cMyXls1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交易方账号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交易方名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 是否当前关联;
        private System.Windows.Forms.DataGridViewTextBoxColumn 所属区域;
        private System.Windows.Forms.DataGridViewTextBoxColumn 注册类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 累计卖出金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 累计买入金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 经纪人累计收益;
        private System.Windows.Forms.DataGridViewTextBoxColumn 累计竞标次数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 累计中标次数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 已发货待付款金额;
    }
}
