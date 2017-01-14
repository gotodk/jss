namespace 客户端主程序.SubForm.CenterForm.Seller
{
    partial class FBdldjSelectWarse
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FBdldjSelectWarse));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbTBLX = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelUC2 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.ucPager1 = new 客户端主程序.SubForm.UCPager();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.myxz = new System.Windows.Forms.DataGridViewImageColumn();
            this.panelUC5 = new 客户端主程序.SubForm.CenterForm.publicUC.PanelUC();
            this.cbThreeSPFL = new System.Windows.Forms.ComboBox();
            this.cbSecondSPFL = new System.Windows.Forms.ComboBox();
            this.cbStartSPFL = new System.Windows.Forms.ComboBox();
            this.basicButton2 = new Com.Seezt.Skins.BasicButton();
            this.ucTextBox6 = new 客户端主程序.SubForm.UCTextBox(this.components);
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
            this.panel1.Controls.Add(this.cbTBLX);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panelUC2);
            this.panel1.Controls.Add(this.panelUC5);
            this.panel1.Location = new System.Drawing.Point(2, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(646, 368);
            this.panel1.TabIndex = 29;
            // 
            // cbTBLX
            // 
            this.cbTBLX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTBLX.FormattingEnabled = true;
            this.cbTBLX.Items.AddRange(new object[] {
            "半年度",
            "年度",
            "定量定价"});
            this.cbTBLX.Location = new System.Drawing.Point(91, 48);
            this.cbTBLX.Name = "cbTBLX";
            this.cbTBLX.Size = new System.Drawing.Size(121, 20);
            this.cbTBLX.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "*投标类型：";
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
            this.panelUC2.Location = new System.Drawing.Point(4, 73);
            this.panelUC2.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC2.Name = "panelUC2";
            this.panelUC2.Padding = new System.Windows.Forms.Padding(1);
            this.panelUC2.SetShowBorder = 1;
            this.panelUC2.Size = new System.Drawing.Size(638, 290);
            this.panelUC2.TabIndex = 17;
            // 
            // ucPager1
            // 
            this.ucPager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucPager1.DFT = null;
            this.ucPager1.HT_Where = null;
            this.ucPager1.Location = new System.Drawing.Point(3, 259);
            this.ucPager1.Name = "ucPager1";
            this.ucPager1.Size = new System.Drawing.Size(634, 30);
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
            this.myxz});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Silver;
            this.dataGridView1.Location = new System.Drawing.Point(1, 3);
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
            this.dataGridView1.Size = new System.Drawing.Size(636, 252);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseEnter);
            // 
            // myxz
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.myxz.DefaultCellStyle = dataGridViewCellStyle2;
            this.myxz.HeaderText = "  ";
            this.myxz.Image = global::客户端主程序.Properties.Resources.myxz;
            this.myxz.Name = "myxz";
            this.myxz.ReadOnly = true;
            this.myxz.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.myxz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.myxz.Width = 35;
            // 
            // panelUC5
            // 
            this.panelUC5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUC5.BackColor = System.Drawing.Color.AliceBlue;
            this.panelUC5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUC5.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.panelUC5.Controls.Add(this.cbThreeSPFL);
            this.panelUC5.Controls.Add(this.cbSecondSPFL);
            this.panelUC5.Controls.Add(this.cbStartSPFL);
            this.panelUC5.Controls.Add(this.basicButton2);
            this.panelUC5.Controls.Add(this.ucTextBox6);
            this.panelUC5.Location = new System.Drawing.Point(4, 4);
            this.panelUC5.Margin = new System.Windows.Forms.Padding(0);
            this.panelUC5.Name = "panelUC5";
            this.panelUC5.SetShowBorder = 1;
            this.panelUC5.Size = new System.Drawing.Size(638, 40);
            this.panelUC5.TabIndex = 16;
            // 
            // cbThreeSPFL
            // 
            this.cbThreeSPFL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThreeSPFL.FormattingEnabled = true;
            this.cbThreeSPFL.Location = new System.Drawing.Point(401, 10);
            this.cbThreeSPFL.Name = "cbThreeSPFL";
            this.cbThreeSPFL.Size = new System.Drawing.Size(100, 20);
            this.cbThreeSPFL.TabIndex = 64;
            // 
            // cbSecondSPFL
            // 
            this.cbSecondSPFL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSecondSPFL.FormattingEnabled = true;
            this.cbSecondSPFL.Location = new System.Drawing.Point(281, 10);
            this.cbSecondSPFL.Name = "cbSecondSPFL";
            this.cbSecondSPFL.Size = new System.Drawing.Size(100, 20);
            this.cbSecondSPFL.TabIndex = 63;
            this.cbSecondSPFL.SelectedIndexChanged += new System.EventHandler(this.cbSecondSPFL_SelectedIndexChanged);
            // 
            // cbStartSPFL
            // 
            this.cbStartSPFL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStartSPFL.FormattingEnabled = true;
            this.cbStartSPFL.Location = new System.Drawing.Point(161, 10);
            this.cbStartSPFL.Name = "cbStartSPFL";
            this.cbStartSPFL.Size = new System.Drawing.Size(100, 20);
            this.cbStartSPFL.TabIndex = 20;
            this.cbStartSPFL.SelectedIndexChanged += new System.EventHandler(this.cbStartSPFL_SelectedIndexChanged);
            // 
            // basicButton2
            // 
            this.basicButton2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.basicButton2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.basicButton2.ForeColor = System.Drawing.Color.DarkBlue;
            this.basicButton2.Location = new System.Drawing.Point(521, 9);
            this.basicButton2.Name = "basicButton2";
            this.basicButton2.Size = new System.Drawing.Size(50, 21);
            this.basicButton2.TabIndex = 18;
            this.basicButton2.Texts = "查询";
            // 
            // ucTextBox6
            // 
            this.ucTextBox6.BackColor = System.Drawing.Color.White;
            this.ucTextBox6.BorderColor = System.Drawing.Color.Gray;
            this.ucTextBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucTextBox6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucTextBox6.ForeColor = System.Drawing.Color.Black;
            this.ucTextBox6.HotColor = System.Drawing.Color.CornflowerBlue;
            this.ucTextBox6.Location = new System.Drawing.Point(11, 9);
            this.ucTextBox6.MaxLength = 500;
            this.ucTextBox6.Name = "ucTextBox6";
            this.ucTextBox6.Size = new System.Drawing.Size(130, 23);
            this.ucTextBox6.TabIndex = 15;
            this.ucTextBox6.WordWrap = false;
            // 
            // FBdldjSelectWarse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 400);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FBdldjSelectWarse";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择xxx";
            this.Load += new System.EventHandler(this.FBdldjSelectWarse_Load);
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
        private UCTextBox ucTextBox6;
        private CenterForm.publicUC.PanelUC panelUC2;
        private UCPager ucPager1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn myxz;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTBLX;
        private System.Windows.Forms.ComboBox cbStartSPFL;
        private System.Windows.Forms.ComboBox cbSecondSPFL;
        private System.Windows.Forms.ComboBox cbThreeSPFL;
    }
}