using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Threading;
using System.IO;

namespace 客户端主程序.SubForm
{
    /// <summary>
    /// 用于上传的委托。显示回调。用于记录上传的处理结果
    /// </summary>
    /// <param name="OutPutHT">需要返回的数据集合</param>
    public delegate void delegateForSC(ListView LV);


    public partial class FormSC : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

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
        /// 待上传的文件列表
        /// </summary>
        string[] ArrFiles;

        /// <summary>
        /// 记录上传完成后数据
        /// </summary>
        ListView LV;

        /// <summary>
        /// 上传结果回调委托
        /// </summary>
        delegateForSC DFS;

        /// <summary>
        /// 角色编号，用于身份标示
        /// </summary>
        string JSBH;

        /// <summary>
        /// 初始化开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
        /// </summary>
        /// <param name="arrfiles">文件数组</param>
        /// <param name="LV_temp">上传结果表</param>
        /// <param name="dfs_temp">上传结果回调委托</param>
        /// <param name="jsbh">角色编号，用于身份标示</param>
        public FormSC(string[] arrfiles,ListView LV_temp,delegateForSC dfs_temp,string jsbh)
        {
            ArrFiles = arrfiles;
            LV = LV_temp;
            LV.Items.Clear();//先清理原有数据
            DFS = dfs_temp;
            JSBH = jsbh;


            InitializeComponent();

            //Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            //this.Icon = ic;
        }

        private void FormSC_Load(object sender, EventArgs e)
        {
            //显示待上传列表
            for (int i = 0; i < ArrFiles.Length; i++)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(ArrFiles[i]);
                string ext = fi.Extension;//图片后缀名
                double l = fi.Length / 1024.0 / 1024.0;//获取图片大小，单位M
                if (l > 1)
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("上传图片最大为1M，请重新选择图片！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "上传完成", Almsg3);
                    FRSE3.ShowDialog();
                    //listView1.Items.Clear();
                    this.Close();
                    return;
                }
                listView1.Items.Add(new ListViewItem(new string[] { Path.GetFileName(ArrFiles[i]), "0K", "等待上传", "0秒", "0K", ArrFiles[i], "" }));
            }

            //滚动条默认值
            progressBar1.Maximum = int.MaxValue;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            //启动上传线程
            goSC();
        }



        /// <summary>
        /// 开始上传
        /// </summary>
        private void goSC()
        {
            //实例化委托,并参数传递给线程执行类开始执行线程
            Hashtable InPutHT = new Hashtable();
            InPutHT["待上传文件"] = ArrFiles;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_SC);
            DataControl.RunThreadClassSC RTCU = new DataControl.RunThreadClassSC(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCU.BeginRun));
            trd.Name = "上传文件";
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_SC(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_SC_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_SC_Invoke(Hashtable OutPutHT)
        {
            if (OutPutHT.ContainsKey("上传全部完成"))
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("全部文件上传结束！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "上传完成", Almsg3);
                FRSE3.ShowDialog();

                //关闭上传窗口并回调
                this.Hide();
                DFS(LV);
                this.Close();
            }
            if (OutPutHT.ContainsKey("显示进度"))
            {
                for (int p = 0; p < listView1.Items.Count; p++)
                {
                    if (listView1.Items[p].SubItems[0].Text == OutPutHT["当前文件名"].ToString())
                    {
                        if (OutPutHT["显示进度"].ToString() == "正在上传")
                        {
                            listView1.Items[p].SubItems[1].Text = OutPutHT["已上传"].ToString();
                            listView1.Items[p].SubItems[2].Text = OutPutHT["上传进度"].ToString();
                            listView1.Items[p].SubItems[3].Text = OutPutHT["已用时"].ToString();
                            listView1.Items[p].SubItems[4].Text = OutPutHT["平均速度"].ToString();
                            progressBar1.Value = (int)OutPutHT["滚动条进度"];
                        }
                        if (OutPutHT["显示进度"].ToString() == "单个完成")
                        {
                            listView1.Items[p].SubItems[1].Text = "100%";
                            listView1.Items[p].SubItems[6].Text = OutPutHT["远程保存路径"].ToString();

                            progressBar1.Value = int.MaxValue;

                            ListViewItem LVI = new ListViewItem(new string[] { listView1.Items[p].SubItems[5].Text, listView1.Items[p].SubItems[6].Text, listView1.Items[p].SubItems[0].Text, JSBH, "上传成功" });
                            LV.Items.Add(LVI);

                        }
                        if (OutPutHT["显示进度"].ToString() == "单个出错(服务器错误)")
                        {
                            listView1.Items[p].SubItems[1].Text = "上传失败";
                            progressBar1.Value = 0;

                            ListViewItem LVI = new ListViewItem(new string[] { listView1.Items[p].SubItems[5].Text, listView1.Items[p].SubItems[6].Text, listView1.Items[p].SubItems[0].Text, JSBH, "服务器错误" });
                            LV.Items.Add(LVI);
                        }
                        if (OutPutHT["显示进度"].ToString() == "单个出错(程序错误)")
                        {
                            listView1.Items[p].SubItems[1].Text = "上传失败";
                            progressBar1.Value = 0;

                            ListViewItem LVI = new ListViewItem(new string[] { listView1.Items[p].SubItems[5].Text, listView1.Items[p].SubItems[6].Text, listView1.Items[p].SubItems[0].Text, JSBH, "程序错误" });
                            LV.Items.Add(LVI);
                        }
                    }
                }

            }


        }


    }
}
