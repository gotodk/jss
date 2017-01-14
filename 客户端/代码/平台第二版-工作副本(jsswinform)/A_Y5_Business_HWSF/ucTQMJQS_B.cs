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
using 客户端主程序.Support;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    public partial class ucTQMJQS_B : UserControl
    {
        public ucTQMJQS_B()
        {
            InitializeComponent();
        }
        /// <summary>
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {            
            #region//基本验证
            if (txtFHDBH.Text.Trim()=="")
            {
                showAlertY("发货单编号不能为空！");
                return;
            }
            if (txtWLGSMC.Text.Trim() == "")
            {
                showAlertY("物流公司名称不能为空！");
                return;
            }
            if (txtWLDH.Text.Trim() == "")
            {
                showAlertY("物流单号不能为空！");
                return;
            }
            if (uCshangchuan1.UpItem.Items.Count <= 0)
            {
                showAlertY("物流签收单不能为空！");
                return;
            }
            #endregion

            #region//基础数据
            string BUYjsbh = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            DataSet dsinfosave = new DataSet();
            Hashtable htcs = new Hashtable();
            htcs["卖家角色编号"] = BUYjsbh;
            htcs["结算账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            htcs["发货单号"] = txtFHDBH.Text.TrimStart('F');
            htcs["物流公司名称"] = txtWLGSMC.Text.Trim();
            htcs["物流单号"] = txtWLDH.Text.Trim();
            htcs["物流签收单"] = uCshangchuan1.UpItem.Items[0].SubItems[1].Text.ToString();
            #endregion
            DataTable dtcs = StringOP.GetDataTableFormHashtable(htcs);
            dsinfosave.Tables.Add(dtcs);


            //禁用提交区域并开启进度
            panelTJQ.Enabled = false;
            PBload.Visible = true;
            //演示新标准
            SRT_demo_Run(dsinfosave);
        }

        //开启一个测试线程
        private void SRT_demo_Run(DataSet InPutHT)
        {
            NewDataControl.SelTQBuyQS OTD = new SelTQBuyQS(InPutHT, new delegateForThread(SRT_demo));
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
            if (dsreturn != null && dsreturn.Tables["返回值单条"].Rows.Count>0)
            {
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
                    case "err":
                        ArrayList Almsg5 = new ArrayList();
                        Almsg5.Add("");
                        Almsg5.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                        FormAlertMessage FRSE5 = new FormAlertMessage("仅确定", "其他", "", Almsg5);
                        FRSE5.ShowDialog();
                        break;
                    default:
                        ArrayList Almsg4 = new ArrayList();
                        Almsg4.Add("");
                        Almsg4.Add("系统繁忙，请稍后重试...");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                        FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                        FRSE4.ShowDialog();

                        break;
                }
            }
            else
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("系统繁忙，请稍后重试...");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
            }

            
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
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "预订单信息提交", Almsg3);
            FRSE3.ShowDialog();
        }
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucTQMJQS_B aaa = new ucTQMJQS_B();
            aaa.Dock = DockStyle.Fill;//铺满
            aaa.AutoScroll = true;//出现滚动条
            aaa.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(aaa); //加入到某一个panel中
            aaa.Show();//显示出来
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
            
        private void ucTQMJQS_B_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Location = new Point(btnSave.Location.X + btnSave.Width + 25, btnSave.Location.Y + panelTJQ.Height);

            //给上传控件赋值
            //取数据
            ListView lv = uCshangchuan1.UpItem;
            //赋值(一定要先取出来)
            lv.Items.Clear();
            //lv.Items.Add(new ListViewItem(new string[] { "", "数据库记录的远程路径", "", "", "" }));

            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "经纪人交易账户")
            {
                showAlertY("抱歉，只有交易方交易账户才能执行此操作！");
                txtFHDBH.Enabled = false;
                txtWLGSMC.Enabled = false;
                txtWLDH.Enabled = false;
                uCshangchuan1.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }
            else
            {
                txtFHDBH.Enabled = true;
                txtWLGSMC.Enabled = true;
                txtWLDH.Enabled = true;
                uCshangchuan1.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            }
        }
        //重置
        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        /// <summary>
        /// 验证输入的发货单编号是否已发送过提醒信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFHDBH_TextChanged(object sender, EventArgs e)
        {

            string number = txtFHDBH.Text.TrimStart('F');
            SRT_TextChanged_Run(number);
        }

        //开启一个测试线程
        private void SRT_TextChanged_Run(string InPutHT)
        {
            NewDataControl.SelTQBuyQS OTD = new SelTQBuyQS(InPutHT, new delegateForThread(SRT_TextChanged));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRunIsHavedQS));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_TextChanged(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_TextChanged_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_TextChanged_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panelTJQ.Enabled = true;
            PBload.Visible = false;
            UPUP();
            //uCshangchuan1.showB = null;
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];
            if (dsreturn != null && dsreturn.Tables["返回值单条"].Rows.Count > 0)
            {
                string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();               
                //显示执行结果
                switch (zt)
                {
                    case "ok":
                        if (dsreturn.Tables["主表"] != null && dsreturn.Tables["主表"].Rows.Count>0)
                        {
                            if (!string.IsNullOrEmpty(dsreturn.Tables["主表"].Rows[0]["请买家签收确认操作时间"].ToString()))
                            {
                                lblTXMesg.Visible = true;
                                txtWLGSMC.Text = dsreturn.Tables["主表"].Rows[0]["物流公司名称"].ToString();
                                txtWLDH.Text = dsreturn.Tables["主表"].Rows[0]["物流单号"].ToString();

                                txtWLGSMC.Enabled = false;
                                txtWLDH.Enabled = false;

                                ListView lv = uCshangchuan1.UpItem;
                                //赋值(一定要先取出来)
                                lv.Items.Clear();
                                lv.Items.Add(new ListViewItem(new string[] { "", dsreturn.Tables["主表"].Rows[0]["物流签收单"].ToString(), "", "", "" }));
                                //uCshangchuan1.showB = new bool[] { false, true, false };
                                uCshangchuan1.Enabled = false;
                                btnSave.Enabled = false;
                                btnCancel.Enabled = false;
                            }
                            else
                            {
                                lblTXMesg.Visible = false;
                                txtWLGSMC.Text = "";
                                txtWLDH.Text = "";

                                txtWLGSMC.Enabled = true;
                                txtWLDH.Enabled = true;
                                uCshangchuan1.Enabled = true;
                              //  uCshangchuan1.showB = new bool[] { true, false, false };
                             
                                ListView lv = uCshangchuan1.UpItem;
                                //赋值(一定要先取出来)
                                lv.Items.Clear();

                                btnSave.Enabled = true;
                                btnCancel.Enabled = true;
                               
                            }
                        }

                     
                        break;
                    default:
                         lblTXMesg.Visible = false;
                                txtWLGSMC.Text = "";
                                txtWLDH.Text = "";

                                txtWLGSMC.Enabled = true;
                                txtWLDH.Enabled = true;
                             
                                ListView lv1 = uCshangchuan1.UpItem;
                                //赋值(一定要先取出来)
                                lv1.Items.Clear();
                                uCshangchuan1.Enabled = true;
                             //   uCshangchuan1.showB = new bool[] { true,false,false};
                                btnSave.Enabled = true;
                                btnCancel.Enabled = true;
                              
                        break;
                }
            }
            else
            {
                ArrayList Almsg4 = new ArrayList();
                Almsg4.Add("");
                Almsg4.Add("系统繁忙，请稍后重试...");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                FRSE4.ShowDialog();
            }
            txtFHDBH.Focus();

        }


    }
}
