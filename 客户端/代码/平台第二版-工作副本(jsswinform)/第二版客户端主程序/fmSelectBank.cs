using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Runtime.InteropServices;
using 客户端主程序.DataControl;
namespace 客户端主程序.SubForm
{
    public partial class fmSelectBank : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        
        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;        
        #region 窗体淡出，窗体最小化最大化等特殊控制，所有窗体都有这个玩意

        /// <summary>
        /// 窗体的Load事件中的淡出处理
        /// </summary>
        private void Init_one_show()
        {
            //设置双缓冲
            this.SetStyle(ControlStyles.DoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
            true);
            this.UpdateStyles();

            //加载淡出计时器
            Timer_DC = new System.Windows.Forms.Timer();
            Timer_DC.Interval = Program.DC_Interval;
            this.Timer_DC.Tick += new System.EventHandler(this.Timer_DC_Tick);
            //淡出效果
            MaxDC();
        }

        /// <summary>
        /// 显示窗体时启动淡出
        /// </summary>
        private void MaxDC()
        {
            this.Opacity = 0;
            Timer_DC.Enabled = true;
        }

        //淡出显示窗体，绕过窗体闪烁问题
        private void Timer_DC_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity + Program.DC_step;
            if (!Program.DC_open)
            {
                this.Opacity = 1;
            }
            if (this.Opacity >= 1)
            {
                Timer_DC.Enabled = false;
            }
        }

        //允许任务栏最小化
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                return cp;
            }
        }


        private int WM_SYSCOMMAND = 0x112;
        private long SC_MAXIMIZE = 0xF030;
        private long SC_MINIMIZE = 0xF020;
        private long SC_CLOSE = 0xF060;
        private long SC_NORMAL = 0xF120;
        private FormWindowState SF = FormWindowState.Normal;
        /// <summary>
        /// 重绘窗体的状态
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref   Message m)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                SF = this.WindowState;
            }
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt64() == SC_MAXIMIZE)
                {
                    MaxDC();
                    this.WindowState = FormWindowState.Maximized;
                    return;
                }
                if (m.WParam.ToInt64() == SC_MINIMIZE)
                {
                    this.WindowState = FormWindowState.Minimized;
                    return;
                }
                if (m.WParam.ToInt64() == SC_NORMAL)
                {
                    MaxDC();
                    this.WindowState = SF;
                    return;
                }
                if (m.WParam.ToInt64() == SC_CLOSE)
                {
                    this.Close();
                    return;
                }
            }
            base.WndProc(ref   m);
        }

        #endregion
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dftForParent_temp"></param>
        public fmSelectBank(delegateForThread dftForParent_temp)
        {

            InitializeComponent();
            dftForParent = dftForParent_temp;            
            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;

            //仅使第一列列标题居中
            dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_BeginBind(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_BeginBind_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件(线程获取到分页数据后的具体绑定处理)
        private void ShowThreadResult_BeginBind_Invoke(Hashtable OutPutHT)
        {
            DataSet ds = (DataSet)OutPutHT["数据"];//获取返回的数据集
            int fys = (int)ds.Tables["附加数据"].Rows[0]["分页数"];
            int jls = (int)ds.Tables["附加数据"].Rows[0]["记录数"];

          

            string msg = (string)ds.Tables["附加数据"].Rows[0]["其他描述"];
            string err = (string)ds.Tables["附加数据"].Rows[0]["执行错误"];
            this.dataGridView1.AutoGenerateColumns = false;
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
            //for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            //{
            //    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "10";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " B_DLYX,I_JYFMC,J_JJRZGZSBH,I_JYFMC,I_LXRSJH,I_JJRFL, createtime "; //检索字段(必须设置)
            //ht_where["search_tbname"] = "  AAA_DLZHXXB  ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " number ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " B_JSZHLX='经纪人交易账户' and I_JJRFL='银行' and S_SFYBFGSSHTG='是' and B_SFYZYX='是' and B_SFYXDL='是' and B_SFDJ='否' and B_SFXM='否' and  J_JJRSFZTXYHSH='否' ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " asc ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " createtime ";  //用于排序的字段(必须设置)        

            //if (txtBankName.Text.Trim()!="")
            //{
            //    ht_where["search_str_where"] += " and I_JYFMC like '%" + txtBankName.Text.Trim() + "%'";
            //}

            ht_where["page_index"] = "0";
            ht_where["page_size"] = "10";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "开通交易账户弹窗分页";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["标签名"] = "选择银行";
            ht_tiaojian["银行名称"] = txtBankName.Text.Trim();
            ht_where["tiaojian"] = ht_tiaojian;
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


        private void FormSelectList_Load(object sender, EventArgs e)
        {
            GetData(null);
        }

        //选择一个数据带入父窗体(仅限单元格控件部分，非任意部分)
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //不处理

        }

        //搞出鼠标小手
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
            else
            {
                dataGridView1.Cursor = Cursors.Default;
            }
        }

        //选择一个数据带入父窗体(单元格任意部分)这个是点击对号的时候
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                //获取选中值
                int rowindex = e.RowIndex;

                //回调
                Hashtable return_ht = new Hashtable();
                return_ht["银行登录邮箱"] = dataGridView1.Rows[rowindex].Cells["银行登录邮箱"].Value.ToString();
                return_ht["银行名称"] = dataGridView1.Rows[rowindex].Cells["银行名称"].Value.ToString();

                return_ht["经纪人资格证书"] = dataGridView1.Rows[rowindex].Cells["经纪人资格证书"].Value.ToString();
                return_ht["交易方名称"] = dataGridView1.Rows[rowindex].Cells["交易方名称"].Value.ToString();
                return_ht["联系电话"] = dataGridView1.Rows[rowindex].Cells["联系电话"].Value.ToString();
                return_ht["经纪人分类"] = dataGridView1.Rows[rowindex].Cells["经纪人分类"].Value.ToString();
                dftForParent(return_ht);

                //关闭弹窗
                this.Close();
            }
        }
        //这个是双击行的时候
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                //获取选中值
                int rowindex = e.RowIndex;

                //回调
                Hashtable return_ht = new Hashtable();
                return_ht["银行登录邮箱"] = dataGridView1.Rows[rowindex].Cells["银行登录邮箱"].Value.ToString();
                return_ht["银行名称"] = dataGridView1.Rows[rowindex].Cells["银行名称"].Value.ToString();

                return_ht["经纪人资格证书"] = dataGridView1.Rows[rowindex].Cells["经纪人资格证书"].Value.ToString();
                return_ht["交易方名称"] = dataGridView1.Rows[rowindex].Cells["交易方名称"].Value.ToString();
                return_ht["联系电话"] = dataGridView1.Rows[rowindex].Cells["联系电话"].Value.ToString();
                return_ht["经纪人分类"] = dataGridView1.Rows[rowindex].Cells["经纪人分类"].Value.ToString();
                dftForParent(return_ht);

                //关闭弹窗
                this.Close();

            }
        }
        //搜索按钮
        private void basicButton2_Click(object sender, EventArgs e)
        {
            GetData(null);
        }

    }
}
