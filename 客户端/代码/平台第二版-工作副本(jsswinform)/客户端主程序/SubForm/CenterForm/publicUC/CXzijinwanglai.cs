using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.CenterForm.publicUC
{
    public partial class CXzijinwanglai : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public CXzijinwanglai()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
            //处理下拉框间距
            this.CBjslx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            this.CBzjlx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            this.CBsylx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            this.CByslx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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
        //加载窗体后处理数据
        private void UserControl1_Load(object sender, EventArgs e)
        {
            GetData(null);
        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_BeginBind(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_BeginBind_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件(线程获取到分页数据后的具体绑定处理)
        private void ShowThreadResult_BeginBind_Invoke(Hashtable OutPutHT)
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
                    dgvcs.Padding = new Padding(10,0,0,0);

                    this.dataGridView1.Columns[i].HeaderCell.Style = dgvcs;
                    this.dataGridView1.Columns[i].DefaultCellStyle = dgvcs;

                }
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            

        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            string DLYX = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();

            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  (select '流水号'=Number,'角色编号'=JSBH,'发生时间'=CSSJ,'资金类型'=ZJLX,'使用类型'=SYLX,'变动金额'=BDJE,'运算类型'=JEYSLX,'变更前余额'=BDQZHYE,'变更后余额'=BDHZHYE,'变更说明'=BGSM from ZZ_ZKLSMXB where DLYX = '" + DLYX + "') as tab  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " 流水号 ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " 流水号 ";  //用于排序的字段(必须设置)
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






        private void basicButton2_Click(object sender, EventArgs e)
        {
            //获取搜索条件
            string begintime = dateTimePicker11.Value.ToShortDateString() + " 00:00:01";
            string endtime = dateTimePicker22.Value.ToShortDateString() + " 23:59:59";
            string jslx = CBjslx.Text; //角色类型
            string zjlx = CBzjlx.Text; //资金类型
            string sylx = CBsylx.Text; //使用类型
            string yslx = CByslx.Text; //运算类型
            //更改搜索条件
            setDefaultSearch();
            ht_where["search_str_where"] = ht_where["search_str_where"] + " and 发生时间 > '" + begintime + "' and 发生时间 < '" + endtime + "' ";
            if (jslx != "不限")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 角色类型='" + jslx + "' ";
            }
            if (zjlx != "不限")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 资金类型='" + zjlx + "' ";
            }
            if (sylx != "不限")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 使用类型='" + sylx + "' ";
            }
            if (yslx != "不限")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 运算类型='" + yslx + "' ";
            }
            //执行查询
            GetData(ht_where);
        }

        private void panelUC5_Paint(object sender, PaintEventArgs e)
        {
            CBjslx.SelectedIndex = 0;
            CBzjlx.SelectedIndex = 0;
            CBsylx.SelectedIndex = 0;
            CByslx.SelectedIndex = 0;
        }
    }
}
