namespace 客户端主程序.SubForm.NewCenterForm
{
    partial class UCshangchuan
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
            this.B_SCSC = new System.Windows.Forms.Label();
            this.B_SC = new System.Windows.Forms.Label();
            this.B_SCCK = new System.Windows.Forms.Label();
            this.timer_SCCK = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.listView1 = new System.Windows.Forms.ListView();
            this.bdfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yclj = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wjm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.jsnumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scjg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // B_SCSC
            // 
            this.B_SCSC.BackColor = System.Drawing.Color.PowderBlue;
            this.B_SCSC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.B_SCSC.ForeColor = System.Drawing.Color.Black;
            this.B_SCSC.Location = new System.Drawing.Point(133, 0);
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
            this.B_SC.Location = new System.Drawing.Point(0, 0);
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
            this.B_SCCK.Location = new System.Drawing.Point(67, 0);
            this.B_SCCK.Name = "B_SCCK";
            this.B_SCCK.Size = new System.Drawing.Size(50, 20);
            this.B_SCCK.TabIndex = 31;
            this.B_SCCK.Text = "查看";
            this.B_SCCK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.B_SCCK.Visible = false;
            this.B_SCCK.Click += new System.EventHandler(this.B_SCCK_Click);
            // 
            // timer_SCCK
            // 
            this.timer_SCCK.Enabled = true;
            this.timer_SCCK.Interval = 500;
            this.timer_SCCK.Tick += new System.EventHandler(this.timer_SCCK_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "图片文件(*.jpg,*.jpeg,*.png,*.gif,*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            this.openFileDialog1.Title = "选择要上传的文件";
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
            this.listView1.Location = new System.Drawing.Point(3, 35);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(385, 85);
            this.listView1.TabIndex = 33;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
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
            // UCshangchuan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.B_SCSC);
            this.Controls.Add(this.B_SCCK);
            this.Controls.Add(this.B_SC);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "UCshangchuan";
            this.Size = new System.Drawing.Size(419, 173);
            this.Load += new System.EventHandler(this.UCshangchuan_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label B_SCSC;
        private System.Windows.Forms.Label B_SC;
        private System.Windows.Forms.Label B_SCCK;
        private System.Windows.Forms.Timer timer_SCCK;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader bdfile;
        private System.Windows.Forms.ColumnHeader yclj;
        private System.Windows.Forms.ColumnHeader wjm;
        private System.Windows.Forms.ColumnHeader jsnumber;
        private System.Windows.Forms.ColumnHeader scjg;
    }
}
