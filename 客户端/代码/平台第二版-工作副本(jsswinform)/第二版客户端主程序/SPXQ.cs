
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Com.Seezt.Skins;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources.Appearance;
using ChartSamplesExplorerCS.LabelsTitlesTooltips;
using 客户端主程序.Support;
using 客户端主程序.NewDataControl;
using System.Runtime.InteropServices;
using 客户端主程序.DataControl;

namespace 客户端主程序
{
    public partial class SPXQ : BasicForm
    {

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;

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

        //淡出计时器
        //System.Windows.Forms.Timer Timer_DC;
        string SPBH = "";
        string HTQX = "";
        public SPXQ(string spbh,string htqx)
        {
  
            //this.TitleYS = new int[] { 0, 0, -50 };

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            InitializeComponent();

            //双缓冲
            SHC(panelLoad);

            SPBH = spbh;
            HTQX = htqx;

            UPUP();

    
        }






        #region 自选商品的添加和删除
        /// <summary>
        /// 启动操作自选商品线程
        /// </summary>
        /// <param name="SPBH"></param>
        /// <param name="edit">加入,删除</param>
        private void SetZXSP(string SPBH, string edit)
        {
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["商品编号"] = SPBH;//
            InPutHT["操作"] = edit;//

            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResultZXSP);
            DataControl.RunThreadClassTest RCT = new DataControl.RunThreadClassTest(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.BeginRunZXSPedit));
            trd.Name = "自选商品操作";
            trd.IsBackground = true;
            trd.Start();

        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResultZXSP(Hashtable OutPutHT)
        {
            try
            {

                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResultZXSP_Invoke), new Hashtable[] { OutPutHT });

            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }


        //处理非线程创建的控件
        private void ShowThreadResultZXSP_Invoke(Hashtable OutPutHT)
        {

            string re = OutPutHT["执行结果"].ToString();

            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(re);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "测试弹窗", Almsg3);
            FRSE3.ShowDialog();

        }


        #endregion

        /// <summary>
        /// 页面滚动条强制置顶
        /// </summary>
        public void UPUP()
        {
            TextBox tb = new TextBox();
            this.Controls.Add(tb);
            tb.Location = new Point(-5000, -5000);
            tb.TabIndex = 0;
            tb.Focus();
            //this.Controls.Remove(tb);
        }

        Hashtable ht = new Hashtable();
        private void SPXQ_Load(object sender, EventArgs e)
        {
            //Init_one_show();

           
            PBload.Visible = true;
            PBload.Location = new Point(PBload.Location.X,290);
            if (SPBH == "" || HTQX == "")
            { return; }
            this.Text = "商品详情[" + SPBH +"*"+HTQX+ "]";
            //填充传入参数哈希表
            Hashtable InPutHT = new Hashtable();
            InPutHT["商品编号"] = SPBH;//
            InPutHT["合同期限"] = HTQX;//
            DataTable td = StringOP.GetDataTableFormHashtable(InPutHT);

            /*
            
            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult);
            DPSPXQ xq = new DPSPXQ(td, tempDFT);
            Thread trd = new Thread(new ThreadStart(xq.BeginRun));
            ht.Add("加载查看详情数据", trd);
            trd.Name = "加载查看详情数据|" + Guid.NewGuid().ToString();
            trd.IsBackground = true;
            trd.Start();
            */


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

        //双缓冲
        private void SHC(Control ct)
        {
            foreach (Control ctin in ct.Controls)
            {
                ctin.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(ctin, true, null);
                SHC(ctin);
            }
            
        }

        //处理非线程创建的控件
        private void ShowThreadResult_Invoke(Hashtable OutPutHT)
        {
     

                panelLoad.Enabled = true;


                DataSet dstest = (DataSet)(OutPutHT["返回值"]);

                if (dstest != null && dstest.Tables[0].Rows.Count >= 1)
                {
                    switch (dstest.Tables["返回值单条"].Rows[0]["执行结果"].ToString())
                    {
                        case "ok":
                            //禁止pnl重绘,这里比较特殊，一定要由这个东西
                            SendMessage(panelLoad.Handle, WM_SETREDRAW, 0, IntPtr.Zero); 
                            if (dstest.Tables["价格走势"] != null && dstest.Tables["价格走势"].Rows.Count > 0)
                            {
                                this.ultraChart1.Data.DataSource = dstest.Tables["价格走势"].DefaultView;
                                this.ultraChart1.Data.DataBind();
                                //行列转换
                                this.ultraChart1.Data.SwapRowsAndColumns = true;
                                //鼠标悬浮在节点时，显示的内容
                                Hashtable labelHash = new Hashtable();
                                labelHash.Add("HIGHLOW", new MyCustomTooltip());
                                this.ultraChart1.LabelHash = labelHash;
                                this.ultraChart1.Tooltips.Format = TooltipStyle.Custom;
                                this.ultraChart1.Tooltips.FormatString = "<HIGHLOW>";
                            }
                            else
                            {
                                DataTable table = new DataTable();

                                table.Columns.AddRange(
                                        new DataColumn[] { new DataColumn("time", System.Type.GetType("System.String")),
                                    new DataColumn("即时", System.Type.GetType("System.Int32")),
                                    new DataColumn("三个月", System.Type.GetType("System.Int32")),
                                    new DataColumn("一年", System.Type.GetType("System.Int32")),
                          });
                                table.Rows.Add(new object[] { "2008", DBNull.Value, DBNull.Value, DBNull.Value });
                                this.ultraChart1.Data.DataSource = table;
                                this.ultraChart1.Data.DataBind();
                                //行列转换
                                this.ultraChart1.Data.SwapRowsAndColumns = true;
                            }
                            this.ultraChart1.LineChart.NullHandling = (NullHandling)System.Enum.Parse(typeof(NullHandling), "InterpolateSimple");//Zero、DontPlot、InterpolateSimple


                            //商品详情
                            DataTable dtXQ = dstest.Tables["商品详情"];
                            if (dtXQ != null && dtXQ.Rows.Count > 0)
                            {
                                lblXKTTitle.Text = dtXQ.Rows[0]["商品名称"].ToString() + "商品价格历史价格走势图（单位：元）";

                                lblSPBH.Text = dtXQ.Rows[0]["商品编号"].ToString();
                                lblSPMC.Text = dtXQ.Rows[0]["商品名称"].ToString();
                                lblXHGG.Text = dtXQ.Rows[0]["型号规格"].ToString();
                                lblJJDW.Text = dtXQ.Rows[0]["计价单位"].ToString();
                                txtSPMS.Text = dstest.Tables["商品描述"].Rows[0]["商品描述"].ToString();
                                txtSPMS.ForeColor = Color.White;
                                lblDQTBLC.Text = dtXQ.Rows[0]["当前投标轮次"].ToString();
                                lblJBZT.Text = dtXQ.Rows[0]["竞标状态"].ToString();
                                lblHTZQ.Text = dtXQ.Rows[0]["合同周期n"].ToString();
                                lblSLDBJ.Text = dtXQ.Rows[0]["上轮定标价"].ToString();
                                lblDQMCZDJ.Text = dtXQ.Rows[0]["当前卖家最低价"].ToString();
                                lblSJF.Text = dtXQ.Rows[0]["升降幅"].ToString();
                                lblMRFDQSL.Text = dtXQ.Rows[0]["买家当前数量"].ToString();
                                lblMCFDQSL.Text = dtXQ.Rows[0]["卖家当前数量"].ToString();
                                lblMRFJRXZSL.Text = dtXQ.Rows[0]["买家今日新增数量"].ToString();
                                lblMCFJRXZSL.Text = dtXQ.Rows[0]["卖家今日新增数量"].ToString();
                                lblMRFQYFGL.Text = dtXQ.Rows[0]["买家区域覆盖率"].ToString();
                                lblMCFQYFGL.Text = dtXQ.Rows[0]["卖家区域覆盖率"].ToString();

                                labelZDJSPTitle.Text = "当前最低价标的" + dtXQ.Rows[0]["商品名称"].ToString() + "商品信息";
                                labelDQMRZGJ.Text = dtXQ.Rows[0]["当前买家最高价"].ToString();
                                labelDQMCZDJ.Text = dtXQ.Rows[0]["当前卖家最低价"].ToString();
                                labelJJPL.Text = dtXQ.Rows[0]["最低价标的经济批量"].ToString();
                                labelTBNSL.Text = dtXQ.Rows[0]["最低价标的投标拟售量"].ToString();
                                labelYDL.Text = dtXQ.Rows[0]["当前集合预订量"].ToString();
                                labelYDZL40.Text = dtXQ.Rows[0]["当前拟订购总量"].ToString();
                                labelDCL.Text = dtXQ.Rows[0]["达成率/中标率"].ToString();
                                labelRJZGFHL.Text = dtXQ.Rows[0]["最低价标的日均最高供货量"].ToString();
                                labelGHZQ.Text = dtXQ.Rows[0]["合同期限"].ToString();
                                ultraLabelSPCD.Text = dtXQ.Rows[0]["当前商品产地"].ToString();

                                //卖家信息
                                DataTable dtSelXX = dstest.Tables["卖家信息"];
                                if (dtSelXX != null && dtSelXX.Rows.Count > 0)
                                {
                                    labelSelMC.Text = dtSelXX.Rows[0]["卖出方名称"].ToString();//卖出方名称
                                    if (!dtSelXX.Rows[0]["税率证明"].ToString().Equals(""))
                                    {
                                        this.lab_shuilv.Text = dtSelXX.Rows[0]["税率证明"].ToString() + "%";
                                    }
                                    else
                                    {
                                        this.lab_shuilv.Text = "0%";
                                    }
                                    Image[] imageCollectio = JYFXYMX.GetXYImages(Convert.ToInt32(dtSelXX.Rows[0]["账户当前信用分值"]));
                                    if (imageCollectio == null)//该用户信用分数为“0”或者“负数”
                                    {
                                    }
                                    else
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

                                    string[] GHQY = dtSelXX.Rows[0]["供货区域"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//供货区域
                                    DataTable dtBGHQY = dstest.Tables["不供货区域"];
                                    tableLayoutPanelGHQY.Controls.Clear();
                                    tableLayoutPanelGHQY.ColumnCount = 3;
                                    if ((GHQY.Length + dtBGHQY.Rows.Count) % 3 == 0)
                                    {
                                        tableLayoutPanelGHQY.RowCount = (GHQY.Length + dtBGHQY.Rows.Count) / 3;
                                    }
                                    else
                                    {
                                        tableLayoutPanelGHQY.RowCount = (GHQY.Length + dtBGHQY.Rows.Count) / 3 + 1;
                                    }

                                    int ct = 0;
                                    int bghqy = 0;
                                    for (int i = 0; i < tableLayoutPanelGHQY.RowCount; i++)
                                    {
                                        tableLayoutPanelGHQY.RowStyles[i].Height = 20;
                                        for (int j = 0; j < 3; j++)
                                        {
                                            #region//作废
                                            //tableLayoutPanelGHQY.ColumnStyles[j].Width = 115;
                                            //Label lblghqy = new Label();
                                            //lblghqy.Width = 130;
                                            //lblghqy.Height = 20;
                                            //lblghqy.ForeColor = Color.White;
                                            //lblghqy.Font = new Font(new FontFamily("Tahoma"), 9);
                                            //lblghqy.Text = GHQY[ct].ToString();
                                            //tableLayoutPanelGHQY.Controls.Add(lblghqy, j, i);                                         
                                            //ct++;
                                            //if (ct > GHQY.Length - 1)
                                            //{
                                            //    break;
                                            //}
                                            #endregion

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

                                                if (dtBGHQY != null && dtBGHQY.Rows.Count > 0)
                                                {
                                                    Label lblghqy = new Label();
                                                    lblghqy.Width = 130;
                                                    lblghqy.Height = 20;
                                                    lblghqy.ForeColor = Color.Gray;
                                                    lblghqy.Font = new Font(new FontFamily("Tahoma"), 9, FontStyle.Strikeout);
                                                    lblghqy.Text = dtBGHQY.Rows[bghqy]["p_namestr"].ToString();
                                                    tableLayoutPanelGHQY.Controls.Add(lblghqy, j, i);
                                                    bghqy++;
                                                    if (bghqy > dtBGHQY.Rows.Count - 1)
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
                                    tableLayoutPanelGHQY.Height = tableLayoutPanelGHQY.RowCount * 20 + 5;
                                    tableLayoutPanelGHQY.Width = 345;

                                }

                            }
                            ultraChart1.Visible = true;
                            PBload.Visible = false;
                           
                            //允许重绘pnl  
                            SendMessage(panelLoad.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                            panelLoad.Invalidate(true);
                            return;
                        case "err":
                            ArrayList Almsg7 = new ArrayList();
                            Almsg7.Add("");
                            Almsg7.Add(dstest.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                            FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "其他", "系统忙", Almsg7);
                            FRSE7.ShowDialog();
                            return;
                        default:
                            ArrayList Almsg4 = new ArrayList();
                            Almsg4.Add("");
                            FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "商品详情查询失败", Almsg4);
                            FRSE4.ShowDialog();

                            return;
                    }

                }
                else
                {
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add("系统繁忙，请稍后重试...");
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "商品详情查询失败", Almsg4);
                    FRSE4.ShowDialog();
                    this.Close();
                    panelLoad.Visible = true;
                    return;
                }
 
 
           
        }
 
        private void SPXQ_FormClosing(object sender, FormClosingEventArgs e)
        {
 
            //线程存在才执行  
            if (ht.ContainsKey("加载查看详情数据"))
            {
                Thread trd = (Thread)ht["加载查看详情数据"];
                ht.Clear(); //删除线程标记
                if (trd != null && trd.IsAlive)
                {
                    trd.Priority = ThreadPriority.Lowest;
                }
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetZXSP(SPBH, "加入");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //当前操作的商品编号 htzq
 
            string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();

            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/mywork/JYPT/JYPT_SHJYJL_YSBZ.aspx?spbh=" + SPBH + "&dlyx=" + DLYX;






            #region 验收货经验交流页面，如果同种页面已经打开，则让他获取焦点，否则打开新页面
            Form spxqll = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name.ToString() == "FormPrdJYJL")
                {
                    string[] spbharr = f.Text.Split('[')[1].Split(']');
                    if (spbharr[0].Trim().ToString() == SPBH.Trim())
                    {
                        spxqll = f;
                        break;
                    }
                    else
                    {
                    }

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
                //((FormPrdJYJL)spxqll).reLoadUrl();
            }
            else
            {
                FormPrdJYJL prdJYJL = new FormPrdJYJL(SPBH, url);

                prdJYJL.StartPosition = FormStartPosition.CenterScreen;
                prdJYJL.ShowInTaskbar = true;
                prdJYJL.Show();
            }
            #endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (linkLabel1.LinkColor == Color.LightCoral)
            {
                linkLabel1.LinkColor = Color.Yellow;
                linkLabel2.LinkColor = Color.Yellow;
            }
            else
            {
                linkLabel1.LinkColor = Color.LightCoral;
                linkLabel2.LinkColor = Color.LightCoral;
            }

        }
 
	}
    
}
