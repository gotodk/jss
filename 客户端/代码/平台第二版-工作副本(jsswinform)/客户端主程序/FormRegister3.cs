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
using 客户端主程序.DataControl;
using System.Text.RegularExpressions;

namespace 客户端主程序
{
    public partial class FormRegister3 : BasicForm
    {
        /// <summary>
        /// 登录窗体
        /// </summary>
        FormLogin fmLogin;
        /// <summary>
        /// id
        /// </summary>
        string uid;
        /// <summary>
        /// name
        /// </summary>
        string uname;
        /// <summary>
        /// pwd
        /// </summary>
        string pwd;
        public FormRegister3(FormLogin fms,string dlyx,string yhm,string upwd)
        {
            InitializeComponent();
            fmLogin = fms;
            uid = dlyx;
            uname = yhm;
            pwd = upwd;
        }
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;
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
        /// <summary>
        /// 开通结算帐户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccountUid_Click(object sender, EventArgs e)
        {
            fmLogin.Show();
            fmLogin.AutoLogin(uid, pwd,uname, true);
            this.Hide();
            this.Dispose();
        }
        /// <summary>
        /// 先去逛逛
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowMain_Click(object sender, EventArgs e)
        {
            fmLogin.Show();
            fmLogin.AutoLogin(uid, pwd,uname, false);
            this.Hide();
            this.Dispose();
        }

        private void FormRegister3_FormClosing(object sender, FormClosingEventArgs e)
        {
            fmLogin.Show();
            this.Hide();
            this.Dispose();
        }
    }
}
