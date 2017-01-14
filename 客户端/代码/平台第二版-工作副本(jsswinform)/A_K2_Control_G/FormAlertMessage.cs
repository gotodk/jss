using System;
using System.Drawing;
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
        ArrayList stextsstr_new = new ArrayList();

        /// <summary>
        /// 实例化弹窗提示
        /// </summary>
        /// <param name="ButtonType">按钮类型，分为"仅确定"和"确定取消"</param>
        /// <param name="picstr">图标类型，未分"其他"和"问号"和"其他"</param>
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

            if (this.Parent == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

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
                AcceptButton = null;
            }
            if (sButtonType == "确定取消")
            {
                glassButton1.Visible = true;
                glassButton2.Visible = true;
                glassButton3.Visible = false;
                AcceptButton = null;
            }
            if (picstr == "对号")/*成功*/
            {
                basicPictureBox7.BackgroundImage = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\duihao.png");
            }
            if (picstr == "问号")/*确定取消*/
            {
                basicPictureBox7.BackgroundImage = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\wenhao.png");
            }
            if (picstr == "其他")
            {
                basicPictureBox7.BackgroundImage = Image.FromFile(@System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\tanhao.png");
            }



            //根据内容自动调整对话框大小

            Graphics graphics = CreateGraphics();
            int maxWidth = 200; //最大宽度
            int maxHeight = 130;
            foreach (string hang in stextsstr)
            {
                if (hang.IndexOf("\n\r") >= 0)
                {
                    string[] arr = hang.Replace("\n\r","●").Split('●');
                    
                    for (int p = 0; p < arr.Length; p++)
                    {
                        SizeF sizeF = graphics.MeasureString(arr[p], textControl1.Font);
                        if (maxWidth <  Convert.ToInt16(sizeF.Width))
                        {
                            maxWidth = Convert.ToInt16(sizeF.Width);
                        }
                        maxHeight = maxHeight + Convert.ToInt16(sizeF.Height);
                        stextsstr_new.Add(arr[p]);
                    }
                       
                }
                else
                {
                    SizeF sizeF = graphics.MeasureString(hang, textControl1.Font);
                    if (maxWidth < Convert.ToInt16(sizeF.Width))
                    {
                        maxWidth = Convert.ToInt16(sizeF.Width);
                    }
                    maxHeight = maxHeight + Convert.ToInt16(sizeF.Height);
                    stextsstr_new.Add(hang);
                }

            }
            this.Width = this.Width - textControl1.Width + 30 + maxWidth;

            this.Height = maxHeight;

            graphics.Dispose();
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

            textControl1.Texts = stextsstr_new;

            
        }

        private void glassButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
