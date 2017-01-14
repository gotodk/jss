namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    partial class ucJYFXYDJCX_B
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.交易方账号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.交易方名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.所在区域 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.当前信用等级 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.当前信用积分 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.操作 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.ucSSQY = new 客户端主程序.SubForm.UCCityList();
            this.uctbJYFMC = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.uctbJYFZH = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
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
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.交易方账号,
            this.交易方名称,
            this.所在区域,
            this.当前信用等级,
            this.当前信用积分,
            this.操作});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(10, 36);
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
            this.dataGridView1.Size = new System.Drawing.Size(1008, 492);
            this.dataGridView1.TabIndex = 25;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // 交易方账号
            // 
            this.交易方账号.DataPropertyName = "交易方账号";
            this.交易方账号.HeaderText = "交易方账号";
            this.交易方账号.Name = "交易方账号";
            this.交易方账号.ReadOnly = true;
            this.交易方账号.Width = 150;
            // 
            // 交易方名称
            // 
            this.交易方名称.DataPropertyName = "交易方名称";
            this.交易方名称.HeaderText = "交易方名称";
            this.交易方名称.Name = "交易方名称";
            this.交易方名称.ReadOnly = true;
            this.交易方名称.Width = 200;
            // 
            // 所在区域
            // 
            this.所在区域.DataPropertyName = "所在区域";
            this.所在区域.HeaderText = "所在区域";
            this.所在区域.Name = "所在区域";
            this.所在区域.ReadOnly = true;
            this.所在区域.Width = 150;
            // 
            // 当前信用等级
            // 
            this.当前信用等级.HeaderText = "当前信用等级";
            this.当前信用等级.Name = "当前信用等级";
            this.当前信用等级.ReadOnly = true;
            this.当前信用等级.Width = 130;
            // 
            // 当前信用积分
            // 
            this.当前信用积分.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.当前信用积分.DataPropertyName = "当前信用积分";
            this.当前信用积分.HeaderText = "当前信用积分";
            this.当前信用积分.Name = "当前信用积分";
            this.当前信用积分.ReadOnly = true;
            this.当前信用积分.Width = 107;
            // 
            // 操作
            // 
            this.操作.HeaderText = "操作";
            this.操作.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.操作.Name = "操作";
            this.操作.ReadOnly = true;
            this.操作.Text = "查看详情";
            this.操作.UseColumnTextForLinkValue = true;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(10, 531);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(1028, 29);
            this.ucPager1.TabIndex = 26;
            // 
            // ucSSQY
            // 
            this.ucSSQY.EnabledItem = new bool[] {
        true,
        true,
        true};
            this.ucSSQY.Location = new System.Drawing.Point(480, 7);
            this.ucSSQY.Name = "ucSSQY";
            this.ucSSQY.SelectedItem = new string[] {
        "",
        "",
        ""};
            this.ucSSQY.Size = new System.Drawing.Size(294, 23);
            this.ucSSQY.TabIndex = 151;
            this.ucSSQY.VisibleItem = new bool[] {
        true,
        true,
        true};
            // 
            // uctbJYFMC
            // 
            this.uctbJYFMC.BackColor = System.Drawing.Color.White;
            this.uctbJYFMC.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.uctbJYFMC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctbJYFMC.CanPASTE = true;
            this.uctbJYFMC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uctbJYFMC.ForeColor = System.Drawing.Color.Black;
            this.uctbJYFMC.HotColor = System.Drawing.Color.CornflowerBlue;
            this.uctbJYFMC.Location = new System.Drawing.Point(86, 7);
            this.uctbJYFMC.Name = "uctbJYFMC";
            this.uctbJYFMC.Size = new System.Drawing.Size(120, 23);
            this.uctbJYFMC.TabIndex = 150;
            this.uctbJYFMC.TextNtip = "";
            this.uctbJYFMC.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(417, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 149;
            this.label3.Text = "所属区域：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 148;
            this.label2.Text = "交易方名称：";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(780, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 152;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.BBsearch_Click);
            // 
            // uctbJYFZH
            // 
            this.uctbJYFZH.BackColor = System.Drawing.Color.White;
            this.uctbJYFZH.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.uctbJYFZH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uctbJYFZH.CanPASTE = true;
            this.uctbJYFZH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uctbJYFZH.ForeColor = System.Drawing.Color.Black;
            this.uctbJYFZH.HotColor = System.Drawing.Color.CornflowerBlue;
            this.uctbJYFZH.Location = new System.Drawing.Point(288, 7);
            this.uctbJYFZH.Name = "uctbJYFZH";
            this.uctbJYFZH.Size = new System.Drawing.Size(120, 23);
            this.uctbJYFZH.TabIndex = 154;
            this.uctbJYFZH.TextNtip = "";
            this.uctbJYFZH.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(214, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 153;
            this.label1.Text = "交易方账号：";
            // 
            // ucJYFXYDJCX_B
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.uctbJYFZH);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.ucSSQY);
            this.Controls.Add(this.uctbJYFMC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "ucJYFXYDJCX_B";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucJYFXYDJCX_B_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private UCCityList ucSSQY;
        private UCTextBox uctbJYFMC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private UCTextBox uctbJYFZH;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交易方账号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交易方名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 所在区域;
        private System.Windows.Forms.DataGridViewTextBoxColumn 当前信用等级;
        private System.Windows.Forms.DataGridViewTextBoxColumn 当前信用积分;
        private System.Windows.Forms.DataGridViewLinkColumn 操作;

    }
}
