namespace 客户端主程序.SubForm.CenterForm.Agent
{
    partial class CXyonghuziliaoshenhe
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
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.dtpTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.lblTimeEnd = new System.Windows.Forms.Label();
            this.dtpTimeStart = new System.Windows.Forms.DateTimePicker();
            this.btnQuery = new Com.Seezt.Skins.BasicButton();
            this.txtYHM = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.cbxZCJS = new System.Windows.Forms.ComboBox();
            this.lblZCJS = new System.Windows.Forms.Label();
            this.lblYHM = new System.Windows.Forms.Label();
            this.lblTImeStart = new System.Windows.Forms.Label();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.JSBH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.审核 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.xh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JSLX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yhm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DLYX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lxrxm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sjh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zcsj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SQGLSJ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelUC5.SuspendLayout();
            this.panelUC2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelUC5
            // 
            this.panelUC5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.dtpTimeEnd);
            this.panelUC5.Controls.Add(this.lblTimeEnd);
            this.panelUC5.Controls.Add(this.dtpTimeStart);
            this.panelUC5.Controls.Add(this.btnQuery);
            this.panelUC5.Controls.Add(this.txtYHM);
            this.panelUC5.Controls.Add(this.cbxZCJS);
            this.panelUC5.Controls.Add(this.lblZCJS);
            this.panelUC5.Controls.Add(this.lblYHM);
            this.panelUC5.Controls.Add(this.lblTImeStart);
            this.panelUC5.Location = new System.Drawing.Point(0, 0);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(979, 40);
            this.panelUC5.TabIndex = 15;
            // 
            // dtpTimeEnd
            // 
            this.dtpTimeEnd.CalendarForeColor = System.Drawing.Color.Black;
            this.dtpTimeEnd.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dtpTimeEnd.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dtpTimeEnd.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dtpTimeEnd.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dtpTimeEnd.Location = new System.Drawing.Point(680, 6);
            this.dtpTimeEnd.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dtpTimeEnd.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dtpTimeEnd.Name = "dtpTimeEnd";
            this.dtpTimeEnd.Size = new System.Drawing.Size(131, 21);
            this.dtpTimeEnd.TabIndex = 22;
            // 
            // lblTimeEnd
            // 
            this.lblTimeEnd.AutoSize = true;
            this.lblTimeEnd.ForeColor = System.Drawing.Color.Black;
            this.lblTimeEnd.Location = new System.Drawing.Point(597, 10);
            this.lblTimeEnd.Name = "lblTimeEnd";
            this.lblTimeEnd.Size = new System.Drawing.Size(89, 12);
            this.lblTimeEnd.TabIndex = 21;
            this.lblTimeEnd.Text = "注册结束时间：";
            // 
            // dtpTimeStart
            // 
            this.dtpTimeStart.CalendarForeColor = System.Drawing.Color.Black;
            this.dtpTimeStart.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dtpTimeStart.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dtpTimeStart.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dtpTimeStart.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dtpTimeStart.Location = new System.Drawing.Point(460, 7);
            this.dtpTimeStart.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dtpTimeStart.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dtpTimeStart.Name = "dtpTimeStart";
            this.dtpTimeStart.Size = new System.Drawing.Size(131, 21);
            this.dtpTimeStart.TabIndex = 20;
            this.dtpTimeStart.Value = new System.DateTime(2013, 3, 6, 16, 28, 54, 0);
            // 
            // btnQuery
            // 
            this.btnQuery.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnQuery.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnQuery.Location = new System.Drawing.Point(834, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(50, 22);
            this.btnQuery.TabIndex = 18;
            this.btnQuery.Texts = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtYHM
            // 
            this.txtYHM.BackColor = System.Drawing.Color.White;
            this.txtYHM.BorderColor = System.Drawing.Color.Gray;
            this.txtYHM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYHM.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYHM.ForeColor = System.Drawing.Color.Black;
            this.txtYHM.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtYHM.Location = new System.Drawing.Point(240, 7);
            this.txtYHM.MaxLength = 500;
            this.txtYHM.Name = "txtYHM";
            this.txtYHM.Size = new System.Drawing.Size(131, 23);
            this.txtYHM.TabIndex = 15;
            this.txtYHM.TextNtip = "";
            this.txtYHM.WordWrap = false;
            // 
            // cbxZCJS
            // 
            this.cbxZCJS.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbxZCJS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxZCJS.FormattingEnabled = true;
            this.cbxZCJS.Items.AddRange(new object[] {
            "全部",
            "卖家",
            "买家"});
            this.cbxZCJS.Location = new System.Drawing.Point(66, 7);
            this.cbxZCJS.Name = "cbxZCJS";
            this.cbxZCJS.Size = new System.Drawing.Size(121, 22);
            this.cbxZCJS.TabIndex = 1;
            // 
            // lblZCJS
            // 
            this.lblZCJS.AutoSize = true;
            this.lblZCJS.ForeColor = System.Drawing.Color.Black;
            this.lblZCJS.Location = new System.Drawing.Point(3, 12);
            this.lblZCJS.Name = "lblZCJS";
            this.lblZCJS.Size = new System.Drawing.Size(65, 12);
            this.lblZCJS.TabIndex = 0;
            this.lblZCJS.Text = "注册角色：";
            // 
            // lblYHM
            // 
            this.lblYHM.AutoSize = true;
            this.lblYHM.ForeColor = System.Drawing.Color.Black;
            this.lblYHM.Location = new System.Drawing.Point(193, 12);
            this.lblYHM.Name = "lblYHM";
            this.lblYHM.Size = new System.Drawing.Size(53, 12);
            this.lblYHM.TabIndex = 2;
            this.lblYHM.Text = "用户名：";
            // 
            // lblTImeStart
            // 
            this.lblTImeStart.AutoSize = true;
            this.lblTImeStart.ForeColor = System.Drawing.Color.Black;
            this.lblTImeStart.Location = new System.Drawing.Point(377, 10);
            this.lblTImeStart.Name = "lblTImeStart";
            this.lblTImeStart.Size = new System.Drawing.Size(89, 12);
            this.lblTImeStart.TabIndex = 8;
            this.lblTImeStart.Text = "注册开始时间：";
            // 
            // panelUC2
            // 
            this.panelUC2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC2.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC2.Controls.Add(this.ucPager1);
            this.panelUC2.Controls.Add(this.dataGridView1);
            this.panelUC2.Location = new System.Drawing.Point(0, 50);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.Padding = new System.Windows.Forms.Padding(1);
            this.panelUC2.SetShowBorder = 1;
            this.panelUC2.Size = new System.Drawing.Size(979, 391);
            this.panelUC2.TabIndex = 4;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(3, 360);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(975, 30);
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
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JSBH,
            this.审核,
            this.xh,
            this.JSLX,
            this.yhm,
            this.DLYX,
            this.lxrxm,
            this.sjh,
            this.zcsj,
            this.zt,
            this.SQGLSJ});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
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
            this.dataGridView1.Size = new System.Drawing.Size(977, 355);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // JSBH
            // 
            this.JSBH.DataPropertyName = "角色编号";
            this.JSBH.HeaderText = "JSBH";
            this.JSBH.Name = "JSBH";
            this.JSBH.ReadOnly = true;
            this.JSBH.Visible = false;
            // 
            // 审核
            // 
            this.审核.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.审核.DataPropertyName = "CAOZUO";
            this.审核.HeaderText = "操作";
            this.审核.Name = "审核";
            this.审核.ReadOnly = true;
            this.审核.Text = "审核";
            this.审核.Width = 40;
            // 
            // xh
            // 
            this.xh.DataPropertyName = "ROWID";
            this.xh.HeaderText = "序号";
            this.xh.Name = "xh";
            this.xh.ReadOnly = true;
            this.xh.Visible = false;
            // 
            // JSLX
            // 
            this.JSLX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.JSLX.DataPropertyName = "结算账户";
            this.JSLX.HeaderText = "结算账户";
            this.JSLX.Name = "JSLX";
            this.JSLX.ReadOnly = true;
            this.JSLX.Width = 83;
            // 
            // yhm
            // 
            this.yhm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.yhm.DataPropertyName = "用户名";
            this.yhm.HeaderText = "用户名";
            this.yhm.Name = "yhm";
            this.yhm.ReadOnly = true;
            this.yhm.Width = 71;
            // 
            // DLYX
            // 
            this.DLYX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DLYX.DataPropertyName = "登录邮箱";
            this.DLYX.HeaderText = "邮箱";
            this.DLYX.Name = "DLYX";
            this.DLYX.ReadOnly = true;
            this.DLYX.Width = 59;
            // 
            // lxrxm
            // 
            this.lxrxm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.lxrxm.DataPropertyName = "联系人姓名";
            this.lxrxm.HeaderText = "联系人姓名";
            this.lxrxm.Name = "lxrxm";
            this.lxrxm.ReadOnly = true;
            this.lxrxm.Width = 95;
            // 
            // sjh
            // 
            this.sjh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sjh.DataPropertyName = "手机号";
            this.sjh.HeaderText = "手机号";
            this.sjh.Name = "sjh";
            this.sjh.ReadOnly = true;
            this.sjh.Width = 71;
            // 
            // zcsj
            // 
            this.zcsj.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.zcsj.DataPropertyName = "注册时间";
            this.zcsj.HeaderText = "注册时间";
            this.zcsj.Name = "zcsj";
            this.zcsj.ReadOnly = true;
            this.zcsj.Width = 83;
            // 
            // zt
            // 
            this.zt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.zt.DataPropertyName = "经纪人审核状态";
            this.zt.HeaderText = "状态";
            this.zt.Name = "zt";
            this.zt.ReadOnly = true;
            this.zt.Width = 59;
            // 
            // SQGLSJ
            // 
            this.SQGLSJ.DataPropertyName = "申请关联时间";
            this.SQGLSJ.HeaderText = "SQGLSJ";
            this.SQGLSJ.Name = "SQGLSJ";
            this.SQGLSJ.ReadOnly = true;
            this.SQGLSJ.Visible = false;
            // 
            // CXyonghuziliaoshenhe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUC5);
            this.Controls.Add(this.panelUC2);
            this.Name = "CXyonghuziliaoshenhe";
            this.Size = new System.Drawing.Size(979, 441);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.panelUC5.ResumeLayout(false);
            this.panelUC5.PerformLayout();
            this.panelUC2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private publicUC.PanelUC panelUC2;
        private UCPager ucPager1;
        private publicUC.PanelUC panelUC5;
        private Com.Seezt.Skins.BasicButton btnQuery;
        private UCTextBox txtYHM;
        private System.Windows.Forms.ComboBox cbxZCJS;
        private System.Windows.Forms.Label lblZCJS;
        private System.Windows.Forms.Label lblYHM;
        private System.Windows.Forms.Label lblTImeStart;
        private System.Windows.Forms.DateTimePicker dtpTimeEnd;
        private System.Windows.Forms.Label lblTimeEnd;
        private System.Windows.Forms.DateTimePicker dtpTimeStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSBH;
        private System.Windows.Forms.DataGridViewLinkColumn 审核;
        private System.Windows.Forms.DataGridViewTextBoxColumn xh;
        private System.Windows.Forms.DataGridViewTextBoxColumn JSLX;
        private System.Windows.Forms.DataGridViewTextBoxColumn yhm;
        private System.Windows.Forms.DataGridViewTextBoxColumn DLYX;
        private System.Windows.Forms.DataGridViewTextBoxColumn lxrxm;
        private System.Windows.Forms.DataGridViewTextBoxColumn sjh;
        private System.Windows.Forms.DataGridViewTextBoxColumn zcsj;
        private System.Windows.Forms.DataGridViewTextBoxColumn zt;
        private System.Windows.Forms.DataGridViewTextBoxColumn SQGLSJ;



    }
}
