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
    public partial class ucYXDTHD_C : UserControl
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucYXDTHD_C()
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
            //ht_where["search_tbname"] = " (select d.Number 中标定标表编号,d.Z_HTBH 电子购货合同编号,t.T_THDXDSJ 下达时间,'T'+t.Number 提货单编号,d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,t.T_THSL 提货数量,t.ZBDJ 定标价格,t.T_DJHKJE 提货金额,t.T_DJHKJE 冻结货款金额,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.F_DQZT 提货单状态, case d.Z_QPZT when '未开始清盘' then '定标' else case d.Z_HTZT when '定标合同到期' then '合同期满'+d.Z_QPZT when '定标合同终止' then '废标'+d.Z_QPZT when '定标执行完成' then '合同完成'+d.Z_QPZT else '异常'   end   end as '合同状态' from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where t.F_DQZT in ('未生成发货单','已生成发货单') and d.Y_YSYDDDLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "') as tab ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " 提货单编号 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 下达时间 ";  //用于排序的字段(必须设置)


            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "货物收发C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "已下达提货单";
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["合同编号"] = uctxtHTBH.Text.Trim();
            ht_tiaojian["提货单号"] = uctxtTHDBH.Text.Trim();
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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {

            //更改搜索条件
            setDefaultSearch();
            //ht_where["search_str_where"] = " 1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 电子购货合同编号 like '%" + uctxtHTBH.Text.Trim() + "%' and 提货单编号 like '%" + uctxtTHDBH.Text.Trim() + "%'";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }
      
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "电子购货合同" && e.RowIndex > -1)
            {
                int pagenumber = 8;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 2];
                string[] MKarr = new string[pagenumber + 2];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + dataGridView1.Rows[e.RowIndex].Cells["中标定标表编号"].Value;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT";

                }
                Hashtable htcs1 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标表编号"].Value;
                CSarr[pagenumber] = htcs1;
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_1";

                Hashtable htcs2 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["中标定标表编号"].Value;
                CSarr[pagenumber + 1] = htcs1;
                MKarr[pagenumber + 1] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_2";
                FormDZGHHT dy = new FormDZGHHT("电子购货合同", CSarr, MKarr, dataGridView1.Rows[e.RowIndex].Cells["中标定标表编号"].Value.ToString());
                dy.Show();
            }
            if (columnName == "thdbh" && e.RowIndex > -1)
            {
                string cs = dataGridView1.Rows[e.RowIndex].Cells["thdbh"].Value.ToString();
                fmDY dy = new fmDY("查看并打印提货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.THDDY" });
                dy.ShowDialog();
            }
        }
        private void ucYXDTHD_C_Load(object sender, EventArgs e)
        {
            GetData(null);
        }
        //导出
        private void btnExport_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "" };
            ////设置导出语句
            //string strSQL = "select * from (select d.Z_HTBH 电子购货合同编号,t.T_THDXDSJ 下达时间,'T'+t.Number 提货单编号,d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,t.T_THSL 提货数量,t.ZBDJ 定标价格,t.T_DJHKJE 提货金额,t.T_DJHKJE 冻结货款金额,'买方名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),'卖方名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.F_DQZT 提货单状态, case d.Z_QPZT when '未开始清盘' then '定标' else case d.Z_HTZT when '定标合同到期' then '合同期满'+d.Z_QPZT when '定标合同终止' then '废标'+d.Z_QPZT when '定标执行完成' then '合同完成'+d.Z_QPZT else '异常'   end   end as '合同状态' from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where  t.F_DQZT in ('未生成发货单','已生成发货单') and d.Y_YSYDDDLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "') as tab where  1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 电子购货合同编号 like '%" + uctxtHTBH.Text.Trim() + "%' and 提货单编号 like '%" + uctxtTHDBH.Text.Trim() + "%' order by 下达时间 asc";
            //CMyXls cMyXls1 = new CMyXls();
            //cMyXls1.BeginRunFrom_ht_where(strSQL, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "已下达提货单";
            ht_export["webmethod"] = "货物收发C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];
            //string[] HideColumns = new string[] { "" };
            //ht_export["HideColumns"] = HideColumns;
            
            cMyXls1.BeginRunFrom_ht_where(ht_export);

        }
    }
}
