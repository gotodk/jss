using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using 客户端主程序.DataControl;
using System.Reflection;
using System.Threading;
using System.IO;

namespace 客户端主程序.SubForm.NewCenterForm
{

    public partial class ImageShow  : BasicForm
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


        Hashtable htT = new Hashtable();
        public ImageShow(Hashtable httemp)
        {
            htT = httemp;
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
          this.Width = 800;
            this.Height = 600;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;


        }

        private void ImageShow_Load(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            pictureBox1.Left = (this.Width - pictureBox1.Width) / 2;
            pictureBox1.Top = (this.Height - pictureBox1.Height) / 2;
            panel1.Enabled = false;
            timer1.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            uCimageV1.Focus();
            uCimageV1.getImage(uCimageV1.image, "");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            uCimageV1.Focus();
            uCimageV1.getImage(uCimageV1.image, "1b1");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            uCimageV1.Focus();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PnG Image|*.png|Wmf  Image|*.wmf";
            saveFileDialog.Filter = "PnG Image|*.png";
            saveFileDialog.FilterIndex = 0;
            if (uCimageV1.image == null)
            {
                return;
            }
            else if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (uCimageV1.image != null)
                {
                    uCimageV1.image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            uCimageV1.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            timer1.Enabled = false;
            panel1.Enabled = true;
            try
            {
                if (htT["类型"].ToString() == "网址")
                {
                    System.Net.WebRequest webreq = System.Net.WebRequest.Create(htT["地址"].ToString());
                    System.Net.WebResponse webres = webreq.GetResponse();
                    Stream stream = webres.GetResponseStream();
                    Image image;
                    image = Image.FromStream(stream);
                    stream.Close();
                    uCimageV1.image = image;
                }
                if (htT["类型"].ToString() == "图片Image实例")
                {
                    ;
                }
                if (htT["类型"].ToString() == "图片本地路径")
                {
                    ;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("图片不存在！");
            }
        }

  


    }
}
