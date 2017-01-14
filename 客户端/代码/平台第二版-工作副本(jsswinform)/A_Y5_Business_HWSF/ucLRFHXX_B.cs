using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    public partial class ucLRFHXX_B : UserControl
    {


        Support.ManageDgv Dgv;

            /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        string PX = "按距最迟发货日天数排序";
          public ucLRFHXX_B()
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
            //ht_where["search_tbname"] = "(select d.Z_SPMC 商品名称,d.Z_GG 规格,'F'+t.Number 发货单编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),d.Z_ZBJG 定标价格,t.T_THSL 提货数量,t.F_FPSFSHTH 发票是否随货同行,DATEDIFF(dd,getdate(),t.T_ZCFHR) 距最迟发货日天数,t.ZBDBXXBBH 中标定标信息表编号,t.F_DQZT 发货单状态,t.F_FPYJXXLRSJ 发票录入时间,d.Y_YSYDDSHQY 买家收货区域 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where d.Z_HTZT='定标' and (t.F_DQZT='已生成发货单' or (t.F_DQZT not in ('未生成发货单','卖家主动退货','撤销') and t.F_FPYJXXLRSJ is null)) and d.T_YSTBDMJJSBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "') as tab";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " 发货单编号 ";  //所检索表的主键(必须设置)

            //ht_where["search_str_where"] = " 1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 发货单编号 like '%" + txtFHDBH.Text.Trim() + "%'";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //switch (PX)
            //{
            //    case "按收货区域排序":
            //        ht_where["search_paixuZD"] = " 买家收货区域 asc, 距最迟发货日天数 ";  //用于排序的字段(必须设置)
            //        break;
            //    default:
            //        ht_where["search_paixuZD"] = " 距最迟发货日天数 ";  //用于排序的字段(必须设置)
            //        break;
            //}

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "货物收发B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "录入发货信息";
            ht_tiaojian["卖方编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            ht_tiaojian["提货单号"] = txtFHDBH.Text.Trim();
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_tiaojian["排序"] = PX;
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
            Dgv = new Support.ManageDgv(this.dataGridView1);
           // Dgv.AddMergeColumns(9, 2, "操作", -1); 将操作放到列表最前列 wyH 2014.09.05
            Dgv.AddMergeColumns(0, 2, "操作", -1);
            //若执行正常
            if (ds.Tables.Contains("主要数据"))
            {
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                dataGridView1.DataSource = null;
            }

            //根据行内容，处理按钮是否可用
            for (int p = 0; p < dataGridView1.RowCount; p++)
            {
                //根据某列的值，更改操作列的按钮 130307000157
                if (dataGridView1["发票是否随货同行", p].Value.ToString() == "是")
                {
                    dataGridView1["录入发票邮寄信息", p] = new DataGridViewTextBoxCell();
                    dataGridView1["录入发票邮寄信息", p].Value = "无需录入发票邮寄信息";//换成想要显示的文字
                    dataGridView1["录入发票邮寄信息", p].Tag = "不让操作哦"; //这五个字固定的，用于点击时的判断
                }
                else
                {
                    if (dataGridView1["发票录入时间", p].Value.ToString() != "")
                    {
                        dataGridView1["录入发票邮寄信息", p] = new DataGridViewTextBoxCell();
                        dataGridView1["录入发票邮寄信息", p].Value = "已录入发票邮寄信息";//换成想要显示的文字
                        dataGridView1["录入发票邮寄信息", p].Tag = "不让操作哦"; //这五个字固定的，用于点击时的判断
                    }
                }

                if (dataGridView1["发货单状态", p].Value.ToString() != "已生成发货单")
                {
                    dataGridView1["录入发货信息", p] = new DataGridViewTextBoxCell();
                    dataGridView1["录入发货信息", p].Value = "已录入发货信息";//换成想要显示的文字
                    dataGridView1["录入发货信息", p].Tag = "不让操作哦"; //这五个字固定的，用于点击时的判断
                }

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
            //ht_where["search_str_where"] = " 1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 发货单编号 like '%" + txtFHDBH.Text.Trim() + "%'";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }
    
        private void ucLRFHXX_B_Load(object sender, EventArgs e)
        {
            GetData(null);
        }

        public string fhdbh = "";//发货单编号
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "发货单编号" && e.RowIndex > -1)
            {
                string cs = dataGridView1.Rows[e.RowIndex].Cells["发货单编号"].Value.ToString();
                fmDY dy = new fmDY("查看并打印发货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.FHDDY" });
                dy.ShowDialog();
            }
            else if (columnName == "录入发货信息" && e.RowIndex > -1)
            {
                //特殊处理，让被禁用的行不响应操作
                if (dataGridView1.Rows[e.RowIndex].Cells["录入发货信息"].Tag != null && dataGridView1.Rows[e.RowIndex].Cells["录入发货信息"].Tag.ToString() == "不让操作哦")
                {
                    return;
                }
                else
                {
                    fhdbh = dataGridView1.Rows[e.RowIndex].Cells["发货单编号"].Value.ToString();
                    LRFHXX fhxx = new LRFHXX(this);
                    fhxx.ShowDialog();
                }
            }
            else if (columnName == "录入发票邮寄信息" && e.RowIndex > -1)
            {
                //特殊处理，让被禁用的行不响应操作
                if (dataGridView1.Rows[e.RowIndex].Cells["录入发票邮寄信息"].Tag != null && dataGridView1.Rows[e.RowIndex].Cells["录入发票邮寄信息"].Tag.ToString() == "不让操作哦")
                {
                    return;
                }
                else
                {
                    fhdbh = dataGridView1.Rows[e.RowIndex].Cells["发货单编号"].Value.ToString();
                    LRFPXX fpxx = new LRFPXX(this);
                    fpxx.ShowDialog();
                }
            }
        }

        private void CKBoxSHAdress_CheckedChanged(object sender, bool Checked)
        {
            if (CKBoxSHAdress.Checked == true)
            {
                PX = "按收货区域排序";
            }
            else
            {
                PX = "按距最迟发货日天数排序";
            }
            //更改搜索条件
            setDefaultSearch();
            //执行查询
            GetData(ht_where);
        }
    }
}
