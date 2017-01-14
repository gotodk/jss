using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.CenterForm.Buyer
{
    public partial class CXzhongbiao : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public CXzhongbiao()
        {
            InitializeComponent();

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);
            //处理下拉框间距
            this.CBleixing.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
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
            e.Graphics.DrawString(CBthis.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X + 2, e.Bounds.Y + 2);
        }

        //加载窗体后处理数据
        private void UserControl1_Load(object sender, EventArgs e)
        {
            CBleixing.SelectedIndex = 0;
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
            string BUYjsbh = PublicDS.PublisDsUser.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  ( select '合同编号'=Number ,'预订单号'=BUYYDDBH,'商品编号'=SPBH,'商品名称'=SPMC,'型号规格'=GGXH,'计价单位'=JJDW,'供货周期'=HTQX, '中标价格'=SELTBJG,'买入价格'=BUYMRJG,'预订量'=ZBSL,'中标预订单金额'=ZBZJE,'订金'=BUYYDDYSDJJE,'下达时间'=BUYXDYDDSJ,'中标时间'=ZBSJ from ZZ_ZBDBXXB where BUYJSBH = '" + BUYjsbh + "' and ZBDBZT='中标' ) as tab  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " 合同编号 ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " 中标时间 ";  //用于排序的字段(必须设置)
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







        private void BBsearch_Click(object sender, EventArgs e)
        {
            //获取搜索条件
            string key = TBkey.Text.Trim();
            string htzq = CBleixing.Text;
            string begintime = dateTimePickerBegin.Value.ToShortDateString() + " 00:00:01";
            string endtime = dateTimePickerEnd.Value.ToShortDateString() + " 23:59:59";
            //更改搜索条件
            setDefaultSearch();
            ht_where["search_str_where"] = ht_where["search_str_where"] + " and 中标时间 > '" + begintime + "' and 中标时间 < '" + endtime + "' ";  //检索条件(必须设置)
            if (htzq != "不限周期")
            {
                ht_where["search_str_where"] = ht_where["search_str_where"] + " and 供货周期='" + htzq + "' ";
            }
            ht_where["search_str_where"] = ht_where["search_str_where"] + " and (预订单号 like '%" + key + "%' or 商品编号 like '%" + key + "%' or 商品名称 like '%" + key + "%') ";
            //执行查询
            GetData(ht_where);
        }
    }
}
