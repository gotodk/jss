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
using ClassLibraryBusinessMonitor;

namespace 客户端主程序.SubForm.CenterForm.Agent
{
    public partial class FormYHZLXQ : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;
        //角色编号
        string str_RoleNumber;
        //角色类型
        string str_RoleType;
        //角色登陆邮箱
        string str_RoleEmail;

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


        public FormYHZLXQ(string strRoleType,string strRoleNumber,string strRoleEmail)
        {
            //存储角色编号
            str_RoleNumber = strRoleNumber;
            //存储角色类型
            str_RoleType = strRoleType;
            //存储角色登陆邮箱
            str_RoleEmail = strRoleEmail;
          
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //====================================================

            InitializeComponent();
         
            //设置最小化大小
            this.MaximizedBounds = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.BackColor = Color.Black;
            this.Width = (int)(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width * 0.8);
            this.Height = (int)(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height * 0.8);
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void IntializeGUI(string strRoleType,string strRoleNumber)
        {
            switch (strRoleType)
            { 
                case "买家账户":
                  //签字承诺书
                    this.lblQZCNS.Visible = false;
                    this.txtQZCNS.Visible = false;
                    this.ckQZCNS.Visible = false;
                    ////公司名称
                    //this.lblGSMC.Visible = false;
                    //this.txtGSMC.Visible = false;
                    ////公司电话
                    //this.lblGSDH.Visible = false;
                    //this.txtGSDH.Visible = false;
                    ////公司地址
                    //this.lblGSDZ.Visible = false;
                    //this.txtGSDZ.Visible = false;
                    ////营业执照
                    //this.lblYYZZ.Visible = false;
                    //this.txtYYZZ.Visible = false;
                    //this.ckYYZZ.Visible = false;
                    ////组织结构代码证
                    //this.lblZZJGDMZ.Visible = false;
                    //this.txtZZJGDMZ.Visible = false;
                    //this.ckZZJGDMZ.Visible = false;
                    ////税务登记证
                    //this.lblSWDJZ.Visible = false;
                    //this.txtSWDJZ.Visible = false;
                    //this.ckSWDJZ.Visible = false;
                    ////开户许可证
                    //this.lblKHXKZ.Visible = false;
                    //this.txtKHXKZ.Visible = false;
                    //this.ckKHXKZ.Visible = false;

                    //this.lblSHYJ.Location = new Point(this.lblSHYJ.Location.X, this.lblSHYJ.Location.Y - 240);
                    //this.txtSHYJ.Location = new Point(this.txtSHYJ.Location.X, this.txtSHYJ.Location.Y - 240);
                    //this.btnPass.Location = new Point(this.btnPass.Location.X, this.btnPass.Location.Y - 240);
                    //this.btnReject.Location = new Point(this.btnReject.Location.X, this.btnReject.Location.Y - 240);
                    
                    //公司名称
                    this.lblGSMC.Location = new Point(this.lblGSMC.Location.X, this.lblGSMC.Location.Y - 30);
                    this.txtGSMC.Location = new Point(this.txtGSMC.Location.X, this.txtGSMC.Location.Y - 30);
                    //公司电话
                    this.lblGSDH.Location = new Point(this.lblGSDH.Location.X, this.lblGSDH.Location.Y - 30);
                    this.txtGSDH.Location = new Point(this.txtGSDH.Location.X, this.txtGSDH.Location.Y - 30);
                    //公司地址
                    this.lblGSDZ.Location = new Point(this.lblGSDZ.Location.X, this.lblGSDZ.Location.Y - 30);
                    this.txtGSDZ.Location = new Point(this.txtGSDZ.Location.X, this.txtGSDZ.Location.Y - 30);
                    //营业执照
                    this.lblYYZZ.Location = new Point(this.lblYYZZ.Location.X, this.lblYYZZ.Location.Y - 30);
                    this.txtYYZZ.Location = new Point(this.txtYYZZ.Location.X, this.txtYYZZ.Location.Y - 30);
                    this.ckYYZZ.Location = new Point(this.ckYYZZ.Location.X, this.ckYYZZ.Location.Y - 30);
                    //组织结构代码证
                    this.lblZZJGDMZ.Location = new Point(this.lblZZJGDMZ.Location.X, this.lblZZJGDMZ.Location.Y - 30);
                    this.txtZZJGDMZ.Location = new Point(this.txtZZJGDMZ.Location.X, this.txtZZJGDMZ.Location.Y - 30);
                    this.ckZZJGDMZ.Location = new Point(this.ckZZJGDMZ.Location.X, this.ckZZJGDMZ.Location.Y - 30);
                    //税务登记证
                    this.lblSWDJZ.Location = new Point(this.lblSWDJZ.Location.X, this.lblSWDJZ.Location.Y - 30);
                    this.txtSWDJZ.Location = new Point(this.txtSWDJZ.Location.X, this.txtSWDJZ.Location.Y - 30);
                    this.ckSWDJZ.Location = new Point(this.ckSWDJZ.Location.X, this.ckSWDJZ.Location.Y - 30);
                    //开户许可证
                    this.lblKHXKZ.Location = new Point(this.lblKHXKZ.Location.X, this.lblKHXKZ.Location.Y - 30);
                    this.txtKHXKZ.Location = new Point(this.txtKHXKZ.Location.X, this.txtKHXKZ.Location.Y - 30);
                    this.ckKHXKZ.Location = new Point(this.ckKHXKZ.Location.X, this.ckKHXKZ.Location.Y - 30);
                    //其他资质
                    this.lblQTZZ.Location = new Point(this.lblQTZZ.Location.X,this.lblQTZZ.Location.Y-30);
                    this.txtQTZZ.Location = new Point(this.txtQTZZ.Location.X,this.txtQTZZ.Location.Y-30);
                    this.lblQTZZCK.Location = new Point(this.lblQTZZCK.Location.X,this.lblQTZZCK.Location.Y-30);

                    this.lblSHYJ.Location = new Point(this.lblSHYJ.Location.X, this.lblSHYJ.Location.Y - 30);
                    this.txtSHYJ.Location = new Point(this.txtSHYJ.Location.X, this.txtSHYJ.Location.Y - 30);
                    this.btnPass.Location = new Point(this.btnPass.Location.X, this.btnPass.Location.Y - 30);
                    this.btnReject.Location = new Point(this.btnReject.Location.X, this.btnReject.Location.Y - 30);
                    //this.PBload.Location = new Point(this.PBload.Location.X, this.btnReject.Location.Y - 60 + 32);
                    //this.Size = new Size(700, this.btnReject.Location.Y + 100);
                    this.PBload.Location = new Point(this.PBload.Location.X, this.PBload.Location.Y );
                    this.Size = new Size(700, this.btnReject.Location.Y + 100);

                    break;
                case "卖家账户":
                    //身份证号 
                    this.lblSFZH.Visible = false;
                    this.txtSFZH.Visible = false;
                    //身份证扫描件
                    this.lblSFZSMJ.Visible = false;
                    this.txtSFZSMJ.Visible = false;
                    this.ckSFZSMJ.Visible = false;
                    //签字承诺书
                    this.lblQZCNS.Visible = false;
                    this.txtQZCNS.Visible = false;
                    this.ckQZCNS.Visible = false;
                    //身份证扫描件的控件  整体往上移30个像素
                    //手机号
                    this.lblSJH.Location = new Point(this.lblSJH.Location.X,this.lblSJH.Location.Y-30);
                    this.txtSJH.Location = new Point(this.txtSJH.Location.X,this.txtSJH.Location.Y-30);
                    //所属区域
                    this.lblSSQY.Location = new Point(this.lblSSQY.Location.X, this.lblSSQY.Location.Y - 30);
                    this.paneSSQY.Location = new Point(this.paneSSQY.Location.X, this.paneSSQY.Location.Y - 30);
                    //详细地址
                    this.lblXXDZ.Location = new Point(this.lblXXDZ.Location.X,this.lblXXDZ.Location.Y-30);
                    this.txtXXDZ.Location = new Point(this.txtXXDZ.Location.X,this.txtXXDZ.Location.Y-30);
                    //邮政编码
                    this.lblYB.Location = new Point(this.lblYB.Location.X,this.lblYB.Location.Y-30);
                    this.txtYB.Location = new Point(this.txtYB.Location.X, this.txtYB.Location.Y - 30);
                    ////签字承诺书
                    //this.lblQZCNS.Location = new Point(this.lblQZCNS.Location.X, this.lblQZCNS.Location.Y - 60);
                    //this.txtQZCNS.Location = new Point(this.txtQZCNS.Location.X, this.txtQZCNS.Location.Y - 60);
                    //this.ckQZCNS.Location = new Point(this.ckQZCNS.Location.X, this.ckQZCNS.Location.Y - 60);
                    //公司名称
                    this.lblGSMC.Location = new Point(this.lblGSMC.Location.X, this.lblGSMC.Location.Y - 90);
                    this.txtGSMC.Location = new Point(this.txtGSMC.Location.X, this.txtGSMC.Location.Y - 90);
                    //公司电话
                    this.lblGSDH.Location = new Point(this.lblGSDH.Location.X, this.lblGSDH.Location.Y - 90);
                    this.txtGSDH.Location = new Point(this.txtGSDH.Location.X, this.txtGSDH.Location.Y - 90);
                    //公司地址
                    this.lblGSDZ.Location = new Point(this.lblGSDZ.Location.X, this.lblGSDZ.Location.Y - 90);
                    this.txtGSDZ.Location = new Point(this.txtGSDZ.Location.X, this.txtGSDZ.Location.Y - 90);
                    //营业执照
                    this.lblYYZZ.Location = new Point(this.lblYYZZ.Location.X, this.lblYYZZ.Location.Y - 90);
                    this.txtYYZZ.Location = new Point(this.txtYYZZ.Location.X, this.txtYYZZ.Location.Y - 90);
                    this.ckYYZZ.Location = new Point(this.ckYYZZ.Location.X, this.ckYYZZ.Location.Y - 90);
                    //组织结构代码证
                    this.lblZZJGDMZ.Location = new Point(this.lblZZJGDMZ.Location.X, this.lblZZJGDMZ.Location.Y - 90);
                    this.txtZZJGDMZ.Location = new Point(this.txtZZJGDMZ.Location.X, this.txtZZJGDMZ.Location.Y - 90);
                    this.ckZZJGDMZ.Location = new Point(this.ckZZJGDMZ.Location.X, this.ckZZJGDMZ.Location.Y - 90);
                    //税务登记证
                    this.lblSWDJZ.Location = new Point(this.lblSWDJZ.Location.X, this.lblSWDJZ.Location.Y - 90);
                    this.txtSWDJZ.Location = new Point(this.txtSWDJZ.Location.X, this.txtSWDJZ.Location.Y - 90);
                    this.ckSWDJZ.Location = new Point(this.ckSWDJZ.Location.X, this.ckSWDJZ.Location.Y - 90);
                    //开户许可证
                    this.lblKHXKZ.Location = new Point(this.lblKHXKZ.Location.X, this.lblKHXKZ.Location.Y - 90);
                    this.txtKHXKZ.Location = new Point(this.txtKHXKZ.Location.X, this.txtKHXKZ.Location.Y - 90);
                    this.ckKHXKZ.Location = new Point(this.ckKHXKZ.Location.X, this.ckKHXKZ.Location.Y - 90);
                    //其他资质
                    this.lblQTZZ.Location = new Point(this.lblQTZZ.Location.X, this.lblQTZZ.Location.Y - 90);
                    this.txtQTZZ.Location = new Point(this.txtQTZZ.Location.X, this.txtQTZZ.Location.Y - 90);
                    this.lblQTZZCK.Location = new Point(this.lblQTZZCK.Location.X, this.lblQTZZCK.Location.Y - 90);

                    this.lblSHYJ.Location = new Point(this.lblSHYJ.Location.X, this.lblSHYJ.Location.Y - 90);
                    this.txtSHYJ.Location = new Point(this.txtSHYJ.Location.X, this.txtSHYJ.Location.Y - 90);
                    this.btnPass.Location = new Point(this.btnPass.Location.X, this.btnPass.Location.Y - 90);
                    this.btnReject.Location = new Point(this.btnReject.Location.X, this.btnReject.Location.Y - 90);
                    this.PBload.Location = new Point(this.PBload.Location.X, this.btnReject.Location.Y - 90+32);
                    this.Size = new Size(700, this.btnReject.Location.Y + 100);
                    break;
            }
        }


        #region 参数加载界面数据

        /// <summary>
        /// 开始启动获取 买家或卖家基础数据的权限
        /// </summary>
        private void BeginGetIntializeData(string strRoleType,string strRoleNumber)
        {
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_IntializeData);
            Hashtable hashTable = new Hashtable();
            hashTable["角色类型"] = strRoleType;
            hashTable["角色编号"] = str_RoleNumber;
            hashTable["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString();
            DataControl.RunThreadClassIntializeData RTCI = new DataControl.RunThreadClassIntializeData(hashTable, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCI.BeginRun_IntializeBuyerSellerData));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_IntializeData(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_IntializeData_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_IntializeData_Invoke(Hashtable OutPutHT)
        {
           DataSet dataSet = (DataSet)OutPutHT["前置数据"];
           
           if (dataSet != null)
            {
                if (str_RoleType.Contains("买家")) //买家角色
                {
                    switch (dataSet.Tables["买家数据表"].Rows[0]["执行状态"].ToString())
                    {
                        case "ok": //查询数据成功

                            //确定是否仅查看
                            if (dataSet.Tables["买家数据表"].Rows[0]["审核状态"].ToString() == "审核通过" || dataSet.Tables["买家数据表"].Rows[0]["审核状态"].ToString() == "已审核")
                            {
                                this.btnPass.Visible = false;
                                this.btnReject.Visible = false;
                            }

                            this.txtZCJS.Text = dataSet.Tables["买家数据表"].Rows[0]["JSLX"].ToString();
                            this.txtYHM.Text = dataSet.Tables["买家数据表"].Rows[0]["YHM"].ToString();
                            this.txtZCYX.Text = dataSet.Tables["买家数据表"].Rows[0]["DLYX"].ToString();
                            this.txtLXRXM.Text = dataSet.Tables["买家数据表"].Rows[0]["LXRXM"].ToString();
                            this.txtSFZH.Text = dataSet.Tables["买家数据表"].Rows[0]["SFZH"].ToString();
                            this.txtSJH.Text = dataSet.Tables["买家数据表"].Rows[0]["SJH"].ToString();
                            //必须先初始化省市区下拉框
                            paneSSQY.initdefault();
                            //设置默认显示的数据，一般只在编辑的时候才用，默认不需要这句话
                            paneSSQY.SelectedItem = new string[] { dataSet.Tables["买家数据表"].Rows[0]["SSSF"].ToString(), dataSet.Tables["买家数据表"].Rows[0]["SSDS"].ToString(), dataSet.Tables["买家数据表"].Rows[0]["SSQX"].ToString() };
                            //设置是否允许选择，特殊情况才用
                            paneSSQY.EnabledItem = new bool[] { false, false, false };
                            this.txtXXDZ.Text = dataSet.Tables["买家数据表"].Rows[0]["XXDZ"].ToString();
                            this.txtYB.Text = dataSet.Tables["买家数据表"].Rows[0]["YZBM"].ToString();
                            this.txtSFZSMJ.Text = dataSet.Tables["买家数据表"].Rows[0]["SFZSMJ"].ToString();
                            //this.txtSHYJ.Text = dataSet.Tables["买家数据表"].Rows[0]["KTSHXX"].ToString();
                            this.txtGSMC.Text = dataSet.Tables["买家数据表"].Rows[0]["GSMC"].ToString();
                            this.txtGSDH.Text = dataSet.Tables["买家数据表"].Rows[0]["GSDH"].ToString();
                            this.txtGSDZ.Text = dataSet.Tables["买家数据表"].Rows[0]["GSDZ"].ToString();
                            this.txtYYZZ.Text = dataSet.Tables["买家数据表"].Rows[0]["YYZZSMJ"].ToString();
                            this.txtZZJGDMZ.Text = dataSet.Tables["买家数据表"].Rows[0]["ZZJGDMZSMJ"].ToString();
                            this.txtSWDJZ.Text = dataSet.Tables["买家数据表"].Rows[0]["SWDJZSMJ"].ToString();
                            this.txtKHXKZ.Text = dataSet.Tables["买家数据表"].Rows[0]["KHXKZSMJ"].ToString();
                            this.txtQTZZ.Text = dataSet.Tables["买家数据表"].Rows[0]["QTZZWJSMJ"].ToString();
                            this.txtSHYJ.Text = dataSet.Tables["买家数据表"].Rows[0]["DLRSHYJ"].ToString();
                            break;
                        case "系统繁忙":
                            ArrayList Almsg7 = new ArrayList();
                            Almsg7.Add("");
                            Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                            FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                            FRSE7.ShowDialog();
                            break;
                        default:
                            ArrayList Almsg4 = new ArrayList();
                            Almsg4.Add("");
                            Almsg4.Add(dataSet.Tables["买家数据表"].Rows[0]["执行结果"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                            FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                            FRSE4.ShowDialog();

                            break;
                    }
                }
                else if (str_RoleType.Contains("卖家"))  //卖家角色
                {
                    switch (dataSet.Tables["卖家数据表"].Rows[0]["执行状态"].ToString())
                    {
                        case "ok": //查询数据成功


                            if (dataSet.Tables["卖家数据表"].Rows[0]["审核状态"].ToString() == "审核通过" || dataSet.Tables["卖家数据表"].Rows[0]["审核状态"].ToString() == "已审核")
                            {
                                this.btnPass.Visible = false;
                                this.btnReject.Visible = false;
                            }

                            this.txtZCJS.Text = dataSet.Tables["卖家数据表"].Rows[0]["JSLX"].ToString();
                            this.txtYHM.Text = dataSet.Tables["卖家数据表"].Rows[0]["YHM"].ToString();
                            this.txtZCYX.Text = dataSet.Tables["卖家数据表"].Rows[0]["DLYX"].ToString();
                            this.txtLXRXM.Text = dataSet.Tables["卖家数据表"].Rows[0]["LXRXM"].ToString();
                            this.txtSFZH.Text = dataSet.Tables["卖家数据表"].Rows[0]["SFZH"].ToString();
                            this.txtSJH.Text = dataSet.Tables["卖家数据表"].Rows[0]["SJH"].ToString();
                            //必须先初始化省市区下拉框
                            paneSSQY.initdefault();
                            //设置默认显示的数据，一般只在编辑的时候才用，默认不需要这句话
                            paneSSQY.SelectedItem = new string[] { dataSet.Tables["卖家数据表"].Rows[0]["SSSF"].ToString(), dataSet.Tables["卖家数据表"].Rows[0]["SSDS"].ToString(), dataSet.Tables["卖家数据表"].Rows[0]["SSQX"].ToString() };
                            //设置是否允许选择，特殊情况才用
                            paneSSQY.EnabledItem = new bool[] { false, false, false };
                            this.txtXXDZ.Text = dataSet.Tables["卖家数据表"].Rows[0]["XXDZ"].ToString();
                            this.txtYB.Text = dataSet.Tables["卖家数据表"].Rows[0]["YZBM"].ToString();
                            //注意  卖家基础数据在前台没有 身份证扫描件
                            this.txtQZCNS.Text = dataSet.Tables["卖家数据表"].Rows[0]["FDDBRQZDCNSSMJ"].ToString();
                            this.txtGSMC.Text = dataSet.Tables["卖家数据表"].Rows[0]["GSMC"].ToString();
                            this.txtGSDH.Text = dataSet.Tables["卖家数据表"].Rows[0]["GSDH"].ToString();
                            this.txtGSDZ.Text = dataSet.Tables["卖家数据表"].Rows[0]["GSDZ"].ToString();
                            this.txtYYZZ.Text = dataSet.Tables["卖家数据表"].Rows[0]["YYZZSMJ"].ToString();
                            this.txtZZJGDMZ.Text = dataSet.Tables["卖家数据表"].Rows[0]["ZZJGDMZSMJ"].ToString();
                            this.txtSWDJZ.Text = dataSet.Tables["卖家数据表"].Rows[0]["SWDJZSMJ"].ToString();
                            this.txtKHXKZ.Text = dataSet.Tables["卖家数据表"].Rows[0]["KHXKZSMJ"].ToString();
                            this.txtQTZZ.Text = dataSet.Tables["卖家数据表"].Rows[0]["QTZZWJSMJ"].ToString();
                            this.txtSHYJ.Text = dataSet.Tables["卖家数据表"].Rows[0]["DLRSHYJ"].ToString();

                            break;
                        case "系统繁忙":
                            ArrayList Almsg7 = new ArrayList();
                            Almsg7.Add("");
                            Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                            FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                            FRSE7.ShowDialog();
                            break;
                        default:
                            ArrayList Almsg4 = new ArrayList();
                            Almsg4.Add("");
                            Almsg4.Add(dataSet.Tables["买家数据表"].Rows[0]["执行结果"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                            FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                            FRSE4.ShowDialog();

                            break;
                    }
                
                
                }
                //this.btnPass.Enabled = true;
                //this.btnReject.Enabled = true;
            }
            else
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("无法初始化基础数据，请检查网络连接。");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "软件初始化提示", Almsg3);
                FRSE3.ShowDialog();
            }
        }

        #endregion




        /// <summary>
        /// 窗体正在关闭是触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormYHZLXQ_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPass_Click(object sender, EventArgs e)
        {

            if (str_RoleType.Contains("买家"))
            {
                //获得表单数据和进行基本验证
                Hashtable ht_skinfoBuyer = new Hashtable();
                ht_skinfoBuyer["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString();
                ht_skinfoBuyer["买家登录账号"] = str_RoleEmail;
                ht_skinfoBuyer["经纪人审核意见"] = txtSHYJ.Text.ToString();
                //开启线程提交数据
                delegateForThread dftBuyerPass = new delegateForThread(ShowThreadResult_VerifyBuyerPass);
                DataControl.RunThreadClassIntializeData RTCTSOU = new DataControl.RunThreadClassIntializeData(ht_skinfoBuyer, dftBuyerPass);
                Thread trd = new Thread(new ThreadStart(RTCTSOU.VerifyBuyerDataPass));
                trd.IsBackground = true;
                trd.Start();
                //发送电子邮件
                Hashtable buyerHashTable = new Hashtable();
                buyerHashTable["登录邮箱"] = txtZCYX.Text.ToString().Trim();
                buyerHashTable["用户名"] = txtYHM.Text.ToString().Trim(); 
                buyerHashTable["结算账户"] = "买家结算账户";
                buyerHashTable["手机号"] = this.txtSJH.Text.ToString();
                buyerHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                //SendPassEmailPhone(buyerHashTable); //未彻底审核通过前，不发送邮件和短信

            }
            else if (str_RoleType.Contains("卖家"))
            {
                //获得表单数据和进行基本验证
                Hashtable ht_skinfoSeller = new Hashtable();
                ht_skinfoSeller["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString();
                ht_skinfoSeller["卖家登录账号"] = str_RoleEmail;
                ht_skinfoSeller["经纪人审核意见"] = txtSHYJ.Text.ToString();
                //开启线程提交数据
                delegateForThread dftSellerPass = new delegateForThread(ShowThreadResult_VerifySellerPass);
                DataControl.RunThreadClassIntializeData RTCTSOU = new DataControl.RunThreadClassIntializeData(ht_skinfoSeller, dftSellerPass);
                Thread trd = new Thread(new ThreadStart(RTCTSOU.VerifySellerDataPass));
                trd.IsBackground = true;
                trd.Start();

                //发送电子邮件
                Hashtable sellerHashTable = new Hashtable();
                sellerHashTable["登录邮箱"] = txtZCYX.Text.ToString().Trim();
                sellerHashTable["用户名"] = txtYHM.Text.ToString().Trim(); 
                sellerHashTable["结算账户"] = "卖家结算账户和买家结算账户";
                sellerHashTable["手机号"] = this.txtSJH.Text.ToString();
                sellerHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                //SendPassEmailPhone(sellerHashTable);//未彻底审核通过前，不发送邮件和短信
            }
            SubmitingState();
        }


        #region//处理审核、驳回按钮状态
        /// <summary>
        /// 提交之前的状态
        /// </summary>
        private void SubmitingState()
        {
            this.PBload.Visible = true;
            this.btnPass.Visible = false;
            this.btnReject.Visible = false;
        }
        /// <summary>
        /// 提交之后的状态
        /// </summary>
        private void SubmitedState()
        {
            this.PBload.Visible = false;
            this.btnPass.Visible = false;
            this.btnReject.Visible = false;
        
        
        }
        #endregion


        #region//买家审核通过的数据处理 【审核通过】
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_VerifyBuyerPass(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_VerifyBuyerPass_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResult_VerifyBuyerPass_Invoke(Hashtable returnHT)
        {
            switch (returnHT["执行状态"].ToString())
            {
                case "ok": //查询数据成功
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("审核信息保存成功！");
                    //Almsg3.Add("同时向该用户邮箱和用户手机分别发送一封提醒邮件和一条提醒短信！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();
                    break;
                case "系统繁忙":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(returnHT["执行结构"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();
                    break;
            }
            SubmitedState();
        }
        #endregion

        #region //买家审核失败的数据处理 【驳回】
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_VerifyBuyerReject(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_VerifyBuyerReject_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResult_VerifyBuyerReject_Invoke(Hashtable returnHT)
        {
            switch (returnHT["执行状态"].ToString())
            {
                case "ok": //查询数据成功
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("审核信息保存成功！");
                    Almsg3.Add("同时向该用户邮箱和用户手机分别发送一封提醒邮件和一条提醒短信！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();
                    break;
                case "系统繁忙":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(returnHT["执行结构"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();
                    break;
            }
            SubmitedState();
        }
        #endregion

        #region //审核卖家通过的数据处理  【审核通过】
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_VerifySellerPass(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_VerifySellerPass_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResult_VerifySellerPass_Invoke(Hashtable returnHT)
        {
            switch (returnHT["执行状态"].ToString())
            {
                case "ok": //查询数据成功
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("审核信息保存成功！");
                    //Almsg3.Add("同时向该用户邮箱和用户手机分别发送一封提醒邮件和一条提醒短信！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();
                    break;
                case "系统繁忙":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(returnHT["执行结构"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();
                    break;
            }
            SubmitedState();
        }
        #endregion

        #region //审核卖家的数据处理  【驳回】
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_VerifySellerReject(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_VerifySellerReject_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResult_VerifySellerReject_Invoke(Hashtable returnHT)
        {
            switch (returnHT["执行状态"].ToString())
            {
                case "ok": //查询数据成功
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("审核信息保存成功！");
                    Almsg3.Add("同时向该用户邮箱和用户手机分别发送一封提醒邮件和一条提醒短信！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();
                    break;
                case "系统繁忙":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(returnHT["执行结构"].ToString());// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();
                    break;
            }
            SubmitedState();
        }
        #endregion

        #region//发送邮件
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_SendPassEmail(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_SendPassEmail_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_SendPassEmail_Invoke(Hashtable returnHT)
        {
          
        }
        /// <summary>
        /// 审核通过后，给用户发送电子邮件
        /// </summary>
        /// <param name="hashTable"></param>
        private void SendPassEmailPhone(Hashtable hashTable)
        {
            //开启线程提交数据
            delegateForThread dftBuyerPass = new delegateForThread(ShowThreadResult_SendPassEmail);
            DataControl.RunThreadClassSendEmailPhone rtcSE = new DataControl.RunThreadClassSendEmailPhone(hashTable, dftBuyerPass);
            Thread trd = new Thread(new ThreadStart(rtcSE.BeginRun_SendPassEmailPhone));
            trd.IsBackground = true;
            trd.Start();
        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_SendRejectEmail(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_SendRejectEmail_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_SendRejectEmail_Invoke(Hashtable returnHT)
        {

        }
        /// <summary>
        /// 审核失败后发送的邮件和短信
        /// </summary>
        /// <param name="hashTable"></param>
        private void SendRejectEmailPhone(Hashtable hashTable)
        {
            //开启线程提交数据
            delegateForThread dftBuyerReject = new delegateForThread(ShowThreadResult_SendRejectEmail);
            DataControl.RunThreadClassSendEmailPhone rtcSE = new DataControl.RunThreadClassSendEmailPhone(hashTable, dftBuyerReject);
            Thread trd = new Thread(new ThreadStart(rtcSE.BeginRun_SendRejectEmailPhone));
            trd.IsBackground = true;
            trd.Start();
        
        }
        #endregion
        /// <summary>
        /// 审核驳回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReject_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSHYJ.Text.Trim()))
            {
                ArrayList Almsg7 = new ArrayList();
                Almsg7.Add("");
                Almsg7.Add("请填写审核意见，审核意见不能为空！");
                FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                FRSE7.ShowDialog();
                return;
            }


            if (str_RoleType == "买家")
            {
                //获得表单数据和进行基本验证
                Hashtable ht_skinfoBuyer = new Hashtable();
                ht_skinfoBuyer["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString();
                ht_skinfoBuyer["买家登录账号"] = str_RoleEmail;
                ht_skinfoBuyer["经纪人审核意见"] = txtSHYJ.Text.ToString();
                //开启线程提交数据
                delegateForThread dftBuyerReject = new delegateForThread(ShowThreadResult_VerifyBuyerReject);
                DataControl.RunThreadClassIntializeData RTCTSOU = new DataControl.RunThreadClassIntializeData(ht_skinfoBuyer, dftBuyerReject);
                Thread trd = new Thread(new ThreadStart(RTCTSOU.VerifyBuyerDataReject));
                trd.IsBackground = true;
                trd.Start();

                //发送电子邮件
                Hashtable rejectHashTable = new Hashtable();
                rejectHashTable["登录邮箱"] = this.txtZCYX.Text.ToString().Trim();
                rejectHashTable["用户名"] = this.txtYHM.Text.ToString().Trim();
                rejectHashTable["手机号"] = this.txtSJH.Text.ToString();
                rejectHashTable["驳回缘由"] = txtSHYJ.Text.ToString().Trim();
                rejectHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                SendRejectEmailPhone(rejectHashTable);
            }
            else if (str_RoleType == "卖家")
            {
                //获得表单数据和进行基本验证
                Hashtable ht_skinfoSeller = new Hashtable();
                ht_skinfoSeller["经纪人角色编号"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["经纪人角色编号"].ToString();
                ht_skinfoSeller["卖家登录账号"] = str_RoleEmail;
                ht_skinfoSeller["经纪人审核意见"] = txtSHYJ.Text.ToString();
                //开启线程提交数据
                delegateForThread dftSellerReject = new delegateForThread(ShowThreadResult_VerifySellerReject);
                DataControl.RunThreadClassIntializeData RTCTSOU = new DataControl.RunThreadClassIntializeData(ht_skinfoSeller, dftSellerReject);
                Thread trd = new Thread(new ThreadStart(RTCTSOU.VerifySellerDataReject));
                trd.IsBackground = true;
                trd.Start();

                //发送电子邮件
                Hashtable buyerHashTable = new Hashtable();
                buyerHashTable["登录邮箱"] =this.txtZCYX.Text.ToString().Trim();
                buyerHashTable["用户名"] = this.txtYHM.Text.ToString().Trim();
                buyerHashTable["手机号"] = this.txtSJH.Text.ToString();
                buyerHashTable["驳回缘由"] = txtSHYJ.Text.ToString().Trim();
                buyerHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                SendRejectEmailPhone(buyerHashTable);
            }
            SubmitingState();
           
        }
        /// <summary>
        /// 窗体实例换完毕的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormYHZLXQ_Load(object sender, EventArgs e)
        {
            //初始化界面
            IntializeGUI(str_RoleType, str_RoleNumber);
            //加载基本信息
            BeginGetIntializeData(str_RoleType, str_RoleNumber);

            ////开始的时候让通过和驳回按钮不可用
            //this.btnPass.Enabled = false;
            //this.btnReject.Enabled = false;
            this.PBload.Visible = false;
        }
        #region
        /// <summary>
        /// 查看身份证扫描件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckSFZSMJ_Click(object sender, EventArgs e)
        {
            //有地址
            if (txtSFZSMJ.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + txtSFZSMJ.Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

        /// <summary>
        /// 查看签字承诺书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckQZCNS_Click(object sender, EventArgs e)
        {      //有地址
            if (txtQZCNS.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + txtQZCNS.Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }
        /// <summary>
        /// 查看营业执照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckYYZZ_Click(object sender, EventArgs e)
        {
           
                     //有地址
            if (txtYYZZ.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + txtYYZZ.Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }
      
        /// <summary>
        /// 查看组织结构代码证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckZZJGDMZ_Click(object sender, EventArgs e)
        {
            
                           //有地址
            if (txtZZJGDMZ.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + txtZZJGDMZ.Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }

       
        /// <summary>
        /// 查看税务登记证扫面件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckSWDJZ_Click(object sender, EventArgs e)
        {
                                 //有地址
            if (txtSWDJZ.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + txtSWDJZ.Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }
        /// <summary>
        /// 开户许可证扫面件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckKHXKZ_Click(object sender, EventArgs e)
        {
                                   //有地址
            if (txtKHXKZ.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + txtKHXKZ.Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }
        //其他资质证件
        private void lblQTZZCK_Click(object sender, EventArgs e)
        {
            if (txtQTZZ.Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1] + "/JHJXPT/SaveDir/" + txtQTZZ.Text.Trim().Replace(@"\", "/");
                StringOP.OpenUrl(url);
            }
        }
        #endregion
        

      
    }
}
