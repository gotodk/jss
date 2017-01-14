namespace 客户端主程序.SubForm
{
    partial class FormSC
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.wjm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ysc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bfb = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yys = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pjsd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.bdqlj = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fwqqlj = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.wjm,
            this.ysc,
            this.bfb,
            this.yys,
            this.pjsd,
            this.bdqlj,
            this.fwqqlj});
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.LabelWrap = false;
            this.listView1.Location = new System.Drawing.Point(12, 36);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(560, 186);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // wjm
            // 
            this.wjm.Text = "文件名";
            this.wjm.Width = 130;
            // 
            // ysc
            // 
            this.ysc.Text = "已上传";
            this.ysc.Width = 100;
            // 
            // bfb
            // 
            this.bfb.Text = "上传进度";
            this.bfb.Width = 100;
            // 
            // yys
            // 
            this.yys.Text = "已用时";
            this.yys.Width = 100;
            // 
            // pjsd
            // 
            this.pjsd.Text = "平均速度";
            this.pjsd.Width = 120;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(11, 228);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(560, 23);
            this.progressBar1.TabIndex = 2;
            this.progressBar1.Value = 40;
            // 
            // bdqlj
            // 
            this.bdqlj.Text = "本地全路径";
            // 
            // fwqqlj
            // 
            this.fwqqlj.Text = "服务器全路径";
            // 
            // FormSC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 264);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.listView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSC";
            this.ShowColorButton = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "正在上传，请稍等……";
            this.Load += new System.EventHandler(this.FormSC_Load);
            this.Controls.SetChildIndex(this.listView1, 0);
            this.Controls.SetChildIndex(this.progressBar1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader wjm;
        private System.Windows.Forms.ColumnHeader ysc;
        private System.Windows.Forms.ColumnHeader bfb;
        private System.Windows.Forms.ColumnHeader yys;
        private System.Windows.Forms.ColumnHeader pjsd;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ColumnHeader bdqlj;
        private System.Windows.Forms.ColumnHeader fwqqlj;

    }
}