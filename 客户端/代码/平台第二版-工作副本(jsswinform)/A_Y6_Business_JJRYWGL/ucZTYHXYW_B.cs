using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using System.Threading;
using 客户端主程序.Support;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.JJRYWGL
{
    public partial class ucZTYHXYW_B : UserControl
    {
        Hashtable hashTableInfor;
        Hashtable htUser;
        public ucZTYHXYW_B()
        {
            InitializeComponent();
            this.panelTJQ.Enabled = false;
            this.panel1.Enabled = false;
        }
        /// <summary>
        ///暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("只有经纪人交易账户可以执行此操作！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                return;
            }
            hashTableInfor = new Hashtable();
            hashTableInfor["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            hashTableInfor["买卖家登录邮箱"] = "";
            hashTableInfor["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            hashTableInfor["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            hashTableInfor["操作类型"] = "暂停";
            hashTableInfor["理由"] = "";

            int tag = 0;
            if (String.IsNullOrEmpty(this.txtJYFZH.Text.Trim()))
            {
                this.labRemJYFZH.Text = "请填写交易方账号！";
                this.labRemJYFZH.Visible = true;
                tag += 1;
            }
            else
            {
                if (String.IsNullOrEmpty(this.txtJYFMC.Text.Trim()))
                {
                    this.labRemJYFZH.Text = "请填写可用的交易方账号！";
                    this.labRemJYFZH.Visible = true;
                    tag += 1;
                }
                else
                {
                    hashTableInfor["买卖家登录邮箱"] = this.txtJYFZH.Text.Trim();
                    this.labRemJYFZH.Visible = false;
                }
            }
            if (String.IsNullOrEmpty(this.txtZTHFLY.Text.Trim()))
            {
                this.labRemZTHFLY.Visible = true;
                tag += 1;
            }
            else
            {
                hashTableInfor["理由"] = this.txtZTHFLY.Text.Trim();
                this.labRemZTHFLY.Visible = false;
            }
            if (tag == 0)
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("您有权对其账户下的不良交易方暂停业务，暂停后该交易方将不能进行业务操作。");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("确定取消", "问号", "", Almsg4);
                DialogResult dialogResult = FRSE4.ShowDialog();
                if (dialogResult == DialogResult.Yes)
                {
                //禁用提交区域并开启进度
                panelTJQ.Enabled = false;
                panel1.Enabled = false;
                PBload.Visible = true;
                //演示新标准
                SRT_SetZTHFYHXYW_Run();
            }
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("只有经纪人交易账户可以执行此操作！");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
                return;
            }
            hashTableInfor = new Hashtable();
            hashTableInfor["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            hashTableInfor["买卖家登录邮箱"] = "";
            hashTableInfor["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            hashTableInfor["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            hashTableInfor["操作类型"] = "恢复";
            hashTableInfor["理由"] = "";
            int tag = 0;
            if (String.IsNullOrEmpty(this.txtJYFZH.Text.Trim()))
            {
                this.labRemJYFZH.Text = "请填写交易方账号！";
                this.labRemJYFZH.Visible = true;
                tag += 1;
            }
            else
            {
                if (String.IsNullOrEmpty(this.txtJYFMC.Text.Trim()))
                {
                    this.labRemJYFZH.Text = "请填写可用的交易方账号！";
                    this.labRemJYFZH.Visible = true;
                    tag += 1;
                }
                else
                {
                    hashTableInfor["买卖家登录邮箱"] = this.txtJYFZH.Text.Trim();
                    this.labRemJYFZH.Visible = false;
                }
            }
            if (String.IsNullOrEmpty(this.txtZTHFLY.Text.Trim()))
            {
                this.labRemZTHFLY.Visible = true;
                tag += 1;
            }
            else
            {
                hashTableInfor["理由"] = this.txtZTHFLY.Text.Trim();
                this.labRemZTHFLY.Visible = false;
            }
            if (tag == 0)
            {
                //禁用提交区域并开启进度
                panelTJQ.Enabled = false;
                panel1.Enabled = false;
                PBload.Visible = true;

                //演示新标准
                SRT_SetZTHFYHXYW_Run();
            }
        }

        //开启一个测试线程
        private void SRT_SetZTHFYHXYW_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTableInfor, new delegateForThread(SRT_SetZTHFYHXYW));
            Thread trd = new Thread(new ThreadStart(OTD.SetZTHFYHXYW));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_SetZTHFYHXYW(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_SetZTHFYHXYW_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_SetZTHFYHXYW_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panelTJQ.Enabled = true;
            panel1.Enabled = true;
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

                    break;
            }
        }
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {

            ucZTYHXYW_B uc = new ucZTYHXYW_B();

            uc.Dock = DockStyle.Fill;//铺满
            uc.AutoScroll = true;//出现滚动条
            uc.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(uc); //加入到某一个panel中
            uc.Show();//显示出来
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
      
        private void ucZTYHXYW_B_Load(object sender, EventArgs e)
        {
            this.panelTJQ.Enabled = true;
            this.panel1.Enabled = false;
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y + panelTJQ.Height);
            //非经纪人交易账户 此处的两个按钮不可用
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() != "经纪人交易账户")
            {
                this.panelTJQ.Enabled = false;
                this.panel1.Enabled = false;
            }
            else
            {
                htUser = new Hashtable();
                htUser["登录邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                htUser["经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
                htUser["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                SRT_GetJYZHDQZT_Run();
            }

        }

        #region//查询交易账户的当前状态  
        //开启一个测试线程
        private void SRT_GetJYZHDQZT_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(htUser, new delegateForThread(SRT_GetJYZHDQZT));
            Thread trd = new Thread(new ThreadStart(OTD.GetJYZHDQZT));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetJYZHDQZT(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetJYZHDQZT_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetJYZHDQZT_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "okJudge":
                    //给出表单提交成功的提示
                    if (dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() == "未开通交易账户")
                    {
                        this.panelTJQ.Enabled = true;
                        this.panel1.Enabled = false;

                    }
                    else if (dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"].ToString() == "已开通交易账户")
                    {
                        this.panelTJQ.Enabled = true;
                        this.panel1.Enabled = true;
                    }
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





        /// <summary>
        ///判断获取的交易账户是否有效
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJYFZH_KeyUp(object sender, KeyEventArgs e)
        {
            // ArrayList lv = new ArrayList();
            txtJYFMC.Text = "";

            if (!txtJYFZH.Text.Trim().Equals(""))
            {
                Hashtable hashTabeInfo = new Hashtable();
                hashTabeInfo["买卖家登录邮箱"] = txtJYFZH.Text.Trim();
                if (ValStr.ValidateQuery(hashTabeInfo))
                {
                    labRemJYFZH.Text = "请填写可用的交易方账号！";
                    labRemJYFZH.Visible = true;
                    return;
                }
                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                //InPutHT可以选择灵活使用
                hashTabeInfo["关联经纪人角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
                DataTable dataTable = StringOP.GetDataTableFormHashtable(hashTabeInfo);
                object[] objParams = { dataTable };
                //已移植可替换(已替换)
                //DataSet dsreturn = WSC2013.RunAtServices("GetMMJJYZHData", objParams);
                DataSet dsreturn = WSC2013.RunAtServices("C已审核买卖家信息", objParams);
                DataTable dt = dsreturn.Tables["买卖家数据信息"];
                if (dt != null && dt.Rows.Count > 0)
                {

                    DataRow dr = dt.Rows[0];
                    if (!dr["结算账户类型"].ToString().Trim().Equals("买家卖家交易账户"))
                    {

                        labRemJYFZH.Text = "请填写可用的交易方账号！";
                        labRemJYFZH.Visible = true;
                        return;
                    }
                    labRemJYFZH.Visible = false;
                    txtJYFMC.Text = dr["交易方名称"].ToString();
                  
                }
                else
                {
                    labRemJYFZH.Text = "请填写可用的交易方账号！";
                    labRemJYFZH.Visible = true;
                    return;
                }
            }
            else
            {
                labRemJYFZH.Text = "请填写交易方账号！";
                labRemJYFZH.Visible = true;
                return;
            }
           
        }

      






    }
}
