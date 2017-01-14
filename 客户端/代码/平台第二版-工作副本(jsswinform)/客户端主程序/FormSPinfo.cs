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

namespace 客户端主程序
{
    public partial class FormSPinfo : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;


        /// <summary>
        /// 商品编号
        /// </summary>
        string SPBH = "";

        string HTQX = "";

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

        public FormSPinfo(string SPBHtemp,string HTQXtemp)
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //====================================================

            InitializeComponent();

            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;

            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;

            SPBH = SPBHtemp;
            HTQX = HTQXtemp;
            
        }



        private void FormCenter_Load(object sender, EventArgs e)
        {
            this.Text = "商品详情["+SPBH+"]";

            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["商品编号"] = SPBH;//
            InPutHT["合同期限"] = HTQX;//
            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunZhiDing));
            trd.IsBackground = true;
            trd.Start();

        }



        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }


        //处理非线程创建的控件
        private void ShowThreadResult_Invoke(Hashtable OutPutHT)
        {

            DataSet dstest = (DataSet)(OutPutHT["测试列表数据表"]);

            if (dstest != null && dstest.Tables[0].Rows.Count >= 1)
            {
                //去掉最后六个隐藏字段
                for (int i = 0; i < 6;i++ )
                {
                    dstest.Tables[0].Columns.RemoveAt(dstest.Tables[0].Columns.Count - 1);
                }
                dstest.Tables[0].Columns.RemoveAt(0);//去掉序号

                int lienum = TLPshow.ColumnCount;
                int hangnum = TLPshow.RowCount;
                int shujunum = 0;
                for (int h = 0; h < hangnum; h++)
                {
                    for (int l = 0; l < lienum; l++)
                    {
                        //有数据才加入
                        if (shujunum != dstest.Tables[0].Columns.Count)
                        {
                            //中间文字区域
                            Label ll = new Label();
                            ll.AutoSize = true;
                            ll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            ll.BackColor = Color.AliceBlue;
                            ll.ForeColor = Color.Black;
                            ll.Margin = new System.Windows.Forms.Padding(0);
                            ll.Text = dstest.Tables[0].Columns[shujunum].ColumnName + ":" + dstest.Tables[0].Rows[0][shujunum].ToString();
                            ll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                            ll.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
                            ll.Name = "myshow_" + shujunum.ToString();

                            TLPshow.Controls.Add(ll, l, h);
                        }
                        shujunum++;
                    }

                }
            }



        }


    }
}
