namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    partial class ucWTYCL_C
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
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.问题产生时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.问题类别及详情 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.处理状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发货单号 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.商品编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.商品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发货数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.定标价格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发货金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.买家名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.卖家名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物流单编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票发送方式 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同编号 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.中标定标表编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.txtSPMC.Location = new System.Drawing.Point(86, 5);
            this.txtSPMC.Name = "txtSPMC";
            this.txtSPMC.Size = new System.Drawing.Size(131, 23);
            this.txtSPMC.TabIndex = 144;
            this.txtSPMC.TextNtip = "";
            this.txtSPMC.WordWrap = false;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(293, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 22);
            this.btnExport.TabIndex = 143;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 141;
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
            this.Number,
            this.问题产生时间,
            this.问题类别及详情,
            this.处理状态,
            this.发货单号,
            this.商品编号,
            this.商品名称,
            this.规格,
            this.发货数量,
            this.定标价格,
            this.发货金额,
            this.买家名称,
            this.卖家名称,
            this.物流单编号,
            this.发票编号,
            this.发票发送方式,
            this.合同编号,
            this.中标定标表编号,
            this.合同状态});
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
            this.dataGridView1.TabIndex = 138;
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
            this.ucPager1.TabIndex = 139;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(233, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 140;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // Number
            // 
            this.Number.DataPropertyName = "Number";
            this.Number.HeaderText = "Number";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Visible = false;
            // 
            // 问题产生时间
            // 
            this.问题产生时间.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.问题产生时间.DataPropertyName = "问题产生时间";
            this.问题产生时间.HeaderText = "问题产生时间";
            this.问题产生时间.Name = "问题产生时间";
            this.问题产生时间.ReadOnly = true;
            this.问题产生时间.Width = 107;
            // 
            // 问题类别及详情
            // 
            this.问题类别及详情.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.问题类别及详情.DataPropertyName = "问题类别及详情";
            this.问题类别及详情.HeaderText = "问题类别及详情";
            this.问题类别及详情.Name = "问题类别及详情";
            this.问题类别及详情.ReadOnly = true;
            this.问题类别及详情.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.问题类别及详情.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.问题类别及详情.Width = 119;
            // 
            // 处理状态
            // 
            this.处理状态.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.处理状态.DataPropertyName = "处理状态";
            this.处理状态.FillWeight = 400F;
            this.处理状态.HeaderText = "处理状态";
            this.处理状态.Name = "处理状态";
            this.处理状态.ReadOnly = true;
            this.处理状态.Width = 83;
            // 
            // 发货单号
            // 
            this.发货单号.ActiveLinkColor = System.Drawing.Color.CornflowerBlue;
            this.发货单号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.发货单号.DataPropertyName = "发货单号";
            this.发货单号.HeaderText = "发货单号";
            this.发货单号.Name = "发货单号";
            this.发货单号.ReadOnly = true;
            this.发货单号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.发货单号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.发货单号.Width = 83;
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
            // 发货数量
            // 
            this.发货数量.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.发货数量.DataPropertyName = "提货数量";
            this.发货数量.HeaderText = "发货数量";
            this.发货数量.Name = "发货数量";
            this.发货数量.ReadOnly = true;
            this.发货数量.Width = 83;
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
            // 发货金额
            // 
            this.发货金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.发货金额.DataPropertyName = "提货金额";
            this.发货金额.HeaderText = "发货金额";
            this.发货金额.Name = "发货金额";
            this.发货金额.ReadOnly = true;
            this.发货金额.Width = 83;
            // 
            // 买家名称
            // 
            this.买家名称.DataPropertyName = "买家名称";
            this.买家名称.HeaderText = "买方名称";
            this.买家名称.Name = "买家名称";
            this.买家名称.ReadOnly = true;
            this.买家名称.Width = 160;
            // 
            // 卖家名称
            // 
            this.卖家名称.DataPropertyName = "卖家名称";
            this.卖家名称.HeaderText = "卖方名称";
            this.卖家名称.Name = "卖家名称";
            this.卖家名称.ReadOnly = true;
            this.卖家名称.Width = 160;
            // 
            // 物流单编号
            // 
            this.物流单编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.物流单编号.DataPropertyName = "物流单号";
            this.物流单编号.HeaderText = "物流单编号";
            this.物流单编号.Name = "物流单编号";
            this.物流单编号.ReadOnly = true;
            this.物流单编号.Width = 95;
            // 
            // 发票编号
            // 
            this.发票编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.发票编号.DataPropertyName = "发票编号";
            this.发票编号.HeaderText = "发票编号";
            this.发票编号.Name = "发票编号";
            this.发票编号.ReadOnly = true;
            this.发票编号.Width = 83;
            // 
            // 发票发送方式
            // 
            this.发票发送方式.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.发票发送方式.DataPropertyName = "发票发送方式";
            this.发票发送方式.HeaderText = "发票发送方式";
            this.发票发送方式.Name = "发票发送方式";
            this.发票发送方式.ReadOnly = true;
            this.发票发送方式.Width = 107;
            // 
            // 合同编号
            // 
            this.合同编号.ActiveLinkColor = System.Drawing.Color.CornflowerBlue;
            this.合同编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同编号.DataPropertyName = "合同编号";
            this.合同编号.HeaderText = "对应电子购货合同编号";
            this.合同编号.Name = "合同编号";
            this.合同编号.ReadOnly = true;
            this.合同编号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.合同编号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.合同编号.Width = 155;
            // 
            // 中标定标表编号
            // 
            this.中标定标表编号.DataPropertyName = "中标定标表编号";
            this.中标定标表编号.HeaderText = "中标定标表编号";
            this.中标定标表编号.Name = "中标定标表编号";
            this.中标定标表编号.ReadOnly = true;
            this.中标定标表编号.Visible = false;
            // 
            // 合同状态
            // 
            this.合同状态.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同状态.DataPropertyName = "合同状态";
            this.合同状态.HeaderText = "合同状态";
            this.合同状态.Name = "合同状态";
            this.合同状态.ReadOnly = true;
            this.合同状态.Width = 83;
            // 
            // ucWTYCL_C
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.txtSPMC);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucWTYCL_C";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucWTYCL_C_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCTextBox txtSPMC;
        private Com.Seezt.Skins.BasicButton btnExport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private CMyXls cMyXls1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn 问题产生时间;
        private System.Windows.Forms.DataGridViewLinkColumn 问题类别及详情;
        private System.Windows.Forms.DataGridViewTextBoxColumn 处理状态;
        private System.Windows.Forms.DataGridViewLinkColumn 发货单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发货数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 定标价格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发货金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 买家名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 卖家名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物流单编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发票编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发票发送方式;
        private System.Windows.Forms.DataGridViewLinkColumn 合同编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 中标定标表编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同状态;
    }
}
