using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Reflection;
using 客户端主程序.NewDataControl;
using System.Threading;
using System.IO;
using 客户端主程序.Support;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.MuBan
{
    public partial class FormDZGHHT : BasicForm
    {

        /// <summary>
        /// 程序集
        /// </summary>
        Assembly asm;
        /// <summary>
        /// 类型
        /// </summary>
        Type type;
        /// <summary>
        /// 激活器
        /// </summary>
        object instance;

        Hashtable hashTable;
        string strUrl = "";
        string strUrlTag = "";
        string strZZCK = "";
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        FormBMCNS formBMCNS;



        //要打印的控件
        string[] ModName;
        string Title;
        object CS;
        object[] CSarr = null;
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

        #region //构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title">打印窗体标题</param>
        /// <param name="CS">参数</param>
        /// <param name="modname">模板控件名</param>
        public FormDZGHHT(string title, object[] cs, string[] modname, string Number)
        {
            if (title == "电子购货合同")
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized; //初始为最小化状态       
            }
            CSarr = cs;
            Title = title;
            ModName = modname;
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;
            this.Text = Title;
            hashTable = new Hashtable();
            hashTable["中标定标_Number"] = Number;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title">打印窗体标题</param>
        /// <param name="CS">参数</param>
        /// <param name="modname">模板控件名</param>
        public FormDZGHHT(string title, object[] cs, string[] modname, Hashtable hashTable)
        {
            if (title == "电子购货合同")
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized; //初始为最小化状态   
            }
            CSarr = cs;
            Title = title;
            ModName = modname;
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;
            this.Text = Title;
            strUrl = hashTable["存储路径"].ToString();
            this.basicButton2.Visible = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title">打印窗体标题</param>
        /// <param name="CS">参数</param>
        /// <param name="modname">模板控件名</param>
        public FormDZGHHT(string title, string cs, string[] modname, Hashtable hashTable)
        {
            if (title == "电子购货合同")
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized; //初始为最小化状态   
            }
            CS = cs;
            Title = title;
            ModName = modname;
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;
            this.Text = Title;
            strUrl = hashTable["存储路径"].ToString();
            this.basicButton2.Visible = false;
        }

        /// <summary>
        /// 初始化 123 资质查看利用的构造函数
        /// </summary>
        /// <param name="title">打印窗体标题</param>
        /// <param name="CS">参数</param>
        /// <param name="modname">模板控件名</param>
        public FormDZGHHT(string title, string cs, string[] modname)
        {
            if (title == "电子购货合同")
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized; //初始为最小化状态   
            }
            CS = cs;
            Title = title;
            ModName = modname;
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;
            this.Text = Title;
            this.basicButton2.Visible = true;
            strZZCK = "资质查看";
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title">打印窗体标题</param>
        /// <param name="CS">参数</param>
        /// <param name="modname">模板控件名</param>
        public FormDZGHHT(string title, object[] cs, string[] modname,string Number,Hashtable hashTable)
        {
            if (title == "电子购货合同")
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized; //初始为最小化状态   
            }
            CSarr = cs;
            Title = title;
            ModName = modname;
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;
            this.Text = Title;
            strUrl = hashTable["存储路径"].ToString();
            hashTable = new Hashtable();
            hashTable["中标定标_Number"] = Number;
            this.basicButton2.Visible = false;
        }

        /// <summary>
        /// 初始化  456 资质查看利用的构造函数
        /// </summary>
        /// <param name="title">打印窗体标题</param>
        /// <param name="CS">参数</param>
        /// <param name="modname">模板控件名</param>
        public FormDZGHHT(string title, object[] cs, string[] modname)
        {
            if (title == "电子购货合同")
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized; //初始为最小化状态   
            }
            CSarr = cs;
            Title = title;
            ModName = modname;
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;
            this.Text = Title;
            this.basicButton2.Visible = true;
            strZZCK = "资质查看";
        }
        #endregion

        private void BSave_Click(object sender, EventArgs e)
        {

            //设置Document属性
            this.printDialog1.Document = this.printDocument1;
            //显示打印窗口
            if (this.printDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.printDocument1.Print();
                }
                catch (Exception excep)
                {//显示打印出错消息
                    MessageBox.Show(excep.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        int pagenum = 1;
        int num = 0;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {


            UserControl panelUCDY = (UserControl)(panelDYmain.Controls[num]);

            Bitmap bmp = new Bitmap(panelUCDY.ClientSize.Width, panelUCDY.ClientSize.Height);
            panelUCDY.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));

            e.Graphics.DrawImage(bmp, Point.Empty);
            num++;
            if (num < pagenum)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
                num = 0;
            }


        }


        private void fmDY_Load(object sender, EventArgs e)
        {
            Init_one_show();

            pagenum = ModName.Length;

            if (strZZCK == "资质查看")
            {
                //直接加载数据文件
                JZSJ();
            }
            else
            {
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                this.SetStyle(ControlStyles.ResizeRedraw, true);
                hashTable["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                RunThreadClassKTJYZH runThreadClassKTJYZH = new RunThreadClassKTJYZH(hashTable, new delegateForThread(ShowThreadResult_BMCNS));
                Thread trd = new Thread(new ThreadStart(runThreadClassKTJYZH.JudgeSFQDCNS));
                trd.IsBackground = true;
                trd.Start();
            }
            
        }

        #region//判断是否已签到承诺书
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_BMCNS(Hashtable OutPutHT)
        {
            try
            {
                Invoke(new delegateForThreadShow(ShowThreadResult_BMCNS_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_BMCNS_Invoke(Hashtable OutPutHT)
        {
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "已签":
                    //加载数据
                    //JZSJ();
                    HTMLshow htmlS = new HTMLshow("电子购货合同", "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + hashTable["中标定标_Number"].ToString() + "&g=" + Guid.NewGuid().ToString(), null);
                    htmlS.Show();
                    this.Close();
                    break;
                default:
                    //这是另一种用法
                  this.WindowState = FormWindowState.Minimized; //初始为最小化状态           
                  this.ShowInTaskbar = false; //隐藏任务栏图标
                  Hashtable hastBMCNS = new Hashtable();
                  hastBMCNS["中标定标_Number"] = hashTable["中标定标_Number"].ToString();
                  hastBMCNS["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                  formBMCNS = new FormBMCNS("保密承诺书", this, hastBMCNS);
                  formBMCNS.TopMost = true;
                  formBMCNS.StartPosition = FormStartPosition.CenterScreen;
                  formBMCNS.Show();
                    break;
            }


        }

        #endregion



        /// <summary>
        /// 加载数据
        /// </summary>
        public void JZSJ()
        {
            panelDYmain.Controls.Clear();
            for (int i = 0; i < ModName.Length; i++)
            {
                string Tagstr = ModName[i].ToString();
                string frmTypeName = Tagstr.Replace("[B区]", "").Replace("[C区]", "");
                string dllstr = "";
                Assembly asms = Assembly.GetExecutingAssembly();
                //根据数据库动态生成类实例
                if (Tagstr.IndexOf(")!") >= 0)
                {
                    dllstr = Tagstr.Replace("[B区]", "").Replace("[C区]", "").Substring(0, Tagstr.Replace("[B区]", "").Replace("[C区]", "").IndexOf(")!"));
                    frmTypeName = Tagstr.Substring(Tagstr.IndexOf(")!") + ")!".Length, Tagstr.Length - (Tagstr.IndexOf(")!") + ")!".Length));
                }
                if (dllstr == "" || dllstr.Trim() == asms.FullName.Split(',')[0]+".dll")
                {
                    asm = Assembly.GetExecutingAssembly();
                }
                else
                {
                    asm = Assembly.LoadFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + dllstr);
                }
                //不能为空值
                if (frmTypeName == "")
                {
                    return;
                }
                //根据数据库动态生成类实例
                //asm = Assembly.GetExecutingAssembly();
                type = asm.GetType(frmTypeName, false);
                //若找不到类型，则不再进行操作
                if (type == null)
                {
                    return;
                }
                instance = asm.CreateInstance(frmTypeName, false, BindingFlags.Default, null, new object[] { }, null, null);
                UserControl UC = (UserControl)instance;
                UC.Dock = DockStyle.None;//不铺满
                UC.AutoScroll = false;//出现滚动条
                UC.BackColor = Color.White;
                if (CSarr != null)
                {
                    UC.Tag = CSarr[i];
                }
                else
                {
                    UC.Tag = CS;
                }
                panelDYmain.Padding = new System.Windows.Forms.Padding(1);
                panelDYmain.Controls.Add(UC); //加入到某一个panel中
                UC.Show();//显示出来
            }
            pagenum = ModName.Length;
        
        }

        private void basicButton2_Click(object sender, EventArgs e)
        {
          
           // SRT_GetMMJData_Run();
           // baocuntupian();
            baocuntupianNew();
        }





        /// <summary>
        /// 下载全部图片(默认)
        /// </summary>
        public void baocuntupian()
        {
            this.timer1.Enabled = true;
          
        }
        /// <summary>
        /// 下载全部图片
        /// </summary>
        private void baocuntupianNew()
        {
               // 设置根在桌面
                    folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.Desktop;
                    // 允许在对话框中包括一个新建目录的按钮
                    folderBrowserDialog1.ShowNewFolderButton = true;
                    // 设置对话框的说明信息
                    folderBrowserDialog1.Description = "请选择 《" + Title + "》 打印内容的保存目录:";
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        strUrl = folderBrowserDialog1.SelectedPath;
                    }
            int j = 0;
            for (int i = 0; i < panelDYmain.Controls.Count; i++)
            {
                UserControl panelUCDY = (UserControl)(panelDYmain.Controls[i]);
                if (panelUCDY.MinimumSize == new Size(100, 100))
                {
                    j++;
                }
            }

            if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\Temp"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Temp");
            }
            string tempDirec = Title + "_" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-sss");
            string strTempFile = "";//文件存放的临时目录
            //if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\Temp\" + tempDirec))
            //{
            //    Directory.CreateDirectory(Application.StartupPath + @"\Temp\" + tempDirec);
            //}
            strTempFile = Application.StartupPath + @"\Temp";

            List<FileInfo> fileList = new List<FileInfo>();
            if (j == panelDYmain.Controls.Count)
            {
                this.timer1.Enabled = false;

                for (int i = 0; i < panelDYmain.Controls.Count; i++)
                {

                    //遍历，并一张一张的保存图片
                    UserControl panelUCDY = (UserControl)(panelDYmain.Controls[i]);
                    Bitmap bmp = new Bitmap(panelUCDY.ClientSize.Width, panelUCDY.ClientSize.Height);
                    //Graphics g = Graphics.FromImage(bmp);
                    panelUCDY.DrawToBitmap(bmp, new Rectangle(0, 0, panelUCDY.ClientSize.Width, panelUCDY.ClientSize.Height));
                    string strFilePNG = strTempFile + @"\" + Title + "_" + i.ToString().PadLeft(3, '0') + ".png";
                    bmp.Save(strFilePNG, System.Drawing.Imaging.ImageFormat.Png);
                    FileInfo fi1 = new FileInfo(strFilePNG);
                    fileList.Add(fi1);
                }
            }
            //  调用方法
            string targetZipFilePath = strUrl + @"\" + tempDirec + ".zip";// 扩展名可随意
            FileCompression.Compress(fileList, targetZipFilePath, 5, 5);
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("《" + Title + "》下载完成！");
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "对号", "中国商品批发交易平台", Almsg3);
            FRSE3.ShowDialog();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            baocuntupianNew();
        }

        private void basicButton1_Click(object sender, EventArgs e)
        {
            //设置Document属性
            this.printPreviewDialog1.Document = this.printDocument1;
            try
            {//显示打印预览窗口
                this.printPreviewDialog1.ShowDialog();
            }
            catch (Exception excep)
            {
                //显示打印出错消息
                MessageBox.Show(excep.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }	
        }

      

      
      



    }
}
