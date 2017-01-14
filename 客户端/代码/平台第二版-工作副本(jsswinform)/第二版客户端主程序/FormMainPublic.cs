using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using 客户端主程序.DataControl;
using 客户端主程序.SubForm.NewCenterForm;
using 客户端主程序.NewDataControl;
using 客户端主程序.SubForm.NewCenterForm.GZZD;
using 客户端主程序.Support;
namespace 客户端主程序
{

    public partial class FormMainPublic : BasicForm
    {
        //处理窗口淡出淡入，这个只用于处理提醒窗口
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
 
       
        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;   

        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 我的账户管理中心，只能开一个
        /// </summary>
        Center2013 FC;

        /// <summary>
        /// 线程集合
        /// </summary>dataGridViewCellStyleXXXX
        public Hashtable Thread_HT = new Hashtable();
        /// <summary>
        /// 线程集合
        /// </summary>
        public Hashtable Thread_HT_Sub = new Hashtable();

        ADNew ad;
        FormGJSS ss;
        /// <summary>
        /// 登陆窗口
        /// </summary>
        FormLogin FL;

        /// <summary>
        /// 线程被调用了停止
        /// </summary>
        bool TBestop = false;

        #region 弹窗网页的处理



        /// <summary>
        /// 加载弹出网页
        /// </summary>
        private void initTC()
        {
 
            string url = "http://www.fm8844.com/forever/jypttanchuang/";
            ad = new ADNew(url);
            ad.StartPosition = FormStartPosition.CenterScreen;
            ad.ShowInTaskbar = true;
            
           // ad.Show();
           

        }


 
        #endregion

        #region 键盘精灵的相关处理
        /// <summary>
        /// 键盘鼠标钩子类
        /// </summary>
        UserActivityHook actHook;

        /// <summary>
        /// 自定义一个标签用于快速查找的显示
        /// </summary>
        protected static Panel panel;

        //相应键盘事件
        private void KeyEventHandler_Run(object sender, KeyEventArgs e)
        {

            CMS_datagrid.Hide();
            TB_KEY.Focus();
            if (e.KeyCode == Keys.Enter && txbz == "显示")
            {
                listView1_MouseDoubleClick(null,null);
            }
        }

        //窗口失去焦点时，停止钩子
        private void FormMainPublic_Deactivate(object sender, EventArgs e)
        {
            actHook.Stop();
        }
        //窗口获得焦点时，启动钩子
        private void FormMainPublic_Activated(object sender, EventArgs e)
        {
            actHook.Start(false,true);
        }

        /// <summary>
        /// 初始化键盘精灵
        /// </summary>
        private void initKeyHost()
        {
            //初始化钩子，只挂在键盘钩子
            actHook = new UserActivityHook(false, true);
            actHook.KeyDown += new KeyEventHandler(KeyEventHandler_Run);

            //把键盘精灵搞到顶层区域
            panel_JPJL.Width = 257;
            panel_JPJL.Parent = this;
            panel_JPJL.Visible = true;
            panel_JPJL.BringToFront();
            //弄到屏幕往下的位置
            panel_JPJL.Location = new Point(this.Width - panel_JPJL.Width - 4, this.Height + 20);
            //panel_JPJL.Location = new Point(this.Width - panel_JPJL.Width - 4, this.Height - panel_JPJL.Height - 3);
        }


        //检索内容变更，执行检索(事件)
        private void TB_KEY_TextChanged(object sender, EventArgs e)
        {
            //TB_KEY_TextChanged_Run();

            if (TB_KEY.Text.Trim() != "" && panel_JPJL.Location.Y > this.Height)
            {
                //显示出键盘精灵区域
                txbz = "显示";
                timerJPJL.Enabled = true;
            }
            string nowstr = TB_KEY.Text.Trim();
            listView1.Items.Clear();


            //线程存在才执行  
            if (Thread_HT.ContainsKey("键盘精灵查询"))
            {
                foreach (DictionaryEntry de in (Hashtable)(Thread_HT["键盘精灵查询"]))
                {
                    Thread trdx = (Thread)(de.Value);
                    if (trdx != null)
                    {
                        trdx.Abort();//退出线程   
                    }
                }
                Thread_HT["键盘精灵查询"] = new Hashtable();

            }

            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_jpjl);
            Hashtable InPutHT = new Hashtable();
            InPutHT["新值"] = nowstr;
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.Beginjpjl));
            trd.Name = "键盘精灵查询|" + Guid.NewGuid().ToString();
            trd.IsBackground = true;
            ((Hashtable)(Thread_HT[trd.Name.Split('|')[0]]))[trd.Name] = trd;
            trd.Start();
        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_jpjl(Hashtable OutPutHT)
        {
            try
            {

                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_jpjl_Invoke), new Hashtable[] { OutPutHT });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

                //处理非线程创建的控件
        private void ShowThreadResult_jpjl_Invoke(Hashtable OutPutHT)
        {

            //if (OutPutHT.ContainsKey("效率检测"))
            //{
            //    label1.Text = OutPutHT["效率检测"].ToString();
            //}
            //每次从服务器更新数据后，更新滚动条可设置的最大值，防止出错。
            if (OutPutHT.ContainsKey("查询结果"))
            {      
                //var query = ((IEnumerable<DataRow>)OutPutHT["查询结果"]);
                
                DataTable dt = (DataTable)OutPutHT["查询结果"];

                if (dt == null)
                    return;
                foreach (DataRow items in dt.Rows)
                {
                    ListViewItem LVI = new ListViewItem(new string[] { items["商品编号"].ToString(), items["商品名称"].ToString(), items["合同周期n"].ToString(), items["型号规格"].ToString() });
                    listView1.Items.Add(LVI);
                }

                if (listView1.Items.Count > 0)
                {
                    //listView1.Focus();
                    listView1.Items[0].Selected = true;
                    //listView1.Items[0].Focused = true;
                }

                

            }
        }







        //焦点离开键盘精灵区域时，清理数据并隐藏键盘精灵
        private void panel_JPJL_Leave(object sender, EventArgs e)
        {
            TB_KEY.Text = "";
            listView1.Items.Clear();

            //显示出键盘精灵区域
            txbz = "隐藏";
            timerJPJL.Enabled = true;
        }

        //特效标志
        private string txbz = "";
        //键盘精灵特效
        private void timerJPJL_Tick(object sender, EventArgs e)
        {
            switch (txbz)
            {
                case "显示":
                    panel_JPJL.Location = new Point(panel_JPJL.Location.X, panel_JPJL.Location.Y - 20);
                    if (panel_JPJL.Location.Y < this.Height - panel_JPJL.Height + 20)
                    {
                        panel_JPJL.Location = new Point(panel_JPJL.Location.X, this.Height - panel_JPJL.Height - 3);
                        timerJPJL.Enabled = false;
                    }
                    break;
                case "隐藏":
                    panel_JPJL.Location = new Point(panel_JPJL.Location.X, panel_JPJL.Location.Y + 20);
                    if (panel_JPJL.Location.Y > this.Height + 20)
                    {
                        panel_JPJL.Location = new Point(this.Width - panel_JPJL.Width - 4, this.Height + 20);
                        timerJPJL.Enabled = false;
                    }
                    break;
                default:
                    timerJPJL.Enabled = false;
                    break;
            }

        
        }

     
        //直接关闭键盘精灵
        private void label2_Click(object sender, EventArgs e)
        {
            panel_JPJL_Leave(null,null);
        }

        #endregion




        #region 主窗体的退出控制等事件
        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void FormMainPublic_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        /// <summary>
        /// 退出并关闭系统
        /// </summary>
        public void ExitApp()
        {
            try
            {
                //notifyIconMain.Visible = false;
                //notifyIconMain.Dispose();
                AppSingleton.OnExit(null,null);
                Process.GetCurrentProcess().Kill();  
                //System.Environment.Exit(0);
            }
            catch(Exception ex){
                Process.GetCurrentProcess().Kill();  
            }

        }


        /// <summary>
        /// 退出并关闭系统(仅用于强制踢掉)
        /// </summary>
        /// <param name="msgstr">不让用的消息提示</param>
        public void ExitApp_T(string msgstr)
        {
            try
            {
                //notifyIconMain.Visible = false;
                //notifyIconMain.Dispose();



                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add(msgstr);
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "测试弹窗", Almsg3);
                FRSE3.ShowDialog();

                AppSingleton.OnExit(null, null);
                Process.GetCurrentProcess().Kill();
                //System.Environment.Exit(0);
            }
            catch (Exception ex) {
                Process.GetCurrentProcess().Kill();
            }

        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Activate();
        }

        private void 退出平台ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp();
        }
        #endregion


        #region 窗体淡出，窗体最小化最大化退出等特殊控制，所有窗体都有这个玩意
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
        public void MaxDC()
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
            try
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
            catch (Exception ex) {
                Support.StringOP.WriteLog("主窗体重绘错误：" + ex.ToString());
            }
        }

        #endregion



        #region 提醒检查线程相关
        /// <summary>
        /// 开启提醒检查线程
        /// </summary>
        private void beginTrayMsg()
        {
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            OpenThreadIndex RTCTM = new OpenThreadIndex(null, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCTM.BeginRun));
            trd.Name = "检查提醒";
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_TrayMsg(Hashtable OutPutHT)
        {
            try
            {
         
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_TrayMsg_Invoke), new Hashtable[] { OutPutHT });
        
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_TrayMsg_Invoke(Hashtable OutPutHT)
        {
            if (OutPutHT.ContainsKey("踢人消息"))
            {
                //开始踢人
                ExitApp_T(OutPutHT["踢人消息"].ToString());
            }
            else
            {
                //正常提醒
                SubForm.FormTrayMsg FTM = new SubForm.FormTrayMsg(OutPutHT);
                FTM.Show();
            }
        }
        #endregion

        #region 自选商品的添加和删除





        /// <summary>
        /// 获得自选商品本地临时表
        /// </summary>
        private void InitZXSPtemp()
        {
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();

            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResultZXSP);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunInitZXSPtemp));
            trd.Name = "获得自选商品本地临时表";
            trd.IsBackground = true;
            trd.Start();

        }



        /// <summary>
        /// 启动操作自选商品线程
        /// </summary>
        /// <param name="SPBH"></param>
        /// <param name="edit">加入,删除</param>
        private void SetZXSP(string SPBH, string edit)
        {
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["商品编号"] = SPBH;//
            InPutHT["操作"] = edit;//

            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResultZXSP);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunZXSPedit));
            trd.Name = "自选商品操作";
            trd.IsBackground = true;
            trd.Start();

        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResultZXSP(Hashtable OutPutHT)
        {
            try
            {
        
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResultZXSP_Invoke), new Hashtable[] { OutPutHT });
             
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }


        //处理非线程创建的控件
        private void ShowThreadResultZXSP_Invoke(Hashtable OutPutHT)
        {


            string re = OutPutHT["执行结果"].ToString();
            string spbh = OutPutHT["所操作商品编号"].ToString();
            if (PublicDS.PublisDsZXSP != null)
            {
                if (re.IndexOf("删除") >= 0)
                {
                    int del_index = -99;
                    for (int i = 0; i < PublicDS.PublisDsZXSP.Tables["自选商品临时表"].Rows.Count; i++)
                    {
                        if (spbh.Trim() == PublicDS.PublisDsZXSP.Tables["自选商品临时表"].Rows[i]["商品编号"].ToString().Trim())
                        {
                            del_index = i;
                            break;
                        }
                    }
                    if (del_index != -99)
                    {
                        PublicDS.PublisDsZXSP.Tables["自选商品临时表"].Rows.RemoveAt(del_index);
                    }
   



                }
                if (re.IndexOf("添加") >= 0)
                {
                    PublicDS.PublisDsZXSP.Tables["自选商品临时表"].Rows.Add(new string[] { spbh  });
                }
            }

            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(re);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "测试弹窗", Almsg3);
            FRSE3.ShowDialog();

        }


        #endregion

        #region 主体部分交易数据处理,包括默认和自选商品




        /// <summary>
        /// 初始化默认主大盘的列和样式
        /// </summary>
        private void init_dataGridViewMain_Columns_default()
        {
            //调整列宽和自定义滚动条
            string kuanduliebiao = "42,72,72,107,12,75,77,72,73,73,72,73,44,88,69,91,91,76,110,62,91,88,78,121,74,75,104,72,75,150,100,100,100,100,100,100,100,100,100,";
            string[] liekuan_arr = kuanduliebiao.Split(',');
            for (int i = 0; i < dataGridViewMain.ColumnCount; i++)
            {
                dataGridViewMain.Columns[i].Width = Convert.ToInt32(liekuan_arr[i].Replace(" ", ""));
            }
            int hide_width = dataGridViewMain.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - this.Width + 35;
            if (hide_width < 0)
            {
                hide_width = 0;
            }
            hScrollBar1.Minimum = 0;
            hScrollBar1.Value = 0;
            hScrollBar1.Maximum = hide_width;


 

   

        }
        //保存下默认大盘样式
        ArrayList al_dgv = new ArrayList();
        /// <summary>
        /// 初始化成交详情大盘的列和样式
        /// </summary>
        private void init_dataGridViewMain_Columns_cjxq()
        {

            //清理列
            dataGridViewMain.Columns.Clear();

            //定义默认样式
            DataGridViewCellStyle dataGridViewCellStyleXXXX = new DataGridViewCellStyle();
            dataGridViewCellStyleXXXX.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyleXXXX.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyleXXXX.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyleXXXX.SelectionForeColor = System.Drawing.Color.White;

            //遍历列并赋予属性
            string[] arr_lie = new string[] {"商品编号" , "商品名称" , "计价单位", "定标时间", "定标数量", "定标价格","定标金额", "合同期限","已提货数量","已发货数量","清盘时间","清盘类型" };
            string[] arr_lie_tip = new string[] { "", "按照国家标准、行业标准对商品名称的规定或业内通用名称确定的上线商品名称。", "", "", "", "", "", "交易平台的《电子购货合同》期限分为“即时”、“三个月”和“一年”三种。\r\n“三个月”与“一年”的，买方可在定标后《电子购货合同》到期前5天内自由提货；\r\n“即时”" +
    "的，买方须一次性下达完《提货单》， 卖方须于收到《提货单》后24小时内一次性发货。", "", "", "", "" };
            for (int p = 0; p < arr_lie.Length; p++)
            {
                DataGridViewTextBoxColumn dgvcnew = new DataGridViewTextBoxColumn();
                dgvcnew.DefaultCellStyle = dataGridViewCellStyleXXXX;

               
                switch (p)
                {
                    case 3:
                        DataGridViewCellStyle dataGridViewCellStyleXXXX_new1 = dataGridViewCellStyleXXXX.Clone();
                        dataGridViewCellStyleXXXX_new1.ForeColor = System.Drawing.Color.Yellow;
                        dataGridViewCellStyleXXXX_new1.SelectionForeColor = System.Drawing.Color.Yellow;
                        dgvcnew.DefaultCellStyle = dataGridViewCellStyleXXXX_new1;
                        break;
                    case 4:
                        DataGridViewCellStyle dataGridViewCellStyleXXXX_new2 = dataGridViewCellStyleXXXX.Clone();
                        dataGridViewCellStyleXXXX_new2.ForeColor = System.Drawing.Color.LimeGreen;
                        dataGridViewCellStyleXXXX_new2.SelectionForeColor = System.Drawing.Color.LimeGreen;
                        dgvcnew.DefaultCellStyle = dataGridViewCellStyleXXXX_new2;
                        break;
                    case 5:
                        DataGridViewCellStyle dataGridViewCellStyleXXXX_new3 = dataGridViewCellStyleXXXX.Clone();
                        dataGridViewCellStyleXXXX_new3.ForeColor = System.Drawing.Color.LimeGreen;
                        dataGridViewCellStyleXXXX_new3.SelectionForeColor = System.Drawing.Color.LimeGreen;
                        dgvcnew.DefaultCellStyle = dataGridViewCellStyleXXXX_new3;
                        break;
                    case 6:
                        DataGridViewCellStyle dataGridViewCellStyleXXXX_new4 = dataGridViewCellStyleXXXX.Clone();
                        dataGridViewCellStyleXXXX_new4.ForeColor = System.Drawing.Color.Red;
                        dataGridViewCellStyleXXXX_new4.SelectionForeColor = System.Drawing.Color.Red;
                        dgvcnew.DefaultCellStyle = dataGridViewCellStyleXXXX_new4;
                        break;
                    default:
                        break;
                }

                dgvcnew.FillWeight = 55F;
                dgvcnew.Frozen = false;
                dgvcnew.HeaderText = arr_lie[p];
                dgvcnew.MinimumWidth = 30;
                dgvcnew.Name = arr_lie[p];
                dgvcnew.ReadOnly = true;
                dgvcnew.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                dgvcnew.ToolTipText = arr_lie_tip[p];
                dataGridViewMain.Columns.Add(dgvcnew);
            }


            //调整列宽和自定义滚动条
            string kuanduliebiao = "80,160,80,160,80,80,80,80,100,100,170,100";
            string[] liekuan_arr = kuanduliebiao.Split(',');
            for (int i = 0; i < dataGridViewMain.ColumnCount; i++)
            {
                dataGridViewMain.Columns[i].Width = Convert.ToInt32(liekuan_arr[i].Replace(" ", ""));
            }

            int hide_width = dataGridViewMain.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - this.Width + 35;
            if (hide_width < 0)
            {
                hide_width = 0;
            }
            hScrollBar1.Minimum = 0;
            hScrollBar1.Value = 0;
            hScrollBar1.Maximum = hide_width;




        }



        /// <summary>
        /// 重设大盘样式到默认样式
        /// </summary>
        private void ReSetDataGridView_To_default(ArrayList al)
        {
            //清理列
            dataGridViewMain.Columns.Clear();
            //生成新列
            for (int y = 0; y < al.Count; y++)
            {
                int i = dataGridViewMain.Columns.Add((DataGridViewColumn)(al[y]));
            }
               
        }

        /// <summary>
        /// 启动加载交易数据线程
        /// </summary>
        private void TestLoadList()
        {
            //设置新列表的列头和样式
            ReSetDataGridView_To_default(al_dgv);
            init_dataGridViewMain_Columns_default();

            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["行数"] = dataGridViewMain.Rows.Count;
            InPutHT["列数"] = dataGridViewMain.ColumnCount;
            vScrollBar1.Value = 0;
            Program.nowIndex = vScrollBar1.Value;

            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRun));
            trd.Name = "加载默认数据|" + Guid.NewGuid().ToString();
            trd.IsBackground = true;
            ((Hashtable)(Thread_HT[trd.Name.Split('|')[0]]))[trd.Name] = trd;
            trd.Start();

            //变更菜单
            加入自选商品ToolStripMenuItem.Visible = true;
            删除自选商品ToolStripMenuItem.Visible = false;
        }

        /// <summary>
        /// 开启线程，加载所选二级分类的商品
        /// </summary>
        private void TestLoadListErjiFL(string flbh)
        {
            //设置新列表的列头和样式
            ReSetDataGridView_To_default(al_dgv);
            init_dataGridViewMain_Columns_default();
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["分类编号"] = flbh;//
            InPutHT["行数"] = dataGridViewMain.Rows.Count;
            InPutHT["列数"] = dataGridViewMain.ColumnCount;
            vScrollBar1.Value = 0;
            Program.nowIndex = vScrollBar1.Value;
            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunErJi));
            trd.Name = "加载二级分类限定数据|" + Guid.NewGuid().ToString();
            trd.IsBackground = true;
            ((Hashtable)(Thread_HT[trd.Name.Split('|')[0]]))[trd.Name] = trd;
            trd.Start();
            //变更菜单
            加入自选商品ToolStripMenuItem.Visible = true;
            删除自选商品ToolStripMenuItem.Visible = false;
        }

        /// <summary>
        /// 启动加载自选交易数据线程
        /// </summary>
        private void TestLoadListZXSP()
        {
            //设置新列表的列头和样式
            ReSetDataGridView_To_default(al_dgv);
            init_dataGridViewMain_Columns_default();
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["行数"] = dataGridViewMain.Rows.Count;
            InPutHT["列数"] = dataGridViewMain.ColumnCount;
            vScrollBar1.Value = 0;
            Program.nowIndex = vScrollBar1.Value;
            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunZXSP));
            trd.Name = "加载自选数据|" + Guid.NewGuid().ToString();
            trd.IsBackground = true;
            ((Hashtable)(Thread_HT[trd.Name.Split('|')[0]]))[trd.Name] = trd;
            trd.Start();

            //变更菜单
            加入自选商品ToolStripMenuItem.Visible = false;
            删除自选商品ToolStripMenuItem.Visible = true;
        }



        /// <summary>
        /// 启动加载成交详情线程
        /// </summary>
        private void otherlisttest()
        {
            //设置新列表的列头和样式
            init_dataGridViewMain_Columns_cjxq();

            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["行数"] = dataGridViewMain.Rows.Count;
            InPutHT["列数"] = dataGridViewMain.ColumnCount;
            vScrollBar1.Value = 0;
            Program.nowIndex = vScrollBar1.Value;

            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunCJXQ));
            trd.Name = "加载成交详情|" + Guid.NewGuid().ToString();
            trd.IsBackground = true;
            ((Hashtable)(Thread_HT[trd.Name.Split('|')[0]]))[trd.Name] = trd;
            trd.Start();

            //变更菜单
            加入自选商品ToolStripMenuItem.Visible = false;
            删除自选商品ToolStripMenuItem.Visible = false;
        }



        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult(Hashtable OutPutHT)
        {
            try
            {
            
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_Invoke), new Hashtable[] { OutPutHT });
      
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }



        private void SetOneRowSP(int newrowindex, Hashtable OutPutHT, DataRow row, int[] bzindex, Hashtable ht_old, int this_ColumnCount)
        {
            //特殊处理等级图片
            object str_DJ = null;
            if (dataGridViewMain.Columns.Contains("信用等级n"))
            {
                str_DJ = dataGridViewMain.Rows[newrowindex].Cells["信用等级n"].Value;
            }



            if (str_DJ != null)
            {
                //为主盘处理特殊的列显示

                //处理信用等级
                if (dataGridViewMain.Columns.Contains("当前卖方信用等级") && dataGridViewMain.Rows[newrowindex].Cells[0].Value.ToString().Trim() != "")
                {
                    if (StringOP.IsNumberXS(str_DJ.ToString().Trim().Replace("-", ""), int.MaxValue, 2))
                    {
                        Image[] imageCollectio = JYFXYMX.GetXYImages(Convert.ToDouble(dataGridViewMain.Rows[newrowindex].Cells["信用等级n"].Value.ToString().Trim()));
                        Image newTu = JYFXYMX.MergerImg(imageCollectio);
                        dataGridViewMain.Rows[newrowindex].Cells["当前卖方信用等级"] = new DataGridViewImageCell();
                        dataGridViewMain.Rows[newrowindex].Cells["当前卖方信用等级"].Value = newTu;
                    }
                    else
                    {
                        dataGridViewMain.Rows[newrowindex].Cells["当前卖方信用等级"] = new DataGridViewTextBoxCell();
                        dataGridViewMain.Rows[newrowindex].Cells["当前卖方信用等级"].Value = "";
                    }



                }



                //处理蓝点
                if (dataGridViewMain.Rows[newrowindex].Cells["LD"].Value.ToString().Trim() == "")
                {
                    dataGridViewMain.Rows[newrowindex].Cells["LD"] = new DataGridViewTextBoxCell();
                    dataGridViewMain.Rows[newrowindex].Cells["LD"].Value = "";
                }
                else
                {
                    dataGridViewMain.Rows[newrowindex].Cells["LD"] = new DataGridViewImageCell();
                    dataGridViewMain.Rows[newrowindex].Cells["LD"].Value = landian;
                }


                //处理升降幅
                if (dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value.ToString().Trim() == "--" || dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value.ToString().Trim() == "0.00" || dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value.ToString().Trim() == "0")
                {
                    dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Style.ForeColor = System.Drawing.Color.FromArgb(240, 248, 136); //淡黄色
                }
                else
                {
                    if (dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value.ToString().Trim() != "")
                    {
                        if (dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value.ToString().Trim().IndexOf('-') >= 0)
                        {
                            dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value = "↓" + dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value.ToString();
                            dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Style.ForeColor = System.Drawing.Color.Lime;//绿色
                        }
                        else
                        {
                            dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value = "↑" + dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Value.ToString();
                            dataGridViewMain.Rows[newrowindex].Cells["升降幅"].Style.ForeColor = System.Drawing.Color.Red;//红色
                        }
                    }
                }


            }


            //处理合同期限文字颜色
            //cell.Style.BackColor = System.Drawing.Color.FromArgb(243, 126, 126);
            //cell.Style.SelectionBackColor = System.Drawing.Color.FromArgb(243, 126, 126);
            string qx = dataGridViewMain.Rows[newrowindex].Cells["合同期限"].Value.ToString();
            switch (qx)
            {
                case "即时":
                    dataGridViewMain.Rows[newrowindex].Cells["合同期限"].Style.ForeColor = System.Drawing.Color.FromArgb(250, 190, 190);//淡红色
                    break;
                case "三个月":
                    dataGridViewMain.Rows[newrowindex].Cells["合同期限"].Style.ForeColor = System.Drawing.Color.FromArgb(245, 250, 190); //淡黄色
                    break;
                case "一年":
                    dataGridViewMain.Rows[newrowindex].Cells["合同期限"].Style.ForeColor = System.Drawing.Color.FromArgb(190, 215, 250);//淡蓝色
                    break;
                default:
                    dataGridViewMain.Rows[newrowindex].Cells["合同期限"].Style.ForeColor = System.Drawing.Color.White;
                    break;
            }

            if (OutPutHT.ContainsKey("是否对比标志") && OutPutHT["是否对比标志"].ToString() == "差异底色")
            {
                string keystr = row[bzindex[0]].ToString().Trim() + row[bzindex[1]].ToString().Trim();
                if (ht_old.ContainsKey(keystr))
                {
                    for (int c = 0; c < this_ColumnCount; c++)
                    {
                        if (((ArrayList)(ht_old[keystr]))[c].ToString().Trim() != row[c].ToString().Trim() && c > 0 && dataGridViewMain.Rows[newrowindex].Cells[c].OwningColumn.Name != "当前卖方信用等级" && dataGridViewMain.Rows[newrowindex].Cells[c].OwningColumn.Name != "LD")
                        {

                            dataGridViewMain.Rows[newrowindex].Cells[c].Style.BackColor = System.Drawing.Color.FromArgb(65, 120, 240);
                            dataGridViewMain.Rows[newrowindex].Cells[c].Style.SelectionBackColor = System.Drawing.Color.FromArgb(65, 120, 240);

                        }
                    }
                }

            }
        }

 
        int si1 = 0;
        int h1 = -1;
        //处理非线程创建的控件
        private void ShowThreadResult_Invoke(Hashtable OutPutHT)
        {
            dataGridViewMain.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(dataGridViewMain, true, null);

            try
            {

                //每次从服务器更新数据后，更新滚动条可设置的最大值，防止出错。
                if (OutPutHT.ContainsKey("大盘竖向滚动条最大值") && OutPutHT.ContainsKey("大盘竖向滚动条最大值"))
                {
                    vScrollBar1.Maximum = Convert.ToInt32(OutPutHT["大盘竖向滚动条最大值"]);
                }
                if (TBestop)
                {
                    TBestop = false;
                    PBload.Visible = false;
                    return;
                }

                if (dataGridViewMain == null)
                {
                    PBload.Visible = false;
                    return;
                }
                //DataSet dstest = (DataSet)(OutPutHT["测试列表数据表"]);

                //加载数据，这里不能使用绑定，要手工画上
                if (OutPutHT.ContainsKey("可以绑定") && (bool)OutPutHT["可以绑定"])
                {




                    int this_ColumnCount = dataGridViewMain.ColumnCount;
                    if (OutPutHT.ContainsKey("执行标记") && OutPutHT["执行标记"].ToString() == "一次性处理所有数据")
                    {
                        //禁止pnl重绘,这里比较特殊，一定要由这个东西
                        SendMessage(dataGridViewMain.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

                        //清理并显示新数据 
                        DataTable newall_tb = (DataTable)(OutPutHT["值"]);

                        //保留原数据等待对比
                        Hashtable ht_old = new Hashtable();
                        int[] bzindex = new int[2] { 0, 0 };
                        if (OutPutHT.ContainsKey("是否对比标志") && OutPutHT["是否对比标志"].ToString() == "差异底色")
                        {
                            if (dataGridViewMain.Columns.Contains("商品编号") && dataGridViewMain.Columns.Contains("合同期限"))
                            {
                                bzindex[0] = dataGridViewMain.Columns["商品编号"].Index;
                                bzindex[1] = dataGridViewMain.Columns["合同期限"].Index;
                            }

                            bool need_fugai = false; //是否需要重画表格，跟旧数据一样就不用重画了，减少卡顿。
                            foreach (DataGridViewRow row in dataGridViewMain.Rows)
                            {
                                ArrayList al_old_cell = new ArrayList();
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    string newstr = cell.Value.ToString().Replace("↓", "").Replace("↑", "");
                                    al_old_cell.Add(newstr);
                                    if (newstr != "System.Drawing.Bitmap" && newstr != newall_tb.Rows[cell.RowIndex][cell.ColumnIndex].ToString())
                                    {
                                        need_fugai = true; //发现有不一样的，得覆盖
                                    }
                                   
                                }
                              
                                ht_old[al_old_cell[bzindex[0]].ToString().Trim() + al_old_cell[bzindex[1]].ToString().Trim()] = al_old_cell;

                            }
                            //发现不需要重画界面，跟旧数据一样的，直接返回。什么也不做。
                            if (!need_fugai && dataGridViewMain.RowCount > 0)
                            {
                                //允许重绘pnl  
                                SendMessage(dataGridViewMain.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                                dataGridViewMain.Invalidate(true);
                                return;
                            }
                        }



                        si1 = dataGridViewMain.HorizontalScrollingOffset;
                        if (dataGridViewMain.SelectedRows.Count > 0)
                        {
                            h1 = dataGridViewMain.SelectedRows[0].Index;
                        }
                        else
                        {
                            h1 = -1;
                        }


                        if (OutPutHT.ContainsKey("来源") && OutPutHT["来源"] != null)
                        {
                            if (OutPutHT["来源"].ToString() == "滚轮向下")
                            {
                                int RowsCount = newall_tb.Rows.Count;
                                dataGridViewMain.Rows.RemoveAt(0);
                                int sy = dataGridViewMain.Rows.Add(newall_tb.Rows[RowsCount - 1].ItemArray);
                                SetOneRowSP(sy, OutPutHT, newall_tb.Rows[RowsCount - 1], bzindex, ht_old, this_ColumnCount);
                            }
                            if (OutPutHT["来源"].ToString() == "滚轮向上")
                            {
                                dataGridViewMain.Rows.RemoveAt(dataGridViewMain.Rows.Count - 1);
                                dataGridViewMain.Rows.Insert(0,newall_tb.Rows[0].ItemArray);
                                SetOneRowSP(0, OutPutHT, newall_tb.Rows[0], bzindex, ht_old, this_ColumnCount);
                            }

                        }
                        else
                        {
                            #region 一条一条添加数据。
                            dataGridViewMain.Rows.Clear();
                            foreach (DataRow row in newall_tb.Rows)
                            {
 
                                int newrowindex = dataGridViewMain.Rows.Add(row.ItemArray);
                                SetOneRowSP(newrowindex, OutPutHT, row, bzindex, ht_old, this_ColumnCount);
                              

                            }


                            #endregion
                        }




                        dataGridViewMain.HorizontalScrollingOffset = si1;
                        if (h1 > 0 && dataGridViewMain.Rows.Count > 0)
                        {
                            dataGridViewMain.Rows[0].Selected = false;
                            dataGridViewMain.Rows[h1].Selected = true;
                        }

                        ReSethScrollBar1();
                        //允许重绘pnl  
                        SendMessage(dataGridViewMain.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                        dataGridViewMain.Invalidate(true);

 
                    }

                    
                    PBload.Visible = false;
                }
                else
                {
                    PBload.Visible = false;
                    //dataGridViewMain.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误(更新大盘)：" + ex.ToString());
            }


        }






        #endregion

        #region 软件检查完更新后，真正加载功能数据前，可以提前获取的一些数据或参数

        /// <summary>
        /// 开始启动获取数据的线程
        /// </summary>
        private void BeginGetPublisDsData()
        {
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_PublisDsData);
            DataControl.RunThreadClassCityList RTCCL = new DataControl.RunThreadClassCityList(null, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_PublisDsData(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_PublisDsData_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_PublisDsData_Invoke(Hashtable OutPutHT)
        {
            PublicDS.PublisDsData = (DataSet)OutPutHT["前置数据"];
            //若得到了数据，正式可以让用户使用软件了
            if (PublicDS.PublisDsData != null)
            {

                //开启登陆窗口的操作
                FL.uploadOK();
            }
            else
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("无法初始化基础数据，请检查网络连接。");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "软件初始化提示", Almsg3);
                FRSE3.ShowDialog();
                ExitApp();
            }
        }

        #endregion

        #region 底部的分类样式、功能、数据的处理


        /// <summary>
        /// 所有要展示的分类数据集
        /// </summary>
        DataSet dsFL;

        /// <summary>
        /// 开启加载底部分类线程
        /// </summary>
        private void initFL()
        {
            //开启线程加载底部分类
            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_FL);
            DataControl.RunThreadClassTestFL RCTFL = new DataControl.RunThreadClassTestFL(null, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCTFL.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_FL(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_FL_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }


        //处理非线程创建的控件
        private void ShowThreadResult_FL_Invoke(Hashtable OutPutHT)
        {
            //加载分类列表
            DataSet dstest = (DataSet)(OutPutHT["测试分类列表"]);
            dsFL = dstest;
            Init_FL(dstest);


        }


        /// <summary>
        /// 加载分类到底部菜单
        /// </summary>
        private void Init_FL(DataSet dsfl)
        {
            DataRow[] drRoot_0 = dsfl.Tables[0].Select("SortParentID='0'");
            //清理内容
            TLB_fenlei.Controls.Clear();
            //设置分类数量并开始生成菜单
            int MaxFLnum = 6; //显示的分类数量
            if (drRoot_0.Length < MaxFLnum)
            {
                TLB_fenlei.ColumnCount = drRoot_0.Length * 3;
            }
            else
            {
                TLB_fenlei.ColumnCount = MaxFLnum * 3;
            }
            int flnum = 0;//用来处理分类的数据库ID唯一值
            for (int p = 0; p < TLB_fenlei.ColumnCount; p++)
            {
                string fl_number = drRoot_0[flnum]["SortID"].ToString();
                string fl_name = drRoot_0[flnum]["SortName"].ToString();
                //左边图片
                PictureBox pb1 = new PictureBox();
                pb1.Margin = new System.Windows.Forms.Padding(0);
                pb1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                pb1.TabStop = false;
                pb1.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\qz.jpg");
                pb1.Name = "FL" + fl_number + "-1";
                pb1.Cursor = System.Windows.Forms.Cursors.Hand;
                pb1.Click += new System.EventHandler(fenleishow);

                //中间文字区域
                Label ll = new Label();
                ll.AutoSize = true;
                ll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(149)))), ((int)(((byte)(149)))));
                ll.ForeColor = Color.Black;
                ll.Margin = new System.Windows.Forms.Padding(0);
                ll.Text = fl_name;
                ll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                ll.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
                ll.Name = "FL" + fl_number + "-2";
                ll.Cursor = System.Windows.Forms.Cursors.Hand;
                ll.Click += new System.EventHandler(fenleishow);

                //右边图片
                PictureBox pb2 = new PictureBox();
                pb2.Margin = new System.Windows.Forms.Padding(0);
                pb2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                pb2.TabStop = false;
                pb2.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\zy.jpg");
                pb2.Name = "FL" + fl_number + "-3";
                pb2.Cursor = System.Windows.Forms.Cursors.Hand;
                pb2.Click += new System.EventHandler(fenleishow);

                //放到控件中
                TLB_fenlei.Controls.Add(pb1, p, 0);
                p++;
                TLB_fenlei.Controls.Add(ll, p, 0);
                p++;
                TLB_fenlei.Controls.Add(pb2, p, 0);

                flnum++;
            }


        }

        /// <summary>
        /// 显示详细分类的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fenleishow(object sender, EventArgs e)
        {
            panel_fenlei.Visible = false;

            //获得当前分类ID
            string typestr = sender.GetType().Name;
            string FLnumber = "";
            if (typestr == "Label")
            {
                FLnumber = ((Label)sender).Name.Split('-')[0].Replace("FL", "");
            }
            if (typestr == "PictureBox")
            {
                FLnumber = ((PictureBox)sender).Name.Split('-')[0].Replace("FL", "");
            }
            //获取当前分类控件
            PictureBox dj1 = (PictureBox)findControl(TLB_fenlei, "FL" + FLnumber + "-1");
            Label dj2 = (Label)findControl(TLB_fenlei, "FL" + FLnumber + "-2");
            PictureBox dj3 = (PictureBox)findControl(TLB_fenlei, "FL" + FLnumber + "-3");
            //设置展示层的位置
            panel_fenlei.Location = new Point(dj1.Location.X, TLB_fenlei.Parent.Location.Y - panel_fenlei.Height - 2);

            //根据已经加载好的dsFL数据集动态生成具体的分类内容


            //更改当前分类颜色
            foreach (Control c in TLB_fenlei.Controls)
            {
                if (c.GetType().Name == "Label" || c.GetType().Name == "PictureBox")
                {
                    if (c.Name.Split('-')[1] == "1")
                    {
                        ((PictureBox)c).Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\qz.jpg"); ;
                    }
                    if (c.Name.Split('-')[1] == "2")
                    {
                        ((Label)c).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(149)))), ((int)(((byte)(149)))));
                        ((Label)c).ForeColor = Color.Black;
                    }
                    if (c.Name.Split('-')[1] == "3")
                    {
                        ((PictureBox)c).Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\zy.jpg"); ;
                    }
                }
            }
            dj1.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\hz.jpg");
            dj2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(1)))), ((int)(((byte)(0)))));
            dj2.ForeColor = Color.White;
            dj3.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\hy.jpg");




            //通过指定分类ID，获得所辖所有子分类的商品
            fenleishowBy_SortID(FLnumber);

            //特殊处理，分类编号为48的，不显示二级分类菜单
            if (FLnumber != "48")
            {
                //开启线程，显示具体二级分类
                InitFLerji(FLnumber);

                //显示分类展示层
                panel_fenlei.Visible = true;
                panel_fenlei.Focus();
            }
            else
            {
                panel_fenlei.Visible = false;
            }


        }


        //通过指定分类ID，获得所辖所有子分类的商品
        private void fenleishowBy_SortID(string FLnumber)
        {
            //显示进度
            PBload.Visible = true;
            //停止线程
            StopmainlistTHread();
            //清理
            dataGridViewMain.Rows.Clear();

            //必须先减
            this.dataGridViewMain.CellMouseClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);
            //再加
            this.dataGridViewMain.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);
            //开启线程，加载所选数据列表
            TestLoadListErjiFL(FLnumber);
        }

        /// <summary>
        /// 开线程获取二级分类数据
        /// </summary>
        /// <param name="SortParentID"></param>
        private void InitFLerji(string SortParentID)
        {
            TLPmain.Controls.Clear();

            //开启线程加载底部分类
            //实例化委托,并参数传递给线程执行类开始执行线程
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["父分类"] = SortParentID;//测试参数
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_FLerji);
            DataControl.RunThreadClassTestFL RCTFL = new DataControl.RunThreadClassTestFL(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCTFL.BeginRun_FLerji));
            trd.Name = "加载二级分类";
            trd.IsBackground = true;
            trd.Start();
        }


        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_FLerji(Hashtable OutPutHT)
        {
            try
            {
              
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_FLerji_Invoke), new Hashtable[] { OutPutHT });
             
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }


        //处理非线程创建的控件
        private void ShowThreadResult_FLerji_Invoke(Hashtable OutPutHT)
        {
            try
            {
                //加载分类列表
                DataSet dserji = (DataSet)(OutPutHT["二级分类列表"]);
                if (dserji == null || dserji.Tables[0].Rows.Count < 1)
                {
                    //没有二级分类数据
                    return;
                }
                //开始展示分类
                int lienum = TLPmain.ColumnCount;
                int hangnum = TLPmain.RowCount;
                int shujunum = 0;
                for (int l = 0; l < lienum; l++)
                {
                    for (int h = 0; h < hangnum; h++)
                    {
                        //有数据才加入
                        if (shujunum < dserji.Tables[0].Rows.Count)
                        {
                            //中间文字区域
                            Label ll = new Label();
                            ll.AutoSize = true;
                            ll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                            ll.ForeColor = Color.Black;
                            ll.Margin = new System.Windows.Forms.Padding(0);
                            ll.Text = dserji.Tables[0].Rows[shujunum]["SortName"].ToString();
                            ll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                            ll.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
                            ll.Name = "erjiFL_" + dserji.Tables[0].Rows[shujunum]["SortID"].ToString();
                            ll.Cursor = System.Windows.Forms.Cursors.Hand;
                            ll.Click += new System.EventHandler(fenleishowerji);

                            TLPmain.Controls.Add(ll, l, h);
                        }
                        shujunum++;
                    }

                }
            }
            catch (Exception ex)
            {
                string aaa = ex.ToString();
            }

        }


        //二级分类点击事件
        private void fenleishowerji(object sender, EventArgs e)
        {
            //显示进度
            PBload.Visible = true;
            //停止线程
            StopmainlistTHread();
            //清理
            dataGridViewMain.Rows.Clear();
            Label thisdianji = (Label)sender;
            //必须先减
            this.dataGridViewMain.CellMouseClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);

            this.dataGridViewMain.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);
            //开启线程，加载所选二级分类数据列表
            TestLoadListErjiFL(thisdianji.Name.Replace("erjiFL_", ""));
        }

        /// <summary>
        /// 停止主列表当前所有线程
        /// </summary>
        private void StopmainlistTHread()
        {
            //停止线程
            try
            {
                //线程存在才执行  
                if (Thread_HT.ContainsKey("加载默认数据"))
                {
                    foreach (DictionaryEntry de in (Hashtable)(Thread_HT["加载默认数据"]))
                    {
                        Thread trdx = (Thread)(de.Value);
                        if (trdx != null)
                        {
                            if (trdx.IsAlive)
                            {
                                trdx.Priority = ThreadPriority.Lowest;
                            }
                            TBestop = true;
                        }
                    }
                    Thread_HT["加载默认数据"] = new Hashtable();
                }
                //线程存在才执行
                if (Thread_HT.ContainsKey("加载自选数据"))
                {
                    foreach (DictionaryEntry de in (Hashtable)(Thread_HT["加载自选数据"]))
                    {
                        Thread trdx = (Thread)(de.Value);
                        if (trdx != null)
                        {
                            if (trdx.IsAlive)
                            {
                                trdx.Priority = ThreadPriority.Lowest;
                            }
                            TBestop = true;
                        }
                    }
                    Thread_HT["加载自选数据"] = new Hashtable();
                }
                //线程存在才执行
                if (Thread_HT.ContainsKey("加载二级分类限定数据"))
                {
                    foreach (DictionaryEntry de in (Hashtable)(Thread_HT["加载二级分类限定数据"]))
                    {
                        Thread trdx = (Thread)(de.Value);
                        if (trdx != null)
                        {
                            if (trdx.IsAlive)
                            {
                                trdx.Priority = ThreadPriority.Lowest;
                            }
                            TBestop = true;
                        }
                    }
                    Thread_HT["加载二级分类限定数据"] = new Hashtable();
                }
                //线程存在才执行
                if (Thread_HT.ContainsKey("加载成交详情"))
                {
                    foreach (DictionaryEntry de in (Hashtable)(Thread_HT["加载成交详情"]))
                    {
                        Thread trdx = (Thread)(de.Value);
                        if (trdx != null)
                        {
                            if (trdx.IsAlive)
                            {
                                trdx.Priority = ThreadPriority.Lowest;
                            }
                            TBestop = true;
                        }
                    }
                    Thread_HT["加载成交详情"] = new Hashtable();
                }


            }
            catch(Exception ex) {
                TBestop = true;
                Support.StringOP.WriteLog("停止线程错误：" + ex.ToString());
            
            }
        }

        //关闭分类层
        private void label_panel_fenlei_close_Click(object sender, EventArgs e)
        {
            panel_fenlei.Visible = false;
        }

        ///<summary> 
        ///在winform中查找控件 
        ///</summary> 
        ///<param >父控件</param> 
        ///<param >控件名称</param> 
        ///<returns></returns> 
        private System.Windows.Forms.Control findControl(System.Windows.Forms.Control control, string controlName)
        {
            Control c1;
            foreach (Control c in control.Controls)
            {
                if (c.Name == controlName)
                {
                    return c;
                }
                else if (c.Controls.Count > 0)
                {
                    c1 = findControl(c, controlName);
                    if (c1 != null)
                    {
                        return c1;
                    }
                }
            }
            return null;
        }

        //焦点离开时，关闭详细分类
        private void panel_fenlei_Leave(object sender, EventArgs e)
        {
            panel_fenlei.Visible = false;
        }



        #endregion

        #region 检查更新相关

        /// <summary>
        /// 检查是否存在更新
        /// </summary>
        public void checkAutoUpdate()
        {

            //实例化委托,并参数传递给线程执行类开始执行线程
            FormUp FU = new FormUp();
            Hashtable InPutHT = new Hashtable();
            InPutHT["更新窗体实例"] = FU;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_AutoUpDate);
            DataControl.RunThreadClassUp RTCU = new DataControl.RunThreadClassUp(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCU.BeginRun));
            trd.Name = "检查更新";
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_AutoUpDate(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_AutoUpDate_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_AutoUpDate_Invoke(Hashtable OutPutHT)
        {
            
            string cstr = OutPutHT["检查更新结果"].ToString();
            FormUp FU = (FormUp)OutPutHT["更新窗体实例"];
            if (cstr == "发现更新")
            {
                FU.ShowDialog();
            }
            else if (cstr == "无更新")
            {


                try
                {
                    //启动线程，得到需要提前获取的一些数据或参数
                    BeginGetPublisDsData();
                    //加载底部分类(这是线程)
                    initFL();
                    //加载底部统计分析数据(这是线程)
                    BeginInitTJFX();
                    //加载交易数据(这是线程)
                    TestLoadList();
                }
                catch (Exception ex)
                {
                    Support.StringOP.WriteLog("提前加载大盘数据出错：" + ex.ToString());
                    //ExitApp();
                }

                
            }
            else
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("无法获取更新信息，请检查网络连接。");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "版本更新提示", Almsg3);
                FRSE3.ShowDialog();
                ExitApp();
            }

        }

        #endregion

        #region 加载底部统计分析相关


        /// <summary>
        /// 开启底部统计分析的线程
        /// </summary>
        public void BeginInitTJFX()
        {
            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TJFX);
            DataControl.RunThreadClassTJFX RTCUTJFX = new DataControl.RunThreadClassTJFX(null, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCUTJFX.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_TJFX(Hashtable OutPutHT)
        {
            try
            {
             
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_TJFX_Invoke), new Hashtable[] { OutPutHT });
              
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_TJFX_Invoke(Hashtable OutPutHT)
        {
            tableLayoutPanel2.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel2, true, null);
            tableLayoutPanel3.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel3, true, null);
            tableLayoutPanel4.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel4, true, null);

   
            string[] show1 = (string[])OutPutHT["统计数据1"];
            string[] show2 = (string[])OutPutHT["统计数据2"];
            string[] show3 = (string[])OutPutHT["统计数据3"];
            InitTJ(tableLayoutPanel2, show1);
            InitTJ(tableLayoutPanel3, show2);
            InitTJ(tableLayoutPanel4, show3);
 


        }



        /// <summary>
        /// 根据指定字符串，在指定布局控件中，加载底部统计分析数据
        /// </summary>
        /// <param name="tlp"></param>
        /// <param name="dataStrArr"></param>
        private void InitTJ(TableLayoutPanel tlp, string[] dataStrArr)
        {
            //设置数据量和样式
            tlp.Controls.Clear();
            tlp.RowCount = 1;
            tlp.ColumnCount = dataStrArr.Length;
            //tlp.Refresh();
            for (int i = 0; i < tlp.ColumnStyles.Count; i++)
            {
                tlp.ColumnStyles[i].SizeType = SizeType.AutoSize;
            }

            //设置分辨率处理参数
            int ys_int = 20;//间距
            if (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width < 1200)
            {
                ys_int = 0;
            }

            //添加数据
            for (int i = 0; i < dataStrArr.Length; i++)
            {
                Label newL_title = GetNewLabel(tlp.Name + "_SPYHBlabel_title" + i, Color.Yellow, dataStrArr[i]);
                tlp.Controls.Add(newL_title, i, 0);
                i++;
                Label newL_number = GetNewLabel(tlp.Name + "_SPYHBlabel_number" + i, Color.Aqua, dataStrArr[i]);
                newL_number.Padding = new System.Windows.Forms.Padding(0, 0, ys_int, 0);
                tlp.Controls.Add(newL_number, i, 0);
 
            }

        }

        /// <summary>
        /// 根据指定名称、颜色、显示值，获得一个新的Label
        /// </summary>
        /// <param name="name">空间名称</param>
        /// <param name="C">颜色</param>
        /// <param name="str">显示值</param>
        /// <returns></returns>
        private Label GetNewLabel(string name,Color C,string str)
        {
            Label newL = new Label();
            newL.AutoSize = true;
            newL.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            newL.ForeColor = C;
            newL.Margin = new System.Windows.Forms.Padding(0);
            newL.Name = name;
            newL.Text = str;
            return newL;
        }

        #endregion



        private

        Image landian;
        public FormMainPublic()
        {

            landian = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\landian.gif");
 
            Thread_HT["键盘精灵查询"] = new Hashtable();
            Thread_HT["加载默认数据"] = new Hashtable();
            Thread_HT["加载自选数据"] = new Hashtable();
            Thread_HT["加载二级分类限定数据"] = new Hashtable();
            Thread_HT["加载成交详情"] = new Hashtable();

            InitializeComponent();
            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;

            //初始化界面
            initwindows();

            //开启键盘精灵
            initKeyHost();

            //加载弹出网页
            initTC();
            //竟标信息高亮
            gaoliang(L_jxpx);

            #region  自定义一个Panel，用于“快速查找”标签的提示。
            panel = new Panel();
            this.Controls.Add(panel);
            panel.BringToFront();
            panel.Width = 281;
            panel.Height = 50;
            panel.BackColor = Color.Black;
            panel.ForeColor = Color.Yellow;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(lbkscz.Location.X, lbkscz.Location.Y + 20);
            panel.Paint += delegate(object sender1, PaintEventArgs e1)
            {
                string str1 = "直接在大盘界面使用键盘输入商品编号、拼音全拼、";
                string str2 = "拼音首字母或汉字，均可快速查找。";
                Point startPoint = new Point(5, 5);

                // Set TextFormatFlags to no padding so strings are drawn together.
                TextFormatFlags flags = TextFormatFlags.NoPadding;

                // Declare a proposed size with dimensions set to the maximum integer value.
                Size proposedSize = new Size(int.MaxValue, int.MaxValue);

                Font myfont = new Font("宋体", 9.0F);

                float h = panel.Height;

                PanelDrawing(str1, panel.Width, myfont, e1.Graphics, proposedSize, flags, ref startPoint, Color.Yellow, out h);
                Point secendPoint = new Point(5, startPoint.Y + 20);
                PanelDrawing(str2, panel.Width, myfont, e1.Graphics, proposedSize, flags, ref secendPoint, Color.Yellow, out h);
            };
            panel.Width += 5;
            panel.Visible = false;
            #endregion  
            
        }
   

        /// <summary>
        /// 初始化界面的各项显示
        /// </summary>
        private void initwindows()
        {

            //把分类菜单弹窗搞到顶层
            panel_fenlei.BringToFront();

            //处理头部和左侧图片
            PB_zhzx.BorderStyle = BorderStyle.None;
            PB_zhzx.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\zhzx.png");

            //初始化默认主大盘的列和样式
            init_dataGridViewMain_Columns_default();
            //保存下默认大盘的列和样式
            al_dgv = new ArrayList();
            for (int i = 0; i < dataGridViewMain.Columns.Count; i++)
            {
                al_dgv.Add(dataGridViewMain.Columns[i].Clone() as DataGridViewColumn);
            }
        
            //鼠标滚轮
            this.dataGridViewMain.MouseWheel += new MouseEventHandler(dataGridViewMain_MouseWheel);

            //设置分类和横向滚动条自适应
            TLB_fenlei.Dock = DockStyle.Left;
            hScrollBar1.Dock = DockStyle.Fill;
            panel3.Height = 15;
  
        }

        private void dataGridViewMain_MouseWheel(object sender, MouseEventArgs e)
        {
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["行数"] = dataGridViewMain.Rows.Count;
            InPutHT["列数"] = dataGridViewMain.ColumnCount;

           // L_400.Text = e.Delta.ToString();
            if (e.Delta > 0)
            {
                InPutHT["来源"] = "滚轮向上";
                Program.nowIndex = Program.nowIndex - 1;
            }
            else
            {
                InPutHT["来源"] = "滚轮向下";
                Program.nowIndex = Program.nowIndex + 1;
            }

            if (Program.nowIndex < 0)
            {
                InPutHT["来源"] = "不滚动了";
                Program.nowIndex = 0;
            }

            if (Program.nowIndex > vScrollBar1.Maximum)
            {
                InPutHT["来源"] = "不滚动了";
                Program.nowIndex = vScrollBar1.Maximum;
            }
            vScrollBar1.Value = Program.nowIndex;

  
            //实例化委托,并参数传递给线程执行类开始执行线程
            //delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            //DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            //Thread trd = new Thread(new ThreadStart(RCT.GunDong));
            //trd.IsBackground = true;
            //trd.Start();


            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            RCT.GunDong();



        }
 
        private void FormMainPublic_Load_1(object sender, EventArgs e)
        {

            //
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            notifyIconMain.Icon = ic;
            //====================================================
            //设置二级分类中间竖线
            Label labelSX = new Label();
            labelSX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            labelSX.Name = "labelSX";
            labelSX.Location = new System.Drawing.Point(160, 8);
            labelSX.Size = new System.Drawing.Size(1, 160);
            labelSX.TabIndex = 2;
            panel_fenlei.Controls.Add(labelSX);
            labelSX.BringToFront();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            SHC(this);            

 
            

            ad.Hide();
            FL = new FormLogin(this, ad, "首次打开");
            FL.ShowInTaskbar = true;
            FL.Show();



        }

        //双缓冲
        private void SHC(Control ct)
        {
            foreach (Control ctin in ct.Controls)
            {
                ctin.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(ctin, true, null);
                SHC(ctin);
            }

        }

        /// <summary>
        /// 使用账号密码登陆成功后，处理一些东西
        /// </summary>
        public void userloginOK()
        {
            //登陆成功后，重新将窗体最大化
            this.ShowInTaskbar = true;
            SendMessage(this.Handle, WM_SYSCOMMAND, (int)SC_MAXIMIZE, IntPtr.Zero);

            //处理登陆、注册按钮
            L_emailshow.Text = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["证券资金账号"].ToString();
            //L_emailshow.Text = "已登录";
            //调整账号显示的位置
            L_emailshow.Location = new Point(label4.Location.X - L_emailshow.Width, L_emailshow.Location.Y);

            //初始化自选商品本地临时表
            InitZXSPtemp();

            //开启提醒检查(这是线程)，提醒比较特殊，只有登陆成功后才开启这个无限循环的线程
            beginTrayMsg();

            dataGridViewMain.Focus();
            if (ad != null && !ad.IsDisposed && ad.IsHandleCreated)
            {
                ad.Activate();
            }

            timer1.Enabled = true;

 
        }



        /// <summary>
        /// 将菜单和按钮关联并显示
        /// </summary>
        /// <param name="l">按钮</param>
        /// <param name="cms">菜单</param>
        private void MenuLikeLabel(Label l,ContextMenuStrip cms)
        {

            if ((this.Height - l.Location.Y - l.Height) < cms.Height)
            {
                Point pt = new Point(0, -cms.Height);
                cms.Show(l, pt);
            }
            else
            {
                Point pt = new Point(0, l.Height);
                cms.Show(l, pt);
            }

        }

        private void L_zxzx_Click(object sender, EventArgs e)
        {
            //MenuLikeLabel(L_zxzx, CMS_zxzx);
            //panelTC.Visible = true;

            label3.BackColor = Color.Transparent;
            if (ad != null && !ad.IsDisposed && ad.IsHandleCreated)
            {
                if(ad.WindowState == FormWindowState.Minimized)
                {
                    ad.WindowState = FormWindowState.Normal;
                }//设置窗体状态
                ad.Activate();
            }
            else
            {
                ad = null;
                string url = "http://www.fm8844.com/forever/jypttanchuang/";
                ad = new ADNew(url);
                ad.StartPosition = FormStartPosition.CenterScreen;
                ad.ShowInTaskbar = true;
                ad.Show();
            }
        }

        private void L_bzzx_Click(object sender, EventArgs e)
        {
            ContextMenuStrip CMS_bzzx = new ContextMenuStrip();
            ToolStripMenuItem menu = new ToolStripMenuItem();
            ToolStripMenuItem menu2 = new ToolStripMenuItem();


            label3.BackColor = Color.Transparent;

            menu.Text = "新手入门";
            #region//新手入门
            //menu2.Text = "经纪人问答手册";
            //menu2.Tag="CZSC/JJRWDSC.htm";
            //menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            //menu.DropDownItems.Add(menu2);

            //menu2 = new ToolStripMenuItem();
            //menu2.Text = "买家问答手册";
            //menu2.Tag = "CZSC/BuyWDSX.htm";
            //menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            //menu.DropDownItems.Add(menu2);

            //menu2 = new ToolStripMenuItem();
            //menu2.Text = "卖家问答手册";
            //menu2.Tag = "CZSC/SelWDSC.htm";
            //menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            //menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "交易方操作手册";
            menu2.Tag = "CZSC/JYFCZSC.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "经纪人操作手册";
            menu2.Tag = "CZSC/JJRCZSC.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "开户流程图";
            menu2.Tag = "CZSC/KHLCTFather.html";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);
            #endregion
            CMS_bzzx.Items.Add(menu);


            menu = new ToolStripMenuItem();
            menu.Text = "平台规则";
            #region//平台规则
            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台《电子购货合同》管理规定";
            menu2.Tag = "GLGD/DAGHHTGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台督察审计管理规定";
            menu2.Tag = "GLGD/DCSJGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台交易规则";
            menu2.Tag = "GLGD/JYPTJYGZ.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台交易账户管理规定";
            menu2.Tag = "GLGD/JYPTJYZHGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台交易方管理规定";
            menu2.Tag = "GLGD/JYPTMMJGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台违规与处罚管理规定";
            menu2.Tag = "GLGD/JYPTCFYWGGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);

            menu.DropDownItems.Add(menu2);
            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台信息发布管理规定";
            menu2.Tag = "GLGD/JYPTXXFBGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台货款收付管理规定";
            menu2.Tag = "GLGD/HKSFGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台交易安全管理规定";
            menu2.Tag = "GLGD/YIAQGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台交易规则管理规定";
            menu2.Tag = "GLGD/JYGZGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台财务管理规定";
            menu2.Tag = "GLGD/CWGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台费用收取管理规定";
            menu2.Tag = "GLGD/FYSQGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台经纪人管理规定";
            menu2.Tag = "GLGD/JJRGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台清盘管理规定";
            menu2.Tag = "GLGD/QPGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台业务资料管理规定";
            menu2.Tag = "GLGD/YWZLGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台交易方资金管理规定";
            menu2.Tag = "GLGD/JYZJGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台交易商品管理规定";
            menu2.Tag = "GLGD/JYPTSPGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台数据备份管理规定";
            menu2.Tag = "GLGD/SJBFGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "富美集团中国商品批发交易平台用户信用等级评价管理规定";
            menu2.Tag = "GLGD/YHXYDJPJGLGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);
            #endregion
            CMS_bzzx.Items.Add(menu);


            menu = new ToolStripMenuItem();
            menu.Text = "相关法律法规";
            #region//相关法律法规
            menu2 = new ToolStripMenuItem();
            menu2.Text = "《合同法》";
            menu2.Tag = "FLFG/HTF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《产品质量法》";
            menu2.Tag = "FLFG/CPZLF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《消费者权益保护法》";
            menu2.Tag = "FLFG/XFZQYBZF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《招标投标法》";
            menu2.Tag = "FLFG/ZBTBF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《招标投标法实施条例》";
            menu2.Tag = "FLFG/ZBTBFSSTL.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《电子签名法》";
            menu2.Tag = "FLFG/DZQMF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《计算机软件保护条例》";
            menu2.Tag = "FLFG/RJBHTL.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《互联网信息服务管理办法》";
            menu2.Tag = "FLFG/HLWXXFFGLBF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《信息安全等级保护管理办法》";
            menu2.Tag = "FLFG/XXAQDJGLBF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《关于网上交易的指导意见（暂行）》";
            menu2.Tag = "FLFG/WSJJZDYJ.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《网络商品交易及有关服务行为管理暂行办法》";
            menu2.Tag = "FLFG/WLSPJJJYGFWXWGLBF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);
            //下次开始处
            menu2 = new ToolStripMenuItem();
            menu2.Text = "《大宗商品电子交易规范》";
            menu2.Tag = "FLFG/DZSPDZJYGF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《电子商务模式规范》";
            menu2.Tag = "FLFG/DZSWMSGF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《网络交易服务规范》";
            menu2.Tag = "FLFG/WLJJFWGF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《第三方电子商务交易平台服务规范》";
            menu2.Tag = "FLFG/DSDJYPTFWGF.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《国务院关于清理整顿各类交易场所切实防范金融风险的决定》";
            menu2.Tag = "FLFG/FFJRFXJD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《国务院办公厅关于清理整顿各类交易场所的实施意见》";
            menu2.Tag = "FLFG/QLZDSSYJ.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《山东省人民政府关于贯彻国发【2011】38号文件做好清理整顿各类交易场所工作的意见》";
            menu2.Tag = "FLFG/38HWYJ.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);

            menu2 = new ToolStripMenuItem();
            menu2.Text = "《山东省人民政府办公厅关于贯彻国办发【2012】37号文件进一步做好清理整顿各类交易场所工作的意见》";
            menu2.Tag = "FLFG/37HWYJ.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);
            menu2 = new ToolStripMenuItem();
            menu2.Text = "《商务部、中国人民银行、证券监督管理委员会令2013第3号 <商品现货市场交易特别规定（试行）>》";
            menu2.Tag = "FLFG/SPXHSCJYTBGD.htm";
            menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            menu.DropDownItems.Add(menu2);
            //---

            //menu2 = new ToolStripMenuItem();
            //menu2.Text = "《中华人民共和国农业法》";
            //menu2.Tag = "FLFG/NYF.htm";
            //menu2.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            //menu.DropDownItems.Add(menu2);

            
            #endregion
            CMS_bzzx.Items.Add(menu);
            menu = new ToolStripMenuItem();
            menu.Text = "富美集团中国商品批发交易平台章程";
            menu.Tag = "YJPTZC/JYPTZC.htm";
            menu.Click += new System.EventHandler(this.关于交易平台ToolStripMenuItem_Click);
            CMS_bzzx.Items.Add(menu);


            MenuLikeLabel(L_bzzx, CMS_bzzx);
        }

        private void L_zxff_Click(object sender, EventArgs e)
        {
            ;
        }

        //自定义滚动条滚动
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //if (e.Type == ScrollEventType.EndScroll )
            //{
                int aaa = e.NewValue;
                if (aaa < hScrollBar1.Minimum)
                {
                    aaa = hScrollBar1.Minimum;
                    hScrollBar1.Value = aaa;
                }
                if (aaa > hScrollBar1.Maximum)
                {
                    aaa = hScrollBar1.Maximum;
                    hScrollBar1.Value = aaa;
                }

                //MessageBox.Show(e.ScrollOrientation.ToString());
                dataGridViewMain.HorizontalScrollingOffset = aaa;
                //if (dataGridViewMain.HorizontalScrollingOffset != aaa)
                //{
                //    int hide_width = dataGridViewMain.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - this.Width + 35;
                //    int sss = this.Width - dataGridViewMain.Width;
                //    MessageBox.Show(dataGridViewMain.HorizontalScrollingOffset.ToString() + "|" + hide_width.ToString() + "|" + (dataGridViewMain.HorizontalScrollingOffset - hide_width).ToString() + "|" + sss.ToString());

                //}
                
           // }
            
        }

        //鼠标点击单元格时，显示右键菜单
        private void dataGridViewMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex >= 0)
                    {
                        dataGridViewMain.ClearSelection();
                        dataGridViewMain.Rows[e.RowIndex].Selected = true;

                        //弹出操作菜单
                        CMS_datagrid.Show(MousePosition.X, MousePosition.Y);
                        if (timer2.Enabled)//如果计时器正在运行
                        {
                            timer2.Stop();
                        }
                        timer2.Start();
                    }
                }
                if (e.Button == MouseButtons.Left)
                {
                    if (e.RowIndex >= 0)
                    {
                        dataGridViewMain.ClearSelection();
                        dataGridViewMain.Rows[e.RowIndex].Selected = true;

                        if (timer2.Enabled)//如果计时器正在运行
                        {
                            timer2.Stop();
                        }
                        timer2.Start();

                        if (dataGridViewMain.Columns[e.ColumnIndex].Name == "LD" && dataGridViewMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() != "")
                        {
                            验收货经验交流toolStripMenuItem_Click(null,null);
                        }
                    }
                }
            }
            catch(Exception  ex){}
        }

        //进入我的账户
        private void PB_zhzx_Click(object sender, EventArgs e)
        {

            ad.Close();//关闭咨询弹窗
            //timer1.Stop();
            if (PublicDS.PublisDsUser == null)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请登录后进行操作。");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "登录提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "")
            {

                //未开通结算账户
                Point pt = new Point(0, PB_zhzx.Height);
                CMS_login.Show(PB_zhzx, pt);

               

                return;
            }
           
            //开线程，验证对应经纪人是否被冻结或休眠，并给出提示
            SRT_checkJJRzt_Run();


            
        }


        //开启一个线程,检查所属经纪人状态
        private void SRT_checkJJRzt_Run()
        {
            PB_zhzx.Enabled = false;
            pb_htload.Visible = true;
            OpenThreadIndex OT = new OpenThreadIndex(null, new delegateForThread(SRT_checkJJRzt));
            Thread trd = new Thread(new ThreadStart(OT.BeginRun_JJR));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_checkJJRzt(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_checkJJRzt_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_checkJJRzt_Invoke(Hashtable OutPutHT)
        {
            PB_zhzx.Enabled = true;
            pb_htload.Visible = false;
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];
            DataSet dsreturn_user = dsreturn.Copy();
            dsreturn_user.Tables.Remove("返回值单条");
            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            string zhxm = dsreturn.Tables["返回值单条"].Rows[0]["附件信息5"].ToString().Trim();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //关联经纪人被休眠或被冻结的提示
                    if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
                    {
                        
                        ArrayList Almsg3 = new ArrayList();
                        Almsg3.Add("");

                        if (zhxm == "账户休眠" && PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["经纪人审核状态"].ToString()=="审核中")
                        {
                            Almsg3.Add("您的经纪人账户已休眠，开户申请无法审核，请您进入“账户维护”重新关联经纪人！");
                            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                            FRSE3.ShowDialog();

                        }
                        else
                        {
                            Almsg3.Add(showstr);
                            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                            FRSE3.ShowDialog();
 
                        }
                        
                    }

                    break;
                default:
                    break;
            }

            //重新为基础数据负值
            PublicDS.PublisDsUser = dsreturn_user;
            ////重新给审核状态负值，避免重登陆才能看到审核成功的情况。
            //PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["是否已被经纪人审核通过"] = dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();
            //PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["是否已被分公司审核通过"] = dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();
            //PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["交易方名称"] = dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"].ToString();


            //提示用户未开通结算账户
            bool autogo_zhwh = false; //是否自动进入帐户维护
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "经纪人交易账户")
            {
                if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["是否已被分公司审核通过"].ToString() != "是")
                {
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");

                    //审核中，审核通过，驳回  

                    if (PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["分公司审核状态"].ToString() == "驳回")
                    {
                        Almsg4.Add("您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！");
                        autogo_zhwh = true;
                    }
                    else
                    {
                        
                        Almsg4.Add("您的开户申请正在审核中，请耐心等待！");
                    }


                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                }
                else
                {//第三方存管状态
                    if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["第三方存管状态"].ToString().Trim() != "开通")
                    {
                        ArrayList Almsg5 = new ArrayList();
                        Almsg5.Add("");
                        Almsg5.Add("您当前未绑定第三方存管银行，请到相关银行开通！");
                        FormAlertMessage FRSE5 = new FormAlertMessage("仅确定", "其他", "", Almsg5);
                        FRSE5.ShowDialog();
 
                    }
                }
            }
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "买家卖家交易账户")
            {
                if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["是否已被经纪人审核通过"].ToString() != "是" || PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["是否已被分公司审核通过"].ToString() != "是")
                {
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    if (PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["经纪人审核状态"].ToString() == "驳回" || PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["分公司审核状态"].ToString() == "驳回")
                    {
                        Almsg4.Add("您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请。");
                        autogo_zhwh = true;
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                        FRSE4.ShowDialog();
                    }
                    else
                    {
                        if (zhxm != "账户休眠")
                        {
                            Almsg4.Add("您的开户申请正在审核中，请耐心等待！");//
                            FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                            FRSE4.ShowDialog();
                        }

                    }
                    
                }
                else
                {//第三方存管状态
                    if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["第三方存管状态"].ToString().Trim() != "开通")
                    {
                        ArrayList Almsg5 = new ArrayList();
                        Almsg5.Add("");
                        Almsg5.Add("您当前未绑定第三方存管银行，请到相关银行开通！");
                        FormAlertMessage FRSE5 = new FormAlertMessage("仅确定", "其他", "", Almsg5);
                        FRSE5.ShowDialog();

                    }
                }
            }


        

            //打开后台操作窗体
            if (FC == null || FC.IsDisposed == true)
            {
                FC = new Center2013();
                //设置窗体的特殊颜色
                FC.TitleYS = new int[] { 0, 0, -50 };
                FC.WindowState = FormWindowState.Normal;
                FC.Show();

                if (autogo_zhwh)
                {
                    //强制重定位到某个A级菜单上
                    FC.auto_ClickLabel("账户维护");
                }

            }
            else
            {
                //重新执行load事件
                FC.LoadRun();

                FC.WindowState = FormWindowState.Normal;
                FC.Show();

                if (autogo_zhwh)
                {
                    //强制重定位到某个A级菜单上
                    FC.auto_ClickLabel("账户维护");
                }

                FC.Activate();

                
            }
            

        }

        public void SPopenDialog(FormKTJYZH FK ,bool chongzhi, Hashtable hts)
        {
            if (chongzhi)
            {
                FK = new FormKTJYZH(hts);
                FK.ShowDialog();
                SPopenDialog(FK, FK.chongzhi, hts);
            }
        }

        private void 开通交易账户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hashtable hts = new Hashtable();
            hts["dlyx"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            hts["dlmm"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录密码"].ToString();
            hts["yhm"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["用户名"].ToString();

            Program.fms = new FormKTJYZH(hts);
            SPopenDialog(Program.fms, true, hts);

            

            //if (Program.fms == null || Program.fms.IsDisposed)
            //{
            //    Program.fms = new FormKTJYZH(hts);
            //    Program.fms.ShowDialog();
            //}
            //else
            //{
            //    Program.fms.ShowDialog();
            //    Program.fms.Activate();
            //}
        }

        private void 了解交易账户功能ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArrayList Almsg4 = new ArrayList();
            Almsg4.Add("");
            Almsg4.Add("您尚未提交开通交易账户申请，请及时提交！");// 
            FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
            FRSE4.ShowDialog();

            if (FC == null)
            {
                FC = new Center2013();
                //设置窗体的特殊颜色
                FC.TitleYS = new int[] { 0, 0, -50 };
                FC.Show();
            }
            else
            {
                FC.Show();
                FC.Activate();
            }
        }



        //调试专用
        private void dataGridViewMain_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ////调整并保存合适的列宽
            //string str = "";
            //for (int i = 0; i < dataGridViewMain.Columns.Count; i++)
            //{
            //    str = str + dataGridViewMain.Columns[i].Width + ",";
            //}
            //textBox1.Text = str;




            //这不是调试代码，是正常代码
            ReSethScrollBar1();
        }

        //点击了账户显示，暂无功能
        private void L_emailshow_Click(object sender, EventArgs e)
        {



            /*
            //未登陆时，打开新的登陆口
            if (PublicDS.PublisDsUser == null || PublicDS.PublisDsUser.Tables["返回值单条"].Rows[0]["执行结果"].ToString() != "ok")
            {
                FL = new FormLogin(this, "重新打开");
                FL.Show();
            }
            */
        }

        //退出程序
        private void label4_Click(object sender, EventArgs e)
        {
            MenuLikeLabel(label4, CMSexit);


            
        }

        //竞标排序
        private void L_jxpx_Click(object sender, EventArgs e)
        {
            //必须先减
            this.dataGridViewMain.CellMouseClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);

            this.dataGridViewMain.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);

            //显示进度
            PBload.Visible = true;
            //停止线程
            StopmainlistTHread();
            //清理
            dataGridViewMain.Rows.Clear();

  
            gaoliang(L_jxpx);

            //开启新线程，加载默认数据
            TestLoadList();


            //MenuLikeLabel(L_jxpx, CMSdapan);
        }

        //自选商品
        private void L_dbfx_Click(object sender, EventArgs e)
        {
            //必须先减
            this.dataGridViewMain.CellMouseClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);

            this.dataGridViewMain.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
            if (DLYX == "")
            { return; }
            //显示进度
            PBload.Visible = true;
            //停止线程
            StopmainlistTHread();
            //清理
            dataGridViewMain.Rows.Clear();
            //开启新线程，加载自选数据

            gaoliang(L_zixuan);
            TestLoadListZXSP();
        }

        //信誉记录
        private void L_xyjl_Click(object sender, EventArgs e)
        {

        }

        //半年竞标中
        private void PB_bannian_Click(object sender, EventArgs e)
        {

        }

        //一年竞标中
        private void PB_yinian_Click(object sender, EventArgs e)
        {

        }

        //定量定价竞标中
        private void PB_dingliang_Click(object sender, EventArgs e)
        {

        }

        //自选商品
        private void PB_zixuan_Click(object sender, EventArgs e)
        {

        }

        private void 加入自选商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //当前操作的商品编号
            string SPBH = dataGridViewMain.SelectedRows[0].Cells[1].Value.ToString();

            if (SPBH.Trim() == "")
            {
                return;
            }

            SetZXSP(SPBH,"加入");
            
        }

        private void 删除自选商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //当前操作的商品编号
            string SPBH = dataGridViewMain.SelectedRows[0].Cells[1].Value.ToString();

            if (SPBH.Trim() == "")
            {
                return;
            }

            SetZXSP(SPBH, "删除");

 
        }

        private void 查看详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {

         

            if (dataGridViewMain.Columns.Contains("清盘类型"))
            {
                return;
            }
            //当前操作的商品编号 htzq
            string SPBH = dataGridViewMain.SelectedRows[0].Cells[1].Value.ToString();
            string HTQX = dataGridViewMain.SelectedRows[0].Cells["htzq"].Value.ToString();
    

            #region//商品详情页面，如果同种商品的详情已经打开，则让他获取焦点，否则打开新页面

         
            Form spxqll=null;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name.ToString() == "SPXQ2014")
                {                
                    spxqll = f;
                    break;
                    
                }
            }

            if (spxqll != null && !spxqll.IsDisposed && spxqll.IsHandleCreated)
            {
                if (spxqll.WindowState == FormWindowState.Minimized)
                {
                    spxqll.WindowState = FormWindowState.Maximized;
                }//设置窗体状态
                spxqll.Activate();
                ((SPXQ2014)spxqll).BeginShowDB(SPBH, HTQX,"");
                 
            }
            else
            {
                SPXQ2014 sp2014 = new SPXQ2014();
                sp2014.Show();
                sp2014.BeginShowDB(SPBH, HTQX,"首次");
            }
            
            #endregion

        }

        private void 验收货经验交流toolStripMenuItem_Click(object sender, EventArgs e)
        {
            //当前操作的商品编号 htzq
            string SPBH = dataGridViewMain.SelectedRows[0].Cells[1].Value.ToString();
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();

            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/mywork/JYPT/JYPT_SHJYJL_YSBZ.aspx?spbh=" + SPBH + "&dlyx=" + DLYX;






            #region 验收货经验交流页面，如果同种页面已经打开，则让他获取焦点，否则打开新页面
            Form spxqll = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name.ToString() == "FormPrdJYJL")
                {
                    spxqll = f;
                    break;

                }
                else
                {
                }
            }

            if (spxqll != null && !spxqll.IsDisposed && spxqll.IsHandleCreated)
            {
                if (spxqll.WindowState == FormWindowState.Minimized)
                {
                    spxqll.WindowState = FormWindowState.Normal;
                }//设置窗体状态
                spxqll.Activate();
                ((FormPrdJYJL)spxqll).reLoadUrl(url);
            }
            else
            {
                FormPrdJYJL prdJYJL = new FormPrdJYJL(SPBH, url);

                prdJYJL.StartPosition = FormStartPosition.CenterScreen;
                prdJYJL.ShowInTaskbar = true;
                prdJYJL.Show();
            }
            #endregion



            
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            { return; }
            string SPBH = listView1.SelectedItems[0].SubItems[0].Text;//
            string HTQX = listView1.SelectedItems[0].SubItems[2].Text;//
            //FormSPinfo SPinfo = new FormSPinfo(SPBH, HTQX);
            //SPinfo.Show();
           
            #region//商品详情页面，如果同种商品的详情已经打开，则让他获取焦点，否则打开新页面
            Form spxqll = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name.ToString() == "SPXQ2014")
                {
                    spxqll = f;
                    break;

                }
            }

            if (spxqll != null && !spxqll.IsDisposed && spxqll.IsHandleCreated)
            {
                if (spxqll.WindowState == FormWindowState.Minimized)
                {
                    spxqll.WindowState = FormWindowState.Normal;
                }//设置窗体状态
                spxqll.Activate();
                ((SPXQ2014)spxqll).BeginShowDB(SPBH, HTQX, "");

            }
            else
            {
                SPXQ2014 sp2014 = new SPXQ2014();
                sp2014.Show();
                sp2014.BeginShowDB(SPBH, HTQX, "首次");
            }
            #endregion

        }

        //定标统计
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void 退出登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArrayList Almsg4 = new ArrayList();
            Almsg4.Add("");
            Almsg4.Add("您确定要退出吗？");// 
            FormAlertMessage FRSE4 = new FormAlertMessage("确定取消", "问号", "", Almsg4);
            if (FRSE4.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                ExitApp();
            }
            else
            {
                ;

            }
        }

        private void 重新登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArrayList Almsg4 = new ArrayList();
            Almsg4.Add("");
            Almsg4.Add("您确定要重新登录吗？");// 
            FormAlertMessage FRSE4 = new FormAlertMessage("确定取消", "问号", "", Almsg4);
            if (FRSE4.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                this.Hide();
                //暂时抄一段复杂处理，看看管不管用。简单处理在xp上存在bug
                Process p = Process.Start(Application.ExecutablePath);
                //讓 Process 元件等候相關的處理序進入閒置狀態。 
                p.WaitForInputIdle();
                //設定要等待相關的處理序結束的時間，這邊設定 7 秒。 
                p.WaitForExit(1);
                //若應用程式在指定時間內關閉，則 value.HasExited 為 true 。
                //若是等到指定時間到了都還沒有關閉程式，此時 value.HasExited 為 false，則進入判斷式
                if (!p.HasExited)
                {
                    //測試處理序是否還有回應
                    if (p.Responding)
                    {
                        //關閉使用者介面的處理序
                        ExitApp();
                    }
                    else
                    {
                        //立即停止相關處理序。意即，處理序沒回應，強制關閉
                        ExitApp();
                    }
                }
                if (p != null)
                {
                    p.Close();
                    p.Dispose();
                    p = null;
                }







                //this.Hide();
                //Thread.Sleep(500);
                ExitApp();
            }
            else
            {
                ;

            }
        }


        private void 关于交易平台ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (sender) as ToolStripMenuItem;
            string title = menu.Text;
            string url = menu.Tag.ToString();
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/" + url;
            XSRM xs = new XSRM(title, htcs);
            xs.Width = 980;
            xs.Height = 630;
            xs.ShowDialog();                    
            
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
          
            if (e.Type == ScrollEventType.EndScroll)
            {
               // foreach (DataGridViewRow dgvr in dataGridViewMain.Rows)
                if (dataGridViewMain.SelectedRows.Count > 0)
                {
                    dataGridViewMain.SelectedRows[0].Selected = false;
                }
                //填充传入参数哈希表
                Hashtable InPutHT = new Hashtable();
                InPutHT["行数"] = dataGridViewMain.Rows.Count;
                InPutHT["列数"] = dataGridViewMain.ColumnCount;
                Program.nowIndex = e.NewValue;
                //实例化委托,并参数传递给线程执行类开始执行线程
                delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
                DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
                Thread trd = new Thread(new ThreadStart(RCT.GunDong));
                trd.IsBackground = true;
                trd.Start();

         
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
 
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name.ToString() == "Center2013" || f.Name.ToString() == "FormKTJYZH")
                {                   
                    timer1.Stop();
                    return;
                }
                else
                {        
                }
            }

            if (ad != null && !ad.IsDisposed && ad.IsHandleCreated)
            {
                //ad.Activate();
            }
            else
            {
                ad = null;
                string url = "http://www.fm8844.com/forever/jypttanchuang/";
                ad = new ADNew(url);
                ad.StartPosition = FormStartPosition.CenterScreen;
                ad.ShowInTaskbar = true;
                ad.Show();
                ad.Activate();
            }                        
            
        }
        
        private void label3_Click_1(object sender, EventArgs e)
        {
            //必须先减
            this.dataGridViewMain.CellMouseClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);

            this.dataGridViewMain.CellMouseClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseClick);
            this.dataGridViewMain.CellMouseDoubleClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMain_CellMouseDoubleClick);
            //显示进度
            PBload.Visible = true;
            //停止线程
            StopmainlistTHread();
            //清理
            dataGridViewMain.Rows.Clear();
            gaoliang(label3);
        
            //开启新线程，加载成交详情
            otherlisttest();
        }
        //关闭左键菜单
        private void timer2_Tick(object sender, EventArgs e)
        {
            CMS_datagrid.Close();
            timer2.Stop();
        }

        private void L_400_Click(object sender, EventArgs e)
        {
            //SendMessage(this.Handle, WM_SYSCOMMAND, (int)SC_MAXIMIZE, IntPtr.Zero);
            //MessageBox.Show(PublicDS.PublisTimeServer.ToString());
        }

        private void dataGridViewMain_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                查看详情ToolStripMenuItem_Click(null, null);
            }
            catch (Exception ex) { }
        }

        private void FormMainPublic_FormClosing(object sender, FormClosingEventArgs e)
        {
            ArrayList Almsg4 = new ArrayList();
            Almsg4.Add("");
            Almsg4.Add("您确定要退出吗？");// 
            FormAlertMessage FRSE4 = new FormAlertMessage("确定取消", "问号", "", Almsg4);
            if (FRSE4.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = false;
                ExitApp();
            }
            else
            {
                //CloseReason aaa = e.CloseReason;
                e.Cancel = true;

            }
            
        }

        private void dataGridViewMain_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// 重设横向滚动条参数
        /// </summary>
        private void ReSethScrollBar1()
        {
            try
            {
             
                int hide_width = dataGridViewMain.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - this.Width + 35;
                if (hide_width < 0)
                {
                    hide_width = 0;
                }
                int old_Maximum = hScrollBar1.Maximum;
                int old_Value = hScrollBar1.Value;
                int old_HorizontalScrollingOffset = dataGridViewMain.HorizontalScrollingOffset;
                hScrollBar1.Value = 0;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = hide_width;
                hScrollBar1.Value = dataGridViewMain.HorizontalScrollingOffset;
            
            }
            catch (Exception ex)
            {
                string aa = ex.ToString();
            }
        }

        private void FormMainPublic_Resize(object sender, EventArgs e)
        {
            ReSethScrollBar1();
        }

        int e_ColumnIndex = -1;
        int e_RowIndex = 0;
        string tipstr = "";
        private void dataGridViewMain_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1)
                {
                    e_ColumnIndex = e.ColumnIndex;
                    e_RowIndex = e.RowIndex;

                    if (dataGridViewMain.Columns[e_ColumnIndex].Tag == null || dataGridViewMain.Columns[e_ColumnIndex].Tag.ToString().Trim() == "")
                    {
                        tipstr = dataGridViewMain.Columns[e_ColumnIndex].ToolTipText;
                        dataGridViewMain.Columns[e_ColumnIndex].Tag = tipstr;
                        dataGridViewMain.Columns[e_ColumnIndex].ToolTipText = "";
                    }
                    else
                    {
                        tipstr = dataGridViewMain.Columns[e_ColumnIndex].Tag.ToString().Trim();
                    }

                    timerCtitle.Enabled = true;

                }
            }
            catch {}
        }

        private void timerCtitle_Tick(object sender, EventArgs e)
        {
            try
            {
                if (e_ColumnIndex == -1 || e_RowIndex != -1 || timerCtitle.Enabled == false)
                {
                    return;
                }
                timerCtitle.Enabled = false;


                if (tipstr != "")
                {
                    Rectangle rect = dataGridViewMain.GetCellDisplayRectangle(e_ColumnIndex, e_RowIndex, false);
                    label_dapantou.Location = new Point(rect.Location.X, label_dapantou.Location.Y);
                    if (rect.Location.X > (this.Width - label_dapantou.Width))
                    {
                        label_dapantou.Location = new Point(this.Width - label_dapantou.Width, label_dapantou.Location.Y);
                    }
                    label_dapantou.Text = tipstr;
                    label_dapantou.Visible = true;

                    dataGridViewMain.Columns[e_ColumnIndex].HeaderCell.Style.ForeColor = Color.Yellow;
                }
            }
            catch { }
        }      

        private void dataGridViewMain_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            timerCtitle.Enabled = false;
            if (e.RowIndex == -1)
            {
                
                dataGridViewMain.Columns[e.ColumnIndex].HeaderCell.Style.ForeColor = Color.White;
                label_dapantou.Visible = false;
                label_dapantou.Text = "";
            }
        }

        //默认大盘
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }
        //有买方无卖方
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        //有卖方无买方
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        //快速查找
        private void lbkscz_MouseEnter(object sender, EventArgs e)
        {
            panel.Visible = true       ;

        }       

        private void lbkscz_MouseLeave(object sender, EventArgs e)
        {
            panel.Visible = false;
        }

        /// <summary>
        /// 用不同的颜色绘制字符串
        /// </summary>
        /// <param name="str">要绘制的字符串</param>
        /// <param name="w">panel宽度</param>
        /// <param name="font">字体</param>
        /// <param name="g">绘图图面</param>
        /// <param name="proposedSize">最大值</param>
        /// <param name="flags">关于字符串的显示和布局信息</param>
        /// <param name="startPoint">绘制的起始点</param>
        /// <param name="color">颜色</param>
        /// <param name="height">自动计算的面板宽度</param>
        protected void PanelDrawing(string str, float w, Font font, Graphics g, Size proposedSize, TextFormatFlags flags, ref Point startPoint, Color color, out float height)
        {
            float lineheight = TextRenderer.MeasureText(g, "测试", font, proposedSize, flags).Height;
            for (int i = 0; i < str.Length; i++)
            {
                Size size = TextRenderer.MeasureText(g, str[i].ToString(), font, proposedSize, flags);

                if (startPoint.X < w)
                {
                    TextRenderer.DrawText(g, str[i].ToString(), font, startPoint, color, flags);
                    startPoint.X += size.Width;
                }
                else
                {
                    startPoint.X = 10; startPoint.Y = startPoint.Y + size.Height + 10;
                    TextRenderer.DrawText(g, str[i].ToString(), font,
                    startPoint, color, flags);
                    startPoint.X += size.Width;
                }
            }
            height = startPoint.Y + lineheight + 15;
            //g.Dispose();

        }

        private void lbgjss_Click(object sender, EventArgs e)
        {
            //FormGJSS ss = new FormGJSS("高级搜索");
            //ss.ShowDialog();

            if (ss != null && !ss.IsDisposed && ss.IsHandleCreated)
            {
                if (ss.WindowState == FormWindowState.Minimized)
                {
                    ss.WindowState = FormWindowState.Normal;
                }//设置窗体状态
                ss.Activate();
            }
            else
            {
                ss = new FormGJSS("高级搜索");               
                ss.StartPosition = FormStartPosition.CenterScreen;
                ss.ShowInTaskbar = true;
                ss.Show();
            }

        }


        //高亮菜单
        private void gaoliang(Label L)
        {
            Lxhx.Height = 1;
            Lxhx.Width = L.Width;
            Lxhx.Location = new Point(L.Location.X, L.Location.Y + L.Height + 4);
      
        }

        private void CMS_datagrid_Opening(object sender, CancelEventArgs e)
        {

            if (PublicDS.PublisDsZXSP != null)
            {
                加入自选商品ToolStripMenuItem.Enabled = true;

                //当前操作的商品编号
                string SPBH = dataGridViewMain.SelectedRows[0].Cells[1].Value.ToString();

                for (int i = 0; i < PublicDS.PublisDsZXSP.Tables["自选商品临时表"].Rows.Count; i++)
                {
                    if (SPBH.Trim() == PublicDS.PublisDsZXSP.Tables["自选商品临时表"].Rows[i]["商品编号"].ToString().Trim())
                    {
                        加入自选商品ToolStripMenuItem.Enabled = false;
                        break;
                    }
                }

            }
        }
 
      

     
  










































    }



}
