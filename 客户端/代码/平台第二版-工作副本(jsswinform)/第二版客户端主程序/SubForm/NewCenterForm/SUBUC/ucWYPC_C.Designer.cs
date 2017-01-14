namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    partial class ucWYPC_C
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
            this.cbxXM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtTImeEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dbsj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yddh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xdyddwc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shangpin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xinghao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.dtTimeStart = new System.Windows.Forms.DateTimePicker();
            this.cbxXZ = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(820, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 22);
            this.btnExport.TabIndex = 79;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // cbxXM
            // 
            this.cbxXM.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxXM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxXM.FormattingEnabled = true;
            this.cbxXM.Items.AddRange(new object[] {
            "转入资金"});
            this.cbxXM.Location = new System.Drawing.Point(395, 5);
            this.cbxXM.Name = "cbxXM";
            this.cbxXM.Size = new System.Drawing.Size(119, 22);
            this.cbxXM.TabIndex = 78;
            this.cbxXM.SelectionChangeCommitted += new System.EventHandler(this.cbxXM_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(520, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 77;
            this.label3.Text = "性质：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(348, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 76;
            this.label2.Text = "项目：";
            // 
            // dtTImeEnd
            // 
            this.dtTImeEnd.CalendarForeColor = System.Drawing.Color.Black;
            this.dtTImeEnd.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dtTImeEnd.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dtTImeEnd.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dtTImeEnd.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dtTImeEnd.Location = new System.Drawing.Point(211, 6);
            this.dtTImeEnd.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dtTImeEnd.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dtTImeEnd.Name = "dtTImeEnd";
            this.dtTImeEnd.Size = new System.Drawing.Size(131, 21);
            this.dtTImeEnd.TabIndex = 75;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(188, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 74;
            this.label1.Text = "至";
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
            this.yddh,
            this.xdyddwc,
            this.shangpin,
            this.xinghao});
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
            this.dataGridView1.TabIndex = 68;
            // 
            // dbsj
            // 
            this.dbsj.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dbsj.DataPropertyName = "时间";
            this.dbsj.HeaderText = "时间";
            this.dbsj.Name = "dbsj";
            this.dbsj.ReadOnly = true;
            this.dbsj.Width = 59;
            // 
            // yddh
            // 
            this.yddh.DataPropertyName = "摘要";
            this.yddh.HeaderText = "摘要";
            this.yddh.Name = "yddh";
            this.yddh.ReadOnly = true;
            this.yddh.Width = 400;
            // 
            // xdyddwc
            // 
            this.xdyddwc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.xdyddwc.DataPropertyName = "项目";
            this.xdyddwc.HeaderText = "项目";
            this.xdyddwc.Name = "xdyddwc";
            this.xdyddwc.ReadOnly = true;
            this.xdyddwc.Width = 59;
            // 
            // shangpin
            // 
            this.shangpin.DataPropertyName = "性质";
            this.shangpin.HeaderText = "性质";
            this.shangpin.Name = "shangpin";
            this.shangpin.ReadOnly = true;
            this.shangpin.Width = 400;
            // 
            // xinghao
            // 
            this.xinghao.DataPropertyName = "金额";
            this.xinghao.HeaderText = "金额";
            this.xinghao.Name = "xinghao";
            this.xinghao.ReadOnly = true;
            this.xinghao.Width = 120;
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
            this.ucPager1.TabIndex = 69;
            // 
            // dtTimeStart
            // 
            this.dtTimeStart.CalendarForeColor = System.Drawing.Color.Black;
            this.dtTimeStart.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dtTimeStart.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dtTimeStart.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dtTimeStart.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dtTimeStart.Location = new System.Drawing.Point(51, 6);
            this.dtTimeStart.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dtTimeStart.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dtTimeStart.Name = "dtTimeStart";
            this.dtTimeStart.Size = new System.Drawing.Size(131, 21);
            this.dtTimeStart.TabIndex = 73;
            // 
            // cbxXZ
            // 
            this.cbxXZ.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxXZ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxXZ.DropDownWidth = 260;
            this.cbxXZ.FormattingEnabled = true;
            this.cbxXZ.Items.AddRange(new object[] {
            "银行转商",
            "商转银"});
            this.cbxXZ.Location = new System.Drawing.Point(567, 6);
            this.cbxXZ.Name = "cbxXZ";
            this.cbxXZ.Size = new System.Drawing.Size(170, 22);
            this.cbxXZ.TabIndex = 72;
            this.cbxXZ.SelectionChangeCommitted += new System.EventHandler(this.cbxXZ_SelectionChangeCommitted);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(10, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 71;
            this.label8.Text = "时间：";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(760, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 70;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ucWYPC_C
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.cbxXM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtTImeEnd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.dtTimeStart);
            this.Controls.Add(this.cbxXZ);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucWYPC_C";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucWYPC_C_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Com.Seezt.Skins.BasicButton btnExport;
        private System.Windows.Forms.ComboBox cbxXM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtTImeEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private System.Windows.Forms.DateTimePicker dtTimeStart;
        private System.Windows.Forms.ComboBox cbxXZ;
        private System.Windows.Forms.Label label8;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn dbsj;
        private System.Windows.Forms.DataGridViewTextBoxColumn yddh;
        private System.Windows.Forms.DataGridViewTextBoxColumn xdyddwc;
        private System.Windows.Forms.DataGridViewTextBoxColumn shangpin;
        private System.Windows.Forms.DataGridViewTextBoxColumn xinghao;
        private CMyXls cMyXls1;
    }
}
