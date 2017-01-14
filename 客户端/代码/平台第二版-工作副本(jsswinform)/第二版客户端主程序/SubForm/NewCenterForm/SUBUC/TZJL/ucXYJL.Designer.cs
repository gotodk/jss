namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.TZJL
{
    partial class ucXYJL
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbsm = new System.Windows.Forms.Label();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.dtTImeEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtTimeStart = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.lbdqxyjf = new System.Windows.Forms.Label();
            this.dataGridViewfs = new System.Windows.Forms.DataGridView();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.record = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.variablevalue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finalvalue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewfs)).BeginInit();
            this.SuspendLayout();
            // 
            // lbsm
            // 
            this.lbsm.AutoSize = true;
            this.lbsm.ForeColor = System.Drawing.Color.Black;
            this.lbsm.Location = new System.Drawing.Point(17, 12);
            this.lbsm.Name = "lbsm";
            this.lbsm.Size = new System.Drawing.Size(89, 12);
            this.lbsm.TabIndex = 149;
            this.lbsm.Text = "当前信用积分：";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(518, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 148;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
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
            this.ucPager1.TabIndex = 147;
            // 
            // dtTImeEnd
            // 
            this.dtTImeEnd.CalendarForeColor = System.Drawing.Color.Black;
            this.dtTImeEnd.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dtTImeEnd.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dtTImeEnd.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dtTImeEnd.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dtTImeEnd.Location = new System.Drawing.Point(361, 8);
            this.dtTImeEnd.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dtTImeEnd.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dtTImeEnd.Name = "dtTImeEnd";
            this.dtTImeEnd.Size = new System.Drawing.Size(131, 21);
            this.dtTImeEnd.TabIndex = 156;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(338, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 155;
            this.label1.Text = "至";
            // 
            // dtTimeStart
            // 
            this.dtTimeStart.CalendarForeColor = System.Drawing.Color.Black;
            this.dtTimeStart.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dtTimeStart.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dtTimeStart.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dtTimeStart.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dtTimeStart.Location = new System.Drawing.Point(200, 8);
            this.dtTimeStart.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dtTimeStart.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dtTimeStart.Name = "dtTimeStart";
            this.dtTimeStart.Size = new System.Drawing.Size(131, 21);
            this.dtTimeStart.TabIndex = 154;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(156, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 153;
            this.label8.Text = "时间：";
            // 
            // lbdqxyjf
            // 
            this.lbdqxyjf.AutoSize = true;
            this.lbdqxyjf.Location = new System.Drawing.Point(105, 12);
            this.lbdqxyjf.Name = "lbdqxyjf";
            this.lbdqxyjf.Size = new System.Drawing.Size(0, 12);
            this.lbdqxyjf.TabIndex = 158;
            // 
            // dataGridViewfs
            // 
            this.dataGridViewfs.AllowUserToAddRows = false;
            this.dataGridViewfs.AllowUserToDeleteRows = false;
            this.dataGridViewfs.AllowUserToResizeRows = false;
            this.dataGridViewfs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewfs.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dataGridViewfs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewfs.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewfs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewfs.ColumnHeadersHeight = 25;
            this.dataGridViewfs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewfs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time,
            this.record,
            this.variablevalue,
            this.finalvalue});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewfs.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewfs.EnableHeadersVisualStyles = false;
            this.dataGridViewfs.GridColor = System.Drawing.Color.Silver;
            this.dataGridViewfs.Location = new System.Drawing.Point(10, 35);
            this.dataGridViewfs.Name = "dataGridViewfs";
            this.dataGridViewfs.ReadOnly = true;
            this.dataGridViewfs.RowHeadersVisible = false;
            this.dataGridViewfs.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewfs.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewfs.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewfs.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridViewfs.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewfs.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewfs.RowTemplate.Height = 23;
            this.dataGridViewfs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewfs.Size = new System.Drawing.Size(1010, 492);
            this.dataGridViewfs.TabIndex = 159;
            // 
            // time
            // 
            this.time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.time.DataPropertyName = "cjsj";
            this.time.HeaderText = "产生时间";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            this.time.Width = 83;
            // 
            // record
            // 
            this.record.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.record.DataPropertyName = "ys";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.record.DefaultCellStyle = dataGridViewCellStyle2;
            this.record.HeaderText = "信用等级评估记录";
            this.record.Name = "record";
            this.record.ReadOnly = true;
            // 
            // variablevalue
            // 
            this.variablevalue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.variablevalue.DataPropertyName = "fs";
            this.variablevalue.HeaderText = "加减分值";
            this.variablevalue.Name = "variablevalue";
            this.variablevalue.ReadOnly = true;
            this.variablevalue.Width = 83;
            // 
            // finalvalue
            // 
            this.finalvalue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.finalvalue.DataPropertyName = "xyjf";
            this.finalvalue.HeaderText = "信用积分";
            this.finalvalue.Name = "finalvalue";
            this.finalvalue.ReadOnly = true;
            this.finalvalue.Width = 83;
            // 
            // ucXYJL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.dataGridViewfs);
            this.Controls.Add(this.lbdqxyjf);
            this.Controls.Add(this.dtTImeEnd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtTimeStart);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lbsm);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucXYJL";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucXYJL_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewfs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbsm;
        private UCPager ucPager1;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private System.Windows.Forms.DateTimePicker dtTImeEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtTimeStart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbdqxyjf;
        private System.Windows.Forms.DataGridView dataGridViewfs;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn record;
        private System.Windows.Forms.DataGridViewTextBoxColumn variablevalue;
        private System.Windows.Forms.DataGridViewTextBoxColumn finalvalue;

    }
}
