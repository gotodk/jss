using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;
using 客户端主程序.Support;
using 客户端主程序.NewDataControl;


namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucCGX_C : UserControl
    {

           /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucCGX_C()
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

            //ht_where["serach_Row_str"] = " tab.*,b.SPMC as 商品名称,b.GG as 规格 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " (select DLYX, '投标单' as  '单据类型', Number as 单据编号,HTQX as '合同期限',SPBH as  '商品编号',TBNSL as '数量',TBJG as '价格',TBJE as '金额',CreateTime from AAA_TBDCGB  union all select DLYX,'预订单' as  '单据类型',Number as 单据编号,HTQX as '合同期限',SPBH as '商品编号',NDGSL as '数量',NMRJG as '价格',NDGJE as '金额',CreateTime from AAA_YDDCGB) as tab left join AAA_PTSPXXB as b on tab.商品编号 = b.SPBH ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " tab.单据编号+tab.单据类型 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " tab.DLYX = '" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' "; //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " CreateTime ";  //用于排序的字段(必须设置)  

            string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "草稿箱";
            ht_tiaojian["用户邮箱"] = dlyx;
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
        private void GetData()
        {
            setDefaultSearch();
            //查询条件
            //if (!txtSPMC.Text.Trim().Equals(""))
            //{
            //    ht_where["search_str_where"] += " and b.SPMC like '%" + txtSPMC.Text.Trim() + "%' ";
            //}
            //if (!cbxDJLB.SelectedItem.ToString().Trim().Equals("请选择单据类别"))
            //{
            //    ht_where["search_str_where"] += " and 单据类型='" + cbxDJLB.SelectedItem.ToString().Trim() + "' ";
            //}

            Hashtable ht_tiaojian = (Hashtable)ht_where["tiaojian"];
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["单据类型"] = cbxDJLB.SelectedItem.ToString();

            ht_where["tiaojian"] = ht_tiaojian;
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

        private void CBXM_DrawItem(object sender, DrawItemEventArgs e)
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
           
            //执行查询
            GetData();
        }

      
        private void ucCGX_C_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            cbxDJLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            cbxDJLB.SelectedIndex = 0;
            GetData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //string[] HideColumns = new string[] { "CreateTime","Number" };            
            //string sql = " select tab.单据类型,tab.商品编号,b.SPMC as 商品名称,b.GG as 规格,tab.合同期限,tab.数量,tab.价格,tab.金额,tab.CreateTime,tab.Number from (select DLYX, '投标单' as  '单据类型', Number,HTQX as '合同期限',SPBH as  '商品编号',TBNSL as '数量',TBJG as '价格',TBJE as '金额',CreateTime from AAA_TBDCGB  union all select DLYX,'预订单' as  '单据类型',Number,HTQX as '合同期限',SPBH as  '商品编号',NDGSL as '数量',NMRJG as '价格',NDGJE as '金额',CreateTime from AAA_YDDCGB) as tab left join  AAA_PTSPXXB as b on tab.商品编号 = b.SPBH where " + ht_where["search_str_where"].ToString() + " order by tab.CreateTime ASC";
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "草稿箱";
            ht_export["webmethod"] = "商品买卖C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];
            //string[] HideColumns = new string[] { "CreateTime", "Number" };
            //ht_export["HideColumns"] = HideColumns;

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           


            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
             Hashtable httt = new Hashtable();
            if (columnName == "使用草稿" && e.RowIndex > -1)
            {


                DataGridViewRow dgr = this.dataGridView1.Rows[e.RowIndex];

                string Number = dgr.Cells["单据编号"].Value.ToString().Trim();
                string Strtype = dgr.Cells["单据类型"].Value.ToString().Trim();
                string jsbh = "";




              
                if (Strtype.Equals("投标单"))
                {
                    fmTBDCGTJ2 tbdcg = new fmTBDCGTJ2(Number);
                    tbdcg.ShowDialog();
                }
                if (Strtype.Equals("预订单"))
                {
                    fmYDDCGTJ2 cgtj = new fmYDDCGTJ2(Number);
                    cgtj.ShowDialog();
                }
              
                
            }
        }
    }
}
