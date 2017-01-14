using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Reflection;
using 客户端主程序.NewDataControl;
using System.Threading;

namespace 客户端主程序.SubForm.NewCenterForm.MuBan
{
    public partial class FormBMCNS : BasicForm
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
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 电子购货合同
        /// </summary>
        FormDZGHHT fromDZGHHT;

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

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title">打印窗体标题</param>
        /// <param name="fromDZ">电子购货合同</param>
        /// <param name="CS">参数</param>
        /// <param name="modname">模板控件名</param>
        public FormBMCNS(string title, FormDZGHHT fromDZ ,Hashtable hash)
        {
            hashTable = hash;

            InitializeComponent();
            //this.TitleYS = new int[] { 0, 0, -50 };
            fromDZGHHT = fromDZ;
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;

            this.Text = title;
       
        }


        //调用远程模板，并实时分页截图。
        private void BeginShow()
        {
            this.webBrowser1.Width = 700;
            //this.webBrowser1.Height = 6200;
            //获取可滚动区域（必须）
            Rectangle rectBody = this.webBrowser1.Document.Body.ScrollRectangle;
            int width = rectBody.Width;
            int height = rectBody.Height;
            //重新设置浏览器高度和宽度（必须）
            this.webBrowser1.Width = width;
            this.webBrowser1.Height = height;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            BeginShow();
           
        }
        /// <summary>
        /// load函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormBMCNS_Load(object sender, EventArgs e)
        {
       
          
            this.btnAccept.Enabled = false;
            string strUrl = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/BZCNS.aspx?Number=" + hashTable["中标定标_Number"].ToString() + "&DLYX=" + hashTable["登录邮箱"].ToString();
            webBrowser1.Url = new Uri(strUrl);
            this.btnAccept.Enabled = true;
          
        }
        /// <summary>
        ///点击我同意的处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            btnAccept.Enabled = false;
            RunThreadClassKTJYZH runThreadClassKTJYZH = new RunThreadClassKTJYZH(hashTable, new delegateForThread(ShowThreadResult_BMCNS_Accept));
            Thread trd = new Thread(new ThreadStart(runThreadClassKTJYZH.JYZHQDCNS));
            trd.IsBackground = true;
            trd.Start();
        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_BMCNS_Accept(Hashtable OutPutHT)
        {
            try
            {
                Invoke(new delegateForThreadShow(ShowThreadResult_BMCNS_Accept_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_BMCNS_Accept_Invoke(Hashtable OutPutHT)
        {
            btnAccept.Enabled = true;
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    this.WindowState = FormWindowState.Minimized; //初始为最小化状态           
            this.ShowInTaskbar = false; //隐藏任务栏图标

                    //对合同主体的查看做特殊的处理
            if (fromDZGHHT.Text == "电子购货合同")
            {
                fromDZGHHT.Hide();
                fromDZGHHT.Close();
                HTMLshow htmlS = new HTMLshow("电子购货合同", "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=" + hashTable["中标定标_Number"].ToString() + "&g=" + Guid.NewGuid().ToString(), null);
                htmlS.Show();
            }
            else
            {
                fromDZGHHT.JZSJ();
                fromDZGHHT.WindowState = FormWindowState.Normal;
                fromDZGHHT.Show();
            }



                    break;
                default:
                      ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    break;
            }
            this.MinimumSize = new Size(100, 100);

        }

      
      
    }
}
