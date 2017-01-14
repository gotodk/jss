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
    public partial class UCCityList : UserControl
    {
        /// <summary>
        /// 获取或设置被选择的省市区
        /// </summary>
        [Description("获取或设置被选择的省市区"), Category("Appearance")]
        public string[] SelectedItem
        {
            get
            {
                if (CityList_Promary.SelectedItem == null)
                {
                    return new string[] { "", "", "" };
                }
                else
                {
                    return new string[] { CityList_Promary.SelectedItem.ToString(), ZZ_CityList_City.SelectedItem.ToString(), ZZ_CityList_qu.SelectedItem.ToString() };
                }
            }
            set
            {
                CityList_Promary.SelectedItem = value[0];
                ZZ_CityList_City.SelectedItem = value[1];
                ZZ_CityList_qu.SelectedItem = value[2];
            }
        }

        /// <summary>
        /// 获取或设置省市区是否只读
        /// </summary>
        [Description("获取或设置省市区是否只读"), Category("Appearance")]
        public bool[] EnabledItem
        {
            get
            {
                return new bool[] { CityList_Promary.Enabled, ZZ_CityList_City.Enabled, ZZ_CityList_qu.Enabled };
            }
            set
            {
                CityList_Promary.Enabled = value[0];
                ZZ_CityList_City.Enabled = value[1];
                ZZ_CityList_qu.Enabled = value[2];
            }
        }

        /// <summary>
        /// 获取或设置省市区是否显示
        /// </summary>
        [Description("获取或设置省市区是否显示"), Category("Appearance")]
        public bool[] VisibleItem
        {
            get
            {
                return new bool[] { CityList_Promary.Visible, ZZ_CityList_City.Visible, ZZ_CityList_qu.Visible };
            }
            set
            {
                CityList_Promary.Visible = value[0];
                ZZ_CityList_City.Visible = value[1];
                ZZ_CityList_qu.Visible = value[2];
            }
        }


        //省市区
        DataTable DSPromary, DSCity, DSqu;

        public UCCityList()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        public void initdefault()
        {

            DSPromary = PublicDS.PublisDsData.Tables["省"];
            DSCity = PublicDS.PublisDsData.Tables["市"];
            DSqu = PublicDS.PublisDsData.Tables["区"];

            CityList_Promary.Items.Clear();
            CityList_Promary.Items.Add("请选择省份");
            foreach (DataRow dr in DSPromary.Rows)
            {
                CityList_Promary.Items.Add(dr["省名"].ToString());
            }
            CityList_Promary.SelectedIndex = 0;

            ZZ_CityList_City.Items.Clear();
            ZZ_CityList_City.Items.Add("请选择城市");
            ZZ_CityList_City.SelectedIndex = 0;


            ZZ_CityList_qu.Items.Clear();
            ZZ_CityList_qu.Items.Add("请选择区县");
            ZZ_CityList_qu.SelectedIndex = 0;
        }

        private void UCCityList_Load(object sender, EventArgs e)
        {
            //initdefault();
            //处理下拉框间距
            this.CityList_Promary.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            this.ZZ_CityList_City.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            this.ZZ_CityList_qu.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
        }
        //处理下拉框间距
        private void CB_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox CBthis = (ComboBox)sender;
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(CBthis.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X + 2, e.Bounds.Y + 3);
        }

        private void CityList_Promary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DSPromary == null || DSCity == null || DSqu == null)
            { return; }
            ZZ_CityList_City.Items.Clear();
            ZZ_CityList_City.Items.Add("请选择城市");
    
            foreach (DataRow dr in DSCity.Select("所属省名 = '" + CityList_Promary.SelectedItem.ToString() + "'"))
            {
                ZZ_CityList_City.Items.Add(dr["市名"].ToString());
            }

            if (ZZ_CityList_City.Items.Count == 2)
            {
                ZZ_CityList_City.SelectedIndex = 1;
            }
            else
            {
                ZZ_CityList_City.SelectedIndex = 0;
            }
        }

        private void ZZ_CityList_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DSPromary == null || DSCity == null || DSqu == null)
            { return; }

            ZZ_CityList_qu.Items.Clear();
            ZZ_CityList_qu.Items.Add("请选择区县");

            foreach (DataRow dr in DSqu.Select("所属市名 = '" + ZZ_CityList_City.SelectedItem.ToString() + "'"))
            {
                ZZ_CityList_qu.Items.Add(dr["区名"].ToString());
            }
            if (ZZ_CityList_qu.Items.Count == 2)
            {
                ZZ_CityList_qu.SelectedIndex = 1;
            }
            else
            {
                ZZ_CityList_qu.SelectedIndex = 0;
            }
        }

        private void ZZ_CityList_qu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DSPromary == null || DSCity == null || DSqu == null)
            { return; }
        }




    
    }
}
