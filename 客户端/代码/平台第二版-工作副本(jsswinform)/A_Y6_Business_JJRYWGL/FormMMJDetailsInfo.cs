using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using 客户端主程序.DataControl;
using System.Threading;
using 客户端主程序.NewDataControl;
using 客户端主程序.SubForm.NewCenterForm;

namespace 客户端主程序
{
    public partial class FormMMJDetailsInfo : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;
        Hashtable HTuser;
        Hashtable HTLookFile;//保存图片路径
        string strSFYMJZWC = "未加载完成";
      

        //用来协助判断，用户所属平台管理机构的变量 
        string strTagSF = ""; //身份
        string strTagDS = "";//地市
      
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

        #region//窗体构造函数
        /// <summary>
        /// 开通交易账户构造函数重载
        /// </summary>
        /// <param name="HTuser_Copy"></param>
        public FormMMJDetailsInfo(Hashtable HTuser_Copy)
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //====================================================

            InitializeComponent();

            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;

            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;

            HTuser = HTuser_Copy;
            SRT_GetMMJData_Run();
        }

        private void FormMMJDetails_Load(object sender, EventArgs e)
        {
            cbxKHYH.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
          //隐藏控件的显示
            this.panZGZS.Visible = false;
            this.panJJRMC.Visible = false;
            this.panJJRLXDH.Visible = false;
            this.panGLJG.Visible = false;

            this.panZZJGDMZ.Visible = false;
            this.panSWDJZ.Visible = false;
            this.panNSRZGZ.Visible = false;
            this.panKHXKZ.Visible = false;
            this.panFDDBR.Visible = false;
            this.panFDDBRSFZH.Visible = false;
            this.panFDDBRSQS.Visible = false;

            this.panYYZZ.Visible = false;
            //初始化所属区域
             this.ucSSQY.initdefault();
            //初始化平台管理机构
             DataSet dataSet = PublicDS.PublisDsData;
             cbxGLJG.Items.Add("请选择平台管理机构");

             string[] distinctcols = new string[] { "FGSname" };
             DataTable dtfd = new DataTable("distinctTable");
             DataView mydataview = new DataView(dataSet.Tables["分公司对照表"]);
             dtfd = mydataview.ToTable(true, distinctcols);
             foreach (DataRow dr in dtfd.Rows)
             {
                 cbxGLJG.Items.Add(dr["FGSname"].ToString());
             }
             cbxGLJG.SelectedIndex = 0;
            //处理下拉框间距
            cbxGLJG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
        }

        //处理下拉框间距
        private void CB_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox CBthis = (ComboBox)sender;
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(CBthis.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X + 2, e.Bounds.Y + 3);
        }
        #endregion

        
        /// <summary>
        /// 绑定界面用户数据
        /// </summary>
        private void BindDataFaceData(DataSet dsReturn)
        {
            DataTable dataTableMMJ = dsReturn.Tables["买家卖家交易账户基本信息"];
            DataTable dataTableMMJ_JJR = dsReturn.Tables["买家卖家关联经纪人账户基本信息"];
            HTLookFile = new Hashtable();
            if (dataTableMMJ.Rows.Count > 0 && dataTableMMJ_JJR.Rows.Count>0)
            {
                if (dataTableMMJ.Rows[0]["B_JSZHLX"].ToString() == "买家卖家交易账户")
                {
                    this.radJJR.Checked = false;
                    this.radMMJ.Checked = true;
                }
                else
                {
                    this.radJJR.Checked = true;
                    this.radMMJ.Checked = false;
                }

                if (dataTableMMJ.Rows[0]["I_ZCLB"].ToString() == "单位")
                {
                    this.radDW.Checked = true;
                    this.radZRR.Checked = false;
                }
                else
                {
                    this.radDW.Checked = false;
                    this.radZRR.Checked = true;
                }

                this.txtJJFMC.Text = dataTableMMJ.Rows[0]["I_JYFMC"].ToString();
                this.txtYYZZ.Text = dataTableMMJ.Rows[0]["I_YYZZZCH"].ToString();
                HTLookFile["营业执照扫描件"] = dataTableMMJ.Rows[0]["I_YYZZSMJ"].ToString();
                this.txtSFZ.Text = dataTableMMJ.Rows[0]["I_SFZH"].ToString();
                HTLookFile["身份证扫描件"] = dataTableMMJ.Rows[0]["I_SFZSMJ"].ToString();
                HTLookFile["身份证反面扫描件"] = dataTableMMJ.Rows[0]["I_SFZFMSMJ"].ToString();
                this.txtZZJGDMZ.Text = dataTableMMJ.Rows[0]["I_ZZJGDMZDM"].ToString();
                HTLookFile["组织机构代码证扫描件"] = dataTableMMJ.Rows[0]["I_ZZJGDMZSMJ"].ToString();
                this.txtSWDJZ.Text = dataTableMMJ.Rows[0]["I_SWDJZSH"].ToString();
                HTLookFile["税务登记证扫描件"] = dataTableMMJ.Rows[0]["I_SWDJZSMJ"].ToString();
                HTLookFile["一般纳税人资格证明扫描件"] = dataTableMMJ.Rows[0]["I_YBNSRZGZSMJ"].ToString();
                this.txtKHXKZ.Text = dataTableMMJ.Rows[0]["I_KHXKZH"].ToString();
                HTLookFile["开户许可证扫描件"] = dataTableMMJ.Rows[0]["I_KHXKZSMJ"].ToString();
                HTLookFile["预留印鉴卡扫描件"] = dataTableMMJ.Rows[0]["I_YLYJK"].ToString();
                this.txtFDDBR.Text = dataTableMMJ.Rows[0]["I_FDDBRXM"].ToString();
                this.txtFDDBRSHZH.Text = dataTableMMJ.Rows[0]["I_FDDBRSFZH"].ToString(); 
                HTLookFile["法定代表人身份证扫描件"] = dataTableMMJ.Rows[0]["I_FDDBRSFZSMJ"].ToString();
                HTLookFile["法定代表人身份证反面扫描件"] = dataTableMMJ.Rows[0]["I_FDDBRSFZFMSMJ"].ToString();
                HTLookFile["法定代表人授权书扫描件"] = dataTableMMJ.Rows[0]["I_FDDBRSQS"].ToString();
                this.txtJYFLXDH.Text = dataTableMMJ.Rows[0]["I_JYFLXDH"].ToString();
                this.ucSSQY.SelectedItem = new string[] { dataTableMMJ.Rows[0]["I_SSQYS"].ToString(), dataTableMMJ.Rows[0]["I_SSQYSHI"].ToString(), dataTableMMJ.Rows[0]["I_SSQYQ"].ToString() };
                this.ucSSQY.EnabledItem = new bool[] {false,false,false };
                this.txtXXDZ.Text = dataTableMMJ.Rows[0]["I_XXDZ"].ToString();
                this.txtLXRXM.Text = dataTableMMJ.Rows[0]["I_LXRXM"].ToString();
                this.txtLXRSJH.Text = dataTableMMJ.Rows[0]["I_LXRSJH"].ToString();
                //this.txtKHYH.Text = dataTableMMJ.Rows[0]["I_KHYH"].ToString();
                this.cbxKHYH.SelectedItem = dataTableMMJ.Rows[0]["I_KHYH"].ToString();
                this.txtYHZH.Text = dataTableMMJ.Rows[0]["I_YHZH"].ToString();
                //绑定对应的平台管理机构数据
                if (dataTableMMJ.Rows[0]["I_PTGLJG"] != null && dataTableMMJ.Rows[0]["I_PTGLJG"].ToString() != "")
                {
                    string strFGSName = dataTableMMJ.Rows[0]["I_PTGLJG"].ToString();
                    for (int i = 0; i < cbxGLJG.Items.Count; i++)
                    {
                        string strSelectItem = cbxGLJG.Items[i].ToString();
                        if (strSelectItem == strFGSName)
                        {
                            cbxGLJG.SelectedIndex = i;
                        }
                    }
                    this.cbxGLJG.Enabled = false;
                }

                this.txtJJRZGZS.Text = dataTableMMJ_JJR.Rows[0]["J_JJRZGZSBH"].ToString();
                this.txtJJRMC.Text = dataTableMMJ_JJR.Rows[0]["I_JYFMC"].ToString();
                this.txtJJRLXDH.Text = dataTableMMJ_JJR.Rows[0]["I_JYFLXDH"].ToString();
                this.txtZLTJSJ.Text = dataTableMMJ.Rows[0]["资料提交时间"].ToString();
                //经纪人审核
                this.txtCSJL.Text = dataTableMMJ.Rows[0]["JJRSHZT"].ToString();
                if (dataTableMMJ.Rows[0]["JJRSHSJ"] is DBNull || dataTableMMJ.Rows[0]["JJRSHSJ"] == null)
                {
                    this.txtCSSJ.Text = "";
                }
                else
                {
                    this.txtCSSJ.Text = dataTableMMJ.Rows[0]["JJRSHSJ"].ToString();
                }
                this.txtCSYJ.Text = dataTableMMJ.Rows[0]["JJRSHYJ"].ToString();
                //分公司审核
                this.txtFSJL.Text = dataTableMMJ.Rows[0]["FGSSHZT"].ToString();
                if (dataTableMMJ.Rows[0]["FGSSHSJ"] is DBNull || dataTableMMJ.Rows[0]["FGSSHSJ"] == null)
                {
                    this.txtFSSJ.Text = "";
                }
                else
                {
                    this.txtFSSJ.Text = dataTableMMJ.Rows[0]["FGSSHSJ"].ToString();
                }
                this.txtFSYJ.Text = dataTableMMJ.Rows[0]["FGSSHYJ"].ToString();
                //根据界面的数据，决定显示哪些界面内容
                if (dataTableMMJ.Rows[0]["B_JSZHLX"].ToString() == "买家卖家交易账户")
                {
                    this.panZGZS.Visible = true;
                    this.panJJRMC.Visible = true;
                    this.panJJRLXDH.Visible = true;
                    this.panGLJG.Visible = false;
                }
                else
                {
                    this.panZGZS.Visible = false;
                    this.panJJRMC.Visible = false;
                    this.panJJRLXDH.Visible = false;
                    this.panGLJG.Visible = true;
                }
                if (dataTableMMJ.Rows[0]["I_ZCLB"].ToString() == "单位")
                {
                    this.panYYZZ.Visible = true;
                    this.panSFZ.Visible = false;
                    this.panZZJGDMZ.Visible = true;
                    this.panSWDJZ.Visible = true;
                   // this.panNSRZGZ.Visible = true;
                    this.panKHXKZ.Visible = true;
                    this.panYLYJK.Visible = true;
                    this.panFDDBR.Visible = true;
                    this.panFDDBRSFZH.Visible = true;
                    this.panFDDBRSQS.Visible = true;
                }
                else
                {
                    this.panYYZZ.Visible = false;
                    this.panSFZ.Visible = true;
                    this.panZZJGDMZ.Visible = false;
                    this.panSWDJZ.Visible = false;
                    this.panNSRZGZ.Visible = false;
                    this.panKHXKZ.Visible = false;
                    this.panYLYJK.Visible = false;
                    this.panFDDBR.Visible = false;
                    this.panFDDBRSFZH.Visible = false;
                    this.panFDDBRSQS.Visible = false;
                }



            }
            strSFYMJZWC = "页面加载完成";
        }
      

        #region//开启线程获取，界面数据并且绑定界面

        //开启一个线程提交数据
        private void SRT_GetMMJData_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(HTuser, new delegateForThread(SRT_GetMMJData));
            Thread trd = new Thread(new ThreadStart(OTD.GetMMJData));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetMMJData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetMMJData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetMMJData_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //绑定界面用户数据
                    BindDataFaceData(dsreturn);
                    panJJFMC.Focus();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
        }

        #endregion






        #region//对文件 查看 的处理

        ImageShow IS;

        /// <summary>
        /// 查看上传的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookFile(object sender, EventArgs e)
        {
            //查看按钮
            Label lb = sender as Label;
            string StrID = lb.Name;
            if (StrID != "")
            {
                string strURL = "";
                switch (StrID)
                {
                    case "lab_YYZZ_CK":
                        strURL = HTLookFile["营业执照扫描件"].ToString();
                        break;
                    case "lab_SFZ_CK":
                        strURL = HTLookFile["身份证扫描件"].ToString();
                        break;
                    case "labZZJGDMZ_CK":
                        strURL = HTLookFile["组织机构代码证扫描件"].ToString();
                        break;
                    case "lab_SWDJZ_CK":
                        strURL = HTLookFile["税务登记证扫描件"].ToString();
                        break;
                    case "lab_NSRZGZ_CK":
                        strURL = HTLookFile["一般纳税人资格证明扫描件"].ToString();
                        break;
                    case "lab_KHXKZ_CK":
                        strURL = HTLookFile["开户许可证扫描件"].ToString();
                        break;
                    case "lab_YLYJK_CK":
                        strURL = HTLookFile["预留印鉴卡扫描件"].ToString();
                        break;
                    case "lab_FDDBRSHZH_CK":
                        strURL = HTLookFile["法定代表人身份证扫描件"].ToString();
                        break;
                    case "lab_FDDBRSQS_CK":
                        strURL = HTLookFile["法定代表人授权书扫描件"].ToString();
                        break;
                    case "lab_SFZ_FM_CK":
                        strURL = HTLookFile["身份证反面扫描件"].ToString();
                        break;
                    case "lab_FDDBRSHZH_FM_CK":
                        strURL = HTLookFile["法定代表人身份证反面扫描件"].ToString();
                        break;
                    default:
                        break;

                }
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + strURL.Replace(@"\", "/");
                        //StringOP.OpenUrl(url);

                        Hashtable htT = new Hashtable();
                        htT["类型"] = "网址";
                        htT["地址"] = url;
                        if (IS == null || IS.IsDisposed == true)
                        {
                            IS = new ImageShow(htT);
                        }
                        IS.Show();
                        IS.Activate();
            }
        }
        #endregion
        /// <summary>
        /// 系统根据用户选择的所属区域帮助选择，相应的平台管理机构
        /// </summary>
        private void HelpSelectGLJG()
        { 
            if (!ucSSQY.SelectedItem[0].ToString().Contains("请选择") && !ucSSQY.SelectedItem[1].ToString().Contains("请选择"))
            {
                if (this.radJJR.Checked)
                {
                   
                    string strSSSF = ucSSQY.SelectedItem[0].ToString();
                    string strSSDS = ucSSQY.SelectedItem[1].ToString();
                    if (strSSSF != strTagSF && strSSDS != strTagDS)
                    {
                    DataSet dataSet = PublicDS.PublisDsData;
                  DataRow[] dataRow=dataSet.Tables["分公司对照表"].Select(" Pname ='" + strSSSF + "' and Cname='" + strSSDS + "'");
                  if (dataRow.Length > 0)
                  {
                      for (int i = 0; i < cbxGLJG.Items.Count; i++)
                      {
                          string strSelectItem = cbxGLJG.Items[i].ToString();
                          string strFGSName = dataRow[0]["FGSname"].ToString();
                          if (strSelectItem == strFGSName)
                          {
                              cbxGLJG.SelectedIndex = i;
                          }
                      }
                      strTagSF = ucSSQY.SelectedItem[0].ToString();
                      strTagDS = ucSSQY.SelectedItem[1].ToString();
                  }
                    }
                }

            }
        }

       

        private void cbxGLJG_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.radJJR.Checked)
            {
               
            }
                      
        }

       
       
      







      
       










    }
}
      