using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.NewDataControl;
using System.Threading;
using 客户端主程序.DataControl;
using System.Collections;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH
{
    public partial class ucKPXX_B : UserControl
    {
        public ucKPXX_B()
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
                flowLayoutPanel1.Enabled = true;
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
                flowLayoutPanel1.Enabled = false;

            }
                       
            string shzt = ds.Tables["状态值单条"].Rows[0]["交易账户审核状态"].ToString(); ;
            if(sfktjyzh=="否")
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
                else if(shzt== "审核中")
                {
                    Almsg3.Add("您的开户申请正在审核中，请耐心等待！");
                }
                              
               
                FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                FRSE3.ShowDialog();
                flowLayoutPanel1.Enabled = false;
                labelZT.Visible = false;
            }

        }

        private void ucKPXX_B_Load(object sender, EventArgs e)
        {
            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            PBload.Visible = true;
            SRT_GetBasicData_Run();
        }      


        #region//获取基本数据信息，并绑定到界面上
        //开启一个线程获取基础数据
        private void SRT_GetBasicData_Run()
        {
            Hashtable InputHT = new Hashtable();            
            InputHT["dlyx"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            InputHT["type"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            RunThreadClassKPXX OTD = new RunThreadClassKPXX(InputHT, new delegateForThread(SRT_GetBasicData));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRunGetInfo));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetBasicData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetBasicData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetBasicData_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            UPUP();
            PBload.Visible = false;
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //绑定界面用户数据
                    BindDataFaceData(dsreturn);
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
        /// 绑定界面用户数据
        /// </summary>
        private void BindDataFaceData(DataSet dsReturn)
        {
            DataTable dtkpxx = dsReturn.Tables["开票信息"].Copy();
            DataTable dtzcxx = dsReturn.Tables["注册信息"].Copy();
            //如果系统中已经存在开票信息的相关数据
            if (dtkpxx.Rows.Count > 0)
            {
                labelNumber.Text = dtkpxx.Rows[0]["number"].ToString();//开票信息的number  
                labelSHZT.Text = dtkpxx.Rows[0]["zt"].ToString();
                labelZCLB.Text = dtkpxx.Rows[0]["zclb"].ToString();//交易账号的注册类别
                labelID.Text = dtkpxx.Rows[0]["BGid"].ToString();//最后一条变更申请的id
                labelChangeZT.Text = dtkpxx.Rows[0]["clzt"].ToString();//变更申请的处理状态

                //发票邮递信息绑定
                uctbFPJSDWMC.Text = dtkpxx.Rows[0]["fpjsdwmc"].ToString();
                uctbFPJSDZ.Text = dtkpxx.Rows[0]["fpjsdz"].ToString();
                uctbFPJSLXR.Text = dtkpxx.Rows[0]["fpjslxr"].ToString();
                uctbFPJSLXRDH.Text = dtkpxx.Rows[0]["fpjslxrdh"].ToString();

                labelZCSCXX.Text = dtkpxx.Rows[0]["I_YBNSRZGZSMJ"].ToString().Trim();//注册时提交的一般纳税人资格证明

                #region 根据注册类别、发票类型，确定页面显示的内容
                if (dtkpxx.Rows[0]["zclb"].ToString() == "自然人")
                {
                    //自然人只有普通发票
                    rbPTFP.Checked = true;
                    panelDWMC.Visible = false;
                    panelZRRMC.Visible = true;
                    rbJYFMC.Text = dtkpxx.Rows[0]["I_JYFMC"].ToString();
                    if (dtkpxx.Rows[0]["dwmc"].ToString() == "个人")
                    {
                        rbGR.Checked = true;
                    }
                    else
                    {
                        rbJYFMC.Checked = true;
                    }
                    panelZYFPXX.Visible = false;
                }

                else if (dtkpxx.Rows[0]["ZCLB"].ToString() == "单位")
                {
                    //单位的分为专用发票和普通发票两种
                    panelDWMC.Visible = true;
                    uctbDWMC.Text = dtkpxx.Rows[0]["dwmc"].ToString();
                    panelZRRMC.Visible = false;


                    if (dtkpxx.Rows[0]["fplx"].ToString() == "增值税普通发票")
                    {
                        rbPTFP.Checked = true;
                        panelZYFPXX.Visible = false;
                        //给增值税专用发票的信息赋值,赋值的内容为开通交易账户的信息，用于后面的变更发票类型使用
                        uctbNSRSBH.Text = dtkpxx.Rows[0]["I_SWDJZSH"].ToString();
                        uctbDWDZ.Text = dtkpxx.Rows[0]["I_XXDZ"].ToString();
                        uctbLXDH.Text = dtkpxx.Rows[0]["I_JYFLXDH"].ToString();
                        uctbKHH.Text = dtkpxx.Rows[0]["I_KHYH"].ToString();
                        uctbKHZH.Text = dtkpxx.Rows[0]["I_YHZH"].ToString();
                        //一般纳税人资格证查看
                        if (dtkpxx.Rows[0]["I_YBNSRZGZSMJ"].ToString().Trim() != "")
                        {
                            ListView lvNSRZGZ = pan_SC_NSRZGZ.UpItem;
                            lvNSRZGZ.Items.Clear();
                            lvNSRZGZ.Items.Add(new ListViewItem(new string[] { "", dtkpxx.Rows[0]["I_YBNSRZGZSMJ"].ToString(), "", "", "" }));
                        }
                        panelZYFPXX.Visible = false;
                    }
                    else if (dtkpxx.Rows[0]["fplx"].ToString() == "增值税专用发票")
                    {
                        rbZYFP.Checked = true; 
                       
                        uctbNSRSBH.Text = dtkpxx.Rows[0]["ybnsrsbh"].ToString();
                        uctbDWDZ.Text = dtkpxx.Rows[0]["dwdz"].ToString();
                        uctbLXDH.Text = dtkpxx.Rows[0]["lxdh"].ToString();
                        uctbKHH.Text = dtkpxx.Rows[0]["khh"].ToString();
                        uctbKHZH.Text = dtkpxx.Rows[0]["khzh"].ToString();
                        //一般纳税人资格证查看
                        ListView lvNSRZGZ = pan_SC_NSRZGZ.UpItem;
                        lvNSRZGZ.Items.Clear();
                        lvNSRZGZ.Items.Add(new ListViewItem(new string[] { "", dtkpxx.Rows[0]["ybnsrzgz"].ToString(), "", "", "" }));
                        panelZYFPXX.Visible = true;
                    }
                }

                #endregion

                if (dtkpxx.Rows[0]["zt"].ToString() == "待审核")
                {
                    labelZT.Text = "您的开票信息尚未审核完毕，您可以进行修改！";
                    btnCommit.Visible = false;
                    btnEdit.Visible = true;
                    btnChange.Visible = false;
                    btnCommitChange.Visible = false;
                    btnReset.Enabled = true;
                }
                else if (dtkpxx.Rows[0]["zt"].ToString() == "驳回")
                {
                    labelZT.Text = "您的开票信息未审核通过，请修改后重新提交。未通过原因：" + dtkpxx.Rows[0]["shxx"].ToString();
                    btnCommit.Visible = false;
                    btnEdit.Visible = true;
                    btnChange.Visible = false;
                    btnCommitChange.Visible = false;
                    btnReset.Visible = true;
                }
                else if (dtkpxx.Rows[0]["zt"].ToString() == "已生效")
                {
                    labelZT.Text = "以下为当前有效的开票信息，请您核对！";
                    rbPTFP.Enabled = false;
                    rbZYFP.Enabled = false;
                    uctbDWMC.Enabled = false;
                    rbGR.Enabled = false;
                    rbZYFP.Enabled = false;
                    uctbNSRSBH.Enabled = false;
                    uctbDWDZ.Enabled = false;
                    uctbLXDH.Enabled = false;
                    uctbKHH.Enabled = false;
                    uctbKHZH.Enabled = false;
                    pan_SC_NSRZGZ.showB = new bool[] { false, true, false };
                    uctbFPJSDWMC.Enabled = false;
                    uctbFPJSDZ.Enabled = false;
                    uctbFPJSLXR.Enabled = false;
                    uctbFPJSLXRDH.Enabled = false;

                    //绑定开票信息变更申请部分的内容，只在存在有效开票信息时才可能存在变更申请
                    BindBGXX(dtkpxx);
                }
            }
            else//没有提交过任何发票信息的话，带出注册信息
            {
                labelNumber.Text = "";
                labelSHZT.Text = "";
                labelID.Text = "";
                labelChangeZT.Text = "";

                if (dtzcxx.Rows[0]["S_SFYBJJRSHTG"].ToString() == "否" || dtzcxx.Rows[0]["S_SFYBFGSSHTG"].ToString() == "否")
                {//如果分公司或者经纪人没有审核完成，则不允许提交开票信息
                    labelZT.Text = "您的交易账户尚未开通完成，暂时无法提交开票信息！";
                    panelDWMC.Visible = true;
                    panelZRRMC.Visible = false;
                    panelZYFPXX.Visible = false;
                    panelFPJSXX.Visible = true;
                    panelKPXXBG.Visible = false;
                    fpanelbtn.Visible = false;

                    panelFPLX.Enabled = false;
                    panelDWMC.Enabled = false;
                    panelFPJSXX.Enabled = false;
                }
                else
                {
                    labelZCLB.Text = dtzcxx.Rows[0]["I_ZCLB"].ToString();
                    labelZCSCXX.Text = dtzcxx.Rows[0]["I_YBNSRZGZSMJ"].ToString();
                    labelZT.Text = "请提交您的开票信息。";
               
                    if (dtzcxx.Rows[0]["I_ZCLB"].ToString() == "自然人")
                    {
                        rbPTFP.Checked = true;
                        rbJYFMC.Text = dtzcxx.Rows[0]["I_JYFMC"].ToString();
                        rbJYFMC.Checked = true;
                        panelDWMC.Visible = false;
                        panelZRRMC.Visible = true;                       
                        panelZYFPXX.Visible = false;
                        panelFPJSXX.Visible = true;                      
                    }
                    else if (dtzcxx.Rows[0]["I_ZCLB"].ToString() == "单位")
                    {
                        rbZYFP.Checked = true;
                        uctbDWMC.Text = dtzcxx.Rows[0]["I_JYFMC"].ToString();
                        panelDWMC.Visible = true;
                        panelZRRMC.Visible = false;
                        
                        uctbNSRSBH.Text = dtzcxx.Rows[0]["I_SWDJZSH"].ToString();
                        uctbDWDZ.Text = dtzcxx.Rows[0]["I_XXDZ"].ToString();
                        uctbLXDH.Text = dtzcxx.Rows[0]["I_JYFLXDH"].ToString();
                        uctbKHH.Text = dtzcxx.Rows[0]["I_KHYH"].ToString();
                        uctbKHZH.Text = dtzcxx.Rows[0]["I_YHZH"].ToString();                       
                        if (dtzcxx.Rows[0]["I_YBNSRZGZSMJ"].ToString() != "")
                        {
                            //一般纳税人资格证查看
                            ListView lvNSRZGZ = pan_SC_NSRZGZ.UpItem;
                            lvNSRZGZ.Items.Clear();
                            lvNSRZGZ.Items.Add(new ListViewItem(new string[] { "", dtzcxx.Rows[0]["I_YBNSRZGZSMJ"].ToString(), "", "", "" }));
                        }
                        panelZYFPXX.Visible = true;
                       
                    }                   

                    //发票邮递信息
                    uctbFPJSDWMC.Text = dtzcxx.Rows[0]["I_JYFMC"].ToString();
                    uctbFPJSDZ.Text = dtzcxx.Rows[0]["I_XXDZ"].ToString();
                    uctbFPJSLXR.Text = dtzcxx.Rows[0]["I_LXRXM"].ToString();
                    uctbFPJSLXRDH.Text = dtzcxx.Rows[0]["I_LXRSJH"].ToString();

                    panelKPXXBG.Visible = false;
                    panelBGNSZGZM.Visible = false;
                    btnCommit.Visible = true;
                    btnEdit.Visible = false;
                    btnChange.Visible = false;
                    btnCommitChange.Visible = false;
                    btnReset.Visible = true;
                }
            }
        }

        //绑定开票信息变更申请部分的内容
        private void BindBGXX(DataTable dtkpxx)
        {
            if (dtkpxx.Rows[0]["bgsqsmj"].ToString() != "")
            {
                panelKPXXBG.Visible = true;
                //变更申请扫描件查看
                ListView lvBGSQ = pan_SC_BG.UpItem;
                lvBGSQ.Items.Clear();
                lvBGSQ.Items.Add(new ListViewItem(new string[] { "", dtkpxx.Rows[0]["bgsqsmj"].ToString(), "", "", "" }));
                //一般纳税人资格证
                if (dtkpxx.Rows[0]["ybnsrzgzm"].ToString().Trim() != "")
                {
                    ListView lvYBNSRZGZMBG = pan_SC_YBNSRZGZBG.UpItem;
                    lvYBNSRZGZMBG.Items.Clear();
                    lvYBNSRZGZMBG.Items.Add(new ListViewItem(new string[] { "", dtkpxx.Rows[0]["ybnsrzgzm"].ToString(), "", "", "" }));
                }

                if (labelZCLB.Text == "自然人")
                {
                    panelBGNSZGZM.Visible = false;
                }
                else if (labelZCLB.Text == "单位")
                {
                    panelBGNSZGZM.Visible = true;
                }

                if (dtkpxx.Rows[0]["clzt"].ToString() == "待处理")
                {
                    labelBGZT.Text = "您的变更申请尚未审核完成，您可以进行修改！";
                    btnCommit.Visible = false;
                    btnEdit.Visible = false;
                    btnChange.Visible = false;  
                    btnCommitChange.Visible = true;
                    btnCommitChange.Texts = "提交修改";
                    btnReset.Visible = true;                   
                }
                else if (dtkpxx.Rows[0]["clzt"].ToString() == "驳回")
                {
                    labelBGZT.Text = "您的变更申请未审核通过，请修改后重新提交。未通过原因：" + dtkpxx.Rows[0]["slbz"].ToString() ;
                    btnCommit.Visible = false;
                    btnEdit.Visible = false;
                    btnChange.Visible = false;  
                    btnCommitChange.Visible = true;
                    btnCommitChange.Texts = "提交修改";
                    btnReset.Visible = true;

                }
                else if (dtkpxx.Rows[0]["clzt"].ToString() == "已修改")
                {
                    //如果是已处理完成的话，不显示变更申请。
                    labelID.Text = "";
                    labelBGZT.Text = "";
                    pan_SC_BG.UpItem.Clear();
                    pan_SC_YBNSRZGZBG.UpItem.Clear();
                    panelKPXXBG.Visible = false;
                    panelBGNSZGZM.Visible = false;
                    btnCommit.Visible = false;
                    btnEdit.Visible = false;
                    btnChange.Visible = true;
                    btnCommitChange.Visible = false;                   
                    btnReset.Visible = false;
                }               
            }
            else
            {
                //如果没有提交过变更申请的话，显示申请变更按钮
                btnCommit.Visible = false;
                btnEdit.Visible = false;
                btnChange.Visible = true;
                btnCommitChange.Visible = false;
                btnReset.Visible = false;
                panelKPXXBG.Visible = false;
                panelBGNSZGZM.Visible = false;
            }
        }
        #endregion      
        
        private void rbPTFP_CheckedChanged(object sender, EventArgs e)
        {
            ResetTX();//将所有文本框后面的提醒隐藏
            if (rbPTFP.Checked == true && labelZCLB.Text == "单位")
            {
                if (labelZCSCXX.Text.Trim ()!="")
                {
                    rbZYFP.Checked = true;
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add("您交易账户注册类别为单位，且已经上传了一般纳税人资格证明，请选择增值税专用发票。");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                }
                else
                {
                    panelZYFPXX.Visible = false;
                    panelDWMC.Visible = true;
                    panelZRRMC.Visible = false;
                }
            }
        }

        private void rbZYFP_CheckedChanged(object sender, EventArgs e)
        {
            ResetTX();//隐藏文本框后的提醒文字
            if (rbZYFP.Checked == true)
            {
                if (labelZCLB.Text == "自然人")
                {
                    rbPTFP.Checked = true;
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add("您交易账户注册类别为自然人，请选择增值税普通发票。");// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();
                }
                else
                {
                    panelDWMC.Visible = true;
                    panelZRRMC.Visible = false;
                    panelZYFPXX.Visible = true;
                }
            }
           
        }

        #region 提交/修改开票信息        
        private void btnCommit_Click(object sender, EventArgs e)
        {
            bool canCommit = true;
            Hashtable CommitHT = new Hashtable();          
            CommitHT["Number"] = labelNumber.Text;
            CommitHT["原状态"] = labelSHZT.Text;
            CommitHT["登陆邮箱"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            CommitHT["交易账户类型"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            CommitHT["注册类别"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["注册类别"].ToString();
            CommitHT["客户编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["证券资金账号"].ToString();
            CommitHT["平台管理机构"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["平台管理机构"].ToString();
            CommitHT["发票类型"] = "";
            CommitHT["单位名称"] = "";
            CommitHT["纳税人识别号"] = "";
            CommitHT["单位地址"] = "";
            CommitHT["联系电话"] = "";
            CommitHT["开户行"] = "";
            CommitHT["开户账号"] = "";
            CommitHT["一般纳税人资格证"] = "";
            CommitHT["发票接收单位名称"] = "";
            CommitHT["发票接收地址"] = "";
            CommitHT["发票接收联系人"] = "";
            CommitHT["发票接收联系人电话"] = "";
            if (rbPTFP.Checked == true)
            {
                CommitHT["发票类型"] = rbPTFP.Text;
                if (labelZCLB.Text.Trim() == "自然人")
                {
                    CommitHT["单位名称"] = rbGR.Checked == true ? rbGR.Text : rbJYFMC.Text;
                }
                else
                {
                    CommitHT["单位名称"] = uctbDWMC.Text.Trim();
                }
            }
            else if (rbZYFP.Checked == true)
            {
                CommitHT["发票类型"] = rbZYFP.Text;
                CommitHT["单位名称"] = uctbDWMC.Text.Trim ();
                if (uctbNSRSBH.Text.Trim() == "")
                {
                    label_NSRSBHTX.Visible = true;
                    canCommit = false;
                }
                else
                {
                    label_NSRSBHTX.Visible = false;
                    CommitHT["纳税人识别号"] = uctbNSRSBH.Text.Trim (); 
                }
                if (uctbDWDZ.Text.Trim() == "")
                {
                    label_DWDZTX.Visible = true;
                    canCommit = false;
                }
                else
                {
                    label_DWDZTX.Visible = false;
                    CommitHT["单位地址"] = uctbDWDZ.Text.Trim ();
                }
                if (uctbLXDH.Text.Trim() == "")
                {
                    label_LXDHTX.Visible = true;
                    canCommit = false;
                }
                else
                {
                    label_LXDHTX.Visible = false;
                    CommitHT["联系电话"] = uctbLXDH.Text.Trim ();
                }
                if (uctbKHH.Text.Trim() == "")
                {
                    label_KHHTX.Visible = true;
                    canCommit = false;
                }
                else
                {
                    label_KHHTX.Visible = false;
                    CommitHT["开户行"] = uctbKHH.Text.Trim ();
                }
                if (uctbKHZH.Text.Trim() == "")
                {
                    label_KHZHTX.Visible = true;
                    canCommit = false;
                }
                else
                {
                    label_KHZHTX.Visible = false;
                    CommitHT["开户账号"] = uctbKHZH.Text.Trim ();
                }
                if (pan_SC_NSRZGZ.UpItem == null || pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
                {
                    label_NSRZGZTX.Visible = true;
                    canCommit = false;
                }
                else
                {
                    label_NSRZGZTX.Visible = false;
                    CommitHT["一般纳税人资格证"] = pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                }
            }
            if (uctbFPJSDWMC.Text.Trim() == "")
            {
                label_FPJSDWMCTX.Visible = true;
                canCommit = false;
            }
            else 
            {
                label_FPJSDWMCTX.Visible = false;
                CommitHT["发票接收单位名称"] = uctbFPJSDWMC.Text.Trim();
            }
            if (uctbFPJSDZ.Text.Trim() == "")
            {
                label_FPJSDZTX.Visible = true;
                canCommit = false;
            }
            else
            {
                label_FPJSDZTX.Visible = false;
                CommitHT["发票接收地址"] = uctbFPJSDZ.Text.Trim();
            }
            if (uctbFPJSLXR.Text.Trim() == "")
            {
                label_FPJSLXRTX.Visible = true;
                canCommit = false;
            }
            else
            {
                label_FPJSLXRTX.Visible = false;
                CommitHT["发票接收联系人"] = uctbFPJSLXR.Text.Trim();
            }
            if (uctbFPJSLXRDH.Text.Trim() == "")
            {
                label_FPJSLXRDHTX.Visible = true;
                canCommit = false;
            }
            else
            {
                label_FPJSLXRDHTX.Visible = false;
                CommitHT["发票接收联系人电话"] = uctbFPJSLXRDH.Text.Trim();
            }

            if (canCommit == false)
            {
                ScrollToPoint();
                return;
            }
            else
            {
                //禁用提交区域并开启进度
                flowLayoutPanel1.Enabled = false;
                PBload.Visible = true;
                //提交账户信息
                SRT_CommitKPXX_Run(CommitHT);
            }
        }
              
        //开启一个测试线程
        private void SRT_CommitKPXX_Run(Hashtable InPutHT)
        {
            RunThreadClassKPXX OTD = new RunThreadClassKPXX(InPutHT, new delegateForThread(SRT_CommitKPXX));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRunCommit));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_CommitKPXX(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_CommitKPXX_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_CommitKPXX_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            flowLayoutPanel1.Enabled = true;
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
                    SRT_GetBasicData_Run();
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
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

        //申请变更按钮操作
        private void btnChange_Click(object sender, EventArgs e)
        {
            panelKPXXBG.Visible = true;
            labelBGZT.Text = "请上传变更信息扫描件。单位需加盖公章，自然人需本人签字。";
            btnCommit.Visible = false;
            btnEdit.Visible = false;
            btnChange.Visible = false;
            btnCommitChange.Visible = true;
            btnCommitChange.Texts = "提交变更";
            btnReset.Visible = true;
            label_BGTX.Visible = false;
            TipsSFZZFM.Visible = true;
            if (labelZCLB.Text == "自然人")
            {
                panelBGNSZGZM.Visible = false;
            }
            else
            {
                panelBGNSZGZM.Visible = true;
            }

            //这是滚动条显示的位置
            Point scrollPoint = new Point();
            scrollPoint.X = this.panelKPXXBG.Location.X;
            scrollPoint.Y = this.panelKPXXBG.Location.Y-30;
            this.AutoScrollPosition = scrollPoint;
        }

        #region 提交、修改变更信息
        private void btnCommitChange_Click(object sender, EventArgs e)
        {
            bool CanCommit = true;
            Hashtable ht = new Hashtable();
            ht["Number"] = labelNumber.Text.Trim();
            ht["ID"] = labelID.Text.Trim();
            ht["原状态"] = labelChangeZT.Text.Trim();            
            ht["变更信息扫描件"] = "";
            ht["一般纳税人资格证明"] = "";
            if (pan_SC_BG.UpItem == null || pan_SC_BG.UpItem.Items.Count <= 0)
            {
                label_BGTX.Visible = true;
                TipsSFZZFM.Visible = false;
                CanCommit = false;
            }
            else
            {
                ht["变更信息扫描件"] = pan_SC_BG.UpItem.Items[0].SubItems[1].Text.Trim();
                CanCommit = true;
            }
            if (pan_SC_YBNSRZGZBG.UpItem != null && pan_SC_YBNSRZGZBG.UpItem.Items.Count > 0)
            {
                ht["一般纳税人资格证明"] = pan_SC_YBNSRZGZBG.UpItem.Items[0].SubItems[1].Text.Trim();
            }
            if (CanCommit == false)
            {
                ScrollToPoint();
                return;
            }
            else
            {
                panelKPXXBG.Enabled = false;
                PBload.Visible = true;
              
                SRT_CommitKPXXChange_Run(ht);
            }

        }
        //开启一个测试线程
        private void SRT_CommitKPXXChange_Run(Hashtable InPutHT)
        {
            RunThreadClassKPXX OTD = new RunThreadClassKPXX(InPutHT, new delegateForThread(SRT_CommitKPXXChange));
            Thread trd = new Thread(new ThreadStart(OTD.BeginRunCommitKPXXChange));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_CommitKPXXChange(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_CommitKPXXChange_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_CommitKPXXChange_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            panelKPXXBG.Enabled = true;
            PBload.Visible = false;
            //UPUP();

            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];
            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    SRT_GetBasicData_Run();
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    label_BGTX.Visible = false;
                    TipsSFZZFM.Visible = true;
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

        //重置按钮操作
        private void btnReset_Click(object sender, EventArgs e)
        {            
            //将上传控件先清空
            pan_SC_NSRZGZ.UpItem.Clear();
            pan_SC_BG.UpItem.Clear();
            pan_SC_YBNSRZGZBG.UpItem.Clear();

            ResetTX();//隐藏所有文本框后面的提醒文字           

            //重新绑定一遍页面内容
            SRT_GetBasicData_Run();            
        }

        //隐藏所有没有填写的提醒文字
        private void ResetTX()
        {
            label_NSRSBHTX.Visible = false;
            label_DWDZTX.Visible = false;
            label_LXDHTX.Visible = false;
            label_KHHTX.Visible = false;
            label_KHZHTX.Visible = false;
            label_NSRZGZTX.Visible = false;
            label_FPJSDWMCTX.Visible = false;
            label_FPJSDZTX.Visible = false;
            label_FPJSLXRTX.Visible = false;
            label_FPJSLXRDHTX.Visible = false;
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

        /// <summary>
        /// 根据提示信息，自动定位到首个提示的位置
        /// </summary>
        private void ScrollToPoint()
        {
            Point scrollPoint = new Point();
            if (this.label_NSRSBHTX.Visible == true)
            {
                scrollPoint.X = this.label_NSRSBHTX.Parent.Location.X;
                scrollPoint.Y = this.label_NSRSBHTX.Parent.Location.Y + 30;
            }
            else if (this.label_DWDZTX.Visible == true)
            {
                scrollPoint.X = this.label_DWDZTX.Parent.Location.X;
                scrollPoint.Y = this.label_DWDZTX.Parent.Location.Y + 60;
            }
            else if (this.label_LXDHTX.Visible == true)
            {
                scrollPoint.X = this.label_LXDHTX.Parent.Location.X;
                scrollPoint.Y = this.label_LXDHTX.Parent.Location.Y +90;
            }
            else if (this.label_KHHTX.Visible == true)
            {
                scrollPoint.X = this.label_KHHTX.Parent.Location.X;
                scrollPoint.Y = this.label_KHHTX.Parent.Location.Y + 120;
            }
            else if (this.label_KHZHTX.Visible == true)
            {
                scrollPoint.X = this.label_KHZHTX.Parent.Location.X;
                scrollPoint.Y = this.label_KHZHTX.Parent.Location.Y + 150;
            }
            else if (this.label_NSRZGZTX.Visible == true)
            {
                scrollPoint.X = this.label_NSRZGZTX.Parent.Location.X;
                scrollPoint.Y = this.label_NSRZGZTX.Parent.Location.Y + 180;
            }
            else if (this.label_FPJSDWMCTX.Visible == true)
            {
                scrollPoint.X = this.label_FPJSDWMCTX.Parent.Location.X;
                scrollPoint.Y = this.label_FPJSDWMCTX.Parent.Location.Y + 30;
            }
            else if (this.label_FPJSDZTX.Visible == true)
            {
                scrollPoint.X = this.label_FPJSDZTX.Parent.Location.X;
                scrollPoint.Y = this.label_FPJSDZTX.Parent.Location.Y +60;
            }
            else if (this.label_FPJSLXRTX.Visible == true)
            {
                scrollPoint.X = this.label_FPJSLXRTX.Parent.Location.X;
                scrollPoint.Y = this.label_FPJSLXRTX.Parent.Location.Y +90;
            }
            else if (this.label_FPJSLXRDHTX.Visible == true)
            {
                scrollPoint.X = this.label_FPJSLXRDHTX.Parent.Location.X;
                scrollPoint.Y = this.label_FPJSLXRDHTX.Parent.Location.Y +120;
            }
            else if (this.label_BGTX.Visible == true)
            {
                scrollPoint.X = this.label_BGTX.Parent.Location.X;
                scrollPoint.Y = this.label_BGTX.Parent.Location.Y + 50;
            }

            this.AutoScrollPosition = scrollPoint;

        }
        //重新设置按钮的间距 edit by zzf 2014.9.10 
        private void fpanelbtn_Paint(object sender, PaintEventArgs e)
        {
            int d = 0;//用来判定是第几个可见按钮
            for (int i = 0; i < fpanelbtn.Controls.Count; i++)
			{
                if (fpanelbtn.Controls[i].GetType() == typeof(Com.Seezt.Skins.BasicButton))
                {                    
                    if (fpanelbtn.Controls[i].Visible == true)
                    {
                        d++;//第几个可见按钮
                        Com.Seezt.Skins.BasicButton b = (Com.Seezt.Skins.BasicButton)fpanelbtn.Controls[i];
                        //如果不是第一个可见的按钮，设置左边的间距为20像素
                        if (d != 1)
                        { b.Margin = new Padding(20, 3, 3, 3); }
                    }
                }			 
			}      
           
        }        
    }
}
