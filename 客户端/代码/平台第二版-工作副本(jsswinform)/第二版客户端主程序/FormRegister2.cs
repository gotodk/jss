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
using System.Text.RegularExpressions;
using 客户端主程序.NewDataControl;
namespace 客户端主程序
{
    public partial class FormRegister2 : BasicForm
    {
        /// <summary>
        /// 主键
        /// </summary>
        string pk;
        /// <summary>
        /// 登陆窗体
        /// </summary>
        FormLogin fmLogin;
        /// <summary>
        /// 登录邮箱
        /// </summary>
        string dlyx = string.Empty;
        /// <summary>
        /// 用户名（昵称）
        /// </summary>
        string yhm = string.Empty;
        /// <summary>
        /// 登录密码
        /// </summary>
        string dlmm = string.Empty;
        /// <summary>
        /// 验证码
        /// </summary>
        string yzm = string.Empty;
        /// <summary>
        /// 重新发送邮件时，是否显示成功对话框
        /// </summary>
        bool isShowDialog = true;
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="uid">登录邮箱</param>
        /// <param name="uname">用户名（昵称）</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="valNum">验证码</param>
        public FormRegister2(string PKNumber,string uid,string uname,string pwd,string valNum,FormLogin fms)
        {
            InitializeComponent();
            pk = PKNumber;
            dlyx = uid;
            yhm = uname;
            dlmm = pwd;
            yzm = valNum;
            fmLogin = fms;
            //MessageBox.Show("1");
        }
        /// <summary>
        /// 重载构造函数，用于登录时判断是否激活邮箱，自动发送邮件
        /// </summary>
        /// <param name="PKNumber"></param>
        /// <param name="uid"></param>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <param name="fms"></param>
        public FormRegister2(string PKNumber, string uid, string uname, string pwd, FormLogin fms)
        {
            InitializeComponent();
            pk = PKNumber;
            dlyx = uid;
            yhm = uname;
            dlmm = pwd;
            fmLogin = fms;
        }
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
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRegister2_Load(object sender, EventArgs e)
        {
            Init_one_show();
            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;

            //MessageBox.Show("2");
            string emailUrl = "http://mail.";
            if (dlyx != string.Empty && dlyx != null && dlyx.Trim() != "")
            {
                string[] email = dlyx.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                if (email.Length == 2)
                {
                    emailUrl += email[1];
                }
                else
                {
                    emailUrl += "fm8844.com";
                }
            }
            tips.SetToolTip(lbCheckEmail, emailUrl);//设置tooltip
        }
        /// <summary>
        /// 去查看邮箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbCheckEmail_Click(object sender, EventArgs e)
        {
            string url = tips.GetToolTip(lbCheckEmail);
            Support.NetLink.OpenLink(url, true);
        }
        /// <summary>
        /// 再次发送(重新发送一次邮件，服务器端更新验证码和验证码时间)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSendAgain_Click(object sender, EventArgs e)
        {
            SetEnable(false);
            string RandomNumber = "123456";// Support.StringOP.GetRandomNumber(10, true);
            Hashtable ht = new Hashtable();
            string email = dlyx;
            ht["dlyx"] = email;
            ht["valNum"] = RandomNumber;
            delegateForThread tempDFT = new delegateForThread(showResult_SendAgain);
            //DataControl.RunThreadClassReSendValNum dcr = new RunThreadClassReSendValNum(ht, tempDFT);
            G_Reg g = new G_Reg(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(g.SendAgain));
            trd.IsBackground = true;
            trd.Start();
        }
        /// <summary>
        /// 更换邮箱(重新实例化第一步,将此步从内存中回收)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbChangeEmail_Click(object sender, EventArgs e)
        {
            FormRegister fm = new FormRegister(pk, dlyx, yhm, dlmm, fmLogin, yzm);
            fm.Show();
            this.Hide();
            this.Dispose();
        }
        /// <summary>
        /// 一次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void showResult_SendAgain(Hashtable OutPutHT)
        {
            try
            {
               
                Invoke(new delegateForThreadShow(showThreadResult_SendAgain_Invoke), new Hashtable[] { OutPutHT });
             
            }
            catch(Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 再次发送二次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void showThreadResult_SendAgain_Invoke(Hashtable OutPutHT)
        {
            SetEnable(true);
            DataSet ds = (DataSet)OutPutHT["执行结果"];
            DataTable dt = ds.Tables[0];
            if (dt.Rows[0]["执行结果"].ToString() == "FASLE")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add(dt.Rows[0]["提示文本"].ToString());
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
            }
            else
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("已再次发送验证码，请注意查收。");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                if (isShowDialog)
                {
                    fm.ShowDialog();
                }
                yzm = "123456";// dt.Rows[0]["YZM"].ToString();//更新验证码
            }
        }

        /// <summary>
        /// 设置本页面的控件可用性
        /// </summary>
        /// <param name="b"></param>
        private void SetEnable(bool b)
        {
            PBload.Visible = (!b);
            btnNext.Enabled = b;
            lbSendAgain.Enabled = b;
            lbCheckEmail.Enabled = b;
            lbChangeEmail.Enabled = b;
        }
        /// <summary>
        /// 下一步，注册成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            string uInputVal = txtValNum.Text;//用户输入的验证码
            if (uInputVal.Trim() == "")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请输入验证码！");
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return;
            }

            if (uInputVal != yzm)
            {
                //ArrayList al = new ArrayList();
                //al.Add("");
                //al.Add("验证码不正确，请重新输入！");
                //FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                //fm.ShowDialog();
                //return;

                SetEnable(false);
                Hashtable ht = new Hashtable();
                ht["dlyx"] = dlyx;
                ht["valNum"] = yzm;
                delegateForThread tempDFT = new delegateForThread(ShowResult_RegStepTwoToThree);
                G_Reg g = new G_Reg(ht, tempDFT);
                //DataControl.RunThreadClassAccountID rtccl = new RunThreadClassAccountID(ht, tempDFT);
                Thread trd = new Thread(new ThreadStart(g.AccountID));
                trd.IsBackground = true;
                trd.Start();
            }
            else
            {
                //提示服务器执行可登录语句
                SetEnable(false);
                Hashtable ht = new Hashtable();
                ht["dlyx"] = dlyx;
                ht["valNum"] = yzm;
                delegateForThread tempDFT = new delegateForThread(ShowResult_RegStepTwoToThree);
                G_Reg g = new G_Reg(ht, tempDFT);
                //DataControl.RunThreadClassAccountID rtccl = new RunThreadClassAccountID(ht, tempDFT);
                Thread trd = new Thread(new ThreadStart(g.AccountID));
                trd.IsBackground = true;
                trd.Start();
            }
        }
        private void ShowResult_RegStepTwoToThree(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                Invoke(new delegateForThreadShow(ShowResult_RegStepTwoToThree_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout();
            }
            catch(Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 成功的二次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void ShowResult_RegStepTwoToThree_Invoke(Hashtable OutPutHT)
        {
            SetEnable(true);
            DataSet ds = (DataSet)OutPutHT["执行结果"];
            if (ds.Tables[0].Rows[0]["执行结果"].ToString() == "SUCCESS")
            {
                string dlyx = ds.Tables[0].Rows[0]["附件信息1"].ToString();//用户帐号
                string yhm = ds.Tables[0].Rows[0]["附件信息2"].ToString();//昵称
                string dlmm = ds.Tables[0].Rows[0]["附件信息3"].ToString();//密码
                FormRegister3 fm3 = new FormRegister3(fmLogin,dlyx,yhm,dlmm);
                fm3.Show();
                this.Hide();
                this.Dispose();
            }
            else
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add(ds.Tables[0].Rows[0]["提示文本"].ToString());
                FormAlertMessage fm = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                fm.ShowDialog();
                return;
            }
        }

        private void FormRegister2_FormClosing(object sender, FormClosingEventArgs e)
        {
            fmLogin.Show();
            this.Hide();
            this.Dispose();
        }
        /// <summary>
        /// 再次发送邮件，只在登录时用到
        /// </summary>
        public void SendEmailAgain()
        {
            isShowDialog = false;
            SetEnable(false);
            string RandomNumber = "123456"; //Support.StringOP.GetRandomNumber(10, true);
            Hashtable ht = new Hashtable();
            string email = dlyx;
            ht["dlyx"] = email;
            ht["valNum"] = RandomNumber;
            delegateForThread tempDFT = new delegateForThread(showResult_SendAgain);
            G_Reg g = new G_Reg(ht, tempDFT);
            Thread trd = new Thread(new ThreadStart(g.SendAgain));
            trd.IsBackground = true;
            trd.Start();
        }

      

       
    }
}
