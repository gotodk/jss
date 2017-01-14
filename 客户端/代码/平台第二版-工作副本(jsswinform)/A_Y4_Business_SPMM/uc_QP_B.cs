using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using 客户端主程序.DataControl;
using 客户端主程序.SubForm.NewCenterForm.MuBan;
using System.Threading;
using 客户端主程序.Support;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    
    public partial class uc_QP_B : UserControl
    {
             /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();

        string dlyx = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();

        protected static DataTable dt;
        public uc_QP_B()
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
            //ht_where["search_tbname"] = "  (SELECT  AAA_ZBDBXXB.Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,AAA_ZBDBXXB.Z_HTQX 合同期限,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额,  CAST( SUM( T_DJHKJE/ZBDJ) AS numeric(18,2)) AS 争议数量 ,SUM(T_DJHKJE) AS  争议金额,'人工清盘' AS 清盘类型,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '合同期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同完成' ELSE '其他' END AS 清盘原因 ,T_YSTBDDLYX AS 卖方邮箱,Y_YSYDDDLYX AS 买方邮箱,Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认 ,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据  FROM AAA_THDYFHDXXB JOIN AAA_ZBDBXXB ON AAA_ZBDBXXB.Number=AAA_THDYFHDXXB.ZBDBXXBBH WHERE (RTRIM( LTRIM( Q_SFYQR)) !='是' or  Q_SFYQR is null) and (T_YSTBDDLYX='" + dlyx + "' OR Y_YSYDDDLYX='" + dlyx + "') AND Z_QPZT='清盘中' AND F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','撤销','卖家主动退货') GROUP BY  Z_HTBH,Z_HTJSRQ,Z_SPMC,Z_SPBH,Z_GG,AAA_ZBDBXXB.Z_HTQX,Z_ZBSL,Z_ZBJG,Z_ZBJE,AAA_ZBDBXXB.Z_HTZT,AAA_ZBDBXXB.Number,Z_LYBZJJE,Z_QPZT,T_YSTBDDLYX ,Y_YSYDDDLYX ,Z_QPKSSJ ,Z_QPJSSJ ,Q_ZMSCFDLYX,Q_SFYQR ,Q_ZMWJLJ, Q_ZFLYZH , Q_ZFMBZH ,Q_ZFJE ,Q_CLYJ  ) as tab  ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " 主键 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " ASC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 清盘开始时间 ";  //用于排序的字段(必须设置)

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖B区清盘";
            Hashtable ht_tiaojian = new Hashtable();            
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
                dt = ds.Tables[0];
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
        public  void GetData(Hashtable HT_Where_temp)
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
            //ht_where["search_str_where"] = " 1=1 AND 商品名称 like '%"+txtSPMC.Text.Trim()+"%' ";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);
        }
      
     
        private void uc_QP_B_Load(object sender, EventArgs e)
        {
            GetData(null);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击列表中某列中的控件时处理
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "详情及操作" && e.RowIndex > -1)
            {
                //特殊处理，让被禁用的行不响应操作
                if (dataGridView1.Rows[e.RowIndex].Cells["详情及操作"].Tag != null && dataGridView1.Rows[e.RowIndex].Cells["详情及操作"].Tag.ToString() == "不让操作哦")
                {
                    return;
                }
                
                //执行点击后应该执行的事件
                //MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells["详情及操作"].Value.ToString());
                //重新读取需要的数据
                Hashtable htab = new Hashtable();                
                htab["dlyx"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                htab["num"] = dataGridView1.Rows[e.RowIndex].Cells["主键"].Value;
                SRT_demo_Run(htab);

                ////if (dt != null && dt.Rows.Count > 0)
                ////{
                ////    DataRow[] drs = dt.Select("主键='" + dataGridView1.Rows[e.RowIndex].Cells["主键"].Value + "'");
                ////    FormQPDetail fmqp = new FormQPDetail(drs[0]);

                ////    fmqp.ShowUpdate += new DisplayUpdate(GetData);
                ////    fmqp.ShowDialog();
 
                ////}

            }
            if (columnName == "电子购货合同编号" && e.RowIndex > -1)
            {
                int pagenumber = 8;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber +2];
                string[] MKarr = new string[pagenumber + 2];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + dataGridView1.Rows[e.RowIndex].Cells["主键"].Value;
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT";

                }
                Hashtable htcs1 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["主键"].Value;
                CSarr[pagenumber] = htcs1;
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_1";

                Hashtable htcs2 = new Hashtable();
                htcs1["要传递的参数1"] = dataGridView1.Rows[e.RowIndex].Cells["主键"].Value;
                CSarr[pagenumber+1] = htcs1;
                MKarr[pagenumber + 1] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTDZGHHT_2";
                FormDZGHHT dy = new FormDZGHHT("电子购货合同", CSarr, MKarr, dataGridView1.Rows[e.RowIndex].Cells["主键"].Value.ToString());
                dy.Show();
            }
        }


        //开启一个线程重新获取清盘数据，防止数据更新导致的两个客户端的数据不同步问题
        private void SRT_demo_Run(Hashtable InPutHT)
        {
            //OpenThreadDataUpdate OTD = new OpenThreadDataUpdate(InPutHT, new delegateForThread(SRT_demo), "GetQBInfo");
            //已移植可替换(已替换)
            OpenThreadQP OTD = new OpenThreadQP(InPutHT, new delegateForThread(SRT_demo));

            Thread trd = new Thread(new ThreadStart(OTD.BeginRun));

            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_demo_Invoke(Hashtable OutPutHT)
        {
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            if (dsreturn != null && dsreturn.Tables[0].Rows.Count > 0)
            {


                if (dsreturn.Tables.Contains("返回值单条") && dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "err")
                {

                    string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    return;
                }
                    DataRow[] drs = dsreturn.Tables[0].Select();
                    FormQPDetail fmqp = new FormQPDetail(drs[0]);

                    fmqp.ShowUpdate += new DisplayUpdate(GetData);
                    fmqp.ShowDialog();
   

            }
            else
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("无法获取数据，请重新登录！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();

            }
          
        }

    }
}
