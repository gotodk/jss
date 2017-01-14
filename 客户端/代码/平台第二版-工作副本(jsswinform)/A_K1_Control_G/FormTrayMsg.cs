using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Runtime.InteropServices;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm
{
    public partial class FormTrayMsg : BasicForm
    {
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        /*
        1. AW_SLIDE : 使用滑动类型, 默认为该类型. 当使用 AW_CENTER 效果时, 此效果被忽略
        2. AW_ACTIVE: 激活窗口, 在使用了 AW_HIDE 效果时不可使用此效果
        3. AW_BLEND: 使用淡入效果
        4. AW_HIDE: 隐藏窗口
        5. AW_CENTER: 与 AW_HIDE 效果配合使用则效果为窗口几内重叠,  单独使用窗口向外扩展.
        6. AW_HOR_POSITIVE : 自左向右显示窗口
        7. AW_HOR_NEGATIVE: 自右向左显示窗口
        8. AW_VER_POSITVE: 自顶向下显示窗口
        9. AW_VER_NEGATIVE : 自下向上显示窗口
        */
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;
        public const Int32 AW_ACTIVATE = 0x00020000;
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;

        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        static int a = 118;
        static int b = 255;
        static string nowstr = "a";
        Hashtable OutPutHT = new Hashtable();

        public FormTrayMsg(Hashtable OutPutHT_temp)
        {

            //=======================所有窗体都有这个玩意=========
            //Init_one_show();
            //====================================================
            OutPutHT = OutPutHT_temp;
            InitializeComponent();

            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;


            //Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            //Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);

            ////  为了在白色背景上显示，尽量生成深色
            //int int_Red = RandomNum_First.Next(256);
            //int int_Green = RandomNum_Sencond.Next(256);
            //int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            //int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            ////118,118,188  128,128,128
            if (nowstr == "a")
            {
                panel1.BackColor = Color.FromArgb(a, a, a);
                richTextBox1.BackColor = Color.FromArgb(a, a, a);
                richTextBox1.ForeColor = Color.White;
                nowstr = "b";
            }
            else
            {
                panel1.BackColor = Color.FromArgb(b, b, b);
                richTextBox1.BackColor = Color.FromArgb(b, b, b);
                richTextBox1.ForeColor = Color.Red;
                nowstr = "a";
            }




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

        private void FormTrayMsg_FormClosed(object sender, FormClosedEventArgs e)
        {
            //动态关闭窗体
            AnimateWindow(this.Handle, 800, AW_SLIDE + AW_HIDE + AW_CENTER);

        }

        private void FormTrayMsg_Load(object sender, EventArgs e)
        {
            this.Text = "提醒 - " + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();

            //动画启动
            this.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - this.Width, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - this.Height);

            AnimateWindow(this.Handle, 800, AW_VER_NEGATIVE);

            //处理提醒：
            DataSet dstx = (DataSet)OutPutHT["提醒内容"];
            richTextBox1.Text = "";
            for (int i = 0; i < dstx.Tables["提醒数据"].Rows.Count; i++)
            {

                richTextBox1.AppendText("●" + ((System.DateTime)(dstx.Tables["提醒数据"].Rows[i]["TXSJ"])).ToString("yyyy年MM月dd日HH时mm分ss秒") + "：\r\n" + dstx.Tables["提醒数据"].Rows[i]["TXNRWB"].ToString() + "\r\n\r\n");
            }
            //richTextBox1.Text = OutPutHT["提醒内容"].ToString();
            this.Text = "提醒信息";
            textBox1.Focus();
            textBox1.Location = new Point(5000, 5000);
        }
    }
}
