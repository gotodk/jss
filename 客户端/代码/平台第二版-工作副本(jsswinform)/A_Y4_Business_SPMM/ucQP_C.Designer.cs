namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    partial class ucQP_C
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnExport = new Com.Seezt.Skins.BasicButton();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.清盘开始时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.清盘原因 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.清盘类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.清盘状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.清盘结束时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.电子购货合同编号 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.订金解冻金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.履约保证金解冻金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.清盘详情 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbxDJLB = new System.Windows.Forms.ComboBox();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxQPLX = new System.Windows.Forms.ComboBox();
            this.ucTextBox1 = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(651, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 22);
            this.btnExport.TabIndex = 111;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(223, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 110;
            this.label3.Text = "清盘状态：";
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.清盘开始时间,
            this.清盘原因,
            this.清盘类型,
            this.清盘状态,
            this.清盘结束时间,
            this.电子购货合同编号,
            this.订金解冻金额,
            this.履约保证金解冻金额,
            this.清盘详情,
            this.Number});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
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
            this.dataGridView1.TabIndex = 105;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // 清盘开始时间
            // 
            this.清盘开始时间.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.清盘开始时间.DataPropertyName = "清盘开始时间";
            this.清盘开始时间.HeaderText = "清盘开始时间";
            this.清盘开始时间.Name = "清盘开始时间";
            this.清盘开始时间.ReadOnly = true;
            this.清盘开始时间.Width = 107;
            // 
            // 清盘原因
            // 
            this.清盘原因.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.清盘原因.DataPropertyName = "清盘原因";
            this.清盘原因.HeaderText = "清盘原因";
            this.清盘原因.Name = "清盘原因";
            this.清盘原因.ReadOnly = true;
            this.清盘原因.Width = 83;
            // 
            // 清盘类型
            // 
            this.清盘类型.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.清盘类型.DataPropertyName = "清盘类型";
            this.清盘类型.FillWeight = 400F;
            this.清盘类型.HeaderText = "清盘类型";
            this.清盘类型.Name = "清盘类型";
            this.清盘类型.ReadOnly = true;
            this.清盘类型.Width = 83;
            // 
            // 清盘状态
            // 
            this.清盘状态.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.清盘状态.DataPropertyName = "清盘状态";
            this.清盘状态.HeaderText = "清盘状态";
            this.清盘状态.Name = "清盘状态";
            this.清盘状态.ReadOnly = true;
            this.清盘状态.Width = 83;
            // 
            // 清盘结束时间
            // 
            this.清盘结束时间.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.清盘结束时间.DataPropertyName = "清盘结束时间";
            this.清盘结束时间.HeaderText = "清盘结束时间";
            this.清盘结束时间.Name = "清盘结束时间";
            this.清盘结束时间.ReadOnly = true;
            this.清盘结束时间.Width = 107;
            // 
            // 电子购货合同编号
            // 
            this.电子购货合同编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.电子购货合同编号.DataPropertyName = "合同编号";
            this.电子购货合同编号.HeaderText = "电子购货合同编号";
            this.电子购货合同编号.LinkColor = System.Drawing.Color.RoyalBlue;
            this.电子购货合同编号.Name = "电子购货合同编号";
            this.电子购货合同编号.ReadOnly = true;
            this.电子购货合同编号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.电子购货合同编号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.电子购货合同编号.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.电子购货合同编号.Width = 131;
            // 
            // 订金解冻金额
            // 
            this.订金解冻金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.订金解冻金额.DataPropertyName = "订金解冻金额";
            this.订金解冻金额.HeaderText = "订金解冻金额";
            this.订金解冻金额.Name = "订金解冻金额";
            this.订金解冻金额.ReadOnly = true;
            this.订金解冻金额.Width = 107;
            // 
            // 履约保证金解冻金额
            // 
            this.履约保证金解冻金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.履约保证金解冻金额.DataPropertyName = "履约保证金解冻金额";
            this.履约保证金解冻金额.HeaderText = "履约保证金解冻金额";
            this.履约保证金解冻金额.Name = "履约保证金解冻金额";
            this.履约保证金解冻金额.ReadOnly = true;
            this.履约保证金解冻金额.Width = 143;
            // 
            // 清盘详情
            // 
            this.清盘详情.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.清盘详情.HeaderText = "清盘详情";
            this.清盘详情.Name = "清盘详情";
            this.清盘详情.ReadOnly = true;
            this.清盘详情.Text = "清盘详情";
            this.清盘详情.UseColumnTextForLinkValue = true;
            this.清盘详情.Width = 64;
            // 
            // Number
            // 
            this.Number.DataPropertyName = "Number";
            this.Number.HeaderText = "Number";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Visible = false;
            // 
            // cbxDJLB
            // 
            this.cbxDJLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxDJLB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDJLB.FormattingEnabled = true;
            this.cbxDJLB.Items.AddRange(new object[] {
            "请选择清盘状态",
            "清盘中",
            "清盘结束"});
            this.cbxDJLB.Location = new System.Drawing.Point(287, 7);
            this.cbxDJLB.Name = "cbxDJLB";
            this.cbxDJLB.Size = new System.Drawing.Size(140, 22);
            this.cbxDJLB.TabIndex = 108;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(591, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 107;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(11, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 116;
            this.label2.Text = "清盘类型：";
            // 
            // cbxQPLX
            // 
            this.cbxQPLX.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxQPLX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxQPLX.FormattingEnabled = true;
            this.cbxQPLX.Items.AddRange(new object[] {
            "请选择清盘类型",
            "人工清盘",
            "自动清盘"});
            this.cbxQPLX.Location = new System.Drawing.Point(74, 6);
            this.cbxQPLX.Name = "cbxQPLX";
            this.cbxQPLX.Size = new System.Drawing.Size(140, 22);
            this.cbxQPLX.TabIndex = 115;
            // 
            // ucTextBox1
            // 
            this.ucTextBox1.BackColor = System.Drawing.Color.White;
            this.ucTextBox1.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBox1.CanPASTE = true;
            this.ucTextBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ucTextBox1.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBox1.Location = new System.Drawing.Point(444, 6);
            this.ucTextBox1.Name = "ucTextBox1";
            this.ucTextBox1.Size = new System.Drawing.Size(131, 23);
            this.ucTextBox1.TabIndex = 114;
            this.ucTextBox1.TextNtip = "电子购货合同编号";
            this.ucTextBox1.WordWrap = false;
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
            this.ucPager1.TabIndex = 106;
            // 
            // ucQP_C
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.cbxDJLB);
            this.Controls.Add(this.cbxQPLX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ucTextBox1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucQP_C";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucQP_C_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Com.Seezt.Skins.BasicButton btnExport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private System.Windows.Forms.ComboBox cbxDJLB;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private UCTextBox ucTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxQPLX;
        private CMyXls cMyXls1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 清盘开始时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 清盘原因;
        private System.Windows.Forms.DataGridViewTextBoxColumn 清盘类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 清盘状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn 清盘结束时间;
        private System.Windows.Forms.DataGridViewLinkColumn 电子购货合同编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订金解冻金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 履约保证金解冻金额;
        private System.Windows.Forms.DataGridViewLinkColumn 清盘详情;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
    }
}
