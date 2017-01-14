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

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.SPMM
{
    public partial class fmNMRSP_AddSP : BasicForm
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




        public fmNMRSP_AddSP()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="dftForParent_temp"></param>
        public fmNMRSP_AddSP(delegateForThread dftForParent_temp)
        {
    
            InitializeComponent();
            dftForParent = dftForParent_temp;

            //初始化分页回调(带分页的页面，先把这个放上)
            ucPager1.DFT = new delegateForThread(ShowThreadResult_BeginBind);

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = false;
            
        }
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
        bool ss = true;
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
        /// 设置默认搜索条件(这个例子中的哈希表键值一个也不能少)
        /// </summary>
        private void setDefaultSearch()
        {
            #region tableName
            string tableName = " (SELECT AAA_PTSPXXB.Number, AAA_ZXSPJLB.DLYX, AAA_PTSPXXB.SPBH, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, '" + "即时" + "' AS HTQX, ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND HTQX='" + "即时" + "' ORDER BY TBJG ASC),0) AS ZDJG, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND HTQX='" + "即时" + "' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPL,AAA_PTSPXXB.SCZZYQ,(CAST(RIGHT(AAA_PTSPXXB.SPBH,4) AS int)) AS 右边编号, CAST(REPLACE(LEFT(AAA_PTSPXXB.SPBH,2),'L','') AS int) AS 左边编号 , (SELECT TOP 1 ZLBZYZM FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS ZLBZYZM ,(SELECT TOP 1 CPJCBG FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS CPJCBG , (SELECT TOP 1 PGZFZRFLCNS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS PGZFZRFLCNS , (SELECT TOP 1 FDDBRCNS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS FDDBRCNS , (SELECT TOP 1 SHFWGDYCN FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS SHFWGDYCN , (SELECT TOP 1 CPSJSQS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS CPSJSQS  FROM AAA_ZXSPJLB INNER JOIN AAA_PTSPXXB ON AAA_PTSPXXB.SPBH=AAA_ZXSPJLB.SPBH WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND SFYX='是' AND AAA_PTSPXXB.SPBH NOT IN (SELECT SPBH FROM AAA_CSSPB WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND (SHZT='未审核' OR SHZT='审核通过') AND SFBGZZHBZ='否' GROUP BY SPBH) ) AS TABLE1 ";//自选商品
            string tableName_Other = " (SELECT AAA_PTSPXXB.Number, AAA_PTSPXXB.SPBH, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, '" + "即时" + "' AS HTQX, ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND HTQX='" + "即时" + "' ORDER BY TBJG ASC),0) AS ZDJG, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND HTQX='" + "即时" + "' ORDER BY MJSDJJPL DESC) ,0) AS MJJJPL,AAA_PTSPXXB.SCZZYQ,(CAST(RIGHT(SPBH,4) AS int)) AS 右边编号, CAST(REPLACE(LEFT(SPBH,2),'L','') AS int) AS 左边编号,(SELECT TOP 1 ZLBZYZM FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS ZLBZYZM , (SELECT TOP 1 CPJCBG FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS CPJCBG , (SELECT TOP 1 PGZFZRFLCNS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS PGZFZRFLCNS , (SELECT TOP 1 FDDBRCNS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS FDDBRCNS , (SELECT TOP 1 SHFWGDYCN FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS SHFWGDYCN , (SELECT TOP 1 CPSJSQS FROM AAA_TBD WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND AAA_TBD.SPBH=AAA_PTSPXXB.SPBH ORDER BY CreateTime DESC) AS CPSJSQS  FROM AAA_PTSPXXB  WHERE SFYX='是' AND AAA_PTSPXXB.SPBH NOT IN (SELECT SPBH FROM AAA_CSSPB WHERE DLYX='" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() + "' AND (SHZT='未审核' OR SHZT='审核通过') AND SFBGZZHBZ='否' GROUP BY SPBH) ) AS TABLE1 ";
            #endregion

            ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
            ht_where["page_index"] = "0";
            ht_where["page_size"] = "25";  //每页显示的条数(必须设置)
            ht_where["serach_Row_str"] = "*";


            #region ht_where["search_tbname"]
            ht_where["search_tbname"] = tableName;
            if (basicRadioButton2.Checked)
                ht_where["search_tbname"] = tableName_Other;
            #endregion

            ht_where["search_mainid"] = " Number ";
            ht_where["search_str_where"] = " 1=1 ";
            if (txtSPMC.Text.Trim() != "")
            {
                ht_where["search_str_where"] = " 1=1 AND SPMC LIKE '%" + this.txtSPMC.Text.Trim() + "%' ";
            }
            ht_where["search_paixu"] = " ASC ";
            ht_where["search_paixuZD"] = " 左边编号 ASC,右边编号 ";

        }

        private void basicButton2_Click(object sender, EventArgs e)
        {
            GetData(null);
        }

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                //获取选中值
                int rowindex = e.RowIndex;
                //回调
                Hashtable return_ht = new Hashtable();
                return_ht["商品编号"] = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();//商品编号
                return_ht["商品名称"] = dataGridView1.Rows[rowindex].Cells[2].Value.ToString();//商品编号
                return_ht["规格"] = dataGridView1.Rows[rowindex].Cells[3].Value.ToString();//商品编号
                return_ht["合同期限"] = dataGridView1.Rows[rowindex].Cells[4].Value.ToString();//商品编号
                return_ht["计价单位"] = dataGridView1.Rows[rowindex].Cells[5].Value.ToString();//商品编号
                return_ht["平台批量"] = dataGridView1.Rows[rowindex].Cells[6].Value.ToString();//商品编号
                return_ht["最低价格"] = dataGridView1.Rows[rowindex].Cells[7].Value.ToString();//商品编号
                return_ht["卖家批量"] = dataGridView1.Rows[rowindex].Cells[8].Value.ToString();//商品编号
                return_ht["上传资质"] = dataGridView1.Rows[rowindex].Cells[9].Value.ToString();//该商品需要上传的资质
                return_ht["ZLBZYZM"] = dataGridView1.Rows[rowindex].Cells[10].Value;//下面是6个必要的资质，最后一次使用过的
                return_ht["CPJCBG"] = dataGridView1.Rows[rowindex].Cells[11].Value;
                return_ht["PGZFZRFLCNS"] = dataGridView1.Rows[rowindex].Cells[12].Value;
                return_ht["FDDBRCNS"] = dataGridView1.Rows[rowindex].Cells[13].Value;
                return_ht["SHFWGDYCN"] = dataGridView1.Rows[rowindex].Cells[14].Value;
                return_ht["CPSJSQS"] = dataGridView1.Rows[rowindex].Cells[15].Value;
                dftForParent(return_ht);
                //关闭弹窗
                this.Close();

            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                //获取选中值
                int rowindex = e.RowIndex;

                //回调
                Hashtable return_ht = new Hashtable();
                return_ht["商品编号"] = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();//商品编号
                return_ht["商品名称"] = dataGridView1.Rows[rowindex].Cells[2].Value.ToString();//商品编号
                return_ht["规格"] = dataGridView1.Rows[rowindex].Cells[3].Value.ToString();//商品编号
                return_ht["合同期限"] = dataGridView1.Rows[rowindex].Cells[4].Value.ToString();//商品编号
                return_ht["计价单位"] = dataGridView1.Rows[rowindex].Cells[5].Value.ToString();//商品编号
                return_ht["平台批量"] = dataGridView1.Rows[rowindex].Cells[6].Value.ToString();//商品编号
                return_ht["最低价格"] = dataGridView1.Rows[rowindex].Cells[7].Value.ToString();//商品编号
                return_ht["卖家批量"] = dataGridView1.Rows[rowindex].Cells[8].Value.ToString();//商品编号
                return_ht["上传资质"] = dataGridView1.Rows[rowindex].Cells[9].Value.ToString();//该商品需要上传的资质
                return_ht["ZLBZYZM"] = dataGridView1.Rows[rowindex].Cells[10].Value;//下面是6个必要的资质，最后一次使用过的
                return_ht["CPJCBG"] = dataGridView1.Rows[rowindex].Cells[11].Value;
                return_ht["PGZFZRFLCNS"] = dataGridView1.Rows[rowindex].Cells[12].Value;
                return_ht["FDDBRCNS"] = dataGridView1.Rows[rowindex].Cells[13].Value;
                return_ht["SHFWGDYCN"] = dataGridView1.Rows[rowindex].Cells[14].Value;
                return_ht["CPSJSQS"] = dataGridView1.Rows[rowindex].Cells[15].Value;
                dftForParent(return_ht);
                //关闭弹窗
                this.Close();

            }
        }

        private void fmNMRSP_AddSP_Load(object sender, EventArgs e)
        {
            GetData(null);
        }
    }
}
