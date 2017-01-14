namespace 客户端主程序.SubForm.CenterForm.Buyer
{
    partial class FBYDD
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
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.label23 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSelectWarse = new System.Windows.Forms.Label();
            this.txtDW = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtXHGG = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtSPMC = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtGHZQ = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtYDL = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.txtNMRJG = new 客户端主程序.SubForm.UCTextBox(this.components);
            this.label34 = new System.Windows.Forms.Label();
            this.ucCityList1 = new 客户端主程序.SubForm.UCCityList();
            this.label32 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblMaxJJPL = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ResetB = new Com.Seezt.Skins.BasicButton();
            this.basicButton2 = new Com.Seezt.Skins.BasicButton();
            this.lblTBJ = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).BeginInit();
            this.panelUC5.SuspendLayout();
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
            // panelUC5
            // 
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.label23);
            this.panelUC5.Controls.Add(this.label3);
            this.panelUC5.Controls.Add(this.lblSelectWarse);
            this.panelUC5.Controls.Add(this.txtDW);
            this.panelUC5.Controls.Add(this.txtXHGG);
            this.panelUC5.Controls.Add(this.txtSPMC);
            this.panelUC5.Controls.Add(this.txtGHZQ);
            this.panelUC5.Controls.Add(this.txtYDL);
            this.panelUC5.Controls.Add(this.txtNMRJG);
            this.panelUC5.Controls.Add(this.label34);
            this.panelUC5.Controls.Add(this.ucCityList1);
            this.panelUC5.Controls.Add(this.label32);
            this.panelUC5.Controls.Add(this.label28);
            this.panelUC5.Controls.Add(this.label13);
            this.panelUC5.Controls.Add(this.label11);
            this.panelUC5.Controls.Add(this.label21);
            this.panelUC5.Controls.Add(this.label17);
            this.panelUC5.Controls.Add(this.label16);
            this.panelUC5.Controls.Add(this.lblMaxJJPL);
            this.panelUC5.Controls.Add(this.label4);
            this.panelUC5.Controls.Add(this.ResetB);
            this.panelUC5.Controls.Add(this.basicButton2);
            this.panelUC5.Controls.Add(this.lblTBJ);
            this.panelUC5.Controls.Add(this.label2);
            this.panelUC5.Controls.Add(this.label1);
            this.panelUC5.ForeColor = System.Drawing.Color.DarkBlue;
            this.panelUC5.Location = new System.Drawing.Point(10, 10);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(726, 414);
            this.panelUC5.TabIndex = 16;
            this.panelUC5.Paint += new System.Windows.Forms.PaintEventHandler(this.panelUC5_Paint);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label23.ForeColor = System.Drawing.Color.Red;
            this.label23.Location = new System.Drawing.Point(282, 39);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(314, 28);
            this.label23.TabIndex = 107;
            this.label23.Text = "当前品类商品最低投标价：x元；\r\n投标拟售量x；集合预订量x；所有标的中最大经济批量：X";
            this.label23.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(282, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 99;
            this.label3.Text = "商品编号：";
            this.label3.Visible = false;
            // 
            // lblSelectWarse
            // 
            this.lblSelectWarse.BackColor = System.Drawing.Color.PowderBlue;
            this.lblSelectWarse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSelectWarse.ForeColor = System.Drawing.Color.Black;
            this.lblSelectWarse.Location = new System.Drawing.Point(124, 13);
            this.lblSelectWarse.Name = "lblSelectWarse";
            this.lblSelectWarse.Size = new System.Drawing.Size(59, 20);
            this.lblSelectWarse.TabIndex = 98;
            this.lblSelectWarse.Text = "选择";
            this.lblSelectWarse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSelectWarse.Click += new System.EventHandler(this.lblSelectWarse_Click);
            // 
            // txtDW
            // 
            this.txtDW.BackColor = System.Drawing.Color.White;
            this.txtDW.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtDW.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDW.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDW.ForeColor = System.Drawing.Color.Black;
            this.txtDW.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtDW.Location = new System.Drawing.Point(126, 129);
            this.txtDW.MaxLength = 10;
            this.txtDW.Name = "txtDW";
            this.txtDW.ReadOnly = true;
            this.txtDW.Size = new System.Drawing.Size(150, 23);
            this.txtDW.TabIndex = 97;
            this.txtDW.TextNtip = "";
            this.txtDW.WordWrap = false;
            // 
            // txtXHGG
            // 
            this.txtXHGG.BackColor = System.Drawing.Color.White;
            this.txtXHGG.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtXHGG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXHGG.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtXHGG.ForeColor = System.Drawing.Color.Black;
            this.txtXHGG.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtXHGG.Location = new System.Drawing.Point(126, 99);
            this.txtXHGG.MaxLength = 10;
            this.txtXHGG.Name = "txtXHGG";
            this.txtXHGG.ReadOnly = true;
            this.txtXHGG.Size = new System.Drawing.Size(150, 23);
            this.txtXHGG.TabIndex = 96;
            this.txtXHGG.TextNtip = "";
            this.txtXHGG.WordWrap = false;
            // 
            // txtSPMC
            // 
            this.txtSPMC.BackColor = System.Drawing.Color.White;
            this.txtSPMC.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtSPMC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSPMC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSPMC.ForeColor = System.Drawing.Color.Black;
            this.txtSPMC.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtSPMC.Location = new System.Drawing.Point(126, 69);
            this.txtSPMC.MaxLength = 10;
            this.txtSPMC.Name = "txtSPMC";
            this.txtSPMC.ReadOnly = true;
            this.txtSPMC.Size = new System.Drawing.Size(150, 23);
            this.txtSPMC.TabIndex = 95;
            this.txtSPMC.TextNtip = "";
            this.txtSPMC.WordWrap = false;
            // 
            // txtGHZQ
            // 
            this.txtGHZQ.BackColor = System.Drawing.Color.White;
            this.txtGHZQ.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtGHZQ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGHZQ.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGHZQ.ForeColor = System.Drawing.Color.Black;
            this.txtGHZQ.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtGHZQ.Location = new System.Drawing.Point(126, 39);
            this.txtGHZQ.MaxLength = 10;
            this.txtGHZQ.Name = "txtGHZQ";
            this.txtGHZQ.ReadOnly = true;
            this.txtGHZQ.Size = new System.Drawing.Size(150, 23);
            this.txtGHZQ.TabIndex = 94;
            this.txtGHZQ.TextNtip = "";
            this.txtGHZQ.WordWrap = false;
            // 
            // txtYDL
            // 
            this.txtYDL.BackColor = System.Drawing.Color.White;
            this.txtYDL.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtYDL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYDL.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYDL.ForeColor = System.Drawing.Color.Black;
            this.txtYDL.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtYDL.Location = new System.Drawing.Point(126, 189);
            this.txtYDL.MaxLength = 10;
            this.txtYDL.Name = "txtYDL";
            this.txtYDL.OpenZS = true;
            this.txtYDL.Size = new System.Drawing.Size(150, 23);
            this.txtYDL.TabIndex = 93;
            this.txtYDL.TextNtip = "";
            this.txtYDL.WordWrap = false;
            this.txtYDL.TextChanged += new System.EventHandler(this.txtYDL_TextChanged);
            // 
            // txtNMRJG
            // 
            this.txtNMRJG.BackColor = System.Drawing.Color.White;
            this.txtNMRJG.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.txtNMRJG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNMRJG.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNMRJG.ForeColor = System.Drawing.Color.Black;
            this.txtNMRJG.HotColor = System.Drawing.Color.CornflowerBlue;
            this.txtNMRJG.Location = new System.Drawing.Point(126, 159);
            this.txtNMRJG.MaxLength = 10;
            this.txtNMRJG.Name = "txtNMRJG";
            this.txtNMRJG.OpenLWXS = true;
            this.txtNMRJG.Size = new System.Drawing.Size(150, 23);
            this.txtNMRJG.TabIndex = 92;
            this.txtNMRJG.TextNtip = "";
            this.txtNMRJG.WordWrap = false;
            this.txtNMRJG.TextChanged += new System.EventHandler(this.txtNMRJG_TextChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.ForeColor = System.Drawing.Color.Black;
            this.label34.Location = new System.Drawing.Point(55, 104);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(65, 12);
            this.label34.TabIndex = 88;
            this.label34.Text = "型号规格：";
            // 
            // ucCityList1
            // 
            this.ucCityList1.EnabledItem = new bool[] {
        true,
        true,
        true};
            this.ucCityList1.Location = new System.Drawing.Point(126, 249);
            this.ucCityList1.Name = "ucCityList1";
            this.ucCityList1.SelectedItem = new string[] {
        "",
        "",
        ""};
            this.ucCityList1.Size = new System.Drawing.Size(380, 23);
            this.ucCityList1.TabIndex = 86;
            this.ucCityList1.VisibleItem = new bool[] {
        true,
        true,
        true};
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(53, 252);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(65, 12);
            this.label32.TabIndex = 85;
            this.label32.Text = "收货区域：";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label28.ForeColor = System.Drawing.Color.Red;
            this.label28.Location = new System.Drawing.Point(124, 219);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(185, 14);
            this.label28.TabIndex = 80;
            this.label28.Text = "预订单金额：0元  支付定金：0元";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(67, 195);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 78;
            this.label13.Text = "预订量：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(19, 164);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 12);
            this.label11.TabIndex = 75;
            this.label11.Text = "拟买入价格(元)：";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(55, 134);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 12);
            this.label21.TabIndex = 65;
            this.label21.Text = "计价单位：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(55, 74);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 59;
            this.label17.Text = "商品名称：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(55, 44);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 56;
            this.label16.Text = "供货周期：";
            // 
            // lblMaxJJPL
            // 
            this.lblMaxJJPL.AutoSize = true;
            this.lblMaxJJPL.ForeColor = System.Drawing.Color.Red;
            this.lblMaxJJPL.Location = new System.Drawing.Point(425, 179);
            this.lblMaxJJPL.Name = "lblMaxJJPL";
            this.lblMaxJJPL.Size = new System.Drawing.Size(11, 12);
            this.lblMaxJJPL.TabIndex = 47;
            this.lblMaxJJPL.Text = "*";
            this.lblMaxJJPL.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(282, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 12);
            this.label4.TabIndex = 46;
            this.label4.Text = "所有标的中最大经济批量：";
            this.label4.Visible = false;
            // 
            // ResetB
            // 
            this.ResetB.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ResetB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ResetB.ForeColor = System.Drawing.Color.DarkBlue;
            this.ResetB.Location = new System.Drawing.Point(226, 366);
            this.ResetB.Name = "ResetB";
            this.ResetB.Size = new System.Drawing.Size(50, 22);
            this.ResetB.TabIndex = 38;
            this.ResetB.Texts = "重置";
            this.ResetB.Click += new System.EventHandler(this.ResetB_Click);
            // 
            // basicButton2
            // 
            this.basicButton2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton2.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton2.Location = new System.Drawing.Point(126, 366);
            this.basicButton2.Name = "basicButton2";
            this.basicButton2.Size = new System.Drawing.Size(50, 22);
            this.basicButton2.TabIndex = 18;
            this.basicButton2.Texts = "提交";
            this.basicButton2.Click += new System.EventHandler(this.basicButton2_Click);
            // 
            // lblTBJ
            // 
            this.lblTBJ.AutoSize = true;
            this.lblTBJ.ForeColor = System.Drawing.Color.Red;
            this.lblTBJ.Location = new System.Drawing.Point(402, 159);
            this.lblTBJ.Name = "lblTBJ";
            this.lblTBJ.Size = new System.Drawing.Size(35, 12);
            this.lblTBJ.TabIndex = 44;
            this.lblTBJ.Text = "*元，";
            this.lblTBJ.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(282, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 43;
            this.label2.Text = "最低价标的投标价格：";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(31, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 39;
            this.label1.Text = "选择投标商品：";
            // 
            // FBYDD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PBload);
            this.Controls.Add(this.panelUC5);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "FBYDD";
            this.Size = new System.Drawing.Size(746, 437);
            ((System.ComponentModel.ISupportInitialize)(this.PBload)).EndInit();
            this.panelUC5.ResumeLayout(false);
            this.panelUC5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer_SCCK;
        private System.Windows.Forms.PictureBox PBload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTBJ;
        private Com.Seezt.Skins.BasicButton basicButton2;
        private Com.Seezt.Skins.BasicButton ResetB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblMaxJJPL;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label32;
        private UCCityList ucCityList1;
        private System.Windows.Forms.Label label34;
        public UCTextBox txtNMRJG;
        public UCTextBox txtYDL;
        public UCTextBox txtGHZQ;
        public UCTextBox txtSPMC;
        public UCTextBox txtXHGG;
        public UCTextBox txtDW;
        private System.Windows.Forms.Label lblSelectWarse;
        private System.Windows.Forms.Label label23;
        private publicUC.PanelUC panelUC5;
        private System.Windows.Forms.Label label3;

    }
}
