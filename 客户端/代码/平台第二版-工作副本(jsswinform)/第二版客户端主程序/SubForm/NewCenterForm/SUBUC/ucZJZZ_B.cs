using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.NewDataControl;
using 客户端主程序.DataControl;
using System.Threading;
using System.Collections;
using 客户端主程序.SubForm.NewCenterForm.GZZD;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC
{
    public partial class ucZJZZ_B : UserControl
    {
        public ucZJZZ_B()
        {
            InitializeComponent();
        }
        
        private void ucZJZZ_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            ////设置进度条的位置，放到按钮旁边
            //PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 10, btnSave.Location.Y - 5);
            PBload.Location = new Point(this.panelTJQ.Location.X + this.panelBtn.Location.X + this.btnSave.Location.X + this.btnSave.Width + 10, this.panelTJQ.Location.Y + this.panelBtn.Location.Y);

            txtKHYH.Text = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["开户银行"].ToString().Trim();
            txtYHZH.Text = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["银行账号"].ToString().Trim();
            plKZJE.Visible = false;
        }

        //开启一个测试线程
        private void SRT_OnLoad_Run(Hashtable InPutHT)
        {
            OpenThreadDataUpdate OTD = new OpenThreadDataUpdate(InPutHT, new delegateForThread(SRT_OnLoad), "C最大可转金额");
            Thread trd = new Thread(new ThreadStart(OTD.BeginRun));

            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_OnLoad(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_OnLoad_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_OnLoad_Invoke(Hashtable OutPutHT)
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
                case "okerr":
                    lblDQZHZDKZJE.Text = showstr;
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
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            double zzje = 0;
            //if (cbxZZLB.SelectedItem == null || cbxZZLB.SelectedItem.ToString() == "")
            //{
            //    ArrayList Almsg3 = new ArrayList();
            //    Almsg3.Add("");
            //    Almsg3.Add("请选择转账类别！");
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
            //    FRSE3.ShowDialog();
            //}
            //验证
            if (txtJE.Text.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("请输入金额！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();

            }
            #region//模拟运行期间，暂时去掉这两个验证
            //else if (plMM.Visible == true && txtZJMM.Text.Trim() == "")
            //{
            //    ArrayList Almsg3 = new ArrayList();
            //    Almsg3.Add("");
            //    Almsg3.Add("请输入银行卡密码！");
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
            //    FRSE3.ShowDialog();
            //}
            //else if (uctextZQZJMM.Text.Trim() == "")
            //{
            //    ArrayList Almsg3 = new ArrayList();
            //    Almsg3.Add("");
            //    Almsg3.Add("请输入证券资金密码！");
            //    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
            //    FRSE3.ShowDialog();
            //}
            #endregion
            else if (double.TryParse(txtJE.Text.Trim(), out zzje) == false)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("录入的金额格式不正确！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
            }
            else if (zzje == 0)
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("转入或转出金额不可为零！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();

            }
            else
            {
                //禁用提交区域并开启进度
                panelTJQ.Enabled = false;
                PBload.Location = new Point(this.panelTJQ.Location.X + this.panelBtn.Location.X + this.btnSave.Location.X + this.btnSave.Width + 10, this.panelTJQ.Location.Y + this.panelBtn.Location.Y);
                PBload.Visible = true;
                //演示新标准
                Hashtable htab = new Hashtable();                         
                //htab["zzlb"] = rdBankToS.Checked == true ? rdBankToS.Text : rdSToBank.Text; //转账方式
                //htab["je"] = txtJE.Text.Trim();//转账金额
                //htab["zjmm"] = txtZJMM.Text.Trim();//银行密码
                //htab["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
                //htab["zqzjmm"] = uctextZQZJMM.Text.Trim();//证券资金密码
                ////测试账号
                //htab["yhzh"] = "9843010200420116";
                //htab["证券资金账号"] = "130626000977";
                htab["转账类别"] = rdBankToS.Checked == true ? rdBankToS.Text : rdSToBank.Text; //转账方式
                htab["金额"] = txtJE.Text.Trim();//转账金额
                htab["银行取款密码"] = txtZJMM.Text.Trim();//银行密码
                htab["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
                htab["交易资金密码"] = uctextZQZJMM.Text.Trim();//证券资金密码
                htab["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                htab["开户银行"] = txtKHYH.Text .Trim ();
                htab["银行账号"] = txtYHZH.Text.Trim();
                SRT_demo_Run(htab);

            }

        }

        //开启一个测试线程
        private void SRT_demo_Run(Hashtable InPutHT)
        {
            OpenThreadDataUpdate OTD = new OpenThreadDataUpdate(InPutHT, new delegateForThread(SRT_demo), "C资金转账");
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

                    //cbxZZLB.SelectedIndex = 0;
                    rdBankToS.Checked = true;
                    rdSToBank.Checked = false;
                    txtJE.Text = "";
                    txtZJMM.Text = "";
                    uctextZQZJMM.Text = "";
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //cbxZZLB.SelectedIndex = 0;
            rdBankToS.Checked = true;
            rdSToBank.Checked = false;
            txtJE.Text = "";
            txtZJMM.Text = "";
            uctextZQZJMM.Text = "";
            reset();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hashtable htcs = new Hashtable();
            htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/moban/XNBCSXZ.htm";
            XSRM xs = new XSRM(linkLabel1.Text.Trim(), htcs);
            xs.ShowDialog();
        }

        //银转商
        private void rdBankToS_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBankToS.Checked == true)//银转商
            {
                rdSToBank.Checked = false;
                plKZJE.Visible = false;//当前账户最大可转金额
                //plMM.Visible = true;//密码部分
            }
            else
            {
                //禁用提交区域并开启进度
               panelTJQ.Enabled = false;
               PBload.Location = new Point(this.panelTJQ.Location.X + this.panelBtn.Location.X + this.btnSave.Location.X + this.btnSave.Width + 10, this.panelTJQ.Location.Y + this.panelBtn.Location.Y);
                PBload.Visible = true;

                Hashtable htab = new Hashtable();
                ////测试账号
                //htab["yhzh"] = "9843010200420116";
                //htab["证券资金账号"] = "130626000977";
                htab["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();

                SRT_OnLoad_Run(htab);

                rdSToBank.Checked = true;
                plKZJE.Visible = true;//当前账户最大可转金额
                //plMM.Visible = false;//密码部分
                txtZJMM.Text = "";
            }
        }

    }
}
