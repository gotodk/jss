namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    partial class ucDBYBZH_C
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
            this.txtSPMC = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.btnExport = new Com.Seezt.Skins.BasicButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.cbxDJLB = new System.Windows.Forms.ComboBox();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            this.查看详情 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.查看保证函 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.合同编号 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.定标时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.下单时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.商品编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.商品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同期限 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.定标数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.定标价格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.定标金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.投标保证金解冻金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订金冻结金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.履约保证金冻结金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.违约扣罚记录 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.履约保证金冻结剩余比例 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否拆单 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.中标定标编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSPMC
            // 
            this.txtSPMC.BackColor = System.Drawing.Color.White;
            this.txtSPMC.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtSPMC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSPMC.CanPASTE = true;
            this.txtSPMC.Counts = 2;
            this.txtSPMC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSPMC.ForeColor = System.Drawing.Color.Black;
            this.txtSPMC.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtSPMC.Location = new System.Drawing.Point(82, 5);
            this.txtSPMC.Name = "txtSPMC";
            this.txtSPMC.Size = new System.Drawing.Size(131, 23);
            this.txtSPMC.TabIndex = 120;
            this.txtSPMC.TextNtip = "";
            this.txtSPMC.WordWrap = false;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(524, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 22);
            this.btnExport.TabIndex = 119;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(238, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 118;
            this.label3.Text = "单据类别：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 117;
            this.label2.Text = "商品名称：";
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
            this.查看详情,
            this.查看保证函,
            this.合同编号,
            this.定标时间,
            this.下单时间,
            this.单据类型,
            this.单据编号,
            this.商品编号,
            this.商品名称,
            this.规格,
            this.合同期限,
            this.定标数量,
            this.定标价格,
            this.定标金额,
            this.投标保证金解冻金额,
            this.订金冻结金额,
            this.履约保证金冻结金额,
            this.违约扣罚记录,
            this.履约保证金冻结剩余比例,
            this.是否拆单,
            this.中标定标编号});
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
            this.dataGridView1.Location = new System.Drawing.Point(10, 34);
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
            this.dataGridView1.TabIndex = 113;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
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
            this.ucPager1.TabIndex = 114;
            // 
            // cbxDJLB
            // 
            this.cbxDJLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxDJLB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDJLB.FormattingEnabled = true;
            this.cbxDJLB.Items.AddRange(new object[] {
            "请选择单据类别",
            "预订单",
            "投标单"});
            this.cbxDJLB.Location = new System.Drawing.Point(301, 6);
            this.cbxDJLB.Name = "cbxDJLB";
            this.cbxDJLB.Size = new System.Drawing.Size(140, 22);
            this.cbxDJLB.TabIndex = 116;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(464, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 115;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // 查看详情
            // 
            this.查看详情.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.查看详情.HeaderText = "查看详情";
            this.查看详情.LinkColor = System.Drawing.Color.RoyalBlue;
            this.查看详情.Name = "查看详情";
            this.查看详情.ReadOnly = true;
            this.查看详情.Text = "查看详情";
            this.查看详情.UseColumnTextForLinkValue = true;
            this.查看详情.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.查看详情.Width = 64;
            // 
            // 查看保证函
            // 
            this.查看保证函.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.查看保证函.HeaderText = "查看保证函";
            this.查看保证函.LinkColor = System.Drawing.Color.RoyalBlue;
            this.查看保证函.Name = "查看保证函";
            this.查看保证函.ReadOnly = true;
            this.查看保证函.Text = "查看保证函";
            this.查看保证函.UseColumnTextForLinkValue = true;
            this.查看保证函.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.查看保证函.Width = 76;
            // 
            // 合同编号
            // 
            this.合同编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同编号.DataPropertyName = "合同编号";
            this.合同编号.HeaderText = "合同编号";
            this.合同编号.LinkColor = System.Drawing.Color.RoyalBlue;
            this.合同编号.Name = "合同编号";
            this.合同编号.ReadOnly = true;
            this.合同编号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.合同编号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.合同编号.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.合同编号.Width = 83;
            // 
            // 定标时间
            // 
            this.定标时间.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.定标时间.DataPropertyName = "定标时间";
            this.定标时间.HeaderText = "定标时间";
            this.定标时间.Name = "定标时间";
            this.定标时间.ReadOnly = true;
            this.定标时间.Width = 83;
            // 
            // 下单时间
            // 
            this.下单时间.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.下单时间.DataPropertyName = "下单时间";
            this.下单时间.HeaderText = "下单时间";
            this.下单时间.Name = "下单时间";
            this.下单时间.ReadOnly = true;
            this.下单时间.Width = 83;
            // 
            // 单据类型
            // 
            this.单据类型.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.单据类型.DataPropertyName = "单据类型";
            this.单据类型.HeaderText = "单据类型";
            this.单据类型.Name = "单据类型";
            this.单据类型.ReadOnly = true;
            this.单据类型.Width = 83;
            // 
            // 单据编号
            // 
            this.单据编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.单据编号.DataPropertyName = "单据编号";
            this.单据编号.HeaderText = "单据编号";
            this.单据编号.Name = "单据编号";
            this.单据编号.ReadOnly = true;
            this.单据编号.Width = 83;
            // 
            // 商品编号
            // 
            this.商品编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.商品编号.DataPropertyName = "商品编号";
            this.商品编号.HeaderText = "商品编号";
            this.商品编号.Name = "商品编号";
            this.商品编号.ReadOnly = true;
            this.商品编号.Width = 83;
            // 
            // 商品名称
            // 
            this.商品名称.DataPropertyName = "商品名称";
            this.商品名称.HeaderText = "商品名称";
            this.商品名称.Name = "商品名称";
            this.商品名称.ReadOnly = true;
            this.商品名称.Width = 160;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            this.规格.Width = 160;
            // 
            // 合同期限
            // 
            this.合同期限.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同期限.DataPropertyName = "合同期限";
            this.合同期限.HeaderText = "合同期限";
            this.合同期限.Name = "合同期限";
            this.合同期限.ReadOnly = true;
            this.合同期限.Width = 83;
            // 
            // 定标数量
            // 
            this.定标数量.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.定标数量.DataPropertyName = "定标数量";
            this.定标数量.HeaderText = "定标数量";
            this.定标数量.Name = "定标数量";
            this.定标数量.ReadOnly = true;
            this.定标数量.Width = 83;
            // 
            // 定标价格
            // 
            this.定标价格.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.定标价格.DataPropertyName = "定标价格";
            this.定标价格.HeaderText = "定标价格";
            this.定标价格.Name = "定标价格";
            this.定标价格.ReadOnly = true;
            this.定标价格.Width = 83;
            // 
            // 定标金额
            // 
            this.定标金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.定标金额.DataPropertyName = "定标金额";
            this.定标金额.HeaderText = "定标金额";
            this.定标金额.Name = "定标金额";
            this.定标金额.ReadOnly = true;
            this.定标金额.Width = 83;
            // 
            // 投标保证金解冻金额
            // 
            this.投标保证金解冻金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.投标保证金解冻金额.DataPropertyName = "投标保证金解冻金额";
            this.投标保证金解冻金额.HeaderText = "投标保证金解冻金额";
            this.投标保证金解冻金额.Name = "投标保证金解冻金额";
            this.投标保证金解冻金额.ReadOnly = true;
            this.投标保证金解冻金额.Width = 143;
            // 
            // 订金冻结金额
            // 
            this.订金冻结金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.订金冻结金额.DataPropertyName = "订金冻结金额";
            this.订金冻结金额.HeaderText = "订金冻结金额";
            this.订金冻结金额.Name = "订金冻结金额";
            this.订金冻结金额.ReadOnly = true;
            this.订金冻结金额.Width = 107;
            // 
            // 履约保证金冻结金额
            // 
            this.履约保证金冻结金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.履约保证金冻结金额.DataPropertyName = "履约保证金冻结金额";
            this.履约保证金冻结金额.HeaderText = "履约保证金冻结金额";
            this.履约保证金冻结金额.Name = "履约保证金冻结金额";
            this.履约保证金冻结金额.ReadOnly = true;
            this.履约保证金冻结金额.Width = 143;
            // 
            // 违约扣罚记录
            // 
            this.违约扣罚记录.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.违约扣罚记录.DataPropertyName = "违约扣罚记录";
            this.违约扣罚记录.HeaderText = "违约扣罚记录";
            this.违约扣罚记录.Name = "违约扣罚记录";
            this.违约扣罚记录.ReadOnly = true;
            this.违约扣罚记录.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.违约扣罚记录.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.违约扣罚记录.Width = 107;
            // 
            // 履约保证金冻结剩余比例
            // 
            this.履约保证金冻结剩余比例.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.履约保证金冻结剩余比例.DataPropertyName = "履约保证金冻结剩余比例";
            this.履约保证金冻结剩余比例.HeaderText = "履约保证金冻结剩余比例%";
            this.履约保证金冻结剩余比例.Name = "履约保证金冻结剩余比例";
            this.履约保证金冻结剩余比例.ReadOnly = true;
            this.履约保证金冻结剩余比例.Width = 179;
            // 
            // 是否拆单
            // 
            this.是否拆单.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.是否拆单.DataPropertyName = "是否拆单";
            this.是否拆单.HeaderText = "是否拆单";
            this.是否拆单.Name = "是否拆单";
            this.是否拆单.ReadOnly = true;
            this.是否拆单.Width = 83;
            // 
            // 中标定标编号
            // 
            this.中标定标编号.DataPropertyName = "中标定标编号";
            this.中标定标编号.HeaderText = "中标定标编号";
            this.中标定标编号.Name = "中标定标编号";
            this.中标定标编号.ReadOnly = true;
            this.中标定标编号.Visible = false;
            // 
            // ucDBYBZH_C
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.txtSPMC);
            this.Controls.Add(this.cbxDJLB);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucDBYBZH_C";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucDBYBZH_C_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCTextBox txtSPMC;
        private Com.Seezt.Skins.BasicButton btnExport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private System.Windows.Forms.ComboBox cbxDJLB;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private CMyXls cMyXls1;
        private System.Windows.Forms.DataGridViewLinkColumn 查看详情;
        private System.Windows.Forms.DataGridViewLinkColumn 查看保证函;
        private System.Windows.Forms.DataGridViewLinkColumn 合同编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 定标时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 下单时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同期限;
        private System.Windows.Forms.DataGridViewTextBoxColumn 定标数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 定标价格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 定标金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 投标保证金解冻金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订金冻结金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 履约保证金冻结金额;
        private System.Windows.Forms.DataGridViewLinkColumn 违约扣罚记录;
        private System.Windows.Forms.DataGridViewTextBoxColumn 履约保证金冻结剩余比例;
        private System.Windows.Forms.DataGridViewTextBoxColumn 是否拆单;
        private System.Windows.Forms.DataGridViewTextBoxColumn 中标定标编号;
    }
}
