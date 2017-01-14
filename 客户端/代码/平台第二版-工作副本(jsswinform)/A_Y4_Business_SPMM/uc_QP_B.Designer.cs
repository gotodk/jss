namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    partial class uc_QP_B
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
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.详情及操作 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.电子购货合同编号 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.合同结束日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.商品编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.商品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同期限 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单价 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.争议数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.争议金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.清盘类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.清盘原因 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.主键 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.txtSPMC.TabIndex = 126;
            this.txtSPMC.TextNtip = "";
            this.txtSPMC.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 124;
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
            this.详情及操作,
            this.电子购货合同编号,
            this.合同结束日期,
            this.商品编号,
            this.商品名称,
            this.规格,
            this.合同期限,
            this.合同数量,
            this.单价,
            this.合同金额,
            this.争议数量,
            this.争议金额,
            this.清盘类型,
            this.清盘原因,
            this.主键});
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
            this.dataGridView1.TabIndex = 120;
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
            this.ucPager1.TabIndex = 121;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(245, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 122;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // 详情及操作
            // 
            this.详情及操作.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.详情及操作.HeaderText = "详情及操作";
            this.详情及操作.LinkColor = System.Drawing.Color.RoyalBlue;
            this.详情及操作.Name = "详情及操作";
            this.详情及操作.ReadOnly = true;
            this.详情及操作.Text = "清盘处理";
            this.详情及操作.UseColumnTextForLinkValue = true;
            this.详情及操作.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.详情及操作.Width = 76;
            // 
            // 电子购货合同编号
            // 
            this.电子购货合同编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.电子购货合同编号.DataPropertyName = "电子购货合同编号";
            this.电子购货合同编号.HeaderText = "电子购货合同编号";
            this.电子购货合同编号.LinkColor = System.Drawing.Color.RoyalBlue;
            this.电子购货合同编号.Name = "电子购货合同编号";
            this.电子购货合同编号.ReadOnly = true;
            this.电子购货合同编号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.电子购货合同编号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.电子购货合同编号.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.电子购货合同编号.Width = 131;
            // 
            // 合同结束日期
            // 
            this.合同结束日期.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同结束日期.DataPropertyName = "合同结束日期";
            this.合同结束日期.FillWeight = 160F;
            this.合同结束日期.HeaderText = "电子购货合同结束日期";
            this.合同结束日期.Name = "合同结束日期";
            this.合同结束日期.ReadOnly = true;
            this.合同结束日期.Width = 155;
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
            this.商品名称.FillWeight = 160F;
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
            this.规格.Width = 115;
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
            // 合同数量
            // 
            this.合同数量.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同数量.DataPropertyName = "合同数量";
            this.合同数量.HeaderText = "合同数量";
            this.合同数量.Name = "合同数量";
            this.合同数量.ReadOnly = true;
            this.合同数量.Width = 83;
            // 
            // 单价
            // 
            this.单价.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.单价.DataPropertyName = "单价";
            this.单价.HeaderText = "单价";
            this.单价.Name = "单价";
            this.单价.ReadOnly = true;
            this.单价.Width = 59;
            // 
            // 合同金额
            // 
            this.合同金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同金额.DataPropertyName = "合同金额";
            this.合同金额.HeaderText = "合同金额";
            this.合同金额.Name = "合同金额";
            this.合同金额.ReadOnly = true;
            this.合同金额.Width = 83;
            // 
            // 争议数量
            // 
            this.争议数量.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.争议数量.DataPropertyName = "争议数量";
            this.争议数量.HeaderText = "争议数量";
            this.争议数量.Name = "争议数量";
            this.争议数量.ReadOnly = true;
            this.争议数量.Width = 83;
            // 
            // 争议金额
            // 
            this.争议金额.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.争议金额.DataPropertyName = "争议金额";
            this.争议金额.HeaderText = "争议金额";
            this.争议金额.Name = "争议金额";
            this.争议金额.ReadOnly = true;
            this.争议金额.Width = 83;
            // 
            // 清盘类型
            // 
            this.清盘类型.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.清盘类型.DataPropertyName = "清盘类型";
            this.清盘类型.HeaderText = "清盘类型";
            this.清盘类型.Name = "清盘类型";
            this.清盘类型.ReadOnly = true;
            this.清盘类型.Width = 83;
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
            // 主键
            // 
            this.主键.DataPropertyName = "主键";
            this.主键.HeaderText = "主键";
            this.主键.Name = "主键";
            this.主键.ReadOnly = true;
            this.主键.Visible = false;
            // 
            // uc_QP_B
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.txtSPMC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "uc_QP_B";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.uc_QP_B_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCTextBox txtSPMC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private System.Windows.Forms.DataGridViewLinkColumn 详情及操作;
        private System.Windows.Forms.DataGridViewLinkColumn 电子购货合同编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同结束日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同期限;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单价;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 争议数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 争议金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 清盘类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 清盘原因;
        private System.Windows.Forms.DataGridViewTextBoxColumn 主键;
    }
}
