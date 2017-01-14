using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.NewDataControl;
using System.Collections;
using System.Threading;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH
{
    public partial class ucUpdateBankAccount_B : UserControl
    {
         
        public ucUpdateBankAccount_B()
        {
            InitializeComponent();

            #region 变更银行账户部分暂时隐藏

            this.label2.Visible = false;
            this.label4.Visible = false;
            this.label5.Visible = false;
            this.label6.Visible = false;
            this.comboBox1.Visible = false;
            this.uctextZQZJMM.Visible = false;
            this.uctxtPasword.Visible = false;
            this.btnSave.Visible = false;
            #endregion
           
           
        }

      
        /// <summary>
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtxmaqr.Text.ToString().Trim()=="")
            {
                ArrayList al = new ArrayList();
                al.Add("");
                al.Add("请输入新银行账号！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", al);
                FRSE3.ShowDialog();
                return;
            }

            ArrayList all = new ArrayList();
            all.Add("");
            all.Add("您确定要变更新的银行账号吗？");
            FormAlertMessage fm = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", all);
            
            if (fm.ShowDialog() == DialogResult.Yes)
            {
                //禁用提交区域并开启进度
                panelTJQ.Enabled = false;
                PBload.Visible = true;
                PBload.Location = new Point(this.panelTJQ.Location.X + this.basicButton1.Location.X + 10, this.panelTJQ.Location.Y + this.basicButton1.Location.Y);
                //演示新标准
                //演示新标准
                Hashtable htab = new Hashtable();               
                htab["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
                //wyh add 2014.04.21添加邮箱字段
                htab["用户邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                htab["bNewAcct"] = txtxmaqr.Text.ToString();//新银行账号
                SRT_demo_Run(htab, "C券商端发起-变更银行账户"); 
            }
         
        }

        //开启一个测试线程
        private void SRT_demo_Run(Hashtable InPutHT,string MethodNmae)
        {
            OpenThreadDataUpdate OTD = new OpenThreadDataUpdate(InPutHT, new delegateForThread(SRT_demo), MethodNmae);
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

                    reset();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                    if (showstr == "账号激活出现异常，请联系客服人员！")
                    {
                        this.btnSave.Enabled = false;
                    }

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

        private void ucSPMR_B_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            //PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y);      

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (uctxtPasword.Text.Trim()=="")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("原银行卡密码不能为空！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
                return;
            }
            if (uctextZQZJMM.Text.Trim() == "")
            {
                ArrayList Almsg3 = new ArrayList();
                Almsg3.Add("");
                Almsg3.Add("证券资金密码不能为空！");
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
                return;
            }

            ArrayList all = new ArrayList();
            all.Add("");
            all.Add("您确定要变更存管银行吗？");
            FormAlertMessage fm = new FormAlertMessage("确定取消", "问号", "中国商品批发交易平台", all);
            DialogResult boxresult = fm.ShowDialog();
            if (boxresult == DialogResult.Yes)
            {
                //禁用提交区域并开启进度
                panelTJQ.Enabled = false;
                PBload.Visible = true;
                PBload.Location = new Point(this.panelTJQ.Location.X + this.basicButton1.Location.X + 10, this.panelTJQ.Location.Y + this.basicButton1.Location.Y);
                //演示新标准
                //演示新标准
                Hashtable htab = new Hashtable();
                htab["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
                htab["新存管银行"] = comboBox1.SelectedText.ToString();//新银行账号
                htab["原银行卡密码"] = uctxtPasword.Text.Trim();
                htab["证券资金密码"] = uctextZQZJMM.Text.Trim();//证券资金密码
                SRT_demo_Run(htab, "SRequest_UpdateCGBank");
            }
        }

      

    }
}
