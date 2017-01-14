using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using 客户端主程序.DataControl;
using 客户端主程序.SubForm.NewCenterForm;
using 客户端主程序.NewDataControl;
using 客户端主程序.SubForm.NewCenterForm.GZZD;
using 客户端主程序.Support;
using 客户端主程序.SubForm;
namespace 客户端主程序
{

    public partial class SPXQ2014 : BasicForm
    {


        Pen pen_bjline = new Pen(Color.FromArgb(110, 19, 18), 1);

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;   

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
            Timer_DC.Interval = 1;
            this.Timer_DC.Tick += new System.EventHandler(this.Timer_DC_Tick);
            //淡出效果
            MaxDC();
        }

        /// <summary>
        /// 显示窗体时启动淡出
        /// </summary>
        public void MaxDC()
        {
            this.Opacity = 0;
            Timer_DC.Enabled = true;
        }

        //淡出显示窗体，绕过窗体闪烁问题
        private void Timer_DC_Tick(object sender, EventArgs e)
        {

            this.Opacity = this.Opacity + Program.DC_step + 1;
            if (!Program.DC_open)
            {
                this.Opacity = 1;
            }
       
            if (this.Opacity >= 1)
            {
                Timer_DC.Enabled = false;
                gogo = true;
             
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
        private FormWindowState SF = FormWindowState.Maximized;
        /// <summary>
        /// 重绘窗体的状态
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref   Message m)
        {
            try
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
            catch (Exception ex) {
                Support.StringOP.WriteLog("主窗体重绘错误：" + ex.ToString());
            }
        }

        #endregion


       

        public SPXQ2014()
        {


            InitializeComponent();

            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;

            this.Size = new System.Drawing.Size(800, 500);

            //初始化界面
            initwindows();

      
            
        }
   

        /// <summary>
        /// 初始化界面的各项显示
        /// </summary>
        private void initwindows()
        {



            //隐藏标志位
            Lbzw.Text = "";
            Lbzw.BorderStyle = BorderStyle.None;
            Lbzw.SendToBack();

       
        }

        /// <summary>
        /// 设置进度条
        /// </summary>
        /// <param name="b"></param>
        private void SetLoad(bool b)
        {
            //进度条
            panelLoad.Width = panel2.Width;
            panelLoad.Height = panel2.Height;
            panelLoad.Location = panel2.Location;


            if (!b)
            {
                pictureBox4.Visible = b;
                timer2.Enabled = true;
            }
            else
            {
                pictureBox4.Visible = b;
                panelLoad.Visible = b;
            }
        }
 
        private void FormMainPublic_Load_1(object sender, EventArgs e)
        {

            //
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            //====================================================


            SetLoad(true);
             
        }





        /// <summary>
        /// 将菜单和按钮关联并显示
        /// </summary>
        /// <param name="l">按钮</param>
        /// <param name="cms">菜单</param>
        private void MenuLikeLabel(Label l,ContextMenuStrip cms)
        {

            if ((this.Height - l.Location.Y - l.Height) < cms.Height)
            {
                Point pt = new Point(0, -cms.Height);
                cms.Show(l, pt);
            }
            else
            {
                Point pt = new Point(0, l.Height);
                cms.Show(l, pt);
            }

        }

        string sSPBH = "";
        string sHTQX = "";
        Hashtable ht_xc = new Hashtable();

        /// <summary>
        /// 开始显示数据
        /// </summary>
        public void BeginShowDB(string SPBH, string HTQX,string shouci)
        {
            if (HTQX == "--")
            {
                HTQX = "";

            }
            sSPBH = SPBH;
            sHTQX = HTQX;

            SetLoad(true);

            if (shouci == "首次")
            {
                //不直接开启，用计时器开启。

                return;
            }
            


            //先初始化窗体中的显示数据
            yhBchart1.ShowDB(null);
            DBspxq = null;
            tableLayoutPanelGHQY.Controls.Clear();
            flowLayoutPanelSelXJ.Controls.Clear();
            foreach(Control c in panel2.Controls)
            {
                if (c.GetType().Name == "ForDP2013")
                {
                    ((ForDP2013)c).SP_ShuZhi = "";
                }
            }
            Pspbh.Text = "";
            Pspnc.Text = "";
            Pggbz.Text = "";
            Pxingneng.Text = "";
            Pmiaoshu.Text = "";
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPXQ2014));
            pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
         
            //开启线程加载数据
            if (SPBH == "")
            { return; }
 

            //实例化委托,并参数传递给线程执行类开始执行线程
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["商品编号"] = SPBH;//
            InPutHT["合同期限"] = HTQX;//
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DPSPXQ xq = new DPSPXQ(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(xq.BeginRun2014));
            ht_xc["加载查看详情数据"]=trd;
            trd.Name = "加载查看详情数据|" + Guid.NewGuid().ToString();
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

        /// <summary>
        /// 首次获得的数据
        /// </summary>
        DataSet DBspxq = null;


 

                //处理非线程创建的控件
        private void ShowThreadResult_Invoke(Hashtable OutPutHT)
        {
            DataSet dsre = (DataSet)OutPutHT["返回值"];
            DBspxq = dsre.Copy();
            //禁止pnl重绘,这里比较特殊，一定要由这个东西
            SendMessage(panel2.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

            //处理商品信息
            Pspbh.Text = sSPBH;
            if (dsre.Tables.Contains("商品信息") && dsre.Tables["商品信息"].Rows.Count > 0)
            {
                Pspnc.Text = dsre.Tables["商品信息"].Rows[0]["商品名称"].ToString();
                Pggbz.Text = dsre.Tables["商品信息"].Rows[0]["规格标准"].ToString();

                //临时注释，先用商品描述，后台开发完成再换回来
                //string xn = dsre.Tables["商品信息"].Rows[0]["商品性能"].ToString();
                string xn = dsre.Tables["商品信息"].Rows[0]["商品描述"].ToString();
                //if (xn.Length >= 115)
                //{
                //    xn = xn.Substring(0, 115);
                //}
                //else
                //{
                //    xn = xn.PadRight(115);
                //}
                Pxingneng.Text =  xn;

                string ms = dsre.Tables["商品信息"].Rows[0]["商品描述"].ToString();
                //if (ms.Length >= 90)
                //{
                //    ms = ms.Substring(0, 90);
                //}
                //else
                //{
                //    ms = ms.PadRight(90);
                //}
                Pmiaoshu.Text = ms;

                



                //图片
                string tupian = dsre.Tables["商品信息"].Rows[0]["商品图片"].ToString();
                if (tupian.Trim() != "")
                {
                    string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/sptp/";
                    try
                    {
                        WebClient wc = new WebClient();
                        Image image = Image.FromStream(wc.OpenRead(url + tupian));
                        pictureBox2.Image = image;
                    }
                    catch {
                        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPXQ2014));
                        pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
                    }

                }
                else
                {
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPXQ2014));
                    pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
                }
            }


            //处理曲线图
            if (dsre.Tables.Contains("数据点_即时") && dsre.Tables.Contains("数据点_三个月") && dsre.Tables.Contains("数据点_一年"))
            {


                DataSet dsceshi = yhBchart1.initReturnDataSet();
                dsceshi.Tables.Remove("数据点");

                if (sHTQX == "")
                {
                    this.linkLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

                    this.linkLabel2.BackColor = System.Drawing.Color.Black;
                    this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel2.LinkColor = System.Drawing.Color.White;

                    this.linkLabel3.BackColor = System.Drawing.Color.Black;
                    this.linkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel3.LinkColor = System.Drawing.Color.White;

                    dsceshi.Tables.Add(dsre.Tables["数据点_" + "即时"].Copy());
                    dsceshi.Tables["数据点_" + "即时"].TableName = "数据点";
                }
                else
                {
                    dsceshi.Tables.Add(dsre.Tables["数据点_" + sHTQX].Copy());
                    dsceshi.Tables["数据点_" + sHTQX].TableName = "数据点";
                }


                if (sHTQX == "即时")
                {
                    this.linkLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

                    this.linkLabel2.BackColor = System.Drawing.Color.Black;
                    this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel2.LinkColor = System.Drawing.Color.White;

                    this.linkLabel3.BackColor = System.Drawing.Color.Black;
                    this.linkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel3.LinkColor = System.Drawing.Color.White;
                }
                if (sHTQX == "三个月")
                {
                    this.linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel2.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

                    this.linkLabel1.BackColor = System.Drawing.Color.Black;
                    this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel1.LinkColor = System.Drawing.Color.White;

                    this.linkLabel3.BackColor = System.Drawing.Color.Black;
                    this.linkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel3.LinkColor = System.Drawing.Color.White;
                }
                if (sHTQX == "一年")
                {
                    this.linkLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    this.linkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel3.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

                    this.linkLabel2.BackColor = System.Drawing.Color.Black;
                    this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel2.LinkColor = System.Drawing.Color.White;

                    this.linkLabel1.BackColor = System.Drawing.Color.Black;
                    this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    this.linkLabel1.LinkColor = System.Drawing.Color.White;
                }

                dsceshi.Tables["参数"].Rows.Add(new string[] { "中标时间", "", "中标价格", "元", "否" });

                yhBchart1.ShowDB(dsceshi);

               

            }


            //处理中间的竟标信息
            if (DBspxq.Tables.Contains("竟标信息") && DBspxq.Tables["竟标信息"].Rows.Count > 0)
            {
                forDP20131.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["最高买入价"].ToString();
                forDP20132.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["最低卖出价"].ToString();
                forDP20133.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["经济批量"].ToString();
                forDP20134.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["拟售量"].ToString();
                forDP20135.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["集合预订量"].ToString();
                forDP20136.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["拟订购总量"].ToString();
                forDP20137.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["达成率"].ToString();
                forDP20138.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["日均最高供货量"].ToString();
                forDP20139.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["合同期限"].ToString();
                forDP201310.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["发票税率"].ToString();
                forDP201311.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["卖方名称"].ToString();
                forDP201312.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["卖方信用"].ToString();
                forDP201313.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["商品产地"].ToString();


                forDP201319.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["竟标轮次"].ToString();
                forDP201325.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["状态"].ToString();
                forDP201318.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["合同期限"].ToString();
                forDP201324.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["上轮定标价"].ToString();

                forDP201317.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["最低卖出价"].ToString();
                forDP201323.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["升降幅"].ToString();
                forDP201316.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["买方数量"].ToString();
                forDP201322.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["卖方数量"].ToString();
                forDP201315.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["买方新增"].ToString();
                forDP201321.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["卖方新增"].ToString();
                forDP201314.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["买方区域覆盖"].ToString();
                forDP201320.SP_ShuZhi = DBspxq.Tables["竟标信息"].Rows[0]["卖方区域覆盖"].ToString();

                if(DBspxq.Tables["竟标信息"].Rows[0]["卖方信用"].ToString() != "" && DBspxq.Tables["竟标信息"].Rows[0]["卖方信用"].ToString() != "--")
                {
                    Image[] imageCollectio = JYFXYMX.GetXYImages(Convert.ToDouble(DBspxq.Tables["竟标信息"].Rows[0]["卖方信用"]));
                    if (imageCollectio != null)//该用户信用分数为“0”或者“负数”
                    {
                        flowLayoutPanelSelXJ.Controls.Clear();
                        for (int i = 0; i < imageCollectio.Length; i++)
                        {
                            PictureBox pictureModule = new PictureBox();
                            pictureModule.Image = imageCollectio[i];
                            pictureModule.Size = new Size(20, 17);
                            pictureModule.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureModule.Padding = new Padding(0, 0, 0, 0);
                            pictureModule.Margin = new Padding(0, 0, 0, 0);

                            flowLayoutPanelSelXJ.Controls.Add(pictureModule);
                        }
                    }
                }




                tableLayoutPanelGHQY.Controls.Clear();
                if (DBspxq.Tables["竟标信息"].Rows[0]["供货区域"].ToString() != "" && DBspxq.Tables["竟标信息"].Rows[0]["供货区域"].ToString() != "--")
                {
                    string[] GHQY = DBspxq.Tables["竟标信息"].Rows[0]["供货区域"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//供货区域

                    string strGHQY = DBspxq.Tables["竟标信息"].Rows[0]["供货区域"].ToString().Replace("|", "','");
                    strGHQY = strGHQY.Substring(2, strGHQY.Length - 4);
                    DataRow[] dr = PublicDS.PublisDsData.Tables["省"].Select("省名 not in (" + strGHQY + ")");
        
                    tableLayoutPanelGHQY.ColumnCount = 3;
                    if ((GHQY.Length + dr.Length) % 3 == 0)
                    {
                        tableLayoutPanelGHQY.RowCount = (GHQY.Length + dr.Length) / 3;
                    }
                    else
                    {
                        tableLayoutPanelGHQY.RowCount = (GHQY.Length + dr.Length) / 3 + 1;
                    }

                    int ct = 0;
                    int bghqy = 0;
                    for (int i = 0; i < tableLayoutPanelGHQY.RowCount; i++)
                    {
                        tableLayoutPanelGHQY.RowStyles[i].Height = 20;
                        for (int j = 0; j < 3; j++)
                        {
                            tableLayoutPanelGHQY.ColumnStyles[j].Width = 115;
                            if (ct <= GHQY.Length - 1)
                            {
                                Label lblghqy = new Label();
                                lblghqy.Width = 130;
                                lblghqy.Height = 20;
                                lblghqy.ForeColor = Color.White;
                                lblghqy.Font = new Font(new FontFamily("Tahoma"), 9);
                                lblghqy.Text = GHQY[ct].ToString();
                                tableLayoutPanelGHQY.Controls.Add(lblghqy, j, i);
                                ct++;
                                if (ct == GHQY.Length)
                                {
                                    if (j < 2)
                                    {
                                        j++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            if (ct > GHQY.Length - 1)
                            {

                                if (dr != null && dr.Length > 0)
                                {
                                    Label lblghqy = new Label();
                                    lblghqy.Width = 130;
                                    lblghqy.Height = 20;
                                    lblghqy.ForeColor = Color.Gray;
                                    lblghqy.Font = new Font(new FontFamily("Tahoma"), 9, FontStyle.Strikeout);
                                    lblghqy.Text = dr[bghqy]["省名"].ToString();
                                    tableLayoutPanelGHQY.Controls.Add(lblghqy, j, i);
                                    bghqy++;
                                    if (bghqy > dr.Length - 1)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }
                    tableLayoutPanelGHQY.Height = tableLayoutPanelGHQY.RowCount * 20 + 2;
                }

                

            }



            //处理右侧统计信息
            if (dsre.Tables.Contains("右侧各种") && dsre.Tables["右侧各种"].Rows.Count > 0)
            {
                forDP201327.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["拟售量"].ToString();
                forDP201326.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["拟订购总量"].ToString();
                forDP201329.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["最大值"].ToString();
                forDP201328.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["最小值"].ToString();
                forDP201335.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["拟售次数"].ToString();
                forDP201334.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["拟订购次数"].ToString();

                forDP201333.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["买入次数"].ToString();
                forDP201332.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["买入金额"].ToString();

                forDP201331.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["卖出次数"].ToString();
                forDP201330.SP_ShuZhi = dsre.Tables["右侧各种"].Rows[0]["卖出金额"].ToString();
            }



            Init_FL();


            //允许重绘pnl  
            SendMessage(panel2.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            panel2.Invalidate(true);




            
            /*
            //造一个测试的曲线图模拟数据
            DataSet dsceshi = yhBchart1.initReturnDataSet();
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "160", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "400", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "20", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "50", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "20", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "800", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "2000", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "345", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "657", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "678", "这是附加文字", "" });
            dsceshi.Tables["数据点"].Rows.Add(new string[] { "2013", "789", "这是附加文字", "" });
            dsceshi.Tables["参数"].Rows.Add(new string[] { "日期", "年", "中标单价", "元", "否" });
            */


            SetLoad(false);
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //依据标志位控件的位置和大小(Lyiju)(Lyiju2)(yhBchart1)，画红线
            //画布
            Graphics g = e.Graphics;

            //设置抗锯齿
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            //设置背景色
            g.Clear(Color.Black);


            //画Lyiju标志位的红线,依据这个标志的X轴位置，画他左边的红线，依据这个标志位的宽度，画他右边的红线

            int Lyiju_X = Lyiju.Location.X;
            int Lyiju_width = Lyiju.Width;
            int p2_height = panel2.Height;
            g.DrawLine(pen_bjline, Lyiju_X - 10, 0, Lyiju_X - 10, p2_height);
            g.DrawLine(pen_bjline, Lyiju_X + Lyiju_width + 10, 0, Lyiju_X + Lyiju_width + 10, p2_height);

            //画yhBchart1标志位的红线,依据这个标志的Y位置和高度，画他下边的红线，通到Lyiju标志位左边的红线
            int yhBchart1_Y = yhBchart1.Location.Y;
            int yhBchart1_height = yhBchart1.Height;
            g.DrawLine(pen_bjline, 0, yhBchart1_Y + yhBchart1_height + 10, Lyiju_X - 10, yhBchart1_Y + yhBchart1_height + 10);

            //画Lyiju2标志位的红线,依据这个标志的Y位置，画他上边的红线，通到Lyiju标志位左边的红线
            int Lyiju2_Y = Lyiju2.Location.Y;
            g.DrawLine(pen_bjline, 0, Lyiju2_Y - 10, Lyiju_X - 10, Lyiju2_Y - 10);

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (DBspxq == null)
            {
                return;
            }
            this.linkLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

            this.linkLabel2.BackColor = System.Drawing.Color.Black;
            this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel2.LinkColor = System.Drawing.Color.White;

            this.linkLabel3.BackColor = System.Drawing.Color.Black;
            this.linkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel3.LinkColor = System.Drawing.Color.White;

            DataSet dsceshi = yhBchart1.initReturnDataSet();
            dsceshi.Tables.Remove("数据点");

            dsceshi.Tables.Add(DBspxq.Tables["数据点_即时"].Copy());
            dsceshi.Tables["数据点_即时"].TableName = "数据点";
            dsceshi.Tables["参数"].Rows.Add(new string[] { "中标时间", "", "中标价格", "元", "否" });

            yhBchart1.ShowDB(dsceshi);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (DBspxq == null)
            {
                return;
            }
            this.linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel2.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

            this.linkLabel1.BackColor = System.Drawing.Color.Black;
            this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel1.LinkColor = System.Drawing.Color.White;

            this.linkLabel3.BackColor = System.Drawing.Color.Black;
            this.linkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel3.LinkColor = System.Drawing.Color.White;

            DataSet dsceshi = yhBchart1.initReturnDataSet();
            dsceshi.Tables.Remove("数据点");

            dsceshi.Tables.Add(DBspxq.Tables["数据点_三个月"].Copy());
            dsceshi.Tables["数据点_三个月"].TableName = "数据点";
            dsceshi.Tables["参数"].Rows.Add(new string[] { "中标时间", "", "中标价格", "元", "否" });

            yhBchart1.ShowDB(dsceshi);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (DBspxq == null)
            {
                return;
            }
            this.linkLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.linkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel3.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

            this.linkLabel2.BackColor = System.Drawing.Color.Black;
            this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel2.LinkColor = System.Drawing.Color.White;

            this.linkLabel1.BackColor = System.Drawing.Color.Black;
            this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel1.LinkColor = System.Drawing.Color.White;

            DataSet dsceshi = yhBchart1.initReturnDataSet();
            dsceshi.Tables.Remove("数据点");

            dsceshi.Tables.Add(DBspxq.Tables["数据点_一年"].Copy());
            dsceshi.Tables["数据点_一年"].TableName = "数据点";
            dsceshi.Tables["参数"].Rows.Add(new string[] { "中标时间", "", "中标价格", "元", "否" });

            yhBchart1.ShowDB(dsceshi);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 加载经验交流菜单
        /// </summary>
        private void Init_FL()
        {
 
            DataTable auto1 = new DataTable();
            auto1.TableName = "交流";
            auto1.Columns.Add("标示", typeof(string));
            auto1.Columns.Add("显示文字", typeof(string));
            auto1.Columns.Add("网址", typeof(string));
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/mywork/JYPT/";
            auto1.Rows.Add("JL_001", "商品俗称", url + "JYPT_SHJYJL_SPSC.aspx");
            auto1.Rows.Add("JL_002", "商品描述", url + "JYPT_SHJYJL_SPMS.aspx");
            auto1.Rows.Add("JL_003", "验收方法", url + "JYPT_SHJYJL_YSBZ.aspx");
            auto1.Rows.Add("JL_004", "质量标准", url + "JYPT_SHJYJL_ZLBZ.aspx");
 
            DataRow[] drRoot_0 = auto1.Select("1=1");
            //清理内容
            TLB_fenlei.Controls.Clear();
            //设置数量并开始生成菜单
            int MaxFLnum = 6; //显示的数量
            if (drRoot_0.Length < MaxFLnum)
            {
                TLB_fenlei.ColumnCount = drRoot_0.Length * 3;
            }
            else
            {
                TLB_fenlei.ColumnCount = MaxFLnum * 3;
            }
            int flnum = 0;//用来处理 唯一值
            for (int p = 0; p < TLB_fenlei.ColumnCount; p++)
            {
                string fl_number = drRoot_0[flnum]["标示"].ToString();
                string fl_name = drRoot_0[flnum]["显示文字"].ToString();
                //左边图片
                PictureBox pb1 = new PictureBox();
                pb1.Margin = new System.Windows.Forms.Padding(0);
                pb1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                pb1.TabStop = false;
                pb1.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\qz.jpg");
                pb1.Name = "FL" + fl_number + "-1";
                pb1.Tag = drRoot_0[flnum];
                pb1.Cursor = System.Windows.Forms.Cursors.Hand;
                pb1.Click += new System.EventHandler(fenleishow);

                //中间文字区域
                Label ll = new Label();
                ll.AutoSize = true;
                ll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(149)))), ((int)(((byte)(149)))));
                ll.ForeColor = Color.Black;
                ll.Margin = new System.Windows.Forms.Padding(0);
                ll.Text = fl_name;
                ll.Tag = drRoot_0[flnum];
                ll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                ll.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
                ll.Name = "FL" + fl_number + "-2";
                ll.Cursor = System.Windows.Forms.Cursors.Hand;
                ll.Click += new System.EventHandler(fenleishow);

                //右边图片
                PictureBox pb2 = new PictureBox();
                pb2.Margin = new System.Windows.Forms.Padding(0);
                pb2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                pb2.TabStop = false;
                pb2.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\zy.jpg");
                pb2.Name = "FL" + fl_number + "-3";
                pb2.Tag = drRoot_0[flnum];
                pb2.Cursor = System.Windows.Forms.Cursors.Hand;
                pb2.Click += new System.EventHandler(fenleishow);

                //放到控件中
                TLB_fenlei.Controls.Add(pb1, p, 0);
                p++;
                TLB_fenlei.Controls.Add(ll, p, 0);
                p++;
                TLB_fenlei.Controls.Add(pb2, p, 0);
                flnum++;
            }
        }

        ///<summary> 
        ///在winform中查找控件 
        ///</summary> 
        ///<param >父控件</param> 
        ///<param >控件名称</param> 
        ///<returns></returns> 
        private System.Windows.Forms.Control findControl(System.Windows.Forms.Control control, string controlName)
        {
            Control c1;
            foreach (Control c in control.Controls)
            {
                if (c.Name == controlName)
                {
                    return c;
                }
                else if (c.Controls.Count > 0)
                {
                    c1 = findControl(c, controlName);
                    if (c1 != null)
                    {
                        return c1;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 显示详细分类的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fenleishow(object sender, EventArgs e)
        {

            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();



            //获得当前分类ID
            string typestr = sender.GetType().Name;
            DataRow tagDR = null;
            if (typestr == "Label")
            {
                tagDR = (DataRow)(((Label)sender).Tag);
                 
            }
            if (typestr == "PictureBox")
            {
                tagDR = (DataRow)(((PictureBox)sender).Tag);
            }
            //获取当前控件
            PictureBox dj1 = (PictureBox)findControl(TLB_fenlei, "FL" + tagDR["标示"].ToString() + "-1");
            Label dj2 = (Label)findControl(TLB_fenlei, "FL" + tagDR["标示"].ToString() + "-2");
            PictureBox dj3 = (PictureBox)findControl(TLB_fenlei, "FL" + tagDR["标示"].ToString() + "-3");
 
            //更改当前分类颜色
            foreach (Control c in TLB_fenlei.Controls)
            {
                if (c.GetType().Name == "Label" || c.GetType().Name == "PictureBox")
                {
                    if (c.Name.Split('-')[1] == "1")
                    {
                        ((PictureBox)c).Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\qz.jpg"); ;
                    }
                    if (c.Name.Split('-')[1] == "2")
                    {
                        ((Label)c).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(149)))), ((int)(((byte)(149)))));
                        ((Label)c).ForeColor = Color.Black;
                    }
                    if (c.Name.Split('-')[1] == "3")
                    {
                        ((PictureBox)c).Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\zy.jpg"); ;
                    }
                }
            }
            dj1.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\hz.jpg");
            dj2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(1)))), ((int)(((byte)(0)))));
            dj2.ForeColor = Color.White;
            dj3.Image = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\hy.jpg");


            string url = tagDR["网址"].ToString() + "?spbh=" + sSPBH + "&dlyx=" + DLYX;
            #region 验收货经验交流页面，如果同种页面已经打开，则让他获取焦点，否则打开新页面
            Form spxqll = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name.ToString() == "FormPrdJYJL")
                {
                    spxqll = f;
                    break;

                }
                else
                {
                }
            }

            if (spxqll != null && !spxqll.IsDisposed && spxqll.IsHandleCreated)
            {
                if (spxqll.WindowState == FormWindowState.Minimized)
                {
                    spxqll.WindowState = FormWindowState.Normal;
                }//设置窗体状态
                spxqll.Activate();
                ((FormPrdJYJL)spxqll).reLoadUrl(url);
            }
            else
            {
                FormPrdJYJL prdJYJL = new FormPrdJYJL(sSPBH, url);

                prdJYJL.StartPosition = FormStartPosition.CenterScreen;
                prdJYJL.ShowInTaskbar = true;
                prdJYJL.Show();
            }
            #endregion


        }

 

        //打开图册
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
        }

        bool gogo = false;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (gogo)
            {
                timer1.Enabled = false;
                BeginShowDB(sSPBH,sHTQX,"");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //if (panelLoad.Height > 0)
            //{
            //    panelLoad.Height = panelLoad.Height - 7000;
            //}
            //else
            //{
            //    panelLoad.Visible = false;
            //    panelLoad.Height = panel2.Height;
            //    timer2.Enabled = false;
            //}

            panelLoad.Visible = false;
            panelLoad.Height = panel2.Height;
            timer2.Enabled = false;
     
        }

    








 


 
      

     
  










































    }



}
