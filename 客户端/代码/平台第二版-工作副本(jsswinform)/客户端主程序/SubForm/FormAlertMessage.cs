using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;

namespace 客户端主程序
{
    public partial class FormAlertMessage : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;
        string sButtonType;
        string spicstr;
        string stitlestr;
        ArrayList stextsstr;


        /// <summary>
        /// 实例化弹窗提示
        /// </summary>
        /// <param name="ButtonType">按钮类型，分为"仅确定"和"确定取消"</param>
        /// <param name="picstr">图标类型，未分"叹号"和"问号"和"错误"</param>
        /// <param name="titlestr">弹窗标题的文字</param>
        /// <param name="textsstr">弹窗内容文字</param>
        public FormAlertMessage(string ButtonType,string picstr , string titlestr, ArrayList textsstr)
        {
            this.TitleYS = new int[] { 0, 0, -50 };

            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //====================================================

            InitializeComponent();
            TaskMenu.Show(this);//增加任务栏右键菜单

            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;

            glassButton1.DialogResult = DialogResult.Yes;
            glassButton2.DialogResult = DialogResult.Cancel;
            glassButton3.DialogResult = DialogResult.OK;
            this.DialogResult = DialogResult.Cancel; 
            sButtonType = ButtonType;
            spicstr = picstr;
            stitlestr = titlestr;
            stextsstr = textsstr;
            glassButton2.Image = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\qx.jpg");
            glassButton1.Image = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\xg.jpg");
            glassButton3.Image = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\xg.jpg");
            if (sButtonType == "仅确定")
            {
                glassButton1.Visible = false;
                glassButton2.Visible = false;
                glassButton3.Visible = true;
            }
            if (sButtonType == "确定取消")
            {
                glassButton1.Visible = true;
                glassButton2.Visible = true;
                glassButton3.Visible = false;
            }
            if (picstr == "叹号")/*成功*/
            {
                basicPictureBox7.BackgroundImage = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\tanhao.png");
            }
            if (picstr == "问号")/*确定取消*/
            {
                basicPictureBox7.BackgroundImage = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\wenhao.png");
            }
            if (picstr == "错误")
            {
                basicPictureBox7.BackgroundImage = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\cuowu.png");
            }



            //根据内容自动调整对话框大小
            int maxnumber = 20;
            foreach (string hang in stextsstr)
            {
                if (maxnumber < hang.Length)
                {
                    maxnumber = hang.Length;
                }
            }
            this.Width = 70 + 13 * maxnumber;

            int newg = 100 + 20 * stextsstr.Count;
            if (newg < 160)
            {
                newg = 160;
            }
            this.Height = newg;
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

        private void FormReSendEmail_Load(object sender, EventArgs e)
        {
            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;

           // this.Text = stitlestr;
            this.Text = "中国商品批发交易平台";
            textControl1.Texts = stextsstr;

            
        }

        private void glassButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
