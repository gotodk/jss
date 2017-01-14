using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    public partial class ucJJFWGJL_C : UserControl
    {
        public ucJJFWGJL_C()
        {
         InitializeComponent();
              //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }

             /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        //string tab = "";
        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {           
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " a.*,b.* "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " AAA_View_JYFWGJL as a left join AAA_View_JJRGLJYFinfo as b on a.交易方账号=b.交易方账号 and a.关联经纪人账号=b.关联经纪人账号 ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " a.关联经纪人账号='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 违约时间 ";  //用于排序的字段(必须设置)

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "经纪人业务C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "交易方违规记录";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["交易方名称"] = txtJYFMC.Text.Trim();
            string[] ssq = ucCityList1.SelectedItem;
            ht_tiaojian["省份"] = ssq[0].ToString();
            ht_tiaojian["地市"] = ssq[1].ToString();
            ht_tiaojian["区县"] = ssq[2].ToString();           
            ht_where["tiaojian"] = ht_tiaojian;
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void STR_BeginBind(Hashtable returnHT)
        {
            try { Invoke(new delegateForThreadShow(STR_BeginBind_Invoke), new Hashtable[] { returnHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件(线程获取到分页数据后的具体绑定处理)
        private void STR_BeginBind_Invoke(Hashtable OutPutHT)
        {
            DataSet ds = (DataSet)OutPutHT["数据"];//获取返回的数据集
            int fys = (int)ds.Tables["附加数据"].Rows[0]["分页数"];
            int jls = (int)ds.Tables["附加数据"].Rows[0]["记录数"];
            string msg = (string)ds.Tables["附加数据"].Rows[0]["其他描述"];
            string err = (string)ds.Tables["附加数据"].Rows[0]["执行错误"];

            //是否自动绑定列
            dataGridView1.AutoGenerateColumns = false;

            //若执行正常
            if (ds.Tables.Contains("主要数据"))
            {
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                dataGridView1.DataSource = null;
            }
            //取消列表的自动排序
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                if (i == 0)
                {
                    //让第一列留出一点空白
                    DataGridViewCellStyle dgvcs = new DataGridViewCellStyle();
                    dgvcs.Padding = new Padding(10, 0, 0, 0);
                    this.dataGridView1.Columns[i].HeaderCell.Style = dgvcs;
                    this.dataGridView1.Columns[i].DefaultCellStyle = dgvcs;
                }
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// 设置条件，获取通过分页控件数据,留空则使用默认条件
        /// </summary>
        /// <param name="HT_Where_temp">留空则使用默认条件</param>
        private void GetData(Hashtable HT_Where_temp)
        {
            if (HT_Where_temp == null)
            {
                setDefaultSearch();
            }
            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();
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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {

            //更改搜索条件
            setDefaultSearch();

            //if (txtJYFMC.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = ht_where["search_str_where"] + " and 交易方名称 like '%" + txtJYFMC.Text.Trim() + "%' ";  //检索条件(必须设置)
            //}
            //string[] ssq = ucCityList1.SelectedItem;
            //string sql_sqq = "";
            //if (ssq[0] != "请选择省份")
            //{
            //    sql_sqq = sql_sqq + " and  所属省='" + ssq[0] + "' ";
            //}
            //if (ssq[1] != "请选择城市")
            //{
            //    sql_sqq = sql_sqq + " and  所属市='" + ssq[1] + "' ";
            //}
            //if (ssq[2] != "请选择区县")
            //{
            //    sql_sqq = sql_sqq + " and  所属区县='" + ssq[2] + "' ";
            //}
            //ht_where["search_str_where"] = ht_where["search_str_where"] + " " + sql_sqq + "  ";
            //执行查询
            GetData(ht_where);
        }
        private void ucJJFWGJL_C_Load(object sender, EventArgs e)
        {
            ucCityList1.initdefault();
            GetData(null);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //string sql = "select a.交易方账号,b.交易方名称,b.是否当前关联,b.所属区域,b.注册类型,b.联系人,b.联系电话,a.违约事项,a.违约赔偿金金额,a.违约时间 from AAA_View_JYFWGJL as a left join AAA_View_JJRGLJYFinfo as b on a.交易方账号=b.交易方账号 and a.关联经纪人账号=b.关联经纪人账号 where " + ht_where["search_str_where"].ToString() + " order by 违约时间 DESC";
            //string[] HideColumns = new string[] { };
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "交易方违规记录";
            ht_export["webmethod"] = "经纪人业务C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];           

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }

    }
}
