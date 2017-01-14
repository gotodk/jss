namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    partial class ucJJFJBZL_C
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
            this.ucCityList1 = new 客户端主程序.SubForm.UCCityList();
            this.txtJYFMC = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.btnExport = new Com.Seezt.Skins.BasicButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column11 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否当前关联 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.卖家角色编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.交易方名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.关联表Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cbxFSJL = new System.Windows.Forms.ComboBox();
            this.cbxSFDQGL = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ucCityList1
            // 
            this.ucCityList1.EnabledItem = new bool[] {
        true,
        true,
        true};
            this.ucCityList1.Location = new System.Drawing.Point(396, 6);
            this.ucCityList1.Name = "ucCityList1";
            this.ucCityList1.SelectedItem = new string[] {
        "",
        "",
        ""};
            this.ucCityList1.Size = new System.Drawing.Size(439, 23);
            this.ucCityList1.TabIndex = 138;
            this.ucCityList1.VisibleItem = new bool[] {
        true,
        true,
        true};
            // 
            // txtJYFMC
            // 
            this.txtJYFMC.BackColor = System.Drawing.Color.White;
            this.txtJYFMC.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtJYFMC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJYFMC.CanPASTE = true;
            this.txtJYFMC.Counts = 2;
            this.txtJYFMC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJYFMC.ForeColor = System.Drawing.Color.Black;
            this.txtJYFMC.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtJYFMC.Location = new System.Drawing.Point(84, 5);
            this.txtJYFMC.Name = "txtJYFMC";
            this.txtJYFMC.Size = new System.Drawing.Size(86, 23);
            this.txtJYFMC.TabIndex = 137;
            this.txtJYFMC.TextNtip = "";
            this.txtJYFMC.WordWrap = false;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(1076, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 22);
            this.btnExport.TabIndex = 136;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(335, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 135;
            this.label3.Text = "所属区域：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(11, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 134;
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
            this.Column11,
            this.Column1,
            this.Column2,
            this.是否当前关联,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column10,
            this.Column13,
            this.卖家角色编号,
            this.交易方名称,
            this.关联表Number});
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
            this.dataGridView1.Size = new System.Drawing.Size(1133, 492);
            this.dataGridView1.TabIndex = 131;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // Column11
            // 
            this.Column11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column11.HeaderText = "资料详情";
            this.Column11.LinkColor = System.Drawing.Color.RoyalBlue;
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Text = "资料详情";
            this.Column11.UseColumnTextForLinkValue = true;
            this.Column11.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.Column11.Width = 64;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column1.DataPropertyName = "交易方账号";
            this.Column1.HeaderText = "交易方账号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 95;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "交易方名称";
            this.Column2.HeaderText = "交易方名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 160;
            // 
            // 是否当前关联
            // 
            this.是否当前关联.DataPropertyName = "是否当前关联";
            this.是否当前关联.HeaderText = "是否当前关联";
            this.是否当前关联.Name = "是否当前关联";
            this.是否当前关联.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "所属区域";
            this.Column3.HeaderText = "所属区域";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 160;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column4.DataPropertyName = "注册类型";
            this.Column4.HeaderText = "注册类型";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 83;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column5.DataPropertyName = "联系人";
            this.Column5.HeaderText = "联系人";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 71;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column6.DataPropertyName = "联系电话";
            this.Column6.HeaderText = "联系电话";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 83;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column7.DataPropertyName = "初审记录";
            this.Column7.HeaderText = "初审记录";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 83;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column8.DataPropertyName = "复审记录";
            this.Column8.HeaderText = "复审记录";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 83;
            // 
            // Column10
            // 
            this.Column10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column10.DataPropertyName = "暂停新业务时间";
            this.Column10.HeaderText = "暂停新业务时间";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 119;
            // 
            // Column13
            // 
            this.Column13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column13.DataPropertyName = "账户状态";
            this.Column13.HeaderText = "账户状态";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.Width = 83;
            // 
            // 卖家角色编号
            // 
            this.卖家角色编号.DataPropertyName = "卖家角色编号";
            this.卖家角色编号.HeaderText = "卖家角色编号";
            this.卖家角色编号.Name = "卖家角色编号";
            this.卖家角色编号.ReadOnly = true;
            this.卖家角色编号.Visible = false;
            // 
            // 交易方名称
            // 
            this.交易方名称.DataPropertyName = "交易方名称";
            this.交易方名称.HeaderText = "交易方名称";
            this.交易方名称.Name = "交易方名称";
            this.交易方名称.ReadOnly = true;
            this.交易方名称.Visible = false;
            // 
            // 关联表Number
            // 
            this.关联表Number.DataPropertyName = "Number";
            this.关联表Number.HeaderText = "关联表Number";
            this.关联表Number.Name = "关联表Number";
            this.关联表Number.ReadOnly = true;
            this.关联表Number.Visible = false;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(10, 531);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(1133, 29);
            this.ucPager1.TabIndex = 132;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(1003, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 133;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(174, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 152;
            this.label1.Text = "复审记录：";
            // 
            // cbxFSJL
            // 
            this.cbxFSJL.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxFSJL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFSJL.FormattingEnabled = true;
            this.cbxFSJL.Items.AddRange(new object[] {
            "请选择",
            "审核中",
            "驳回",
            "审核通过"});
            this.cbxFSJL.Location = new System.Drawing.Point(237, 6);
            this.cbxFSJL.Name = "cbxFSJL";
            this.cbxFSJL.Size = new System.Drawing.Size(93, 22);
            this.cbxFSJL.TabIndex = 151;
            // 
            // cbxSFDQGL
            // 
            this.cbxSFDQGL.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxSFDQGL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSFDQGL.FormattingEnabled = true;
            this.cbxSFDQGL.Items.AddRange(new object[] {
            "请选择",
            "是",
            "否"});
            this.cbxSFDQGL.Location = new System.Drawing.Point(914, 5);
            this.cbxSFDQGL.Name = "cbxSFDQGL";
            this.cbxSFDQGL.Size = new System.Drawing.Size(70, 22);
            this.cbxSFDQGL.TabIndex = 153;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(829, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 154;
            this.label4.Text = "是否当前关联：";
            // 
            // ucJJFJBZL_C
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.cbxSFDQGL);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ucCityList1);
            this.Controls.Add(this.cbxFSJL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtJYFMC);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucJJFJBZL_C";
            this.Size = new System.Drawing.Size(1154, 564);
            this.Load += new System.EventHandler(this.ucJJFJBZL_C_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCCityList ucCityList1;
        private UCTextBox txtJYFMC;
        private Com.Seezt.Skins.BasicButton btnExport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private CMyXls cMyXls1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxFSJL;
        private System.Windows.Forms.DataGridViewLinkColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 是否当前关联;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn 卖家角色编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交易方名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 关联表Number;
        private System.Windows.Forms.ComboBox cbxSFDQGL;
        private System.Windows.Forms.Label label4;
    }
}
