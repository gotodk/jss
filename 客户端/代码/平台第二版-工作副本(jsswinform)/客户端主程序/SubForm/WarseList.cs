using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm
{
    public partial class WarseList : UserControl
    {
       
        /// <summary>
        /// 获取或设置被选择的一级、二级分类
        /// </summary>
        [Description("获取或设置被选择的一级、二级分类"), Category("Appearance")]
        public string[] SelectedItem
        {
            get
            {
                if (cbStartSPFL.SelectedItem == null)
                {
                    return new string[] { "", "", "" };
                }
                else
                {
                    return new string[] { cbStartSPFL.SelectedItem.ToString(), cbSecondSPFL.SelectedItem.ToString() };
                }
            }
            set
            {
                cbStartSPFL.SelectedItem = value[0];
                cbSecondSPFL.SelectedItem = value[1];
            }
        }

        /// <summary>
        /// 获取或设置一级、二级分类是否只读
        /// </summary>
        [Description("获取或设置一级、二级分类是否只读"), Category("Appearance")]
        public bool[] EnabledItem
        {
            get
            {
                return new bool[] { cbStartSPFL.Enabled, cbSecondSPFL.Enabled };
            }
            set
            {
                cbStartSPFL.Enabled = value[0];
                cbSecondSPFL.Enabled = value[1];
            }
        }
        //省市区
        DataTable DSStartSPFL, DSSecondSPFL;

        public WarseList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        public void initdefault()
        {

            DSStartSPFL = PublicDS.PublisDsData.Tables["一级分类"];
            DSSecondSPFL = PublicDS.PublisDsData.Tables["二级分类"];

            cbStartSPFL.Items.Clear();
            cbStartSPFL.Items.Add("一级分类");
            foreach (DataRow dr in DSStartSPFL.Rows)
            {
                cbStartSPFL.Items.Add(dr["商品名称"].ToString());
            }
            cbStartSPFL.SelectedIndex = 0;

            cbSecondSPFL.Items.Clear();
            cbSecondSPFL.Items.Add("二级分类");
            cbSecondSPFL.SelectedIndex = 0;

        }

        private void cbStartSPFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DSStartSPFL == null || DSSecondSPFL == null)
            { return; }
            cbSecondSPFL.Items.Clear();
            cbSecondSPFL.Items.Add("二级分类");

            foreach (DataRow dr in DSSecondSPFL.Select("所属商品名称 = '" + cbStartSPFL.SelectedItem.ToString() + "'"))
            {
                cbSecondSPFL.Items.Add(dr["商品名称"].ToString());
            }

            if (cbSecondSPFL.Items.Count == 2)
            {
                cbSecondSPFL.SelectedIndex = 1;
            }
            else
            {
                cbSecondSPFL.SelectedIndex = 0;
            }
        }

        private void cbSecondSPFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DSStartSPFL == null || DSSecondSPFL == null)
            { return; }

        }


    }
}
