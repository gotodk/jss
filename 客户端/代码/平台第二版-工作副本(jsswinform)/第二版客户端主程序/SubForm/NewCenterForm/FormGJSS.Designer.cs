namespace 客户端主程序.SubForm.NewCenterForm
{
    partial class FormGJSS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelUCedit = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelresult = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.详情及操作 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.商品编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.商品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.型号规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.一级分类 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同结束日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.合同期限 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.计价单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.主键 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.panelserach = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.cbHTQX = new System.Windows.Forms.ComboBox();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.cbSSFL = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSPMC = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.txtSPBH = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtGGXH = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.lbssfl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelUCedit.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelresult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelserach.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelUCedit
            // 
            this.panelUCedit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUCedit.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUCedit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUCedit.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUCedit.Controls.Add(this.panel1);
            this.panelUCedit.Location = new System.Drawing.Point(2, 30);
            this.panelUCedit.Margin = new System.Windows.Forms.Padding(0);
            this.panelUCedit.Name = "panelUCedit";
            this.panelUCedit.SetShowBorder = 1;
            this.panelUCedit.Size = new System.Drawing.Size(816, 568);
            this.panelUCedit.TabIndex = 29;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.panelresult);
            this.panel1.Controls.Add(this.panelserach);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(816, 568);
            this.panel1.TabIndex = 0;
            // 
            // panelresult
            // 
            this.panelresult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelresult.BackColor = System.Drawing.Color.AliceBlue;
            this.panelresult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelresult.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelresult.Controls.Add(this.dataGridView1);
            this.panelresult.Controls.Add(this.ucPager1);
            this.panelresult.Location = new System.Drawing.Point(4, 82);
            this.panelresult.Margin = new System.Windows.Forms.Padding(0);
            this.panelresult.Name = "panelresult";
            this.panelresult.Padding = new System.Windows.Forms.Padding(2);
            this.panelresult.SetShowBorder = 1;
            this.panelresult.Size = new System.Drawing.Size(808, 479);
            this.panelresult.TabIndex = 142;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.详情及操作,
            this.商品编号,
            this.商品名称,
            this.型号规格,
            this.一级分类,
            this.合同结束日期,
            this.合同期限,
            this.计价单位,
            this.主键});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(2, 2);
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
            this.dataGridView1.Size = new System.Drawing.Size(804, 441);
            this.dataGridView1.TabIndex = 130;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // 详情及操作
            // 
            this.详情及操作.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.详情及操作.FillWeight = 9.152957F;
            this.详情及操作.HeaderText = "操作";
            this.详情及操作.LinkColor = System.Drawing.Color.RoyalBlue;
            this.详情及操作.Name = "详情及操作";
            this.详情及操作.ReadOnly = true;
            this.详情及操作.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.详情及操作.Text = "查看详情";
            this.详情及操作.UseColumnTextForLinkValue = true;
            this.详情及操作.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.详情及操作.Width = 40;
            // 
            // 商品编号
            // 
            this.商品编号.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.商品编号.DataPropertyName = "商品编号";
            this.商品编号.FillWeight = 349.2386F;
            this.商品编号.HeaderText = "商品编号";
            this.商品编号.Name = "商品编号";
            this.商品编号.ReadOnly = true;
            this.商品编号.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.商品编号.Width = 83;
            // 
            // 商品名称
            // 
            this.商品名称.DataPropertyName = "商品名称";
            this.商品名称.FillWeight = 201.0807F;
            this.商品名称.HeaderText = "商品名称";
            this.商品名称.Name = "商品名称";
            this.商品名称.ReadOnly = true;
            this.商品名称.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // 型号规格
            // 
            this.型号规格.DataPropertyName = "型号规格";
            this.型号规格.FillWeight = 263.9159F;
            this.型号规格.HeaderText = "规格标准";
            this.型号规格.Name = "型号规格";
            this.型号规格.ReadOnly = true;
            this.型号规格.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // 一级分类
            // 
            this.一级分类.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.一级分类.DataPropertyName = "一级分类";
            this.一级分类.FillWeight = 9.152957F;
            this.一级分类.HeaderText = "一级分类";
            this.一级分类.Name = "一级分类";
            this.一级分类.ReadOnly = true;
            this.一级分类.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.一级分类.Width = 83;
            // 
            // 合同结束日期
            // 
            this.合同结束日期.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同结束日期.DataPropertyName = "二级分类";
            this.合同结束日期.FillWeight = 9.152957F;
            this.合同结束日期.HeaderText = "二级分类";
            this.合同结束日期.Name = "合同结束日期";
            this.合同结束日期.ReadOnly = true;
            this.合同结束日期.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.合同结束日期.Width = 83;
            // 
            // 合同期限
            // 
            this.合同期限.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.合同期限.DataPropertyName = "合同期限";
            this.合同期限.FillWeight = 9.152957F;
            this.合同期限.HeaderText = "合同期限";
            this.合同期限.Name = "合同期限";
            this.合同期限.ReadOnly = true;
            this.合同期限.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.合同期限.Width = 83;
            // 
            // 计价单位
            // 
            this.计价单位.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.计价单位.DataPropertyName = "计价单位";
            this.计价单位.FillWeight = 9.152957F;
            this.计价单位.HeaderText = "计价单位";
            this.计价单位.Name = "计价单位";
            this.计价单位.ReadOnly = true;
            this.计价单位.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.计价单位.Width = 83;
            // 
            // 主键
            // 
            this.主键.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.主键.DataPropertyName = "主键";
            this.主键.HeaderText = "主键";
            this.主键.Name = "主键";
            this.主键.ReadOnly = true;
            this.主键.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.主键.Visible = false;
            this.主键.Width = 60;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(9, 445);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(790, 29);
            this.ucPager1.TabIndex = 131;
            // 
            // panelserach
            // 
            this.panelserach.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelserach.BackColor = System.Drawing.Color.AliceBlue;
            this.panelserach.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelserach.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelserach.Controls.Add(this.cbHTQX);
            this.panelserach.Controls.Add(this.btnSearch);
            this.panelserach.Controls.Add(this.cbSSFL);
            this.panelserach.Controls.Add(this.label2);
            this.panelserach.Controls.Add(this.label4);
            this.panelserach.Controls.Add(this.txtSPMC);
            this.panelserach.Controls.Add(this.label3);
            this.panelserach.Controls.Add(this.txtSPBH);
            this.panelserach.Controls.Add(this.txtGGXH);
            this.panelserach.Controls.Add(this.lbssfl);
            this.panelserach.Controls.Add(this.label1);
            this.panelserach.Location = new System.Drawing.Point(4, 4);
            this.panelserach.Margin = new System.Windows.Forms.Padding(0);
            this.panelserach.Name = "panelserach";
            this.panelserach.SetShowBorder = 1;
            this.panelserach.Size = new System.Drawing.Size(808, 70);
            this.panelserach.TabIndex = 141;
            // 
            // cbHTQX
            // 
            this.cbHTQX.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbHTQX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHTQX.FormattingEnabled = true;
            this.cbHTQX.Items.AddRange(new object[] {
            "全部",
            "即时",
            "三个月",
            "一年"});
            this.cbHTQX.Location = new System.Drawing.Point(381, 8);
            this.cbHTQX.Name = "cbHTQX";
            this.cbHTQX.Size = new System.Drawing.Size(150, 22);
            this.cbHTQX.TabIndex = 137;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(722, 40);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 22);
            this.btnSearch.TabIndex = 127;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSSFL
            // 
            this.cbSSFL.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbSSFL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSSFL.FormattingEnabled = true;
            this.cbSSFL.Items.AddRange(new object[] {
            "所属分类"});
            this.cbSSFL.Location = new System.Drawing.Point(77, 8);
            this.cbSSFL.Name = "cbSSFL";
            this.cbSSFL.Size = new System.Drawing.Size(222, 22);
            this.cbSSFL.TabIndex = 140;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(215, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 128;
            this.label2.Text = "商品名称：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(447, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 139;
            this.label4.Text = "规格标准：";
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
            this.txtSPMC.Location = new System.Drawing.Point(283, 39);
            this.txtSPMC.Name = "txtSPMC";
            this.txtSPMC.Size = new System.Drawing.Size(150, 23);
            this.txtSPMC.TabIndex = 129;
            this.txtSPMC.TextNtip = "";
            this.txtSPMC.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(11, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 138;
            this.label3.Text = "商品编号：";
            // 
            // txtSPBH
            // 
            this.txtSPBH.BackColor = System.Drawing.Color.White;
            this.txtSPBH.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtSPBH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSPBH.CanPASTE = true;
            this.txtSPBH.Counts = 2;
            this.txtSPBH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSPBH.ForeColor = System.Drawing.Color.Black;
            this.txtSPBH.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtSPBH.Location = new System.Drawing.Point(77, 40);
            this.txtSPBH.Name = "txtSPBH";
            this.txtSPBH.Size = new System.Drawing.Size(124, 23);
            this.txtSPBH.TabIndex = 132;
            this.txtSPBH.TextNtip = "";
            this.txtSPBH.WordWrap = false;
            // 
            // txtGGXH
            // 
            this.txtGGXH.BackColor = System.Drawing.Color.White;
            this.txtGGXH.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtGGXH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGGXH.CanPASTE = true;
            this.txtGGXH.Counts = 2;
            this.txtGGXH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGGXH.ForeColor = System.Drawing.Color.Black;
            this.txtGGXH.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtGGXH.Location = new System.Drawing.Point(515, 40);
            this.txtGGXH.Name = "txtGGXH";
            this.txtGGXH.Size = new System.Drawing.Size(150, 23);
            this.txtGGXH.TabIndex = 133;
            this.txtGGXH.TextNtip = "";
            this.txtGGXH.WordWrap = false;
            // 
            // lbssfl
            // 
            this.lbssfl.AutoSize = true;
            this.lbssfl.ForeColor = System.Drawing.Color.Black;
            this.lbssfl.Location = new System.Drawing.Point(11, 13);
            this.lbssfl.Name = "lbssfl";
            this.lbssfl.Size = new System.Drawing.Size(65, 12);
            this.lbssfl.TabIndex = 136;
            this.lbssfl.Text = "所属分类：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(315, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 134;
            this.label1.Text = "合同期限：";
            // 
            // FormGJSS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(820, 600);
            this.Controls.Add(this.panelUCedit);
            this.MaximizeBox = false;
            this.Name = "FormGJSS";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "高级搜索";
            this.Load += new System.EventHandler(this.FormGJSS_Load);
            this.Controls.SetChildIndex(this.panelUCedit, 0);
            this.panelUCedit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelresult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelserach.ResumeLayout(false);
            this.panelserach.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private 客户端主程序.SubForm.CenterForm.publicUC.PanelUC panelUCedit;
        private System.Windows.Forms.Panel panel1;
        private UCTextBox txtSPMC;
        private System.Windows.Forms.Label label2;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private System.Windows.Forms.Label label1;
        private UCTextBox txtGGXH;
        private UCTextBox txtSPBH;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbHTQX;
        private System.Windows.Forms.Label lbssfl;
        private System.Windows.Forms.ComboBox cbSSFL;
        private CenterForm.publicUC.PanelUC panelresult;
        private CenterForm.publicUC.PanelUC panelserach;
        private System.Windows.Forms.DataGridViewLinkColumn 详情及操作;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 商品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 型号规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 一级分类;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同结束日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合同期限;
        private System.Windows.Forms.DataGridViewTextBoxColumn 计价单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 主键;
    }
}