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
    public partial class FormResetPwd : BasicForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FormResetPwd()
        {
            InitializeComponent();
            this.btnNext.Enabled = false;
            this.lbEmailText.Text = "";
            this.lbPhoneText.Text = "";
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
        /// Email单选按钮被选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnEmail_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPhone.Checked)
            {
                rbtnPhone.Checked = false;
                rbtnEmail.Checked = true;
            }
            ValEmail();
        }
        /// <summary>
        /// Phone单选按钮被选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnPhone_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnEmail.Checked)
            {
                rbtnEmail.Checked = false;
                rbtnPhone.Checked = true;
            }
            ValPhone();
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_UserEmail_KeyUp(object sender, KeyEventArgs e)
        {
            ValEmail();
        }
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhone_KeyUp(object sender, KeyEventArgs e)
        {
            ValPhone();
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        private void ValEmail()
        {
            this.lbPhoneText.Text = "";
            bool isEmail = rbtnEmail.Checked ? true : false;
            if (isEmail)
            {
                //如果Email单选按钮被选中，则执行下面的邮箱验证，并显示相应的信息
                string mail = this.txt_UserEmail.Text;
                //bool b = Regex.IsMatch(mail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                bool b = Support.ValStr.isEmail(mail);//没找到很靠谱的正则表达式
                if (!b)
                {
                    this.btnNext.Enabled = false;
                    this.lbEmailText.Text = "邮箱格式不正确";
                }
                else
                {
                    this.btnNext.Enabled = true;
                    this.lbEmailText.Text = "";
                }
            }
        }
        /// <summary>
        /// 验证手机
        /// </summary>
        private void ValPhone()
        {
            this.lbEmailText.Text = "";
            bool isPhone = rbtnPhone.Checked ? true : false;
            if (isPhone)
            {
                //如果手机号码被选中，则执行下面的手机号码验证，并显示相应的信息
                string phone = this.txtPhone.Text;
                bool b = Support.ValStr.isPhone(phone);
                if (!b)
                {
                    this.btnNext.Enabled = false;
                    this.lbPhoneText.Text = "手机号格式不正确";
                }
                else
                {
                    this.btnNext.Enabled = true;
                    this.lbPhoneText.Text = "";
                }
            }
        }
        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormResetPwd_Load(object sender, EventArgs e)
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            //MessageBox.Show();
        }
        /// <summary>
        /// 下一步按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            setEnabled(false);
            string typeToSet = null;
            string RandomNumber = Support.StringOP.GetRandomNumber(10, true);//随机验证码
            bool isEmail = rbtnEmail.Checked ? true : false;
            Hashtable ht = new Hashtable();
            if (isEmail)
            {
                typeToSet = "email";
                string email = this.txt_UserEmail.Text.Trim();
                ht["dlyx"] = email;
                ht["typeToSet"] = typeToSet;
                ht["RandomNumber"] = RandomNumber;
                delegateForThread tempDFT = new delegateForThread(showThreadResult_RePwd);
                DataControl.RunThreadClassRePwd rePwd = new RunThreadClassRePwd(ht,tempDFT);
                Thread td = new Thread(new ThreadStart(rePwd.runThreading));
                td.IsBackground = true;
                td.Start();
            }
            else
            {
                typeToSet = "phone";
                string phone = this.txtPhone.Text.Trim();
                ht["dlyx"] = phone;
                ht["typeToSet"] = typeToSet;
                ht["RandomNumber"] = RandomNumber;
                delegateForThread tempDFT = new delegateForThread(showThreadResult_RePwd);
                DataControl.RunThreadClassRePwd rePwd = new RunThreadClassRePwd(ht, tempDFT);
                Thread td = new Thread(new ThreadStart(rePwd.runThreading));
                td.IsBackground = true;
                td.Start();
            }
        }
        /// <summary>
        /// 一次回调
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
        /// 二次回调
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void showThreadResult_RePwd_Invoke(Hashtable OutPutHT)
        {
            setEnabled(true);
            DataSet ds = (DataSet)OutPutHT["执行结果"];
            //MessageBox.Show(ds.Tables.Count.ToString());
            //如果是两个，说明成功了。如果是1个说明没用户。如果是null，说明方法有异常。
            if (ds.Tables.Count == 2)
            {
                if (ds.Tables["帐号信息"].Rows[0]["查询信息"].ToString().Trim() != "正常")
                {
                    ArrayList al = new ArrayList();
                    al.Add("");
                    al.Add("系统繁忙，请稍后再试！");
                    FormAlertMessage alert = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", al);
                    alert.ShowDialog();
                    return;
                }
                else
                {
                    string ValNumber = ds.Tables["帐号信息"].Rows[0]["本次验证码"].ToString();//验证码
                    string type = ds.Tables["验证码信息"].Rows[0]["subType"].ToString().Trim();//选择方式信息
                    string uid = ds.Tables["帐号信息"].Rows[0]["DLYX"].ToString();//email
                    if (type == "email")
                    {
                        this.Hide();
                        FormResetPwd2 fm2 = new FormResetPwd2(ValNumber, uid, ds);
                        fm2.Show();
                    }
                    else
                    {
                        this.Hide();
                        FormResetPwd3 fm3 = new FormResetPwd3(ValNumber, uid, ds);
                        fm3.Show();
                    }
                }
            }
            else
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("系统中不存在该帐号！");
                FormAlertMessage alert = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", al);
                alert.ShowDialog();
                return;
            }

        }
        /// <summary>
        /// 设置可用性
        /// </summary>
        /// <param name="setFor"></param>
        private void setEnabled(bool setFor)
        {
            rbtnEmail.Enabled = setFor;
            rbtnPhone.Enabled = setFor;
            txt_UserEmail.Enabled = setFor;
            txtPhone.Enabled = setFor;
            btnNext.Enabled = setFor;
        }
    }
}
