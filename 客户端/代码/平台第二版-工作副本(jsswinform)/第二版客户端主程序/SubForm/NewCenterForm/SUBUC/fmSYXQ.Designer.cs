namespace 客户端主程序.SubForm
{
    partial class fmSYXQ
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.btnExport = new Com.Seezt.Skins.BasicButton();
            this.btnSearch = new Com.Seezt.Skins.BasicButton();
            this.txtJYFMC = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.cMyXls1 = new 客户端主程序.SubForm.NewCenterForm.CMyXls(this.components);
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panelUC2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelUC5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panelUC2);
            this.panel1.Controls.Add(this.panelUC5);
            this.panel1.Location = new System.Drawing.Point(2, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(633, 440);
            this.panel1.TabIndex = 29;
            // 
            // panelUC2
            // 
            this.panelUC2.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC2.Controls.Add(this.ucPager1);
            this.panelUC2.Controls.Add(this.dataGridView1);
            this.panelUC2.Location = new System.Drawing.Point(3, 52);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.Padding = new System.Windows.Forms.Padding(1);
            this.panelUC2.SetShowBorder = 1;
            this.panelUC2.Size = new System.Drawing.Size(626, 388);
            this.panelUC2.TabIndex = 17;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(3, 357);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(616, 30);
            this.ucPager1.TabIndex = 4;
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
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(1, 1);
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
            this.dataGridView1.Size = new System.Drawing.Size(618, 352);
            this.dataGridView1.TabIndex = 3;
            // 
            // panelUC5
            // 
            this.panelUC5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.btnExport);
            this.panelUC5.Controls.Add(this.btnSearch);
            this.panelUC5.Controls.Add(this.txtJYFMC);
            this.panelUC5.Controls.Add(this.label10);
            this.panelUC5.Location = new System.Drawing.Point(4, 4);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(626, 40);
            this.panelUC5.TabIndex = 16;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExport.Location = new System.Drawing.Point(331, 7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(50, 21);
            this.btnExport.TabIndex = 19;
            this.btnExport.Texts = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSearch.Location = new System.Drawing.Point(257, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 21);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.Texts = "查询";
            this.btnSearch.Click += new System.EventHandler(this.basicButton2_Click);
            // 
            // txtJYFMC
            // 
            this.txtJYFMC.BackColor = System.Drawing.Color.White;
            this.txtJYFMC.BorderColor = System.Drawing.Color.Gray;
            this.txtJYFMC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJYFMC.CanPASTE = true;
            this.txtJYFMC.Counts = 2;
            this.txtJYFMC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJYFMC.ForeColor = System.Drawing.Color.Black;
            this.txtJYFMC.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtJYFMC.Location = new System.Drawing.Point(82, 7);
            this.txtJYFMC.MaxLength = 500;
            this.txtJYFMC.Name = "txtJYFMC";
            this.txtJYFMC.Size = new System.Drawing.Size(131, 23);
            this.txtJYFMC.TabIndex = 16;
            this.txtJYFMC.TextNtip = "";
            this.txtJYFMC.WordWrap = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(6, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "交易方名称：";
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column1.DataPropertyName = "收益产生时间";
            this.Column1.HeaderText = "收益产生时间";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 103;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "交易方名称";
            this.Column2.HeaderText = "交易方名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.DataPropertyName = "交易性质";
            this.Column3.HeaderText = "交易性质";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 79;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "收益金额";
            this.Column4.HeaderText = "收益金额";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "收益累计";
            this.Column5.HeaderText = "收益累计";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // fmSYXQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 472);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmSYXQ";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "收益详情";
            this.Load += new System.EventHandler(this.FormSelectList_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panelUC2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelUC5.ResumeLayout(false);
            this.panelUC5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CenterForm.publicUC.PanelUC panelUC5;
        private Com.Seezt.Skins.BasicButton btnSearch;
        private UCTextBox txtJYFMC;
        private System.Windows.Forms.Label label10;
        private CenterForm.publicUC.PanelUC panelUC2;
        private UCPager ucPager1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Com.Seezt.Skins.BasicButton btnExport;
        private NewCenterForm.CMyXls cMyXls1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}