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
    public partial class ucSCFHD_C : UserControl
    {
              /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        string PX = "按距最迟发货日天数排序";
        public ucSCFHD_C()
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

            //ht_where["search_tbname"] = " (select d.Z_SPMC 商品名称,d.Z_GG 规格,'T'+t.Number 提货单编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),d.Y_YSYDDSHQY 买家收货区域,d.Z_ZBJG 定标价格,t.T_THSL 提货数量,DATEDIFF(dd,getdate(),t.T_ZCFHR) 距最迟发货日天数,t.ZBDBXXBBH 中标定标信息表编号,t.F_FPYJXXLRSJ 发票邮寄信息录入时间,t.F_FPHM 发票号码,t.F_FPSFSHTH 发票是否随货同行,t.T_THDXDSJ 提货单下达时间,t.F_SELTYZXFHCZSJ 卖家同意重新发货操作时间  from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where d.Z_HTZT='定标' and t.F_DQZT='未生成发货单' and d.T_YSTBDMJJSBH='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "') as tab ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " 提货单编号 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 提货单编号 like '%" + uctxtTHDBH.Text.Trim() + "%'";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //switch (PX)
            //{
            //    case "按收货区域排序":
            //ht_where["search_paixuZD"] = " 买家收货区域 asc, 距最迟发货日天数 ";  //用于排序的字段(必须设置)
            //        break;
            //    default:
            //ht_where["search_paixuZD"] = " 距最迟发货日天数 asc,提货单下达时间 ";  //用于排序的字段(必须设置)
            //        break;
            //}


            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "货物收发B区";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "生成发货单";
            ht_tiaojian["卖方编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            ht_tiaojian["提货单号"] = uctxtTHDBH.Text.Trim();
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
            //ht_where["search_str_where"] = " 1=1 and 商品名称 like '%" + txtSPMC.Text.Trim() + "%' and 提货单编号 like '%"+uctxtTHDBH.Text.Trim()+"%'";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }

        public string thddh = "";//提货单单号
        public string ZBDBXXBBH = "";//中标定标信息表编号
        public string FPYJTime = "";//发票邮寄信息录入时间
        public string FPSFSHTX = "";//发票是否随货同行
        public string FPHM = "";//发票号码
        public string SelCXFHTime = "";//卖家同意重新发货操作时间
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName=="生成发货单"&& e.RowIndex>-1)
            {
                thddh = dataGridView1.Rows[e.RowIndex].Cells["提货单编号"].Value.ToString();//提货单单号
                FPYJTime = dataGridView1.Rows[e.RowIndex].Cells["发票邮寄信息录入时间"].Value.ToString();//发票邮寄信息录入时间
                FPSFSHTX = dataGridView1.Rows[e.RowIndex].Cells["发票是否随货同行"].Value.ToString();//发票是否随货同行
                FPHM = dataGridView1.Rows[e.RowIndex].Cells["发票号码"].Value.ToString();//发票号码
                ZBDBXXBBH = dataGridView1.Rows[e.RowIndex].Cells["中标定标信息表编号"].Value.ToString();//中标定标信息表编号
                SelCXFHTime = dataGridView1.Rows[e.RowIndex].Cells["卖家同意重新发货操作时间"].Value.ToString();//卖家同意重新发货操作时间
                SCFHD fhd = new SCFHD(this);
                fhd.ShowDialog();
            }
            else if (columnName == "提货单编号" && e.RowIndex > -1)
            {
                string cs = dataGridView1.Rows[e.RowIndex].Cells["提货单编号"].Value.ToString();
                fmDY dy = new fmDY("查看并打印提货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.THDDY" });
                dy.ShowDialog();
            }

        }
        private void ucSCFHD_C_Load(object sender, EventArgs e)
        {
            GetData(null);
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
