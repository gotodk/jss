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
using System.Threading;

namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class CXzhongbiaoLYHZJ : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        CXzhongbiao CXb;
        

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

        public CXzhongbiaoLYHZJ(CXzhongbiao CXbtemp)
        {
    
            InitializeComponent();
            CXb = CXbtemp;
            this.TitleYS = new int[] { 0, 0, -50 };

            //图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
        }

        private void CXweizhongbiaoEdit_Load(object sender, EventArgs e)
        {
            LLLzb.Text = CXb.Enumber; //主表编号
            LLLzibiao.Text = CXb.Eid;//子表编号
            

            panelUCedit.Enabled = false;
            resetloadLocation(BBJNLYBZJ, PBload);

            //开线程，获取款项数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_money);
            Hashtable ht = new Hashtable();
            ht["邮箱"] = PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString().Trim();
            DataControl.RunThreadClassMoney RTCM = new DataControl.RunThreadClassMoney(ht, dft);
            Thread trd = new Thread(new ThreadStart(RTCM.BeginRun));
            trd.IsBackground = true;
            trd.Start();
       
        }


        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_money(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_money_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_money_Invoke(Hashtable returnHT)
        {

            //显示执行结果
            string returnds = returnHT["执行结果"].ToString();
            string yue = returnHT["余额"].ToString();
            if(yue == "")
            {
                yue = "0";
            }
            switch (returnds)
            {
                case "ok":
                    panelUCedit.Enabled = true;
                    PBload.Visible = false;

                    label2.Visible = true;

                    //余额不足
                    if (Convert.ToDouble(yue) < Convert.ToDouble(CXb.Elyhzj))
                    {
                        label2.Text = "缴纳履约保证金后即中标，且合同生效！\n\r\n\r您本次投标需缴纳的履约保证金为***元，您当前余额为|||元，不足以缴纳请及时充值！".Replace("***", CXb.Elyhzj).Replace("|||", yue);
                        nothingqueding.Visible = true;
                        BBJNLYBZJ.Visible = false;
                        basicButton1.Visible = false;
                    }
                    else //余额充足
                    {
                        label2.Text = "缴纳履约保证金后即中标，且合同生效！\n\r\n\r您本次投标需缴纳的履约保证金为***元，您当前余额为|||元，您确定要缴纳吗？\n\r\n\r确定后直接从你帐户中扣除！".Replace("***", CXb.Elyhzj).Replace("|||", yue);
                        nothingqueding.Visible = false;
                        BBJNLYBZJ.Visible = true;
                        basicButton1.Visible = true;
                        basicButton1.Location = new Point(243, 122);
                    }

                    

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
                    Almsg4.Add(returnds);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }


        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y +23 );
            PB.Visible = true;
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="err"></param>
        private void showAlertY(string err)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(err);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "投标信息提交", Almsg3);
            FRSE3.ShowDialog();
        }

        //开始缴纳履约保证金
        private void BBJNLYBZJ_Click(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("您确定要缴纳履约保证金吗？");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "xxx", Almsg3);
            DialogResult dr = FRSE3.ShowDialog();
            if (dr != DialogResult.Yes)
            {
                return;
            }


            //禁用提交区域并开启进度
            BBJNLYBZJ.Enabled = false;
            resetloadLocation(BBJNLYBZJ, PBload);

            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassLYBZJ RTC = new DataControl.RunThreadClassLYBZJ(LLLzb.Text, LLLzibiao.Text, PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim(), dft);
            Thread trd = new Thread(new ThreadStart(RTC.BeginRun));
            trd.IsBackground = true;
            trd.Start();

        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_save(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_save_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_save_Invoke(Hashtable returnHT)
        {
            BBJNLYBZJ.Enabled = true;
            PBload.Visible = false;
            //显示执行结果
            string returnds = returnHT["执行结果"].ToString();
            switch (returnds)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("缴纳成功！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    //重新绑定数据
                    CXb.GetData(null);
                    this.Close();

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
                    Almsg4.Add(returnds);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }

        private void basicButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nothingqueding_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
