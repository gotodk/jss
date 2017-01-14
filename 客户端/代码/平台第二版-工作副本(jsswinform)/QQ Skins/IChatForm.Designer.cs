using Com.Seezt.Skins;
namespace Com.Seezt.Skins
{
    partial class IChatForm
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
            if (imageAttr != null)
            {
                imageAttr.Dispose();
            }
            base.Dispose(disposing);
            System.GC.Collect();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IChatForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolBarBg = new System.Windows.Forms.PictureBox();
            this.qqShowBg = new System.Windows.Forms.PictureBox();
            this.ButtonMax = new System.Windows.Forms.PictureBox();
            this.ButtonClose = new System.Windows.Forms.PictureBox();
            this.ButtonMin = new System.Windows.Forms.PictureBox();
            this.description = new System.Windows.Forms.Label();
            this.nikeName = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.toolBarBg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qqShowBg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonMin)).BeginInit();
            this.SuspendLayout();
            // 
            // toolBarBg
            // 
            this.toolBarBg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolBarBg.BackColor = System.Drawing.Color.Transparent;
            this.toolBarBg.Location = new System.Drawing.Point(5, 335);
            this.toolBarBg.Name = "toolBarBg";
            this.toolBarBg.Size = new System.Drawing.Size(375, 22);
            this.toolBarBg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.toolBarBg.TabIndex = 37;
            this.toolBarBg.TabStop = false;
            this.toolBarBg.Paint += new System.Windows.Forms.PaintEventHandler(this.toolBarBg_Paint);
            // 
            // qqShowBg
            // 
            this.qqShowBg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.qqShowBg.BackColor = System.Drawing.Color.Transparent;
            this.qqShowBg.Location = new System.Drawing.Point(385, 85);
            this.qqShowBg.Name = "qqShowBg";
            this.qqShowBg.Size = new System.Drawing.Size(150, 250);
            this.qqShowBg.TabIndex = 34;
            this.qqShowBg.TabStop = false;
            this.qqShowBg.Paint += new System.Windows.Forms.PaintEventHandler(this.qqShowBg_Paint);
            // 
            // ButtonMax
            // 
            this.ButtonMax.BackColor = System.Drawing.Color.Transparent;
            this.ButtonMax.Location = new System.Drawing.Point(478, 0);
            this.ButtonMax.Name = "ButtonMax";
            this.ButtonMax.Size = new System.Drawing.Size(25, 18);
            this.ButtonMax.TabIndex = 26;
            this.ButtonMax.TabStop = false;
            this.ButtonMax.MouseLeave += new System.EventHandler(this.ButtonMax_MouseLeave);
            this.ButtonMax.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonMax_MouseClick);
            this.ButtonMax.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonMax_MouseDown);
            this.ButtonMax.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonMax_Paint);
            this.ButtonMax.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonMax_MouseUp);
            this.ButtonMax.MouseEnter += new System.EventHandler(this.ButtonMax_MouseEnter);
            // 
            // ButtonClose
            // 
            this.ButtonClose.BackColor = System.Drawing.Color.Transparent;
            this.ButtonClose.Location = new System.Drawing.Point(502, 0);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(38, 18);
            this.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ButtonClose.TabIndex = 25;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.MouseLeave += new System.EventHandler(this.ButtonClose_MouseLeave);
            this.ButtonClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonClose_MouseClick);
            this.ButtonClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonClose_MouseDown);
            this.ButtonClose.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonClose_Paint);
            this.ButtonClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonClose_MouseUp);
            this.ButtonClose.MouseEnter += new System.EventHandler(this.ButtonClose_MouseEnter);
            // 
            // ButtonMin
            // 
            this.ButtonMin.BackColor = System.Drawing.Color.Transparent;
            this.ButtonMin.Location = new System.Drawing.Point(454, 0);
            this.ButtonMin.Name = "ButtonMin";
            this.ButtonMin.Size = new System.Drawing.Size(25, 18);
            this.ButtonMin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ButtonMin.TabIndex = 24;
            this.ButtonMin.TabStop = false;
            this.ButtonMin.MouseLeave += new System.EventHandler(this.ButtonMin_MouseLeave);
            this.ButtonMin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonMin_MouseClick);
            this.ButtonMin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonMin_MouseDown);
            this.ButtonMin.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonMin_Paint);
            this.ButtonMin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonMin_MouseUp);
            this.ButtonMin.MouseEnter += new System.EventHandler(this.ButtonMin_MouseEnter);
            // 
            // description
            // 
            this.description.AutoSize = true;
            this.description.BackColor = System.Drawing.Color.Transparent;
            this.description.Location = new System.Drawing.Point(40, 26);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(0, 12);
            this.description.TabIndex = 39;
            this.description.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IChatForm_MouseDown);
            // 
            // nikeName
            // 
            this.nikeName.ActiveLinkColor = System.Drawing.Color.Black;
            this.nikeName.BackColor = System.Drawing.Color.Transparent;
            this.nikeName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nikeName.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.nikeName.LinkColor = System.Drawing.Color.Black;
            this.nikeName.Location = new System.Drawing.Point(40, 9);
            this.nikeName.Name = "nikeName";
            this.nikeName.Size = new System.Drawing.Size(177, 12);
            this.nikeName.TabIndex = 38;
            this.nikeName.VisitedLinkColor = System.Drawing.Color.Black;
            this.nikeName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IChatForm_MouseDown);
            // 
            // IChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(540, 490);
            this.Controls.Add(this.description);
            this.Controls.Add(this.nikeName);
            this.Controls.Add(this.toolBarBg);
            this.Controls.Add(this.qqShowBg);
            this.Controls.Add(this.ButtonMax);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonMin);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IChatForm";
            this.Text = "IChatForm";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.IChatForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IChatForm_MouseDown);
            this.Resize += new System.EventHandler(this.IChatForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.toolBarBg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qqShowBg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ButtonMax;
        private System.Windows.Forms.PictureBox ButtonClose;
        private System.Windows.Forms.PictureBox ButtonMin;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox qqShowBg;
        protected System.Windows.Forms.PictureBox toolBarBg;
        protected System.Windows.Forms.Label description;
        protected System.Windows.Forms.LinkLabel nikeName;
    }
}