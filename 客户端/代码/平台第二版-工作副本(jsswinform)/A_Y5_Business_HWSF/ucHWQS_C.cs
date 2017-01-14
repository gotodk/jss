using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;
using 客户端主程序.SubForm.NewCenterForm.MuBan;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    public partial class ucHWQS_C : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucHWQS_C()
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
            //ht_where["search_tbname"] = "  (select b.Number 中标定标信息表编号,F_BUYWYYSHCZSJ as 签收时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 提货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 提货金额,a.T_THSL as 无异议签收数量 ,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买家名称,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖家名称 ,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号,a.F_FPHM as 发票号码, b.Z_HTBH as 电子合同编号, case b.Z_QPZT when '未开始清盘' then '定标' else case b.Z_HTZT when '定标合同到期' then '合同期满'+b.Z_QPZT when '定标合同终止' then '废标'+b.Z_QPZT when '定标执行完成' then '合同完成'+b.Z_QPZT else '异常'   end   end as '合同状态' from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  where F_DQZT in ('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货') and (b.Y_YSYDDMJJSBH = '" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString() + "' or b.T_YSTBDMJJSBH ='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "') ) as tab  ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " 发货单号 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " desc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 签收时间 ";  //用于排序的字段(必须设置)

            ////查询条件
            //if (!txtSPMC.Text.Trim().Equals(""))
            //{
            //    ht_where["search_str_where"] += " and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' ";
            //}
            //if (!ucTextBox1.Text.Trim().Equals(""))
            //{
            //    ht_where["search_str_where"] += " and 发货单号 like '%" + ucTextBox1.Text.Trim() + "%' ";
            //}

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "货物收发C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "货物签收";
            ht_tiaojian["买家编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            ht_tiaojian["卖家编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["发货单号"] = ucTextBox1.Text.Trim();
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

            //执行查询
            GetData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //对应的电子购货合同编号
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "对应电子购货合同编号" && e.RowIndex > -1)
            {
                int pagenumber = 8;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 2];
                string[] MKarr = new string[pagenumber + 2];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT";

                }
                Hashtable htcs1 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value;
                CSarr[pagenumber] = htcs1;
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_1";

                Hashtable htcs2 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value;
                CSarr[pagenumber + 1] = htcs1;
                MKarr[pagenumber + 1] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_2";
                FormDZGHHT dy = new FormDZGHHT("电子购货合同", CSarr, MKarr, dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value.ToString());
                dy.Show();
            }
            if (columnName == "发货单号" && e.RowIndex > -1)
            {
                string cs = dataGridView1.Rows[e.RowIndex].Cells["发货单号"].Value.ToString();
                fmDY dy = new fmDY("查看并打印发货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.FHDDY" });
                dy.ShowDialog();

            }
        }

        private void ucHWQS_C_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "" };
            ////设置导出语句
            //string sql = "select * from  (select F_BUYWYYSHCZSJ as 签收时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 提货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 提货金额,a.T_THSL as 无异议签收数量 ,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买方名称,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖方名称 ,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号再再再,a.F_FPHM as 发票号码, b.Z_HTBH as 电子合同编号,case b.Z_QPZT when '未开始清盘' then '定标' else case b.Z_HTZT when '定标合同到期' then '合同期满'+b.Z_QPZT when '定标合同终止' then '废标'+b.Z_QPZT when '定标执行完成' then '合同完成'+b.Z_QPZT else '异常'   end   end as '合同状态' from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  where F_DQZT in ('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货') and b.Y_YSYDDMJJSBH = '" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString() + "' ) as tab  where  " + ht_where["search_str_where"].ToString() + " order by 签收时间 desc";
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "货物签收";
            ht_export["webmethod"] = "货物收发C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];
            //string[] HideColumns = new string[] { "" };
            //ht_export["HideColumns"] = HideColumns;

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }
    }
}
