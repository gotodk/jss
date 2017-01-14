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

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class ucQP_C : UserControl
    {
          /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        public ucQP_C()
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
           
            string dlyx= PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();           

            //ht_where["serach_Row_str"] = " Number,CONVERT(varchar(20), Z_QPKSSJ, 20) as '清盘开始时间',(case  when  Z_QPJSSJ IS NULL then '---' else CONVERT(varchar(20), Z_QPJSSJ, 20)   end) as '清盘结束时间',(CASE Z_HTZT WHEN '未定标废标' THEN '废标' WHEN '定标合同到期' THEN '合同期满' WHEN '定标合同终止' THEN '废标' WHEN '定标执行完成' THEN '合同完成' ELSE '其他' END) AS 清盘原因,(case when Z_QPKSSJ=Z_QPJSSJ then '自动清盘' else '人工清盘' end) as 清盘类型,Z_HTBH as 合同编号 , Z_QPZT as 清盘状态,(case Y_YSYDDDLYX when '"+dlyx+"' then convert(varchar(20),Z_DJJE) else '---' end) as 订金解冻金额 ,(case  when T_YSTBDDLYX='"+dlyx+"' and Z_QPZT='清盘结束' then convert(varchar(20),Z_LYBZJJE) else '---' end)  as 履约保证金解冻金额 "; //检索字段(必须设置)
            //ht_where["search_tbname"] = " AAA_ZBDBXXB ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " (T_YSTBDDLYX='"+dlyx+"' or Y_YSYDDDLYX='"+dlyx+"')  and (Z_QPZT='清盘中' or Z_QPZT='清盘结束') ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " Z_QPKSSJ ";  //用于排序的字段(必须设置)   

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖C区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "清盘";
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
            //if (!ucTextBox1.Text.Trim().Equals("电子购货合同编号") && !ucTextBox1.Text.Trim().Equals(""))
            //{
            //    ht_where["search_str_where"] += " and Z_HTBH like '%" + ucTextBox1.Text.Trim() + "%' ";
            //}
            //if (!cbxDJLB.SelectedItem.ToString().Trim().Equals("请选择清盘状态"))
            //{
            //    ht_where["search_str_where"] += " and Z_QPZT='" + cbxDJLB.SelectedItem.ToString().Trim() + "' ";
            //}
            //if (!cbxQPLX.SelectedItem.ToString().Trim().Equals("请选择清盘类型"))
            //{
            //    ht_where["search_str_where"] += " and (case when Z_QPKSSJ=Z_QPJSSJ then '自动清盘' else '人工清盘' end)='" + cbxQPLX.SelectedItem.ToString().Trim() + "' ";
            //}

            Hashtable ht_tiaojian = (Hashtable)ht_where["tiaojian"];
            ht_tiaojian["合同编号"] = ucTextBox1.Text.Trim();
            ht_tiaojian["清盘状态"] = cbxDJLB.SelectedItem.ToString();
            ht_tiaojian["清盘类型"] = cbxQPLX.SelectedItem.ToString();

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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {

            //更改搜索条件
            setDefaultSearch();
           //检索条件(必须设置)
            //执行查询
            GetData();
        }


        private void ucQP_C_Load(object sender, EventArgs e)
        {
            //处理下拉框间距
            cbxDJLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            cbxDJLB.SelectedIndex = 0;
            //处理下拉框间距
            cbxQPLX.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.QPLX_DrawItem);
            cbxQPLX.SelectedIndex = 0;
            GetData();
        }

        //处理下拉框间距
        private void QPLX_DrawItem(object sender, DrawItemEventArgs e)
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            //string[] HideColumns = new string[] { };            
            //string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            //string sql = "select CONVERT(varchar(20), Z_QPKSSJ, 20) as '清盘开始时间',(CASE Z_HTZT WHEN '未定标废标' THEN '废标' WHEN '定标合同到期' THEN '合同期满' WHEN '定标合同终止' THEN '废标' WHEN '定标执行完成' THEN '合同完成' ELSE '其他' END) AS 清盘原因,(case when Z_QPKSSJ=Z_QPJSSJ then '自动清盘' else '人工清盘' end) as 清盘类型, Z_QPZT as 清盘状态,(case  when  Z_QPJSSJ IS NULL then '---' else CONVERT(varchar(20), Z_QPJSSJ, 20)   end) as '清盘结束时间',Z_HTBH as 电子购货合同编号 ,(case Y_YSYDDDLYX when '" + dlyx + "' then convert(varchar(20),Z_DJJE) else '---' end) as 订金解冻金额 ,(case  when T_YSTBDDLYX='" + dlyx + "' and Z_QPZT='清盘结束' then convert(varchar(20),Z_LYBZJJE) else '---' end)  as 履约保证金解冻金额 from AAA_ZBDBXXB where " + ht_where["search_str_where"].ToString() + " order by Z_QPKSSJ ASC";
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "清盘";
            ht_export["webmethod"] = "商品买卖C区导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];
            //string[] HideColumns = new string[] { "" };
            //ht_export["HideColumns"] = HideColumns;

            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;


            if (columnName == "清盘详情" && e.RowIndex > -1)
            {
                DataGridViewRow dgr = this.dataGridView1.Rows[e.RowIndex];
                string Strtype = dgr.Cells["清盘类型"].Value.ToString().Trim();
                string Number = dgr.Cells["Number"].Value.ToString().Trim();
                if (Strtype.Equals("自动清盘"))
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add(" ");
                    Almsg3.Add("自动清盘数据没有清盘详情可查看！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", Almsg3);
                    FRSE3.ShowDialog();
                    return;
                }
                if (Strtype.Equals("人工清盘"))
                {
                    Hashtable ht = new Hashtable();
                    ht["Number"] = Number;
                    FormQPDetailcheck fpr = new FormQPDetailcheck(ht);
                    fpr.ShowDialog();
                }
            }

            if (columnName == "电子购货合同编号" && e.RowIndex > -1)
            {
                int pagenumber = 8;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 2];
                string[] MKarr = new string[pagenumber + 2];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + dataGridView1.Rows[e.RowIndex].Cells["Number"].Value;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT";

                }
                Hashtable htcs1 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["Number"].Value;
                CSarr[pagenumber] = htcs1;
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_1";

                Hashtable htcs2 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["Number"].Value;
                CSarr[pagenumber + 1] = htcs1;
                MKarr[pagenumber + 1] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_2";
                FormDZGHHT dy = new FormDZGHHT("电子购货合同", CSarr, MKarr, dataGridView1.Rows[e.RowIndex].Cells["Number"].Value.ToString());
                dy.Show();
            }
        }

     
    }
}
