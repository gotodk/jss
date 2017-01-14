namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    partial class ucLRFHXX_B
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.txtFHDBH = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtSPMC = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.CKBoxSHAdress = new Com.Seezt.Skins.BasicCheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.录入发货信息 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.录入发票邮寄信息 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.商品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发货单编号 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.定标价格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.提货数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.买家名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.买家收货区域 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票是否随货同行 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.距最迟发货日天数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发货单状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票录入时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(238, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 142;
            this.label3.Text = "发货单编号：";
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
            this.录入发货信息,
            this.录入发票邮寄信息,
            this.商品名称,
            this.规格,
            this.发货单编号,
            this.定标价格,
            this.提货数量,
            this.买家名称,
            this.买家收货区域,
            this.发票是否随货同行,
            this.距最迟发货日天数,
            this.发货单状态,
            this.发票录入时间});
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
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(464, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 140;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtFHDBH
            // 
            this.txtFHDBH.BackColor = System.Drawing.Color.White;
            this.txtFHDBH.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtFHDBH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFHDBH.CanPASTE = true;
            this.txtFHDBH.Counts = 2;
            this.txtFHDBH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFHDBH.ForeColor = System.Drawing.Color.Black;
            this.txtFHDBH.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtFHDBH.Location = new System.Drawing.Point(313, 6);
            this.txtFHDBH.Name = "txtFHDBH";
            this.txtFHDBH.Size = new System.Drawing.Size(131, 23);
            this.txtFHDBH.TabIndex = 145;
            this.txtFHDBH.TextNtip = "";
            this.txtFHDBH.WordWrap = false;
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
            // CKBoxSHAdress
            // 
            this.CKBoxSHAdress.AutoSize = true;
            this.CKBoxSHAdress.BackColor = System.Drawing.Color.Transparent;
            this.CKBoxSHAdress.Checked = false;
            this.CKBoxSHAdress.CheckState = Com.Seezt.Skins.BasicCheckBox.CheckStates.Unchecked;
            this.CKBoxSHAdress.Location = new System.Drawing.Point(614, 9);
            this.CKBoxSHAdress.Name = "CKBoxSHAdress";
            this.CKBoxSHAdress.Size = new System.Drawing.Size(90, 15);
            this.CKBoxSHAdress.TabIndex = 167;
            this.CKBoxSHAdress.Texts = "按收货区域";
            this.CKBoxSHAdress.CheckedChanged += new Com.Seezt.Skins.BasicCheckBox.CheckedChangedEventHandler(this.CKBoxSHAdress_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(567, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 166;
            this.label1.Text = "排序：";
            // 
            // 录入发货信息
            // 
            this.录入发货信息.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.录入发货信息.HeaderText = "操作";
            this.录入发货信息.LinkColor = System.Drawing.Color.RoyalBlue;
            this.录入发货信息.Name = "录入发货信息";
            this.录入发货信息.ReadOnly = true;
            this.录入发货信息.Text = "录入发货信息";
            this.录入发货信息.UseColumnTextForLinkValue = true;
            this.录入发货信息.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.录入发货信息.Width = 40;
            // 
            // 录入发票邮寄信息
            // 
            this.录入发票邮寄信息.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.录入发票邮寄信息.HeaderText = "";
            this.录入发票邮寄信息.LinkColor = System.Drawing.Color.RoyalBlue;
            this.录入发票邮寄信息.Name = "录入发票邮寄信息";
            this.录入发票邮寄信息.ReadOnly = true;
            this.录入发票邮寄信息.Text = "录入发票邮寄信息";
            this.录入发票邮寄信息.UseColumnTextForLinkValue = true;
            this.录入发票邮寄信息.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.录入发票邮寄信息.Width = 6;
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
            // 发货单编号
            // 
            this.发货单编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.发货单编号.DataPropertyName = "发货单编号";
            this.发货单编号.FillWeight = 400F;
            this.发货单编号.HeaderText = "发货单编号";
            this.发货单编号.LinkColor = System.Drawing.Color.RoyalBlue;
            this.发货单编号.Name = "发货单编号";
            this.发货单编号.ReadOnly = true;
            this.发货单编号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.发货单编号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.发货单编号.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.发货单编号.Width = 95;
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
            // 提货数量
            // 
            this.提货数量.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.提货数量.DataPropertyName = "提货数量";
            this.提货数量.HeaderText = "提货数量";
            this.提货数量.Name = "提货数量";
            this.提货数量.ReadOnly = true;
            this.提货数量.Width = 83;
            // 
            // 买家名称
            // 
            this.买家名称.DataPropertyName = "买家名称";
            this.买家名称.HeaderText = "买方名称";
            this.买家名称.Name = "买家名称";
            this.买家名称.ReadOnly = true;
            this.买家名称.Width = 160;
            // 
            // 买家收货区域
            // 
            this.买家收货区域.DataPropertyName = "买家收货区域";
            this.买家收货区域.HeaderText = "买方收货区域";
            this.买家收货区域.Name = "买家收货区域";
            this.买家收货区域.ReadOnly = true;
            this.买家收货区域.Width = 160;
            // 
            // 发票是否随货同行
            // 
            this.发票是否随货同行.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.发票是否随货同行.DataPropertyName = "发票是否随货同行";
            this.发票是否随货同行.HeaderText = "发票是否随货同行";
            this.发票是否随货同行.Name = "发票是否随货同行";
            this.发票是否随货同行.ReadOnly = true;
            this.发票是否随货同行.Width = 131;
            // 
            // 距最迟发货日天数
            // 
            this.距最迟发货日天数.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.距最迟发货日天数.DataPropertyName = "距最迟发货日天数";
            this.距最迟发货日天数.HeaderText = "距最迟发货日天数";
            this.距最迟发货日天数.Name = "距最迟发货日天数";
            this.距最迟发货日天数.ReadOnly = true;
            this.距最迟发货日天数.Width = 131;
            // 
            // 发货单状态
            // 
            this.发货单状态.DataPropertyName = "发货单状态";
            this.发货单状态.HeaderText = "发货单状态";
            this.发货单状态.Name = "发货单状态";
            this.发货单状态.ReadOnly = true;
            this.发货单状态.Visible = false;
            // 
            // 发票录入时间
            // 
            this.发票录入时间.DataPropertyName = "发票录入时间";
            this.发票录入时间.HeaderText = "发票录入时间";
            this.发票录入时间.Name = "发票录入时间";
            this.发票录入时间.ReadOnly = true;
            this.发票录入时间.Visible = false;
            // 
            // ucLRFHXX_B
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.CKBoxSHAdress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFHDBH);
            this.Controls.Add(this.txtSPMC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucLRFHXX_B";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucLRFHXX_B_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCTextBox txtFHDBH;
        private UCTextBox txtSPMC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private Com.Seezt.Skins.BasicCheckBox CKBoxSHAdress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewLinkColumn 录入发货信息;
        private System.Windows.Forms.DataGridViewLinkColumn 录入发票邮寄信息;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewLinkColumn 发货单编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 定标价格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 提货数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 买家名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 买家收货区域;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发票是否随货同行;
        private System.Windows.Forms.DataGridViewTextBoxColumn 距最迟发货日天数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发货单状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发票录入时间;
    }
}
