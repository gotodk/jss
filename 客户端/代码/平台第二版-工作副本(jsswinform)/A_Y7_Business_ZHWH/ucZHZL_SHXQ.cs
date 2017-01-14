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
using 客户端主程序.DataControl;
using 客户端主程序.NewDataControl;
using System.Threading;

namespace 客户端主程序.SubForm
{
    public partial class ucZHZL_SHXQ : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 用户数据信息
        /// </summary>
        Hashtable HTuser = new Hashtable();

        /// <summary>
        /// 对勾选择后的回调指针
        /// </summary>
        delegateForThread dftForParent;

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

        public ucZHZL_SHXQ(delegateForThread dftForParent_temp)
        {
    
            InitializeComponent();

            dftForParent = dftForParent_temp;
            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.flowLayoutPanel1.Enabled = false;
        }
        /// <summary>
        /// 页面加载完以后的数据信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucZHZL_SHXQ_Load(object sender, EventArgs e)
        {
         
            HTuser["DLYX"]= PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
             HTuser["JSZHLX"]= PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            SRT_GetJYZHSHData_Run();
        }



        /// <summary>
        /// 绑定界面审核详情数据信息
        /// </summary>
        private void BindDataFaceData(DataSet dsReturn)
        {
            DataTable dataTableMMJ = dsReturn.Tables["交易账户审核数据信息"];
            if(dataTableMMJ.Rows.Count>0)
            {
                this.txtKTJYZHSJ.Text = dataTableMMJ.Rows[0]["申请时间"].ToString();
                this.txtJJRSHZT.Text = dataTableMMJ.Rows[0]["经纪人审核状态"].ToString();
                this.txtJJRSHSJ.Text = dataTableMMJ.Rows[0]["经纪人审核时间"].ToString();
                this.txtJJRSHYJ.Text = dataTableMMJ.Rows[0]["经纪人审核意见"].ToString();

                this.txtFGSSHZT.Text = dataTableMMJ.Rows[0]["分公司审核状态"].ToString();
                this.txtFGSSHSJ.Text = dataTableMMJ.Rows[0]["分公司审核时间"].ToString();
                this.txtFGSSHYJ.Text = dataTableMMJ.Rows[0]["分公司审核意见"].ToString();

                if (dataTableMMJ.Rows[0]["JSZHLX"].ToString() == "买家卖家交易账户")
                {
                    this.panKTJYZHSJ.Visible = true;
                    this.panJJRSHZT.Visible = true;
                    this.panJJRSHSJ.Visible = true;
                    this.panJJRSHYJ.Visible = true;

                    //this.panFGSHX.Visible = true;
                    this.panFGSSHZT.Visible = true;
                    this.panFGSSHSJ.Visible = true;
                    this.panFGSSHYJ.Visible = true;

                }
                else
                {
                    this.panKTJYZHSJ.Visible = true;
                    this.panJJRSHZT.Visible = false;
                    this.panJJRSHSJ.Visible = false;
                    this.panJJRSHYJ.Visible = false;

                    //this.panFGSHX.Visible = false;
                    this.panFGSSHZT.Visible = true;
                    this.panFGSSHSJ.Visible = true;
                    this.panFGSSHYJ.Visible = true;
                
                }
                this.flowLayoutPanel1.Enabled = true;

            }




           
        }


        #region//开启线程获取，界面数据并且绑定界面

        //开启一个线程提交数据
        private void SRT_GetJYZHSHData_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(HTuser, new delegateForThread(SRT_GetJYZHSHData));
            Thread trd = new Thread(new ThreadStart(OTD.GetJYZHSHData));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetJYZHSHData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetJYZHSHData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetJYZHSHData_Invoke(Hashtable OutPutHT)
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

        private void txtFGSSHYJ_TextChanged(object sender, EventArgs e)
        {
            //this.txtFGSSHYJ.SelectionStart = this.txtFGSSHYJ.Text.Length;
            //this.txtFGSSHYJ.SelectionLength = 0;
            //this.txtFGSSHYJ.ScrollToCaret();
            //this.txtFGSSHYJ.AppendText("");

        }











    }
}
