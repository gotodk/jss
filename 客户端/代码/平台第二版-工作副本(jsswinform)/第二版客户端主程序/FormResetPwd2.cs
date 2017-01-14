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
    /// <summary>
    /// 通过邮箱修改密码
    /// </summary>
    public partial class FormResetPwd2 : BasicForm
    {
        /// <summary>
        /// 验证码（在第一步中首先生成，返回到服务器，然后服务器插入记录，反向查询后返回该验证码）
        /// </summary>
        public string ValsNumber;
        /// <summary>
        /// 将要修改密码的用户email
        /// </summary>
        public string uid;
        /// <summary>
        /// 由第一步查询返回的DataSet
        /// </summary>
        public DataSet dsReturn;
        /// <summary>
        /// 邮件倒计时线程
        /// </summary>
        Thread thdForMail = null;
        /// <summary>
        /// 线程调用flag
        /// </summary>
        bool bForMail = true;


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
        /// 初始化构造函数
        /// </summary>
        /// <param name="vals">验证码</param>
        /// <param name="email">用户Email</param>
        public FormResetPwd2(string vals,string email,DataSet ds)
        {
            InitializeComponent();
            dg = new DGEmailCount(ShowCount);//实例化计数委托
            this.ValsNumber = vals;
            this.uid = email;
            this.dsReturn = ds;
            this.lbValNumberTips.Text = "";
            this.lbNewPwdTips.Text = "";
            this.lbNewPwdAgainTips.Text = "";
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            string vals = this.txtValsNumber.Text;
            string pwd = this.txtNewPwd.Text;
            string repwd = this.txtNewPwdAgain.Text;
            #region 验证过程
            bool val1 = valNumbers();
            bool val2 = valPwdFormat();
            bool val3 = valPwdConpare();
            if (!(val1 && val2 && val3))
            {
                return;
            }
            #endregion
            Hashtable InPutHT = new Hashtable();
            InPutHT["dlyx"] = uid;
            InPutHT["types"] = uid;
            InPutHT["pwd"] = pwd;
            InPutHT["valNum"] = vals;
            InPutHT["typeToSet"] = "email";
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_ChangePwd);
            //DataControl.RunThreadClassChangePwd cgPWD = new DataControl.RunThreadClassChangePwd(InPutHT, tempDFT);
            G_RePwd g = new G_RePwd(InPutHT, tempDFT);
            Thread thd = new Thread(new ThreadStart(g.ChangePwd));
            thd.IsBackground = true;
            thd.Start();
        }
        /// <summary>
        /// 修改密码一次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void ShowThreadResult_ChangePwd(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                Invoke(new delegateForThreadShow(ShowThreadResult_ChangePwd_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 修改密码二次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void ShowThreadResult_ChangePwd_Invoke(Hashtable OutPutHT)
        {
            //'' AS Uid, '' AS Email,'' AS Phone,'' AS NewPwd,'' AS Results （ON WEBSERVICES）
            DataSet ds = (DataSet)OutPutHT["执行结果"];
            if (ds != null && ds.Tables[0].Rows[0]["Result"].ToString() == "成功")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("您的密码修改成功，请登录！");
                FormAlertMessage alert = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                alert.ShowDialog();
                this.Hide();
                this.Dispose();
            }
            else
            {
                bool b = ds == null ? true : false;
                if (b)
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add("系统错误，请联系我司客户！");
                    FormAlertMessage alert = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                    alert.ShowDialog();
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["Result"].ToString().IndexOf("已重新发送验证邮件") > 1)
                    {
                        ReSendEmail();
                    }
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add(ds.Tables[0].Rows[0]["Result"].ToString());
                    FormAlertMessage alert = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                    alert.ShowDialog();
                }
            }
        }
        /// <summary>
        /// 窗体载入时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormResetPwd2_Load(object sender, EventArgs e)
        {
            Init_one_show();
            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\ji.ico");
            this.Icon = ic;

            string emailUrl = "http://mail.";
            if (uid != string.Empty && uid != null && uid != "")
            {
                string[] email =uid.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                if (email.Length == 2)
                {
                    emailUrl += email[1];
                }
                else
                {
                    //一般代码走不到这一步
                    emailUrl += "fm8844.com";
                }
            }
            tips.SetToolTip(lbCheckEmail,emailUrl);
        }
        /// <summary>
        /// 打开邮箱链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbCheckEmail_Click(object sender, EventArgs e)
        {
            string url = tips.GetToolTip(lbCheckEmail);//得到网址
            Support.NetLink.OpenLink(url, true);
        }
        /// <summary>
        /// 再次发送邮件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbSendAgain_Click(object sender, EventArgs e)
        {
            setEnable(false);
            string typeToSet = "email";
            string RandomNumber = Support.StringOP.GetRandomNumber(10, true);
            Hashtable ht = new Hashtable();
            string email = uid;
            ht["dlyx"] = email;
            ht["typeToSet"] = typeToSet;
            ht["RandomNumber"] = RandomNumber;
            delegateForThread tempDFT = new delegateForThread(showThreadResult_RePwd);
            //DataControl.RunThreadClassRePwd rePwd = new RunThreadClassRePwd(ht, tempDFT);
            G_RePwd g = new G_RePwd(ht, tempDFT);
            Thread td = new Thread(new ThreadStart(g.RePwd));
            td.IsBackground = true;
            td.Start();
        }
        /// <summary>
        /// 再次发送邮件一次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void showThreadResult_RePwd(Hashtable OutPutHT)
        {
            try
            {
                this.SuspendLayout();
                Invoke(new delegateForThreadShow(showThreadResult_RePwd_Invoke), new Hashtable[] { OutPutHT });
                this.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 再次发送邮件二次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void showThreadResult_RePwd_Invoke(Hashtable OutPutHT)
        {
            setEnable(true);

            DataSet ds = (DataSet)OutPutHT["执行结果"];

            if (ds.Tables["返回值单条"].Rows[0]["执行结果"].ToString().Trim() == "用户不存在")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("系统中不存在该帐号！");
                FormAlertMessage alert = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                alert.ShowDialog();
                return;
            }           
            else if (ds.Tables["返回值单条"].Rows[0]["执行结果"].ToString().Trim() == "SUCCESS")
            {                

                uid = ds.Tables["返回值单条"].Rows[0]["附件信息1"].ToString();//email
                ValsNumber = ds.Tables["返回值单条"].Rows[0]["附件信息2"].ToString();//验证码
                //string type = ds.Tables["返回值单条"].Rows[0]["附件信息3"].ToString().Trim();//选择方式信息
                
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("已再次发送验证码，请注意查收。");
                FormAlertMessage alert = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                alert.ShowDialog();
                try
                {
                    thdForMail = new Thread(EmailCount);
                    thdForMail.IsBackground = true;
                    thdForMail.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

             else  
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("系统繁忙，请稍后再试！");
                FormAlertMessage alert = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", al);
                alert.ShowDialog();
                return;
            }    

           
        }
        /// <summary>
        /// 设置控件可用性
        /// </summary>
        /// <param name="ControlToSet"></param>
        private void setEnable(bool ControlToSet)
        {
            this.lbSendAgain.Enabled = ControlToSet;
            this.btnNext.Enabled = ControlToSet;
        }
        /// <summary>
        /// 验证码是否匹配
        /// </summary>
        /// <returns></returns>
        private bool valNumbers()
        {
            string vals = this.txtValsNumber.Text;
            if (vals.Trim() == "")
            {
                this.lbValNumberTips.Text = "验证码不可为空";
                return false;
            }
            else if (vals != ValsNumber)
            {
                this.lbValNumberTips.Text = "验证码不正确";
                return false;
            }
            else
            {
                this.lbValNumberTips.Text = "";
                return true;
            }
        }
        /// <summary>
        /// 密码格式是否正确
        /// </summary>
        /// <returns></returns>
        private bool valPwdFormat()
        {
            string pwd = this.txtNewPwd.Text;
            if (Support.ValStr.isPTPwd(pwd))
            {
                this.lbNewPwdTips.Text = "";
                return true;
            }
            else
            {
                this.lbNewPwdTips.Text = "密码格式不正确";
                if (pwd == "")
                {
                    this.lbNewPwdTips.Text = "密码不可为空";
                }
                return false;
            }
        }
        /// <summary>
        /// 两次输入密码是否一致
        /// </summary>
        /// <returns></returns>
        private bool valPwdConpare()
        {
            string pwd = this.txtNewPwd.Text;
            string repwd = this.txtNewPwdAgain.Text;
            if (repwd.Trim() == "")
            {
                this.lbNewPwdAgainTips.Text = "请再次输入密码";
                return false;
            }
            else if (pwd != repwd)
            {
                this.lbNewPwdAgainTips.Text = "两次输入密码不一致";
                return false;
            }
            else
            {
                this.lbNewPwdAgainTips.Text = "";
                return true;
            }
        }
        /// <summary>
        /// 计数委托
        /// </summary>
        /// <param name="msg"></param>
        delegate void DGEmailCount(string msg);
        /// <summary>
        /// 安全的跨线程访问计数委托
        /// </summary>
        DGEmailCount dg = null;
        /// <summary>
        /// 计数委托的实现
        /// </summary>
        /// <param name="msg"></param>
        private void ShowCount(string msg)
        {
            this.lbSendAgain.Text = msg;
        }
        /// <summary>
        /// 线程内执行计数
        /// </summary>
        private void EmailCount()
        {
            if (bForMail)
            {
                Color c = lbSendAgain.ForeColor;
                lbSendAgain.ForeColor = Color.DarkSlateGray;
                lbSendAgain.Click -= new System.EventHandler(this.lbSendAgain_Click);
                //lbSendAgain.Enabled = false;
                for (int i = 60; i > 0; i--)
                {
                    if (bForMail)
                    {
                        Thread.Sleep(1000);
                        this.Invoke(dg, i.ToString() + "秒后可以再次发送");
                    }
                    else
                        return;
                }
                //lbSendAgain.Enabled = true;
                lbSendAgain.ForeColor = c;
                //lbSendAgain.Text = "再次发送";
                this.Invoke(dg, "再次发送");
                this.lbSendAgain.Click += new EventHandler(this.lbSendAgain_Click);
            }
        }
        /// <summary>
        /// 验证密码格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNewPwd_KeyUp(object sender, KeyEventArgs e)
        {
            if(txtNewPwd.Text!="")
            valPwdFormat();
            if(txtNewPwdAgain.Text!="")
            valPwdConpare();
        }
        /// <summary>
        /// 验证两次输入密码是否一致
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNewPwdAgain_KeyUp(object sender, KeyEventArgs e)
        {
            if(txtNewPwdAgain.Text!="")
            valPwdConpare();
        }
        /// <summary>
        /// 验证验证码是否正确
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValsNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if(txtValsNumber.Text!="")
            valNumbers();
        }
        /// <summary>
        /// 重新发送邮件方法。
        /// </summary>
        private void ReSendEmail()
        {
            setEnable(false);
            string typeToSet = "email";
            string RandomNumber = Support.StringOP.GetRandomNumber(10, true);
            Hashtable ht = new Hashtable();
            string email = uid;
            ht["dlyx"] = email;
            ht["typeToSet"] = typeToSet;
            ht["RandomNumber"] = RandomNumber;
            delegateForThread tempDFT = new delegateForThread(showThreadResult_RePwd);
            //DataControl.RunThreadClassRePwd rePwd = new RunThreadClassRePwd(ht, tempDFT);
            G_RePwd g = new G_RePwd(ht, tempDFT);
            Thread td = new Thread(new ThreadStart(g.RePwd));
            td.IsBackground = true;
            td.Start();
        }

        private void FormResetPwd2_Activated(object sender, EventArgs e)
        {
            txtValsNumber.Focus();
        }

        private void FormResetPwd2_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                bForMail = false;
                if (thdForMail != null)
                {
                    thdForMail.Abort();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
