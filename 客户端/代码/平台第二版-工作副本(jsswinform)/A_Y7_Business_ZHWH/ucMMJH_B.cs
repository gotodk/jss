using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.NewDataControl;
using 客户端主程序.DataControl;
using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH
{
    public partial class ucMMJH_B : UserControl
    {
        public ucMMJH_B()
        {
            InitializeComponent();

            Hashtable htin = new Hashtable();
            htin["dlyx"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
            SRT_Thread_Run(htin);
        }
        //开启一个用于获取最新基础验证信息的线程
        private void SRT_Thread_Run(Hashtable InPutHT)
        {
            RunThreadClassValidation vth = new RunThreadClassValidation(InPutHT, new delegateForThread(SRT_Thread));

            Thread trd = new Thread(new ThreadStart(vth.BeginRun));
            trd.IsBackground = true;
            trd.Start();

        }
        private void SRT_Thread(Hashtable OutPutHT)
        {
            try
            {
                Invoke(new delegateForThreadShow(SRT_Thread_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }

        private void SRT_Thread_Invoke(Hashtable OutPutHT)
        {
            DataSet ds = (DataSet)OutPutHT["返回值"];

            string sfxm = ds.Tables["状态值单条"].Rows[0]["是否休眠"].ToString();
            string sfdj = ds.Tables["状态值单条"].Rows[0]["是否冻结"].ToString();
            string djgnx = ds.Tables["状态值单条"].Rows[0]["冻结功能项"].ToString();
            string sfktjyzh = ds.Tables["状态值单条"].Rows[0]["是否开通交易账户"].ToString();

            if (sfxm == "否" && sfdj == "否")
            {
                panelTJQ.Enabled = true;
                panZQZJMM.Enabled = true;
            }

            else
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                if (sfxm == "是")
                { Almsg3.Add("您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！"); }
                if (sfdj == "是")
                {
                    Almsg3.Add("您的交易账户处于冻结状态，请与平台服务人员联系！");
                    Almsg3.Add("被冻结功能：" + djgnx);
                }
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
                panelTJQ.Enabled = false;
                panZQZJMM.Enabled = false;

            }

            string shzt = ds.Tables["状态值单条"].Rows[0]["交易账户审核状态"].ToString(); ;
            if (sfktjyzh == "否")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");

                if (shzt == "未申请")
                {
                    Almsg3.Add("您尚未提交开通交易账户申请，请及时提交！");
                }
                else if (shzt == "未通过")
                {
                    Almsg3.Add("您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！");
                }
                else if (shzt == "审核中")
                {
                    Almsg3.Add("您的开户申请正在审核中，请耐心等待！");
                }


                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
                panelTJQ.Enabled = false;
                panZQZJMM.Enabled = false;
            }

        }       

        /// <summary>
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //文本框验证
            if (txtyma.Text.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("原密码不可为空值！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtxma.Text.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("新密码不可为空值！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtxmaqr.Text.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("确认新密码不可为空值！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtyma.Text.Trim() != PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录密码"].ToString().Trim())
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("原密码输入错误！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtxma.Text.Length < 6)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("新密码不可少于6位！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtxma.Text.Trim() != txtxmaqr.Text.Trim())
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("系统检测到两次输入的新密码不一致，请重新输入！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtyma.Text.Trim() == txtxma.Text.Trim())
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("输入的新密码和原密码相同，未做更改！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();

            }

            else
            {
                //禁用提交区域并开启进度
                panelTJQ.Enabled = false;
                PBload.Visible = true;

                //演示新标准
                Hashtable htab = new Hashtable();
                htab["yma"] = txtyma.Text.Trim();
                htab["xma"] = txtxma.Text.Trim();
                htab["xmaqr"] = txtxmaqr.Text.Trim();
                htab["dlyx"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();


                SRT_demo_Run(htab);

            }

        }

        //开启一个测试线程
        private void SRT_demo_Run(Hashtable InPutHT)
        {
            OPenThreadPsdChange OTD = new OPenThreadPsdChange(InPutHT, new delegateForThread(SRT_demo));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_demo_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panelTJQ.Enabled = true;
            PBload.Visible = false;
            UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录密码"] = txtxma.Text.Trim();
                    reset();
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
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            this.Dock = DockStyle.Fill;//铺满
            this.AutoScroll = true;//出现滚动条
            this.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(this); //加入到某一个panel中
            this.Show();//显示出来
            //将滚动条置顶
            UPUP();
            txtyma.Text = "";
            txtxma.Text = "";
            txtxmaqr.Text = "";
        }



        /// <summary>
        /// 页面滚动条强制置顶
        /// </summary>
        private void UPUP()
        {
            TextBox tb = new TextBox();
            this.Controls.Add(tb);
            tb.Location = new Point(-5000, -5000);
            tb.TabIndex = 0;
            tb.Focus();
            //this.Controls.Remove(tb);
        }

        private void ucSPMR_B_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y);
            pictureBox1.Location = new Point(btnZQZJ.Location.X + btnZQZJ.Width + 25, this.panZQZJMM.Location.Y + btnZQZJ.Location.Y);
        }

        private void txtyma_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 交易资金密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZQZJ_Click(object sender, EventArgs e)
        {
            //文本框验证
            if (txtYZQZJMM.Text.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("原交易资金密码不能为空！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtXZQZJMM.Text.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("新交易资金密码不能为空！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (this.txtQRXZQZJMM.Text.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("确认新交易资金密码不能为空！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtYZQZJMM.Text.Trim() != PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户密码"].ToString().Trim())
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("原交易资金密码输入错误！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (!Support.ValStr.isZQZJMM(txtXZQZJMM.Text.Trim()))
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("新交易资金密码必须是6位数字！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtXZQZJMM.Text.Trim() != txtQRXZQZJMM.Text.Trim())
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("系统检测到两次输入的新交易资金密码不一致，请重新输入！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (txtYZQZJMM.Text.Trim() == txtXZQZJMM.Text.Trim())
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("输入的新交易资金密码和原交易资金密码相同，未做更改！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();

            }

            else
            {

                //禁用提交区域并开启进度
                panZQZJMM.Enabled = false;
                pictureBox1.Visible = true;

                //演示新标准
                Hashtable htab = new Hashtable();
                htab["原证券资金密码"] = txtYZQZJMM.Text.Trim();
                htab["新证券资金密码"] = txtXZQZJMM.Text.Trim();
                htab["确认新证券资金密码密码"] = txtQRXZQZJMM.Text.Trim();
                htab["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();


                SRT_ZQZJMM_Run(htab);

            }
        }


        //开启一个测试线程
        private void SRT_ZQZJMM_Run(Hashtable InPutHT)
        {
            OPenThreadPsdChange OTD = new OPenThreadPsdChange(InPutHT, new delegateForThread(SRT_ZQZJMM));
            Thread trd = new Thread(new ThreadStart(OTD.RunZQZJMMChange));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_ZQZJMM(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_ZQZJMM_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_ZQZJMM_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panZQZJMM.Enabled = true;
            pictureBox1.Visible = false;
            UPUP();


            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户密码"] = txtXZQZJMM.Text.Trim();
                    reset();
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
        //验证密码
        private void ValUPwd()
        {
            string pwd = this.txtxma.Text;
            if (pwd.Trim().Length < 6)
            {
                this.TipsErrorUPwd.Visible = true;
                this.TipsErrorUPwd.Text = "密码长度限制在6—16位之间";
            }
            else if (!Support.ValStr.isPTPwd(pwd) && !(pwd.Trim().Length < 6))
            {
                this.TipsErrorUPwd.Visible = true;
                this.TipsErrorUPwd.Text = "请勿使用数字、字母以外的特殊字符";
            }
            else if (pwd.Trim().Length >= 6 && pwd.Trim().Length <= 8)
            {
                if (Regex.IsMatch(pwd, @"^[a-z]{6,8}$") || Regex.IsMatch(pwd, @"^[0-9]{6,8}$") || Regex.IsMatch(pwd, @"^[A-Z]{6,8}$"))
                {
                    this.TipsErrorUPwd.Visible = true;
                    this.TipsErrorUPwd.Text = "密码强度：弱";
                }
                else if (Regex.IsMatch(pwd, @"^[a-zA-Z]{6,8}$") || Regex.IsMatch(pwd, @"^[a-z0-9]{6,8}$") || Regex.IsMatch(pwd, @"^[A-Z0-9]{6,8}$") || Regex.IsMatch(pwd, @"^[a-zA-Z0-9]{6,8}$"))
                {
                    this.TipsErrorUPwd.Visible = true;
                    this.TipsErrorUPwd.Text = "密码强度：中";
                }
            }
            else if (pwd.Trim().Length >= 9)
            {
                if (Regex.IsMatch(pwd, @"^[a-z]{9,16}$") || Regex.IsMatch(pwd, @"^[A-Z]{9,16}$") || Regex.IsMatch(pwd, @"^[0-9]{9,16}$"))
                {
                    this.TipsErrorUPwd.Visible = true;
                    this.TipsErrorUPwd.Text = "密码强度：中";
                }
                else if (Regex.IsMatch(pwd, @"^[a-zA-Z]{9,16}$") || Regex.IsMatch(pwd, @"^[a-z0-9]{9,16}$") || Regex.IsMatch(pwd, @"^[A-Z0-9]{9,16}$") || Regex.IsMatch(pwd, @"^[a-zA-Z0-9]{9,16}$"))
                {
                    this.TipsErrorUPwd.Visible = true;
                    this.TipsErrorUPwd.Text = "密码强度：强";
                }
            }
            else
                this.TipsErrorUPwd.Visible = false;
        }
        //验证密码是否一致
        private void ValUPwdAgain()
        {
            string pwd = this.txtxma.Text;
            string pwdRe = this.txtxmaqr.Text;
            if (!(pwd == pwdRe))
            {
                this.TipsErrorPwdAgain.Visible = true;
                this.TipsErrorPwdAgain.Text = "两次密码输入不一致";
            }
            else
                this.TipsErrorPwdAgain.Visible = false;
        }
        private void txtxma_TextChanged(object sender, EventArgs e)
        {
            
            if (txtxma.Text.Trim() != "")
            { 
                txtxma.PasswordChar = '*';
                ValUPwd();
                ValUPwdAgain();
                TipsErrorUPwd.Visible = true;
            }
            else
            {
                ValUPwdAgain();
                txtxma.PasswordChar = '\0';
                TipsErrorUPwd.Visible = false;
            }
        }

        private void txtXZQZJMM_TextChanged(object sender, EventArgs e)
        {
            if (txtXZQZJMM.Text.Trim() != "")
            { txtXZQZJMM.PasswordChar = '*'; }
            else
            {
                txtXZQZJMM.PasswordChar = '\0';
            }
        }

        private void txtxmaqr_TextChanged(object sender, EventArgs e)
        {
           
            if (txtxmaqr.Text.Trim() != "")
            {             
             
                ValUPwdAgain();

            }
            else
            {

                TipsErrorPwdAgain.Visible = false;
            }
        }

        private void txtXZQZJMM_Enter(object sender, EventArgs e)
        {
            txtXZQZJMM.OpenZS = true;
        }

        private void txtXZQZJMM_Leave(object sender, EventArgs e)
        {
            txtXZQZJMM.OpenZS = false;
            if (txtXZQZJMM.Text.Trim() == "")
            {
                txtXZQZJMM.TextNtip = "6位数字";
            }
        }



    }
}
