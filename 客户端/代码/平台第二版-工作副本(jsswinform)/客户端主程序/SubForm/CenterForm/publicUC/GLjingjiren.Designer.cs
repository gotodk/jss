namespace 客户端主程序.SubForm.CenterForm.publicUC
{
    partial class GLjingjiren
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
            this.PBload = new System.Windows.Forms.PictureBox();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.ucTextBox5 = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.panelUC1 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.lb1 = new System.Windows.Forms.Label();
            this.lb5 = new System.Windows.Forms.Label();
            this.lb4 = new System.Windows.Forms.Label();
            this.lb2 = new System.Windows.Forms.Label();
            this.lb3 = new System.Windows.Forms.Label();
            this.lb6 = new System.Windows.Forms.Label();
            this.lb7 = new System.Windows.Forms.Label();
            this.ResetB = new Com.Seezt.Skins.BasicButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnTijiao = new Com.Seezt.Skins.BasicButton();
            this.Lselect = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.申请关联时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.经纪人用户名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.经纪人邮箱 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.联系人姓名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.手机号码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否接收新用户 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.审核状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.默认关联经纪人 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.操作 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.角色登陆账号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelUC1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // PBload
            // 
            this.PBload.Image = global::客户端主程序.Properties.Resources.indicator_medium;
            this.PBload.Location = new System.Drawing.Point(791, 3);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 45;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // bgWorker
            // 
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panel1.Controls.Add(this.ucTextBox5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panelUC1);
            this.panel1.Controls.Add(this.ResetB);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.btnTijiao);
            this.panel1.Controls.Add(this.Lselect);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.SetShowBorder = 1;
            this.panel1.Size = new System.Drawing.Size(979, 313);
            this.panel1.TabIndex = 66;
            // 
            // ucTextBox5
            // 
            this.ucTextBox5.BackColor = System.Drawing.Color.White;
            this.ucTextBox5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.ucTextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBox5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBox5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ucTextBox5.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBox5.Location = new System.Drawing.Point(134, 45);
            this.ucTextBox5.Name = "ucTextBox5";
            this.ucTextBox5.Size = new System.Drawing.Size(254, 23);
            this.ucTextBox5.TabIndex = 60;
            this.ucTextBox5.TextNtip = "请输入准确的经纪人邮箱或用户名";
            this.ucTextBox5.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = "新增关联经纪人";
            // 
            // panelUC1
            // 
            this.panelUC1.BackColor = System.Drawing.Color.White;
            this.panelUC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC1.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC1.Controls.Add(this.lb1);
            this.panelUC1.Controls.Add(this.lb5);
            this.panelUC1.Controls.Add(this.lb4);
            this.panelUC1.Controls.Add(this.lb2);
            this.panelUC1.Controls.Add(this.lb3);
            this.panelUC1.Controls.Add(this.lb6);
            this.panelUC1.Controls.Add(this.lb7);
            this.panelUC1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelUC1.Location = new System.Drawing.Point(134, 79);
            this.panelUC1.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC1.Name = "panelUC1";
            this.panelUC1.SetShowBorder = 1;
            this.panelUC1.Size = new System.Drawing.Size(379, 161);
            this.panelUC1.TabIndex = 58;
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.ForeColor = System.Drawing.Color.Black;
            this.lb1.Location = new System.Drawing.Point(3, 5);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(0, 17);
            this.lb1.TabIndex = 20;
            this.lb1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb5
            // 
            this.lb5.AutoSize = true;
            this.lb5.ForeColor = System.Drawing.Color.Black;
            this.lb5.Location = new System.Drawing.Point(3, 93);
            this.lb5.Name = "lb5";
            this.lb5.Size = new System.Drawing.Size(0, 17);
            this.lb5.TabIndex = 21;
            this.lb5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb4
            // 
            this.lb4.AutoSize = true;
            this.lb4.ForeColor = System.Drawing.Color.Black;
            this.lb4.Location = new System.Drawing.Point(3, 71);
            this.lb4.Name = "lb4";
            this.lb4.Size = new System.Drawing.Size(0, 17);
            this.lb4.TabIndex = 0;
            this.lb4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb2
            // 
            this.lb2.AutoSize = true;
            this.lb2.ForeColor = System.Drawing.Color.Black;
            this.lb2.Location = new System.Drawing.Point(3, 27);
            this.lb2.Name = "lb2";
            this.lb2.Size = new System.Drawing.Size(0, 17);
            this.lb2.TabIndex = 2;
            this.lb2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb3
            // 
            this.lb3.AutoSize = true;
            this.lb3.ForeColor = System.Drawing.Color.Black;
            this.lb3.Location = new System.Drawing.Point(3, 49);
            this.lb3.Name = "lb3";
            this.lb3.Size = new System.Drawing.Size(0, 17);
            this.lb3.TabIndex = 8;
            this.lb3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb6
            // 
            this.lb6.AutoSize = true;
            this.lb6.ForeColor = System.Drawing.Color.Black;
            this.lb6.Location = new System.Drawing.Point(3, 115);
            this.lb6.Name = "lb6";
            this.lb6.Size = new System.Drawing.Size(0, 17);
            this.lb6.TabIndex = 8;
            this.lb6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb7
            // 
            this.lb7.AutoSize = true;
            this.lb7.ForeColor = System.Drawing.Color.Black;
            this.lb7.Location = new System.Drawing.Point(3, 137);
            this.lb7.Name = "lb7";
            this.lb7.Size = new System.Drawing.Size(0, 17);
            this.lb7.TabIndex = 21;
            this.lb7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ResetB
            // 
            this.ResetB.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ResetB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ResetB.ForeColor = System.Drawing.Color.DarkBlue;
            this.ResetB.Location = new System.Drawing.Point(212, 281);
            this.ResetB.Name = "ResetB";
            this.ResetB.Size = new System.Drawing.Size(50, 22);
            this.ResetB.TabIndex = 38;
            this.ResetB.Texts = "取消";
            this.ResetB.Click += new System.EventHandler(this.ResetB_Click);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(71, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 12);
            this.label2.TabIndex = 57;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(57, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "经纪人信息：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTijiao
            // 
            this.btnTijiao.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnTijiao.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTijiao.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnTijiao.Location = new System.Drawing.Point(131, 281);
            this.btnTijiao.Name = "btnTijiao";
            this.btnTijiao.Size = new System.Drawing.Size(50, 22);
            this.btnTijiao.TabIndex = 18;
            this.btnTijiao.Texts = "保存";
            this.btnTijiao.Click += new System.EventHandler(this.btnTijiao_Click);
            // 
            // Lselect
            // 
            this.Lselect.BackColor = System.Drawing.Color.PowderBlue;
            this.Lselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Lselect.ForeColor = System.Drawing.Color.Black;
            this.Lselect.Location = new System.Drawing.Point(400, 47);
            this.Lselect.Name = "Lselect";
            this.Lselect.Size = new System.Drawing.Size(50, 20);
            this.Lselect.TabIndex = 33;
            this.Lselect.Text = "确定";
            this.Lselect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Lselect.Click += new System.EventHandler(this.Lselect_Click);
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(42, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "经纪人：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.ucPager1);
            this.panel2.Location = new System.Drawing.Point(0, 323);
            this.panel2.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.SetShowBorder = 1;
            this.panel2.Size = new System.Drawing.Size(979, 251);
            this.panel2.TabIndex = 65;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(20, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 14);
            this.label4.TabIndex = 60;
            this.label4.Text = "已保存的关联经纪人";
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
            this.编号,
            this.申请关联时间,
            this.经纪人用户名,
            this.经纪人邮箱,
            this.联系人姓名,
            this.手机号码,
            this.是否接收新用户,
            this.审核状态,
            this.默认关联经纪人,
            this.操作,
            this.角色登陆账号});
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
            this.dataGridView1.Location = new System.Drawing.Point(1, 38);
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
            this.dataGridView1.Size = new System.Drawing.Size(977, 168);
            this.dataGridView1.TabIndex = 52;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // 编号
            // 
            this.编号.DataPropertyName = "编号";
            this.编号.HeaderText = "编号";
            this.编号.Name = "编号";
            this.编号.ReadOnly = true;
            this.编号.Visible = false;
            // 
            // 申请关联时间
            // 
            this.申请关联时间.DataPropertyName = "申请关联时间";
            this.申请关联时间.HeaderText = "申请关联时间";
            this.申请关联时间.Name = "申请关联时间";
            this.申请关联时间.ReadOnly = true;
            this.申请关联时间.Visible = false;
            // 
            // 经纪人用户名
            // 
            this.经纪人用户名.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.经纪人用户名.DataPropertyName = "经纪人用户名";
            this.经纪人用户名.HeaderText = "经纪人用户名";
            this.经纪人用户名.Name = "经纪人用户名";
            this.经纪人用户名.ReadOnly = true;
            this.经纪人用户名.Width = 107;
            // 
            // 经纪人邮箱
            // 
            this.经纪人邮箱.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.经纪人邮箱.DataPropertyName = "经纪人邮箱";
            this.经纪人邮箱.FillWeight = 120F;
            this.经纪人邮箱.HeaderText = "经纪人邮箱";
            this.经纪人邮箱.Name = "经纪人邮箱";
            this.经纪人邮箱.ReadOnly = true;
            this.经纪人邮箱.Width = 95;
            // 
            // 联系人姓名
            // 
            this.联系人姓名.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.联系人姓名.DataPropertyName = "联系人姓名";
            this.联系人姓名.HeaderText = "联系人姓名";
            this.联系人姓名.Name = "联系人姓名";
            this.联系人姓名.ReadOnly = true;
            this.联系人姓名.Width = 95;
            // 
            // 手机号码
            // 
            this.手机号码.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.手机号码.DataPropertyName = "手机号码";
            this.手机号码.HeaderText = "手机号码";
            this.手机号码.Name = "手机号码";
            this.手机号码.ReadOnly = true;
            this.手机号码.Width = 83;
            // 
            // 是否接收新用户
            // 
            this.是否接收新用户.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.是否接收新用户.DataPropertyName = "是否接收新用户";
            this.是否接收新用户.HeaderText = "是否接收新用户";
            this.是否接收新用户.Name = "是否接收新用户";
            this.是否接收新用户.ReadOnly = true;
            this.是否接收新用户.Width = 119;
            // 
            // 审核状态
            // 
            this.审核状态.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.审核状态.DataPropertyName = "审核状态";
            this.审核状态.HeaderText = "审核状态";
            this.审核状态.Name = "审核状态";
            this.审核状态.ReadOnly = true;
            this.审核状态.Width = 83;
            // 
            // 默认关联经纪人
            // 
            this.默认关联经纪人.DataPropertyName = "默认关联经纪人";
            this.默认关联经纪人.FillWeight = 120F;
            this.默认关联经纪人.HeaderText = "默认关联经纪人";
            this.默认关联经纪人.LinkColor = System.Drawing.Color.RoyalBlue;
            this.默认关联经纪人.Name = "默认关联经纪人";
            this.默认关联经纪人.ReadOnly = true;
            this.默认关联经纪人.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.默认关联经纪人.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            // 
            // 操作
            // 
            this.操作.FillWeight = 70F;
            this.操作.HeaderText = "操作";
            this.操作.LinkColor = System.Drawing.Color.RoyalBlue;
            this.操作.Name = "操作";
            this.操作.ReadOnly = true;
            this.操作.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.操作.Text = "查看详情";
            this.操作.UseColumnTextForLinkValue = true;
            this.操作.VisitedLinkColor = System.Drawing.Color.RoyalBlue;
            this.操作.Width = 70;
            // 
            // 角色登陆账号
            // 
            this.角色登陆账号.DataPropertyName = "角色登陆账号";
            this.角色登陆账号.HeaderText = "角色登陆账号";
            this.角色登陆账号.Name = "角色登陆账号";
            this.角色登陆账号.ReadOnly = true;
            this.角色登陆账号.Visible = false;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(1, 212);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(977, 30);
            this.ucPager1.TabIndex = 53;
            // 
            // GLjingjiren
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.PBload);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "GLjingjiren";
            this.Size = new System.Drawing.Size(979, 574);
            this.Load += new System.EventHandler(this.GLjingjiren_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelUC1.ResumeLayout(false);
            this.panelUC1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PBload;
        private Com.Seezt.Skins.BasicButton ResetB;
        private System.Windows.Forms.Label Lselect;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lb7;
        private System.Windows.Forms.Label label1;
        private Com.Seezt.Skins.BasicButton btnTijiao;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lb6;
        private System.Windows.Forms.Label label2;
        private PanelUC panelUC1;
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.Label lb5;
        private System.Windows.Forms.Label lb4;
        private System.Windows.Forms.Label lb2;
        private System.Windows.Forms.Label lb3;
        private PanelUC panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UCPager ucPager1;
        private PanelUC panel1;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private UCTextBox ucTextBox5;
        private System.Windows.Forms.DataGridViewTextBoxColumn 编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 申请关联时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 经纪人用户名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 经纪人邮箱;
        private System.Windows.Forms.DataGridViewTextBoxColumn 联系人姓名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 手机号码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 是否接收新用户;
        private System.Windows.Forms.DataGridViewTextBoxColumn 审核状态;
        private System.Windows.Forms.DataGridViewLinkColumn 默认关联经纪人;
        private System.Windows.Forms.DataGridViewLinkColumn 操作;
        private System.Windows.Forms.DataGridViewTextBoxColumn 角色登陆账号;
    }
}
