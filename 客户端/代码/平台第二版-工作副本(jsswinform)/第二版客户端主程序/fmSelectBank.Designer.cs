﻿namespace 客户端主程序.SubForm
{
    partial class fmSelectBank
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmSelectBank));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.basicButton2 = new Com.Seezt.Skins.BasicButton();
            this.txtBankName = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.银行登录邮箱 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.银行名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.创建时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.经纪人资格证书 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.交易方名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.联系电话 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.经纪人分类 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.panel1.Size = new System.Drawing.Size(551, 358);
            this.panel1.TabIndex = 29;
            // 
            // panelUC2
            // 
            this.panelUC2.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC2.Controls.Add(this.ucPager1);
            this.panelUC2.Controls.Add(this.dataGridView1);
            this.panelUC2.Location = new System.Drawing.Point(4, 52);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.Padding = new System.Windows.Forms.Padding(1);
            this.panelUC2.SetShowBorder = 1;
            this.panelUC2.Size = new System.Drawing.Size(543, 302);
            this.panelUC2.TabIndex = 17;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(3, 262);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(534, 30);
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
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
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
            this.银行登录邮箱,
            this.银行名称,
            this.创建时间,
            this.经纪人资格证书,
            this.交易方名称,
            this.联系电话,
            this.经纪人分类});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(1, 1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(541, 259);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseEnter);
            // 
            // panelUC5
            // 
            this.panelUC5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.basicButton2);
            this.panelUC5.Controls.Add(this.txtBankName);
            this.panelUC5.Controls.Add(this.label10);
            this.panelUC5.Location = new System.Drawing.Point(4, 4);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(543, 40);
            this.panelUC5.TabIndex = 16;
            // 
            // basicButton2
            // 
            this.basicButton2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton2.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton2.Location = new System.Drawing.Point(257, 9);
            this.basicButton2.Name = "basicButton2";
            this.basicButton2.Size = new System.Drawing.Size(50, 21);
            this.basicButton2.TabIndex = 18;
            this.basicButton2.Texts = "查询";
            this.basicButton2.Click += new System.EventHandler(this.basicButton2_Click);
            // 
            // txtBankName
            // 
            this.txtBankName.BackColor = System.Drawing.Color.White;
            this.txtBankName.BorderColor = System.Drawing.Color.Gray;
            this.txtBankName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBankName.CanPASTE = true;
            this.txtBankName.Counts = 2;
            this.txtBankName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBankName.ForeColor = System.Drawing.Color.Black;
            this.txtBankName.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtBankName.Location = new System.Drawing.Point(68, 8);
            this.txtBankName.MaxLength = 500;
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(167, 23);
            this.txtBankName.TabIndex = 16;
            this.txtBankName.TextNtip = "";
            this.txtBankName.WordWrap = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(6, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "银行名称：";
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "选择";
            this.Column1.Image = ((System.Drawing.Image)(resources.GetObject("Column1.Image")));
            this.Column1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Column1.MinimumWidth = 45;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 45;
            // 
            // 银行登录邮箱
            // 
            this.银行登录邮箱.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.银行登录邮箱.DataPropertyName = "B_DLYX";
            this.银行登录邮箱.HeaderText = "银行登录邮箱";
            this.银行登录邮箱.Name = "银行登录邮箱";
            this.银行登录邮箱.ReadOnly = true;
            this.银行登录邮箱.Visible = false;
            this.银行登录邮箱.Width = 103;
            // 
            // 银行名称
            // 
            this.银行名称.DataPropertyName = "I_JYFMC";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.银行名称.DefaultCellStyle = dataGridViewCellStyle3;
            this.银行名称.HeaderText = "银行名称";
            this.银行名称.MinimumWidth = 495;
            this.银行名称.Name = "银行名称";
            this.银行名称.ReadOnly = true;
            // 
            // 创建时间
            // 
            this.创建时间.DataPropertyName = "createtime";
            this.创建时间.HeaderText = "创建时间";
            this.创建时间.Name = "创建时间";
            this.创建时间.ReadOnly = true;
            this.创建时间.Visible = false;
            // 
            // 经纪人资格证书
            // 
            this.经纪人资格证书.DataPropertyName = "J_JJRZGZSBH";
            this.经纪人资格证书.HeaderText = "经纪人资格证书";
            this.经纪人资格证书.Name = "经纪人资格证书";
            this.经纪人资格证书.ReadOnly = true;
            this.经纪人资格证书.Visible = false;
            // 
            // 交易方名称
            // 
            this.交易方名称.DataPropertyName = "I_JYFMC";
            this.交易方名称.HeaderText = "交易方名称";
            this.交易方名称.Name = "交易方名称";
            this.交易方名称.ReadOnly = true;
            this.交易方名称.Visible = false;
            // 
            // 联系电话
            // 
            this.联系电话.DataPropertyName = "I_LXRSJH";
            this.联系电话.HeaderText = "联系电话";
            this.联系电话.Name = "联系电话";
            this.联系电话.ReadOnly = true;
            this.联系电话.Visible = false;
            // 
            // 经纪人分类
            // 
            this.经纪人分类.DataPropertyName = "I_JJRFL";
            this.经纪人分类.HeaderText = "经纪人分类";
            this.经纪人分类.Name = "经纪人分类";
            this.经纪人分类.ReadOnly = true;
            this.经纪人分类.Visible = false;
            // 
            // fmSelectBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 390);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmSelectBank";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择银行名称";
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
        private Com.Seezt.Skins.BasicButton basicButton2;
        private UCTextBox txtBankName;
        private System.Windows.Forms.Label label10;
        private CenterForm.publicUC.PanelUC panelUC2;
        private UCPager ucPager1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 银行登录邮箱;
        private System.Windows.Forms.DataGridViewTextBoxColumn 银行名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 创建时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 经纪人资格证书;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交易方名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 联系电话;
        private System.Windows.Forms.DataGridViewTextBoxColumn 经纪人分类;
    }
}