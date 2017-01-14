namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    partial class ucYXDTHD_C
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
            this.label1 = new System.Windows.Forms.Label();
            this.uctxtTHDBH = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.uctxtHTBH = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtSPMC = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            this.dbsj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thdbh = new System.Windows.Forms.DataGridViewLinkColumn();
            this.xdyddwc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shangpin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xinghao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hthsl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.经济批量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.最低价标的经济批量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.电子购货合同 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.中标定标表编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(773, 7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 22);
            this.btnExport.TabIndex = 143;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(222, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 142;
            this.label3.Text = "电子购货合同编号：";
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
            this.dbsj,
            this.thdbh,
            this.xdyddwc,
            this.shangpin,
            this.xinghao,
            this.jg,
            this.hthsl,
            this.经济批量,
            this.最低价标的经济批量,
            this.Column1,
            this.Column2,
            this.电子购货合同,
            this.Column3,
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
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(713, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 140;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(475, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 146;
            this.label1.Text = "提货单编号：";
            // 
            // uctxtTHDBH
            // 
            this.uctxtTHDBH.BackColor = System.Drawing.Color.White;
            this.uctxtTHDBH.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.uctxtTHDBH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctxtTHDBH.CanPASTE = true;
            this.uctxtTHDBH.Counts = 2;
            this.uctxtTHDBH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uctxtTHDBH.ForeColor = System.Drawing.Color.Black;
            this.uctxtTHDBH.HotColor = System.Drawing.Color.CornflowerBlue;
            this.uctxtTHDBH.Location = new System.Drawing.Point(553, 5);
            this.uctxtTHDBH.Name = "uctxtTHDBH";
            this.uctxtTHDBH.Size = new System.Drawing.Size(131, 23);
            this.uctxtTHDBH.TabIndex = 147;
            this.uctxtTHDBH.TextNtip = "";
            this.uctxtTHDBH.WordWrap = false;
            // 
            // uctxtHTBH
            // 
            this.uctxtHTBH.BackColor = System.Drawing.Color.White;
            this.uctxtHTBH.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.uctxtHTBH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctxtHTBH.CanPASTE = true;
            this.uctxtHTBH.Counts = 2;
            this.uctxtHTBH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uctxtHTBH.ForeColor = System.Drawing.Color.Black;
            this.uctxtHTBH.HotColor = System.Drawing.Color.CornflowerBlue;
            this.uctxtHTBH.Location = new System.Drawing.Point(335, 6);
            this.uctxtHTBH.Name = "uctxtHTBH";
            this.uctxtHTBH.Size = new System.Drawing.Size(131, 23);
            this.uctxtHTBH.TabIndex = 145;
            this.uctxtHTBH.TextNtip = "";
            this.uctxtHTBH.WordWrap = false;
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
            this.txtSPMC.Location = new System.Drawing.Point(86, 6);
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
            // dbsj
            // 
            this.dbsj.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dbsj.DataPropertyName = "下达时间";
            this.dbsj.HeaderText = "下达时间";
            this.dbsj.Name = "dbsj";
            this.dbsj.ReadOnly = true;
            this.dbsj.Width = 83;
            // 
            // thdbh
            // 
            this.thdbh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.thdbh.DataPropertyName = "提货单编号";
            this.thdbh.HeaderText = "提货单编号";
            this.thdbh.LinkColor = System.Drawing.Color.RoyalBlue;
            this.thdbh.Name = "thdbh";
            this.thdbh.ReadOnly = true;
            this.thdbh.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.thdbh.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.thdbh.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.thdbh.Width = 95;
            // 
            // xdyddwc
            // 
            this.xdyddwc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.xdyddwc.DataPropertyName = "商品编号";
            this.xdyddwc.FillWeight = 400F;
            this.xdyddwc.HeaderText = "商品编号";
            this.xdyddwc.Name = "xdyddwc";
            this.xdyddwc.ReadOnly = true;
            this.xdyddwc.Width = 83;
            // 
            // shangpin
            // 
            this.shangpin.DataPropertyName = "商品名称";
            this.shangpin.HeaderText = "商品名称";
            this.shangpin.Name = "shangpin";
            this.shangpin.ReadOnly = true;
            this.shangpin.Width = 160;
            // 
            // xinghao
            // 
            this.xinghao.DataPropertyName = "规格";
            this.xinghao.HeaderText = "规格";
            this.xinghao.Name = "xinghao";
            this.xinghao.ReadOnly = true;
            this.xinghao.Width = 160;
            // 
            // jg
            // 
            this.jg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.jg.DataPropertyName = "提货数量";
            this.jg.HeaderText = "提货数量";
            this.jg.Name = "jg";
            this.jg.ReadOnly = true;
            this.jg.Width = 83;
            // 
            // hthsl
            // 
            this.hthsl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.hthsl.DataPropertyName = "定标价格";
            this.hthsl.HeaderText = "定标价格";
            this.hthsl.Name = "hthsl";
            this.hthsl.ReadOnly = true;
            this.hthsl.Width = 83;
            // 
            // 经济批量
            // 
            this.经济批量.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.经济批量.DataPropertyName = "提货金额";
            this.经济批量.HeaderText = "提货金额";
            this.经济批量.Name = "经济批量";
            this.经济批量.ReadOnly = true;
            this.经济批量.Width = 83;
            // 
            // 最低价标的经济批量
            // 
            this.最低价标的经济批量.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.最低价标的经济批量.DataPropertyName = "冻结货款金额";
            this.最低价标的经济批量.HeaderText = "冻结货款金额";
            this.最低价标的经济批量.Name = "最低价标的经济批量";
            this.最低价标的经济批量.ReadOnly = true;
            this.最低价标的经济批量.Width = 107;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "买家名称";
            this.Column1.HeaderText = "买方名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 160;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "卖家名称";
            this.Column2.HeaderText = "卖方名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 160;
            // 
            // 电子购货合同
            // 
            this.电子购货合同.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.电子购货合同.DataPropertyName = "电子购货合同编号";
            this.电子购货合同.HeaderText = "对应电子购货合同编号";
            this.电子购货合同.LinkColor = System.Drawing.Color.RoyalBlue;
            this.电子购货合同.Name = "电子购货合同";
            this.电子购货合同.ReadOnly = true;
            this.电子购货合同.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.电子购货合同.Width = 136;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.DataPropertyName = "提货单状态";
            this.Column3.HeaderText = "提货单状态";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 95;
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
            // ucYXDTHD_C
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.uctxtTHDBH);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uctxtHTBH);
            this.Controls.Add(this.txtSPMC);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucYXDTHD_C";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucYXDTHD_C_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCTextBox uctxtHTBH;
        private UCTextBox txtSPMC;
        private Com.Seezt.Skins.BasicButton btnExport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private UCTextBox uctxtTHDBH;
        private System.Windows.Forms.Label label1;
        private CMyXls cMyXls1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dbsj;
        private System.Windows.Forms.DataGridViewLinkColumn thdbh;
        private System.Windows.Forms.DataGridViewTextBoxColumn xdyddwc;
        private System.Windows.Forms.DataGridViewTextBoxColumn shangpin;
        private System.Windows.Forms.DataGridViewTextBoxColumn xinghao;
        private System.Windows.Forms.DataGridViewTextBoxColumn jg;
        private System.Windows.Forms.DataGridViewTextBoxColumn hthsl;
        private System.Windows.Forms.DataGridViewTextBoxColumn 经济批量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 最低价标的经济批量;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewLinkColumn 电子购货合同;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 中标定标表编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同状态;
    }
}
