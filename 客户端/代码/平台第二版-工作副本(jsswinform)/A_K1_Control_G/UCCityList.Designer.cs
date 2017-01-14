namespace 客户端主程序.SubForm
{
    partial class UCCityList
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
            this.CityList_Promary = new System.Windows.Forms.ComboBox();
            this.ZZ_CityList_City = new System.Windows.Forms.ComboBox();
            this.ZZ_CityList_qu = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // CityList_Promary
            // 
            this.CityList_Promary.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CityList_Promary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CityList_Promary.FormattingEnabled = true;
            this.CityList_Promary.Items.AddRange(new object[] {
            "请选择省份"});
            this.CityList_Promary.Location = new System.Drawing.Point(0, 0);
            this.CityList_Promary.MaxDropDownItems = 10;
            this.CityList_Promary.Name = "CityList_Promary";
            this.CityList_Promary.Size = new System.Drawing.Size(121, 22);
            this.CityList_Promary.TabIndex = 2;
            this.CityList_Promary.SelectedIndexChanged += new System.EventHandler(this.CityList_Promary_SelectedIndexChanged);
            // 
            // ZZ_CityList_City
            // 
            this.ZZ_CityList_City.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ZZ_CityList_City.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ZZ_CityList_City.FormattingEnabled = true;
            this.ZZ_CityList_City.Items.AddRange(new object[] {
            "请选择城市"});
            this.ZZ_CityList_City.Location = new System.Drawing.Point(127, 0);
            this.ZZ_CityList_City.Name = "ZZ_CityList_City";
            this.ZZ_CityList_City.Size = new System.Drawing.Size(164, 22);
            this.ZZ_CityList_City.TabIndex = 3;
            this.ZZ_CityList_City.SelectedIndexChanged += new System.EventHandler(this.ZZ_CityList_City_SelectedIndexChanged);
            this.ZZ_CityList_City.EnabledChanged += new System.EventHandler(this.ZZ_CityList_City_EnabledChanged);
            // 
            // ZZ_CityList_qu
            // 
            this.ZZ_CityList_qu.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ZZ_CityList_qu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ZZ_CityList_qu.FormattingEnabled = true;
            this.ZZ_CityList_qu.Items.AddRange(new object[] {
            "请选择区县"});
            this.ZZ_CityList_qu.Location = new System.Drawing.Point(297, 0);
            this.ZZ_CityList_qu.Name = "ZZ_CityList_qu";
            this.ZZ_CityList_qu.Size = new System.Drawing.Size(134, 22);
            this.ZZ_CityList_qu.TabIndex = 4;
            this.ZZ_CityList_qu.SelectedIndexChanged += new System.EventHandler(this.ZZ_CityList_qu_SelectedIndexChanged);
            this.ZZ_CityList_qu.EnabledChanged += new System.EventHandler(this.ZZ_CityList_qu_EnabledChanged);
            // 
            // UCCityList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ZZ_CityList_qu);
            this.Controls.Add(this.ZZ_CityList_City);
            this.Controls.Add(this.CityList_Promary);
            this.Name = "UCCityList";
            this.Size = new System.Drawing.Size(439, 23);
            this.Load += new System.EventHandler(this.UCCityList_Load);
            this.EnabledChanged += new System.EventHandler(this.UCCityList_EnabledChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CityList_Promary;
        private System.Windows.Forms.ComboBox ZZ_CityList_City;
        private System.Windows.Forms.ComboBox ZZ_CityList_qu;
    }
}
