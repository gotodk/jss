using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;
using System.Threading;
using 客户端主程序.NewDataControl;
using 客户端主程序.Support;


namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucYCTBDGL_B : UserControl
    {
            /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        public ucYCTBDGL_B()
        {
         InitializeComponent();
                    //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(STR_BeginBind);
        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " * "; //检索字段(必须设置)

            //ht_where["search_tbname"] = "  (select a.FWZXSHSJ,b.Number 投标单号,b.SPBH 商品编号,b.SPMC 商品名称,b.GG 规格,b.ZT 投标单状态,b.HTQX 合同期限,a.JYGLBSHWTGHSFXG 是否修改,isnull(convert(varchar(20),a.MJZXXGSJ,120),'--') 最后修改时间 from AAA_TBZLSHB a left join AAA_TBD b on a.TBDH=b.Number where b.DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' and a.FWZXSHZT='审核异常' and a.JYGLBSHZT<>'审核通过') AS Table1  ";

            //ht_where["search_mainid"] = " 投标单号 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " ASC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " FWZXSHSJ ";  //用于排序的字段(必须设置)
            //if (txtSPMC.Text.Trim()!="")
            //{
            //    ht_where["search_str_where"] += " and 商品名称 like '%" + txtSPMC.Text.Trim() + "%'";
            //}

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "异常投标单";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
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
        public void GetData(Hashtable HT_Where_temp)
        {
            if (HT_Where_temp == null)
            {
                setDefaultSearch();
            }
            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();
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
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }

        private void ucYCTBDGL_B_Load(object sender, EventArgs e)
        {
           
            GetData(null);
        }
        public string TBDH = "";
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "Column11" && e.RowIndex >= 0)
            {
                TBDH = dataGridView1.Rows[e.RowIndex].Cells["投标单号"].Value.ToString();
                YCTBD_CKXQ xq = new YCTBD_CKXQ(this);
                xq.ShowDialog();
            }
        }
        
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            GetData(null);
        }
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucYCTBDGL_B YDD = new ucYCTBDGL_B();
            YDD.Dock = DockStyle.Fill;//铺满
            YDD.AutoScroll = true;//出现滚动条
            YDD.BackColor = Color.AliceBlue;
            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(YDD); //加入到某一个panel中
            YDD.Show();//显示出来
        }


    }
}
