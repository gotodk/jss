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
    public partial class ucDB_B : UserControl
    {

            /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        Hashtable ht = new Hashtable();
        public ucDB_B()
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
            //ht_where["serach_Row_str"] = "*,case 合同期限 when '即时' then datediff(day,getdate(),(dateadd(day,( select JSHTDBXTHDQX from AAA_PTDTCSSDB ),中标时间))) else (( select MJLYBZJDJQX from AAA_PTDTCSSDB ) - datediff(day,中标时间,getdate()) ) end as 距定标最后时间天数";         
            //ht_where["search_tbname"] = "((select  T_YSTBDBH as 投标单编号 ,Z_SPBH as 商品编号, Z_SPMC as 商品名称,Z_GG as 规格, T_YSTBDDJTBBZJ as 投标保证金冻结金额,T_YSTBDTBNSL as 投标拟售量,Sum(  isnull(Z_ZBSL,0.00)) as 中标数量,Z_ZBJG as 中标价格,Sum ( isnull(Z_ZBJE,0.00)) as 中标金额,Z_ZBSJ as 中标时间, sum (isnull (Z_LYBZJJE,0.00)) as 应冻结履约保证金金额 ,Z_HTQX as 合同期限,'' as Number,'供货区域'=(select GHQY from AAA_TBD where AAA_TBD.Number=T_YSTBDBH)  from AAA_ZBDBXXB where  Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' and T_YSTBDMJJSBH ='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "' and T_YSTBDDLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "'  and Z_HTQX <>'即时'   group by T_YSTBDBH,Z_SPBH,Z_SPMC,Z_GG,Z_ZBSJ,T_YSTBDTBNSL,T_YSTBDTBNSL,Z_ZBJG,Z_ZBSJ,T_YSTBDDJTBBZJ,Z_HTQX ) union (select  T_YSTBDBH as 投标单编号 ,Z_SPBH as 商品编号, Z_SPMC as 商品名称,Z_GG as 规格, T_YSTBDDJTBBZJ as 投标保证金冻结金额,T_YSTBDTBNSL as 投标拟售量,isnull(Z_ZBSL,0.00) as 中标数量,Z_ZBJG as 中标价格, isnull(Z_ZBJE,0.00) as 中标金额,Z_ZBSJ as 中标时间, isnull (Z_LYBZJJE,0.00) as 应冻结履约保证金金额 ,Z_HTQX as 合同期限,  Number,'供货区域'=(select GHQY from AAA_TBD where AAA_TBD.Number=T_YSTBDBH)  from AAA_ZBDBXXB where  Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' and T_YSTBDMJJSBH ='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "' and T_YSTBDDLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "'  and Z_HTQX ='即时' ) ) as tab";
            //ht_where["search_mainid"] = " 投标单编号 ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = "1=1 ";  //检索条件(必须设置)当前交易账户下所有中标的，且需冻结履约保证金的记录，中标记录中带有本账户卖家角色编号的数据
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " 中标时间 ";  //用于排序的字段(必须设置)
            //if (this.txtSPMC.Text.Trim() != "")
            //{
            //    //过滤特殊字符
            //    ht_where["search_str_where"] += " and tab.商品名称 like '%" + txtSPMC.Text.Trim() + "%' ";  //检索条件(必须设置)
            //}

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "商品买卖B区定标";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["卖方编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
            ht_where["tiaojian"] = ht_tiaojian;
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void STR_BeginBind(Hashtable returnHT)
        {
            try { 
                Invoke(new delegateForThreadShow(STR_BeginBind_Invoke), new Hashtable[] { returnHT }); 
                }
            catch (Exception ex) 
            { 
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
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
                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        DateTime dt = Convert.ToDateTime(dr["中标时间"].ToString().Trim()).AddDays(5);
                //        TimeSpan ts = dt - DateTime.Now;
                //        dr["距定标最后时间天数"] = ts.Days;
                //    }
                //}
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
            this.Enabled = true;

           



          
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

      
       
        private void ucDB_B_Load(object sender, EventArgs e)
        {
          
            GetData();
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            //更改搜索条件
           // setDefaultSearch();
            this.Enabled = false;
            if (this.txtSPMC.Text.Trim() != "")
            {
                Hashtable ht = new Hashtable();
                ht["aaaaaaa"] = this.txtSPMC.Text.Trim();
                if (StringOP.ValidateQuery(ht))
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("你输入的内容存在不合法字符！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", Almsg3);
                    FRSE3.ShowDialog();
                    return;
                }
            }
           GetData(); 
           //过滤特殊字符
            
            //执行查询

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if (dataGridView1.SelectedCells.Count > 0)
                //{
                    string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
                  
                  
                    if (columnName == "定标" && e.RowIndex > -1)
                    {
                        this.Enabled = false;
                        //获取释放投标保证金

                        ht["登陆邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                        ht["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                        ht["卖家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
                        ht["投标单号"] = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString(); //.SelectedCells[1].Value.ToString(); 
                        ht["Number"] = dataGridView1.Rows[e.RowIndex].Cells["Number"].Value.ToString();//dataGridView1.SelectedCells[12].Value.ToString();
                        ht["合同期限"] = dataGridView1.Rows[e.RowIndex].Cells["合同期限"].Value.ToString();//dataGridView1.SelectedCells[13].Value.ToString();
                        ht["ispass"] = "no";

                        //ht["登陆邮箱"] = dsuser.Tables[0].Rows[0]["是否冻结"].ToString().Trim();
                         
                        SRT_demo_Run(ht);
 
                    }
                //}
            }
            catch (Exception ex)
            {
 
            }


        }

        private void SRT_demo_Run(Hashtable InPutHT)
        {
            RunThreadClassdbiao OTD = new RunThreadClassdbiao(InPutHT, new delegateForThread(SRT_demo));
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

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    this.Enabled = true;
                    GetData();
                    break;
                case "pass":
                     ArrayList Almsg5 = new ArrayList();
                     Almsg5.Add("");
                     Almsg5.Add(showstr);
                     
                     FormAlertMessage FRSE5 = new FormAlertMessage("确定取消", "问号", "", Almsg5);
                    DialogResult dialogResult = FRSE5.ShowDialog();
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.Enabled = false;
                        ht["ispass"] = "ok";
                        SRT_demo_Run(ht);
                    }
                    else
                    {
                        this.Enabled = true;
                    }

                    //GetData();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    this.Enabled = true;
                    break;
            }



        }

        //导出功能
        private void btn_export_Click(object sender, EventArgs e)
        {
            ////设置要隐藏的列(原分页中，有些列可能是隐藏不显示的，导出时，可以选择滤掉)
            //string[] HideColumns = new string[] { "" };
            ////设置导出语句
            //string sql = " select *,(case 合同期限 when '即时' then datediff(day,getdate(),(dateadd(day,( select JSHTDBXTHDQX from AAA_PTDTCSSDB ),中标时间))) else (( select MJLYBZJDJQX from AAA_PTDTCSSDB ) - datediff(day,中标时间,getdate()) ) end ) as 距定标最后时间天数,'供货区域'=(select GHQY from AAA_TBD where Number=tab.投标单编号) from  (select  T_YSTBDBH as 投标单编号 ,Z_SPBH as 商品编号, Z_SPMC as 商品名称,Z_GG as 规格, T_YSTBDDJTBBZJ as 投标保证金冻结金额,T_YSTBDTBNSL as 投标拟售量,Sum(  isnull(Z_ZBSL,0.00)) as 中标数量,Z_ZBJG as 中标价格,Sum ( isnull(Z_ZBJE,0.00)) as 中标金额,Z_ZBSJ as 中标时间, sum (isnull (Z_LYBZJJE,0.00)) as 应冻结履约保证金金额,Z_HTQX as 合同期限  from AAA_ZBDBXXB where " + " Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' and T_YSTBDMJJSBH ='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString() + "' and T_YSTBDDLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' " + "    group by T_YSTBDBH,Z_SPBH,Z_SPMC,Z_GG,Z_ZBSJ,T_YSTBDTBNSL,T_YSTBDTBNSL,Z_ZBJG,Z_ZBSJ,T_YSTBDDJTBBZJ,Z_HTQX) as tab  where " + ht_where["search_str_where"].ToString();
            //cMyXls1.BeginRunFrom_ht_where(sql, HideColumns);

            Hashtable ht_export = new Hashtable();
            ht_export["filename"] = "定标";
            ht_export["webmethod"] = "商品买卖B区定标导出";
            ht_export["tiaojian"] = (Hashtable)ht_where["tiaojian"];           
            cMyXls1.BeginRunFrom_ht_where(ht_export);
        }

    }
}
