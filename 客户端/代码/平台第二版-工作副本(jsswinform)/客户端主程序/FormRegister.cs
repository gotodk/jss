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

namespace 客户端主程序
{
    public partial class FormRegister : BasicForm
    {
        /// <summary>
        /// 随时准备返回登录窗口
        /// </summary>
        FormLogin fmLogin;
        /// <summary>
        /// 用户邮箱
        /// </summary>
        string uEmail;
        /// <summary>
        /// 用户名
        /// </summary>
        string uName;
        /// <summary>
        /// 密码
        /// </summary>
        string uPwd;
        /// <summary>
        /// 主键
        /// </summary>
        string pk = string.Empty;
        /// <summary>
        /// 构造函数
        /// </summary>
        public FormRegister(FormLogin fms)
        {
            InitializeComponent();
            fmLogin = fms;
            this.txtUserEmail.Focus();//窗体载入后，用户邮箱文本框获得焦点
        }
        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="email"></param>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        public FormRegister(string pkNumber,string email,string uname,string pwd,FormLogin fms)
        {
            InitializeComponent();
            pk = pkNumber;
            uEmail = email;
            uName = uname;
            uPwd = pwd;
            fmLogin = fms;
            this.txtUserEmail.Focus();//窗体载入后，用户邮箱文本框获得焦点
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
        private void FormRegister_Load(object sender, EventArgs e)
        {
            //四个Error提示
            this.TipsErrorEmail.Visible = false;//邮箱错误提示
            this.TipsErrorUName.Visible = false;//用户名错误提示
            this.TipsErrorUPwd.Visible = false;//密码错误提示
            this.TipsErrorPwdAgain.Visible = false;//再次输入密码错误提示
            //四个Input提示
            this.TipsInputEmail.Visible = true;
            this.TipsInputUName.Visible = false;
            this.TipsInputPwd.Visible = false;
            this.TipsInputPwdAgain.Visible = false;
            //窗口载入后
            
            this.txtUserEmail.LostFocus+=txtUserEmail_LostFocus;//注册失焦事件
            this.txtUserPwd.LostFocus += txtUserPwd_LostFocus;//密码框失焦事件
            this.txtUserName.LostFocus += txtUserName_LostFocus;//注册用户名文本框失焦
            this.txtUserPwdAgain.LostFocus += txtUserPwdAgain_LostFocus;//注册再次输入密码失焦事件
            if (uEmail != null && uEmail != string.Empty && uEmail != "")
            {
                this.txtUserEmail.Text = uEmail;
            }
            if (uName != null && uName != string.Empty && uName != "")
            {
                this.txtUserName.Text = uName;
            }
            if (uPwd != null && uPwd != string.Empty && uPwd != "")
            {
                this.txtUserPwd.Text = uPwd;
                this.txtUserPwdAgain.Text = uPwd;
            }

            //处理回车登陆
            Button Btemp = new Button();
            Btemp.Location = new Point(-200, -200);
            Btemp.Width = 0;
            Btemp.Height = 0;
            Btemp.Click += new System.EventHandler(this.btnNext_Click);
            this.AcceptButton = Btemp;
        }
        #region 四个文本框失焦事件
        /// <summary>
        /// 密码文本框失焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtUserPwdAgain_LostFocus(object sender, EventArgs e)
        {
            this.TipsInputPwdAgain.Visible = false;
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 用户名文本框失焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtUserName_LostFocus(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            this.TipsInputUName.Visible = false;
        }
        /// <summary>
        /// 密码框失焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtUserPwd_LostFocus(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            this.TipsInputPwd.Visible = false;
        }
        /// <summary>
        /// Email失焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtUserEmail_LostFocus(object sender, EventArgs e)
        {
            this.TipsInputEmail.Visible = false;
        }
        #endregion
        #region 四个文本框的验证
        /// <summary>
        /// 邮箱验证方法
        /// </summary>
        private bool ValEmail(bool b)
        {
            string email = this.txtUserEmail.Text;
            if (!Support.ValStr.isEmail(email))
            {
                this.TipsErrorEmail.Visible = true;
                this.TipsErrorEmail.Text = "邮箱格式不正确";
                return false;
            }
            else
            {
                this.TipsErrorEmail.Visible = false;
                return true;
            }
        }
        private void ValEmail()
        {
            string email = this.txtUserEmail.Text;
            if (!Support.ValStr.isEmail(email))
            {
                this.TipsErrorEmail.Visible = true;
                this.TipsErrorEmail.Text = "邮箱格式不正确";
            }
            else
                this.TipsErrorEmail.Visible = false;
        }
        /// <summary>
        /// 用户名验证方法
        /// </summary>
        private bool ValUName(bool b)
        {
            string uname = this.txtUserName.Text;
            if (!Support.ValStr.isUserName(uname))
            {
                this.TipsErrorUName.Visible = true;
                this.TipsErrorUName.Text = "用户名格式不正确";
                return false;
            }
            else
            {
                this.TipsErrorUName.Visible = false;
                return true;
            }
        }
        private void ValUName()
        {
            string uname = this.txtUserName.Text;
            if (!Support.ValStr.isUserName(uname))
            {
                this.TipsErrorUName.Visible = true;
                this.TipsErrorUName.Text = "用户名格式不正确";
            }
            else
                this.TipsErrorUName.Visible = false;
        }
        /// <summary>
        /// 用户密码验证方法
        /// </summary>
        private bool ValUPwd(bool b)
        {
            string pwd = this.txtUserPwd.Text;
            if (!Support.ValStr.isPTPwd(pwd))
            {
                this.TipsErrorUPwd.Visible = true;
                this.TipsErrorUPwd.Text = "密码格式不正确";
                return false;
            }
            else
            {
                this.TipsErrorUPwd.Visible = false;
                return true;
            }
        }
        private void ValUPwd()
        {
            string pwd = this.txtUserPwd.Text;
            if (!Support.ValStr.isPTPwd(pwd))
            {
                this.TipsErrorUPwd.Visible = true;
                this.TipsErrorUPwd.Text = "密码格式不正确";
            }
            else
                this.TipsErrorUPwd.Visible = false;
        }
        /// <summary>
        /// 再次输入密码验证方法
        /// </summary>
        private bool ValUPwdAgain(bool b)
        {
            string pwd = this.txtUserPwd.Text;
            string pwdRe = this.txtUserPwdAgain.Text;
            if (!(pwd == pwdRe))
            {
                this.TipsErrorPwdAgain.Visible = true;
                this.TipsErrorPwdAgain.Text = "两次密码输入不一致";
                return false;
            }
            else
            {
                this.TipsErrorPwdAgain.Visible = false;
                return true;
            }
        }
        private void ValUPwdAgain()
        {
            string pwd = this.txtUserPwd.Text;
            string pwdRe = this.txtUserPwdAgain.Text;
            if (!(pwd == pwdRe))
            {
                this.TipsErrorPwdAgain.Visible = true;
                this.TipsErrorPwdAgain.Text = "两次密码输入不一致";
            }
            else
                this.TipsErrorPwdAgain.Visible = false;
        }
        #endregion
        #region 四个文本框的焦点事件
        /// <summary>
        /// Email文本框单击事件（焦点事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserEmail_Click(object sender, EventArgs e)
        {
            if (txtUserEmail.Text.Trim() == "")
            {
                this.TipsInputEmail.Visible = true;
                this.TipsErrorEmail.Visible = false;
            }
            else
            {
                this.TipsInputEmail.Visible = false;
            }
        }
        /// <summary>
        /// 用户名文本框单击事件（焦点事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.Trim() == "")
            {
                this.TipsInputUName.Visible = true;
                this.TipsErrorUName.Visible = false;
            }
            else
                this.TipsInputUName.Visible = false;
        }
        /// <summary>
        /// 密码文本框单击事件（焦点事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserPwd_Click(object sender, EventArgs e)
        {
            if (txtUserPwd.Text.Trim() == "")
            {
                TipsInputPwd.Visible = true;
                this.TipsErrorUPwd.Visible = false;
            }
            else
                TipsInputPwd.Visible = false;
        }
        /// <summary>
        /// 再次输入密码文本框单击事件（焦点事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserPwdAgain_Click(object sender, EventArgs e)
        {
            if (txtUserPwdAgain.Text.Trim() == "")
            {
                this.TipsErrorPwdAgain.Visible = false;
                TipsInputPwdAgain.Visible = true;
            }
            else
                TipsInputPwdAgain.Visible = false;
        }
        #endregion
        #region 四个文本框的输入事件

        private void txtUserPwdAgain_KeyUp(object sender, KeyEventArgs e)
        {
            ValUPwd();
            ValUPwdAgain();
            if (txtUserPwdAgain.Text == "")
            {
                TipsInputPwdAgain.Visible = true;
                TipsErrorPwdAgain.Visible = false;
            }
            else
                TipsInputPwdAgain.Visible = false;
        }       

        private void txtUserPwd_KeyUp(object sender, KeyEventArgs e)
        {
            ValUPwd();
            if(txtUserPwdAgain.Text!="")
                ValUPwdAgain();
            if (txtUserPwd.Text == "")
            {
                TipsInputPwd.Visible = true;
                TipsErrorUPwd.Visible = false;
            }
            else
                TipsInputPwd.Visible = false;
        }
        #endregion
        /// <summary>
        /// 提交第一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            setTipInputVisible(false);//隐藏输入提示按钮
            if (ValEmail(false) && ValUName(false) && ValUPwd(false) && ValUPwdAgain(false))
            {
                if (pk == string.Empty)
                {
                    SetEnable(false);
                    Hashtable ht = new Hashtable();
                    ht["dlyx"] = this.txtUserEmail.Text;//邮箱
                    ht["yhm"] = this.txtUserName.Text;//用户名
                    ht["pwd"] = this.txtUserPwd.Text;//密码
                    ht["yzm"] = Support.StringOP.GetRandomNumber(10, true);//验证码
                    delegateForThread tempDFT = new delegateForThread(showResult_registerFirst);
                    DataControl.RunThreadClassRegister rtccl = new RunThreadClassRegister(ht, tempDFT);
                    Thread trd = new Thread(new ThreadStart(rtccl.beginRun));
                    trd.IsBackground = true;
                    trd.Start();
                }
                else
                {
                    SetEnable(false);
                    Hashtable ht = new Hashtable();
                    ht["pk"] = pk;//邮箱
                    ht["dlyx"] = this.txtUserEmail.Text;//邮箱
                    ht["yhm"] = this.txtUserName.Text;//用户名
                    ht["pwd"] = this.txtUserPwd.Text;//密码
                    ht["yzm"] = Support.StringOP.GetRandomNumber(10, true);//验证码
                    delegateForThread tempDFT = new delegateForThread(showResult_registerFirst_Change);
                    DataControl.RunThreadClassUserChangeRegister rtccl = new RunThreadClassUserChangeRegister(ht, tempDFT);
                    Thread trd = new Thread(new ThreadStart(rtccl.beginRun));
                    trd.IsBackground = true;
                    trd.Start();
                }
            }
            else
            {
                ValEmail();
                ValUName();
                ValUPwd();
                ValUPwdAgain();
                return;
            }
        }
        /// <summary>
        /// 一次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void showResult_registerFirst(Hashtable OutPutHT)
        {
            try
            {
             
                Invoke(new delegateForThreadShow(showResult_Register), new Hashtable[] { OutPutHT });
            
            }
            catch(Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 二次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void showResult_Register(Hashtable OutPutHT)
        {
            SetEnable(true);
            DataSet ds = (DataSet)OutPutHT["执行结果"];
            if (ds.Tables[0].Rows[0]["RESULT"].ToString() == "SUCCESS")
            {
                string pk = ds.Tables[0].Rows[0]["pk"].ToString();//主键
                string dlyx = ds.Tables[0].Rows[0]["DLYX"].ToString();//邮箱
                string yhm = ds.Tables[0].Rows[0]["YHM"].ToString();//用户名
                string dlmm = ds.Tables[0].Rows[0]["DLMM"].ToString();//登录密码
                string yzm = ds.Tables[0].Rows[0]["YZM"].ToString();//验证码
                FormRegister2 fr = new FormRegister2(pk,dlyx, yhm, dlmm, yzm,fmLogin);
                fr.Show();
                this.Hide();
                this.Dispose();
            }
            else
            {
                //如果失败
                ArrayList al = new ArrayList();
                al.Add("");
                FormAlertMessage fm = null;
                //al.Add()
                if (ds.Tables[0].Rows[0]["REASON"].ToString() == "已存在该帐户")
                {
                    //登录邮箱重复时提示：很抱歉，该邮箱已被注册，您可以尝试其他邮箱！
                    al.Add("很抱歉，该邮箱已被注册，");
                    al.Add("您可以尝试其他邮箱！");
                    fm = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                }
                else if (ds.Tables[0].Rows[0]["REASON"].ToString() == "用户名重复")
                {
                    al.Add("很抱歉，该用户名已被注册，");
                    al.Add("您可以尝试其他用户名！");
                    fm = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                }
                else
                {
                    Support.StringOP.WriteLog("注册时服务器端代码异常：" + ds.Tables[0].Rows[0]["REASON"].ToString());
                    al.Add("服务器繁忙，请稍后再试！");
                    fm = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                }
                
               
            }
        }


        private void showResult_registerFirst_Change(Hashtable OutPutHT)
        {
            try
            {
            
                Invoke(new delegateForThreadShow(showResult_Register_Change), new Hashtable[] { OutPutHT });
              
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.Message);
            }
        }
        private void showResult_Register_Change(Hashtable OutPutHT)
        {
            SetEnable(true);
            DataSet ds = (DataSet)OutPutHT["执行结果"];
            if (ds.Tables[0].Rows[0]["RESULT"].ToString() == "SUCCESS")
            {
                string pk = ds.Tables[0].Rows[0]["pk"].ToString();//主键
                string dlyx = ds.Tables[0].Rows[0]["DLYX"].ToString();//邮箱
                string yhm = ds.Tables[0].Rows[0]["YHM"].ToString();//用户名
                string dlmm = ds.Tables[0].Rows[0]["DLMM"].ToString();//登录密码
                string yzm = ds.Tables[0].Rows[0]["YZM"].ToString();//验证码
                FormRegister2 fr = new FormRegister2(pk, dlyx, yhm, dlmm, yzm, fmLogin);
                fr.Show();
                this.Hide();
                this.Dispose();
            }
            else
            {
                //如果失败
                ArrayList al = new ArrayList();
                al.Add("");
                FormAlertMessage fm = null;
                //al.Add()
                if (ds.Tables[0].Rows[0]["REASON"].ToString() == "已存在该帐户" || ds.Tables[0].Rows[0]["REASON"].ToString() == "用户名重复")
                {
                    al.Add(ds.Tables[0].Rows[0]["REASON"].ToString());
                    fm = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                }
                else
                {
                    Support.StringOP.WriteLog("注册时服务器端代码异常：" + ds.Tables[0].Rows[0]["REASON"].ToString());
                    al.Add("服务器繁忙，请稍后再试！");
                    fm = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", al);
                    fm.ShowDialog();
                }


            }
        }
        private void SetEnable(bool b)
        {
            this.btnNext.Enabled = b;
            this.PBload.Visible = (!b);
        }

        /// <summary>
        /// 窗体关闭时，显示登录窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            fmLogin.Show();
            this.Hide();
            this.Dispose();
        }
        void setTipInputVisible(bool b)
        {
            this.TipsInputEmail.Visible = b;
            this.TipsInputUName.Visible = b;
            this.TipsInputPwd.Visible = b;
            this.TipsInputPwdAgain.Visible = b;
        }

        private void txtUserEmail_Leave(object sender, EventArgs e)
        {
            ValEmail();
            if (txtUserEmail.Text == "")
            {
                this.TipsInputEmail.Visible = true;
                this.TipsErrorEmail.Visible = false;
            }
            else
                this.TipsInputEmail.Visible = false;
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            ValUName();
            if (txtUserName.Text == "")
            {
                this.TipsInputUName.Visible = true;
                this.TipsErrorUName.Visible = false;
            }
            else
                this.TipsInputUName.Visible = false;
        }
    }
}
