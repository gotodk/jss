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

namespace 客户端主程序
{
    public partial class FormLogin : BasicForm
    {
        /// <summary>
        /// 主显示窗体
        /// </summary>
        FormMainPublic FMP;

        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        /// <summary>
        /// 是否首次打开
        /// </summary>
        string Isreopen;

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


        public FormLogin(FormMainPublic FMPtemp,string isreopen)
        {
            InitializeComponent();
            FMP = FMPtemp;
            Isreopen = isreopen;

        }

        /// <summary>
        /// 检查更新成功，让登陆可用
        /// </summary>
        public void uploadOK()
        {
            username.Enabled = true;
            userpassword.Enabled = true;
            MFZC.Enabled = true;
            WJMM.Enabled = true;
            JZMM.Enabled = true;
            Blogin.Enabled = true;
            PBload.Visible = false;

            //让账号输入框获得焦点
            string uid = DataControl.XMLConfig.GetConfig_NoENC("用户本地配置", "UserEmail");
            bool isSavePwd = Boolean.Parse(DataControl.XMLConfig.GetConfig_NoENC("用户本地配置", "IsSavePassWord"));
            if (isSavePwd)
            {
                this.username.Text = uid;
                string pwd = DataControl.XMLConfig.GetConfig_NoENC("用户本地配置", "UserPassWord");
                this.userpassword.Text = pwd;
                this.JZMM.Checked = true;
                this.Blogin.Focus();
            }
            else
            {
                this.username.Text = uid;
                this.userpassword.Focus();
            }
            //username.Focus();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //演示用账号密码验证登陆后，显示主窗体并关闭登陆窗口
            FMP.Show();
            this.Hide();
            FMP.userloginOK();
            
        }
        Image initimg;
        private void FormLogin_Load(object sender, EventArgs e)
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;

            //处理回车登陆
            Button Btemp = new Button();
            Btemp.Location = new Point(-200,-200);
            Btemp.Width = 0;
            Btemp.Height = 0;
            Btemp.Click += new System.EventHandler(this.basicButton2_Click);
            this.AcceptButton = Btemp;

            initimg = new Bitmap(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\loginbg.jpg");
            pictureBox1.Image = initimg;
            //====================================================
            //首次打开时更新，并启动各种线程，非首次打开不需要
            if (Isreopen == "首次打开")
            {
                //启动检查更新的线程,检查成功后，加载数据
                FMP.checkAutoUpdate();
            }
            else
            {
                //检查完毕后，恢复按钮
                Blogin.Enabled = true;
                PBload.Visible = false;
                username.Enabled = true;
                userpassword.Enabled = true;
                MFZC.Enabled = true;
                WJMM.Enabled = true;
                JZMM.Enabled = true;


            }
        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Isreopen == "首次打开")
            {
                //关闭登陆窗体时，同时关闭主窗体
                FMP.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //演示游客登陆后，显示主窗体并关闭登陆窗口
            FMP.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("测试弹出框测试弹出框。");
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "测试弹窗", Almsg3);
            FRSE3.ShowDialog();


            ArrayList Almsgxx = new ArrayList();
            Almsgxx.Add("");
            Almsgxx.Add("您是否xxxxx。");
            FormAlertMessage FRSExx = new FormAlertMessage("确定取消", "问号", "弹窗标题xxx", Almsgxx);
            DialogResult dr = FRSExx.ShowDialog();
            if (dr == DialogResult.Yes)
            {
                MessageBox.Show("点了是");
            }
            else
            {
                MessageBox.Show("点了否");
                return;
            }
        }

        private void basicButton2_Click(object sender, EventArgs e)
        {

            


            //回车时，如果未启动这个按钮，不允许提交
            if (!Blogin.Enabled)
            {
                return;
            }

            //本地格式验证
            string user = username.Text;
            string pass = userpassword.Text;
            //string ip = "local";
            //return;
            if (user.Trim() == "" || pass.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("用户名或密码不能为空，请重新输入！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
                FRSE3.ShowDialog();

                return;
            }


            //显示等待
            Blogin.Enabled = false;
            PBload.Visible = true;
            username.Enabled = false;
            userpassword.Enabled = false;
            MFZC.Enabled = false;
            WJMM.Enabled = false;
            JZMM.Enabled = false;



            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["账号"] = user;
            InPutHT["密码"] = pass;
            InPutHT["IP地址"] = "";
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_Login);
            DataControl.RunThreadClassLogin RTCCL = new DataControl.RunThreadClassLogin(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
            trd.IsBackground = true;
            trd.Start();




        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_Login(Hashtable OutPutHT)
        {
            try
            {
                
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_Login_Invoke), new Hashtable[] { OutPutHT });
           
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_Login_Invoke(Hashtable OutPutHT)
        {
            
            //检查完毕后，恢复按钮
            Blogin.Enabled = true;
            PBload.Visible = false;
            username.Enabled = true;
            userpassword.Enabled = true;
            MFZC.Enabled = true;
            WJMM.Enabled = true;
            JZMM.Enabled = true;

            //演示用账号密码验证登陆后，显示主窗体并关闭登陆窗口
            DataSet dsuser = (DataSet)OutPutHT["登陆结果"]; //获取登陆结果
            PublicDS.PublisDsUser = dsuser; //告诉主窗体登陆结果

            //判断是否登陆成功并给出提示
            if (dsuser != null && dsuser.Tables.Count == 2 && dsuser.Tables[0].Rows.Count == 1 && dsuser.Tables["登录附加"].Rows[0]["是否登录成功"].ToString() == "成功")
            {
                //保存用户的配置信息
                DataControl.XMLConfig.SetConfig_ENC("用户本地配置", "UserEmail", this.username.Text.Trim());
                DataControl.XMLConfig.SetConfig_ENC("用户本地配置", "UserPassWord", this.userpassword.Text.Trim());
                DataControl.XMLConfig.SetConfig_ENC("用户本地配置", "IsSavePassWord", this.JZMM.Checked.ToString());
                DataControl.XMLConfig.SaveConfig();
                //登陆成功了
                FMP.Show();
                if (isShowAccountID)
                {
                    Hashtable hts = new Hashtable();
                    hts["dlyx"] = regID;
                    hts["dlmm"] = regPwd;
                    hts["yhm"] = regUname;
                    Formjhjxjszhzc fms = new Formjhjxjszhzc(hts);
                    fms.Show();
                }
                this.Hide();
                FMP.userloginOK();
            }
            else 
            {
                if (dsuser == null)
                {
                    //提示错误
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("系统错误，登陆失败！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
                    FRSE3.ShowDialog();
                }
                else
                {
                    //提示错误
                    string errmsage = dsuser.Tables["登录附加"].Rows[0]["错误信息1"].ToString();
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(errmsage);
                    if (dsuser.Tables["登录附加"].Rows[0]["错误信息2"].ToString().Trim() == "未激活")
                    {
                        FormAlertMessage frms3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
                        FormRegister2 reg2 = new FormRegister2(dsuser.Tables[0].Rows[0]["Number"].ToString(), dsuser.Tables[0].Rows[0]["dlyx"].ToString(), dsuser.Tables[0].Rows[0]["yhm"].ToString(), dsuser.Tables[0].Rows[0]["dlmm"].ToString(), dsuser.Tables[0].Rows[0]["YXYZM"].ToString(), this);
                        DateTime YZMGQSJ = Convert.ToDateTime(dsuser.Tables[0].Rows[0]["YZMGQSJ"]);//获得当前帐号验证码过期的时间
                        if (DateTime.Now > YZMGQSJ)
                        {
                            //如果现在的时间大于过期时间，说明验证码已经过期。则需要重新调用发送验证码邮件方法
                            ArrayList alTipsValNum = new ArrayList();
                            alTipsValNum.Add(" ");
                            alTipsValNum.Add("您的邮箱验证码已失效，已重新发送验证邮件，请重新输入验证码！");
                            FormAlertMessage fmVals = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", alTipsValNum);
                            fmVals.ShowDialog();
                            reg2.SendEmailAgain();
                            reg2.Show();
                            this.Hide();
                        }
                        else
                        {
                            ArrayList alTipsValNum = new ArrayList();
                            alTipsValNum.Add(" ");
                            alTipsValNum.Add("您的邮箱未验证，请重新输入验证码！");
                            FormAlertMessage fmVals = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", alTipsValNum);
                            fmVals.ShowDialog();
                            reg2.Show();
                            this.Hide();
                        }
                        //MessageBox.Show(YXMGQSJ.ToString());
                        //reg2.SendEmailAgain();
                        //reg2.Show();
                        //this.Hide();
                        return;
                    }





                    if (dsuser.Tables["登录附加"].Rows[0]["错误信息2"].ToString().Trim() == "休眠")
                    {
                        Almsg3.Add("您现在要解除休眠吗？");
                        FormAlertMessage msgbox = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", Almsg3);
                        DialogResult dr = msgbox.ShowDialog();
                        if (dr == DialogResult.Yes)
                        {
                            ArrayList minMoney = new ArrayList();
                            minMoney.Add("");
                            minMoney.Add("您确定缴纳200元人民币解除");
                            minMoney.Add("该账号的休眠状态吗？");
                            FormAlertMessage minusMoney = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", minMoney);
                            DialogResult drs = minusMoney.ShowDialog();
                            if (drs == DialogResult.Yes)
                            {
                                //这里填写扣钱代码和解除休眠UpdateSql（或调用解除休眠方法）
                                
                                minMoney.Clear();
                                minMoney.Add("");
                                minMoney.Add("扣款200RMB，解除休眠成功！");
                                FormAlertMessage okmsg = new FormAlertMessage("仅确定", "叹号", "中国商品批发交易平台", minMoney);
                                okmsg.ShowDialog();
                                FMP.Show();
                                this.Hide();
                                FMP.userloginOK();
                            }
                        }
                    }
                    else
                    {
                        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "中国商品批发交易平台", Almsg3);
                        FRSE3.ShowDialog();
                    }
                    //若是被休眠了，弹出休眠解除窗口
                    //string errmsageType = dsuser.Tables["登录附加"].Rows[0]["错误信息2"].ToString();
                    //if (errmsageType == "休眠")
                    //{
                    //    这里添加代码，弹出解除休眠的确认窗体;
                    //}

                }
            }
        }



        private void basicButton1_Click(object sender, EventArgs e)
        {
            //演示游客登陆后，显示主窗体并关闭登陆窗口
            FMP.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //演示游客登陆后，显示主窗体并关闭登陆窗口
            //FMP.Show();
            //this.Hide();
        }

        //记住密码
        private void JZMM_CheckedChanged(object sender, EventArgs e)
        {
            //string x = "607a5c5477e1dee526167f87468dcffa6bb9c3b8cf36e6e63f727fec72cf5369";
            //MessageBox.Show(Support.StringOP.uncMe(x,"mimamima"));
            //string y = "82042108@qq.com";
            //MessageBox.Show(Support.StringOP.encMe(y, "mimamima"));

        }

        //忘记密码
        private void WJMM_Click(object sender, EventArgs e)
        {
            FormResetPwd fmreset = new FormResetPwd();
            fmreset.Show();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MFZC_Click(object sender, EventArgs e)
        {
            FormRegister fm = new FormRegister(this);
            fm.Show();
            this.Hide();
        }

        public bool isShowAccountID = false;//是否显示开通结算帐户
        string regID;
        string regPwd;
        string regUname;
        /// <summary>
        /// 自动登录的实现
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        public void AutoLogin(string uid,string pwd,string uname,bool isShow)
        {
            isShowAccountID = isShow;//是否显示开通结算帐户
            regID = uid;
            regPwd = pwd;
            regUname = uname;
            this.username.Text = uid;
            this.userpassword.Text = pwd;
            //本地格式验证
            string user = uid;
            string pass = pwd;

            //显示等待
            Blogin.Enabled = false;
            PBload.Visible = true;
            username.Enabled = false;
            userpassword.Enabled = false;
            MFZC.Enabled = false;
            WJMM.Enabled = false;
            JZMM.Enabled = false;

            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["账号"] = user;
            InPutHT["密码"] = pass;
            InPutHT["IP地址"] = "";
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_Login);
            DataControl.RunThreadClassLogin RTCCL = new DataControl.RunThreadClassLogin(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCCL.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }


        //登陆动画
        int weizhiX = -100;
        System.Drawing.Image image_init = System.Drawing.Image.FromFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\loginbg.jpg");
        System.Drawing.Image image_yun = System.Drawing.Image.FromFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\yun002.png");
        private void timer1_Tick(object sender, EventArgs e)
        {
            System.Drawing.Image image = (Image)image_init.Clone();
            System.Drawing.Image copyImage = image_yun;
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(copyImage, new Rectangle(weizhiX, -50, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
            g.Dispose();
            pictureBox1.Image = image;
            weizhiX++;
            if (weizhiX >= image_init.Width)
            {
                weizhiX = -330;
            }
        }
    }
}
