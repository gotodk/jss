using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using 客户端主程序.SubForm.CenterForm;
using 客户端主程序.DataControl;
using System.Reflection;
using System.Threading;
using 客户端主程序.Support;
using System.Runtime.InteropServices;

namespace 客户端主程序.SubForm.NewCenterForm
{

    public partial class Center2013  : BasicForm
    {

        [DllImport("user32.dll")] 
         public static extern bool ReleaseCapture(); 

      [DllImport("user32.dll")] 
       public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam); 



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

        public Center2013()
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //设置图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            //====================================================

            InitializeComponent();

            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;
          this.Width = (int)(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width * 0.8);
            this.Height = (int)(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height * 0.8);
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;


        }


        /// <summary>
        /// 窗体加载执行
        /// </summary>
        public void LoadRun()
        {
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["交易方名称"].ToString().Trim() == "")
            {
                this.Text = "交易账户(模拟运行)-" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
            }
            else if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["证券资金账号"].ToString().Trim() == "")
            {
                this.Text = "交易账户(模拟运行)-" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim() + "(" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["交易方名称"].ToString().Trim() + ")";
            }
            else
            {
                this.Text = "交易账户(模拟运行)-" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim() + "(" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["交易方名称"].ToString().Trim() + ")(交易方编号：" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["证券资金账号"].ToString().Trim() + ")";
            }
            //测试本Form的text内容的长度
            Graphics graphics = CreateGraphics();
            SizeF sizeF = graphics.MeasureString(this.Text, this.Font);
            int maxWidth = Convert.ToInt32(sizeF.Width);
            //this.flowLayoutPanel1.Location = new Point(maxWidth + this.Icon.Size.Width + 75, this.flowLayoutPanel1.Location.Y);
            Image[] imageCollectio = JYFXYMX.GetXYImages(Convert.ToDouble(PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["账户当前信用分值"]));
            flowLayoutPanel1.Controls.Clear();
            if (imageCollectio == null)//该用户信用分数为“0”或者“负数”
            {
                flowLayoutPanel1.Visible = false;
            }
            else
            {
                flowLayoutPanel1.Visible = true;
                for (int i = 0; i < imageCollectio.Length; i++)
                {
                    PictureBox pictureModule = new PictureBox();
                    pictureModule.Image = imageCollectio[i];
                    pictureModule.Size = new Size(16, 13);
                    pictureModule.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureModule.Padding = new Padding(0, 0, 0, 0);
                    pictureModule.Margin = new Padding(0, 0, 0, 0);
 
                    pictureModule.MouseDown += pictureModule_MouseDown;
                    flowLayoutPanel1.Controls.Add(pictureModule);
                }

            }
        }

        private void Center2013_Load(object sender, EventArgs e)
        {
            //设置三个菜单的属性,传递父窗体

            uCmenuBBB.Center2013CC = this;
            uCmenuCCC.Center2013CC = this;

            uca1.Center2013CC = this;
            LoadRun();
            
        }

        //自动定位菜单
        public void auto_ClickLabel(string labeltext)
        {
            uca1.auto_Click_label(labeltext);
        }

        void pictureModule_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0); 
        }

        TextBox tb = new TextBox();
        
        private void splitContainer2_MouseUp(object sender, MouseEventArgs e)
        {
            tb.Location = new Point(-1000,-1000);
            this.Controls.Add(tb);
            tb.Focus();
            
        }

        private void splitContainer1_MouseUp(object sender, MouseEventArgs e)
        {
            tb.Location = new Point(-1000, -1000);
            this.Controls.Add(tb);
            tb.Focus();
        }



        private void Center2013_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void flowLayoutPanel1_Move(object sender, EventArgs e)
        {
           ReleaseCapture(); 
          SendMessage(this.Handle, 0x0112, 0xF012, 0); 

        }

        private void flowLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0); 

        }

        private void flowLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0); 
        }
    }
}
