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
using System.Threading;

namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class FBdldjSelectWarse : BasicForm
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
        Hashtable htInput;

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

        public FBdldjSelectWarse(Hashtable htInput, delegateForThread dftForParent_temp)
        {
    
            InitializeComponent();

            dftForParent = dftForParent_temp;
            this.htInput = htInput;

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            GetSatrtSPFL();
            switch (htInput["投标"].ToString())
            {
                case "年度":
                    cbTBLX.SelectedIndex = 0;
                    cbTBLX.Enabled = true;
                    cbTBLX.Items.RemoveAt(2);
                    break;
                default:
                    cbTBLX.SelectedIndex = 2;
                    cbTBLX.Enabled = false;
                    break;
            }
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
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = "SPBH as 商品编号,SPMC as 商品名称, GGXH as 规格型号,JJDW as 计价单位,ZXJJPL as 最小经济批量 "; //检索字段(必须设置)
            ht_where["search_tbname"] = "  ZZ_PTSPXXB  ";  //检索的表(必须设置)
            ht_where["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ht_where["search_str_where"] = " 1=1 and SFYX='是'";  //检索条件(必须设置)
            ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ht_where["search_paixuZD"] = " Number ";  //用于排序的字段(必须设置)
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


        private void FBdldjSelectWarse_Load(object sender, EventArgs e)
        {
            GetData(null);
        }

        //点击搜索按钮
        private void BBsearch_Click(object sender, EventArgs e)
        {
            //获取搜索条件
            string IP = ucTextBox6.Text.Trim();


            //更改搜索条件
            setDefaultSearch();
            //ht_where["search_str_where"] = " 1=1 and IP like '%" + IP + "%'";  //检索条件(必须设置)
            //执行查询
            GetData(ht_where);

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

        //选择一个数据带入父窗体(单元格任意部分)
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                //获取选中值
                int rowindex = e.RowIndex;
                string selectstr = dataGridView1.Rows[rowindex].Cells[2].Value.ToString();

                //回调
                Hashtable return_ht = new Hashtable();
                return_ht["测试值"] = selectstr;
                dftForParent(return_ht);

                //关闭弹窗
                this.Close();

            }
        }

        #region//获取一级商品分类

        private void GetSatrtSPFL()
        {
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_SPFL);
            DataControl.RunThreadClassStartSPFL RTCCL = new DataControl.RunThreadClassStartSPFL(tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_SPFL(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_SPFL_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_SPFL_Invoke(Hashtable OutPutHT)
        {
            string yhm = PublicDS.PublisDsUser.Tables[0].Rows[0]["YHM"].ToString();//用户名
            cbStartSPFL.Items.Clear();
            cbStartSPFL.Items.Add("请选择");
            cbStartSPFL.SelectedIndex = 0;
            DataSet ds = (DataSet)OutPutHT["商品分类"]; //获取商品分类
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                switch (ds.Tables[0].Rows[0]["状态"].ToString())
                {
                    case "系统繁忙":
                        ArrayList Almsg1 = new ArrayList();
                        Almsg1.Add("");
                        Almsg1.Add(yhm + "用户，当前系统繁忙，请稍后重试！");
                        FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                        FRSE1.ShowDialog();
                        return;
                    default:
                        DataTable tb = ds.Tables[0];
                        if (tb.Rows.Count > 0)
                        {
                            foreach (DataRow RowStartSPFL in tb.Rows)
                            {
                                cbStartSPFL.Items.Add(RowStartSPFL["SortName"].ToString());
                            }
                        }
                        break;
                }

            }
            else
            {
                ArrayList Almsg1 = new ArrayList();
                Almsg1.Add("");
                Almsg1.Add(yhm + "用户，当前系统繁忙，请稍后重试！");
                FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                FRSE1.ShowDialog();
                return;
            }
        }

        #endregion

        #region//获取二级商品分类
        private void cbStartSPFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbSecondSPFL.Items.Clear();
            switch (cbStartSPFL.SelectedItem.ToString().Trim())
            {
                case "请选择":
                    cbSecondSPFL.Items.Add("请选择");
                    cbSecondSPFL.SelectedIndex = 0;
                    break;
                default:
                    Hashtable HT = new Hashtable();
                    HT["商品名称"] = cbStartSPFL.SelectedItem.ToString();
                    delegateForThread tempDFT = new delegateForThread(ShowThreadResult_SecondChildSPFL);
                    DataControl.RunThreadClassChildSPFL RTCCL = new DataControl.RunThreadClassChildSPFL(HT, tempDFT);
                    Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
                    trd.IsBackground = true;
                    trd.Start();
                    break;
            }

        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_SecondChildSPFL(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_SecondChildSPFL_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_SecondChildSPFL_Invoke(Hashtable OutPutHT)
        {
            string yhm = PublicDS.PublisDsUser.Tables[0].Rows[0]["YHM"].ToString();//用户名
            cbSecondSPFL.Items.Add("请选择");
            cbSecondSPFL.SelectedIndex = 0;
            DataSet ds = (DataSet)OutPutHT["子类商品分类"]; //获取商品分类
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                switch (ds.Tables[0].Rows[0]["状态"].ToString())
                {
                    case "系统繁忙":
                        ArrayList Almsg1 = new ArrayList();
                        Almsg1.Add("");
                        Almsg1.Add(yhm + "用户，当前系统繁忙，请稍后重试22！");
                        FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                        FRSE1.ShowDialog();
                        return;
                    default:
                        DataTable tb = ds.Tables[0];
                        if (tb.Rows.Count > 0)
                        {
                            foreach (DataRow ChileSPFL in tb.Rows)
                            {
                                cbSecondSPFL.Items.Add(ChileSPFL["SortName"].ToString());
                            }
                        }
                        break;
                }

            }
        }
        #endregion

        #region//获取三级商品分类
        private void cbSecondSPFL_SelectedIndexChanged(object sender, EventArgs e)
        {

            cbThreeSPFL.Items.Clear();
            switch (cbStartSPFL.SelectedItem.ToString().Trim())
            {
                case "请选择":
                    cbThreeSPFL.Items.Add("请选择");
                    cbThreeSPFL.SelectedIndex = 0;
                    break;
                default:
                    Hashtable HT = new Hashtable();
                    HT["商品名称"] = cbSecondSPFL.SelectedItem.ToString();
                    delegateForThread tempDFT = new delegateForThread(ShowThreadResult_ThreeChildSPFL);
                    DataControl.RunThreadClassChildSPFL RTCCL = new DataControl.RunThreadClassChildSPFL(HT, tempDFT);
                    Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
                    trd.IsBackground = true;
                    trd.Start();
                    break;
            }
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_ThreeChildSPFL(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_ThreeChildSPFL_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_ThreeChildSPFL_Invoke(Hashtable OutPutHT)
        {
            string yhm = PublicDS.PublisDsUser.Tables[0].Rows[0]["YHM"].ToString();//用户名
            cbThreeSPFL.Items.Add("请选择");
            cbThreeSPFL.SelectedIndex = 0;
            DataSet ds = (DataSet)OutPutHT["子类商品分类"]; //获取商品分类
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                switch (ds.Tables[0].Rows[0]["状态"].ToString())
                {
                    case "系统繁忙":
                        ArrayList Almsg1 = new ArrayList();
                        Almsg1.Add("");
                        Almsg1.Add(yhm + "用户，当前系统繁忙，请稍后重试22！");
                        FormAlertMessage FRSE1 = new FormAlertMessage("仅确定", "叹号", "提示", Almsg1);
                        FRSE1.ShowDialog();
                        return;
                    default:
                        DataTable tb = ds.Tables[0];
                        if (tb.Rows.Count > 0)
                        {
                            foreach (DataRow ChileSPFL in tb.Rows)
                            {
                                cbThreeSPFL.Items.Add(ChileSPFL["SortName"].ToString());
                            }
                        }
                        break;
                }

            }
        }
        #endregion

    }
}
