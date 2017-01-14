namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    partial class ucSYGL
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
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            this.txtFZJG = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtYGGH = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtYGXM = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.分支机构 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.员工工号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.员工姓名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.收益累计 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(686, 7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 22);
            this.btnExport.TabIndex = 91;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(11, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 88;
            this.label2.Text = "分支机构：";
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
            this.分支机构,
            this.员工工号,
            this.员工姓名,
            this.收益累计,
            this.Column1});
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
            this.dataGridView1.TabIndex = 80;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
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
            this.ucPager1.TabIndex = 81;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(626, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 82;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtFZJG
            // 
            this.txtFZJG.BackColor = System.Drawing.Color.White;
            this.txtFZJG.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtFZJG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFZJG.CanPASTE = true;
            this.txtFZJG.Counts = 2;
            this.txtFZJG.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFZJG.ForeColor = System.Drawing.Color.Black;
            this.txtFZJG.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtFZJG.Location = new System.Drawing.Point(75, 8);
            this.txtFZJG.Name = "txtFZJG";
            this.txtFZJG.Size = new System.Drawing.Size(131, 23);
            this.txtFZJG.TabIndex = 92;
            this.txtFZJG.TextNtip = "";
            this.txtFZJG.WordWrap = false;
            // 
            // txtYGGH
            // 
            this.txtYGGH.BackColor = System.Drawing.Color.White;
            this.txtYGGH.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtYGGH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYGGH.CanPASTE = true;
            this.txtYGGH.Counts = 2;
            this.txtYGGH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYGGH.ForeColor = System.Drawing.Color.Black;
            this.txtYGGH.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtYGGH.Location = new System.Drawing.Point(278, 8);
            this.txtYGGH.Name = "txtYGGH";
            this.txtYGGH.Size = new System.Drawing.Size(131, 23);
            this.txtYGGH.TabIndex = 94;
            this.txtYGGH.TextNtip = "";
            this.txtYGGH.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(214, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 93;
            this.label1.Text = "员工工号：";
            // 
            // txtYGXM
            // 
            this.txtYGXM.BackColor = System.Drawing.Color.White;
            this.txtYGXM.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtYGXM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYGXM.CanPASTE = true;
            this.txtYGXM.Counts = 2;
            this.txtYGXM.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYGXM.ForeColor = System.Drawing.Color.Black;
            this.txtYGXM.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtYGXM.Location = new System.Drawing.Point(478, 9);
            this.txtYGXM.Name = "txtYGXM";
            this.txtYGXM.Size = new System.Drawing.Size(131, 23);
            this.txtYGXM.TabIndex = 96;
            this.txtYGXM.TextNtip = "";
            this.txtYGXM.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(414, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 95;
            this.label3.Text = "员工姓名：";
            // 
            // 分支机构
            // 
            this.分支机构.DataPropertyName = "银行人员信息表员工隶属机构";
            this.分支机构.HeaderText = "分支机构";
            this.分支机构.Name = "分支机构";
            this.分支机构.ReadOnly = true;
            this.分支机构.Width = 200;
            // 
            // 员工工号
            // 
            this.员工工号.DataPropertyName = "中标定标信息表辅助表员工工号";
            this.员工工号.HeaderText = "员工工号";
            this.员工工号.Name = "员工工号";
            this.员工工号.ReadOnly = true;
            this.员工工号.Width = 200;
            // 
            // 员工姓名
            // 
            this.员工姓名.DataPropertyName = "银行人员信息表员工姓名";
            this.员工姓名.HeaderText = "员工姓名";
            this.员工姓名.Name = "员工姓名";
            this.员工姓名.ReadOnly = true;
            this.员工姓名.Width = 150;
            // 
            // 收益累计
            // 
            this.收益累计.DataPropertyName = "收益累计";
            this.收益累计.HeaderText = "收益累计";
            this.收益累计.Name = "收益累计";
            this.收益累计.ReadOnly = true;
            this.收益累计.Width = 120;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "操作";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Text = "收益详情";
            this.Column1.UseColumnTextForLinkValue = true;
            // 
            // ucSYGL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.txtYGXM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtYGGH);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFZJG);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ucPager1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucSYGL";
            this.Size = new System.Drawing.Size(1031, 564);
            this.Load += new System.EventHandler(this.ucJJRSY_C_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Com.Seezt.Skins.BasicButton btnExport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private CMyXls cMyXls1;
        private UCTextBox txtFZJG;
        private UCTextBox txtYGGH;
        private System.Windows.Forms.Label label1;
        private UCTextBox txtYGXM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 分支机构;
        private System.Windows.Forms.DataGridViewTextBoxColumn 员工工号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 员工姓名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 收益累计;
        private System.Windows.Forms.DataGridViewLinkColumn Column1;
    }
}
