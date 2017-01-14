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
using 客户端主程序.SubForm;
using 客户端主程序.Support;
using 客户端主程序.NewDataControl;
using 客户端主程序.SubForm.NewCenterForm.MuBan;
using 客户端主程序.SubForm.NewCenterForm.GZZD;
using System.Runtime.InteropServices;

namespace 客户端主程序
{
    public partial class FormKTJYZH : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;
        Hashtable HTuser;
        Hashtable hashTableInfor = null;

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;

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
        /// 开通交易账户构造函数
        /// </summary>
        public FormKTJYZH()
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //====================================================

            InitializeComponent();

            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;

            //this.txtJJRZGZS.CanPASTE = false;
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;



        }

        /// <summary>
        /// 开通交易账户构造函数重载
        /// </summary>
        /// <param name="HTuser_Copy"></param>
        public FormKTJYZH(Hashtable HTuser_Copy)
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
            this.ShowInTaskbar = true;

            HTuser = HTuser_Copy;

        }
        private void FormKTJYZH_Load(object sender, EventArgs e)
        {
            //隐藏控件的显示
            this.panZGZS.Visible = false;
            this.panJJRMC.Visible = false;
            this.panJJRLXDH.Visible = false;
            this.panGLJG.Visible = false;

            this.panZZJGDMZ.Visible = false;
            this.panSWDJZ.Visible = false;
            this.panNSRZGZ.Visible = false;
            this.panKHXKZ.Visible = false;
            this.panYLYJK.Visible = false;
            this.panFDDBR.Visible = false;
            this.panFDDBRSHZH.Visible = false;
            this.panFDDBRSQS.Visible = false;

            this.panYYZZ.Visible = false;
            //设置加水印加字样
            this.pan_SC_YYZZ.ManageData = "加水印加字样";
            this.pan_SC_SFZ.ManageData = "加水印加字样";
            this.pan_SC_ZZJGDMZ.ManageData = "加水印加字样";
            this.pan_SC_SWDJZ.ManageData = "加水印加字样";

            this.pan_SC_NSRZGZ.ManageData = "加水印加字样";
            this.pan_SC_KHXKZ.ManageData = "加水印加字样";
            this.pan_SC_YLYJK.ManageData = "加水印加字样";
            this.pan_SC_FDDBRSHZH.ManageData = "加水印加字样";
            this.pan_SC_FDDBRSQS.ManageData = "加水印加字样";

            this.pan_SC_SFZ_FM.ManageData = "加水印加字样";
            this.pan_SC_FDDBRSHZH_FM.ManageData = "加水印加字样";
            //初始化所属区域
            this.ucSSQY.initdefault();
            //处理下拉框间距
            cbxGLJG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            //预指定开户银行
            cbxKHYH.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            //所在院系
            //cbxSZYX.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);

            cbxKHYH.SelectedIndex = 0;
            this.PBload.Location = new Point(this.flowLayoutPanel1.Location.X + this.panTiJiao.Location.X + this.btnSave.Location.X + this.btnSave.Width + 25, this.flowLayoutPanel1.Location.Y + this.panTiJiao.Location.Y + this.btnSave.Location.Y);


            //2013.12.06 wyh add 鼠标略过显示文字
            toolTip1.AutoPopDelay = 25000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 0;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.radFGS, "所属地服务中心，会为您提供及时服务！");
            toolTip1.SetToolTip(this.radYWTZB, "银行经纪人和富美集团总部员工请选择“平台总部” ");
            toolTip1.SetToolTip(this.radGXTW, "参加“全国大学生课余创业实践活动”的高校师生请选择“高校团委”"); 
            


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

        #region//调整界面

        /// <summary>
        /// 选择经纪人交易账户（此时界面上不出现，您的经纪人资格证书编号、您的经纪人名称、您的经纪人联系电话）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radJJR_CheckedChanged(object sender, bool Checked)
        {
            //禁止pnl重绘,这里比较特殊，一定要由这个东西
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            if (Checked == true)
            {
                this.panZGZS.Visible = false;
                this.panJJRMC.Visible = false;
                this.panJJRLXDH.Visible = false;

                #region//20140421--周丽修改--因分公司撤销，交易平台现阶段相关模块 去掉业务管理部门
                this.panGLJG.Visible = false;

                //this.panGLJG.Visible = true;
                this.cbxGLJG.Items.Clear();
                DataSet dataSet = PublicDS.PublisDsData;
                cbxGLJG.Items.Add("请选择业务管理部门");

                string[] distinctcols = new string[] { "FGSname" };
                DataTable dtfd = new DataTable("distinctTable");
                DataView mydataview = new DataView(dataSet.Tables["分公司对照表"]);
                dtfd = mydataview.ToTable(true, distinctcols);
                DataRow[] dataRows = dtfd.Select("FGSname like '%分公司%'");
                foreach (DataRow dr in dataRows)
                {
                    cbxGLJG.Items.Add(dr["FGSname"].ToString());
                }
                cbxGLJG.SelectedIndex = 0;
                #endregion
                this.timer_PTGLJG.Enabled = true;
                SetRemindInforInVisiable();
                this.linkZHKTXY.Visible = true;
                this.linkZHKTXY.Text = "交易账户经纪人开通协议";
                this.CB_TYXY.Visible = true;
                //请选择业务管理部门分类
                this.panGLBMFL.Visible = true;
                this.panYWFWBM.Visible = false;
                this.panGLYH.Visible = false;
                this.panBankYGGH.Visible = false;
                this.panGLZFJG.Visible = false;
                this.radFGS.Checked = true;
                this.radYWTZB.Checked = false;
                this.radGXTW.Checked = false;
                this.panHYXH.Visible = false;

            }
            //允许重绘pnl  
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            flowLayoutPanel1.Invalidate(true);
        }
        /// <summary>
        /// 买卖家交易账户（此时界面上不出现，平台管理机构）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radMMJ_CheckedChanged(object sender, bool Checked)
        {
            //禁止pnl重绘,这里比较特殊，一定要由这个东西
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            if (Checked == true)
            {
                this.panZGZS.Visible = true;
                this.panJJRMC.Visible = true;
                this.panJJRLXDH.Visible = true;
                this.panGLJG.Visible = false;
                this.timer_PTGLJG.Enabled = false;
                SetRemindInforInVisiable();
                this.linkZHKTXY.Visible = true;
                this.linkZHKTXY.Text = "交易方账户开通协议";
                this.CB_TYXY.Visible = true;
                this.panGLBMFL.Visible = false;
                this.panTWGLBM.Visible = false;
                this.panYWFWBM.Visible = true;
                this.radFW_JJR.Checked = true;
                this.radFW_Bank.Checked = false;
                this.radFW_ZF.Checked = false;
                this.panSZYX.Visible = false;
                this.radHangYeXieHui.Checked = false;
            }
            //允许重绘pnl  
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            flowLayoutPanel1.Invalidate(true);
        }
        /// <summary>
        /// 同意协议处理以后的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_TYXY_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CB_TYXY.Checked == true)
            {
                this.btnSave.Enabled = true;
            }
            else
            {
                this.btnSave.Enabled = false;
            }
        }


        /// <summary>
        /// 单位名称（根据其选中情况，调整界面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radDW_CheckedChanged(object sender, bool Checked)
        {
            //禁止pnl重绘,这里比较特殊，一定要由这个东西
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            if (Checked == true)
            {
                this.txtJJFMC.Text = "";
                this.txtJYFLXDH.Text = "";
                this.txtXXDZ.Text = "";
                this.txtLXRXM.Text = "";
                this.txtLXRSJH.Text = "";
                this.txtZQZJMM.Text = "";
                string str = this.txtJJFMC.Text;
                this.txtJJFMC.TextNtip = "请填写单位全称，与您在银行开户时的账户企业名称一致";
                if (this.txtXXDZ.Text.Trim() == "")
                { this.txtXXDZ.TextNtip = "请不要填写省市区信息"; }
                if (str.Trim() != "")
                {
                    this.txtJJFMC.Text = str;
                }
                this.panYYZZ.Visible = true;
                this.panZZJGDMZ.Visible = true;
                this.panSWDJZ.Visible = true;
                // this.panNSRZGZ.Visible = true;
                this.panNSRZGZ.Visible = false;
                this.panKHXKZ.Visible = true;
                this.panYLYJK.Visible = true;
                this.panFDDBR.Visible = true;
                this.panFDDBRSHZH.Visible = true;
                this.panFDDBRSQS.Visible = true;
                this.panSFZ.Visible = false;
                SetRemindInforInVisiable();
                this.labJYFMC_TSXX.Text = "交易方名称须与《组织机构代码证》的单位全称完全一致，否则交易账户将无法与银行的第三方存管账户绑定，不能出、入金！";
                this.panJYFMC_TSXX.Visible = true;


            }
            //允许重绘pnl  
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            flowLayoutPanel1.Invalidate(true);
        }
        /// <summary>
        /// 自然人（根据其选中情况，调整界面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radZRR_CheckedChanged(object sender, bool Checked)
        {
            //禁止pnl重绘,这里比较特殊，一定要由这个东西
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            if (Checked == true)
            {
                this.txtJJFMC.Text = "";
                this.txtJYFLXDH.Text = "";
                this.txtXXDZ.Text = "";
                this.txtLXRXM.Text = "";
                this.txtLXRSJH.Text = "";
                this.txtZQZJMM.Text = "";
                string str = this.txtJJFMC.Text;
                this.txtJJFMC.TextNtip = "请输入自然人姓名";
                if (this.txtXXDZ.Text.Trim() == "")
                { this.txtXXDZ.TextNtip = "请不要填写省市区信息"; }
                if (str.Trim() != "")
                {
                    this.txtJJFMC.Text = str;
                }
                this.panYYZZ.Visible = false;
                this.panSFZ.Visible = true;
                this.panZZJGDMZ.Visible = false;
                this.panSWDJZ.Visible = false;
                this.panNSRZGZ.Visible = false;
                this.panKHXKZ.Visible = false;
                this.panYLYJK.Visible = false;
                this.panFDDBR.Visible = false;
                this.panFDDBRSHZH.Visible = false;
                this.panFDDBRSQS.Visible = false;

                SetRemindInforInVisiable();
                this.labJYFMC_TSXX.Text = "交易方名称须与《身份证》的自然人姓名完全一致，否则交易账户将无法与银行的第三方存管账户绑定，不能出、入金！";
                this.panJYFMC_TSXX.Visible = true;
            }
            //允许重绘pnl  
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            flowLayoutPanel1.Invalidate(true);


        }
        #endregion







        #region//对文件上传、查看、删除的处理
        /*
        /// <summary>
        ///上传文件
        /// </summary>
        private void UpLoadFile(object sender, EventArgs e)
        {
            //开启上传
            //根据控件ID判断数据存储到那个listView
            Label lb = sender as Label;
            string StrID = lb.Name;
            if (StrID != "")
            {
                OpenFileDialog ofd = null;
                ListView lv = null;
                    ofd = openFileDialog1;
                switch (StrID)
                {
                    case "lab_YYZZ_SC":
                        lv = listYYZZ;
                        break;
                    case "lab_SFZ_SC":
                        lv = listSFZH;
                        break;
                    case "labZZJGDMZ_SC":
                        lv = listZZJGDMZ;
                        break;
                    case "lab_SWDJZ_SC":
                        lv = listSWDJZ;
                        break;
                    case "lab_NSRZGZ_SC":
                        lv = listNSZGZ;
                        break;
                    case "lab_KHXKZ_SC":
                        lv = listKHXKZ;
                        break;
                    case "lab_FDDBRSHZH_SC":
                        lv = listFDDBR;
                        break;
                    case "lab_FDDBRSQS_SC":
                        lv = listFDDBRSQS;
                        break;
                    default:
                        break;

                }

                if (ofd != null && lv != null)
                {

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string[] fName = ofd.FileNames;


                        //若选择选择对话框允许选择多个，则还要检验不能超过5个
                        //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                        // 开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                        FormSC FSC = new FormSC(fName, lv, new delegateForSC(UpLoadSucceed), "结算账户开通");
                        FSC.ShowDialog();

                    }
                }



            }
        
        
        }
        /// <summary>
        /// 处理上传完以后的控件，查看删除是否显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_SCCK_Tick(object sender, EventArgs e)
        {
            //根据列表中是否已存数据确定显示"营业执照查看，删除"按钮与否

            if (listYYZZ != null && listYYZZ.Items.Count > 0)
            {
                lab_YYZZ_CK.Visible = true;
                lab_YYZZ_Del.Visible = true;
            }
            else
            {
                lab_YYZZ_CK.Visible = false;
                lab_YYZZ_Del.Visible = false;
            }

            //根据列表中是否已存数据确定显示"身份很证查看·删除"按钮与否
            if (listSFZH != null && listSFZH.Items.Count > 0)
            {
                lab_SFZ_CK.Visible = true;
                lab_SFZ_Del.Visible = true;
            }
            else
            {
                lab_SFZ_CK.Visible = false;
                lab_SFZ_Del.Visible = false;
            }

            //根据列表中是否已存数据确定显示"组织机构代码证查看·删除"按钮与否

            if (listZZJGDMZ != null && listZZJGDMZ.Items.Count > 0)
            {
                labZZJGDMZ_CK.Visible = true;
                labZZJGDMZ_Del.Visible = true;
            }
            else
            {
                labZZJGDMZ_CK.Visible = false;
                labZZJGDMZ_Del.Visible = false;
            }

            //根据列表中是否已存数据确定显示"税务登记证查看·删除"按钮与否
            if (listSWDJZ != null && listSWDJZ.Items.Count > 0)
            {
                lab_SWDJZ_CK.Visible = true;
                lab_SWDJZ_Del.Visible = true;
            }
            else
            {
                lab_SWDJZ_CK.Visible = false;
                lab_SWDJZ_Del.Visible = false;
            }

            //一般纳税人资格证
            if (listNSZGZ != null && listNSZGZ.Items.Count > 0)
            {
                lab_NSRZGZ_CK.Visible = true;
                lab_NSRZGZ_Del.Visible = true;
            }
            else
            {
                lab_NSRZGZ_CK.Visible = false;
                lab_NSRZGZ_Del.Visible = false;
            }

            //开户许可证
            if (listKHXKZ != null && listKHXKZ.Items.Count > 0)
            {
                lab_KHXKZ_CK.Visible = true;
                lab_KHXKZ_Del.Visible = true;
            }
            else
            {
                lab_KHXKZ_CK.Visible = false;
                lab_KHXKZ_Del.Visible = false;
            }
            //根据列表中是否已存数据确定显示"法定代表人身份证查看删除"按钮与否
            if (listFDDBR.Items.Count > 0)
            {
                lab_FDDBRSHZH_CK.Visible = true;
                lab_FDDBRSHZH_Del.Visible = true;
            }
            else
            {
                lab_FDDBRSHZH_CK.Visible = false;
                lab_FDDBRSHZH_Del.Visible = false;
            }

            //根据列表中是否已存数据确定显示"法定代表人授权书查看删除"按钮与否
            if (listFDDBRSQS.Items.Count > 0)
            {
                lab_FDDBRSQS_CK.Visible = true;
                lab_FDDBRSQS_Del.Visible = true;
            }
            else
            {
                lab_FDDBRSQS_CK.Visible = false;
                lab_FDDBRSQS_Del.Visible = false;
            }
        }

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
                ListView lv = null;
                switch (StrID)
                {
                    case "lab_YYZZ_CK":
                        lv = listYYZZ;
                        break;
                    case "lab_SFZ_CK":
                        lv = listSFZH;
                        break;
                    case "labZZJGDMZ_CK":
                        lv = listZZJGDMZ;
                        break;
                    case "lab_SWDJZ_CK":
                        lv = listSWDJZ;
                        break;
                    case "lab_NSRZGZ_CK":
                        lv = listNSZGZ;
                        break;
                    case "lab_KHXKZ_CK":
                        lv = listKHXKZ;
                        break;
                    case "lab_FDDBRSHZH_CK":
                        lv = listFDDBR;
                        break;
                    case "lab_FDDBRSQS_CK":
                        lv = listFDDBRSQS;
                        break;
                    default:
                        break;

                }

                if (lv != null)
                {
                    //有地址
                    if (lv.Items.Count > 0 && lv.Items[0].SubItems[1].Text.Trim() != "")
                    {
                        string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + lv.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                        StringOP.OpenUrl(url);
                    }

                }
            }
        
        
        }

        /// <summary>
        /// 删除上传的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteFile(object sender, EventArgs e)
        {
            //开启上传
            //根据控件ID判断数据存储到那个listView
            Label lb = sender as Label;
            string StrID = lb.Name;
            string deletetype = "";
            if (StrID != "")
            {
                ListView lv = null;
                switch (StrID)
                {
                    case "lab_YYZZ_Del":
                        lv = listYYZZ;
                        deletetype = "营业执照扫描件";
                        break;
                    case "lab_SFZ_Del":
                        lv = listSFZH;
                        deletetype = "身份证扫描件";
                        break;
                    case "labZZJGDMZ_Del":
                        lv = listZZJGDMZ;
                        deletetype = "组织机构代码证扫描件";
                        break;
                    case "lab_SWDJZ_Del":
                        lv = listSWDJZ;
                        deletetype = "税务登记证扫描件";
                        break;
                    case "lab_NSRZGZ_Del":
                        lv = listNSZGZ;
                        deletetype = "一般纳税人资格证明扫描件";
                        break;
                    case "lab_KHXKZ_Del":
                        lv = listKHXKZ;
                        deletetype = "开户许可证扫描件";
                        break;
                    case "lab_FDDBRSHZH_Del":
                        lv = listFDDBR;
                        deletetype = "法定代表人身份证扫描件";
                        break;
                    case "lab_FDDBRSQS_Del":
                        lv = listFDDBRSQS;
                        deletetype = "法定代表人授权书扫描件";
                        break;
                    default:
                        break;

                }
                if (lv != null)
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您确定要删除" + deletetype + "吗？");
                    FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", Almsg3);
                    DialogResult db = FRSE3.ShowDialog();
                    //FRSE3.
                    if (db == DialogResult.Yes)
                    {
                        lv.Items.Clear();
                    }
                }
            }
        
        
        }
        /// <summary>
        /// 若全部上传完成，在主窗体进行数据处理，根据情况编写，没有处理也要带着这个方法
        /// </summary>
        /// <param name="LV">上传结果集合</param>
        private void UpLoadSucceed(ListView LV)
        {
            //这里的LV,实际上是打开上传时指定的那个隐藏控件listView1，所以这个方法在多个上传按钮时，只需要写一次就行
            //MessageBox.Show("回调测试" + LV.Name);

        }
       */
        #endregion





        #region//提交、重置数据
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //重新设置提交按钮的位置
            this.PBload.Location = new Point(this.flowLayoutPanel1.Location.X + this.panTiJiao.Location.X + this.btnSave.Location.X + this.btnSave.Width + 25, this.flowLayoutPanel1.Location.Y + this.panTiJiao.Location.Y + this.btnSave.Location.Y);




            hashTableInfor = new Hashtable();

            Hashtable hastTableDBL = new Hashtable();
            hastTableDBL["路径"] = "DBL.png";




            #region  //备份文件
            /*
            #region//未选择经纪人、买卖家、单位、个人
            if (this.radJJR.Checked == false && this.radMMJ.Checked == false && this.radDW.Checked == false && this.radZRR.Checked == false)//未选择经纪人、买卖家、单位、个人
           {
               this.labRemJYZH.Text = "请选择交易账户！";
               this.labRemJYZH.Visible = true;

               this.labRemZCLB.Text = "请选择注册类别！";
               this.labRemZCLB.Visible = true;
               if (this.txtJJFMC.Text.Trim() == "")
               {
                   this.labRemJJFMC.Text = "请输入真实的单位名称或自然人姓名！";
                   this.labRemJJFMC.Visible = true;
               }
               else
               {
                   this.labRemJJFMC.Text = "请输入真实的单位名称或自然人姓名！";
                   this.labRemJJFMC.Visible = false;
               }
               if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号，并上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                   {
                       this.labRemSFZ.Text = "请填写正确的身份证号！";
                       this.labRemSFZ.Visible = true;
                   }
                   else
                   {
                       if (JudgeSFZH("个人身份证号", this.txtSFZ.Text.Trim()))
                       {
                           this.labRemSFZ.Text = "请上传身份证扫描件！";
                           this.labRemSFZ.Visible = true;
                       }
                       else
                       {
                           this.labRemSFZ.Text = "此身份证号已经被注册！";
                           this.labRemSFZ.Visible = true;
                       }
                     
                   }
               }
               else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
               {
                   if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                   {
                       this.labRemSFZ.Text = "请填写正确的身份证号！";
                       this.labRemSFZ.Visible = true;
                
                   }
                   else
                   {
                       if (JudgeSFZH("个人身份证号", this.txtSFZ.Text.Trim()))
                       {
                           this.labRemSFZ.Visible = false;
                       }
                       else
                       {
                           this.labRemSFZ.Text = "此身份证号已经被注册！";
                           this.labRemSFZ.Visible = true;
                       }
                   }
               }
               if (this.txtJYFLXDH.Text.Trim() == "")
               {
                   labRemJYFLXDH.Visible = true;
               }
               else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
               {
                   labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                   labRemJYFLXDH.Visible = true;
               }
               else
               {
                   labRemJYFLXDH.Visible = false;
               }

               if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
               {
                   labRemSSQY.Visible = true;
               }
               else
               {
                   labRemSSQY.Visible = false;
               }

               if (this.txtXXDZ.Text.Trim() == "")
               {
                   this.labRemXXDZ.Visible = true;

               }
               else
               {
                   this.labRemXXDZ.Visible = false;
               }

               if (this.txtLXRXM.Text.Trim() == "")
               {
                   this.labRemLXRXM.Visible = true;
               }
               else
               {
                   this.labRemLXRXM.Visible = false;
               }

               if (this.txtLXRSJH.Text.Trim() == "")
               {
                   this.labRemLXRSJH.Visible = true;
               }
               else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
               {
                   this.labRemLXRSJH.Visible = true;
                   this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
               }
               else
               {
                   this.labRemLXRSJH.Visible = false;
               }
               if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
               {
                   this.labRemKHYH.Visible = true;
               }
               else
               {
                   this.labRemKHYH.Visible = false;
               }

               //if (this.txtYHZH.Text.Trim() == "")
               //{
               //    this.labRemYHZH.Visible = true;
               //}
               //else
               //{
               //    this.labRemYHZH.Visible = false;
               //}


               return;
           }
           #endregion

           #region//未选择买卖家、单位、个人、选择经纪人
           if (this.radJJR.Checked == true && this.radMMJ.Checked == false && this.radDW.Checked == false && this.radZRR.Checked == false)//未选择买卖家、单位、个人、选择经纪人
           {
               this.labRemJYZH.Visible = false;
               this.labRemZCLB.Text = "请选择注册类别！";
               this.labRemZCLB.Visible = true;
               if (this.txtJJFMC.Text.Trim() == "")
               {
                   this.labRemJJFMC.Text = "请输入真实的单位名称或自然人姓名！";
                   this.labRemJJFMC.Visible = true;
               }
               else
               {
                   this.labRemJJFMC.Text = "请输入真实的单位名称或自然人姓名！";
                   this.labRemJJFMC.Visible = false;
               }
               //身份证扫描件
               if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号，并上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号！";
                   this.labRemSFZ.Visible = true;
               }
               else
               {
                   this.labRemSFZ.Visible = false;
               }
               if (this.txtJYFLXDH.Text.Trim() == "")
               {
                   labRemJYFLXDH.Visible = true;
               }
               else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
               {
                   labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                   labRemJYFLXDH.Visible = true;
               }
               else
               {
                   labRemJYFLXDH.Visible = false;
               }

               if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
               {
                   labRemSSQY.Visible = true;
               }
               else
               {
                   labRemSSQY.Visible = false;
               }

               if (this.txtXXDZ.Text.Trim() == "")
               {
                   this.labRemXXDZ.Visible = true;

               }
               else
               {
                   this.labRemXXDZ.Visible = false;
               }

               if (this.txtLXRXM.Text.Trim() == "")
               {
                   this.labRemLXRXM.Visible = true;
               }
               else
               {
                   this.labRemLXRXM.Visible = false;
               }

               if (this.txtLXRSJH.Text.Trim() == "")
               {
                   this.labRemLXRSJH.Visible = true;
               }
               else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
               {
                   this.labRemLXRSJH.Visible = true;
                   this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
               }
               else
               {
                   this.labRemLXRSJH.Visible = false;
               }
               if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
               {
                   this.labRemKHYH.Visible = true;
               }
               else
               {
                   this.labRemKHYH.Visible = false;
               }

               //if (this.txtYHZH.Text.Trim() == "")
               //{
               //    this.labRemYHZH.Visible = true;
               //}
               //else
               //{
               //    this.labRemYHZH.Visible = false;
               //}
               if (this.cbxGLJG.Text == "请选择业务管理部门")
               {
                   this.labRemGLJG.Visible = true;

               }
               else
               {
                   this.labRemGLJG.Visible = false;
               }

               return;
           }
           #endregion

           #region//未选择经纪人、单位、个人、选择买卖家
           if (this.radJJR.Checked == false && this.radMMJ.Checked == true && this.radDW.Checked == false && this.radZRR.Checked == false)//未选择经纪人、单位、个人、选择买卖家
           {
               this.labRemJYZH.Visible = false;

               this.labRemZCLB.Text = "请选择注册类别！";
               this.labRemZCLB.Visible = true;
               if (this.txtJJFMC.Text.Trim() == "")
               {
                   this.labRemJJFMC.Text = "请输入真实的单位名称或自然人姓名！";
                   this.labRemJJFMC.Visible = true;
               }
               else
               {
                   this.labRemJJFMC.Text = "请输入真实的单位名称或自然人姓名！";
                   this.labRemJJFMC.Visible = false;
               }
               //身份证扫描件
               if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号，并上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号！";
                   this.labRemSFZ.Visible = true;
               }
               else
               {
                   this.labRemSFZ.Visible = false;
               }
               if (this.txtJYFLXDH.Text.Trim() == "")
               {
                   labRemJYFLXDH.Visible = true;
               }
               else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
               {
                   labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                   labRemJYFLXDH.Visible = true;
               }
               else
               {
                   labRemJYFLXDH.Visible = false;
               }

               if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
               {
                   labRemSSQY.Visible = true;
               }
               else
               {
                   labRemSSQY.Visible = false;
               }

               if (this.txtXXDZ.Text.Trim() == "")
               {
                   this.labRemXXDZ.Visible = true;

               }
               else
               {
                   this.labRemXXDZ.Visible = false;
               }

               if (this.txtLXRXM.Text.Trim() == "")
               {
                   this.labRemLXRXM.Visible = true;
               }
               else
               {
                   this.labRemLXRXM.Visible = false;
               }

               if (this.txtLXRSJH.Text.Trim() == "")
               {
                   this.labRemLXRSJH.Visible = true;
               }
               else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
               {
                   this.labRemLXRSJH.Visible = true;
                   this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
               }
               else
               {
                   this.labRemLXRSJH.Visible = false;
               }
               if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
               {
                   this.labRemKHYH.Visible = true;
               }
               else
               {
                   this.labRemKHYH.Visible = false;
               }

               //if (this.txtYHZH.Text.Trim() == "")
               //{
               //    this.labRemYHZH.Visible = true;
               //}
               //else
               //{
               //    this.labRemYHZH.Visible = false;
               //}

               if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
               {
                   this.lablRemJJRZGZS.Visible = true;
               }
               else
               {
                   this.lablRemJJRZGZS.Visible = false;
               }

             

               return;
           }
           #endregion

           #region//未选择经纪人、买卖家、单位、选择个人
           if (this.radJJR.Checked == false && this.radMMJ.Checked == false && this.radDW.Checked == false && this.radZRR.Checked == true)//未选择经纪人、买卖家、单位、选择个人
           {
               this.labRemJYZH.Text  = "请选择交易账户！";
               this.labRemJYZH.Visible = true;
               this.labRemZCLB.Visible = false;
               if (this.txtJJFMC.Text.Trim() == "")
               {
                   this.labRemJJFMC.Text = "请输入自然人姓名！";
                   this.labRemJJFMC.Visible = true;
               }
               else
               {
                   this.labRemJJFMC.Text = "请输入自然人姓名！";
                   this.labRemJJFMC.Visible = false;
               }
               //身份证扫描件
               if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号，并上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号！";
                   this.labRemSFZ.Visible = true;
               }
               else
               {
                   this.labRemSFZ.Visible = false;
               }
               if (this.txtJYFLXDH.Text.Trim() == "")
               {
                   labRemJYFLXDH.Visible = true;
               }
               else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
               {
                   labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                   labRemJYFLXDH.Visible = true;
               }
               else
               {
                   labRemJYFLXDH.Visible = false;
               }

               if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
               {
                   labRemSSQY.Visible = true;
               }
               else
               {
                   labRemSSQY.Visible = false;
               }

               if (this.txtXXDZ.Text.Trim() == "")
               {
                   this.labRemXXDZ.Visible = true;

               }
               else
               {
                   this.labRemXXDZ.Visible = false;
               }

               if (this.txtLXRXM.Text.Trim() == "")
               {
                   this.labRemLXRXM.Visible = true;
               }
               else
               {
                   this.labRemLXRXM.Visible = false;
               }

               if (this.txtLXRSJH.Text.Trim() == "")
               {
                   this.labRemLXRSJH.Visible = true;
               }
               else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
               {
                   this.labRemLXRSJH.Visible = true;
                   this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
               }
               else
               {
                   this.labRemLXRSJH.Visible = false;
               }
               if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
               {
                   this.labRemKHYH.Visible = true;
               }
               else
               {
                   this.labRemKHYH.Visible = false;
               }

               //if (this.txtYHZH.Text.Trim() == "")
               //{
               //    this.labRemYHZH.Visible = true;
               //}
               //else
               //{
               //    this.labRemYHZH.Visible = false;
               //}

               if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
               {
                   this.lablRemJJRZGZS.Visible = true;
               }
               else
               {
                   this.lablRemJJRZGZS.Visible = false;
               }
               return;
           }
           #endregion

           #region//未选择经纪人、买卖家、选择个人、选择了单位
           if (this.radJJR.Checked == false && this.radMMJ.Checked == false && this.radDW.Checked == true && this.radZRR.Checked == false)//未选择经纪人、买卖家、选择个人、选择了单位
           {

               this.labRemJYZH.Text = "请选择交易账户！";
               this.labRemJYZH.Visible = true;
               this.labRemZCLB.Visible = false;
               if (this.txtJJFMC.Text.Trim() == "")
               {
                   this.labRemJJFMC.Text = "请输入自然人姓名！";
                   this.labRemJJFMC.Visible = true;
               }
               else
               {
                   this.labRemJJFMC.Text = "请输入自然人姓名！";
                   this.labRemJJFMC.Visible = false;
               }
               //身份证扫描件
               if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号，并上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSFZ.Text = "请上传身份证扫描件！";
                   this.labRemSFZ.Visible = true;
               }
               else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
               {
                   this.labRemSFZ.Text = "请填写身份证号！";
                   this.labRemSFZ.Visible = true;
               }
               else
               {
                   this.labRemSFZ.Visible = false;
               }
               //营业执照
               if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
               {
                   this.labRemYYZZ.Text = "请填写营业执照注册号，并上传扫描件！";
                   this.labRemYYZZ.Visible = true;
               }
               else if (this.txtYYZZ.Text.Trim() != "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
               {
                   this.labRemYYZZ.Text = "请上传营业执照扫描件！";
                   this.labRemYYZZ.Visible = true;
               }
               else if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem != null && this.pan_SC_YYZZ.UpItem.Items.Count > 0))
               {
                   this.labRemYYZZ.Text = "请填写营业执照注册号！";
                   this.labRemYYZZ.Visible = true;
               }
               else
               {
                   this.labRemYYZZ.Visible = false;
               }
               //组织机构代码证
               if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
               {
                   this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码，并上传扫描件！";
                   this.labRemZZJGDMZ.Visible = true;
               }
               else if (this.txtZZJGDMZ.Text.Trim() != "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
               {
                   this.labRemZZJGDMZ.Text = "请上传组织机构代码证扫描件！";
                   this.labRemZZJGDMZ.Visible = true;
               }
               else if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem != null && this.pan_SC_ZZJGDMZ.UpItem.Items.Count > 0))
               {
                   this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码！";
                   this.labRemZZJGDMZ.Visible = true;
               }
               else
               {
                   this.labRemZZJGDMZ.Visible = false;
               }
               //税务登记证
               if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSWDJZ.Text = "请填写税务登记证税号，并上传扫描件！";
                   this.labRemSWDJZ.Visible = true;
               }
               else if (this.txtSWDJZ.Text.Trim() != "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
               {
                   this.labRemSWDJZ.Text = "请上传税务登记证扫描件！";
                   this.labRemSWDJZ.Visible = true;
               }
               else if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem != null && this.pan_SC_SWDJZ.UpItem.Items.Count > 0))
               {
                   this.labRemSWDJZ.Text = "请填写税务登记证税号！";
                   this.labRemSWDJZ.Visible = true;
               }
               else
               {
                   this.labRemSWDJZ.Visible = false;
               }
               //纳税人资格证
               if (this.pan_SC_NSRZGZ.UpItem == null || this.pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
               {
                   this.labRemNSRZGZ.Text = "请上传纳税人资格证明扫描件！";
                   this.labRemNSRZGZ.Visible = true;
               }
               else if (this.pan_SC_NSRZGZ.UpItem != null && this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
               {
                   this.labRemNSRZGZ.Text = "请上传纳税人资格证明扫描件！";
                   this.labRemNSRZGZ.Visible =false;
               }
               //开户许可证
               if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
               {
                   this.labRemKHXKZ.Text = "请填写开户许可证号，并上传扫描件！";
                   this.labRemKHXKZ.Visible = true;
               }
               else if (this.txtKHXKZ.Text.Trim() != "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
               {
                   this.labRemKHXKZ.Text = "请上传开户许可证扫描件！";
                   this.labRemKHXKZ.Visible = true;
               }
               else if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem != null && this.pan_SC_KHXKZ.UpItem.Items.Count > 0))
               {
                   this.labRemKHXKZ.Text = "请填写开户许可证号！";
                   this.labRemKHXKZ.Visible = true;
               }
               else
               {
                   this.labRemKHXKZ.Visible = false;
               }
               //预留印鉴卡
               if (this.pan_SC_YLYJK.UpItem == null || this.pan_SC_YLYJK.UpItem.Items.Count <= 0)
               {
                   this.labRemYLYJK.Text = "请上传预留印鉴卡扫描件！";
                   this.labRemYLYJK.Visible = true;
               }
               else if (this.pan_SC_YLYJK.UpItem != null && this.pan_SC_YLYJK.UpItem.Items.Count > 0)
               {
                   this.labRemYLYJK.Text = "请上传预留印鉴卡扫描件！";
                   this.labRemYLYJK.Visible = false;
               }
               //法定代表人姓名
               if (this.txtFDDBR.Text.Trim() == "")
               {
                   this.labRemFDDBR.Text = "请填写法定代表人姓名！";
                   this.labRemFDDBR.Visible = true;
               }
               else
               {
                   this.labRemFDDBR.Text = "！";
                   this.labRemFDDBR.Visible = false;
               }
               //法定代表人身份证号扫描件
               if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
               {
                   this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号，并上扫描件！";
                   this.labRemFDDBRSHZH.Visible = true;
               }
               else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
               {
                   this.labRemFDDBRSHZH.Text = "请上法定代表人身份证扫描件！";
                   this.labRemFDDBRSHZH.Visible = true;
               }
               else if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
               {
                   this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号！";
                   this.labRemFDDBRSHZH.Visible = true;
               }
               else
               {
                   this.labRemFDDBRSHZH.Visible = false;
               }

               //法定带代表人授权书
               if (this.pan_SC_FDDBRSQS.UpItem == null || this.pan_SC_FDDBRSQS.UpItem.Items.Count <= 0)
               {
                   this.labRemFDDBRSQS.Text = "请上传法定代表人授权书扫描件！";
                   this.labRemFDDBRSQS.Visible = true;
               }
               else if (this.pan_SC_FDDBRSQS.UpItem != null && this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
               {
                   this.labRemFDDBRSQS.Text = "";
                   this.labRemFDDBRSQS.Visible = false;
               }
              

               if (this.txtJYFLXDH.Text.Trim() == "")
               {
                   labRemJYFLXDH.Visible = true;
               }
               else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
               {
                   labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                   labRemJYFLXDH.Visible = true;
               }
               else
               {
                   labRemJYFLXDH.Visible = false;
               }

               if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
               {
                   labRemSSQY.Visible = true;
               }
               else
               {
                   labRemSSQY.Visible = false;
               }

               if (this.txtXXDZ.Text.Trim() == "")
               {
                   this.labRemXXDZ.Visible = true;

               }
               else
               {
                   this.labRemXXDZ.Visible = false;
               }

               if (this.txtLXRXM.Text.Trim() == "")
               {
                   this.labRemLXRXM.Visible = true;
               }
               else
               {
                   this.labRemLXRXM.Visible = false;
               }

               if (this.txtLXRSJH.Text.Trim() == "")
               {
                   this.labRemLXRSJH.Visible = true;
               }
               else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
               {
                   this.labRemLXRSJH.Visible = true;
                   this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
               }
               else
               {
                   this.labRemLXRSJH.Visible = false;
               }
               if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
               {
                   this.labRemKHYH.Visible = true;
               }
               else
               {
                   this.labRemKHYH.Visible = false;
               }

               //if (this.txtYHZH.Text.Trim() == "")
               //{
               //    this.labRemYHZH.Visible = true;
               //}
               //else
               //{
               //    this.labRemYHZH.Visible = false;
               //}

               if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
               {
                   this.lablRemJJRZGZS.Visible = true;
               }
               else
               {
                   this.lablRemJJRZGZS.Visible = false;
               }
               return;
           }
           #endregion

           #region//经纪人交易账号
           if (this.radJJR.Checked == true)//经纪人交易账户
           {
               if (this.radDW.Checked == true)//单位注册
               {
                   this.labRemZCLB.Visible = false;
                   this.labRemJYZH.Visible = false;

                   int tag = 0;
                   hashTableInfor["经纪人单位交易账户类别"] = "";
                   hashTableInfor["经纪人单位交易注册类别"] = "";
                   hashTableInfor["经纪人单位交易方名称"] = "";
                   hashTableInfor["经纪人单位营业执照注册号"] = "";
                   hashTableInfor["经纪人单位营业执照扫描件"] = "";
                   hashTableInfor["经纪人单位组织机构代码证代码证"] = "";
                   hashTableInfor["经纪人单位组织机构代码证代码证扫描件"] = "";
                   hashTableInfor["经纪人单位税务登记证税号"] = "";
                   hashTableInfor["经纪人单位税务登记证扫描件"] = "";
                   hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = "";
                   hashTableInfor["经纪人单位开户许可证号"] = "";
                   hashTableInfor["经纪人单位预留印鉴卡扫描件"] = "";
                   hashTableInfor["经纪人单位开户许扫描件"] = "";
                   hashTableInfor["经纪人单位法定代表人姓名"] = "";
                   hashTableInfor["经纪人单位法定代表人身份证号"] = "";
                   hashTableInfor["经纪人单位法定代表人身份证扫描件"] = "";
                   hashTableInfor["经纪人单位法定代表人授权书"] = "";
                   hashTableInfor["经纪人单位交易方联系电话"] = "";
                   hashTableInfor["经纪人单位所属省份"] = "";
                   hashTableInfor["经纪人单位所属地市"] = "";
                   hashTableInfor["经纪人单位所属区县"] = "";
                   hashTableInfor["经纪人单位详细地址"] = "";
                   hashTableInfor["经纪人单位联系人姓名"] = "";
                   hashTableInfor["经纪人单位联系人手机号"] = "";
                   hashTableInfor["经纪人单位开户银行"] = "";
                   hashTableInfor["经纪人单位银行账号"] = "";
                   hashTableInfor["经纪人单位平台管理机构"] = "";
                   hashTableInfor["经纪人单位登录邮箱"] = HTuser["dlyx"].ToString();
                   hashTableInfor["经纪人单位用户名"] = HTuser["yhm"].ToString();
                   hashTableInfor["经纪人单位交易账户类别"] = "经纪人交易账户";
                   hashTableInfor["经纪人单位交易注册类别"] = "单位";
                   hashTableInfor["方法类别"] = "经纪人单位";//这里是为了方便选择处理方法


                   string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                   if (this.txtJJFMC.Text.Trim() == "")
                   {
                       this.labRemJJFMC.Text = "请输入真实的单位名称！";
                       this.labRemJJFMC.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemJJFMC.Text = "请输入真实的单位名称！";
                       this.labRemJJFMC.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位交易方名称"] = this.txtJJFMC.Text.Trim();
                     
                   }
                   //营业执照
                   if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemYYZZ.Text = "请填写营业执照注册号，并上传扫描件！";
                       this.labRemYYZZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtYYZZ.Text.Trim() != "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemYYZZ.Text = "请上传营业执照扫描件！";
                       this.labRemYYZZ.Visible = true;
                       JJRDWTag = "不完整";
                   }
                   else if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem != null && this.pan_SC_YYZZ.UpItem.Items.Count > 0))
                   {
                       this.labRemYYZZ.Text = "请填写营业执照注册号！";
                       this.labRemYYZZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemYYZZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位营业执照注册号"] = this.txtYYZZ.Text;
                       hashTableInfor["经纪人单位营业执照扫描件"] = this.pan_SC_YYZZ.UpItem.Items[0].SubItems[1].Text.Trim(); 
                   }
                   //组织机构代码证
                   if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码，并上传扫描件！";
                       this.labRemZZJGDMZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtZZJGDMZ.Text.Trim() != "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemZZJGDMZ.Text = "请上传组织机构代码证扫描件！";
                       this.labRemZZJGDMZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem != null && this.pan_SC_ZZJGDMZ.UpItem.Items.Count > 0))
                   {
                       this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码！";
                       this.labRemZZJGDMZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemZZJGDMZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位组织机构代码证代码证"] = this.txtZZJGDMZ.Text;
                       hashTableInfor["经纪人单位组织机构代码证代码证扫描件"] = this.pan_SC_ZZJGDMZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //税务登记证
                   if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemSWDJZ.Text = "请填写税务登记证税号，并上传扫描件！";
                       this.labRemSWDJZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtSWDJZ.Text.Trim() != "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemSWDJZ.Text = "请上传税务登记证扫描件！";
                       this.labRemSWDJZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem != null && this.pan_SC_SWDJZ.UpItem.Items.Count > 0))
                   {
                       this.labRemSWDJZ.Text = "请填写税务登记证税号！";
                       this.labRemSWDJZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemSWDJZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位税务登记证税号"] = this.txtSWDJZ.Text;
                       hashTableInfor["经纪人单位税务登记证扫描件"] = this.pan_SC_SWDJZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //纳税人资格证
                   if (this.pan_SC_NSRZGZ.UpItem == null || this.pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
                   {
                       this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                       this.labRemNSRZGZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.pan_SC_NSRZGZ.UpItem != null && this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                   {
                       this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                       this.labRemNSRZGZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = this.pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //开户许可证
                   if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemKHXKZ.Text = "请填写开户许可证号，并上传扫描件！";
                       this.labRemKHXKZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtKHXKZ.Text.Trim() != "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemKHXKZ.Text = "请上传开户许可证扫描件！";
                       this.labRemKHXKZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem != null && this.pan_SC_KHXKZ.UpItem.Items.Count > 0))
                   {
                       this.labRemKHXKZ.Text = "请填写开户许可证号！";
                       this.labRemKHXKZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemKHXKZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位开户许可证号"] = this.txtKHXKZ.Text;
                       hashTableInfor["经纪人单位开户许扫描件"] = this.pan_SC_KHXKZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //预留印鉴卡
                   if (this.pan_SC_YLYJK.UpItem == null || this.pan_SC_YLYJK.UpItem.Items.Count <= 0)
                   {
                       this.labRemYLYJK.Text = "请上传预留印鉴卡扫描件！";
                       this.labRemYLYJK.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.pan_SC_YLYJK.UpItem != null && this.pan_SC_YLYJK.UpItem.Items.Count > 0)
                   {
                       this.labRemYLYJK.Text = "请上传预留印鉴卡扫描件！";
                       this.labRemYLYJK.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位预留印鉴卡扫描件"] = this.pan_SC_YLYJK.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //法定代表人姓名
                   if (this.txtFDDBR.Text.Trim() == "")
                   {
                       this.labRemFDDBR.Text = "请填写法定代表人姓名！";
                       this.labRemFDDBR.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemFDDBR.Text = "";
                       this.labRemFDDBR.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位法定代表人姓名"] = this.txtFDDBR.Text;
                   }
                   //法定代表人身份证号扫描件
                   if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                   {
                       this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号，并上传扫描件！";
                       this.labRemFDDBRSHZH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                   {
                       if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                       {
                           this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                           this.labRemFDDBRSHZH.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                       else
                       {
                           this.labRemFDDBRSHZH.Text = "请上传法定代表人身份证扫描件！";
                           this.labRemFDDBRSHZH.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                   }
                   else if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                   {
                       this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号！";
                       this.labRemFDDBRSHZH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                   {
                       if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                       {
                           this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                           this.labRemFDDBRSHZH.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                       else
                       {
                           if (JudgeSFZH("法定代表人身份证号", this.labRemFDDBRSHZH.Text.Trim()))
                           {
                               this.labRemFDDBRSHZH.Visible = false;
                               JJRDWTag = "完整";
                               hashTableInfor["经纪人单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                               hashTableInfor["经纪人单位法定代表人身份证扫描件"] = this.pan_SC_FDDBRSHZH.UpItem.Items[0].SubItems[1].Text.Trim();
                           }
                           else
                           {
                               this.labRemFDDBRSHZH.Text = "此身份证号已经被注册！";
                               this.labRemFDDBRSHZH.Visible = true;
                               JJRDWTag = "不完整";
                               tag += 1;
                           }


                         
                       }
                     
                   }
                   //法定带代表人授权书
                   if (this.pan_SC_FDDBRSQS.UpItem == null || this.pan_SC_FDDBRSQS.UpItem.Items.Count <= 0)
                   {
                       this.labRemFDDBRSQS.Text  = "请上传法定代表人授权书扫描件！";
                       this.labRemFDDBRSQS.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.pan_SC_FDDBRSQS.UpItem != null && this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                   {
                       this.labRemFDDBRSQS.Text = "";
                       this.labRemFDDBRSQS.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位法定代表人授权书"] = this.pan_SC_FDDBRSQS.UpItem.Items[0].SubItems[1].Text.Trim();
                   }


                   if (this.txtJYFLXDH.Text.Trim() == "")
                   {
                       labRemJYFLXDH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                   {
                       labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                       labRemJYFLXDH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       labRemJYFLXDH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位交易方联系电话"] = this.txtJYFLXDH.Text;
                   }

                   if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                   {
                       labRemSSQY.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       labRemSSQY.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位所属省份"] = ucSSQY.SelectedItem[0];
                       hashTableInfor["经纪人单位所属地市"] = ucSSQY.SelectedItem[1];
                       hashTableInfor["经纪人单位所属区县"] = ucSSQY.SelectedItem[2];
                   }

                   if (this.txtXXDZ.Text == "")
                   {
                       this.labRemXXDZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;

                   }
                   else
                   {
                       this.labRemXXDZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位详细地址"] = this.txtXXDZ.Text;
                   }

                   if (this.txtLXRXM.Text == "")
                   {
                       this.labRemLXRXM.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemLXRXM.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位联系人姓名"] = this.txtLXRXM.Text;
                   }

                   if (this.txtLXRSJH.Text == "")
                   {
                       this.labRemLXRSJH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                   {
                       this.labRemLXRSJH.Visible = true;
                       this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                       JJRDWTag = "不完整";
                       tag += 1;

                   }
                   else
                   {
                       this.labRemLXRSJH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位联系人手机号"] = this.txtLXRSJH.Text;
                   }
                   if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                   {
                       this.labRemKHYH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemKHYH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位开户银行"] = this.cbxKHYH.Text;
                   }

                   //if (this.txtYHZH.Text.Trim() == "")
                   //{
                   //    this.labRemYHZH.Visible = true;
                   //    JJRDWTag = "不完整";
                   //    tag += 1;
                   //}
                   //else
                   //{
                   //    this.labRemYHZH.Visible = false;
                   //    JJRDWTag = "完整";
                   //    hashTableInfor["经纪人单位银行账号"] = this.txtYHZH.Text.Trim();
                   //}
                   if (this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString() == "请选择业务管理部门")
                   {
                       this.labRemGLJG.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemGLJG.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人单位平台管理机构"] = this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString();
                      
                   }
                   if (tag==0)
                   {
                       //禁用提交区域并开启进度
                       flowLayoutPanel1.Enabled = false;
                       PBload.Visible = true;
                       //提交账户信息
                       SRT_SubmitJYZHXX_Run();
                   }
                   else
                   {
                       return;
                   }

               }
               else if(this.radZRR.Checked==true)//自然人注册
               {
                   this.labRemJYZH.Visible = false;
                   this.labRemZCLB.Visible = false;

                   int tag = 0;
                        


                   hashTableInfor["经纪人个人交易账户类别"] = "";
                   hashTableInfor["经纪人个人交易注册类别"] = "";
                   hashTableInfor["经纪人个人交易方名称"] = "";
                   hashTableInfor["经纪人个人身份证号"] = "";
                   hashTableInfor["经纪人个人身份证扫描件"] = "";
                   hashTableInfor["经纪人个人交易方联系电话"] = "";
                   hashTableInfor["经纪人个人所属省份"] = "";
                   hashTableInfor["经纪人个人所属地市"] = "";
                   hashTableInfor["经纪人个人所属区县"] = "";
                   hashTableInfor["经纪人个人详细地址"] = "";
                   hashTableInfor["经纪人个人联系人姓名"] = "";
                   hashTableInfor["经纪人个人联系人手机号"] = "";
                   hashTableInfor["经纪人个人开户银行"] = "";
                   hashTableInfor["经纪人个人银行账号"] = "";
                   hashTableInfor["经纪人个人平台管理机构"] = "";
                   hashTableInfor["经纪人个人登录邮箱"] = HTuser["dlyx"].ToString();
                   hashTableInfor["经纪人个人用户名"] = HTuser["yhm"].ToString();
                   hashTableInfor["经纪人个人交易账户类别"] = "经纪人交易账户";
                   hashTableInfor["经纪人个人交易注册类别"] = "自然人";
                   hashTableInfor["方法类别"] = "经纪人个人";//这里是为了方便选择处理方法


                   string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                   if (this.txtJJFMC.Text.Trim() == "")
                   {
                       this.labRemJJFMC.Text = "请输入自然人姓名！";
                       this.labRemJJFMC.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemJJFMC.Text = "请输入自然人姓名！";
                       this.labRemJJFMC.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人个人交易方名称"] = this.txtJJFMC.Text.Trim();

                   }
                   //身份证扫描件
                   if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemSFZ.Text = "请填写身份证号，并上传身份证扫描件！";
                       this.labRemSFZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                   {
                       if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                       {
                           this.labRemSFZ.Text = "请填写正确的身份证号！";
                           this.labRemSFZ.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                       else
                       {
                           this.labRemSFZ.Text = "请上传身份证扫描件！";
                           this.labRemSFZ.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                   }
                   else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                   {
                       this.labRemSFZ.Text = "请填写身份证号！";
                       this.labRemSFZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                   {
                       if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                       {
                           this.labRemSFZ.Text = "请填写正确的身份证号！";
                           this.labRemSFZ.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                       else
                       {
                           if (JudgeSFZH("个人身份证号", this.txtSFZ.Text.Trim()))
                           {
                               this.labRemSFZ.Visible = false;
                               JJRDWTag = "完整";
                               hashTableInfor["经纪人个人身份证号"] = this.txtSFZ.Text.Trim();
                               hashTableInfor["经纪人个人身份证扫描件"] = this.pan_SC_SFZ.UpItem.Items[0].SubItems[1].Text.Trim();
                           }
                           else
                           {
                               this.labRemSFZ.Text = "此身份证号已经被注册！";
                               this.labRemSFZ.Visible = true;
                               JJRDWTag = "不完整";
                               tag += 1;
                           }
                       }
                   }
                  

                   if (this.txtJYFLXDH.Text.Trim() == "")
                   {
                       labRemJYFLXDH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                   {
                       labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                       labRemJYFLXDH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       labRemJYFLXDH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人个人交易方联系电话"] = this.txtJYFLXDH.Text;
                   }
                   if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                   {
                       labRemSSQY.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       labRemSSQY.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人个人所属省份"] = ucSSQY.SelectedItem[0];
                       hashTableInfor["经纪人个人所属地市"] = ucSSQY.SelectedItem[1];
                       hashTableInfor["经纪人个人所属区县"] = ucSSQY.SelectedItem[2];
                   }

                   if (this.txtXXDZ.Text == "")
                   {
                       this.labRemXXDZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;

                   }
                   else
                   {
                       this.labRemXXDZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人个人详细地址"] = this.txtXXDZ.Text;
                   }

                   if (this.txtLXRXM.Text == "")
                   {
                       this.labRemLXRXM.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemLXRXM.Visible = false;
                       JJRDWTag = "完整";
                        hashTableInfor["经纪人个人联系人姓名"] =this.txtLXRXM.Text;
                   }

                   if (this.txtLXRSJH.Text == "")
                   {
                       this.labRemLXRSJH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                   {
                       this.labRemLXRSJH.Visible = true;
                       this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                       JJRDWTag = "不完整";
                       tag += 1;
                   
                   }
                   else
                   {
                       this.labRemLXRSJH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人个人联系人手机号"] = this.txtLXRSJH.Text;
                   }
                   if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                   {
                       this.labRemKHYH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemKHYH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人个人开户银行"] = this.cbxKHYH.Text;
                   }

                   //if (this.txtYHZH.Text.Trim() == "")
                   //{
                   //    this.labRemYHZH.Visible = true;
                   //    JJRDWTag = "不完整";
                   //    tag += 1;
                   //}
                   //else
                   //{
                   //    this.labRemYHZH.Visible = false;
                   //    JJRDWTag = "完整";
                   //    hashTableInfor["经纪人个人银行账号"] = this.txtYHZH.Text.Trim();
                   //}
                   if (this.cbxGLJG.Text== "请选择业务管理部门")
                   {
                       this.labRemGLJG.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemGLJG.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["经纪人个人平台管理机构"] = this.cbxGLJG.Text;
                   }
                   if (tag==0)
                   {
                       //禁用提交区域并开启进度
                       flowLayoutPanel1.Enabled = false;
                       PBload.Visible = true;

                       //提交账户信息
                       SRT_SubmitJYZHXX_Run();
                   }
                   else
                   {
                       return;
                   }
               
               }
           
           }
           #endregion
           #region//买卖家交易账户
           else if (this.radMMJ.Checked==true)//买卖家交易账户
           {
               if (this.radDW.Checked == true)//单位注册
               {
                   this.labRemZCLB.Visible = false;
                   this.labRemJYZH.Visible = false;
                   int tag = 0;
                   hashTableInfor["买卖家单位交易账户类别"] = "";
                   hashTableInfor["买卖家单位交易注册类别"] = "";
                   hashTableInfor["买卖家单位交易方名称"] = "";
                   hashTableInfor["买卖家单位营业执照注册号"] = "";
                   hashTableInfor["买卖家单位营业执照扫描件"] = "";
                   hashTableInfor["买卖家单位组织机构代码证代码证"] = "";
                   hashTableInfor["买卖家单位组织机构代码证代码证扫描件"] = "";
                   hashTableInfor["买卖家单位税务登记证税号"] = "";
                   hashTableInfor["买卖家单位税务登记证扫描件"] = "";
                   hashTableInfor["买卖家单位一般纳税人资格证扫描件"] = "";
                   hashTableInfor["买卖家单位开户许可证号"] = "";
                   hashTableInfor["买卖家单位开户许扫描件"] = "";
                   hashTableInfor["买卖家单位预留印鉴卡扫描件"] = "";
                   hashTableInfor["买卖家单位法定代表人姓名"] = "";
                   hashTableInfor["买卖家单位法定代表人身份证号"] = "";
                   hashTableInfor["买卖家单位法定代表人身份证扫描件"] = "";
                   hashTableInfor["买卖家单位法定代表人授权书"] = "";
                   hashTableInfor["买卖家单位交易方联系电话"] = "";
                   hashTableInfor["买卖家单位所属省份"] = "";
                   hashTableInfor["买卖家单位所属地市"] = "";
                   hashTableInfor["买卖家单位所属区县"] = "";
                   hashTableInfor["买卖家单位详细地址"] = "";
                   hashTableInfor["买卖家单位联系人姓名"] = "";
                   hashTableInfor["买卖家单位联系人手机号"] = "";
                   hashTableInfor["买卖家单位开户银行"] = "";
                   hashTableInfor["买卖家单位银行账号"] = "";
                   hashTableInfor["买卖家单位经纪人资格证书编号"] = "";
                   hashTableInfor["买卖家单位经纪人经纪人名称"] = "";
                   hashTableInfor["买卖家单位经纪人联系电话"] = "";
                   hashTableInfor["买卖家单位登录邮箱"] = HTuser["dlyx"].ToString();
                   hashTableInfor["买卖家单位用户名"] = HTuser["yhm"].ToString();
                   hashTableInfor["买卖家单位交易账户类别"] = "买家卖家交易账户";
                   hashTableInfor["买卖家单位交易注册类别"] = "单位";
                   hashTableInfor["方法类别"] = "买卖家单位";//这里是为了方便选择处理方法


                   string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                   if (this.txtJJFMC.Text.Trim() == "")
                   {
                       this.labRemJJFMC.Text = "请输入真实的单位名称！";
                       this.labRemJJFMC.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemJJFMC.Text = "请输入真实的单位名称！";
                       this.labRemJJFMC.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位交易方名称"] = this.txtJJFMC.Text.Trim();

                   }
                   //营业执照
                   if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemYYZZ.Text = "请填写营业执照注册号，并上传扫描件！";
                       this.labRemYYZZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtYYZZ.Text.Trim() != "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemYYZZ.Text = "请上传营业执照扫描件！";
                       this.labRemYYZZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem != null && this.pan_SC_YYZZ.UpItem.Items.Count > 0))
                   {
                       this.labRemYYZZ.Text = "请填写营业执照注册号！";
                       this.labRemYYZZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemYYZZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位营业执照注册号"] = this.txtYYZZ.Text;
                       hashTableInfor["买卖家单位营业执照扫描件"] = this.pan_SC_YYZZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //组织机构代码证
                   if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码，并上传扫描件！";
                       this.labRemZZJGDMZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtZZJGDMZ.Text.Trim() != "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemZZJGDMZ.Text = "请上传组织机构代码证扫描件！";
                       this.labRemZZJGDMZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem != null && this.pan_SC_ZZJGDMZ.UpItem.Items.Count > 0))
                   {
                       this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码！";
                       this.labRemZZJGDMZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemZZJGDMZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位组织机构代码证代码证"] = this.txtZZJGDMZ.Text;
                       hashTableInfor["买卖家单位组织机构代码证代码证扫描件"] = this.pan_SC_ZZJGDMZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //税务登记证
                   if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemSWDJZ.Text = "请填写税务登记证税号，并上传扫描件！";
                       this.labRemSWDJZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtSWDJZ.Text.Trim() != "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemSWDJZ.Text = "请上传税务登记证扫描件！";
                       this.labRemSWDJZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem != null && this.pan_SC_SWDJZ.UpItem.Items.Count > 0))
                   {
                       this.labRemSWDJZ.Text = "请填写税务登记证税号！";
                       this.labRemSWDJZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemSWDJZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位税务登记证税号"] = this.txtSWDJZ.Text;
                       hashTableInfor["买卖家单位税务登记证扫描件"] = this.pan_SC_SWDJZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //纳税人资格证
                   if (this.pan_SC_NSRZGZ.UpItem == null || this.pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
                   {
                       this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                       this.labRemNSRZGZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.pan_SC_NSRZGZ.UpItem != null && this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                   {
                       this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                       this.labRemNSRZGZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位一般纳税人资格证扫描件"] = this.pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //开户许可证
                   if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemKHXKZ.Text = "请填写开户许可证号，并上传开户许可证扫描件！";
                       this.labRemKHXKZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtKHXKZ.Text.Trim() != "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemKHXKZ.Text = "请上传开户许可证扫描件！";
                       this.labRemKHXKZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem != null && this.pan_SC_KHXKZ.UpItem.Items.Count > 0))
                   {
                       this.labRemKHXKZ.Text = "请填写开户许可证号！";
                       this.labRemKHXKZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemKHXKZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位开户许可证号"] = this.txtKHXKZ.Text;
                       hashTableInfor["买卖家单位开户许扫描件"] = this.pan_SC_KHXKZ.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //预留印鉴卡
                   if (this.pan_SC_YLYJK.UpItem == null || this.pan_SC_YLYJK.UpItem.Items.Count <= 0)
                   {
                       this.labRemYLYJK.Text = "请上传预留印鉴卡扫描件！";
                       this.labRemYLYJK.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.pan_SC_YLYJK.UpItem != null && this.pan_SC_YLYJK.UpItem.Items.Count > 0)
                   {
                       this.labRemYLYJK.Text = "请上传预留印鉴卡扫描件！";
                       this.labRemYLYJK.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位预留印鉴卡扫描件"] = this.pan_SC_YLYJK.UpItem.Items[0].SubItems[1].Text.Trim();
                   }
                   //法定代表人姓名
                   if (this.txtFDDBR.Text.Trim() == "")
                   {
                       this.labRemFDDBR.Text = "请填写法定代表人姓名！";
                       this.labRemFDDBR.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemFDDBR.Text = "";
                       this.labRemFDDBR.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位法定代表人姓名"] = this.txtFDDBR.Text;
                   }
                   //法定代表人身份证号扫描件
                   if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                   {
                       this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号，并上传扫描件！";
                       this.labRemFDDBRSHZH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                   {
                       if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                       {
                           this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                           this.labRemFDDBRSHZH.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                       else
                       {
                           this.labRemFDDBRSHZH.Text = "请上传法定代表人身份证扫描件！";
                           this.labRemFDDBRSHZH.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                   }
                   else if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                   {
                       this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号！";
                       this.labRemFDDBRSHZH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                   {
                       if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                       {
                           this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                           this.labRemFDDBRSHZH.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                       else
                       {
                           if (JudgeSFZH("法定代表人身份证号", this.txtFDDBRSHZH.Text.Trim()))
                           {
                               this.labRemFDDBRSHZH.Visible = false;
                               JJRDWTag = "完整";
                               hashTableInfor["买卖家单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                               hashTableInfor["买卖家单位法定代表人身份证扫描件"] = this.pan_SC_FDDBRSHZH.UpItem.Items[0].SubItems[1].Text.Trim();
                           }
                           else
                           {
                               this.labRemFDDBRSHZH.Text = "此身份证号已经被注册！";
                               this.labRemFDDBRSHZH.Visible = true;
                               JJRDWTag = "不完整";
                               tag += 1;
                           }
                       }
                   }
                 

                   //法定带代表人授权书
                   if (this.pan_SC_FDDBRSQS.UpItem == null || this.pan_SC_FDDBRSQS.UpItem.Items.Count <= 0)
                   {
                       this.labRemFDDBRSQS.Text = "请上传法定代表人授权书扫描件！";
                       this.labRemFDDBRSQS.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.pan_SC_FDDBRSQS.UpItem != null && this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                   {
                       this.labRemFDDBRSQS.Text = "";
                       this.labRemFDDBRSQS.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位法定代表人授权书"] = this.pan_SC_FDDBRSQS.UpItem.Items[0].SubItems[1].Text.Trim();
                   }


                   if (this.txtJYFLXDH.Text.Trim() == "")
                   {
                       labRemJYFLXDH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                   {
                       labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                       labRemJYFLXDH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       labRemJYFLXDH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位交易方联系电话"] = this.txtJYFLXDH.Text;
                   }

                   if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                   {
                       labRemSSQY.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       labRemSSQY.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位所属省份"] = ucSSQY.SelectedItem[0];
                       hashTableInfor["买卖家单位所属地市"] = ucSSQY.SelectedItem[1];
                       hashTableInfor["买卖家单位所属区县"] = ucSSQY.SelectedItem[2];
                   }

                   if (this.txtXXDZ.Text == "")
                   {
                       this.labRemXXDZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;

                   }
                   else
                   {
                       this.labRemXXDZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位详细地址"] = this.txtXXDZ.Text;
                   }

                   if (this.txtLXRXM.Text == "")
                   {
                       this.labRemLXRXM.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemLXRXM.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位联系人姓名"] = this.txtLXRXM.Text;
                   }

                   if (this.txtLXRSJH.Text == "")
                   {
                       this.labRemLXRSJH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                   {
                       this.labRemLXRSJH.Visible = true;
                       this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                       JJRDWTag = "不完整";
                       tag += 1;

                   }
                   else
                   {
                       this.labRemLXRSJH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位联系人手机号"] = this.txtLXRSJH.Text;
                   }
                   if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                   {
                       this.labRemKHYH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemKHYH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位开户银行"] = this.cbxKHYH.Text;
                   }

                   //if (this.txtYHZH.Text.Trim() == "")
                   //{
                   //    this.labRemYHZH.Visible = true;
                   //    JJRDWTag = "不完整";
                   //    tag += 1;
                   //}
                   //else
                   //{
                   //    this.labRemYHZH.Visible = false;
                   //    JJRDWTag = "完整";
                   //    hashTableInfor["买卖家单位银行账号"] = this.txtYHZH.Text.Trim();
                   //}

                   if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                   {
                       this.lablRemJJRZGZS.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.lablRemJJRZGZS.Visible = false;
                       JJRDWTag = "完整";
                   }

                   if (this.txtJJRZGZS.Text.Trim() == "")
                   {
                       this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                       this.lablRemJJRZGZS.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                   {
                       this.lablRemJJRZGZS.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家单位经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                       hashTableInfor["买卖家单位经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                       hashTableInfor["买卖家单位经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                   }
                   else
                   {
                       this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                       this.lablRemJJRZGZS.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   
                   }
                   if (tag==0)
                   {
                       //禁用提交区域并开启进度
                       flowLayoutPanel1.Enabled = false;
                       PBload.Visible = true;
                       //提交账户信息
                       SRT_SubmitJYZHXX_Run();
                   }
                   else
                   {
                       return;
                   }



               }
               else if (this.radZRR.Checked == true)//自然人注册
               {
                   int tag = 0;
                   hashTableInfor["买卖家个人交易账户类别"] = "";
                   hashTableInfor["买卖家个人交易注册类别"] = "";
                   hashTableInfor["买卖家个人交易方名称"] = "";
                   hashTableInfor["买卖家个人身份证号"] = "";
                   hashTableInfor["买卖家个人身份证扫描件"] = "";
                   hashTableInfor["买卖家个人交易方联系电话"] = "";
                   hashTableInfor["买卖家个人所属省份"] = "";
                   hashTableInfor["买卖家个人所属地市"] = "";
                   hashTableInfor["买卖家个人所属区县"] = "";
                   hashTableInfor["买卖家个人详细地址"] = "";
                   hashTableInfor["买卖家个人联系人姓名"] = "";
                   hashTableInfor["买卖家个人联系人手机号"] = "";
                   hashTableInfor["买卖家个人开户银行"] = "";
                   hashTableInfor["买卖家个人银行账号"] = "";
                   hashTableInfor["买卖家个人经纪人资格证书编号"] = "";
                   hashTableInfor["买卖家个人经纪人经纪人名称"] = "";
                   hashTableInfor["买卖家个人经纪人联系电话"] = "";
                   hashTableInfor["买卖家个人登录邮箱"] = HTuser["dlyx"].ToString();
                   hashTableInfor["买卖家个人用户名"] = HTuser["yhm"].ToString();
                   hashTableInfor["买卖家个人交易账户类别"] = "买家卖家交易账户";
                   hashTableInfor["买卖家个人交易注册类别"] = "自然人";
                   hashTableInfor["方法类别"] = "买卖家个人";//这里是为了方便选择处理方法


                   string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                   if (this.txtJJFMC.Text.Trim() == "")
                   {
                       this.labRemJJFMC.Text = "请输入真实的自然人姓名！";
                       this.labRemJJFMC.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemJJFMC.Text = "请输入真实的单位名称！";
                       this.labRemJJFMC.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家个人交易方名称"] = this.txtJJFMC.Text.Trim();

                   }
                   //身份证扫描件
                   if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                   {
                       this.labRemSFZ.Text = "请填写身份证号，并上传身份证扫描件！";
                       this.labRemSFZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                   {

                       if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                       {
                           this.labRemSFZ.Text = "请填写正确的身份证号！";
                           this.labRemSFZ.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                       else
                       {
                           this.labRemSFZ.Text = "请上传身份证扫描件！";
                           this.labRemSFZ.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                   }
                   else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                   {
                       this.labRemSFZ.Text = "请填写身份证号！";
                       this.labRemSFZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                   {
                       if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                       {
                           this.labRemSFZ.Text = "请填写正确的身份证号！";
                           this.labRemSFZ.Visible = true;
                           JJRDWTag = "不完整";
                           tag += 1;
                       }
                       else
                       {
                         

                           if (JudgeSFZH("个人身份证号", this.txtSFZ.Text.Trim()))
                           {
                               this.labRemSFZ.Visible = false;
                               JJRDWTag = "完整";
                               hashTableInfor["买卖家个人身份证号"] = this.txtSFZ.Text.Trim();
                               hashTableInfor["买卖家个人身份证扫描件"] = this.pan_SC_SFZ.UpItem.Items[0].SubItems[1].Text.Trim();
                           }
                           else
                           {
                               this.labRemSFZ.Text = "此身份证号已经被注册！";
                               this.labRemSFZ.Visible = true;
                               JJRDWTag = "不完整";
                               tag += 1;
                           }
                       }
                   }
                


                   if (this.txtJYFLXDH.Text.Trim() == "")
                   {
                       labRemJYFLXDH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                   {
                       labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                       labRemJYFLXDH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       labRemJYFLXDH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家个人交易方联系电话"] = this.txtJYFLXDH.Text;
                   }

                   if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                   {
                       labRemSSQY.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       labRemSSQY.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家个人所属省份"] = ucSSQY.SelectedItem[0];
                       hashTableInfor["买卖家个人所属地市"] = ucSSQY.SelectedItem[1];
                       hashTableInfor["买卖家个人所属区县"] = ucSSQY.SelectedItem[2];
                   }

                   if (this.txtXXDZ.Text == "")
                   {
                       this.labRemXXDZ.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;

                   }
                   else
                   {
                       this.labRemXXDZ.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家个人详细地址"] = this.txtXXDZ.Text;
                   }

                   if (this.txtLXRXM.Text == "")
                   {
                       this.labRemLXRXM.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemLXRXM.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家个人联系人姓名"] = this.txtLXRXM.Text;
                   }

                   if (this.txtLXRSJH.Text == "")
                   {
                       this.labRemLXRSJH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                   {
                       this.labRemLXRSJH.Visible = true;
                       this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                       JJRDWTag = "不完整";
                       tag += 1;

                   }
                   else
                   {
                       this.labRemLXRSJH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家个人联系人手机号"] = this.txtLXRSJH.Text;
                   }
                   if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                   {
                       this.labRemKHYH.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.labRemKHYH.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家个人开户银行"] = this.cbxKHYH.Text;
                   }

                   //if (this.txtYHZH.Text.Trim() == "")
                   //{
                   //    this.labRemYHZH.Visible = true;
                   //    JJRDWTag = "不完整";
                   //    tag += 1;
                   //}
                   //else
                   //{
                   //    this.labRemYHZH.Visible = false;
                   //    JJRDWTag = "完整";
                   //    hashTableInfor["买卖家个人银行账号"] = this.txtYHZH.Text.Trim();
                   //}

                   if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                   {
                       this.lablRemJJRZGZS.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else
                   {
                       this.lablRemJJRZGZS.Visible = false;
                       JJRDWTag = "完整";
                   }

                   if (this.txtJJRZGZS.Text.Trim() == "")
                   {
                       this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                       this.lablRemJJRZGZS.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;
                   }
                   else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                   {
                       this.lablRemJJRZGZS.Visible = false;
                       JJRDWTag = "完整";
                       hashTableInfor["买卖家个人经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                       hashTableInfor["买卖家个人经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                       hashTableInfor["买卖家个人经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                   }
                   else
                   {
                       this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                       this.lablRemJJRZGZS.Visible = true;
                       JJRDWTag = "不完整";
                       tag += 1;

                   }
                   if (  tag==0)
                   {
                       //禁用提交区域并开启进度
                       flowLayoutPanel1.Enabled = false;
                       PBload.Visible = true;
                       //提交账户信息
                       SRT_SubmitJYZHXX_Run();
                   }
                   else
                   {
                       return;
                   }

               }
           }
           #endregion
          */
            #endregion

            #region  //此处是上线前的临时文件

            #region//未选择经纪人、买卖家、单位、个人
            if (this.radJJR.Checked == false && this.radMMJ.Checked == false && this.radDW.Checked == false && this.radZRR.Checked == false)//未选择经纪人、买卖家、单位、个人
            {
                this.labRemJYZH.Text = "请选择交易账户！";
                this.labRemJYZH.Visible = true;

                this.labRemZCLB.Text = "请选择注册类别！";
                this.labRemZCLB.Visible = true;
                if (this.txtJJFMC.Text.Trim() == "")
                {
                    this.labRemJJFMC.Text = "请输入真实的单位名称！";
                    this.labRemJJFMC.Visible = true;
                }
                else
                {
                    this.labRemJJFMC.Text = "请输入真实的单位名称！";
                    this.labRemJJFMC.Visible = false;
                }
                if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                    {
                        this.labRemSFZ.Text = "请填写正确的身份证号！";
                        this.labRemSFZ.Visible = true;
                        //this.TipsSFZZFM.Visible = false;
                    }
                    else
                    {
                        this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                        this.labRemSFZ.Visible = true;
                        //  this.TipsSFZZFM.Visible = false;
                    }
                }
                else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号！";
                    this.labRemSFZ.Visible = true;
                    //  this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                {

                    if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                    {
                        this.labRemSFZ.Text = "请填写正确的身份证号！";
                        this.labRemSFZ.Visible = true;
                        // this.TipsSFZZFM.Visible = false;

                    }
                    else
                    {
                        this.labRemSFZ.Visible = false;
                        // this.TipsSFZZFM.Visible = true;
                    }
                }
                //身份证反面扫描件
                if (this.pan_SC_SFZ_FM.UpItem == null || this.pan_SC_SFZ_FM.UpItem.Items.Count <= 0)
                {
                    this.labRemSFZ_FM.Visible = true;
                }
                else
                {
                    this.labRemSFZ_FM.Visible = false;
                }

                if (this.txtJYFLXDH.Text.Trim() == "")
                {
                    labRemJYFLXDH.Visible = true;
                }
                else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                {
                    labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                    labRemJYFLXDH.Visible = true;
                }
                else
                {
                    labRemJYFLXDH.Visible = false;
                }

                if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                {
                    labRemSSQY.Visible = true;
                }
                else
                {
                    labRemSSQY.Visible = false;
                }

                if (this.txtXXDZ.Text.Trim() == "")
                {
                    this.labRemXXDZ.Visible = true;

                }
                else
                {
                    this.labRemXXDZ.Visible = false;
                }

                if (this.txtLXRXM.Text.Trim() == "")
                {
                    this.labRemLXRXM.Visible = true;
                }
                else
                {
                    this.labRemLXRXM.Visible = false;
                }

                if (this.txtLXRSJH.Text.Trim() == "")
                {
                    this.labRemLXRSJH.Visible = true;
                }
                else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                {
                    this.labRemLXRSJH.Visible = true;
                    this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                }
                else
                {
                    this.labRemLXRSJH.Visible = false;
                }
                if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                {
                    this.labRemKHYH.Visible = true;
                }
                else
                {
                    this.labRemKHYH.Visible = false;
                }

                //if (this.txtYHZH.Text.Trim() == "")
                //{
                //    this.labRemYHZH.Visible = true;
                //}
                //else
                //{
                //    this.labRemYHZH.Visible = false;
                //}

                //交易资金密码
                if (this.txtZQZJMM.Text.Trim() == "")
                {
                    this.labZQZJMM.Visible = true;
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                {
                    this.labZQZJMM.Visible = true;
                    this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else
                {
                    this.labZQZJMM.Visible = false;
                    label6.Visible = !this.labZQZJMM.Visible;
                }

                return;
            }
            #endregion

            #region//未选择买卖家、单位、个人、选择经纪人
            if (this.radJJR.Checked == true && this.radMMJ.Checked == false && this.radDW.Checked == false && this.radZRR.Checked == false)//未选择买卖家、单位、个人、选择经纪人
            {
                this.labRemJYZH.Visible = false;
                this.labRemZCLB.Text = "请选择注册类别！";
                this.labRemZCLB.Visible = true;
                if (this.txtJJFMC.Text.Trim() == "")
                {
                    this.labRemJJFMC.Text = "请输入真实的单位名称！";
                    this.labRemJJFMC.Visible = true;
                }
                else
                {
                    this.labRemJJFMC.Text = "请输入真实的单位名称！";
                    this.labRemJJFMC.Visible = false;
                }
                //身份证扫描件
                if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else
                {
                    this.labRemSFZ.Visible = false;
                    // this.TipsSFZZFM.Visible = true;
                }


                //身份证反面扫描件
                if (this.pan_SC_SFZ_FM.UpItem == null || this.pan_SC_SFZ_FM.UpItem.Items.Count <= 0)
                {
                    this.labRemSFZ_FM.Visible = true;
                }
                else
                {
                    this.labRemSFZ_FM.Visible = false;
                }
                if (this.txtJYFLXDH.Text.Trim() == "")
                {
                    labRemJYFLXDH.Visible = true;
                }
                else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                {
                    labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                    labRemJYFLXDH.Visible = true;
                }
                else
                {
                    labRemJYFLXDH.Visible = false;
                }

                if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                {
                    labRemSSQY.Visible = true;
                }
                else
                {
                    labRemSSQY.Visible = false;
                }

                if (this.txtXXDZ.Text.Trim() == "")
                {
                    this.labRemXXDZ.Visible = true;

                }
                else
                {
                    this.labRemXXDZ.Visible = false;
                }

                if (this.txtLXRXM.Text.Trim() == "")
                {
                    this.labRemLXRXM.Visible = true;
                }
                else
                {
                    this.labRemLXRXM.Visible = false;
                }

                if (this.txtLXRSJH.Text.Trim() == "")
                {
                    this.labRemLXRSJH.Visible = true;
                }
                else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                {
                    this.labRemLXRSJH.Visible = true;
                    this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                }
                else
                {
                    this.labRemLXRSJH.Visible = false;
                }
                if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                {
                    this.labRemKHYH.Visible = true;
                }
                else
                {
                    this.labRemKHYH.Visible = false;
                }

                //if (this.txtYHZH.Text.Trim() == "")
                //{
                //    this.labRemYHZH.Visible = true;
                //}
                //else
                //{
                //    this.labRemYHZH.Visible = false;
                //}
                if (this.cbxGLJG.Text == "请选择业务管理部门")
                {
                    this.labRemGLJG.Visible = true;

                }
                else
                {
                    this.labRemGLJG.Visible = false;
                }
                if (this.radFGS.Checked == true)//选择分公司
                {
                    if (this.cbxGLJG.Text == "请选择业务管理部门")
                    {
                        this.labRemGLJG.Visible = true;
                  
                    }
                    else
                    {
                        this.labRemGLJG.Visible = false;
                    }
                }
                if (this.radYWTZB.Checked == true)//业务拓展部
                {
                  
                }

                if (this.radGXTW.Checked == true)//高校团委
                {
                    if (String.IsNullOrEmpty(this.labGXTW_ZhangHao.Text))
                    {
                        this.labRemGXTW.Visible = true;
                      
                    }
                    else
                    {
                        this.labRemGXTW.Visible = false;
                    }
                    //ComboBoxItem sex_item = (ComboBoxItem)cbxSZYX.SelectedItem;
                    if (String.IsNullOrEmpty(this.txtSZYX.Text.Trim()))
                    {
                        this.labRemXZYX.Visible = true;
                      
                    }
                    else
                    {
                        this.labRemXZYX.Visible = false;
                    }


                }
                //交易资金密码
                if (this.txtZQZJMM.Text.Trim() == "")
                {
                    this.labZQZJMM.Visible = true;
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                {
                    this.labZQZJMM.Visible = true;
                    this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else
                {
                    this.labZQZJMM.Visible = false;
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                return;
            }
            #endregion

            #region//未选择经纪人、单位、个人、选择买卖家
            if (this.radJJR.Checked == false && this.radMMJ.Checked == true && this.radDW.Checked == false && this.radZRR.Checked == false)//未选择经纪人、单位、个人、选择买卖家
            {
                this.labRemJYZH.Visible = false;

                this.labRemZCLB.Text = "请选择注册类别！";
                this.labRemZCLB.Visible = true;
                if (this.txtJJFMC.Text.Trim() == "")
                {
                    this.labRemJJFMC.Text = "请输入真实的单位名称！";
                    this.labRemJJFMC.Visible = true;
                }
                else
                {
                    this.labRemJJFMC.Text = "请输入真实的单位名称！";
                    this.labRemJJFMC.Visible = false;
                }
                //身份证扫描件
                if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    //  this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号！";
                    this.labRemSFZ.Visible = true;
                    //this.TipsSFZZFM.Visible = false;
                }
                else
                {
                    this.labRemSFZ.Visible = false;
                    //  this.TipsSFZZFM.Visible = true;
                }
                //身份证反面扫描件
                if (this.pan_SC_SFZ_FM.UpItem == null || this.pan_SC_SFZ_FM.UpItem.Items.Count <= 0)
                {
                    this.labRemSFZ_FM.Visible = true;
                }
                else
                {
                    this.labRemSFZ_FM.Visible = false;
                }
                if (this.txtJYFLXDH.Text.Trim() == "")
                {
                    labRemJYFLXDH.Visible = true;
                }
                else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                {
                    labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                    labRemJYFLXDH.Visible = true;
                }
                else
                {
                    labRemJYFLXDH.Visible = false;
                }

                if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                {
                    labRemSSQY.Visible = true;
                }
                else
                {
                    labRemSSQY.Visible = false;
                }

                if (this.txtXXDZ.Text.Trim() == "")
                {
                    this.labRemXXDZ.Visible = true;

                }
                else
                {
                    this.labRemXXDZ.Visible = false;
                }

                if (this.txtLXRXM.Text.Trim() == "")
                {
                    this.labRemLXRXM.Visible = true;
                }
                else
                {
                    this.labRemLXRXM.Visible = false;
                }

                if (this.txtLXRSJH.Text.Trim() == "")
                {
                    this.labRemLXRSJH.Visible = true;
                }
                else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                {
                    this.labRemLXRSJH.Visible = true;
                    this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                }
                else
                {
                    this.labRemLXRSJH.Visible = false;
                }
                if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                {
                    this.labRemKHYH.Visible = true;
                }
                else
                {
                    this.labRemKHYH.Visible = false;
                }

                //if (this.txtYHZH.Text.Trim() == "")
                //{
                //    this.labRemYHZH.Visible = true;
                //}
                //else
                //{
                //    this.labRemYHZH.Visible = false;
                //}

                #region//20131218周丽作废
                //if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                //{
                //    this.lablRemJJRZGZS.Visible = true;
                //}
                //else
                //{
                //    this.lablRemJJRZGZS.Visible = false;
                //}
                #endregion

                if (this.radFW_JJR.Checked == true)
                {
                    if (this.txtJJRZGZS.Text.Trim() == "")
                    {
                        this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                        this.lablRemJJRZGZS.Visible = true;
                   
                    }
                    else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                    {
                        this.lablRemJJRZGZS.Visible = false;
                    }
                    else
                    {
                        this.lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                        this.lablRemJJRZGZS.Visible = true;
                    }
                }
                else
                {
                    if (radFW_Bank.Checked == true)
                    {
                        if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                        {

                            this.labRemYHMC.Visible = true;
                        }
                        else
                        {
                            this.labRemYHMC.Visible = false;
                        }
                        if (string.IsNullOrEmpty(this.uctextBankYGGH.Text.Trim()))
                        {
                            this.lblBankYGGH.Visible = true;
                        }
                        else
                        {
                            this.lblBankYGGH.Visible = false;
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(this.labZFMC.Text))
                        {

                            this.labRemZFMC.Visible = true;
                        }
                        else
                        {
                            this.labRemZFMC.Visible = false;
                        }

                        if (String.IsNullOrEmpty(this.labHangYeXieHui.Text))
                        {

                            this.labRemHangYeXieHui.Visible = true;
                        }
                        else
                        {
                            this.labRemHangYeXieHui.Visible = false;
                        }

                        if (this.txtJJRZGZS.Text.Trim() == "")
                        {
                            this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                            this.lablRemJJRZGZS.Visible = true;

                        }
                        else
                        {
                            this.lablRemJJRZGZS.Text = "您填写的经纪人资格证书编号无效！";

                            this.lablRemJJRZGZS.Visible = true;
                        }
                    }

                    

                    
                }
                //交易资金密码
                if (this.txtZQZJMM.Text.Trim() == "")
                {
                    this.labZQZJMM.Visible = true;
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                {
                    this.labZQZJMM.Visible = true;
                    this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else
                {
                    this.labZQZJMM.Visible = false;
                    label6.Visible = !this.labZQZJMM.Visible;
                }

                return;
            }
            #endregion

            #region//未选择经纪人、买卖家、单位、选择个人
            if (this.radJJR.Checked == false && this.radMMJ.Checked == false && this.radDW.Checked == false && this.radZRR.Checked == true)//未选择经纪人、买卖家、单位、选择个人
            {
                this.labRemJYZH.Text = "请选择交易账户！";
                this.labRemJYZH.Visible = true;
                this.labRemZCLB.Visible = false;
                if (this.txtJJFMC.Text.Trim() == "")
                {
                    this.labRemJJFMC.Text = "请输入自然人姓名！";
                    this.labRemJJFMC.Visible = true;
                }
                else
                {
                    this.labRemJJFMC.Text = "请输入自然人姓名！";
                    this.labRemJJFMC.Visible = false;
                }
                //身份证扫描件
                if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    //  this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else
                {
                    this.labRemSFZ.Visible = false;
                    //  this.TipsSFZZFM.Visible = true;
                }
                //身份证反面扫描件
                if (this.pan_SC_SFZ_FM.UpItem == null || this.pan_SC_SFZ_FM.UpItem.Items.Count <= 0)
                {
                    this.labRemSFZ_FM.Visible = true;
                }
                else
                {
                    this.labRemSFZ_FM.Visible = false;
                }
                if (this.txtJYFLXDH.Text.Trim() == "")
                {
                    labRemJYFLXDH.Visible = true;
                }
                else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                {
                    labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                    labRemJYFLXDH.Visible = true;
                }
                else
                {
                    labRemJYFLXDH.Visible = false;
                }

                if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                {
                    labRemSSQY.Visible = true;
                }
                else
                {
                    labRemSSQY.Visible = false;
                }

                if (this.txtXXDZ.Text.Trim() == "")
                {
                    this.labRemXXDZ.Visible = true;

                }
                else
                {
                    this.labRemXXDZ.Visible = false;
                }

                if (this.txtLXRXM.Text.Trim() == "")
                {
                    this.labRemLXRXM.Visible = true;
                }
                else
                {
                    this.labRemLXRXM.Visible = false;
                }

                if (this.txtLXRSJH.Text.Trim() == "")
                {
                    this.labRemLXRSJH.Visible = true;
                }
                else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                {
                    this.labRemLXRSJH.Visible = true;
                    this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                }
                else
                {
                    this.labRemLXRSJH.Visible = false;
                }
                if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                {
                    this.labRemKHYH.Visible = true;
                }
                else
                {
                    this.labRemKHYH.Visible = false;
                }

                //if (this.txtYHZH.Text.Trim() == "")
                //{
                //    this.labRemYHZH.Visible = true;
                //}
                //else
                //{
                //    this.labRemYHZH.Visible = false;
                //}

                if (radFW_Bank.Checked == true)
                {
                }
                else
                {
                    if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                    {
                        this.lablRemJJRZGZS.Visible = true;
                    }
                    else
                    {
                        this.lablRemJJRZGZS.Visible = false;
                    }
                }
                

                //交易资金密码
                if (this.txtZQZJMM.Text.Trim() == "")
                {
                    this.labZQZJMM.Visible = true;
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                {
                    this.labZQZJMM.Visible = true;
                    this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else
                {
                    this.labZQZJMM.Visible = false;
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                return;
            }
            #endregion

            #region//未选择经纪人、买卖家、选择个人、选择了单位
            if (this.radJJR.Checked == false && this.radMMJ.Checked == false && this.radDW.Checked == true && this.radZRR.Checked == false)//未选择经纪人、买卖家、选择个人、选择了单位
            {

                this.labRemJYZH.Text = "请选择交易账户！";
                this.labRemJYZH.Visible = true;
                this.labRemZCLB.Visible = false;
                if (this.txtJJFMC.Text.Trim() == "")
                {
                    this.labRemJJFMC.Text = "请输入自然人姓名！";
                    this.labRemJJFMC.Visible = true;
                }
                else
                {
                    this.labRemJJFMC.Text = "请输入自然人姓名！";
                    this.labRemJJFMC.Visible = false;
                }
                //身份证扫描件
                if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    //this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                {
                    this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                {
                    this.labRemSFZ.Text = "请填写身份证号！";
                    this.labRemSFZ.Visible = true;
                    // this.TipsSFZZFM.Visible = false;
                }
                else
                {
                    this.labRemSFZ.Visible = false;
                    //  this.TipsSFZZFM.Visible = true;
                }
                //营业执照
                if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                {
                    this.labRemYYZZ.Text = "请填写营业执照注册号，并上传扫描件！";
                    this.labRemYYZZ.Visible = true;
                }
                else if (this.txtYYZZ.Text.Trim() != "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                {
                    this.labRemYYZZ.Text = "请上传营业执照扫描件！";
                    this.labRemYYZZ.Visible = true;
                }
                else if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem != null && this.pan_SC_YYZZ.UpItem.Items.Count > 0))
                {
                    this.labRemYYZZ.Text = "请填写营业执照注册号！";
                    this.labRemYYZZ.Visible = true;
                }
                else
                {
                    this.labRemYYZZ.Visible = false;
                }
                //组织机构代码证
                if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                {
                    this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码，并上传扫描件！";
                    this.labRemZZJGDMZ.Visible = true;
                }
                else if (this.txtZZJGDMZ.Text.Trim() != "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                {
                    this.labRemZZJGDMZ.Text = "请上传组织机构代码证扫描件！";
                    this.labRemZZJGDMZ.Visible = true;
                }
                else if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem != null && this.pan_SC_ZZJGDMZ.UpItem.Items.Count > 0))
                {
                    this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码！";
                    this.labRemZZJGDMZ.Visible = true;
                }
                else
                {
                    this.labRemZZJGDMZ.Visible = false;
                }
                //税务登记证
                //if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                //{
                //    this.labRemSWDJZ.Text = "请填写税务登记证税号，并上传扫描件！";
                //    this.labRemSWDJZ.Visible = true;
                //}
                //else if (this.txtSWDJZ.Text.Trim() != "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                //{
                //    this.labRemSWDJZ.Text = "请上传税务登记证扫描件！";
                //    this.labRemSWDJZ.Visible = true;
                //}
                //else if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem != null && this.pan_SC_SWDJZ.UpItem.Items.Count > 0))
                //{
                //    this.labRemSWDJZ.Text = "请填写税务登记证税号！";
                //    this.labRemSWDJZ.Visible = true;
                //}
                //else
                //{
                //    this.labRemSWDJZ.Visible = false;
                //}
                //纳税人资格证
                //if (this.pan_SC_NSRZGZ.UpItem == null || this.pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
                //{
                //    this.labRemNSRZGZ.Text = "一般纳税人资格证明扫描件！";
                //    this.labRemNSRZGZ.Visible = true;
                //}
                //else if (this.pan_SC_NSRZGZ.UpItem != null && this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                //{
                //    this.labRemNSRZGZ.Text = "一般纳税人资格证明扫描件！";
                //    this.labRemNSRZGZ.Visible = false;
                //}
                //开户许可证
                //if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                //{
                //    this.labRemKHXKZ.Text = "请填写开户许可证号，并上传扫描件！";
                //    this.labRemKHXKZ.Visible = true;
                //}
                //else if (this.txtKHXKZ.Text.Trim() != "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                //{
                //    this.labRemKHXKZ.Text = "请上传开户许可证扫描件！";
                //    this.labRemKHXKZ.Visible = true;
                //}
                //else if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem != null && this.pan_SC_KHXKZ.UpItem.Items.Count > 0))
                //{
                //    this.labRemKHXKZ.Text = "请填写开户许可证号！";
                //    this.labRemKHXKZ.Visible = true;
                //}
                //else
                //{
                //    this.labRemKHXKZ.Visible = false;
                //}
                //预留印鉴卡
                if (this.pan_SC_YLYJK.UpItem == null || this.pan_SC_YLYJK.UpItem.Items.Count <= 0)
                {
                    this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                    this.labRemYLYJK.Visible = true;
                }
                else if (this.pan_SC_YLYJK.UpItem != null && this.pan_SC_YLYJK.UpItem.Items.Count > 0)
                {
                    this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                    this.labRemYLYJK.Visible = false;
                }
                //法定代表人姓名
                if (this.txtFDDBR.Text.Trim() == "")
                {
                    this.labRemFDDBR.Text = "请填写法定代表人姓名！";
                    this.labRemFDDBR.Visible = true;
                }
                else
                {
                    this.labRemFDDBR.Text = "！";
                    this.labRemFDDBR.Visible = false;
                }
                //法定代表人身份证号扫描件
                if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                {
                    this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号，并上扫正面扫描件！";
                    this.labRemFDDBRSHZH.Visible = true;
                }
                else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                {
                    this.labRemFDDBRSHZH.Text = "请上法定代表人身份证正面扫描件！";
                    this.labRemFDDBRSHZH.Visible = true;
                }
                else if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                {
                    this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号！";
                    this.labRemFDDBRSHZH.Visible = true;
                }
                else
                {
                    this.labRemFDDBRSHZH.Visible = false;
                }

                if (this.pan_SC_FDDBRSHZH_FM.UpItem == null || this.pan_SC_FDDBRSHZH_FM.UpItem.Items.Count <= 0)
                {
                    this.labRemFDDBRSHZH_FM.Visible = true;
                }
                else
                {
                    this.labRemFDDBRSHZH_FM.Visible = false;
                }

                //法定带代表人授权书
                //if (this.pan_SC_FDDBRSQS.UpItem == null || this.pan_SC_FDDBRSQS.UpItem.Items.Count <= 0)
                //{
                //    this.labRemFDDBRSQS.Text = "请上传法定代表人授权书扫描件！";
                //    this.labRemFDDBRSQS.Visible = true;
                //}
                //else if (this.pan_SC_FDDBRSQS.UpItem != null && this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                //{
                //    this.labRemFDDBRSQS.Text = "";
                //    this.labRemFDDBRSQS.Visible = false;
                //}


                if (this.txtJYFLXDH.Text.Trim() == "")
                {
                    labRemJYFLXDH.Visible = true;
                }
                else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                {
                    labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                    labRemJYFLXDH.Visible = true;
                }
                else
                {
                    labRemJYFLXDH.Visible = false;
                }

                if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                {
                    labRemSSQY.Visible = true;
                }
                else
                {
                    labRemSSQY.Visible = false;
                }

                if (this.txtXXDZ.Text.Trim() == "")
                {
                    this.labRemXXDZ.Visible = true;

                }
                else
                {
                    this.labRemXXDZ.Visible = false;
                }

                if (this.txtLXRXM.Text.Trim() == "")
                {
                    this.labRemLXRXM.Visible = true;
                }
                else
                {
                    this.labRemLXRXM.Visible = false;
                }

                if (this.txtLXRSJH.Text.Trim() == "")
                {
                    this.labRemLXRSJH.Visible = true;
                }
                else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                {
                    this.labRemLXRSJH.Visible = true;
                    this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                }
                else
                {
                    this.labRemLXRSJH.Visible = false;
                }
                if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                {
                    this.labRemKHYH.Visible = true;
                }
                else
                {
                    this.labRemKHYH.Visible = false;
                }

                //if (this.txtYHZH.Text.Trim() == "")
                //{
                //    this.labRemYHZH.Visible = true;
                //}
                //else
                //{
                //    this.labRemYHZH.Visible = false;
                //}

                if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                {
                    this.lablRemJJRZGZS.Visible = true;
                }
                else
                {
                    this.lablRemJJRZGZS.Visible = false;
                }

                //交易资金密码
                if (this.txtZQZJMM.Text.Trim() == "")
                {
                    this.labZQZJMM.Visible = true;
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                {
                    this.labZQZJMM.Visible = true;
                    this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                    label6.Visible = !this.labZQZJMM.Visible;
                }
                else
                {
                    this.labZQZJMM.Visible = false;
                    label6.Visible = !this.labZQZJMM.Visible;
                }


                return;
            }
            #endregion

            #region//经纪人交易账号
            if (this.radJJR.Checked == true)//经纪人交易账户
            {
                if (this.radDW.Checked == true)//单位注册
                {
                    this.labRemZCLB.Visible = false;
                    this.labRemJYZH.Visible = false;

                    int tag = 0;
                    hashTableInfor["经纪人单位交易账户类别"] = "";
                    hashTableInfor["经纪人单位交易注册类别"] = "";
                    hashTableInfor["经纪人单位交易方名称"] = "";
                    hashTableInfor["经纪人单位营业执照注册号"] = "";
                    hashTableInfor["经纪人单位营业执照扫描件"] = "";
                    hashTableInfor["经纪人单位组织机构代码证代码证"] = "";
                    hashTableInfor["经纪人单位组织机构代码证代码证扫描件"] = "";
                    hashTableInfor["经纪人单位税务登记证税号"] = "";
                    hashTableInfor["经纪人单位税务登记证扫描件"] = "";
                    hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = "";
                    hashTableInfor["经纪人单位开户许可证号"] = "";
                    hashTableInfor["经纪人单位开户许扫描件"] = "";
                    hashTableInfor["经纪人单位法定代表人姓名"] = "";
                    hashTableInfor["经纪人单位法定代表人身份证号"] = "";
                    hashTableInfor["经纪人单位法定代表人身份证扫描件"] = "";
                    hashTableInfor["经纪人单位法定代表人身份证反面扫描件"] = "";
                    hashTableInfor["经纪人单位法定代表人授权书"] = "";
                    hashTableInfor["经纪人单位交易方联系电话"] = "";
                    hashTableInfor["经纪人单位所属省份"] = "";
                    hashTableInfor["经纪人单位所属地市"] = "";
                    hashTableInfor["经纪人单位所属区县"] = "";
                    hashTableInfor["经纪人单位详细地址"] = "";
                    hashTableInfor["经纪人单位联系人姓名"] = "";
                    hashTableInfor["经纪人单位联系人手机号"] = "";
                    hashTableInfor["经纪人单位开户银行"] = "";
                    hashTableInfor["经纪人单位银行账号"] = "";
                    hashTableInfor["经纪人单位平台管理机构"] = "";
                    hashTableInfor["经纪人单位登录邮箱"] = HTuser["dlyx"].ToString();
                    hashTableInfor["经纪人单位用户名"] = HTuser["yhm"].ToString();
                    hashTableInfor["经纪人单位交易账户类别"] = "经纪人交易账户";
                    hashTableInfor["经纪人单位交易注册类别"] = "单位";
                    hashTableInfor["经纪人单位证券资金密码"] = "";
                    hashTableInfor["经纪人单位院系编号"] = "";
                    hashTableInfor["经纪人单位经纪人分类"] = "一般经纪人";
                    hashTableInfor["经纪人单位业务管理部门分类"] = "";
                    hashTableInfor["方法类别"] = "经纪人单位";//这里是为了方便选择处理方法


                    string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                    if (this.txtJJFMC.Text.Trim() == "")
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位交易方名称"] = this.txtJJFMC.Text.Trim();

                    }
                    //营业执照
                    if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemYYZZ.Text = "请填写营业执照注册号，并上传扫描件！";
                        this.labRemYYZZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtYYZZ.Text.Trim() != "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemYYZZ.Text = "请上传营业执照扫描件！";
                        this.labRemYYZZ.Visible = true;
                        JJRDWTag = "不完整";
                    }
                    else if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem != null && this.pan_SC_YYZZ.UpItem.Items.Count > 0))
                    {
                        this.labRemYYZZ.Text = "请填写营业执照注册号！";
                        this.labRemYYZZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemYYZZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位营业执照注册号"] = this.txtYYZZ.Text;
                        hashTableInfor["经纪人单位营业执照扫描件"] = this.pan_SC_YYZZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //组织机构代码证
                    if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码，并上传扫描件！";
                        this.labRemZZJGDMZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtZZJGDMZ.Text.Trim() != "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemZZJGDMZ.Text = "请上传组织机构代码证扫描件！";
                        this.labRemZZJGDMZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem != null && this.pan_SC_ZZJGDMZ.UpItem.Items.Count > 0))
                    {
                        this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码！";
                        this.labRemZZJGDMZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemZZJGDMZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位组织机构代码证代码证"] = this.txtZZJGDMZ.Text;
                        hashTableInfor["经纪人单位组织机构代码证代码证扫描件"] = this.pan_SC_ZZJGDMZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //税务登记证
                    if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSWDJZ.Text = "请填写税务登记证税号，并上传扫描件！";
                        this.labRemSWDJZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSWDJZ.Text.Trim() != "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSWDJZ.Text = "请上传税务登记证扫描件！";
                        this.labRemSWDJZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem != null && this.pan_SC_SWDJZ.UpItem.Items.Count > 0))
                    {
                        this.labRemSWDJZ.Text = "请填写税务登记证税号！";
                        this.labRemSWDJZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemSWDJZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位税务登记证税号"] = this.txtSWDJZ.Text;
                        hashTableInfor["经纪人单位税务登记证扫描件"] = this.pan_SC_SWDJZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    ////纳税人资格证
                    //if (this.pan_SC_NSRZGZ.UpItem == null || this.pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
                    //{
                    //    this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                    //    this.labRemNSRZGZ.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else if (this.pan_SC_NSRZGZ.UpItem != null && this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                    //{
                    //    this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                    //    this.labRemNSRZGZ.Visible = false;
                    //    JJRDWTag = "完整";
                    //    hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = this.pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    //}
                    if (this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                    {
                        hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = this.pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    else
                    {
                        hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = hastTableDBL["路径"].ToString();

                    }
                    //开户许可证 2014.7.21 取消注释
                    if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemKHXKZ.Text = "请填写开户许可证号，并上传扫描件！";
                        this.labRemKHXKZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtKHXKZ.Text.Trim() != "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemKHXKZ.Text = "请上传开户许可证扫描件！";
                        this.labRemKHXKZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem != null && this.pan_SC_KHXKZ.UpItem.Items.Count > 0))
                    {
                        this.labRemKHXKZ.Text = "请填写开户许可证号！";
                        this.labRemKHXKZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHXKZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位开户许可证号"] = this.txtKHXKZ.Text;
                        hashTableInfor["经纪人单位开户许扫描件"] = this.pan_SC_KHXKZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //预留印鉴卡
                    if (this.pan_SC_YLYJK.UpItem == null || this.pan_SC_YLYJK.UpItem.Items.Count <= 0)
                    {
                        this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                        this.labRemYLYJK.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.pan_SC_YLYJK.UpItem != null && this.pan_SC_YLYJK.UpItem.Items.Count > 0)
                    {
                        this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                        this.labRemYLYJK.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位预留印鉴卡扫描件"] = this.pan_SC_YLYJK.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    if (this.pan_SC_KHXKZ.UpItem.Items.Count > 0)
                    {
                        if (this.txtKHXKZ.Text.Trim() == "")
                        {
                            hashTableInfor["经纪人单位开户许可证号"] = "待补录";
                        }
                        else
                        {
                            hashTableInfor["经纪人单位开户许可证号"] = this.txtKHXKZ.Text;
                        }
                        hashTableInfor["经纪人单位开户许扫描件"] = this.pan_SC_KHXKZ.UpItem.Items[0].SubItems[1].Text.Trim();

                    }
                    else
                    {
                        if (this.txtKHXKZ.Text.Trim() == "")
                        {
                            hashTableInfor["经纪人单位开户许可证号"] = "待补录";
                        }
                        else
                        {
                            hashTableInfor["经纪人单位开户许可证号"] = this.txtKHXKZ.Text;
                        }
                        hashTableInfor["经纪人单位开户许扫描件"] = hastTableDBL["路径"].ToString();
                    }


                    //法定代表人姓名 2014.7.21 取消注释
                    if (this.txtFDDBR.Text.Trim() == "")
                    {
                        this.labRemFDDBR.Text = "请填写法定代表人姓名！";
                        this.labRemFDDBR.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemFDDBR.Text = "";
                        this.labRemFDDBR.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位法定代表人姓名"] = this.txtFDDBR.Text;
                    }
                    if (this.txtFDDBR.Text.Trim().ToString() == "")
                    {
                        hashTableInfor["经纪人单位法定代表人姓名"] = "待补录";
                    }
                    else
                    {
                        hashTableInfor["经纪人单位法定代表人姓名"] = this.txtFDDBR.Text;
                    }
                    //法定代表人身份证号扫描件
                    if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                    {
                        this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号，并上传正面扫描件！";
                        this.labRemFDDBRSHZH.Visible = true;
                        //this.TipFRSFZ.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                        {
                            this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                            this.labRemFDDBRSHZH.Visible = true;
                            //this.TipFRSFZ.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim()) == "身份证有效"))
                        // {
                        //     string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim());
                        //     if (strIdValueInfor == "身份证有效")
                        //     {

                       //     }
                        //     else
                        //     {
                        //         this.labRemFDDBRSHZH.Text = strIdValueInfor;
                        //         this.labRemFDDBRSHZH.Visible = true;
                        //         tag += 1;

                       //     }
                        //}
                        else
                        {
                            this.labRemFDDBRSHZH.Text = "请上传法定代表人身份证正面扫描件！";
                            this.labRemFDDBRSHZH.Visible = true;
                            //this.TipFRSFZ.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                    {
                        this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号！";
                        this.labRemFDDBRSHZH.Visible = true;
                        //this.TipFRSFZ.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                        {
                            this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                            this.labRemFDDBRSHZH.Visible = true;
                            //this.TipFRSFZ.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemFDDBRSHZH.Text = strIdValueInfor;
                        //        this.labRemFDDBRSHZH.Visible = true;
                        //        tag += 1;

                        //    }
                        //}
                        else
                        {


                            if (JudgeSFZH("法定代表人身份证号", this.txtFDDBRSHZH.Text.Trim()))
                            {
                                this.labRemFDDBRSHZH.Visible = false;
                                //this.TipFRSFZ.Visible = true;
                                JJRDWTag = "完整";
                                hashTableInfor["经纪人单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                                hashTableInfor["经纪人单位法定代表人身份证扫描件"] = this.pan_SC_FDDBRSHZH.UpItem.Items[0].SubItems[1].Text.Trim();
                            }
                            else
                            {
                                this.labRemFDDBRSHZH.Text = "此身份证号已经被注册！";
                                this.labRemFDDBRSHZH.Visible = true;
                                //this.TipFRSFZ.Visible = false;
                                JJRDWTag = "不完整";
                                tag += 1;
                            }
                        }
                    }

                    if (this.pan_SC_FDDBRSHZH_FM.UpItem == null || this.pan_SC_FDDBRSHZH_FM.UpItem.Items.Count <= 0)
                    {
                        this.labRemFDDBRSHZH_FM.Visible = true;
                        tag += 1;
                    }
                    else
                    {

                        this.labRemFDDBRSHZH_FM.Visible = false;
                        hashTableInfor["经纪人单位法定代表人身份证反面扫描件"] = this.pan_SC_FDDBRSHZH_FM.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //if (this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0)
                    //{
                    //    if (this.txtFDDBRSHZH.Text.Trim() == "")
                    //    {
                    //        hashTableInfor["经纪人单位法定代表人身份证号"] = "待补录";
                    //    }
                    //    else
                    //    {
                    //        hashTableInfor["经纪人单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                    //    }
                    //    hashTableInfor["经纪人单位法定代表人身份证扫描件"] = this.pan_SC_FDDBRSHZH.UpItem.Items[0].SubItems[1].Text.Trim();
                    //}
                    //else
                    //{
                    //    if (this.txtFDDBRSHZH.Text.Trim() == "")
                    //    {
                    //        hashTableInfor["经纪人单位法定代表人身份证号"] = "待补录";
                    //    }
                    //    else
                    //    {
                    //        hashTableInfor["经纪人单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                    //    }
                    //    hashTableInfor["经纪人单位法定代表人身份证扫描件"] = hastTableDBL["路径"].ToString();

                    //}




                    //}
                    //法定带代表人授权书 2014.7.21 取消注释
                    if (this.pan_SC_FDDBRSQS.UpItem == null || this.pan_SC_FDDBRSQS.UpItem.Items.Count <= 0)
                    {
                        this.labRemFDDBRSQS.Text = "请上传法定代表人授权书扫描件！";
                        this.labRemFDDBRSQS.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.pan_SC_FDDBRSQS.UpItem != null && this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                    {
                        this.labRemFDDBRSQS.Text = "";
                        this.labRemFDDBRSQS.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位法定代表人授权书"] = this.pan_SC_FDDBRSQS.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    if (this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                    {
                        hashTableInfor["经纪人单位法定代表人授权书"] = this.pan_SC_FDDBRSQS.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    else
                    {
                        hashTableInfor["经纪人单位法定代表人授权书"] = hastTableDBL["路径"].ToString();

                    }

                    if (this.txtJYFLXDH.Text.Trim() == "")
                    {
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                    {
                        labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemJYFLXDH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位交易方联系电话"] = this.txtJYFLXDH.Text;
                    }

                    if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                    {
                        labRemSSQY.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemSSQY.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位所属省份"] = ucSSQY.SelectedItem[0];
                        hashTableInfor["经纪人单位所属地市"] = ucSSQY.SelectedItem[1];
                        hashTableInfor["经纪人单位所属区县"] = ucSSQY.SelectedItem[2];
                    }

                    if (this.txtXXDZ.Text == "")
                    {
                        this.labRemXXDZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemXXDZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位详细地址"] = this.txtXXDZ.Text;
                    }

                    if (this.txtLXRXM.Text == "")
                    {
                        this.labRemLXRXM.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRXM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位联系人姓名"] = this.txtLXRXM.Text;
                    }

                    if (this.txtLXRSJH.Text == "")
                    {
                        this.labRemLXRSJH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                    {
                        this.labRemLXRSJH.Visible = true;
                        this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemLXRSJH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位联系人手机号"] = this.txtLXRSJH.Text;
                    }
                    if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                    {
                        this.labRemKHYH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHYH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位开户银行"] = this.cbxKHYH.Text;
                    }


                    //if (this.cbxKHYH.Text.Trim() == "")
                    //{
                    //    hashTableInfor["经纪人单位开户银行"] = "待补录";
                    //}
                    //else
                    //{
                    //    hashTableInfor["经纪人单位开户银行"] = this.cbxKHYH.Text;
                    //}


                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    this.labRemYHZH.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemYHZH.Visible = false;
                    //    JJRDWTag = "完整";
                    //    hashTableInfor["经纪人单位银行账号"] = this.txtYHZH.Text.Trim();
                    //}

                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    hashTableInfor["经纪人单位银行账号"] = "待补录";
                    //}
                    //else
                    //{
                    //    hashTableInfor["经纪人单位银行账号"] = this.txtYHZH.Text.Trim();
                    //}
                    //交易资金密码
                    if (this.txtZQZJMM.Text.Trim() == "")
                    {
                        this.labZQZJMM.Visible = true;
                        label6.Visible = !this.labZQZJMM.Visible;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                    {
                        this.labZQZJMM.Visible = true;
                        this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                        label6.Visible = !this.labZQZJMM.Visible;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labZQZJMM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人单位证券资金密码"] = this.txtZQZJMM.Text;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }



                    if (this.radFGS.Checked == true)//选择分公司
                    {
                        if (this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString() == "请选择业务管理部门")
                        {
                            this.labRemGLJG.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else
                        {
                            this.labRemGLJG.Visible = false;
                            JJRDWTag = "完整";
                            hashTableInfor["经纪人单位平台管理机构"] = this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString();
                            hashTableInfor["经纪人单位业务管理部门分类"] = "分公司";
                        }
                    }

                    if (this.radYWTZB.Checked == true)//平台总部
                    {
                        hashTableInfor["经纪人单位平台管理机构"] = "平台总部";
                        hashTableInfor["经纪人单位业务管理部门分类"] = "平台总部";
                    }

                    if (this.radGXTW.Checked == true)//高校团委
                    {
                        if (String.IsNullOrEmpty(this.labGXTW_ZhangHao.Text))
                        {
                            this.labRemGXTW.Visible = true;
                            tag += 1;
                        }
                        else
                        {
                            this.labRemGXTW.Visible = false;
                        }
                        //ComboBoxItem sex_item = (ComboBoxItem)cbxSZYX.SelectedItem;
                        if (String.IsNullOrEmpty(this.txtSZYX.Text.Trim()))
                        {
                            this.labRemXZYX.Visible = true;
                            tag += 1;
                        }
                        else
                        {
                            this.labRemXZYX.Visible = false;
                            hashTableInfor["经纪人单位平台管理机构"] = this.labTWMC.Text.Trim();
                            hashTableInfor["经纪人单位院系编号"] = this.txtSZYX.Text.Trim();
                            hashTableInfor["经纪人单位业务管理部门分类"] = "高校团委";
                        }


                    }
                    if (tag == 0)
                    {
                        //禁用提交区域并开启进度
                        flowLayoutPanel1.Enabled = false;
                        PBload.Visible = true;
                        //提交账户信息
                        SRT_SubmitJYZHXX_Run();
                    }
                    else
                    {
                        return;
                    }

                }
                else if (this.radZRR.Checked == true)//自然人注册
                {
                    this.labRemJYZH.Visible = false;
                    this.labRemZCLB.Visible = false;

                    int tag = 0;



                    hashTableInfor["经纪人个人交易账户类别"] = "";
                    hashTableInfor["经纪人个人交易注册类别"] = "";
                    hashTableInfor["经纪人个人交易方名称"] = "";
                    hashTableInfor["经纪人个人身份证号"] = "";
                    hashTableInfor["经纪人个人身份证扫描件"] = "";
                    hashTableInfor["经纪人个人身份证反面扫描件"] = "";
                    hashTableInfor["经纪人个人交易方联系电话"] = "";
                    hashTableInfor["经纪人个人所属省份"] = "";
                    hashTableInfor["经纪人个人所属地市"] = "";
                    hashTableInfor["经纪人个人所属区县"] = "";
                    hashTableInfor["经纪人个人详细地址"] = "";
                    hashTableInfor["经纪人个人联系人姓名"] = "";
                    hashTableInfor["经纪人个人联系人手机号"] = "";
                    hashTableInfor["经纪人个人开户银行"] = "";
                    hashTableInfor["经纪人个人银行账号"] = "";
                    hashTableInfor["经纪人个人平台管理机构"] = "";
                    hashTableInfor["经纪人个人登录邮箱"] = HTuser["dlyx"].ToString();
                    hashTableInfor["经纪人个人用户名"] = HTuser["yhm"].ToString();
                    hashTableInfor["经纪人个人交易账户类别"] = "经纪人交易账户";
                    hashTableInfor["经纪人个人交易注册类别"] = "自然人";
                    hashTableInfor["经纪人个人院系编号"] = "";
                    hashTableInfor["经纪人个人经纪人分类"] = "一般经纪人";
                    hashTableInfor["经纪人个人业务管理部门分类"] = "";
                    hashTableInfor["方法类别"] = "经纪人个人";//这里是为了方便选择处理方法    
                    hashTableInfor["经纪人个人证券资金密码"] = "";


                    string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                    if (this.txtJJFMC.Text.Trim() == "")
                    {
                        this.labRemJJFMC.Text = "请输入自然人姓名！";
                        this.labRemJJFMC.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemJJFMC.Text = "请输入自然人姓名！";
                        this.labRemJJFMC.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人交易方名称"] = this.txtJJFMC.Text.Trim();

                    }
                    //身份证扫描件
                    if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                        this.labRemSFZ.Visible = true;
                        //this.TipsSFZZFM.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                        {
                            this.labRemSFZ.Text = "请填写正确的身份证号！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else if (!(Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim()) == "身份证有效"))
                        {
                            string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim());
                            if (strIdValueInfor == "身份证有效")
                            {

                            }
                            else
                            {
                                this.labRemSFZ.Text = strIdValueInfor;
                                this.labRemSFZ.Visible = true;
                                tag += 1;
                            }
                        }
                        else
                        {
                            this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                    }
                    else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                    {
                        this.labRemSFZ.Text = "请填写身份证号！";
                        this.labRemSFZ.Visible = true;
                        //this.TipsSFZZFM.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                        {
                            this.labRemSFZ.Text = "请填写正确的身份证号！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else if (!(Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim()) == "身份证有效"))
                        {
                            string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim());
                            if (strIdValueInfor == "身份证有效")
                            {

                            }
                            else
                            {
                                this.labRemSFZ.Text = strIdValueInfor;
                                this.labRemSFZ.Visible = true;
                                tag += 1;
                            }
                        }
                        else
                        {
                            if (JudgeSFZH("个人身份证号", this.txtSFZ.Text.Trim()))
                            {
                                this.labRemSFZ.Visible = false;
                                //this.TipsSFZZFM.Visible = true;
                                JJRDWTag = "完整";
                                hashTableInfor["经纪人个人身份证号"] = this.txtSFZ.Text.Trim();
                                hashTableInfor["经纪人个人身份证扫描件"] = this.pan_SC_SFZ.UpItem.Items[0].SubItems[1].Text.Trim();
                            }
                            else
                            {
                                this.labRemSFZ.Text = "此身份证号已经被注册！";
                                this.labRemSFZ.Visible = true;
                                //this.TipsSFZZFM.Visible = false;
                                JJRDWTag = "不完整";
                                tag += 1;
                            }
                        }
                    }

                    //身份证反面扫描件
                    if (this.pan_SC_SFZ_FM.UpItem == null || this.pan_SC_SFZ_FM.UpItem.Items.Count <= 0)
                    {
                        this.labRemSFZ_FM.Visible = true;
                        tag += 1;
                    }
                    else
                    {
                        this.labRemSFZ_FM.Visible = false;
                        hashTableInfor["经纪人个人身份证反面扫描件"] = this.pan_SC_SFZ_FM.UpItem.Items[0].SubItems[1].Text.Trim();
                    }

                    if (this.txtJYFLXDH.Text.Trim() == "")
                    {
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                    {
                        labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemJYFLXDH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人交易方联系电话"] = this.txtJYFLXDH.Text;
                    }
                    if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                    {
                        labRemSSQY.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemSSQY.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人所属省份"] = ucSSQY.SelectedItem[0];
                        hashTableInfor["经纪人个人所属地市"] = ucSSQY.SelectedItem[1];
                        hashTableInfor["经纪人个人所属区县"] = ucSSQY.SelectedItem[2];
                    }

                    if (this.txtXXDZ.Text == "")
                    {
                        this.labRemXXDZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemXXDZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人详细地址"] = this.txtXXDZ.Text;
                    }

                    if (this.txtLXRXM.Text == "")
                    {
                        this.labRemLXRXM.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRXM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人联系人姓名"] = this.txtLXRXM.Text;
                    }

                    if (this.txtLXRSJH.Text == "")
                    {
                        this.labRemLXRSJH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                    {
                        this.labRemLXRSJH.Visible = true;
                        this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemLXRSJH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人联系人手机号"] = this.txtLXRSJH.Text;
                    }
                    if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                    {
                        this.labRemKHYH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHYH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人开户银行"] = this.cbxKHYH.Text;
                    }

                    //if (this.cbxKHYH.Text.Trim() == "")
                    //{
                    //    hashTableInfor["经纪人个人开户银行"] = "待补录";

                    //}
                    //else
                    //{
                    //    hashTableInfor["经纪人个人开户银行"] = this.cbxKHYH.Text;
                    //}

                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    this.labRemYHZH.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemYHZH.Visible = false;
                    //    JJRDWTag = "完整";
                    //    hashTableInfor["经纪人个人银行账号"] = this.txtYHZH.Text.Trim();
                    //}
                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    hashTableInfor["经纪人个人银行账号"] = "待补录";
                    //}
                    //else
                    //{
                    //    hashTableInfor["经纪人个人银行账号"] = this.txtYHZH.Text.Trim();

                    //}
                    if (this.radFGS.Checked == true)//选择分公司
                    {
                        if (this.cbxGLJG.Text == "请选择业务管理部门")
                        {
                            this.labRemGLJG.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else
                        {
                            this.labRemGLJG.Visible = false;
                            JJRDWTag = "完整";
                            hashTableInfor["经纪人个人平台管理机构"] = this.cbxGLJG.Text;
                            hashTableInfor["经纪人个人业务管理部门分类"] = "分公司";
                        }
                    }
                    if (this.radYWTZB.Checked == true)//平台总部
                    {
                        hashTableInfor["经纪人个人平台管理机构"] = "平台总部";
                        hashTableInfor["经纪人个人业务管理部门分类"] = "平台总部";
                    }

                    if (this.radGXTW.Checked == true)//高校团委
                    {
                        if (String.IsNullOrEmpty(this.labGXTW_ZhangHao.Text))
                        {
                            this.labRemGXTW.Visible = true;
                            tag += 1;
                        }
                        else
                        {
                            this.labRemGXTW.Visible = false;
                        }
                        //ComboBoxItem sex_item = (ComboBoxItem)cbxSZYX.SelectedItem;
                        if (String.IsNullOrEmpty(this.txtSZYX.Text.Trim()))
                        {
                            this.labRemXZYX.Visible = true;
                            tag += 1;
                        }
                        else
                        {
                            this.labRemXZYX.Visible = false;
                            hashTableInfor["经纪人个人平台管理机构"] = this.labTWMC.Text.Trim();
                            hashTableInfor["经纪人个人院系编号"] = this.txtSZYX.Text.Trim();
                            hashTableInfor["经纪人个人业务管理部门分类"] = "高校团委";
                        }


                    }

                    //交易资金密码
                    if (this.txtZQZJMM.Text.Trim() == "")
                    {
                        this.labZQZJMM.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }
                    else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                    {
                        this.labZQZJMM.Visible = true;
                        this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                        JJRDWTag = "不完整";
                        tag += 1;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }
                    else
                    {
                        this.labZQZJMM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人证券资金密码"] = this.txtZQZJMM.Text;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }





                    if (tag == 0)
                    {
                        //禁用提交区域并开启进度
                        flowLayoutPanel1.Enabled = false;
                        PBload.Visible = true;

                        //提交账户信息
                        SRT_SubmitJYZHXX_Run();
                    }
                    else
                    {
                        return;
                    }

                }

            }
            #endregion
            #region//买卖家交易账户
            else if (this.radMMJ.Checked == true)//买卖家交易账户
            {
                if (this.radDW.Checked == true)//单位注册
                {
                    this.labRemZCLB.Visible = false;
                    this.labRemJYZH.Visible = false;
                    int tag = 0;
                    hashTableInfor["买卖家单位交易账户类别"] = "";
                    hashTableInfor["买卖家单位交易注册类别"] = "";
                    hashTableInfor["买卖家单位交易方名称"] = "";
                    hashTableInfor["买卖家单位营业执照注册号"] = "";
                    hashTableInfor["买卖家单位营业执照扫描件"] = "";
                    hashTableInfor["买卖家单位组织机构代码证代码证"] = "";
                    hashTableInfor["买卖家单位组织机构代码证代码证扫描件"] = "";
                    hashTableInfor["买卖家单位税务登记证税号"] = "";
                    hashTableInfor["买卖家单位税务登记证扫描件"] = "";
                    hashTableInfor["买卖家单位一般纳税人资格证扫描件"] = "";
                    hashTableInfor["买卖家单位开户许可证号"] = "";
                    hashTableInfor["买卖家单位开户许扫描件"] = "";
                    hashTableInfor["买卖家单位法定代表人姓名"] = "";
                    hashTableInfor["买卖家单位法定代表人身份证号"] = "";
                    hashTableInfor["买卖家单位法定代表人身份证扫描件"] = "";
                    hashTableInfor["买卖家单位法定代表人身份证反面扫描件"] = "";
                    hashTableInfor["买卖家单位法定代表人授权书"] = "";
                    hashTableInfor["买卖家单位交易方联系电话"] = "";
                    hashTableInfor["买卖家单位所属省份"] = "";
                    hashTableInfor["买卖家单位所属地市"] = "";
                    hashTableInfor["买卖家单位所属区县"] = "";
                    hashTableInfor["买卖家单位详细地址"] = "";
                    hashTableInfor["买卖家单位联系人姓名"] = "";
                    hashTableInfor["买卖家单位联系人手机号"] = "";
                    hashTableInfor["买卖家单位开户银行"] = "";
                    hashTableInfor["买卖家单位银行账号"] = "";
                    hashTableInfor["买卖家单位经纪人资格证书编号"] = "";
                    hashTableInfor["买卖家单位经纪人经纪人名称"] = "";
                    hashTableInfor["买卖家单位经纪人联系电话"] = "";
                    hashTableInfor["买卖家单位登录邮箱"] = HTuser["dlyx"].ToString();
                    hashTableInfor["买卖家单位用户名"] = HTuser["yhm"].ToString();
                    hashTableInfor["买卖家单位交易账户类别"] = "买家卖家交易账户";
                    hashTableInfor["买卖家单位交易注册类别"] = "单位";
                    hashTableInfor["方法类别"] = "买卖家单位";//这里是为了方便选择处理方法
                    hashTableInfor["买卖家单位证券资金密码"] = "";
                    hashTableInfor["买卖家单位银行工作人员工号"] = "";
                    hashTableInfor["买卖家单位关联银行"] = "";
                    hashTableInfor["买卖家单位业务服务部门"] = "";
                    if(this.radFW_JJR.Checked==true||this.radFW_Bank.Checked==true)
                    {
                        hashTableInfor["买卖家单位业务服务部门"] = lblJJRFL.Text.Trim();
                    }

                    string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                    if (this.txtJJFMC.Text.Trim() == "")
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位交易方名称"] = this.txtJJFMC.Text.Trim();

                    }
                    //营业执照
                    if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemYYZZ.Text = "请填写营业执照注册号，并上传扫描件！";
                        this.labRemYYZZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtYYZZ.Text.Trim() != "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemYYZZ.Text = "请上传营业执照扫描件！";
                        this.labRemYYZZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem != null && this.pan_SC_YYZZ.UpItem.Items.Count > 0))
                    {
                        this.labRemYYZZ.Text = "请填写营业执照注册号！";
                        this.labRemYYZZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemYYZZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位营业执照注册号"] = this.txtYYZZ.Text;
                        hashTableInfor["买卖家单位营业执照扫描件"] = this.pan_SC_YYZZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //组织机构代码证
                    if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码，并上传扫描件！";
                        this.labRemZZJGDMZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtZZJGDMZ.Text.Trim() != "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemZZJGDMZ.Text = "请上传组织机构代码证扫描件！";
                        this.labRemZZJGDMZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem != null && this.pan_SC_ZZJGDMZ.UpItem.Items.Count > 0))
                    {
                        this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码！";
                        this.labRemZZJGDMZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemZZJGDMZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位组织机构代码证代码证"] = this.txtZZJGDMZ.Text;
                        hashTableInfor["买卖家单位组织机构代码证代码证扫描件"] = this.pan_SC_ZZJGDMZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //税务登记证
                    if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSWDJZ.Text = "请填写税务登记证税号，并上传扫描件！";
                        this.labRemSWDJZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSWDJZ.Text.Trim() != "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSWDJZ.Text = "请上传税务登记证扫描件！";
                        this.labRemSWDJZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem != null && this.pan_SC_SWDJZ.UpItem.Items.Count > 0))
                    {
                        this.labRemSWDJZ.Text = "请填写税务登记证税号！";
                        this.labRemSWDJZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemSWDJZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位税务登记证税号"] = this.txtSWDJZ.Text;
                        hashTableInfor["买卖家单位税务登记证扫描件"] = this.pan_SC_SWDJZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //纳税人资格证
                    //if (this.pan_SC_NSRZGZ.UpItem == null || this.pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
                    //{
                    //    this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                    //    this.labRemNSRZGZ.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else if (this.pan_SC_NSRZGZ.UpItem != null && this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                    //{
                    //    this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                    //    this.labRemNSRZGZ.Visible = false;
                    //    JJRDWTag = "完整";
                    //    hashTableInfor["买卖家单位一般纳税人资格证扫描件"] = this.pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    //}
                    if (this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                    {
                        hashTableInfor["买卖家单位一般纳税人资格证扫描件"] = this.pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    else
                    {
                        hashTableInfor["买卖家单位一般纳税人资格证扫描件"] = hastTableDBL["路径"].ToString();

                    }
                    //开户许可证 2014.7.21 取消注释
                    if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemKHXKZ.Text = "请填写开户许可证号，并上传开户许可证扫描件！";
                        this.labRemKHXKZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtKHXKZ.Text.Trim() != "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemKHXKZ.Text = "请上传开户许可证扫描件！";
                        this.labRemKHXKZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem != null && this.pan_SC_KHXKZ.UpItem.Items.Count > 0))
                    {
                        this.labRemKHXKZ.Text = "请填写开户许可证号！";
                        this.labRemKHXKZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHXKZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位开户许可证号"] = this.txtKHXKZ.Text;
                        hashTableInfor["买卖家单位开户许扫描件"] = this.pan_SC_KHXKZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //预留印鉴卡
                    if (this.pan_SC_YLYJK.UpItem == null || this.pan_SC_YLYJK.UpItem.Items.Count <= 0)
                    {
                        this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                        this.labRemYLYJK.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.pan_SC_YLYJK.UpItem != null && this.pan_SC_YLYJK.UpItem.Items.Count > 0)
                    {
                        this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                        this.labRemYLYJK.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位预留印鉴卡扫描件"] = this.pan_SC_YLYJK.UpItem.Items[0].SubItems[1].Text.Trim();
                    }

                    if (this.pan_SC_KHXKZ.UpItem.Items.Count > 0)
                    {
                        if (this.txtKHXKZ.Text.Trim() == "")
                        {
                            hashTableInfor["买卖家单位开户许可证号"] = "待补录";
                        }
                        else
                        {
                            hashTableInfor["买卖家单位开户许可证号"] = this.txtKHXKZ.Text;
                        }
                        hashTableInfor["买卖家单位开户许扫描件"] = this.pan_SC_KHXKZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    else
                    {
                        if (this.txtKHXKZ.Text.Trim() == "")
                        {
                            hashTableInfor["买卖家单位开户许可证号"] = "待补录";
                        }
                        else
                        {
                            hashTableInfor["买卖家单位开户许可证号"] = this.txtKHXKZ.Text;
                        }
                        hashTableInfor["买卖家单位开户许扫描件"] = hastTableDBL["路径"].ToString();

                    }
                    //法定代表人姓名 2014.7.21 取消注释
                    if (this.txtFDDBR.Text.Trim() == "")
                    {
                        this.labRemFDDBR.Text = "请填写法定代表人姓名！";
                        this.labRemFDDBR.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemFDDBR.Text = "";
                        this.labRemFDDBR.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位法定代表人姓名"] = this.txtFDDBR.Text;
                    }
                    if (this.txtFDDBR.Text.Trim() == "")
                    {
                        hashTableInfor["买卖家单位法定代表人姓名"] = "待补录";
                    }
                    else
                    {
                        hashTableInfor["买卖家单位法定代表人姓名"] = this.txtFDDBR.Text;
                    }



                    //法定代表人身份证号扫描件
                    if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                    {
                        this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号，并上传正面扫描件！";
                        this.labRemFDDBRSHZH.Visible = true;
                        //  this.TipFRSFZ.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                        {
                            this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                            this.labRemFDDBRSHZH.Visible = true;
                            //this.TipFRSFZ.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemFDDBRSHZH.Text = strIdValueInfor;
                        //        this.labRemFDDBRSHZH.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            this.labRemFDDBRSHZH.Text = "请上传法定代表人身份证正面扫描件！";
                            this.labRemFDDBRSHZH.Visible = true;
                            // this.TipFRSFZ.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                    {
                        this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号！";
                        this.labRemFDDBRSHZH.Visible = true;
                        // this.TipFRSFZ.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                        {
                            this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                            this.labRemFDDBRSHZH.Visible = true;
                            // this.TipFRSFZ.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemFDDBRSHZH.Text = strIdValueInfor;
                        //        this.labRemFDDBRSHZH.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            if (JudgeSFZH("法定代表人身份证号", this.txtFDDBRSHZH.Text.Trim()))
                            {

                                this.labRemFDDBRSHZH.Visible = false;
                                //  this.TipFRSFZ.Visible = true;
                                JJRDWTag = "完整";
                                hashTableInfor["买卖家单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                                hashTableInfor["买卖家单位法定代表人身份证扫描件"] = this.pan_SC_FDDBRSHZH.UpItem.Items[0].SubItems[1].Text.Trim();
                            }
                            else
                            {
                                this.labRemFDDBRSHZH.Text = "此身份证号已经被注册！";
                                this.labRemFDDBRSHZH.Visible = true;
                                // this.TipFRSFZ.Visible = false;
                                JJRDWTag = "不完整";
                                tag += 1;
                            }



                        }
                    }


                    if (this.pan_SC_FDDBRSHZH_FM.UpItem == null || this.pan_SC_FDDBRSHZH_FM.UpItem.Items.Count <= 0)
                    {
                        this.labRemFDDBRSHZH_FM.Visible = true;
                        tag += 1;
                    }
                    else
                    {

                        this.labRemFDDBRSHZH_FM.Visible = false;
                        hashTableInfor["买卖家单位法定代表人身份证反面扫描件"] = this.pan_SC_FDDBRSHZH_FM.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //if(this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0)
                    //{
                    //  if(this.txtFDDBRSHZH.Text.Trim() == "")
                    //  {
                    //  hashTableInfor["买卖家单位法定代表人身份证号"] ="待补录";

                    //  }
                    //    else
                    //  {

                    //  hashTableInfor["买卖家单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                    //  }
                    //     hashTableInfor["买卖家单位法定代表人身份证扫描件"] = this.pan_SC_FDDBRSHZH.UpItem.Items[0].SubItems[1].Text.Trim();
                    //}
                    //else
                    //{
                    //  if(this.txtFDDBRSHZH.Text.Trim() == "")
                    //  {
                    //  hashTableInfor["买卖家单位法定代表人身份证号"] ="待补录";

                    //  }
                    //    else
                    //  {

                    //  hashTableInfor["买卖家单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                    //  }
                    //     hashTableInfor["买卖家单位法定代表人身份证扫描件"] = hastTableDBL["路径"].ToString();


                    //}

                    //法定带代表人授权书 2014.7.21 取消注释
                    if (this.pan_SC_FDDBRSQS.UpItem == null || this.pan_SC_FDDBRSQS.UpItem.Items.Count <= 0)
                    {
                        this.labRemFDDBRSQS.Text = "请上传法定代表人授权书扫描件！";
                        this.labRemFDDBRSQS.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.pan_SC_FDDBRSQS.UpItem != null && this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                    {
                        this.labRemFDDBRSQS.Text = "";
                        this.labRemFDDBRSQS.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位法定代表人授权书"] = this.pan_SC_FDDBRSQS.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    if (this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                    {
                        hashTableInfor["买卖家单位法定代表人授权书"] = this.pan_SC_FDDBRSQS.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    else
                    {
                        hashTableInfor["买卖家单位法定代表人授权书"] = hastTableDBL["路径"].ToString();

                    }

                    if (this.txtJYFLXDH.Text.Trim() == "")
                    {
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                    {
                        labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemJYFLXDH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位交易方联系电话"] = this.txtJYFLXDH.Text;
                    }

                    if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                    {
                        labRemSSQY.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemSSQY.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位所属省份"] = ucSSQY.SelectedItem[0];
                        hashTableInfor["买卖家单位所属地市"] = ucSSQY.SelectedItem[1];
                        hashTableInfor["买卖家单位所属区县"] = ucSSQY.SelectedItem[2];
                    }

                    if (this.txtXXDZ.Text == "")
                    {
                        this.labRemXXDZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemXXDZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位详细地址"] = this.txtXXDZ.Text;
                    }

                    if (this.txtLXRXM.Text == "")
                    {
                        this.labRemLXRXM.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRXM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位联系人姓名"] = this.txtLXRXM.Text;
                    }

                    if (this.txtLXRSJH.Text == "")
                    {
                        this.labRemLXRSJH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                    {
                        this.labRemLXRSJH.Visible = true;
                        this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemLXRSJH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位联系人手机号"] = this.txtLXRSJH.Text;
                    }
                    if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                    {
                        this.labRemKHYH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHYH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位开户银行"] = this.cbxKHYH.Text;
                    }
                    //if (this.cbxKHYH.Text.Trim() == "")
                    //{
                    // hashTableInfor["买卖家单位开户银行"] = "待补录";
                    //}
                    // else
                    //{
                    //  hashTableInfor["买卖家单位开户银行"] = this.cbxKHYH.Text;

                    //}





                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    this.labRemYHZH.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemYHZH.Visible = false;
                    //    JJRDWTag = "完整";
                    //    hashTableInfor["买卖家单位银行账号"] = this.txtYHZH.Text.Trim();
                    //}

                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    // hashTableInfor["买卖家单位银行账号"] = "待补录";
                    //}
                    //else
                    //{
                    // hashTableInfor["买卖家单位银行账号"] = this.txtYHZH.Text.Trim();
                    //}



                    if (this.radFW_Bank.Checked == true)
                    {
                    }
                    else
                    {
                        if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                        {
                            this.lablRemJJRZGZS.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else
                        {
                            this.lablRemJJRZGZS.Visible = false;
                            JJRDWTag = "完整";
                        }
                    }
                    

                    if (this.radFW_JJR.Checked == true)//选择一般经纪人
                    {


                        if (this.txtJJRZGZS.Text.Trim() == "")
                        {
                            this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                            this.lablRemJJRZGZS.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                        {
                            this.lablRemJJRZGZS.Visible = false;
                            JJRDWTag = "完整";
                            hashTableInfor["买卖家单位经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                            hashTableInfor["买卖家单位经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                            hashTableInfor["买卖家单位经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                        }
                        else
                        {
                            this.lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                            this.lablRemJJRZGZS.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;

                        }
                    }
                    else if (this.radFW_Bank.Checked == true)
                    {
                        if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                        {
                            tag += 1;
                            this.labRemYHMC.Visible = true;
                            JJRDWTag = "不完整";
                        }
                        else
                        {
                            hashTableInfor["买卖家单位关联银行"] = this.labYHMC.Text.Trim();
                            this.labRemYHMC.Visible = false;
                            JJRDWTag = "完整";
                        }
                        if (this.uctextBankYGGH.Text.Trim() == "")
                        {
                            tag += 1;
                            this.lblBankYGGH.Visible = true;
                            JJRDWTag = "不完整";
                        }
                        else
                        {
                            hashTableInfor["买卖家单位银行工作人员工号"] = this.uctextBankYGGH.Text.Trim();
                            this.lblBankYGGH.Visible = false;
                            JJRDWTag = "完整";
                        }
                        hashTableInfor["买卖家单位经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                        hashTableInfor["买卖家单位经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                        hashTableInfor["买卖家单位经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                        {
                            tag += 1;
                            this.labRemYHMC.Visible = true;
                        }
                        else
                        {
                            this.labRemYHMC.Visible = false;
                        }
                        if (String.IsNullOrEmpty(this.uctextBankYGGH.Text.Trim()))
                        {
                            tag += 1;
                            this.lblBankYGGH.Visible = true;
                        }
                        else
                        {
                            this.lblBankYGGH.Visible = false;
                        }

                        if (String.IsNullOrEmpty(this.labZFMC.Text))
                        {
                            tag += 1;
                            this.labRemZFMC.Visible = true;
                        }
                        else
                        {
                            this.labRemZFMC.Visible = false;
                        }

                        if (String.IsNullOrEmpty(this.labHangYeXieHui.Text))
                        {
                            tag += 1;
                          this.labRemHangYeXieHui.Visible=true;
                        }
                        else
                        {
                            this.labRemHangYeXieHui.Visible = false;
                        }

                        if (this.txtJJRZGZS.Text.Trim() == "")
                        {
                            this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                            this.lablRemJJRZGZS.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else
                        {
                            this.lablRemJJRZGZS.Text = "您填写的经纪人资格证书编号无效！";
                            this.lablRemJJRZGZS.Visible = true;
                            tag += 1;
                        }
                    
                    }

                    //交易资金密码
                    if (this.txtZQZJMM.Text.Trim() == "")
                    {
                        this.labZQZJMM.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }
                    else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                    {
                        this.labZQZJMM.Visible = true;
                        this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                        JJRDWTag = "不完整";
                        tag += 1;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }
                    else
                    {
                        this.labZQZJMM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家单位证券资金密码"] = this.txtZQZJMM.Text;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }






                    if (tag == 0)
                    {
                        //禁用提交区域并开启进度
                        flowLayoutPanel1.Enabled = false;
                        PBload.Visible = true;
                        //提交账户信息
                        SRT_SubmitJYZHXX_Run();
                    }
                    else
                    {
                        return;
                    }



                }
                else if (this.radZRR.Checked == true)//自然人注册
                {
                    int tag = 0;
                    hashTableInfor["买卖家个人交易账户类别"] = "";
                    hashTableInfor["买卖家个人交易注册类别"] = "";
                    hashTableInfor["买卖家个人交易方名称"] = "";
                    hashTableInfor["买卖家个人身份证号"] = "";
                    hashTableInfor["买卖家个人身份证扫描件"] = "";
                    hashTableInfor["买卖家个人身份证反面扫描件"] = "";
                    hashTableInfor["买卖家个人交易方联系电话"] = "";
                    hashTableInfor["买卖家个人所属省份"] = "";
                    hashTableInfor["买卖家个人所属地市"] = "";
                    hashTableInfor["买卖家个人所属区县"] = "";
                    hashTableInfor["买卖家个人详细地址"] = "";
                    hashTableInfor["买卖家个人联系人姓名"] = "";
                    hashTableInfor["买卖家个人联系人手机号"] = "";
                    hashTableInfor["买卖家个人开户银行"] = "";
                    hashTableInfor["买卖家个人银行账号"] = "";
                    hashTableInfor["买卖家个人经纪人资格证书编号"] = "";
                    hashTableInfor["买卖家个人经纪人经纪人名称"] = "";
                    hashTableInfor["买卖家个人经纪人联系电话"] = "";
                    hashTableInfor["买卖家个人登录邮箱"] = HTuser["dlyx"].ToString();
                    hashTableInfor["买卖家个人用户名"] = HTuser["yhm"].ToString();
                    hashTableInfor["买卖家个人交易账户类别"] = "买家卖家交易账户";
                    hashTableInfor["买卖家个人交易注册类别"] = "自然人";
                    hashTableInfor["方法类别"] = "买卖家个人";//这里是为了方便选择处理方法      
                    hashTableInfor["买卖家个人证券资金密码"] = "";
                    hashTableInfor["买卖家个人关联银行工作人员工号"] = "";
                    hashTableInfor["买卖家个人关联银行"] = "";
                    hashTableInfor["买卖家个人业务服务部门"] = "";
                    if (this.radFW_JJR.Checked == true || this.radFW_Bank.Checked == true)
                    {
                        hashTableInfor["买卖家个人业务服务部门"] = lblJJRFL.Text.Trim();
                    }

                    string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                    if (this.txtJJFMC.Text.Trim() == "")
                    {
                        this.labRemJJFMC.Text = "请输入真实的自然人姓名！";
                        this.labRemJJFMC.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家个人交易方名称"] = this.txtJJFMC.Text.Trim();

                    }
                    //身份证扫描件
                    if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                        this.labRemSFZ.Visible = true;
                        // this.TipsSFZZFM.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                    {

                        if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                        {
                            this.labRemSFZ.Text = "请填写正确的身份证号！";
                            this.labRemSFZ.Visible = true;
                            //  this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemSFZ.Text = strIdValueInfor;
                        //        this.labRemSFZ.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                            this.labRemSFZ.Visible = true;
                            // this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                    }
                    else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                    {
                        this.labRemSFZ.Text = "请填写身份证号！";
                        this.labRemSFZ.Visible = true;
                        //  this.TipsSFZZFM.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                        {
                            this.labRemSFZ.Text = "请填写正确的身份证号！";
                            this.labRemSFZ.Visible = true;
                            // this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemSFZ.Text = strIdValueInfor;
                        //        this.labRemSFZ.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {

                            if (JudgeSFZH("法定代表人身份证号", this.txtSFZ.Text.Trim()))
                            {

                                this.labRemSFZ.Visible = false;
                                // this.TipsSFZZFM.Visible = true;
                                JJRDWTag = "完整";
                                hashTableInfor["买卖家个人身份证号"] = this.txtSFZ.Text.Trim();
                                hashTableInfor["买卖家个人身份证扫描件"] = this.pan_SC_SFZ.UpItem.Items[0].SubItems[1].Text.Trim();
                            }
                            else
                            {
                                this.labRemSFZ.Text = "此身份证号已经被注册！";
                                this.labRemSFZ.Visible = true;
                                // this.TipsSFZZFM.Visible = false;
                                JJRDWTag = "不完整";
                                tag += 1;
                            }
                        }
                    }
                    //身份证反面扫描件
                    if (this.pan_SC_SFZ_FM.UpItem == null || this.pan_SC_SFZ_FM.UpItem.Items.Count <= 0)
                    {
                        this.labRemSFZ_FM.Visible = true;
                        tag += 1;
                    }
                    else
                    {
                        this.labRemSFZ_FM.Visible = false;
                        hashTableInfor["买卖家个人身份证反面扫描件"] = this.pan_SC_SFZ_FM.UpItem.Items[0].SubItems[1].Text.Trim();
                    }



                    if (this.txtJYFLXDH.Text.Trim() == "")
                    {
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isDH(this.txtJYFLXDH.Text.Trim()))
                    {
                        labRemJYFLXDH.Text = "交易方联系电话格式不正确！";
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemJYFLXDH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家个人交易方联系电话"] = this.txtJYFLXDH.Text;
                    }

                    if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                    {
                        labRemSSQY.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemSSQY.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家个人所属省份"] = ucSSQY.SelectedItem[0];
                        hashTableInfor["买卖家个人所属地市"] = ucSSQY.SelectedItem[1];
                        hashTableInfor["买卖家个人所属区县"] = ucSSQY.SelectedItem[2];
                    }

                    if (this.txtXXDZ.Text == "")
                    {
                        this.labRemXXDZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemXXDZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家个人详细地址"] = this.txtXXDZ.Text;
                    }

                    if (this.txtLXRXM.Text == "")
                    {
                        this.labRemLXRXM.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRXM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家个人联系人姓名"] = this.txtLXRXM.Text;
                    }

                    if (this.txtLXRSJH.Text == "")
                    {
                        this.labRemLXRSJH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                    {
                        this.labRemLXRSJH.Visible = true;
                        this.labRemLXRSJH.Text = "联系人手机号格式不正确！";
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemLXRSJH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家个人联系人手机号"] = this.txtLXRSJH.Text;
                    }
                    if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                    {
                        this.labRemKHYH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHYH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家个人开户银行"] = this.cbxKHYH.Text;
                    }

                    //if (this.cbxKHYH.Text.Trim() == "")
                    //{
                    //     hashTableInfor["买卖家个人开户银行"] = "待补录";
                    //}
                    //else
                    //{
                    // hashTableInfor["买卖家个人开户银行"] = this.cbxKHYH.Text;
                    //}
                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    this.labRemYHZH.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemYHZH.Visible = false;
                    //    JJRDWTag = "完整";
                    //    hashTableInfor["买卖家个人银行账号"] = this.txtYHZH.Text.Trim();
                    //}
                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //  hashTableInfor["买卖家个人银行账号"] ="待补录";
                    //}
                    //else
                    //{
                    // hashTableInfor["买卖家个人银行账号"] = this.txtYHZH.Text.Trim();
                    //}

                    if (this.radFW_Bank.Checked == true)
                    { }
                    else
                    {
                        if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                        {
                            this.lablRemJJRZGZS.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else
                        {
                            this.lablRemJJRZGZS.Visible = false;
                            JJRDWTag = "完整";
                        }
                    }
                    
                    if (this.radFW_JJR.Checked == true)
                    {
                        if (this.txtJJRZGZS.Text.Trim() == "")
                        {
                            this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                            this.lablRemJJRZGZS.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                        {
                            this.lablRemJJRZGZS.Visible = false;
                            JJRDWTag = "完整";
                            hashTableInfor["买卖家个人经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                            hashTableInfor["买卖家个人经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                            hashTableInfor["买卖家个人经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                        }
                        else
                        {
                            this.lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                            this.lablRemJJRZGZS.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;

                        }
                    }
                    else if (this.radFW_Bank.Checked==true)
                    {
                        if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                        {
                            tag += 1;
                            this.labRemYHMC.Visible = true;
                            JJRDWTag = "不完整";
                        }
                        else
                        {
                            hashTableInfor["买卖家个人关联银行"] = this.labYHMC.Text.Trim();
                            this.labRemYHMC.Visible = false;
                            JJRDWTag = "完整";
                        }
                        if (this.uctextBankYGGH.Text.Trim() == "")
                        {
                            tag += 1;
                            this.lblBankYGGH.Visible = true;
                            JJRDWTag = "不完整";
                        }
                        else
                        {
                            hashTableInfor["买卖家个人关联银行工作人员工号"] = this.uctextBankYGGH.Text.Trim();
                            this.lblBankYGGH.Visible = false;
                            JJRDWTag = "完整";
                        }
                        hashTableInfor["买卖家个人经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                        hashTableInfor["买卖家个人经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                        hashTableInfor["买卖家个人经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                        {
                            tag += 1;
                            this.labRemYHMC.Visible = true;
                        }
                        else
                        {
                            this.labRemYHMC.Visible = false;
                        }
                        if (string.IsNullOrEmpty(this.uctextBankYGGH.Text.Trim()))
                        {
                            tag += 1;
                            this.lblBankYGGH.Visible = true;
                        }
                        else
                        {
                            this.lblBankYGGH.Visible = false;
                        }

                        if (String.IsNullOrEmpty(this.labZFMC.Text))
                        {
                            tag += 1;
                            this.labRemZFMC.Visible = true;
                        }
                        else
                        {
                            this.labRemZFMC.Visible = false;
                        }

                        if (String.IsNullOrEmpty(this.labHangYeXieHui.Text))
                        {
                            tag += 1;
                            this.labRemHangYeXieHui.Visible = true;
                        }
                        else
                        {
                            this.labRemHangYeXieHui.Visible = false;
                        }

                        if (this.txtJJRZGZS.Text.Trim() == "")
                        {
                            this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                            this.lablRemJJRZGZS.Visible = true;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else
                        {
                            this.lablRemJJRZGZS.Text = "您填写的经纪人资格证书编号无效！";
                            tag += 1;
                            this.lablRemJJRZGZS.Visible = true;
                        }
                    }

                    //交易资金密码
                    if (this.txtZQZJMM.Text.Trim() == "")
                    {
                        this.labZQZJMM.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }
                    else if (!Support.ValStr.isZQZJMM(this.txtZQZJMM.Text))
                    {
                        this.labZQZJMM.Visible = true;
                        this.labZQZJMM.Text = "交易资金密码必须是6位数字！";
                        JJRDWTag = "不完整";
                        tag += 1;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }
                    else
                    {
                        this.labZQZJMM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["买卖家个人证券资金密码"] = this.txtZQZJMM.Text;
                        label6.Visible = !this.labZQZJMM.Visible;
                    }
                    if (tag == 0)
                    {
                        //禁用提交区域并开启进度
                        flowLayoutPanel1.Enabled = false;
                        PBload.Visible = true;
                        //提交账户信息
                        SRT_SubmitJYZHXX_Run();
                    }
                    else
                    {
                        return;
                    }

                }
            }
            #endregion

            #endregion
        }


        public bool chongzhi = false;

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            chongzhi = true;
            this.Close();
            //this.Hide();
            //Program.fms = new FormKTJYZH(HTuser);
            //Program.fms.ShowDialog();
            //this.Dispose();
        }


        //开启一个线程提交数据
        private void SRT_SubmitJYZHXX_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTableInfor, new delegateForThread(SRT_SubmitJYZHXX));
            Thread trd = new Thread(new ThreadStart(OTD.SubmitJYZHXX));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_SubmitJYZHXX(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_SubmitJYZHXX_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_SubmitJYZHXX_Invoke(Hashtable OutPutHT)
        {
            flowLayoutPanel1.Enabled = true;
            PBload.Visible = false;
            //重新开放提交区域,并滚动条强制置顶
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    //显示平台组织机构
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    PublicDS.PublisDsUser = dsreturn;
                    this.Close();
                    break;
                default:
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    flowLayoutPanel1.Enabled = false;
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
        }


        #endregion

        /// <summary>
        /// 隐藏所有提示信息
        /// </summary>
        private void SetRemindInforInVisiable()
        {
            this.labRemJYZH.Visible = false;
            this.labRemZCLB.Visible = false;
            this.labRemJJFMC.Visible = false;
            this.labRemYYZZ.Visible = false;
            this.labRemSFZ.Visible = false;
            this.labRemZZJGDMZ.Visible = false;
            this.labRemSWDJZ.Visible = false;
            this.labRemNSRZGZ.Visible = false;
            this.labRemKHXKZ.Visible = false;
            this.labRemFDDBR.Visible = false;
            this.labRemFDDBRSHZH.Visible = false;
            this.labRemFDDBRSQS.Visible = false;
            this.labRemJYFLXDH.Visible = false;
            this.labRemSSQY.Visible = false;
            this.labRemXXDZ.Visible = false;
            this.labRemLXRXM.Visible = false;
            this.labRemLXRSJH.Visible = false;
            this.labRemKHYH.Visible = false;
            //this.labRemYHZH.Visible = false;
            this.labRemGLJG.Visible = false;
            this.lablRemJJRZGZS.Visible = false;
            this.lablRemJJRMC.Visible = false;
            this.lablRemJJRLXDH.Visible = false;
            // this.TipFRSFZ.Visible = true;
            // this.TipsSFZZFM.Visible = true;
        }





        #region//20131218周丽作废--原因是此方法未使用过
        /// <summary>
        /// 判断经纪人用户是否可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void txtJJRZGZS_KeyUp(object sender, KeyEventArgs e)
        //{

        //    if (this.radFW_JJR.Checked == true||this.radFW_Bank.Checked==true)//一般经纪人
        //    {
        //        if (this.radFW_Bank.Checked == true)
        //        {
        //            lablRemJJRZGZS.Visible = false;
        //        }
        //        if (this.radFW_JJR.Checked == true)
        //        {
        //            if (txtJJRZGZS.Text.Trim().Length < 13)
        //            {
        //                return;
        //            }


        //            // ArrayList lv = new ArrayList();
        //            txtJJRMC.Text = "";
        //            txtJJRLXDH.Text = "";
        //            if (!txtJJRZGZS.Text.Trim().Equals(""))
        //            {
        //                Hashtable htjjryhm = new Hashtable();
        //                htjjryhm["JJRZGZS"] = txtJJRZGZS.Text.Trim();
        //                if (ValStr.ValidateQuery(htjjryhm))
        //                {
        //                    lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号";
        //                    lablRemJJRZGZS.Visible = true;
        //                    return;
        //                }
        //                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
        //                //InPutHT可以选择灵活使用
        //                object[] objParams = { txtJJRZGZS.Text.Trim() };
        //                DataSet dsreturn = WSC2013.RunAtServices("GetJJRYHXX", objParams);
        //                DataTable dt = dsreturn.Tables["经纪人用户信息"];
        //                if (dt != null && dt.Rows.Count > 0)
        //                {

        //                    DataRow dr = dt.Rows[0];
        //                    if (!dr["分公司开通审核状态"].ToString().Trim().Equals("是"))
        //                    {

        //                        lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
        //                        lablRemJJRZGZS.Visible = true;
        //                        return;
        //                    }
        //                    if (!dr["角色类型"].ToString().Trim().Equals("经纪人交易账户"))
        //                    {
        //                        lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号";
        //                        lablRemJJRZGZS.Visible = true;
        //                        return;
        //                    }

        //                    if (!dr["经纪人暂停接受新用户"].ToString().Trim().Equals("否"))
        //                    {
        //                        lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
        //                        lablRemJJRZGZS.Visible = true;
        //                        return;
        //                    }
        //                    if (!dr["是否允许登陆"].ToString().Trim().Equals("是"))
        //                    {
        //                        lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号";
        //                        lablRemJJRZGZS.Visible = true;
        //                        return;
        //                    }
        //                    if (dr["是否冻结账号"].ToString().Trim().Equals("是"))
        //                    {
        //                        lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号";
        //                        lablRemJJRZGZS.Visible = true;
        //                        return;
        //                    }
        //                    if (dr["是否休眠"].ToString().Trim().Equals("是"))
        //                    {
        //                        lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号";
        //                        lablRemJJRZGZS.Visible = true;
        //                        return;
        //                    }
        //                    /*  //此处用来限制 经纪人的经纪人资格证书标号过期以后，不能在关联此经纪人
        //                    DateTime dateTimeNow = DateTime.Now;
        //                    DateTime dateTimeStart = Convert.ToDateTime(dr["经纪人资格证书有效期开始时间"].ToString().Trim());
        //                    DateTime dateTimeEnd = Convert.ToDateTime(dr["经纪人资格证书有效期结束时间"].ToString().Trim());
        //                    if (dateTimeNow.CompareTo(dateTimeStart) >= 0 && dateTimeNow.CompareTo(dateTimeEnd) <= 0)
        //                    {
        //                        lablRemJJRZGZS.Text = "请输入可用的经纪人用户名";
        //                        lablRemJJRZGZS.Visible = true;
        //                        return;
        //                    }*/
        //                    lablRemJJRZGZS.Visible = false;
        //                    txtJJRMC.Text = dr["经纪人名称"].ToString();
        //                    txtJJRLXDH.Text = dr["经纪人联系电话"].ToString();
        //                    lblJJRFL.Text = dr["经纪人分类"].ToString();
        //                }
        //                else
        //                {
        //                    lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号";
        //                    lablRemJJRZGZS.Visible = true;
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号";
        //                lablRemJJRZGZS.Visible = true;
        //                return;
        //            }
        //        }
                

        //    }
        //    else
        //    {
        //        lablRemJJRZGZS.Text = "您输入的经纪人资格证书无效";
        //        lablRemJJRZGZS.Visible = true;

        //    }

        //}

        #endregion

        /// <summary>
        /// 判断经纪人用户是否可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJJRZGZS_TextChanged(object sender, EventArgs e)
        {
            if (this.radFW_JJR.Checked == true || this.radFW_Bank.Checked==true)//一般经纪人
            {
                if (this.radFW_Bank.Checked == true)
                {
                    lablRemJJRZGZS.Visible = false;
                }
                if (this.radFW_JJR.Checked == true)
                {
                
                
                    if (txtJJRZGZS.Text.Trim().Length < 13)
                    {
                        return;
                    }


                    // ArrayList lv = new ArrayList();
                    txtJJRMC.Text = "";
                    txtJJRLXDH.Text = "";
                    if (!txtJJRZGZS.Text.Trim().Equals(""))
                    {
                        Hashtable htjjryhm = new Hashtable();
                        htjjryhm["JJRZGZS"] = txtJJRZGZS.Text.Trim();
                        if (ValStr.ValidateQuery(htjjryhm))
                        {
                            lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                            lablRemJJRZGZS.Visible = true;
                            return;
                        }
                        NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                        //InPutHT可以选择灵活使用
                        object[] objParams = { txtJJRZGZS.Text.Trim() };
                        //已移植可替换(已替换)
                        //DataSet dsreturn = WSC2013.RunAtServices("GetJJRYHXX", objParams);
                        DataSet dsreturn = WSC2013.RunAtServices("C证书获取经纪人信息", objParams);
                        DataTable dt = dsreturn.Tables["经纪人用户信息"];
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            DataRow dr = dt.Rows[0];
                            if (!dr["分公司开通审核状态"].ToString().Trim().Equals("是"))
                            {

                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            if (!dr["结算账户类型"].ToString().Trim().Equals("经纪人交易账户"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }

                            if (!dr["经纪人是否暂停新用户审核"].ToString().Trim().Equals("否"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            if (!dr["是否允许登陆"].ToString().Trim().Equals("是"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            if (dr["是否冻结"].ToString().Trim().Equals("是"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            if (dr["是否休眠"].ToString().Trim().Equals("是"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            /*  //此处用来限制 经纪人的经纪人资格证书标号过期以后，不能在关联此经纪人
                            DateTime dateTimeNow = DateTime.Now;
                            DateTime dateTimeStart = Convert.ToDateTime(dr["经纪人资格证书有效期开始时间"].ToString().Trim());
                            DateTime dateTimeEnd = Convert.ToDateTime(dr["经纪人资格证书有效期结束时间"].ToString().Trim());
                            if (dateTimeNow.CompareTo(dateTimeStart) >= 0 && dateTimeNow.CompareTo(dateTimeEnd) <= 0)
                            {
                                lablRemJJRZGZS.Text = "请输入可用的经纪人用户名";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }*/
                            lablRemJJRZGZS.Visible = false;
                            txtJJRMC.Text = dr["经纪人名称"].ToString();
                            txtJJRLXDH.Text = dr["经纪人联系电话"].ToString();
                            lblJJRFL.Text = dr["经纪人分类"].ToString();
                        }
                        else
                        {
                            lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                            lablRemJJRZGZS.Visible = true;
                            return;
                        }

                    }
                    else
                    {
                        lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                        lablRemJJRZGZS.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                lablRemJJRZGZS.Text = "您输入的经纪人资格证书无效";
                lablRemJJRZGZS.Visible = true;

            }
        }
        /// <summary>
        /// 判断身份证号是否可重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSFZ_KeyUp(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// 判断身份证号是否已经被注册
        /// </summary>
        /// <param name="strZHLX"></param>
        /// <param name="strSqlSFZH"></param>
        /// <returns></returns>
        private bool JudgeSFZH(string strZHLX, string strSqlSFZH)
        {
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            object[] objParams = { strZHLX, strSqlSFZH };
            //已移植可替换(已替换)
            // DataSet dsreturn = WSC2013.RunAtServices("JudgeSFZHXX", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C身份证号重复检验", objParams);
            DataTable dt = dsreturn.Tables["返回值单条"];
            if (dt != null && dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["执行结果"].ToString())
                {
                    case "ok":
                        return true;
                    case "err":
                        return false;
                    default:
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断法定代表人身份证号是否可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFDDBRSHZH_KeyUp(object sender, KeyEventArgs e)
        {

        }






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
                    if (strSSSF != strTagSF || strSSDS != strTagDS)
                    {
                        DataSet dataSet = PublicDS.PublisDsData;
                        DataRow[] dataRow = dataSet.Tables["分公司对照表"].Select(" Pname ='" + strSSSF + "' and Cname='" + strSSDS + "'");
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

        private void timer_PTGLJG_Tick(object sender, EventArgs e)
        {
            HelpSelectGLJG();
        }

        private void cbxGLJG_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.radJJR.Checked)
            {
                this.timer_PTGLJG.Enabled = false;
            }

        }


        #region
        /// <summary>
        ///法定代表人授权书下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkXZMB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/JHJX_File/法定代表人授权书.zip";
            StringOP.OpenUrl(url);
        }

        /// <summary>
        /// 账户开通协议
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkZHKTXY_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.linkZHKTXY.Text.ToString() == "交易方账户开通协议")
            {
                int pagenumber = 4;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 1];
                string[] MKarr = new string[pagenumber + 1];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTMJMJZHKTXY.aspx?Action=KT_ZQ";
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTZHKTXY";

                }
                CSarr[pagenumber] = "附件";
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTZHKTFJ";
                FormZHKTXY dy = new FormZHKTXY("交易方账户开通协议", CSarr, MKarr, "查看");
                dy.Show();
            }
            else if (this.linkZHKTXY.Text.ToString() == "交易账户经纪人开通协议")
            {
                int pagenumber = 3;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 1];
                string[] MKarr = new string[pagenumber + 1];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTJJRZHKTXY.aspx?Action=KT_ZQ";
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTZHKTXY";

                }
                CSarr[pagenumber] = "附件";
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTZHKTFJ";
                FormZHKTXY dy = new FormZHKTXY("经纪人账户开通协议", CSarr, MKarr, "查看");
                dy.Show();

            }
        }

        /// <summary>
        /// 下载开通须知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkXZKTXZ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/JHJX_File/买家卖家开通须知和开户资料申报表.zip";
            StringOP.OpenUrl(url);
        }
        /// <summary>
        /// 模拟版测试须知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/moban/XNBCSXZ.htm";
            XSRM xs = new XSRM("模拟版测试须知", htcs);
            xs.ShowDialog();
        }

        #endregion


        /// <summary>
        /// 交易方名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJJFMC_Leave(object sender, EventArgs e)
        {
            if (this.radZRR.Checked == true)
            {
                if (String.IsNullOrEmpty(this.txtLXRXM.Text.Trim()))
                {
                    this.txtLXRXM.Text = this.txtJJFMC.Text;

                }
            }
        }
        /// <summary>
        /// 交易方联系电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJYFLXDH_Leave(object sender, EventArgs e)
        {
            if (this.radZRR.Checked == true)
            {
                if (String.IsNullOrEmpty(this.txtLXRSJH.Text.Trim()) && txtJYFLXDH.Text.Trim().IndexOf('-') == -1)//没有中划线
                {
                    this.txtLXRSJH.Text = txtJYFLXDH.Text;
                }
            }
        }

        private void TipsSFZZFM_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 选择业务拓展部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radYWTZB_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLJG.Visible = false;
                this.panTWGLBM.Visible = false;
                this.panSZYX.Visible = false;
            }
        }
        /// <summary>
        /// 选择高校团委
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radGXTW_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLJG.Visible = false;
                this.panTWGLBM.Visible = true;
                this.panSZYX.Visible = true;
                ComboBoxItem cbxItem;
                cbxItem = new ComboBoxItem();
                cbxItem.Text = "请选择院系";
                cbxItem.Value = "请选择院系";
                //cbxSZYX.Items.Insert(0, cbxItem);
                //cbxSZYX.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 选择分公司
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radFGS_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                #region//20140421--周丽修改--因分公司撤销
                this.panGLJG.Visible = false;
                //this.panGLJG.Visible = true;
                #endregion
                this.panTWGLBM.Visible = false;
                this.panSZYX.Visible = false;

            }
        }

        /// <summary>
        /// 选择经纪人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radFW_JJR_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLYH.Visible = false;
                this.panBankYGGH.Visible = false;
                this.panGLZFJG.Visible = false;
                this.panHYXH.Visible = false;

                panZGZS.Visible = true;
                panJJRMC.Visible = true;
                panJJRLXDH.Visible = true;

                //清空银行相关数据
                labYHMC.Text = "";
                lblBankDLYX.Text = "";
                uctextBankYGGH.Text = "";

                //清空共同的数据
                lblJJRFL.Text = "";
                txtJJRZGZS.Text = "";
                txtJJRMC.Text = "";
                txtJJRLXDH.Text = "";
            }
        }
        /// <summary>
        /// 选择银行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radFW_Bank_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLYH.Visible = true;
                this.panBankYGGH.Visible = true;
                this.panGLZFJG.Visible = false;
                this.panHYXH.Visible = false;

                panZGZS.Visible = false;
                panJJRMC.Visible = false;
                panJJRLXDH.Visible = false;

                //清空共同的数据
                txtJJRZGZS.Text = "";
                txtJJRMC.Text = "";
                txtJJRLXDH.Text = "";
            }
        }
        /// <summary>
        /// 关联银行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labCKYHBXZ_Click(object sender, EventArgs e)
        {
            fmSelectBank fm = new fmSelectBank(new delegateForThread(SelectBank_demo));
            fm.ShowDialog();
        }
        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectBank_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SelectBank_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectBank_demo_Invoke(Hashtable OutPutHT)
        {
            this.labYHMC.Text = OutPutHT["银行名称"].ToString();
            this.lblBankDLYX.Text = OutPutHT["银行登录邮箱"].ToString();

            this.txtJJRZGZS.Text = OutPutHT["经纪人资格证书"].ToString();
            this.txtJJRMC.Text = OutPutHT["交易方名称"].ToString();
            this.txtJJRLXDH.Text = OutPutHT["联系电话"].ToString();
            this.lblJJRFL.Text = OutPutHT["经纪人分类"].ToString();
        }
        /// <summary>
        /// 选择政府
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radFW_ZF_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLYH.Visible = false;
                this.panBankYGGH.Visible = false;
                this.panGLZFJG.Visible = true;
                this.panHYXH.Visible = false;

                panZGZS.Visible = true;
                panJJRMC.Visible = true;
                panJJRLXDH.Visible = true;

                //清空银行相关数据
                labYHMC.Text = "";
                lblBankDLYX.Text = "";
                uctextBankYGGH.Text = "";

                //清空共同的数据
                lblJJRFL.Text = "";
                txtJJRZGZS.Text = "";
                txtJJRMC.Text = "";
                txtJJRLXDH.Text = "";
            }
        }

        /// <summary>
        /// 政府名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labCKZFBXZ_Click(object sender, EventArgs e)
        {
            fmSelectGover fm = new fmSelectGover(new delegateForThread(SelectGover_demo));
            fm.ShowDialog();

        }

        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectGover_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SelectGover_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectGover_demo_Invoke(Hashtable OutPutHT)
        {
            this.labYHMC.Text = OutPutHT["政府名称"].ToString();
        }
        /// <summary>
        ///查找并选择团委名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labCKTWBXZ_Click(object sender, EventArgs e)
        {
            fmSelectGXTW fm = new fmSelectGXTW(new delegateForThread(XZGXTWMC_demo));
            fm.ShowDialog();
        }
        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void XZGXTWMC_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(XZGXTWMC_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void XZGXTWMC_demo_Invoke(Hashtable OutPutHT)
        {
            this.labTWMC.Text = OutPutHT["高校团委名称"].ToString();
            this.labGXTW_ZhangHao.Text = OutPutHT["高校账号"].ToString();
            //HelperSelectYX();
            //this.timer_XZYX.Enabled = true;
        }


        /// <summary>
        /// 选择行业协会
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radHangYeXieHui_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLYH.Visible = false;
                this.panBankYGGH.Visible = false;
                this.panGLZFJG.Visible = false;
                this.panHYXH.Visible = true;

                panZGZS.Visible = true;
                panJJRMC.Visible = true;
                panJJRLXDH.Visible = true;

                //清空银行相关数据
                labYHMC.Text = "";
                lblBankDLYX.Text = "";
                uctextBankYGGH.Text = "";

                //清空共同的数据
                lblJJRFL.Text = "";
                txtJJRZGZS.Text = "";
                txtJJRMC.Text = "";
                txtJJRLXDH.Text = "";
            }
        }
        /// <summary>
        ///查看并选择行业协会
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labCKBXZHYXH_Click(object sender, EventArgs e)
        {
            fmSelectHYXH fm = new fmSelectHYXH(new delegateForThread(XZGXTWMC_demo));
            fm.ShowDialog();
        }

        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void HYXH_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(HYXH_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void HYXH_demo_Invoke(Hashtable OutPutHT)
        {
            this.labTWMC.Text = OutPutHT["行业协会名称"].ToString();
            //HelperSelectYX();
            //this.timer_XZYX.Enabled = true;
        }




        private void timer_SCCK_Tick(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 选择院系的Timeer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_XZYX_Tick(object sender, EventArgs e)
        {
            HelperSelectYX();
        }

        /// <summary>
        /// 选择院系的代码
        /// </summary>
        private void HelperSelectYX()
        {
            if (this.radGXTW.Checked == true)//选择高校团委
            {
                if (!string.IsNullOrEmpty(this.labGXTW_ZhangHao.Text.Trim()))
                {
                    //cbxSZYX.Items.Clear();
                    ComboBoxItem cbxItem;
                    DataSet dataSet = PublicDS.PublisDsData;
                    cbxItem = new ComboBoxItem();
                    cbxItem.Text = "请选择院系";
                    cbxItem.Value = "请选择院系";
                    //cbxSZYX.Items.Insert(0, cbxItem);

                    string[] distinctcols = new string[] { "院系Number", "高校账户", "院系名称" };
                    DataTable dtfd = new DataTable("distinctTable");
                    DataView mydataview = new DataView(dataSet.Tables["院系表"]);
                    dtfd = mydataview.ToTable(true, distinctcols);
                    DataRow[] dataRow = dtfd.Select("高校账户='" + this.labGXTW_ZhangHao.Text.Trim() + "'");
                    foreach (DataRow dr in dataRow)
                    {
                        cbxItem = new ComboBoxItem();
                        cbxItem.Text = dr["院系名称"].ToString();
                        cbxItem.Value = dr["院系Number"].ToString();
                        //cbxSZYX.Items.Add(cbxItem);

                    }
                    //cbxSZYX.SelectedIndex = 0;

                }

            }
        }

        private void panZQZJMM_Paint(object sender, PaintEventArgs e)
        {

        }
        #region//选择工号

        private void lblSelectBankYGGH_Click(object sender, EventArgs e)
        {
            fmSelectBankYG bankyg = new fmSelectBankYG(new delegateForThread(SelectBankYGGH_demo), this.lblBankDLYX.Text.Trim());
            bankyg.Show();
        }
        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectBankYGGH_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SelectBankYGGH_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectBankYGGH_demo_Invoke(Hashtable OutPutHT)
        {
            this.uctextBankYGGH.Text = OutPutHT["员工工号"].ToString();
        }


        #endregion

        private void uctextBankYGGH_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void lblBankYGGH_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/JHJX_File/交易方预留印鉴表.zip";
            StringOP.OpenUrl(url);
        }

        private void txtZQZJMM_Enter(object sender, EventArgs e)
        {
            txtZQZJMM.OpenZS = true;
        }

        private void txtZQZJMM_Leave(object sender, EventArgs e)
        {
            txtZQZJMM.OpenZS = false;
            if (txtZQZJMM.Text.Trim() == "")
            {
                txtZQZJMM.TextNtip = "请输入6位数字";
            }
        }



    }

    public class ComboBoxItem
    {
        private string _text = null;
        private string _value = null;
        public string Text
        {
            get { return this._text; }
            set { this._text = value; }
        }
        public string Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
        public override string ToString()
        {
            return this._text;
        }
    }






}
