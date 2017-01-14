using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using 客户端主程序.Support;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class fmDY_Demo : BasicForm
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



        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        //要打印的控件
        string[] ModName;
        string Title;
        object CS;
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
        /// <param name="CS">参数</param>
        /// <param name="modname">模板控件名</param>
        public fmDY_Demo(string title, object cs, string[] modname)
        {
            CS = cs;
            Title = title;
            ModName = modname;
            InitializeComponent();
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            //设置窗体显示在屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //不在任务栏显示此信息
            this.ShowInTaskbar = true;
            this.Text = Title;
        }

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



        private void fmDY_Load(object sender, EventArgs e)
        {
            Init_one_show();
            pagenum = ModName.Length;
            panelDYmain.Controls.Clear();
            this.basicButton1.Enabled = false;
            this.BSave.Enabled = false;
            btnGetData_Click(null,null);
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            DataSet dsreturn = WSC2013.RunAtServices("GetYSTP", null);
         
            panelDYmain.Controls.Clear();
            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            if (zt == "ok")
            {
                Assembly asms = Assembly.GetExecutingAssembly();
                

                DataTable dataTable = dsreturn.Tables["图片链接"];
                pagenum = dataTable.Rows.Count;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string Tagstr = ModName[i].ToString();
                    string frmTypeName = Tagstr.Replace("[B区]", "").Replace("[C区]", "");
                    string dllstr = "";

                    //根据数据库动态生成类实例
                    if (Tagstr.IndexOf(")!") >= 0)
                    {
                        dllstr = Tagstr.Replace("[B区]", "").Replace("[C区]", "").Substring(0, Tagstr.Replace("[B区]", "").Replace("[C区]", "").IndexOf(")!"));
                        frmTypeName = Tagstr.Substring(Tagstr.IndexOf(")!") + ")!".Length, Tagstr.Length - (Tagstr.IndexOf(")!") + ")!".Length));
                    }
                    if (dllstr == "" && dllstr.Trim() != asms.FullName.Split(',')[0]+".dll")
                    {
                        asm = Assembly.GetExecutingAssembly();
                    }
                    else
                    {
                        asm = Assembly.LoadFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + dllstr);
                    }
                    //asm = Assembly.GetExecutingAssembly();
                    type = asm.GetType(ModName[i].ToString(), false);
                    //若找不到类型，则不再进行操作
                    instance = asm.CreateInstance(ModName[i].ToString(), false, BindingFlags.Default, null, new object[] { }, null, null);
                    UserControl UC = (UserControl)instance;
                    UC.Dock = DockStyle.None;//不铺满
                    UC.AutoScroll = false;//出现滚动条
                    UC.BackColor = Color.White;
                    string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + dataTable.Rows[i]["TPLJ"].ToString();
                    UC.Tag = url;
                    panelDYmain.Padding = new System.Windows.Forms.Padding(1);
                    panelDYmain.Controls.Add(UC); //加入到某一个panel中
                    UC.Show();//显示出来
                }
            }
            this.basicButton1.Enabled = true;
            this.BSave.Enabled = true;
        }

     
      


    }
}
