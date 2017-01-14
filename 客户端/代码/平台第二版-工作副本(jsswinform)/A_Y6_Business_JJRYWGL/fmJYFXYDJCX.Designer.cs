namespace 客户端主程序.SubForm
{
    partial class fmJYFXYDJCX
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDQJF = new System.Windows.Forms.Label();
            this.lblJYFMC = new System.Windows.Forms.Label();
            this.lblJYFZH = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dbsj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yddh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jjfz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xyjf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.dtTImeEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtTimeStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.basicButton2 = new Com.Seezt.Skins.BasicButton();
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
            this.panel1.Controls.Add(this.lblDQJF);
            this.panel1.Controls.Add(this.lblJYFMC);
            this.panel1.Controls.Add(this.lblJYFZH);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.panelUC2);
            this.panel1.Controls.Add(this.panelUC5);
            this.panel1.Location = new System.Drawing.Point(2, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 486);
            this.panel1.TabIndex = 29;
            // 
            // lblDQJF
            // 
            this.lblDQJF.AutoSize = true;
            this.lblDQJF.ForeColor = System.Drawing.Color.Black;
            this.lblDQJF.Location = new System.Drawing.Point(677, 57);
            this.lblDQJF.Name = "lblDQJF";
            this.lblDQJF.Size = new System.Drawing.Size(53, 12);
            this.lblDQJF.TabIndex = 61;
            this.lblDQJF.Text = "当前积分";
            // 
            // lblJYFMC
            // 
            this.lblJYFMC.AutoSize = true;
            this.lblJYFMC.ForeColor = System.Drawing.Color.Black;
            this.lblJYFMC.Location = new System.Drawing.Point(293, 57);
            this.lblJYFMC.Name = "lblJYFMC";
            this.lblJYFMC.Size = new System.Drawing.Size(65, 12);
            this.lblJYFMC.TabIndex = 60;
            this.lblJYFMC.Text = "交易方名称";
            // 
            // lblJYFZH
            // 
            this.lblJYFZH.AutoSize = true;
            this.lblJYFZH.ForeColor = System.Drawing.Color.Black;
            this.lblJYFZH.Location = new System.Drawing.Point(78, 57);
            this.lblJYFZH.Name = "lblJYFZH";
            this.lblJYFZH.Size = new System.Drawing.Size(65, 12);
            this.lblJYFZH.TabIndex = 59;
            this.lblJYFZH.Text = "交易方账号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(596, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 58;
            this.label5.Text = "当前信用积分：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(224, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 57;
            this.label4.Text = "交易方名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 56;
            this.label3.Text = "交易方账号：";
            // 
            // panelUC2
            // 
            this.panelUC2.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC2.Controls.Add(this.dataGridView1);
            this.panelUC2.Controls.Add(this.ucPager1);
            this.panelUC2.Location = new System.Drawing.Point(0, 79);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.Padding = new System.Windows.Forms.Padding(1);
            this.panelUC2.SetShowBorder = 1;
            this.panelUC2.Size = new System.Drawing.Size(785, 404);
            this.panelUC2.TabIndex = 17;
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
            this.jjfz,
            this.xyjf});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
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
            this.dataGridView1.Size = new System.Drawing.Size(784, 367);
            this.dataGridView1.TabIndex = 147;
            // 
            // dbsj
            // 
            this.dbsj.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dbsj.DataPropertyName = "cjsj";
            this.dbsj.HeaderText = "产生时间";
            this.dbsj.Name = "dbsj";
            this.dbsj.ReadOnly = true;
            this.dbsj.Width = 150;
            // 
            // yddh
            // 
            this.yddh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.yddh.DataPropertyName = "ys";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.yddh.DefaultCellStyle = dataGridViewCellStyle2;
            this.yddh.HeaderText = "信用等级评估记录";
            this.yddh.Name = "yddh";
            this.yddh.ReadOnly = true;
            // 
            // jjfz
            // 
            this.jjfz.DataPropertyName = "fs";
            this.jjfz.HeaderText = "加减分值";
            this.jjfz.Name = "jjfz";
            this.jjfz.ReadOnly = true;
            // 
            // xyjf
            // 
            this.xyjf.DataPropertyName = "xyjf";
            this.xyjf.HeaderText = "信用积分";
            this.xyjf.Name = "xyjf";
            this.xyjf.ReadOnly = true;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(3, 373);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(781, 30);
            this.ucPager1.TabIndex = 4;
            // 
            // panelUC5
            // 
            this.panelUC5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.dtTImeEnd);
            this.panelUC5.Controls.Add(this.label1);
            this.panelUC5.Controls.Add(this.dtTimeStart);
            this.panelUC5.Controls.Add(this.label2);
            this.panelUC5.Controls.Add(this.basicButton2);
            this.panelUC5.Location = new System.Drawing.Point(0, 4);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(785, 40);
            this.panelUC5.TabIndex = 16;
            // 
            // dtTImeEnd
            // 
            this.dtTImeEnd.CalendarForeColor = System.Drawing.Color.Black;
            this.dtTImeEnd.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dtTImeEnd.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dtTImeEnd.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dtTImeEnd.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dtTImeEnd.Location = new System.Drawing.Point(208, 11);
            this.dtTImeEnd.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dtTImeEnd.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dtTImeEnd.Name = "dtTImeEnd";
            this.dtTImeEnd.Size = new System.Drawing.Size(131, 21);
            this.dtTImeEnd.TabIndex = 55;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(185, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 54;
            this.label1.Text = "至";
            // 
            // dtTimeStart
            // 
            this.dtTimeStart.CalendarForeColor = System.Drawing.Color.Black;
            this.dtTimeStart.CalendarMonthBackground = System.Drawing.Color.LightGoldenrodYellow;
            this.dtTimeStart.CalendarTitleBackColor = System.Drawing.Color.CadetBlue;
            this.dtTimeStart.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.dtTimeStart.CalendarTrailingForeColor = System.Drawing.Color.Gray;
            this.dtTimeStart.Location = new System.Drawing.Point(48, 11);
            this.dtTimeStart.MaxDate = new System.DateTime(2030, 12, 1, 0, 0, 0, 0);
            this.dtTimeStart.MinDate = new System.DateTime(2012, 12, 1, 0, 0, 0, 0);
            this.dtTimeStart.Name = "dtTimeStart";
            this.dtTimeStart.Size = new System.Drawing.Size(131, 21);
            this.dtTimeStart.TabIndex = 53;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(7, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 52;
            this.label2.Text = "时间：";
            // 
            // basicButton2
            // 
            this.basicButton2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton2.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton2.Location = new System.Drawing.Point(360, 11);
            this.basicButton2.Name = "basicButton2";
            this.basicButton2.Size = new System.Drawing.Size(50, 21);
            this.basicButton2.TabIndex = 18;
            this.basicButton2.Texts = "查询";
            this.basicButton2.Click += new System.EventHandler(this.BBsearch_Click);
            // 
            // fmJYFXYDJCX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 518);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmJYFXYDJCX";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "交易方信用等级详情";
            this.Load += new System.EventHandler(this.fmJYFXYDJCX_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private CenterForm.publicUC.PanelUC panelUC2;
        private UCPager ucPager1;
        private System.Windows.Forms.DateTimePicker dtTImeEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtTimeStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDQJF;
        private System.Windows.Forms.Label lblJYFMC;
        private System.Windows.Forms.Label lblJYFZH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dbsj;
        private System.Windows.Forms.DataGridViewTextBoxColumn yddh;
        private System.Windows.Forms.DataGridViewTextBoxColumn jjfz;
        private System.Windows.Forms.DataGridViewTextBoxColumn xyjf;
    }
}