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
using 客户端主程序.NewDataControl;
namespace 客户端主程序.SubForm
{
    public partial class fmNMRSP : BasicForm
    {
        Support.ManageDgv Dgv;

        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 查询条件
        /// </summary>
        Hashtable ht_where = new Hashtable();
        SPXZTS rot;
        /// <summary>
        /// 是否为从买入窗体实例化来。
        /// </summary>
        bool isFromBuyAuto = true;

        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;

        /// <summary>
        /// 主要的SQL
        /// </summary>
        string SqlMainSelect = string.Empty;

        protected static DataTable dt;
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
        bool bbb = false;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dftForParent_temp"></param>
        public fmNMRSP(delegateForThread dftForParent_temp)
        {

            InitializeComponent();
            chkHTQX.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            chkHTQX.SelectedIndex = 0;
            dftForParent = dftForParent_temp;
            bbb = true;
            //初始化分页回调(带分页的页面，先把这个放上)
            //ucPager1.IsOpen = true;
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;

        }

        bool ss = true;
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

            if (jls == 0 && ss && basicRadioButton1.Checked)
            {
                ss = false;
                basicRadioButton1.Checked = false;
                basicRadioButton2.Checked = true;
                GetData(null);
            }
            else
            {
                //string Numbers = string.Empty;
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        Numbers += "'" + ds.Tables[0].Rows[i][0].ToString() + "',";
                //    }
                //    Numbers = Numbers.Substring(0, Numbers.Length - 1);
                //    //Numbers+="'"+
                //}

                //string sqls = SqlMainSelect.Replace("XXXXXXXXX", Numbers);//SqlMainSelect + " AND AAA_PTSPXXB.Number IN ( " + Numbers + ") ";
                //Hashtable htsql = new Hashtable();
                //htsql["SQL"] = sqls;
                //delegateForThread tempDFT = new delegateForThread(queryMain);
                //G_Reg g = new G_Reg(htsql, tempDFT);
                //Thread trd = new Thread(new ThreadStart(g.GetQuery));
                //trd.IsBackground = true;
                //trd.Start();
                ////ucPager1.SetPicVisible(true);
             
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
            }
            //ucPager1.SetPicVisible(true);
            //取消列表的自动排序
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //private void queryMain(Hashtable OutPutHT)
        //{
        //    try
        //    {
        //        Invoke(new delegateForThreadShow(querys), new Hashtable[] { OutPutHT });
        //    }
        //    catch (Exception ex)
        //    {
        //        Support.StringOP.WriteLog("委托回调错误：" + ex.Message);
        //    }
        //}
        //private void querys(Hashtable OutPutHT)
        //{
        //    //ucPager1.SetPicVisible(false);
        //    DataSet ds = (DataSet)OutPutHT["执行结果"];
        //    this.dataGridView1.AutoGenerateColumns = false;
        //    //若执行正常
        //    if (ds!=null)
        //    {
        //        dataGridView1.DataSource = ds.Tables[0].DefaultView;
        //    }
        //    else
        //    {
        //        dataGridView1.DataSource = null;
        //    }
        //    //ucPager1.SetPicVisible(false);
        //}

        /// <summary>
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";  //每页显示的条数(必须设置)
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = " id,module,OperateTime,IP as yyyy,'测试'= '测试2','你好' as 你好啊,'测试一个长列名'='测试一个很长的内容测试一个很长的内容测试一个很长的内容' "; //检索字段(必须设置)
            //ht_where["search_tbname"] = "  FMEventLog  ";  //检索的表(必须设置)
            //ht_where["search_mainid"] = " id ";  //所检索表的主键(必须设置)
            //ht_where["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            //ht_where["search_paixu"] = " DESC ";  //排序方式(必须设置)
            //ht_where["search_paixuZD"] = " id ";  //用于排序的字段(必须设置)
            #region tableName
            //string tableName = " (SELECT AAA_PTSPXXB.Number,AAA_PTSPXXB.SPBH,AAA_PTSPXXB.SPMC,(CAST(RIGHT(AAA_PTSPXXB.SPBH,4) AS int)) AS 右边编号, CAST(REPLACE(LEFT(AAA_PTSPXXB.SPBH,2),'L','') AS int) AS 左边编号 FROM AAA_ZXSPJLB INNER JOIN AAA_PTSPXXB ON AAA_PTSPXXB.SPBH=AAA_ZXSPJLB.SPBH WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND SFYX='是' ) AS TABLE1 ";//自选商品
            //string tableName_Other = " (SELECT AAA_PTSPXXB.Number, AAA_PTSPXXB.SPBH,AAA_PTSPXXB.SPMC,(CAST(RIGHT(SPBH,4) AS int)) AS 右边编号, CAST(REPLACE(LEFT(SPBH,2),'L','') AS int) AS 左边编号 FROM AAA_PTSPXXB  WHERE SFYX='是' ) AS TABLE1 ";
            //#endregion

            //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
            //ht_where["page_index"] = "0";
            //ht_where["page_size"] = "10";  //每页显示的条数(必须设置)
            //ht_where["serach_Row_str"] = "*";


            //#region ht_where["search_tbname"]
            //ht_where["search_tbname"] = tableName;
            //SqlMainSelect = "select  Number,SPBH, SPMC, GG, JJDW, JJPL,SCZZYQ, '' AS HTQX,ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='即时' ORDER BY TBJG ASC),0) AS ZDJGJS,ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='三个月' ORDER BY TBJG ASC),0) AS ZDJGSGY,ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='一年' ORDER BY TBJG ASC),0) AS ZDJGYN, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='即时' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPLJS,ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='三个月' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPLSGY, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='一年' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPLYN,ZLBZYZM,CPJCBG,FDDBRCNS,SHFWGDYCN,CPSJSQS,PGZFZRFLCNS,SLZM from (select row_number() over(partition by a.spbh order by b.createtime desc) as 序号,a.*,ZLBZYZM,CPJCBG,FDDBRCNS,SHFWGDYCN,CPSJSQS,PGZFZRFLCNS,SLZM,b.createtime from (select AAA_PTSPXXB.Number,AAA_PTSPXXB.SPBH, SPMC, GG,JJDW,JJPL,SCZZYQ from AAA_ZXSPJLB JOIN   AAA_PTSPXXB ON AAA_ZXSPJLB.SPBH=AAA_PTSPXXB.spbh where AAA_PTSPXXB.number in (XXXXXXXXX) and AAA_ZXSPJLB.dlyx='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ) as a left join  (select spbh,ZLBZYZM,CPJCBG,FDDBRCNS,SHFWGDYCN,CPSJSQS,SLZM,createtime,PGZFZRFLCNS from AAA_TBD where dlyx='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ) as b on b.spbh=a.spbh ) as tab where 序号=1";
            ////SqlMainSelect = " SELECT AAA_PTSPXXB.Number, AAA_ZXSPJLB.DLYX, AAA_PTSPXXB.SPBH, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, '" + this.chkHTQX.SelectedItem.ToString() + "' AS HTQX, ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND HTQX='" + this.chkHTQX.SelectedItem.ToString() + "' ORDER BY TBJG ASC),0) AS ZDJG, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND HTQX='" + this.chkHTQX.SelectedItem.ToString() + "' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPL,AAA_PTSPXXB.SCZZYQ,(CAST(RIGHT(AAA_PTSPXXB.SPBH,4) AS int)) AS 右边编号, CAST(REPLACE(LEFT(AAA_PTSPXXB.SPBH,2),'L','') AS int) AS 左边编号 , (SELECT TOP 1 ZLBZYZM FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS ZLBZYZM ,(SELECT TOP 1 CPJCBG FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS CPJCBG , (SELECT TOP 1 PGZFZRFLCNS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS PGZFZRFLCNS , (SELECT TOP 1 FDDBRCNS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS FDDBRCNS , (SELECT TOP 1 SHFWGDYCN FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS SHFWGDYCN , (SELECT TOP 1 CPSJSQS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS CPSJSQS, 0 as QTZZ, (SELECT TOP 1 SLZM FROM AAA_TBD WHERE AAA_TBD.SPBH=SPBH AND AAA_TBD.SPMC=SPMC AND AAA_TBD.GG=GG AND AAA_TBD.DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ORDER BY CREATETIME DESC) AS SLZM FROM AAA_ZXSPJLB INNER JOIN AAA_PTSPXXB ON AAA_PTSPXXB.SPBH=AAA_ZXSPJLB.SPBH WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND SFYX='是'  ";//自选商品
            //if (basicRadioButton2.Checked)
            //{
            //    //SqlMainSelect = " SELECT AAA_PTSPXXB.Number, AAA_PTSPXXB.SPBH, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, '" + this.chkHTQX.SelectedItem.ToString() + "' AS HTQX, ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND HTQX='" + this.chkHTQX.SelectedItem.ToString() + "' ORDER BY TBJG ASC),0) AS ZDJG, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND HTQX='" + this.chkHTQX.SelectedItem.ToString() + "' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPL,AAA_PTSPXXB.SCZZYQ,(CAST(RIGHT(SPBH,4) AS int)) AS 右边编号, CAST(REPLACE(LEFT(SPBH,2),'L','') AS int) AS 左边编号,(SELECT TOP 1 ZLBZYZM FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS ZLBZYZM , (SELECT TOP 1 CPJCBG FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS CPJCBG , (SELECT TOP 1 PGZFZRFLCNS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS PGZFZRFLCNS , (SELECT TOP 1 FDDBRCNS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS FDDBRCNS , (SELECT TOP 1 SHFWGDYCN FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS SHFWGDYCN , (SELECT TOP 1 CPSJSQS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS CPSJSQS, 0 as QTZZ, (SELECT TOP 1 SLZM FROM AAA_TBD WHERE AAA_TBD.SPBH=SPBH AND AAA_TBD.SPMC=SPMC AND AAA_TBD.GG=GG AND AAA_TBD.DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ORDER BY CREATETIME DESC) AS SLZM FROM AAA_PTSPXXB  WHERE SFYX='是' ";

            //    SqlMainSelect = "select  Number,SPBH, SPMC, GG, JJDW, JJPL,SCZZYQ, '' AS HTQX,ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='即时' ORDER BY TBJG ASC),0) AS ZDJGJS,ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='三个月' ORDER BY TBJG ASC),0) AS ZDJGSGY,ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='一年' ORDER BY TBJG ASC),0) AS ZDJGYN, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='即时' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPLJS,ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='三个月' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPLSGY,ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=tab.SPBH AND ZT='竞标' AND HTQX='一年' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPLYN, ZLBZYZM,CPJCBG,FDDBRCNS,SHFWGDYCN,CPSJSQS,PGZFZRFLCNS,SLZM from (select row_number() over(partition by a.spbh order by b.createtime desc) as 序号,a.*,ZLBZYZM,CPJCBG,FDDBRCNS,SHFWGDYCN,CPSJSQS,PGZFZRFLCNS,SLZM,b.createtime from (select Number,SPBH, SPMC, GG,JJDW,JJPL,SCZZYQ from  AAA_PTSPXXB where number in (XXXXXXXXX)) as a left join  (select spbh,ZLBZYZM,CPJCBG,FDDBRCNS,SHFWGDYCN,CPSJSQS,PGZFZRFLCNS,SLZM,createtime from AAA_TBD where dlyx='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' ) as b on b.spbh=a.spbh ) as tab where 序号=1";


            //    ht_where["search_tbname"] = tableName_Other;
            //}
            #endregion

            //ht_where["search_mainid"] = " Number ";
            //ht_where["search_str_where"] = " 1=1 ";
            //if (txtSPMC.Text.Trim() != "")
            //{
            //    ht_where["search_str_where"] = " 1=1 AND SPMC LIKE '%" + this.txtSPMC.Text.Trim() + "%' ";
            //}
            //ht_where["search_paixu"] = " ASC ";
            //ht_where["search_paixuZD"] = " 左边编号 ASC,右边编号 ";


            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["webmethod"] = "B区选择商品";
            Hashtable ht_tiaojian = new Hashtable();
            ht_tiaojian["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            ht_tiaojian["商品类型"] = basicRadioButton2.Checked == true ? "所有商品" : "自选商品";
            ht_tiaojian["商品名称"] = txtSPMC.Text.Trim();
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

            this.CS1.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\myxz_jishi.png");
            this.CS1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Normal;

            this.CS2.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\myxz_sangeyue.png");
            this.CS2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Normal;

            this.CS3.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\myxz_yinian.png");
            this.CS3.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Normal;

            Dgv = new Support.ManageDgv(this.dataGridView1);
            Dgv.AddMergeColumns(0, 3, "合同期限", -1);

            GetData(null);
        }

        //点击搜索按钮
        private void BBsearch_Click(object sender, EventArgs e)
        {
            GetData(null);
        }

        //搞出鼠标小手
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            if (e.RowIndex >=0 && (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2))
            {

                string Strtext = "";

                if (e.ColumnIndex == 0)
                {
                    Strtext = "选择“即时交易”的，买方须在定标后3日内一次性下达完《提货单》，\n卖方须于收到《提货单》后24小时内一次性发货。";
                }
                if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                {
                    Strtext = "选择“三个月”与“一年”的，买方可在《电子购货合同》到期前5天内随时提货；\n卖方收到《提货单》后须于系统确定的最迟发货日前发货。";
                }
                
                dataGridView1.Cursor = Cursors.Hand;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.Red;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = Strtext;
                //鼠标滑过显示文字

                //2013.12.11 wyh add 鼠标略过显示文字
                //toolTip1.AutoPopDelay = 30000;
                //toolTip1.InitialDelay = 500;
                //toolTip1.ReshowDelay = 0;
                //toolTip1.ShowAlways = true;
                //string Strtext = "1、一个商品一个轮次的竞标中只有一个最低价标的中标，价格相同时投标时间早的优先，中标买方收货区域将在该标的供货区域范围内。\n2、卖方设定的“经济批量”不得高于“平台设定的最大经济批量”；合同期限为“三个月”或“一年”的，“投标拟售量”不得低于“平台设定的最\n 大经济批量”的10倍；平台设定的最大经济批量”的10倍；合同期限为“即时”的，“投标拟售量”不得低于“平台设定的最大经济批量”。\n3、投标单下达的同时系统将自动冻结投标金额的0.5‰（最低不少于1000元/次）作为投标保证金；该投标保证金将在投标单撤销或定标后\n予以解冻；如未履行定标程序，\n后予以解冻；如未履行定标程序，投标保证金将被扣罚，补偿给各中标买方；合同期限为“即时“的投标单，\n可能会一部分中标（不低于卖方设定的一个经济批量），剩余部分将自动转入下一次交易撮合\n低于卖方设定的一个经济批量），剩余部分将\n自动转入下一次交易撮合（若剩余部分数量低于卖方设定的一个经济批量，则自动撤销）。\n4、按有关协议和交易规定，投标单一旦中标，即\n为交易双方正式订立《电子购货合同》。\n5、为保证快速履约，合同期限为“即时”的投标单中标后，卖方须在72小时内自主冻结履约保证金予以定标。\n6、卖方在履行交货时，须按照买方要求开具增值税专用发票或增值税普通发票（非一般纳税人单位须向税务机关申请代开）。\n  ";
                //toolTip1.SetToolTip(this.dataGridView1, Strtext);
              //  toolTip1.Show(Strtext, this.dataGridView1);
               // toolTip1.SetToolTip(this.pictureBox1, Strtext);
                // this.toolTip1.Show(Strtext, this.label33);
               // this.toolTip1.ToolTipTitle = "卖出须知";

            }
            else
            {
                dataGridView1.Cursor = Cursors.Default;
            }
        }
        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >=0 && (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2))
            {

                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.LightGray;
            }
        }

        
        
        //搜索按钮
        private void basicButton2_Click(object sender, EventArgs e)
        {
            GetData(null);
        }

        private void basicRadioButton2_CheckedChanged(object sender, bool Checked)
        {
            GetData(null);
        }

        private void chkHTQX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(bbb)
                GetData(null);
        }

        //选择一个数据带入父窗体(单元格任意部分)这个是点击对号的时候
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2) && e.RowIndex > -1)
            {
                int rowindex = e.RowIndex;
                string strQX = string.Empty;
                object ZDJG = 0;
                object MJJJPL = 0;
                switch (e.ColumnIndex)
                {
                    case 0:
                        strQX = "即时";
                        ZDJG = dataGridView1.Rows[rowindex].Cells["ZDJGJS"].Value;
                        MJJJPL = dataGridView1.Rows[rowindex].Cells["MJJJPLJS"].Value;
                        break;
                    case 1:
                        strQX = "三个月";
                        ZDJG = dataGridView1.Rows[rowindex].Cells["ZDJGSGY"].Value;
                        MJJJPL = dataGridView1.Rows[rowindex].Cells["MJJJPLSGY"].Value;
                        break;
                    case 2:
                        strQX = "一年";
                        ZDJG = dataGridView1.Rows[rowindex].Cells["ZDJGYN"].Value;
                        MJJJPL = dataGridView1.Rows[rowindex].Cells["MJJJPLYN"].Value;
                        break;
                    default:
                        strQX = null;
                        ZDJG = 0;
                        MJJJPL = 0;
                        break;
                }


                if (strQX == null)
                    return;
                else
                {
                    dataGridView1.Rows[rowindex].Cells["合同期限"].Value = strQX;
                }
                //回调
                Hashtable return_ht = new Hashtable();
                return_ht["商品编号"] = dataGridView1.Rows[rowindex].Cells["商品编号"].Value.ToString();//商品编号
                return_ht["商品名称"] = dataGridView1.Rows[rowindex].Cells["商品名称"].Value.ToString();//商品编号
                return_ht["规格"] = dataGridView1.Rows[rowindex].Cells["规格"].Value.ToString();//商品编号
                return_ht["合同期限"] = dataGridView1.Rows[rowindex].Cells["合同期限"].Value.ToString();//商品编号
                return_ht["计价单位"] = dataGridView1.Rows[rowindex].Cells["计价单位"].Value.ToString();//商品编号
                return_ht["平台批量"] = dataGridView1.Rows[rowindex].Cells["Column2"].Value.ToString();//商品编号
                return_ht["最低价格"] = ZDJG;//商品编号
                return_ht["卖家批量"] = MJJJPL;//商品编号
                return_ht["上传资质"] = dataGridView1.Rows[rowindex].Cells["SCZZYQ"].Value.ToString();//该商品需要上传的资质
                return_ht["ZLBZYZM"] = dataGridView1.Rows[rowindex].Cells["ZLBZYZM"].Value;//下面是6个必要的资质，最后一次使用过的
                return_ht["CPJCBG"] = dataGridView1.Rows[rowindex].Cells["CPJCBG"].Value;
                return_ht["PGZFZRFLCNS"] = dataGridView1.Rows[rowindex].Cells["PGZFZRFLCNS"].Value;
                return_ht["FDDBRCNS"] = dataGridView1.Rows[rowindex].Cells["FDDBRCNS"].Value;
                return_ht["SHFWGDYCN"] = dataGridView1.Rows[rowindex].Cells["SHFWGDYCN"].Value;
                return_ht["CPSJSQS"] = dataGridView1.Rows[rowindex].Cells["CPSJSQS"].Value;
                //return_ht["QTZZ"] = dataGridView1.Rows[rowindex].Cells[16].Value;
                return_ht["SLZM"] = dataGridView1.Rows[rowindex].Cells["SLZM"].Value;
                dftForParent(return_ht);
                //关闭弹窗
                this.Close();
            }
            else
            {
                if (e.RowIndex > -1)
                {
                    rot = new SPXZTS(dataGridView1, dftForParent, this, e.RowIndex);
                    rot.Show();
                    this.Enabled = false;
                }
            }
        }
    }
}
