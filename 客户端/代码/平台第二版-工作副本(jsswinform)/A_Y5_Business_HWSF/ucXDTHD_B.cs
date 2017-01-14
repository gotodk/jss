using System;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;
using 客户端主程序.SubForm.NewCenterForm.MuBan;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    public partial class ucXDTHD_B : UserControl
    {
         /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucXDTHD_B()
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
            //ht_where["search_tbname"] = " (select a.Number,a.Z_HTBH 电子购货合同编号,a.Z_SPMC 商品名称,a.Z_SPBH 商品编号,a.Z_GG 商品规格,b.I_JYFMC 卖家名称,'买家名称'=(select I_JYFMC from AAA_DLZHXXB where AAA_DLZHXXB.B_DLYX=a.Y_YSYDDDLYX),isnull(Z_ZBJG,0.00) 定标价格,isnull((convert(int,Z_ZBSL)-convert(int,Z_YTHSL)),0) 可提货数量,isnull(a.T_YSTBDMJSDJJPL,0) 经济批量,isnull(a.Z_PTSDZDJJPL,0) 平台设定最大经济批量,a.YSYDDTJSJ 原始预订单提交时间,a.Y_YSYDDSHQY 原始预订单收货区域,Z_LYBZJJE 履约保证金金额,T_YSTBDDLYX 原始投标单登陆邮箱,Z_HTBH 合同编号,a.Z_HTQX 合同期限 from dbo.AAA_ZBDBXXB a left Join AAA_DLZHXXB b on a.T_YSTBDDLYX=b.B_DLYX  where  isnull((convert(int,Z_ZBSL)-convert(int,Z_YTHSL)),0)>0 and Z_HTZT='定标' and Y_YSYDDDLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "') as tab ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 原始预订单提交时间 ";  //用于排序的字段(必须设置)
            
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "货物收发B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "下达提货单";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["合同编号"] = txtDZGHHTBH.Text.Trim();
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
        public void GetData()
        {
            setDefaultSearch();
            ucPager1.HT_Where = ht_where;
            ucPager1.BeginBindForUCPager();
        }
              
        private void uc_XDTHD_B_Load(object sender, EventArgs e)
        {
            GetData();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            //ht_where["search_str_where"] = " 1=1 and 电子购货合同编号 like '%" + txtDZGHHTBH.Text.Trim() + "%' and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' ";  //检索条件(必须设置)
            //执行查询
                  
            GetData();
        }

        public string number = "";//中标定标的编号
        public string kthsl = "";//可提货数量
        public string dbjg = "";//定标价格
        public string jjpl = "";//经济批量
        public string jyfmc = "";//交易方名称
        public string yyddshqu = "";//原始预订单收货区域
        public string htbh = "";//电子购货合同编号
        public string htzt = "";//合同状态
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "操作" && e.RowIndex > -1)
            {
                int rowindex = e.RowIndex;
                number = dataGridView1.Rows[rowindex].Cells["Column1"].Value.ToString();//中标定标的编号
                kthsl = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells["hthsl"].Value.ToString()).ToString();//可提货数量
                dbjg = Convert.ToDouble(dataGridView1.Rows[rowindex].Cells["jg"].Value.ToString()).ToString();//定标价格
                jjpl = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells["经济批量"].Value.ToString()).ToString() ;//经济批量
                jyfmc = dataGridView1.Rows[rowindex].Cells["买家名称"].Value.ToString();//交易方名称
                yyddshqu = dataGridView1.Rows[rowindex].Cells["shqy"].Value.ToString();//原始预订单收货区域
                htbh = dataGridView1.Rows[rowindex].Cells["合同编号"].Value.ToString();//电子购货合同编号
                htzt = dataGridView1.Rows[rowindex].Cells["合同期限"].Value.ToString();//合同状态
                tihuodanPutIn put = new tihuodanPutIn(this);
                put.ShowDialog();
            }
            else if (columnName == "电子购货合同编号"&&e.RowIndex>-1)
            {
                int pagenumber = 8;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 2];
                string[] MKarr = new string[pagenumber + 2];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT";

                }
                Hashtable htcs1 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value;
                CSarr[pagenumber] = htcs1;
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_1";

                Hashtable htcs2 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value;
                CSarr[pagenumber + 1] = htcs1;
                MKarr[pagenumber + 1] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_2";
                FormDZGHHT dy = new FormDZGHHT("电子购货合同", CSarr, MKarr, dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString());
                dy.Show();
            }
        }
    }
}
