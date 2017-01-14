namespace 客户端主程序.SubForm.CenterForm.publicUC
{
    partial class WHkaipaoxinxi
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer_SCCK = new System.Windows.Forms.Timer(this.components);
            this.PBload = new System.Windows.Forms.PictureBox();
            this.panelUC1 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtdwmc = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtdz = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.cboxfapiaolx = new System.Windows.Forms.ComboBox();
            this.lbNumber = new System.Windows.Forms.Label();
            this.txtimage = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtsbh = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.bdfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yclj = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wjm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.jsnumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scjg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.B_SCSC = new System.Windows.Forms.Label();
            this.B_SC = new System.Windows.Forms.Label();
            this.B_SCCK = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtyhzh = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.txtlxdh = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ResetB = new Com.Seezt.Skins.BasicButton();
            this.btnSave = new Com.Seezt.Skins.BasicButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.basicButton1 = new Com.Seezt.Skins.BasicButton();
            this.panelUC3 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.txtdqfplx = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtdqkpxx = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.btnbiangeng = new Com.Seezt.Skins.BasicButton();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.panelUC4 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lbinfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.panelUC1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelUC2.SuspendLayout();
            this.panelUC5.SuspendLayout();
            this.panelUC3.SuspendLayout();
            this.panelUC4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "图片|*.jpg;*.gif;*.png";
            this.openFileDialog1.Title = "选择要上传的文件";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
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
            this.PBload.Location = new System.Drawing.Point(865, 543);
            this.PBload.Name = "PBload";
            this.PBload.Size = new System.Drawing.Size(32, 32);
            this.PBload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PBload.TabIndex = 46;
            this.PBload.TabStop = false;
            this.PBload.Visible = false;
            // 
            // panelUC1
            // 
            this.panelUC1.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC1.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC1.Controls.Add(this.panel1);
            this.panelUC1.Controls.Add(this.ResetB);
            this.panelUC1.Controls.Add(this.btnSave);
            this.panelUC1.Controls.Add(this.label1);
            this.panelUC1.Location = new System.Drawing.Point(2, 103);
            this.panelUC1.Margin = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.panelUC1.Name = "panelUC1";
            this.panelUC1.SetShowBorder = 1;
            this.panelUC1.Size = new System.Drawing.Size(979, 305);
            this.panelUC1.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.txtdwmc);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.txtdz);
            this.panel1.Controls.Add(this.cboxfapiaolx);
            this.panel1.Controls.Add(this.lbNumber);
            this.panel1.Controls.Add(this.txtimage);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtsbh);
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.panelUC2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtyhzh);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtlxdh);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Location = new System.Drawing.Point(9, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(832, 238);
            this.panel1.TabIndex = 10;
            // 
            // txtdwmc
            // 
            this.txtdwmc.BackColor = System.Drawing.Color.White;
            this.txtdwmc.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtdwmc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtdwmc.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtdwmc.ForeColor = System.Drawing.Color.Black;
            this.txtdwmc.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtdwmc.Location = new System.Drawing.Point(149, 34);
            this.txtdwmc.MaxLength = 45;
            this.txtdwmc.Name = "txtdwmc";
            this.txtdwmc.Size = new System.Drawing.Size(272, 23);
            this.txtdwmc.TabIndex = 12;
            this.txtdwmc.TextNtip = "";
            this.txtdwmc.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(79, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 31;
            this.label3.Text = "单位名称：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(79, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "发票类型：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(65, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 12);
            this.label8.TabIndex = 56;
            this.label8.Text = "*";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(65, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(11, 12);
            this.label15.TabIndex = 66;
            this.label15.Text = "*";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtdz
            // 
            this.txtdz.BackColor = System.Drawing.Color.White;
            this.txtdz.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtdz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtdz.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtdz.ForeColor = System.Drawing.Color.Black;
            this.txtdz.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtdz.Location = new System.Drawing.Point(149, 94);
            this.txtdz.MaxLength = 60;
            this.txtdz.Name = "txtdz";
            this.txtdz.Size = new System.Drawing.Size(272, 23);
            this.txtdz.TabIndex = 14;
            this.txtdz.TextNtip = "";
            this.txtdz.WordWrap = false;
            // 
            // cboxfapiaolx
            // 
            this.cboxfapiaolx.BackColor = System.Drawing.Color.White;
            this.cboxfapiaolx.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboxfapiaolx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxfapiaolx.ForeColor = System.Drawing.Color.Black;
            this.cboxfapiaolx.FormattingEnabled = true;
            this.cboxfapiaolx.Items.AddRange(new object[] {
            "请选择",
            "增值税普通发票",
            "增值税专用发票"});
            this.cboxfapiaolx.Location = new System.Drawing.Point(149, 6);
            this.cboxfapiaolx.Name = "cboxfapiaolx";
            this.cboxfapiaolx.Size = new System.Drawing.Size(154, 22);
            this.cboxfapiaolx.TabIndex = 11;
            this.cboxfapiaolx.SelectedIndexChanged += new System.EventHandler(this.cboxfapiaolx_SelectedIndexChanged);
            // 
            // lbNumber
            // 
            this.lbNumber.AutoSize = true;
            this.lbNumber.Location = new System.Drawing.Point(723, 115);
            this.lbNumber.Name = "lbNumber";
            this.lbNumber.Size = new System.Drawing.Size(0, 12);
            this.lbNumber.TabIndex = 72;
            this.lbNumber.Visible = false;
            // 
            // txtimage
            // 
            this.txtimage.BackColor = System.Drawing.Color.White;
            this.txtimage.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtimage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtimage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtimage.ForeColor = System.Drawing.Color.Black;
            this.txtimage.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtimage.Location = new System.Drawing.Point(436, 170);
            this.txtimage.Name = "txtimage";
            this.txtimage.Size = new System.Drawing.Size(253, 23);
            this.txtimage.TabIndex = 80;
            this.txtimage.TextNtip = "";
            this.txtimage.Visible = false;
            this.txtimage.WordWrap = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(41, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 12);
            this.label9.TabIndex = 70;
            this.label9.Text = "*";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(5, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 69;
            this.label6.Text = "*";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtsbh
            // 
            this.txtsbh.BackColor = System.Drawing.Color.White;
            this.txtsbh.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtsbh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtsbh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtsbh.ForeColor = System.Drawing.Color.Black;
            this.txtsbh.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtsbh.Location = new System.Drawing.Point(149, 64);
            this.txtsbh.MaxLength = 45;
            this.txtsbh.Name = "txtsbh";
            this.txtsbh.Size = new System.Drawing.Size(272, 23);
            this.txtsbh.TabIndex = 13;
            this.txtsbh.TextNtip = "";
            this.txtsbh.WordWrap = false;
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
            this.listView1.Location = new System.Drawing.Point(436, 69);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(385, 85);
            this.listView1.TabIndex = 68;
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
            this.wjm.Width = 69;
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(55, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "纳税人识别号：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelUC2
            // 
            this.panelUC2.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC2.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC2.Controls.Add(this.B_SCSC);
            this.panelUC2.Controls.Add(this.B_SC);
            this.panelUC2.Controls.Add(this.B_SCCK);
            this.panelUC2.Location = new System.Drawing.Point(146, 185);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.SetShowBorder = 0;
            this.panelUC2.Size = new System.Drawing.Size(184, 28);
            this.panelUC2.TabIndex = 67;
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
            this.B_SC.Click += new System.EventHandler(this.B_SC_Click);
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
            this.B_SCCK.Click += new System.EventHandler(this.B_SCCK_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(103, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 44;
            this.label5.Text = "地址：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtyhzh
            // 
            this.txtyhzh.BackColor = System.Drawing.Color.White;
            this.txtyhzh.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtyhzh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtyhzh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtyhzh.ForeColor = System.Drawing.Color.Black;
            this.txtyhzh.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtyhzh.Location = new System.Drawing.Point(149, 154);
            this.txtyhzh.MaxLength = 60;
            this.txtyhzh.Name = "txtyhzh";
            this.txtyhzh.Size = new System.Drawing.Size(272, 23);
            this.txtyhzh.TabIndex = 16;
            this.txtyhzh.TextNtip = "";
            this.txtyhzh.WordWrap = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(78, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 46;
            this.label7.Text = "联系电话：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtlxdh
            // 
            this.txtlxdh.BackColor = System.Drawing.Color.White;
            this.txtlxdh.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtlxdh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtlxdh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtlxdh.ForeColor = System.Drawing.Color.Black;
            this.txtlxdh.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtlxdh.Location = new System.Drawing.Point(149, 124);
            this.txtlxdh.MaxLength = 20;
            this.txtlxdh.Name = "txtlxdh";
            this.txtlxdh.Size = new System.Drawing.Size(272, 23);
            this.txtlxdh.TabIndex = 15;
            this.txtlxdh.TextNtip = "";
            this.txtlxdh.WordWrap = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(19, 193);
            this.label14.Name = "label14";
            this.label14.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label14.Size = new System.Drawing.Size(125, 12);
            this.label14.TabIndex = 62;
            this.label14.Text = "带公章纳税人资格证：";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(41, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 12);
            this.label10.TabIndex = 58;
            this.label10.Text = "*";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(55, 159);
            this.label13.Name = "label13";
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label13.Size = new System.Drawing.Size(89, 12);
            this.label13.TabIndex = 61;
            this.label13.Text = "开户行及账号：";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(89, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 12);
            this.label11.TabIndex = 59;
            this.label11.Text = "*";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(64, 129);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(11, 12);
            this.label12.TabIndex = 60;
            this.label12.Text = "*";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ResetB
            // 
            this.ResetB.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ResetB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ResetB.ForeColor = System.Drawing.Color.DarkBlue;
            this.ResetB.Location = new System.Drawing.Point(231, 277);
            this.ResetB.Name = "ResetB";
            this.ResetB.Size = new System.Drawing.Size(50, 22);
            this.ResetB.TabIndex = 37;
            this.ResetB.Texts = "取消";
            this.ResetB.Click += new System.EventHandler(this.ResetB_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSave.Location = new System.Drawing.Point(160, 277);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(50, 22);
            this.btnSave.TabIndex = 36;
            this.btnSave.Texts = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 14);
            this.label1.TabIndex = 33;
            this.label1.Text = "开票信息维护";
            // 
            // panelUC5
            // 
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.Controls.Add(this.basicButton1);
            this.panelUC5.Location = new System.Drawing.Point(0, 330);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(979, 10);
            this.panelUC5.TabIndex = 68;
            this.panelUC5.Visible = false;
            // 
            // basicButton1
            // 
            this.basicButton1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton1.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton1.Location = new System.Drawing.Point(122, 213);
            this.basicButton1.Name = "basicButton1";
            this.basicButton1.Size = new System.Drawing.Size(70, 21);
            this.basicButton1.TabIndex = 36;
            this.basicButton1.Texts = "变更申请";
            // 
            // panelUC3
            // 
            this.panelUC3.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC3.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC3.Controls.Add(this.txtdqfplx);
            this.panelUC3.Controls.Add(this.txtdqkpxx);
            this.panelUC3.Controls.Add(this.label29);
            this.panelUC3.Controls.Add(this.label30);
            this.panelUC3.Controls.Add(this.btnbiangeng);
            this.panelUC3.Controls.Add(this.label31);
            this.panelUC3.Controls.Add(this.label32);
            this.panelUC3.Controls.Add(this.label33);
            this.panelUC3.Location = new System.Drawing.Point(0, 353);
            this.panelUC3.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC3.Name = "panelUC3";
            this.panelUC3.SetShowBorder = 1;
            this.panelUC3.Size = new System.Drawing.Size(979, 211);
            this.panelUC3.TabIndex = 47;
            // 
            // txtdqfplx
            // 
            this.txtdqfplx.BackColor = System.Drawing.Color.White;
            this.txtdqfplx.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtdqfplx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtdqfplx.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtdqfplx.ForeColor = System.Drawing.Color.Black;
            this.txtdqfplx.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtdqfplx.Location = new System.Drawing.Point(122, 38);
            this.txtdqfplx.MaxLength = 20;
            this.txtdqfplx.Name = "txtdqfplx";
            this.txtdqfplx.ReadOnly = true;
            this.txtdqfplx.Size = new System.Drawing.Size(230, 23);
            this.txtdqfplx.TabIndex = 67;
            this.txtdqfplx.TextNtip = "";
            this.txtdqfplx.WordWrap = false;
            // 
            // txtdqkpxx
            // 
            this.txtdqkpxx.AcceptsReturn = true;
            this.txtdqkpxx.AcceptsTab = true;
            this.txtdqkpxx.BackColor = System.Drawing.Color.White;
            this.txtdqkpxx.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtdqkpxx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtdqkpxx.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtdqkpxx.ForeColor = System.Drawing.Color.Black;
            this.txtdqkpxx.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtdqkpxx.Location = new System.Drawing.Point(122, 68);
            this.txtdqkpxx.MaxLength = 5;
            this.txtdqkpxx.Multiline = true;
            this.txtdqkpxx.Name = "txtdqkpxx";
            this.txtdqkpxx.ReadOnly = true;
            this.txtdqkpxx.Size = new System.Drawing.Size(423, 101);
            this.txtdqkpxx.TabIndex = 34;
            this.txtdqkpxx.TabStop = false;
            this.txtdqkpxx.TextNtip = "";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.Color.Red;
            this.label29.Location = new System.Drawing.Point(40, 43);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(11, 12);
            this.label29.TabIndex = 66;
            this.label29.Text = "*";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.ForeColor = System.Drawing.Color.Red;
            this.label30.Location = new System.Drawing.Point(41, 73);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(11, 12);
            this.label30.TabIndex = 56;
            this.label30.Text = "*";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnbiangeng
            // 
            this.btnbiangeng.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnbiangeng.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnbiangeng.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnbiangeng.Location = new System.Drawing.Point(123, 181);
            this.btnbiangeng.Name = "btnbiangeng";
            this.btnbiangeng.Size = new System.Drawing.Size(70, 22);
            this.btnbiangeng.TabIndex = 36;
            this.btnbiangeng.Texts = "变更申请";
            this.btnbiangeng.Click += new System.EventHandler(this.btnbiangeng_Click);
            // 
            // label31
            // 
            this.label31.AllowDrop = true;
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(20, 10);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(97, 14);
            this.label31.TabIndex = 33;
            this.label31.Text = "当前开票信息";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(54, 43);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(65, 12);
            this.label32.TabIndex = 29;
            this.label32.Text = "发票类型：";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.Location = new System.Drawing.Point(54, 73);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(65, 12);
            this.label33.TabIndex = 31;
            this.label33.Text = "开票信息：";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelUC4
            // 
            this.panelUC4.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC4.BorderColor = System.Drawing.Color.AliceBlue;
            this.panelUC4.Controls.Add(this.pictureBox1);
            this.panelUC4.Controls.Add(this.label17);
            this.panelUC4.Controls.Add(this.label16);
            this.panelUC4.Controls.Add(this.label19);
            this.panelUC4.Controls.Add(this.lbinfo);
            this.panelUC4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUC4.Location = new System.Drawing.Point(0, 0);
            this.panelUC4.Margin = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.panelUC4.Name = "panelUC4";
            this.panelUC4.SetShowBorder = 1;
            this.panelUC4.Size = new System.Drawing.Size(979, 102);
            this.panelUC4.TabIndex = 15;
            this.panelUC4.TabStop = true;
            this.panelUC4.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.pictureBox1.Location = new System.Drawing.Point(0, 92);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(950, 1);
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(0, 66);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(311, 12);
            this.label17.TabIndex = 33;
            this.label17.Text = "3、开票信息如需变更，请提交变更申请及书面证明文件。";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(0, 44);
            this.label16.Name = "label16";
            this.label16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label16.Size = new System.Drawing.Size(569, 12);
            this.label16.TabIndex = 32;
            this.label16.Text = "2、开具增值税专用发票，需上传带公章纳税人资格证扫描件，图片格式为jpg、gif、png，大小不超过1M。";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(0, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 12);
            this.label19.TabIndex = 29;
            this.label19.Text = "说明：";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbinfo
            // 
            this.lbinfo.AutoSize = true;
            this.lbinfo.ForeColor = System.Drawing.Color.Red;
            this.lbinfo.Location = new System.Drawing.Point(0, 22);
            this.lbinfo.Name = "lbinfo";
            this.lbinfo.Size = new System.Drawing.Size(257, 12);
            this.lbinfo.TabIndex = 31;
            this.lbinfo.Text = "1、您尚未提交开票信息，请填写完整后保存。 ";
            this.lbinfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WHkaipaoxinxi
            // 
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.panelUC1);
            this.Controls.Add(this.PBload);
            this.Controls.Add(this.panelUC5);
            this.Controls.Add(this.panelUC3);
            this.Controls.Add(this.panelUC4);
            this.Name = "WHkaipaoxinxi";
            this.Size = new System.Drawing.Size(979, 572);
            this.Load += new System.EventHandler(this.WHkaipaoxinxi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.panelUC1.ResumeLayout(false);
            this.panelUC1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelUC2.ResumeLayout(false);
            this.panelUC5.ResumeLayout(false);
            this.panelUC3.ResumeLayout(false);
            this.panelUC3.PerformLayout();
            this.panelUC4.ResumeLayout(false);
            this.panelUC4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PBload;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer_SCCK;
        private PanelUC panelUC3;
        private UCTextBox txtdqkpxx;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private Com.Seezt.Skins.BasicButton btnbiangeng;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private UCTextBox txtdqfplx;
        private PanelUC panelUC5;
        private Com.Seezt.Skins.BasicButton basicButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private UCTextBox txtdwmc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label15;
        private UCTextBox txtdz;
        private System.Windows.Forms.ComboBox cboxfapiaolx;
        private System.Windows.Forms.Label lbNumber;
        private UCTextBox txtimage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private UCTextBox txtsbh;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader bdfile;
        private System.Windows.Forms.ColumnHeader yclj;
        private System.Windows.Forms.ColumnHeader wjm;
        private System.Windows.Forms.ColumnHeader jsnumber;
        private System.Windows.Forms.ColumnHeader scjg;
        private System.Windows.Forms.Label label4;
        private PanelUC panelUC2;
        private System.Windows.Forms.Label B_SCSC;
        private System.Windows.Forms.Label B_SC;
        private System.Windows.Forms.Label B_SCCK;
        private System.Windows.Forms.Label label5;
        private UCTextBox txtyhzh;
        private System.Windows.Forms.Label label7;
        private UCTextBox txtlxdh;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private Com.Seezt.Skins.BasicButton btnSave;
        private Com.Seezt.Skins.BasicButton ResetB;
        private PanelUC panelUC1;
        private System.Windows.Forms.Label lbinfo;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.PictureBox pictureBox1;
        private PanelUC panelUC4;
    }
}
