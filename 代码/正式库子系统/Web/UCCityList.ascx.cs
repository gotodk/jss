using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using System.Text;
using FMOP.DB;

public partial class FWPTZS_UCCityList : System.Web.UI.UserControl
{
        /// <summary>
        /// 获取或设置被选择的省市区
        /// </summary>
        [Description("获取或设置被选择的省市区"), Category("Appearance")]
        public string[] SelectedItem
        {
            get
            {
                if (CityList_Promary.SelectedValue == null)
                {
                    return new string[] { "", "", "" };
                }
                else
                {
                    return new string[] { CityList_Promary.SelectedValue.ToString(), CityList_City.SelectedValue.ToString(), CityList_qu.SelectedValue.ToString() };
                }
            }
            set
            {
                CityList_Promary.SelectedValue = value[0];
                CityList_Promary_SelectedIndexChanged(null, null);
                CityList_City.SelectedValue = value[1];
                CityList_City_SelectedIndexChanged(null, null);
                CityList_qu.SelectedValue = value[2];               
               
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
                return new bool[] { CityList_Promary.Enabled, CityList_City.Enabled, CityList_qu.Enabled };
            }
            set
            {
                CityList_Promary.Enabled = value[0];
                CityList_City.Enabled = value[1];
                CityList_qu.Enabled = value[2];
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
                return new bool[] { CityList_Promary.Visible, CityList_City.Visible, CityList_qu.Visible };
            }
            set
            {
                CityList_Promary.Visible = value[0];
                CityList_City.Visible = value[1];
                CityList_qu.Visible = value[2];
            }
        }


        //省市区
        DataTable DSPromary, DSCity, DSqu;

   

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        public void initdefault()
        {
            DataSet PublisDsData = new DataSet();

            //获取省市区
            DataSet ds_sheng = DbHelperSQL.Query("select p_number as 省编号,p_namestr as 省名,'0' as 省标记 from AAA_CityList_Promary");
            ds_sheng.Tables[0].TableName = "省";
            DataSet ds_shi = DbHelperSQL.Query("select AAA_CityList_City.c_number as 市编号,AAA_CityList_City.c_namestr as 市名,AAA_CityList_City.p_number as 所属省编号,(select AAA_CityList_Promary.p_namestr from AAA_CityList_Promary where AAA_CityList_Promary.p_number=AAA_CityList_City.p_number) as 所属省名 from AAA_CityList_City");
            ds_shi.Tables[0].TableName = "市";
            DataSet ds_qu = DbHelperSQL.Query("select AAA_CityList_qu.q_number as 区编号,AAA_CityList_qu.q_namestr as 区名,AAA_CityList_qu.c_number as 所属市编号,(select AAA_CityList_City.c_namestr from AAA_CityList_City where AAA_CityList_qu.c_number=AAA_CityList_City.c_number) as 所属市名 from AAA_CityList_qu");
            ds_qu.Tables[0].TableName = "区";
            PublisDsData.Tables.Add(ds_sheng.Tables[0].Copy());
            PublisDsData.Tables.Add(ds_shi.Tables[0].Copy());
            PublisDsData.Tables.Add(ds_qu.Tables[0].Copy());

            ViewState["DSPromary"] = PublisDsData.Tables["省"];
            ViewState ["DSCity"] = PublisDsData.Tables["市"];
            ViewState ["DSqu"] = PublisDsData.Tables["区"];

            CityList_Promary.Items.Clear();
            CityList_Promary.Items.Add("请选择省份");
            foreach (DataRow dr in ((DataTable)ViewState["DSPromary"]).Rows)
            {
                CityList_Promary.Items.Add(new ListItem(dr["省名"].ToString(), dr["省名"].ToString()));
            }
            CityList_Promary.SelectedIndex = 0;

            CityList_City.Items.Clear();
            CityList_City.Items.Add("请选择城市");
            CityList_City.SelectedIndex = 0;


            CityList_qu.Items.Clear();
            CityList_qu.Items.Add("请选择区县");
            CityList_qu.SelectedIndex = 0;
        }

        //private void UCCityList_Load(object sender, EventArgs e)
        //{
        //    //initdefault();

        //}

   
        protected void CityList_Promary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((DataTable )ViewState ["DSPromary"] == null ||(DataTable )ViewState ["DSCity"] == null ||(DataTable )ViewState ["DSqu"] == null)
            { return; }
            CityList_City.Items.Clear();
            CityList_City.Items.Add("请选择城市");

            foreach (DataRow dr in ((DataTable)ViewState["DSCity"]).Select("所属省名 = '" + CityList_Promary.SelectedValue.ToString() + "'"))
            {
                CityList_City.Items.Add(new ListItem(dr["市名"].ToString(), dr["市名"].ToString()));
            }
            //if (CityList_City.Items.Count == 2)
            //{
            //    CityList_City.SelectedIndex = 1;
            //}
            //else
            //{
                CityList_City.SelectedIndex = 0;
            //}
        }
        protected void CityList_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((DataTable)ViewState["DSPromary"] == null || (DataTable)ViewState["DSCity"] == null || (DataTable)ViewState["DSqu"] == null)
            { return; }

            CityList_qu.Items.Clear();
            CityList_qu.Items.Add("请选择区县");

            foreach (DataRow dr in((DataTable)ViewState["DSqu"]).Select("所属市名 = '" + CityList_City.SelectedValue.ToString() + "'"))
            {
                CityList_qu.Items.Add(new ListItem(dr["区名"].ToString(), dr["区名"].ToString()));
            }
            //if (CityList_qu.Items.Count == 2)
            //{
            //    CityList_qu.SelectedIndex = 1;
            //}
            //else
            //{
                CityList_qu.SelectedIndex = 0;
            //}
        }
        protected void CityList_qu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((DataTable)ViewState["DSPromary"] == null || (DataTable)ViewState["DSCity"] == null || (DataTable)ViewState["DSqu"] == null)
            { return; }
        }
}