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
using 客户端主程序通讯类库.客户端类库.UDP通讯;
using System.Net;
using 公用通讯协议类库.共用类库;
using 客户端主程序.DataControl;

namespace 客户端主程序
{




    public partial class FormMainPublic : BasicForm
    {
        //处理窗口淡出淡入，这个只用于处理提醒窗口
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 我的账户管理中心，只能开一个
        /// </summary>
        FormCenter FC;

        /// <summary>
        /// 线程集合
        /// </summary>
        static public Hashtable Thread_HT = new Hashtable();


        /// <summary>
        /// 登陆窗口
        /// </summary>
        FormLogin FL;

        /// <summary>
        /// 线程被调用了停止
        /// </summary>
        bool TBestop = false;

        #region 弹窗网页的处理

        //加载完成后进行处理
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //处理网页
        }

        /// <summary>
        /// 加载弹出网页
        /// </summary>
        private void initTC()
        {
            this.panelTC.MouseMove -= new MouseEventHandler(panelTC_MouseMove);
            int frm_left = this.Width / 2 - panelTC.Width / 2;
            int frm_top = this.Height / 2 - panelTC.Height / 2;
            panelTC.Location = new System.Drawing.Point(frm_left, frm_top);
            
        }

        private int tmpx = 0;
        private int tmpy = 0;
        private void panelTC_MouseUp(object sender, MouseEventArgs e)
        {
            this.panelTC.Cursor = Cursors.Default;
            this.panelTC.MouseMove -= new MouseEventHandler(panelTC_MouseMove);
        }

        private void panelTC_MouseMove(object sender, MouseEventArgs e)
        {
            this.panelTC.Location = new Point(this.panelTC.Location.X
          + e.X - this.tmpx, this.panelTC.Location.Y + e.Y - this.tmpy);
        }

        private void panelTC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                this.tmpx = e.X;

                this.tmpy = e.Y;

                this.panelTC.Cursor = Cursors.SizeAll;

                this.panelTC.MouseMove += new
                MouseEventHandler(panelTC_MouseMove);

            }

        }

        private void label11_Click(object sender, EventArgs e)
        {
            panelTC.Visible = false;
        }
        #endregion

        #region 键盘精灵的相关处理
        /// <summary>
        /// 键盘鼠标钩子类
        /// </summary>
        UserActivityHook actHook;

        //相应键盘事件
        private void KeyEventHandler_Run(object sender, KeyEventArgs e)
        {
            TB_KEY.Focus();
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
            panel_JPJL.Width = 0;
        }


        //检索内容变更，执行检索(事件)
        private void TB_KEY_TextChanged(object sender, EventArgs e)
        {
            TB_KEY_TextChanged_Run();
        }

        /// <summary>
        /// 检索内容变更，执行检索(被调用)
        /// </summary>
        private void TB_KEY_TextChanged_Run()
        {
            //显示出键盘精灵区域
            panel_JPJL.Width = 257;

            

            //测试从主列表编号或名称中找要显示的数据
            //这里只是简单演示，负责数据需要开新线程从服务器读取，要注意防范线程数据冲突
            string nowstr = TB_KEY.Text.Trim();
            listView1.Items.Clear();
            if (nowstr != "")
            {
                for (int i = 0; i < dataGridViewMain.Rows.Count; i++)
                {
                    string a1 = dataGridViewMain.Rows[i].Cells[1].Value.ToString();
                    string b1 = dataGridViewMain.Rows[i].Cells[2].Value.ToString();
                    string c1 = dataGridViewMain.Rows[i].Cells["htzq"].Value.ToString();
                    if (a1.IndexOf(nowstr) >= 0 || b1.IndexOf(nowstr) >= 0)
                    {
                        ListViewItem LVI = new ListViewItem(new string[] { a1, b1, c1 });
                        listView1.Items.Add(LVI);
                    }
                }
            }
                
        }

        //焦点离开键盘精灵区域时，清理数据并隐藏键盘精灵
        private void panel_JPJL_Leave(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            TB_KEY.Text = "";
  
            panel_JPJL.Width = 0;
    
            this.Invalidate();
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
            ExitApp();
        }
        /// <summary>
        /// 退出并关闭系统
        /// </summary>
        public void ExitApp()
        {
            notifyIconMain.Visible = false;
            notifyIconMain.Dispose();
            System.Environment.Exit(0);

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



        #region 提醒检查线程相关
        /// <summary>
        /// 开启提醒检查线程
        /// </summary>
        private void beginTrayMsg()
        {
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            DataControl.RunThreadClassTrayMsg RTCTM = new DataControl.RunThreadClassTrayMsg(null, tempDFT);
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
            SubForm.FormTrayMsg FTM = new SubForm.FormTrayMsg(OutPutHT);
            FTM.Show();
        }
        #endregion

        #region 自选商品的添加和删除
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

            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(re);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "测试弹窗", Almsg3);
            FRSE3.ShowDialog();
        }


        #endregion

        #region 主体部分交易数据处理,包括默认和自选商品


 

        /// <summary>
        /// 启动加载交易数据线程
        /// </summary>
        private void TestLoadList()
        {
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["行数"] = dataGridViewMain.Rows.Count;
            InPutHT["列数"] = dataGridViewMain.ColumnCount;

            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRun));
            trd.Name = "加载默认数据";
            trd.IsBackground = true;
            Thread_HT[trd.Name] = trd;
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
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["分类编号"] = flbh;//
            InPutHT["行数"] = dataGridViewMain.Rows.Count;
            InPutHT["列数"] = dataGridViewMain.ColumnCount;
            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunErJi));
            trd.Name = "加载二级分类限定数据";
            trd.IsBackground = true;
            Thread_HT[trd.Name] = trd;
            trd.Start();
        }

        /// <summary>
        /// 启动加载自选交易数据线程
        /// </summary>
        private void TestLoadListZXSP()
        {
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["行数"] = dataGridViewMain.Rows.Count;
            InPutHT["列数"] = dataGridViewMain.ColumnCount;

            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunZXSP));
            trd.Name = "加载自选数据";
            trd.IsBackground = true;
            Thread_HT[trd.Name] = trd;
            trd.Start();

            //变更菜单
            加入自选商品ToolStripMenuItem.Visible = false;
            删除自选商品ToolStripMenuItem.Visible = true;
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



        DataGridViewColumn dgvc;
        SortOrder so;
        int si1;
        //处理非线程创建的控件
        private void ShowThreadResult_Invoke(Hashtable OutPutHT)
        {

            if (TBestop)
            {
                TBestop = false;
                return;
            }

            if (dataGridViewMain == null)
            { return; }
            //DataSet dstest = (DataSet)(OutPutHT["测试列表数据表"]);

            //加载数据，这里不能使用绑定，要手工画上
            if ((bool)OutPutHT["可以绑定"])
            {

                //DataControl.DataOtherClass.LoadDataSet(dstest, dataGridViewMain);
                if (OutPutHT["执行标记"].ToString() == "保持既定排序和横向滚动条不变")
                {
                    dgvc = dataGridViewMain.SortedColumn;
                    so = dataGridViewMain.SortOrder;
                    si1 = dataGridViewMain.FirstDisplayedScrollingColumnIndex;
      
                }
                if (OutPutHT["执行标记"].ToString() == "恢复排序和横向滚动条")
                {
                    ListSortDirection direction;
                    if (so == SortOrder.Ascending && dgvc != null)
                    {
                        direction = ListSortDirection.Ascending;
                        dataGridViewMain.Sort(dgvc, direction);
                    }
                    if (so == SortOrder.Descending && dgvc != null)
                    {
                        direction = ListSortDirection.Descending;
                        dataGridViewMain.Sort(dgvc, direction);
                    }
                    dataGridViewMain.FirstDisplayedScrollingColumnIndex = si1;
                }
                if (OutPutHT["执行标记"].ToString() == "改变列")
                {
                    for (int c = 0; c < dataGridViewMain.ColumnCount - 1; c++)
                    {
                        DataRow dr = (DataRow)OutPutHT["值"];
                        dataGridViewMain.Rows[Convert.ToInt32(OutPutHT["i"])].Cells[c].Value = dr[c].ToString();
                    }
                }
                if (OutPutHT["执行标记"].ToString() == "增加列")
                {
                    DataRow dr = (DataRow)OutPutHT["值"];
                    int index = dataGridViewMain.Rows.Add();
                    for (int c = 0; c < dataGridViewMain.ColumnCount - 1; c++)
                    {
                        dataGridViewMain.Rows[index].Cells[c].Value = dr[c].ToString();
                    }
                }
                if (OutPutHT["执行标记"].ToString() == "删除列")
                {
                    dataGridViewMain.Rows.RemoveAt( Convert.ToInt32(OutPutHT["值"]));
                }
  
                //L_title.Text = "获取成功！" + DateTime.Now.ToLongTimeString() + "[" + dstest.Tables[0].Rows.Count + "]";
                PBload.Visible = false;
            }
            else
            {
                //label3.Text = "获取成功，但未找到数据。";
                PBload.Visible = false;
                dataGridViewMain.Rows.Clear();
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

                //加载底部分类(这是线程)
                initFL();

                //加载底部统计分析数据(这是线程)
                BeginInitTJFX();



                //开启登陆窗口的操作
                FL.uploadOK();
            }
            else
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("无法初始化基础数据，请检查网络连接。");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "软件初始化提示", Almsg3);
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
            trd.Name = "加载分类列表";
            trd.IsBackground = true;
            Thread_HT[trd.Name] = trd;
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
            panel_fenlei.Location = new Point(dj1.Location.X, TLB_fenlei.Location.Y - panel_fenlei.Height - 2);

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


            //显示分类展示层
            panel_fenlei.Visible = true;
            panel_fenlei.Focus();


            //开启线程，显示具体二级分类
            InitFLerji(FLnumber);
            

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
                    if (shujunum != dserji.Tables[0].Rows.Count)
                    {
                        //中间文字区域
                        Label ll = new Label();
                        ll.AutoSize = true;
                        ll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                        ll.ForeColor = Color.Black;
                        ll.Margin = new System.Windows.Forms.Padding(0);
                        ll.Text = dserji.Tables[0].Rows[shujunum]["SortName"].ToString();
                        ll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
                    Thread trd = (Thread)Thread_HT["加载默认数据"];
                    Thread_HT.Remove("加载默认数据"); //删除线程标记
                    if (trd != null)
                    {
                        trd.Abort();//退出线程   
                        TBestop = true;
                    }
                }
                //线程存在才执行
                if (Thread_HT.ContainsKey("加载自选数据"))
                {
                    Thread trd = (Thread)Thread_HT["加载自选数据"];
                    Thread_HT.Remove("加载自选数据"); //删除线程标记
                    if (trd != null)
                    {
                        trd.Abort();//退出线程   
                        TBestop = true;
                    }
                }
                //线程存在才执行
                if (Thread_HT.ContainsKey("加载二级分类限定数据"))
                {
                    Thread trd = (Thread)Thread_HT["加载二级分类限定数据"];
                    Thread_HT.Remove("加载二级分类限定数据"); //删除线程标记
                    if (trd != null)
                    {
                        trd.Abort();//退出线程   
                        TBestop = true;
                    }
                }
            }
            catch { TBestop = true; }
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
                //启动线程，得到需要提前获取的一些数据或参数
                BeginGetPublisDsData();

                
            }
            else
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("无法获取更新信息，请检查网络连接。");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "版本更新提示", Almsg3);
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
            tlp.Refresh();
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

        public FormMainPublic()
        {




     
            InitializeComponent();

            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;

            //初始化界面
            initwindows();

            //开启键盘精灵
            initKeyHost();

        }



        /// <summary>
        /// 初始化界面的各项显示
        /// </summary>
        private void initwindows()
        {
            //把键盘精灵搞到顶层区域
            panel_JPJL.Parent = this;
            panel_JPJL.BringToFront();
            panel_JPJL.Location = new Point(this.Width - panel_JPJL.Width - 4, this.Height - panel_JPJL.Height - 3);
            panel_JPJL.Width = 0;
            //把分类菜单弹窗搞到顶层
            panel_fenlei.BringToFront();

            //处理头部和左侧图片
            PB_zhzx.BorderStyle = BorderStyle.None;
            PB_zhzx.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\zhzx.png");

            //调整列宽和自定义滚动条
            string kuanduliebiao = "42,72,107,96,42,58,72,69,70,42,87,87,72,87,103,57,75,77,57,75,78,150,100,100,100,100,100,100";
            string[] liekuan_arr = kuanduliebiao.Split(',');
            for (int i = 0; i < dataGridViewMain.ColumnCount; i++)
            {
                dataGridViewMain.Columns[i].Width = Convert.ToInt32(liekuan_arr[i].Replace(" ", ""));
            }
            hScrollBar1.Minimum = 5;
            hScrollBar1.Maximum = dataGridViewMain.ColumnCount-5;




            
  
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

            //加载弹出网页
            initTC();

            //先隐藏这个主窗体，并显示登陆窗口
            this.Hide();
            FL = new FormLogin(this, "首次打开");
            FL.Show();

        }

        /// <summary>
        /// 使用账号密码登陆成功后，处理一些东西
        /// </summary>
        public void userloginOK()
        {
            //处理我的账户按钮
            //处理自选商品按钮


            //处理登陆、注册按钮
            L_emailshow.Text = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString();
            //调整账号显示的位置
            L_emailshow.Location = new Point(label4.Location.X - L_emailshow.Width, L_emailshow.Location.Y);


            //开启提醒检查(这是线程)，提醒比较特殊，只有登陆成功后才开启这个无限循环的线程
            beginTrayMsg();

            //加载交易数据(这是线程)
            TestLoadList();
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
            MenuLikeLabel(L_zxzx, CMS_zxzx);
        }

        private void L_bzzx_Click(object sender, EventArgs e)
        {
            MenuLikeLabel(L_bzzx, CMS_bzzx);
        }

        private void L_zxff_Click(object sender, EventArgs e)
        {
            ;
        }

        //自定义滚动条滚动
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int aaa = e.NewValue;
            if (aaa <= 4)
            {
                aaa = 5;
            }

            dataGridViewMain.FirstDisplayedScrollingColumnIndex = aaa;
            
        }

        //鼠标点击单元格时，显示右键菜单
        private void dataGridViewMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    ////若行已是选中状态就不再进行设置
                    //if (dataGridViewMain.Rows[e.RowIndex].Selected == false)
                    //{
                    //    dataGridViewMain.ClearSelection();
                    //    dataGridViewMain.Rows[e.RowIndex].Selected = true;
                    //}
                    ////只选中一行时设置活动单元格
                    //if (dataGridViewMain.SelectedRows.Count == 1)
                    //{
                    //    dataGridViewMain.CurrentCell = dataGridViewMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    //}

                    dataGridViewMain.ClearSelection();
                    dataGridViewMain.Rows[e.RowIndex].Selected = true;

                    //弹出操作菜单
                    CMS_datagrid.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        //进入我的账户
        private void PB_zhzx_Click(object sender, EventArgs e)
        {


            if (PublicDS.PublisDsUser == null)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请登录后进行操作。");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "登录提示", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            if (PublicDS.PublisDsUser.Tables[0].Rows[0]["JSZHLX"].ToString() == "")
            {
                //未开通结算账户
                Hashtable hts = new Hashtable();
                hts["dlyx"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString();
                hts["dlmm"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLMM"].ToString();
                hts["yhm"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["YHM"].ToString();
                if (Program.fms == null || Program.fms.IsDisposed)
                {
                    Program.fms = new Formjhjxjszhzc(hts);
                    Program.fms.Show();
                }
                else
                {
                    Program.fms.Show();
                    Program.fms.Activate();
                }
                return;
            }


            if (FC == null)
            {
                FC = new FormCenter();
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
            //调整并保存合适的列宽
            string str = "";
            for (int i = 0; i < dataGridViewMain.Columns.Count; i++)
            {
                str = str + dataGridViewMain.Columns[i].Width + ",";
            }
            textBox1.Text = str;
        }

        //点击了账户显示，暂无功能
        private void L_emailshow_Click(object sender, EventArgs e)
        {
    
            //未登陆时，打开新的登陆口
            if (PublicDS.PublisDsUser == null || PublicDS.PublisDsUser.Tables["登录附加"].Rows[0]["是否登录成功"].ToString() != "成功")
            {
                FL = new FormLogin(this, "重新打开");
                FL.Show();
            }
        }

        //退出程序
        private void label4_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        //竞标排序
        private void L_jxpx_Click(object sender, EventArgs e)
        {
            //显示进度
            PBload.Visible = true;
            //停止线程
            StopmainlistTHread();
            //清理
            dataGridViewMain.Rows.Clear();

            //开启新线程，加载默认数据
            TestLoadList();
        }

        //自选商品
        private void L_dbfx_Click(object sender, EventArgs e)
        {
            string DLYX = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            if (DLYX == "")
            { return; }
            //显示进度
            PBload.Visible = true;
            //停止线程
            StopmainlistTHread();
            //清理
            dataGridViewMain.Rows.Clear();
            //开启新线程，加载自选数据
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

            SetZXSP(SPBH,"加入");
            
        }

        private void 删除自选商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //当前操作的商品编号
            string SPBH = dataGridViewMain.SelectedRows[0].Cells[1].Value.ToString();
            SetZXSP(SPBH, "删除");
        }

        private void 查看详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //当前操作的商品编号 htzq
            string SPBH = dataGridViewMain.SelectedRows[0].Cells[1].Value.ToString();
            string HTQX = dataGridViewMain.SelectedRows[0].Cells["htzq"].Value.ToString();
            FormSPinfo SPinfo = new FormSPinfo(SPBH, HTQX);
            SPinfo.Show();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string SPBH = listView1.SelectedItems[0].SubItems[0].Text;//
            string HTQX = listView1.SelectedItems[0].SubItems[2].Text;//
            FormSPinfo SPinfo = new FormSPinfo(SPBH, HTQX);
            SPinfo.Show();
        }

        //定标统计
        private void label3_Click(object sender, EventArgs e)
        {

        }

























   












    }



}
