namespace 客户端主程序.SubForm.CenterForm.Agent
{
    partial class DEMO2
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer_SCCK = new System.Windows.Forms.Timer(this.components);
            this.PBload = new System.Windows.Forms.PictureBox();
            this.panelUC1 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.basicButton1 = new Com.Seezt.Skins.BasicButton();
            this.label2 = new System.Windows.Forms.Label();
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.ResetB = new Com.Seezt.Skins.BasicButton();
            this.Lselect = new System.Windows.Forms.Label();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.B_SCSC = new System.Windows.Forms.Label();
            this.B_SC = new System.Windows.Forms.Label();
            this.B_SCCK = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.bdfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yclj = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wjm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.jsnumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scjg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ucTextBox5 = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.ucTextBox4 = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.ucTextBox3 = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.basicButton2 = new Com.Seezt.Skins.BasicButton();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.panelUC1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelUC5.SuspendLayout();
            this.panelUC2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "图片|*.jpg|其他|*.*";
            this.openFileDialog1.Title = "选择要上传的文件";
            // 
            // timer_SCCK
            // 
            this.timer_SCCK.Enabled = true;
            this.timer_SCCK.Interval = 500;
            this.timer_SCCK.Tick += new System.EventHandler(this.timer_SCCK_Tick);
            // 
            // PBload
            // 
            this.PBload.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBload.Location = new System.Drawing.Point(0, 0);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 44;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // panelUC1
            // 
            this.panelUC1.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC1.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC1.Controls.Add(this.dataGridView1);
            this.panelUC1.Controls.Add(this.panel1);
            this.panelUC1.Controls.Add(this.label2);
            this.panelUC1.Location = new System.Drawing.Point(422, 10);
            this.panelUC1.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC1.Name = "panelUC1";
            this.panelUC1.SetShowBorder = 1;
            this.panelUC1.Size = new System.Drawing.Size(339, 408);
            this.panelUC1.TabIndex = 17;
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
            this.Column4});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(4, 38);
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
            this.dataGridView1.Size = new System.Drawing.Size(331, 315);
            this.dataGridView1.TabIndex = 21;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 80;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.basicButton1);
            this.panel1.Location = new System.Drawing.Point(3, 367);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 38);
            this.panel1.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "时间：xxxxxxxxx";
            // 
            // basicButton1
            // 
            this.basicButton1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton1.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton1.Location = new System.Drawing.Point(244, 9);
            this.basicButton1.Name = "basicButton1";
            this.basicButton1.Size = new System.Drawing.Size(70, 21);
            this.basicButton1.TabIndex = 18;
            this.basicButton1.Texts = "刷新";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(15, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 19;
            this.label2.Text = "投标排名";
            // 
            // panelUC5
            // 
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.ResetB);
            this.panelUC5.Controls.Add(this.Lselect);
            this.panelUC5.Controls.Add(this.panelUC2);
            this.panelUC5.Controls.Add(this.listView1);
            this.panelUC5.Controls.Add(this.ucTextBox5);
            this.panelUC5.Controls.Add(this.ucTextBox4);
            this.panelUC5.Controls.Add(this.ucTextBox3);
            this.panelUC5.Controls.Add(this.label6);
            this.panelUC5.Controls.Add(this.label7);
            this.panelUC5.Controls.Add(this.label1);
            this.panelUC5.Controls.Add(this.basicButton2);
            this.panelUC5.Controls.Add(this.comboBox5);
            this.panelUC5.Controls.Add(this.label8);
            this.panelUC5.Controls.Add(this.label9);
            this.panelUC5.Controls.Add(this.label10);
            this.panelUC5.Location = new System.Drawing.Point(10, 10);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(402, 967);
            this.panelUC5.TabIndex = 16;
            // 
            // ResetB
            // 
            this.ResetB.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ResetB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ResetB.ForeColor = System.Drawing.Color.DarkBlue;
            this.ResetB.Location = new System.Drawing.Point(193, 213);
            this.ResetB.Name = "ResetB";
            this.ResetB.Size = new System.Drawing.Size(50, 21);
            this.ResetB.TabIndex = 38;
            this.ResetB.Texts = "重置";
            this.ResetB.Click += new System.EventHandler(this.ResetB_Click);
            // 
            // Lselect
            // 
            this.Lselect.BackColor = System.Drawing.Color.PowderBlue;
            this.Lselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Lselect.ForeColor = System.Drawing.Color.Black;
            this.Lselect.Location = new System.Drawing.Point(259, 142);
            this.Lselect.Name = "Lselect";
            this.Lselect.Size = new System.Drawing.Size(50, 20);
            this.Lselect.TabIndex = 33;
            this.Lselect.Text = "选择";
            this.Lselect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Lselect.Click += new System.EventHandler(this.Lselect_Click);
            // 
            // panelUC2
            // 
            this.panelUC2.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC2.Controls.Add(this.B_SCSC);
            this.panelUC2.Controls.Add(this.B_SC);
            this.panelUC2.Controls.Add(this.B_SCCK);
            this.panelUC2.Location = new System.Drawing.Point(122, 167);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.SetShowBorder = 0;
            this.panelUC2.Size = new System.Drawing.Size(176, 28);
            this.panelUC2.TabIndex = 18;
            // 
            // B_SCSC
            // 
            this.B_SCSC.BackColor = System.Drawing.Color.PowderBlue;
            this.B_SCSC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.B_SCSC.ForeColor = System.Drawing.Color.Black;
            this.B_SCSC.Location = new System.Drawing.Point(122, 4);
            this.B_SCSC.Name = "B_SCSC";
            this.B_SCSC.Size = new System.Drawing.Size(50, 20);
            this.B_SCSC.TabIndex = 32;
            this.B_SCSC.Text = "删除";
            this.B_SCSC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.B_SCSC.Visible = false;
            this.B_SCSC.Click += new System.EventHandler(this.B_SCSC_Click);
            // 
            // B_SC
            // 
            this.B_SC.BackColor = System.Drawing.Color.PowderBlue;
            this.B_SC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.B_SC.ForeColor = System.Drawing.Color.Black;
            this.B_SC.Location = new System.Drawing.Point(5, 4);
            this.B_SC.Name = "B_SC";
            this.B_SC.Size = new System.Drawing.Size(50, 20);
            this.B_SC.TabIndex = 30;
            this.B_SC.Text = "上传";
            this.B_SC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.B_SC.Click += new System.EventHandler(this.B_SC_Click_1);
            // 
            // B_SCCK
            // 
            this.B_SCCK.BackColor = System.Drawing.Color.PowderBlue;
            this.B_SCCK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.B_SCCK.ForeColor = System.Drawing.Color.Black;
            this.B_SCCK.Location = new System.Drawing.Point(64, 4);
            this.B_SCCK.Name = "B_SCCK";
            this.B_SCCK.Size = new System.Drawing.Size(50, 20);
            this.B_SCCK.TabIndex = 31;
            this.B_SCCK.Text = "查看";
            this.B_SCCK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.B_SCCK.Visible = false;
            this.B_SCCK.Click += new System.EventHandler(this.B_SCCK_Click_1);
            // 
            // listView1
            // 
            this.listView1.AutoArrange = false;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.bdfile,
            this.yclj,
            this.wjm,
            this.jsnumber,
            this.scjg});
            this.listView1.ForeColor = System.Drawing.Color.Black;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(4, 286);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(385, 85);
            this.listView1.TabIndex = 32;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Visible = false;
            // 
            // bdfile
            // 
            this.bdfile.Text = "本地路径";
            this.bdfile.Width = 70;
            // 
            // yclj
            // 
            this.yclj.Text = "远程路径";
            this.yclj.Width = 78;
            // 
            // wjm
            // 
            this.wjm.Text = "原文件名";
            this.wjm.Width = 66;
            // 
            // jsnumber
            // 
            this.jsnumber.Text = "角色编号";
            this.jsnumber.Width = 87;
            // 
            // scjg
            // 
            this.scjg.Text = "上传结果";
            // 
            // ucTextBox5
            // 
            this.ucTextBox5.BackColor = System.Drawing.Color.White;
            this.ucTextBox5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBox5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBox5.ForeColor = System.Drawing.Color.Black;
            this.ucTextBox5.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBox5.Location = new System.Drawing.Point(122, 141);
            this.ucTextBox5.Name = "ucTextBox5";
            this.ucTextBox5.ReadOnly = true;
            this.ucTextBox5.Size = new System.Drawing.Size(131, 23);
            this.ucTextBox5.TabIndex = 29;
            this.ucTextBox5.WordWrap = false;
            // 
            // ucTextBox4
            // 
            this.ucTextBox4.BackColor = System.Drawing.Color.White;
            this.ucTextBox4.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBox4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBox4.ForeColor = System.Drawing.Color.Black;
            this.ucTextBox4.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBox4.Location = new System.Drawing.Point(122, 105);
            this.ucTextBox4.Name = "ucTextBox4";
            this.ucTextBox4.Size = new System.Drawing.Size(131, 23);
            this.ucTextBox4.TabIndex = 28;
            this.ucTextBox4.WordWrap = false;
            // 
            // ucTextBox3
            // 
            this.ucTextBox3.BackColor = System.Drawing.Color.White;
            this.ucTextBox3.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBox3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBox3.ForeColor = System.Drawing.Color.Black;
            this.ucTextBox3.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBox3.Location = new System.Drawing.Point(122, 72);
            this.ucTextBox3.Name = "ucTextBox3";
            this.ucTextBox3.Size = new System.Drawing.Size(131, 23);
            this.ucTextBox3.TabIndex = 27;
            this.ucTextBox3.WordWrap = false;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(20, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "注册角色：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(20, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "资质：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = "半年度/年度投标";
            // 
            // basicButton2
            // 
            this.basicButton2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton2.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton2.Location = new System.Drawing.Point(122, 213);
            this.basicButton2.Name = "basicButton2";
            this.basicButton2.Size = new System.Drawing.Size(50, 21);
            this.basicButton2.TabIndex = 18;
            this.basicButton2.Texts = "提交";
            this.basicButton2.Click += new System.EventHandler(this.basicButton2_Click);
            // 
            // comboBox5
            // 
            this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "全部",
            "经纪人",
            "卖家",
            "买家"});
            this.comboBox5.Location = new System.Drawing.Point(122, 42);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(121, 20);
            this.comboBox5.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(20, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "标书号：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(20, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "投标类型：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(20, 110);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "商品名称：";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DEMO2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PBload);
            this.Controls.Add(this.panelUC1);
            this.Controls.Add(this.panelUC5);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "DEMO2";
            this.Size = new System.Drawing.Size(1065, 989);
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.panelUC1.ResumeLayout(false);
            this.panelUC1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelUC5.ResumeLayout(false);
            this.panelUC5.PerformLayout();
            this.panelUC2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private publicUC.PanelUC panelUC5;
        private Com.Seezt.Skins.BasicButton basicButton2;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private publicUC.PanelUC panelUC1;
        private System.Windows.Forms.Label label2;
        private Com.Seezt.Skins.BasicButton basicButton1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private UCTextBox ucTextBox5;
        private UCTextBox ucTextBox4;
        private UCTextBox ucTextBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label B_SC;
        private System.Windows.Forms.Label B_SCCK;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader bdfile;
        private System.Windows.Forms.ColumnHeader yclj;
        private System.Windows.Forms.ColumnHeader wjm;
        private System.Windows.Forms.ColumnHeader jsnumber;
        private System.Windows.Forms.ColumnHeader scjg;
        private publicUC.PanelUC panelUC2;
        private System.Windows.Forms.Timer timer_SCCK;
        private System.Windows.Forms.Label B_SCSC;
        private System.Windows.Forms.PictureBox PBload;
        private System.Windows.Forms.Label Lselect;
        private Com.Seezt.Skins.BasicButton ResetB;

    }
}
