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
using 客户端主程序.Support;
using 客户端主程序.SubForm.NewCenterForm.MuBan;
using System.Runtime.InteropServices;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.ZHWH
{
    public partial class ucZHZL_B : UserControl
    {
        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;   

        //用来协助判断，用户所属平台管理机构的变量 
        string strTagSF = ""; //身份
        string strTagDS = "";//地市
     
        public void SPopenDialog(FormKTJYZH FK, bool chongzhi, Hashtable hts)
        {
            if (chongzhi)
            {
                FK = new FormKTJYZH(hts);
                FK.ShowDialog();
                SPopenDialog(FK, FK.chongzhi, hts);
            }
        }


        Hashtable HTuser;
        Hashtable hashTableInfor = null;
        //得到当前审核的状态
        Hashtable hashTableVirify = new Hashtable();
        string strSFYMJZWC = "页面未加载完成";
        public ucZHZL_B()
        {
            InitializeComponent();
            //2013.12.06 wyh add 鼠标略过显示文字
            toolTip1.AutoPopDelay = 25000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 0;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.radFGS, "所属地服务中心，会为您提供及时服务！");
            toolTip1.SetToolTip(this.radYWTZB, "银行经纪人和富美集团总部员工请选择“平台总部” ");
            toolTip1.SetToolTip(this.radGXTW, "参加“全国大学生课余创业实践活动”的高校师生请选择“高校团委”"); 

          
        }
        /// <summary>
        /// 绑定数据信息
        /// </summary>
        public void BindData()
        {
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "")
            {
                this.panSZYX.Visible = false;
                this.panGLBMFL.Visible = false;
                this.panelTJQ.Enabled = false;
                this.flowLayoutPanel1.Enabled = false;
                this.linkZHKTXY.Enabled = false;
                return;
            }
            this.panelTJQ.Enabled = false;
            this.flowLayoutPanel1.Enabled = false;
            ucSSQY.initdefault();
            //this.txtJJRZGZS.CanPASTE = false;
            HTuser = new Hashtable();
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "经纪人交易账户")
            {
                HTuser["JSBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["经纪人角色编号"].ToString();
            }
            else
            {
                HTuser["JSBH"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["卖家角色编号"].ToString();
            }
            HTuser["登录_Number"] = "";
            HTuser["关联_Number"] = "";
            HTuser["原平台管理机构"] = "";//保存原来的平台管理机构
            HTuser["原经纪人资格证书编号"] = "";//保存原来的经纪人资格证书编号
            HTuser["原经纪人角色编号"] = "";//保存原来的经纪人角色编号
            HTuser["原买家角色编号"] = "";//保存原来的买家角色编号
            HTuser["原卖家角色编号"] = "";//保存原来的卖家角色编号
            HTuser["原注册类别"] = "";//用来保存上一次交易账户的类别
            HTuser["方法类别"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
            HTuser["DLYX"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
            HTuser["YHM"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["用户名"].ToString();
            HTuser["JYFMC"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["交易方名称"].ToString();
            HTuser["GLJJRBH"] = PublicDS.PublisDsUser.Tables["关联信息"].Rows[0]["关联经纪人编号"].ToString();
            //绑定平台管理机构
            BindPTGLJG();
            SRT_GetBasicData_Run();
        
        }


        #region//获取基本数据信息，并绑定到界面上

        /// <summary>
        /// 绑定界面用户数据
        /// </summary>
        private void BindDataFaceData(DataSet dsReturn)
        {
            string strSHZQ = "开通交易账户审核情况：";

            #region//获取用户数据信息
            DataTable dataTableBasic=new DataTable();
             DataTable dataTableMMJ_JJR=new DataTable();
             if (HTuser["方法类别"].ToString() == "经纪人交易账户")//根据方法类别获取基础数据
             {
                   dataTableBasic = dsReturn.Tables["经纪人交易账户基本信息"];
             }
             else
             { 
              dataTableBasic = dsReturn.Tables["买家卖家交易账户基本信息"];
             dataTableMMJ_JJR=dsReturn.Tables["买家卖家关联经纪人账户基本信息"];
             }
            #endregion

             #region//绑定数据
             if (dataTableBasic.Rows.Count > 0)
            {

                HTuser["登录_Number"] = dataTableBasic.Rows[0]["登录_Number"].ToString();
                HTuser["关联_Number"] = dataTableBasic.Rows[0]["关联_Number"].ToString();

                #region//记录经纪人、买卖家角色编号
                if (dataTableBasic.Rows[0]["J_JJRJSBH"] != null && dataTableBasic.Rows[0]["J_JJRJSBH"].ToString() != "")
                {
                    HTuser["原经纪人角色编号"] = dataTableBasic.Rows[0]["J_JJRJSBH"].ToString();
                }
                else
                {
                    HTuser["原经纪人角色编号"] ="";
                }
                if (dataTableBasic.Rows[0]["J_BUYJSBH"] != null && dataTableBasic.Rows[0]["J_BUYJSBH"].ToString() != "")
                {
                    HTuser["原买家角色编号"] = dataTableBasic.Rows[0]["J_BUYJSBH"].ToString();//保存原来的买家角色编号
                }
                else
                {
                    HTuser["原买家角色编号"] = "";
                }
                if (dataTableBasic.Rows[0]["J_SELJSBH"] != null && dataTableBasic.Rows[0]["J_SELJSBH"].ToString() != "")
                {
                    HTuser["原卖家角色编号"] = dataTableBasic.Rows[0]["J_SELJSBH"].ToString();//保存原来的卖家角色编号
                }
                else
                {
                    HTuser["原卖家角色编号"] = "";
                }
                #endregion
                #region//根据不同交易账户记录审核状态
                if (dataTableBasic.Rows[0]["B_JSZHLX"].ToString() == "买家卖家交易账户")
                {
                    this.radJJR.Checked = false;
                    this.radMMJ.Checked = true;
                    strSHZQ += "经纪人" + dataTableBasic.Rows[0]["JJRSHZT"].ToString()+"；";
                    strSHZQ += "业务管理部门" + dataTableBasic.Rows[0]["FGSSHZT"].ToString() + "。";
                    if (dataTableBasic.Rows[0]["JJRSHZT"].ToString() == "审核通过")
                    { 
                         hashTableVirify["经纪人是否审核通过"]="是";
                    }
                    else
                    {
                           hashTableVirify["经纪人是否审核通过"]="否";
                    }

                    if(dataTableBasic.Rows[0]["FGSSHZT"].ToString()=="审核通过")
                    {
                        hashTableVirify["分公司是否审核通过"] = "是";
                    }
                    else
                    {
                        hashTableVirify["分公司是否审核通过"] = "否";
                    }
                }
                else
                {
                    this.radJJR.Checked = true;
                    this.radMMJ.Checked = false;
                    strSHZQ += "业务管理部门" + dataTableBasic.Rows[0]["FGSSHZT"].ToString() + "。";

                    if (dataTableBasic.Rows[0]["FGSSHZT"].ToString() == "审核通过")
                    {
                        hashTableVirify["分公司是否审核通过"] = "是";
                    }
                    else
                    {
                        hashTableVirify["分公司是否审核通过"] = "否";
                    }


                }
                #endregion

                this.radJJR.Enabled = false;
                this.radMMJ.Enabled = false;
                this.lblDQSHQK.Text = strSHZQ;
                this.LLlishi.Location = new Point(this.lblDQSHQK.Location.X + this.lblDQSHQK.Width+10,this.LLlishi.Height);
                #region//根据注册类别绑定交易方名称
                if (dataTableBasic.Rows[0]["I_ZCLB"].ToString() == "单位")
                {
                    this.radDW.Checked = true;
                    this.radZRR.Checked = false;
                    this.labJYFMC_TSXX.Text = "交易方名称须与《组织机构代码证》的单位全称完全一致，否则交易账户将无法与银行的第三方存管账户绑定，不能出、入金！";
                    this.panJYFMC_TSXX.Visible = true;
                }
                else
                {
                    this.radDW.Checked = false;
                    this.radZRR.Checked = true;
                    this.labJYFMC_TSXX.Text = "交易方名称须与《身份证》的自然人姓名完全一致，否则交易账户将无法与银行的第三方存管账户绑定，不能出、入金！";
                    this.panJYFMC_TSXX.Visible = true;
                }
                #endregion
                HTuser["原注册类别"] = dataTableBasic.Rows[0]["I_ZCLB"].ToString();
                this.txtJJFMC.Text = dataTableBasic.Rows[0]["I_JYFMC"].ToString();
                this.txtYYZZ.Text = dataTableBasic.Rows[0]["I_YYZZZCH"].ToString();
                #region//根据注册类别绑定上传的图片
                if (dataTableBasic.Rows[0]["I_ZCLB"].ToString() == "单位")
                {
                    ListView lvYYZZ = pan_SC_YYZZ.UpItem;
                    //赋值(一定要先取出来)
                    lvYYZZ.Items.Clear();
                    lvYYZZ.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_YYZZSMJ"].ToString(), "", "", "" }));

                    this.txtZZJGDMZ.Text = dataTableBasic.Rows[0]["I_ZZJGDMZDM"].ToString();

                    ListView lvZZJGDMZ = pan_SC_ZZJGDMZ.UpItem;
                    //赋值(一定要先取出来)
                    lvZZJGDMZ.Items.Clear();
                    lvZZJGDMZ.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_ZZJGDMZSMJ"].ToString(), "", "", "" }));

                    this.txtSWDJZ.Text = dataTableBasic.Rows[0]["I_SWDJZSH"].ToString();
                    ListView lvSWDJZ = pan_SC_SWDJZ.UpItem;
                    //赋值(一定要先取出来)
                    lvSWDJZ.Items.Clear();
                    lvSWDJZ.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_SWDJZSMJ"].ToString(), "", "", "" }));

                    ListView lvNSRZGZ = pan_SC_NSRZGZ.UpItem;
                    //赋值(一定要先取出来)
                    lvNSRZGZ.Items.Clear();
                    lvNSRZGZ.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_YBNSRZGZSMJ"].ToString(), "", "", "" }));
                    this.txtKHXKZ.Text = dataTableBasic.Rows[0]["I_KHXKZH"].ToString();
                    ListView lvKHXKZ = pan_SC_KHXKZ.UpItem;
                    //赋值(一定要先取出来)
                    lvKHXKZ.Items.Clear();
                    lvKHXKZ.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_KHXKZSMJ"].ToString(), "", "", "" }));
                 
                    ListView lvYLYJK = pan_SC_YLYJK.UpItem;
                    //赋值(一定要先取出来)
                    lvYLYJK.Items.Clear();
                    lvYLYJK.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_YLYJK"].ToString(), "", "", "" }));

                    this.txtFDDBR.Text = dataTableBasic.Rows[0]["I_FDDBRXM"].ToString();
                    this.txtFDDBRSHZH.Text = dataTableBasic.Rows[0]["I_FDDBRSFZH"].ToString();

                    ListView lvFDDBRSHZH = pan_SC_FDDBRSHZH.UpItem;
                    //赋值(一定要先取出来)
                    lvFDDBRSHZH.Items.Clear();
                    lvFDDBRSHZH.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_FDDBRSFZSMJ"].ToString(), "", "", "" }));

                    ListView lvFDDBRSFZH_FM = pan_SC_FDDBRSHZH_FM.UpItem;
                    //赋值(一定要先取出来)
                    lvFDDBRSFZH_FM.Items.Clear();
                    lvFDDBRSFZH_FM.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_FDDBRSFZFMSMJ"].ToString(), "", "", "" }));

                    ListView lvFDDBRSQS = pan_SC_FDDBRSQS.UpItem;
                    //赋值(一定要先取出来)
                    lvFDDBRSQS.Items.Clear();
                    lvFDDBRSQS.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_FDDBRSQS"].ToString(), "", "", "" }));
                }
                else
                {
                    this.txtSFZ.Text = dataTableBasic.Rows[0]["I_SFZH"].ToString();
                    ListView lvSFZ = pan_SC_SFZ.UpItem;
                    //赋值(一定要先取出来)
                    lvSFZ.Items.Clear();
                    lvSFZ.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_SFZSMJ"].ToString(), "", "", "" }));

                    ListView lvSFZ_FM = pan_SC_SFZ_FM.UpItem;
                    //赋值(一定要先取出来)
                    lvSFZ_FM.Items.Clear();
                    lvSFZ_FM.Items.Add(new ListViewItem(new string[] { "", dataTableBasic.Rows[0]["I_SFZFMSMJ"].ToString(), "", "", "" }));
                }
                #endregion
                #region//绑定基础信息
                this.txtJYFLXDH.Text = dataTableBasic.Rows[0]["I_JYFLXDH"].ToString();
                this.ucSSQY.SelectedItem = new string[] { dataTableBasic.Rows[0]["I_SSQYS"].ToString(), dataTableBasic.Rows[0]["I_SSQYSHI"].ToString(), dataTableBasic.Rows[0]["I_SSQYQ"].ToString() };
                //strTagSF_Real = dataTableBasic.Rows[0]["I_SSQYS"].ToString();
                //strTagDS_Real = dataTableBasic.Rows[0]["I_SSQYSHI"].ToString();
                strTagSF = dataTableBasic.Rows[0]["I_SSQYS"].ToString();
                strTagDS = dataTableBasic.Rows[0]["I_SSQYSHI"].ToString();
                this.txtXXDZ.Text = dataTableBasic.Rows[0]["I_XXDZ"].ToString();
                this.txtLXRXM.Text = dataTableBasic.Rows[0]["I_LXRXM"].ToString();
                this.txtLXRSJH.Text = dataTableBasic.Rows[0]["I_LXRSJH"].ToString();
                //this.txtKHYH.Text = dataTableBasic.Rows[0]["I_KHYH"].ToString();
                this.cbxKHYH.SelectedItem = dataTableBasic.Rows[0]["I_KHYH"].ToString();
                this.txtYHZH.Text = dataTableBasic.Rows[0]["I_YHZH"].ToString();
                #endregion

                //绑定对应的平台管理机构数据
                if (dataTableBasic.Rows[0]["I_PTGLJG"] != null && dataTableBasic.Rows[0]["I_PTGLJG"].ToString() != "")
                {
                    string strFGSName = dataTableBasic.Rows[0]["I_PTGLJG"].ToString();
                    HTuser["原平台管理机构"] = strFGSName;
                    for (int i = 0; i < cbxGLJG.Items.Count; i++)
                    {
                        string strSelectItem = cbxGLJG.Items[i].ToString();
                        if (strSelectItem == strFGSName)
                        {
                            cbxGLJG.SelectedIndex = i;
                            this.timer_PTGLJG.Enabled = false;
                            break;
                        }
                    }
                }
                #region//绑定买卖家账户的经纪人的一些信息
                if (dataTableMMJ_JJR != null && dataTableMMJ_JJR.Rows.Count > 0)
                {
                    this.txtJJRZGZS.Text = dataTableMMJ_JJR.Rows[0]["J_JJRZGZSBH"].ToString();
                    HTuser["原经纪人资格证书编号"] = dataTableMMJ_JJR.Rows[0]["J_JJRZGZSBH"].ToString();
                    this.txtJJRMC.Text = dataTableMMJ_JJR.Rows[0]["I_JYFMC"].ToString();
                    this.txtJJRLXDH.Text = dataTableMMJ_JJR.Rows[0]["I_JYFLXDH"].ToString();
                    lblMMJGLB_Number.Text = dataTableMMJ_JJR.Rows[0]["Number"].ToString();
                    this.lblBankDLYX.Text = dataTableMMJ_JJR.Rows[0]["B_DLYX"].ToString();//关联银行登录账号
                }
                #endregion
                //根据界面的数据，决定显示哪些界面内容
                #region//根据界面的数据，决定显示哪些界面内容--买家卖家交易账户
                if (dataTableBasic.Rows[0]["B_JSZHLX"].ToString() == "买家卖家交易账户")
                {
                    this.panZGZS.Visible = true;
                    this.panJJRMC.Visible = true;
                    this.panJJRLXDH.Visible = true;
                    this.panGLJG.Visible = false;
                    //经纪人分类
                    this.panGLBMFL.Visible = false;
                    this.panTWGLBM.Visible = false;
                    this.panYWFWBM.Visible = true;
                                        
                    this.panSZYX.Visible = false;

                    if (dataTableBasic.Rows[0]["I_YWFWBM"].ToString() == "银行")
                    {
                        radFW_Bank.Checked = true;
                        this.radFW_JJR.Checked = false;
                        this.radFW_ZF.Checked = false;
                        //显示银行
                        this.panGLYH.Visible = true;
                        this.panBankYGGH.Visible = true;
                        //经纪人资格证书等的隐藏
                        this.panZGZS.Visible = false;
                        this.panJJRMC.Visible = false;
                        this.panJJRLXDH.Visible = false;

                        this.labYHMC.Text = dataTableBasic.Rows[0]["I_GLYH"].ToString();//关联银行
                        this.uctextBankYGGH.Text = dataTableBasic.Rows[0]["I_GLYHGZRYGH"].ToString();//关联银行工作人员工号
                        this.lblJJRFL.Text = dataTableBasic.Rows[0]["I_YWFWBM"].ToString();//业务服务部门
                        //为经纪人资格证书等的赋值，只是为了记录,如果没有变更业务服务部门，直接取这三个输入框的值
                        this.txtJJRZGZS.Text = dataTableMMJ_JJR.Rows[0]["J_JJRZGZSBH"].ToString();
                        this.txtJJRMC.Text = dataTableMMJ_JJR.Rows[0]["I_JYFMC"].ToString();
                        this.txtJJRLXDH.Text = dataTableMMJ_JJR.Rows[0]["I_JYFLXDH"].ToString();
                    }
                    else if (dataTableBasic.Rows[0]["I_YWFWBM"].ToString() == "一般经纪人")
                    {
                        radFW_Bank.Checked = false;
                        this.radFW_JJR.Checked = true;
                        this.radFW_ZF.Checked = false;
                        //隐藏银行
                        this.panGLYH.Visible = false;
                        this.panBankYGGH.Visible = false;
                        //经纪人资格证书等的显示
                        this.panZGZS.Visible = true;
                        this.panJJRMC.Visible = true;
                        this.panJJRLXDH.Visible = true;

                        this.lblJJRFL.Text = dataTableBasic.Rows[0]["I_YWFWBM"].ToString();//业务服务部门
                        //为经纪人资格证书等的赋值，只是为了记录,如果没有变更业务服务部门，直接取这三个输入框的值
                        this.txtJJRZGZS.Text = dataTableMMJ_JJR.Rows[0]["J_JJRZGZSBH"].ToString();
                        this.txtJJRMC.Text = dataTableMMJ_JJR.Rows[0]["I_JYFMC"].ToString();
                        this.txtJJRLXDH.Text = dataTableMMJ_JJR.Rows[0]["I_JYFLXDH"].ToString();
                    }
                    else
                    {
                        radFW_Bank.Checked = false;
                        this.radFW_JJR.Checked = true;
                        this.radFW_ZF.Checked = false;
                        //隐藏银行
                        this.panGLYH.Visible = false;
                        this.panBankYGGH.Visible = false;
                        //经纪人资格证书等的显示
                        this.panZGZS.Visible = true;
                        this.panJJRMC.Visible = true;
                        this.panJJRLXDH.Visible = true;
                    }

                }
#endregion
                #region//根据界面的数据，决定显示哪些界面内容--经纪人交易账户
                else
                {
                    this.panZGZS.Visible = false;
                    this.panJJRMC.Visible = false;
                    this.panJJRLXDH.Visible = false;
                    this.panGLJG.Visible = false;
                    //管理部门分类
                    this.panGLBMFL.Visible = true;
                    this.panYWFWBM.Visible = false;
                    this.panGLYH.Visible = false;
                    this.panBankYGGH.Visible = false;
                    this.panGLZFJG.Visible = false;
                    this.radFGS.Checked = true;
                    this.radYWTZB.Checked = false;
                    this.radGXTW.Checked = false;
                    this.panHYXH.Visible = false;

                    if (dataTableBasic.Rows[0]["I_YWGLBMFL"].ToString() == "分公司")
                    {
                        this.radFGS.Checked = true;
                        this.radYWTZB.Checked = false;
                        this.radGXTW.Checked = false;
                        radFGS_CheckedChanged(null, true);
                    }
                    else if (dataTableBasic.Rows[0]["I_YWGLBMFL"].ToString() == "平台总部")
                    {
                   
                        this.radFGS.Checked = false;
                        this.radYWTZB.Checked = true;
                        this.radGXTW.Checked = false;
                        radYWTZB_CheckedChanged(null, true);
                    
                    }
                    else if (dataTableBasic.Rows[0]["I_YWGLBMFL"].ToString() == "高校团委")
                    {
                        this.radFGS.Checked = false;
                        this.radYWTZB.Checked = false;
                        this.radGXTW.Checked = true;
                        radGXTW_CheckedChanged(null, true);
                        this.labTWMC.Text = dataTableBasic.Rows[0]["I_PTGLJG"].ToString();
                        this.labGXTW_ZhangHao.Text = dataTableBasic.Rows[0]["I_PTGLJG"].ToString();//此处再读取账号没有意义
                        this.txtSZYX.Text = dataTableBasic.Rows[0]["I_YXBH"].ToString();              
                    }
                }
                #endregion
                #region//根据界面的数据，决定显示哪些界面内容--不同注册类别
                if (dataTableBasic.Rows[0]["I_ZCLB"].ToString() == "单位")
                {
                    this.panYYZZ.Visible = true;
                    this.panSFZ.Visible = false;
                    this.panZZJGDMZ.Visible = true;
                    this.panSWDJZ.Visible = true;
                   // this.panNSRZGZ.Visible = true;
                    this.panYLYJK.Visible = true;
                    this.panKHXKZ.Visible = true;
                    this.panFDDBR.Visible = true;
                    this.panFDDBRSFZH.Visible = true;
                    this.panFDDBRSQS.Visible = true;
                }
                else
                {
                    this.panYYZZ.Visible = false;
                    this.panSFZ.Visible = true;
                    this.panZZJGDMZ.Visible = false;
                    this.panSWDJZ.Visible = false;
                    this.panNSRZGZ.Visible = false;
                    this.panKHXKZ.Visible = false;
                    this.panYLYJK.Visible = false;
                    this.panFDDBR.Visible = false;
                    this.panFDDBRSFZH.Visible = false;
                    this.panFDDBRSQS.Visible = false;
                }
                #endregion
                //根据审核情况决定哪些字段表可以修改，哪些不可以修改
                #region//根据审核情况决定哪些字段表可以修改，哪些不可以修改--经纪人交易账户
                if (dataTableBasic.Rows[0]["B_JSZHLX"].ToString() == "经纪人交易账户")
                {
                    #region//分公司审核通过
                    if (dataTableBasic.Rows[0]["S_SFYBFGSSHTG"].ToString() == "是")//审核通过
                    {
                        this.radDW.Enabled = false;
                        this.radZRR.Enabled = false;
                        this.txtJJFMC.Enabled = false;

                        this.txtYYZZ.Enabled = false;
                        this.pan_SC_YYZZ.showB = new bool[] {false,true,false };

                        this.txtSFZ.Enabled = false;
                        this.pan_SC_SFZ.showB = new bool[] { false, true, false };
                        this.pan_SC_SFZ_FM.showB = new bool[] { false, true, false };
                        this.txtZZJGDMZ.Enabled = false;
                        this.pan_SC_ZZJGDMZ.showB = new bool[] {false,true,false };

                        this.txtSWDJZ.Enabled = false;
                        this.pan_SC_SWDJZ.showB = new bool[] {false,true,false };

                        this.pan_SC_NSRZGZ.showB = new bool[] {false,true,false };

                        this.txtKHXKZ.Enabled = false;
                        this.pan_SC_KHXKZ.showB = new bool[] {false,true,false };
                        //预留印签卡
                        this.pan_SC_YLYJK.showB = new bool[] {false,true,false };

                        this.txtFDDBR.Enabled = false;

                        this.txtFDDBRSHZH.Enabled = false;
                        this.pan_SC_FDDBRSHZH.showB = new bool[] {false,true,false };
                        this.pan_SC_FDDBRSHZH_FM.showB = new bool[] { false, true, false };
                        this.pan_SC_FDDBRSQS.showB = new bool[] { false, true, false };

                        this.txtJYFLXDH.Enabled = true;

                        ucSSQY.EnabledItem = new bool[] { false, false, false };

                        this.txtXXDZ.Enabled = true;

                        this.txtLXRXM.Enabled = true;

                        this.txtLXRSJH.Enabled = true;

                        //this.txtKHYH.Enabled = false;
                        this.cbxKHYH.Enabled = false;
                       // this.txtYHZH.Enabled = false;

                        this.cbxGLJG.Enabled = false;

                        this.txtJJRZGZS.Enabled = false;

                        this.txtJJRMC.Enabled = false;

                        this.txtJJRLXDH.Enabled = false;

                        this.radFGS.Enabled = false;
                        this.radYWTZB.Enabled = false;
                        this.radGXTW.Enabled = false;

                        this.labCKTWBXZ.Enabled = false;
                        this.txtSZYX.Enabled = false;


                    }
                    #endregion
                    #region//审核中、或驳回
                    else//审核中、或驳回
                    {
                        this.radDW.Enabled = true;
                        this.radZRR.Enabled = true;
                        this.txtJJFMC.Enabled = true;
                        this.txtYYZZ.Enabled = true;
                        this.txtSFZ.Enabled = true;
                        this.txtZZJGDMZ.Enabled = true;
                        this.txtSWDJZ.Enabled = true;
                        this.txtKHXKZ.Enabled = true;
                        this.txtFDDBR.Enabled = true;
                        this.txtFDDBRSHZH.Enabled = true;
                        this.txtJYFLXDH.Enabled = true;
                        this.txtXXDZ.Enabled = true;
                        this.txtLXRXM.Enabled = true;
                        this.txtLXRSJH.Enabled = true;
                        //this.txtKHYH.Enabled = true;
                        this.cbxKHYH.Enabled = true;
                       // this.txtYHZH.Enabled = true;
                        this.cbxGLJG.Enabled = true;
                        this.txtJJRZGZS.Enabled = true;
                        this.txtJJRMC.Enabled = false;
                        this.txtJJRLXDH.Enabled = false;


                        this.radFGS.Enabled = true;
                        this.radYWTZB.Enabled = true;
                        this.radGXTW.Enabled = true;

                        this.labCKTWBXZ.Enabled = true;
                        this.txtSZYX.Enabled = true;


                    }
                    #endregion
                }
                #endregion
                #region//根据审核情况决定哪些字段表可以修改，哪些不可以修改--买卖家交易账户
                else
                {
                    #region//经纪人审核通过、分公司驳回或者审核中
                    if (dataTableBasic.Rows[0]["S_SFYBJJRSHTG"].ToString() == "是" && dataTableBasic.Rows[0]["S_SFYBFGSSHTG"].ToString() == "否")//经纪人审核通过、分公司驳回或者审核中
                    {
                        this.radDW.Enabled = true;
                        this.radZRR.Enabled = true;
                        this.txtJJFMC.Enabled = true;

                        this.txtYYZZ.Enabled = true;
                       // this.pan_SC_YYZZ.showB = new bool[] { true, true, true };

                        this.txtSFZ.Enabled = true;
                       // this.pan_SC_SFZ.showB = new bool[] { true, true, true };

                        this.txtZZJGDMZ.Enabled = true;
                       // this.pan_SC_ZZJGDMZ.showB = new bool[] { true, true, true };

                        this.txtSWDJZ.Enabled = true;
                        //this.pan_SC_SWDJZ.showB = new bool[] { true, true, true };

                       // this.pan_SC_NSRZGZ.showB = new bool[] { true, true, true };

                        this.txtKHXKZ.Enabled = true;
                        //this.pan_SC_KHXKZ.showB = new bool[] { true, true, true };

                        this.txtFDDBR.Enabled = true;

                        this.txtFDDBRSHZH.Enabled = true;
                        //this.pan_SC_FDDBRSHZH.showB = new bool[] { true, true, true };

                       // this.pan_SC_FDDBRSQS.showB = new bool[] { true, true, true };

                        this.txtJYFLXDH.Enabled = true;

                        ucSSQY.EnabledItem = new bool[] { true, true, true };

                        this.txtXXDZ.Enabled = true;

                        this.txtLXRXM.Enabled = true;

                        this.txtLXRSJH.Enabled = true;

                        //this.txtKHYH.Enabled = true;
                        this.cbxKHYH.Enabled = true;
                        //this.txtYHZH.Enabled = true;

                        this.cbxGLJG.Enabled = true;

                        this.txtJJRZGZS.Enabled = false;

                        this.txtJJRMC.Enabled = false;

                        this.txtJJRLXDH.Enabled = false;

                        //经纪人审核通过
                        this.radFW_JJR.Enabled = false;
                        this.radFW_Bank.Enabled = false;
                        this.radFW_ZF.Enabled = false;
                        this.radHangYeXieHui.Enabled = false;
                        this.labCKYHBXZ.Enabled = false;
                        this.uctextBankYGGH.Enabled = false;
                        this.lblSelectBankYGGH.Enabled = false;

                        this.labCKZFBXZ.Enabled = false;
                        this.labCKBXZHYXH.Enabled = false;



                    }
                    #endregion
                    #region//经纪人审核通过、分公司审核通过
                    else if (dataTableBasic.Rows[0]["S_SFYBJJRSHTG"].ToString() == "是" && dataTableBasic.Rows[0]["S_SFYBFGSSHTG"].ToString() == "是")//经纪人审核通过、分公司审核通过
                    {
                        this.radDW.Enabled = false;
                        this.radZRR.Enabled = false;
                        this.txtJJFMC.Enabled = false;

                        this.txtYYZZ.Enabled = false;
                        this.pan_SC_YYZZ.showB = new bool[] { false, true, false };

                        this.txtSFZ.Enabled = false;
                        this.pan_SC_SFZ.showB = new bool[] { false, true, false };
                        this.pan_SC_SFZ_FM.showB = new bool[] { false, true, false };
                        this.txtZZJGDMZ.Enabled = false;
                        this.pan_SC_ZZJGDMZ.showB = new bool[] { false, true, false };

                        this.txtSWDJZ.Enabled = false;
                        this.pan_SC_SWDJZ.showB = new bool[] { false, true, false };

                        this.pan_SC_NSRZGZ.showB = new bool[] { false, true, false };

                        this.txtKHXKZ.Enabled = false;
                        this.pan_SC_KHXKZ.showB = new bool[] { false, true, false };

                        //预留印签卡
                        this.pan_SC_YLYJK.showB = new bool[] { false, true, false };

                        this.txtFDDBR.Enabled = false;

                        this.txtFDDBRSHZH.Enabled = false;
                        this.pan_SC_FDDBRSHZH.showB = new bool[] { false, true, false };
                        this.pan_SC_FDDBRSHZH_FM.showB = new bool[] { false, true, false };
                        this.pan_SC_FDDBRSQS.showB = new bool[] { false, true, false };

                        this.txtJYFLXDH.Enabled = true;

                        ucSSQY.EnabledItem = new bool[] { false, false, false };

                        this.txtXXDZ.Enabled = true;

                        this.txtLXRXM.Enabled = true;

                        this.txtLXRSJH.Enabled = true;

                        //this.txtKHYH.Enabled = false;
                        this.cbxKHYH.Enabled = false;
                       // this.txtYHZH.Enabled = false;

                        this.cbxGLJG.Enabled = false;

                        this.txtJJRZGZS.Enabled = false;

                        this.txtJJRMC.Enabled = false;

                        this.txtJJRLXDH.Enabled = false;
                        //经纪人审核通过
                        this.radFW_JJR.Enabled = false;
                        this.radFW_Bank.Enabled = false;
                        this.radFW_ZF.Enabled = false;
                        this.radHangYeXieHui.Enabled = false;
                        this.labCKYHBXZ.Enabled = false;
                        this.uctextBankYGGH.Enabled = false;
                        this.lblSelectBankYGGH.Enabled = false;
                        this.labCKZFBXZ.Enabled = false;
                        this.labCKBXZHYXH.Enabled = false;



                    }
                    #endregion
                }
                #endregion
            }
#endregion

             if (this.radJJR.Checked==true)//经纪人交易账户
            {
                this.linkZHKTXY.Text = "查看经纪人交易账户开通协议";
                this.timer_PTGLJG.Enabled = true;
            }
             else if (this.radMMJ.Checked==true)//买卖家交易账户
             {                
                 this.linkZHKTXY.Text = "查看交易方交易账户开通协议";
                 this.timer_PTGLJG.Enabled = false;
            }

             this.panelTJQ.Enabled = true;
             this.flowLayoutPanel1.Enabled = true;

            strSFYMJZWC = "页面加载完成";
        }


        //开启一个线程获取基础数据
        private void SRT_GetBasicData_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(HTuser, new delegateForThread(SRT_GetBasicData));
            Thread trd = new Thread(new ThreadStart(OTD.GetBasicData));
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
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    //绑定界面用户数据
                    PublicDS.PublisDsUser = dsreturn;
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
        /// 获取平台管理机构
        /// </summary>
        private void BindPTGLJG()
        {
            DataSet dataSet = PublicDS.PublisDsData;
            cbxGLJG.Items.Add("请选择业务管理部门");

            string[] distinctcols = new string[] { "FGSname" };
            DataTable dtfd = new DataTable("distinctTable");
            DataView mydataview = new DataView(dataSet.Tables["分公司对照表"]);
            dtfd = mydataview.ToTable(true, distinctcols);
            DataRow[] dataRows = dtfd.Select("FGSname like '%分公司%'");
            foreach (DataRow dr in dataRows)
            {
                cbxGLJG.Items.Add(dr["FGSname"].ToString());
            }
            cbxGLJG.SelectedIndex = 0;
        }

        /// <summary>
        /// 判断经纪人用户是否可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJJRZGZS_KeyUp(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// 判断经纪人用户是否可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJJRZGZS_TextChanged(object sender, EventArgs e)
        {
            if (this.txtJJRZGZS.Enabled == false)
            {
                return;
            }
            if (this.radFW_JJR.Checked == true || this.radFW_Bank.Checked == true)//一般经纪人
            {
                if (this.radFW_Bank.Checked == true)
                {
                    lablRemJJRZGZS.Visible = false;
                }
                if (this.radFW_JJR.Checked == true)
                {
                    // ArrayList lv = new ArrayList();
                    txtJJRMC.Text = "";
                    txtJJRLXDH.Text = "";
                    if (!txtJJRZGZS.Text.Trim().Equals(""))
                    {
                        Hashtable htjjryhm = new Hashtable();
                        htjjryhm["JJRZGZS"] = txtJJRZGZS.Text.Trim();
                        if (ValStr.ValidateQuery(htjjryhm))
                        {
                            lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                            lablRemJJRZGZS.Visible = true;
                            return;
                        }
                        NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                        //InPutHT可以选择灵活使用
                        object[] objParams = { txtJJRZGZS.Text.Trim() };
                        //已移植可替换(已替换)
                        //DataSet dsreturn = WSC2013.RunAtServices("GetJJRYHXX", objParams);
                        DataSet dsreturn = WSC2013.RunAtServices("C证书获取经纪人信息", objParams);
                        DataTable dt = dsreturn.Tables["经纪人用户信息"];
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            DataRow dr = dt.Rows[0];
                            if (!dr["分公司开通审核状态"].ToString().Trim().Equals("是"))
                            {

                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            if (!dr["结算账户类型"].ToString().Trim().Equals("经纪人交易账户"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }

                            if (!dr["经纪人是否暂停新用户审核"].ToString().Trim().Equals("否"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            if (!dr["是否允许登陆"].ToString().Trim().Equals("是"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            if (dr["是否冻结"].ToString().Trim().Equals("是"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            if (dr["是否休眠"].ToString().Trim().Equals("是"))
                            {
                                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                lablRemJJRZGZS.Visible = true;
                                return;
                            }
                            lablRemJJRZGZS.Visible = false;
                            txtJJRMC.Text = dr["经纪人名称"].ToString();
                            txtJJRLXDH.Text = dr["经纪人联系电话"].ToString();
                            lblJJRFL.Text = dr["经纪人分类"].ToString();
                        }
                        else
                        {
                            lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                            lablRemJJRZGZS.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                        lablRemJJRZGZS.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                lablRemJJRZGZS.Text = "您填写的经纪人资格证书无效！";
                lablRemJJRZGZS.Visible = true;

            }

        }

        /// <summary>
        /// 单位名称（根据其选中情况，调整界面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radDW_CheckedChanged(object sender, bool Checked)
        {
            //禁止pnl重绘,这里比较特殊，一定要由这个东西
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            if (Checked == true)
            {
                string str = this.txtJJFMC.Text;
                this.txtJJFMC.TextNtip = "请填写单位全称，与您在银行开户时的账户企业名称一致";
                if (this.txtXXDZ.Text.Trim() == "")
                { this.txtXXDZ.TextNtip = "请不要填写省市区信息"; }
                if (str.Trim() != "")
                {
                    this.txtJJFMC.Text = str;
                }
                this.panYYZZ.Visible = true;
                this.panZZJGDMZ.Visible = true;
                this.panSWDJZ.Visible = true;
               // this.panNSRZGZ.Visible = true;
                this.panKHXKZ.Visible = true;
                this.panYLYJK.Visible = true;
                this.panFDDBR.Visible = true;
                this.panFDDBRSFZH.Visible = true;
                this.panFDDBRSQS.Visible = true;
                this.panSFZ.Visible = false;
                SetRemindInforInVisiable();

                this.labJYFMC_TSXX.Text = "交易方名称须与《组织机构代码证》的单位全称完全一致，否则交易账户将无法与银行的第三方存管账户绑定，不能出、入金！";
                this.panJYFMC_TSXX.Visible = true;

            }
            //允许重绘pnl  
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            flowLayoutPanel1.Invalidate(true);
        }
        /// <summary>
        /// 自然人（根据其选中情况，调整界面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radZRR_CheckedChanged(object sender, bool Checked)
        {
            //禁止pnl重绘,这里比较特殊，一定要由这个东西
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            if (Checked == true)
            {
                string str = this.txtJJFMC.Text;
                this.txtJJFMC.TextNtip = "请输入自然人姓名";
                if (this.txtXXDZ.Text.Trim() == "")
                { this.txtXXDZ.TextNtip = "请不要填写省市区信息"; }
                if (str.Trim() != "")
                {
                    this.txtJJFMC.Text = str;
                }
                this.panYYZZ.Visible = false;
                this.panSFZ.Visible = true;
                this.panZZJGDMZ.Visible = false;
                this.panSWDJZ.Visible = false;
                this.panNSRZGZ.Visible = false;
                this.panKHXKZ.Visible = false;
                this.panYLYJK.Visible = false;
                this.panFDDBR.Visible = false;
                this.panFDDBRSFZH.Visible = false;
                this.panFDDBRSQS.Visible = false;
                SetRemindInforInVisiable();
                this.labJYFMC_TSXX.Text = "交易方名称须与《身份证》的自然人姓名完全一致，否则交易账户将无法与银行的第三方存管账户绑定，不能出、入金！";
                this.panJYFMC_TSXX.Visible = true;

            }
            //允许重绘pnl  
            SendMessage(flowLayoutPanel1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            flowLayoutPanel1.Invalidate(true);
        }




        #endregion
        //判断证书编号是否有效
        private bool IsValidZSBH()
        {
           
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            object[] objParams = { txtJJRZGZS.Text.Trim() };           
            DataSet dsreturn = WSC2013.RunAtServices("C证书获取经纪人信息", objParams);
            DataTable dt = dsreturn.Tables["经纪人用户信息"];
            if (dt != null && dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                if (!dr["分公司开通审核状态"].ToString().Trim().Equals("是"))
                {

                    lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                    lablRemJJRZGZS.Visible = true;
                    
                    return false;
                }
                if (!dr["结算账户类型"].ToString().Trim().Equals("经纪人交易账户"))
                {
                    lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                    lablRemJJRZGZS.Visible = true;
                    return false;
                }

                if (!dr["经纪人是否暂停新用户审核"].ToString().Trim().Equals("否"))
                {
                    lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                    lablRemJJRZGZS.Visible = true;
                    return false;
                }
                if (!dr["是否允许登陆"].ToString().Trim().Equals("是"))
                {
                    lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                    lablRemJJRZGZS.Visible = true;
                    return false;
                }
                if (dr["是否冻结"].ToString().Trim().Equals("是"))
                {
                    lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                    lablRemJJRZGZS.Visible = true;
                    return false;
                }
                if (dr["是否休眠"].ToString().Trim().Equals("是"))
                {
                    lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                    lablRemJJRZGZS.Visible = true;
                    return false;
                }
                lablRemJJRZGZS.Visible = false;
                txtJJRMC.Text = dr["经纪人名称"].ToString();
                txtJJRLXDH.Text = dr["经纪人联系电话"].ToString();
                lblJJRFL.Text = dr["经纪人分类"].ToString();
                return true;
            }
            else
            {
                lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                lablRemJJRZGZS.Visible = true;
                return false;
            }
        }


        #region//提交修改后的账号数据信息

        /// <summary>
        ///提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            this.PBload.Location = new Point(this.flowLayoutPanel1.Location.X + this.panTiJiao.Location.X + this.btnSave.Location.X + this.btnSave.Width + 25, this.flowLayoutPanel1.Location.Y + this.panTiJiao.Location.Y + this.btnSave.Location.Y);
            //禁用提交区域并开启进度
            hashTableInfor = new Hashtable();
            #region//经纪人交易账号
            if (this.radJJR.Checked == true)//经纪人交易账户
            {               
                if (this.radDW.Checked == true)//单位注册
                {
                    int tag = 0;
                    hashTableInfor["经纪人单位交易账户类别"] = "";
                    hashTableInfor["经纪人单位交易注册类别"] = "";
                    hashTableInfor["经纪人单位交易方名称"] = "";
                    hashTableInfor["经纪人单位营业执照注册号"] = "";
                    hashTableInfor["经纪人单位营业执照扫描件"] = "";
                    hashTableInfor["经纪人单位组织机构代码证代码证"] = "";
                    hashTableInfor["经纪人单位组织机构代码证代码证扫描件"] = "";
                    hashTableInfor["经纪人单位税务登记证税号"] = "";
                    hashTableInfor["经纪人单位税务登记证扫描件"] = "";
                    hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = "";
                    hashTableInfor["经纪人单位开户许可证号"] = "";
                    hashTableInfor["经纪人单位开户许扫描件"] = "";
                    hashTableInfor["经纪人单位预留印鉴卡扫描件"] = "";
                    hashTableInfor["经纪人单位法定代表人姓名"] = "";
                    hashTableInfor["经纪人单位法定代表人身份证号"] = "";
                    hashTableInfor["经纪人单位法定代表人身份证扫描件"] = "";
                    hashTableInfor["经纪人单位法定代表人身份证反面扫描件"] = "";
                    hashTableInfor["经纪人单位法定代表人授权书"] = "";
                    hashTableInfor["经纪人单位交易方联系电话"] = "";
                    hashTableInfor["经纪人单位所属省份"] = "";
                    hashTableInfor["经纪人单位所属地市"] = "";
                    hashTableInfor["经纪人单位所属区县"] = "";
                    hashTableInfor["经纪人单位详细地址"] = "";
                    hashTableInfor["经纪人单位联系人姓名"] = "";
                    hashTableInfor["经纪人单位联系人手机号"] = "";
                    hashTableInfor["经纪人单位开户银行"] = "";
                    hashTableInfor["经纪人单位银行账号"] = "";
                    hashTableInfor["经纪人单位平台管理机构"] = "";
                    hashTableInfor["经纪人单位登录邮箱"] = HTuser["DLYX"].ToString();
                    hashTableInfor["经纪人单位用户名"] = HTuser["YHM"].ToString();
                    hashTableInfor["经纪人单位登录_Number"] = HTuser["登录_Number"].ToString();
                    hashTableInfor["经纪人单位关联_Number"] = HTuser["关联_Number"].ToString();
                    hashTableInfor["经纪人单位经纪人角色编号"] = HTuser["原经纪人角色编号"].ToString();
                    hashTableInfor["经纪人单位买家角色编号"] = HTuser["原买家角色编号"].ToString();  
                    hashTableInfor["经纪人单位交易账户类别"] = "经纪人交易账户";
                    hashTableInfor["经纪人单位交易注册类别"] = "单位";
                    hashTableInfor["经纪人单位是否更换平台管理机构"] = "否";
                    hashTableInfor["经纪人单位是否更换注册类别"] = "否";
                    hashTableInfor["方法类别"] = "经纪人单位";//这里是为了方便选择处理方法
                    hashTableInfor["经纪人单位是否已被分公司审核通过"] = hashTableVirify["分公司是否审核通过"].ToString();
                    hashTableInfor["经纪人单位院系编号"] = "";
                    hashTableInfor["经纪人单位经纪人分类"] = "一般经纪人";
                    hashTableInfor["经纪人单位业务管理部门分类"] = "";

                    if (hashTableInfor["经纪人单位交易注册类别"].ToString() == HTuser["原注册类别"].ToString())
                    {
                        hashTableInfor["经纪人单位是否更换注册类别"] = "否";
                    }
                    else
                    {
                        hashTableInfor["经纪人单位是否更换注册类别"] = "是";
                    }

                    if (this.txtJJFMC.Text.Trim() == "")
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = false;
                     
                        hashTableInfor["经纪人单位交易方名称"] = this.txtJJFMC.Text.Trim();

                    }
                    //营业执照
                    if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemYYZZ.Text = "请填写营业执照注册号，并上传扫描件！";
                        this.labRemYYZZ.Visible = true;
                    
                        tag += 1;
                    }
                    else if (this.txtYYZZ.Text.Trim() != "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemYYZZ.Text = "请上传营业执照扫描件！";
                        this.labRemYYZZ.Visible = true;
                        tag += 1;
                     
                    }
                    else if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem != null && this.pan_SC_YYZZ.UpItem.Items.Count > 0))
                    {
                        this.labRemYYZZ.Text = "请填写营业执照注册号！";
                        this.labRemYYZZ.Visible = true;
                      
                        tag += 1;
                    }
                    else
                    {
                        this.labRemYYZZ.Visible = false;
                      
                        hashTableInfor["经纪人单位营业执照注册号"] = this.txtYYZZ.Text;
                        hashTableInfor["经纪人单位营业执照扫描件"] = this.pan_SC_YYZZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //组织机构代码证
                    if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码，并上传扫描件！";
                        this.labRemZZJGDMZ.Visible = true;
                    
                        tag += 1;
                    }
                    else if (this.txtZZJGDMZ.Text.Trim() != "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemZZJGDMZ.Text = "请上传组织机构代码证扫描件！";
                        this.labRemZZJGDMZ.Visible = true;
                      
                        tag += 1;
                    }
                    else if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem != null && this.pan_SC_ZZJGDMZ.UpItem.Items.Count > 0))
                    {
                        this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码！";
                        this.labRemZZJGDMZ.Visible = true;
                      
                        tag += 1;
                    }
                    else
                    {
                        this.labRemZZJGDMZ.Visible = false;
                      
                        hashTableInfor["经纪人单位组织机构代码证代码证"] = this.txtZZJGDMZ.Text;
                        hashTableInfor["经纪人单位组织机构代码证代码证扫描件"] = this.pan_SC_ZZJGDMZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }

                  



                    //税务登记证
                    if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSWDJZ.Text = "请填写税务登记证税号，并上传扫描件！";
                        this.labRemSWDJZ.Visible = true;
                      
                        tag += 1;
                    }
                    else if (this.txtSWDJZ.Text.Trim() != "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSWDJZ.Text = "请上传税务登记证扫描件！";
                        this.labRemSWDJZ.Visible = true;
                      
                        tag += 1;
                    }
                    else if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem != null && this.pan_SC_SWDJZ.UpItem.Items.Count > 0))
                    {
                        this.labRemSWDJZ.Text = "请填写税务登记证税号！";
                        this.labRemSWDJZ.Visible = true;
                      
                        tag += 1;
                    }
                    else
                    {
                        this.labRemSWDJZ.Visible = false;
                     
                        hashTableInfor["经纪人单位税务登记证税号"] = this.txtSWDJZ.Text;
                        hashTableInfor["经纪人单位税务登记证扫描件"] = this.pan_SC_SWDJZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //纳税人资格证
                    //if (this.pan_SC_NSRZGZ.UpItem == null || this.pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
                    //{
                    //    this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                    //    this.labRemNSRZGZ.Visible = true;

                    //    tag += 1;
                    //}
                    //else if (this.pan_SC_NSRZGZ.UpItem != null && this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                    //{
                    //    this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                    //    this.labRemNSRZGZ.Visible = false;

                    //    hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = this.pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    //}
                    //开户许可证
                    if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemKHXKZ.Text = "请填写开户许可证号，并上传扫描件！";
                        this.labRemKHXKZ.Visible = true;
                   
                        tag += 1;
                    }
                    else if (this.txtKHXKZ.Text.Trim() != "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemKHXKZ.Text = "请上传开户许可证扫描件！";
                        this.labRemKHXKZ.Visible = true;
                   
                        tag += 1;
                    }
                    else if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem != null && this.pan_SC_KHXKZ.UpItem.Items.Count > 0))
                    {
                        this.labRemKHXKZ.Text = "请填写开户许可证号！";
                        this.labRemKHXKZ.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHXKZ.Visible = false;
                      
                        hashTableInfor["经纪人单位开户许可证号"] = this.txtKHXKZ.Text;
                        hashTableInfor["经纪人单位开户许扫描件"] = this.pan_SC_KHXKZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //预留印鉴卡
                    if (this.pan_SC_YLYJK.UpItem == null || this.pan_SC_YLYJK.UpItem.Items.Count <= 0)
                    {
                        this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                        this.labRemYLYJK.Visible = true;

                        tag += 1;
                    }
                    else if (this.pan_SC_YLYJK.UpItem != null && this.pan_SC_YLYJK.UpItem.Items.Count > 0)
                    {
                        this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                        this.labRemYLYJK.Visible = false;

                        hashTableInfor["经纪人单位预留印鉴卡扫描件"] = this.pan_SC_YLYJK.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //法定代表人姓名
                    if (this.txtFDDBR.Text.Trim() == "")
                    {
                        this.labRemFDDBR.Text = "请填写法定代表人姓名！";
                        this.labRemFDDBR.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        this.labRemFDDBR.Text = "";
                        this.labRemFDDBR.Visible = false;
                     
                        hashTableInfor["经纪人单位法定代表人姓名"] = this.txtFDDBR.Text;
                    }
                    //法定代表人身份证号扫描件
                    if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                    {
                        this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号，并上传正面扫描件！";
                        this.labRemFDDBRSHZH.Visible = true;
                     //   this.TipFRSFZ.Visible = false;
                      
                        tag += 1;
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                        {
                            this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                            this.labRemFDDBRSHZH.Visible = true;
                          //  this.TipFRSFZ.Visible = false;
                          
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemFDDBRSHZH.Text = strIdValueInfor;
                        //        this.labRemFDDBRSHZH.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            this.labRemFDDBRSHZH.Text = "请上传法定代表人身份证正面扫描件！";
                            this.labRemFDDBRSHZH.Visible = true;
                          //  this.TipFRSFZ.Visible = false;
                        
                            tag += 1;
                        }
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                    {
                        this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号！";
                        this.labRemFDDBRSHZH.Visible = true;
                      //  this.TipFRSFZ.Visible = false;
                      
                        tag += 1;
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                        {
                            this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                            this.labRemFDDBRSHZH.Visible = true;
                            //this.TipFRSFZ.Visible = false;
                          
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemFDDBRSHZH.Text = strIdValueInfor;
                        //        this.labRemFDDBRSHZH.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            this.labRemFDDBRSHZH.Visible = false;
                           // this.TipFRSFZ.Visible = true;
                        
                            hashTableInfor["经纪人单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                            hashTableInfor["经纪人单位法定代表人身份证扫描件"] = this.pan_SC_FDDBRSHZH.UpItem.Items[0].SubItems[1].Text.Trim();
                        }

                    }

                    if (this.pan_SC_FDDBRSHZH_FM.UpItem == null || this.pan_SC_FDDBRSHZH_FM.UpItem.Items.Count <= 0)
                    {
                        this.labRemFDDBRSHZH_FM.Visible = true;
                        tag += 1;
                    }
                    else
                    {

                        this.labRemFDDBRSHZH_FM.Visible = false;
                        hashTableInfor["经纪人单位法定代表人身份证反面扫描件"] = this.pan_SC_FDDBRSHZH_FM.UpItem.Items[0].SubItems[1].Text.Trim();
                    }

                    //法定带代表人授权书
                    if (this.pan_SC_FDDBRSQS.UpItem == null || this.pan_SC_FDDBRSQS.UpItem.Items.Count <= 0)
                    {
                        this.labRemFDDBRSQS.Text = "请上传法定代表人授权书扫描件！";
                        this.labRemFDDBRSQS.Visible = true;
                     
                        tag += 1;
                    }
                    else if (this.pan_SC_FDDBRSQS.UpItem != null && this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                    {
                        this.labRemFDDBRSQS.Text = "";
                        this.labRemFDDBRSQS.Visible = false;
                    
                        hashTableInfor["经纪人单位法定代表人授权书"] = this.pan_SC_FDDBRSQS.UpItem.Items[0].SubItems[1].Text.Trim();
                    }


                    if (this.txtJYFLXDH.Text.Trim() == "")
                    {
                        labRemJYFLXDH.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        labRemJYFLXDH.Visible = false;
                      
                        hashTableInfor["经纪人单位交易方联系电话"] = this.txtJYFLXDH.Text;
                    }

                    if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                    {
                        labRemSSQY.Visible = true;
                       
                        tag += 1;
                    }
                    else
                    {
                        labRemSSQY.Visible = false;
                      
                        hashTableInfor["经纪人单位所属省份"] = ucSSQY.SelectedItem[0];
                        hashTableInfor["经纪人单位所属地市"] = ucSSQY.SelectedItem[1];
                        hashTableInfor["经纪人单位所属区县"] = ucSSQY.SelectedItem[2];
                    }

                    if (this.txtXXDZ.Text == "")
                    {
                        this.labRemXXDZ.Visible = true;
                       
                        tag += 1;

                    }
                    else
                    {
                        this.labRemXXDZ.Visible = false;
                     
                        hashTableInfor["经纪人单位详细地址"] = this.txtXXDZ.Text;
                    }

                    if (this.txtLXRXM.Text == "")
                    {
                        this.labRemLXRXM.Visible = true;
                      
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRXM.Visible = false;
                       
                        hashTableInfor["经纪人单位联系人姓名"] = this.txtLXRXM.Text;
                    }

                    if (this.txtLXRSJH.Text == "")
                    {
                        this.labRemLXRSJH.Visible = true;
                       
                        tag += 1;
                    }
                    else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                    {
                        this.labRemLXRSJH.Visible = true;
                        this.labRemLXRSJH.Text = "请填写正确的联系人手机号！";
                     
                        tag += 1;

                    }
                    else
                    {
                        this.labRemLXRSJH.Visible = false;
                     
                        hashTableInfor["经纪人单位联系人手机号"] = this.txtLXRSJH.Text;
                    }
                    //if (this.txtKHYH.Text.Trim() == "")
                    //{
                    //    this.labRemKHYH.Visible = true;
                     
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemKHYH.Visible = false;
                     
                    //    hashTableInfor["经纪人单位开户银行"] = this.txtKHYH.Text;
                    //}
                    if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                    {
                        this.labRemKHYH.Visible = true;                       
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHYH.Visible = false;                       
                        hashTableInfor["经纪人单位开户银行"] = this.cbxKHYH.Text;
                    }



                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    this.labRemYHZH.Visible = true;
                       
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemYHZH.Visible = false;
                      
                        hashTableInfor["经纪人单位银行账号"] = this.txtYHZH.Text.Trim();
                    //}
                    //if (this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString() == "请选择业务管理部门")
                    //{
                    //    this.labRemGLJG.Visible = true;
                      
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemGLJG.Visible = false;
                       
                    //    hashTableInfor["经纪人单位平台管理机构"] = this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString();
                    //    if (hashTableInfor["经纪人单位平台管理机构"].ToString() == HTuser["原平台管理机构"].ToString())
                    //    {
                    //        hashTableInfor["经纪人单位是否更换平台管理机构"] = "否";
                    //    }
                    //    else
                    //    {
                    //        hashTableInfor["经纪人单位是否更换平台管理机构"] = "是";
                    //    }
                    //}


                    if (this.radFGS.Checked == true)//选择分公司
                    {
                        if (this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString() == "请选择业务管理部门")
                        {
                            this.labRemGLJG.Visible = true;
                            tag += 1;
                        }
                        else
                        {
                            this.labRemGLJG.Visible = false;
                            hashTableInfor["经纪人单位平台管理机构"] = this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString();
                            hashTableInfor["经纪人单位业务管理部门分类"] = "分公司";
                            if (hashTableInfor["经纪人单位平台管理机构"].ToString() == HTuser["原平台管理机构"].ToString())
                            {
                                hashTableInfor["经纪人单位是否更换平台管理机构"] = "否";
                            }
                            else
                            {
                                hashTableInfor["经纪人单位是否更换平台管理机构"] = "是";
                            }
                        }
                    }

                    if (this.radYWTZB.Checked == true)//平台总部
                    {
                        hashTableInfor["经纪人单位平台管理机构"] = "平台总部";
                        hashTableInfor["经纪人单位业务管理部门分类"] = "平台总部";
                        if (hashTableInfor["经纪人单位平台管理机构"].ToString() == HTuser["原平台管理机构"].ToString())
                        {
                            hashTableInfor["经纪人单位是否更换平台管理机构"] = "否";
                        }
                        else
                        {
                            hashTableInfor["经纪人单位是否更换平台管理机构"] = "是";
                        }
                    }

                    if (this.radGXTW.Checked == true)//高校团委
                    {
                        if (String.IsNullOrEmpty(this.labGXTW_ZhangHao.Text))
                        {
                            this.labRemGXTW.Visible = true;
                            tag += 1;
                        }
                        else
                        {
                            this.labRemGXTW.Visible = false;
                        }
                        //所在院系
                        if (String.IsNullOrEmpty(this.txtSZYX.Text))
                        {
                            this.labRemXZYX.Visible = true;
                            tag += 1;

                        }
                        else
                        {
                            this.labRemXZYX.Visible = false;
                            hashTableInfor["经纪人单位平台管理机构"] = this.labTWMC.Text.Trim();
                            hashTableInfor["经纪人单位院系编号"] = txtSZYX.Text.Trim();
                            hashTableInfor["经纪人单位业务管理部门分类"] = "高校团委";
                            if (hashTableInfor["经纪人单位平台管理机构"].ToString() == HTuser["原平台管理机构"].ToString())
                            {
                                hashTableInfor["经纪人单位是否更换平台管理机构"] = "否";
                            }
                            else
                            {
                                hashTableInfor["经纪人单位是否更换平台管理机构"] = "是";
                            }
                        }
                    }

                    if (tag == 0)
                    {
                        //禁用提交区域并开启进度
                        flowLayoutPanel1.Enabled = false;
                        PBload.Visible = true;
                        //提交账户信息
                        SRT_UpdateJYZHXX_Run();
                    }
                    else
                    {
                        ScrollToPoint();
                        return;
                    }

                }
                else if (this.radZRR.Checked == true)//自然人注册
                {
                    this.labRemJYZH.Visible = false;
                    this.labRemZCLB.Visible = false;

                    int tag = 0;


                    
                    hashTableInfor["经纪人个人交易账户类别"] = "";
                    hashTableInfor["经纪人个人交易注册类别"] = "";
                    hashTableInfor["经纪人个人交易方名称"] = "";
                    hashTableInfor["经纪人个人身份证号"] = "";
                    hashTableInfor["经纪人个人身份证扫描件"] = "";
                    hashTableInfor["经纪人个人身份证反面扫描件"] = "";
                    hashTableInfor["经纪人个人交易方联系电话"] = "";
                    hashTableInfor["经纪人个人所属省份"] = "";
                    hashTableInfor["经纪人个人所属地市"] = "";
                    hashTableInfor["经纪人个人所属区县"] = "";
                    hashTableInfor["经纪人个人详细地址"] = "";
                    hashTableInfor["经纪人个人联系人姓名"] = "";
                    hashTableInfor["经纪人个人联系人手机号"] = "";
                    hashTableInfor["经纪人个人开户银行"] = "";
                    hashTableInfor["经纪人个人银行账号"] = "";
                    hashTableInfor["经纪人个人平台管理机构"] = "";
                    hashTableInfor["经纪人个人登录邮箱"] = HTuser["DLYX"].ToString();
                    hashTableInfor["经纪人个人用户名"] = HTuser["YHM"].ToString();
                    hashTableInfor["经纪人个人登录_Number"] = HTuser["登录_Number"].ToString();
                    hashTableInfor["经纪人个人关联_Number"] = HTuser["关联_Number"].ToString();
                    hashTableInfor["经纪人个人经纪人角色编号"] = HTuser["原经纪人角色编号"].ToString();
                    hashTableInfor["经纪人个人买家角色编号"] = HTuser["原买家角色编号"].ToString();
                    hashTableInfor["经纪人个人交易账户类别"] = "经纪人交易账户";
                    hashTableInfor["经纪人个人交易注册类别"] = "自然人";
                    hashTableInfor["经纪人个人是否更换平台管理机构"] = "否";
                    hashTableInfor["经纪人个人是否更换注册类别"] = "否"; 
                    hashTableInfor["经纪人个人交易注册类别"] = "自然人";
                    hashTableInfor["方法类别"] = "经纪人个人";//这里是为了方便选择处理方法  
                    hashTableInfor["经纪人个人是否已被分公司审核通过"] = hashTableVirify["分公司是否审核通过"].ToString();
                    hashTableInfor["经纪人个人院系编号"] = "";
                    hashTableInfor["经纪人个人经纪人分类"] = "一般经纪人";
                    hashTableInfor["经纪人个人业务管理部门分类"] = "";

                    string JJRDWTag = "";//标记界面信息是否填写完整   完整、不完整

                    if (hashTableInfor["经纪人个人交易注册类别"].ToString() == HTuser["原注册类别"].ToString())
                    {
                        hashTableInfor["经纪人个人是否更换注册类别"] = "否";
                    }
                    else
                    {
                        hashTableInfor["经纪人个人是否更换注册类别"] = "是";
                    }



                    if (this.txtJJFMC.Text.Trim() == "")
                    {
                        this.labRemJJFMC.Text = "请输入自然人姓名！";
                        this.labRemJJFMC.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemJJFMC.Text = "请输入自然人姓名！";
                        this.labRemJJFMC.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人交易方名称"] = this.txtJJFMC.Text.Trim();

                    }
                    //身份证扫描件
                    if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                        this.labRemSFZ.Visible = true;
                        //this.TipsSFZZFM.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                        {
                            this.labRemSFZ.Text = "请填写正确的身份证号！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else if (!(Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim()) == "身份证有效"))
                        {
                            string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim());
                            if (strIdValueInfor == "身份证有效")
                            {

                            }
                            else
                            {
                                this.labRemSFZ.Text = strIdValueInfor;
                                this.labRemSFZ.Visible = true;
                                tag += 1;
                            }
                        }
                        else
                        {
                            this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                    }
                    else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                    {
                        this.labRemSFZ.Text = "请填写身份证号！";
                        this.labRemSFZ.Visible = true;
                        //this.TipsSFZZFM.Visible = false;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                        {
                            this.labRemSFZ.Text = "请填写正确的身份证号！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                            JJRDWTag = "不完整";
                            tag += 1;
                        }
                        else if (!(Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim()) == "身份证有效"))
                        {
                            string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim());
                            if (strIdValueInfor == "身份证有效")
                            {

                            }
                            else
                            {
                                this.labRemSFZ.Text = strIdValueInfor;
                                this.labRemSFZ.Visible = true;
                                tag += 1;
                            }
                        }
                        else
                        {
                            this.labRemSFZ.Visible = false;
                            //this.TipsSFZZFM.Visible = true;
                            JJRDWTag = "完整";
                            hashTableInfor["经纪人个人身份证号"] = this.txtSFZ.Text.Trim();
                            hashTableInfor["经纪人个人身份证扫描件"] = this.pan_SC_SFZ.UpItem.Items[0].SubItems[1].Text.Trim();

                        }
                    }
                    //身份证反面扫描件
                    if (this.pan_SC_SFZ_FM.UpItem == null || this.pan_SC_SFZ_FM.UpItem.Items.Count <= 0)
                    {
                        this.labRemSFZ_FM.Visible = true;
                        tag += 1;
                    }
                    else
                    {
                        this.labRemSFZ_FM.Visible = false;
                        hashTableInfor["经纪人个人身份证反面扫描件"] = this.pan_SC_SFZ_FM.UpItem.Items[0].SubItems[1].Text.Trim();
                    }


                    if (this.txtJYFLXDH.Text.Trim() == "")
                    {
                        labRemJYFLXDH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemJYFLXDH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人交易方联系电话"] = this.txtJYFLXDH.Text;
                    }
                    if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                    {
                        labRemSSQY.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        labRemSSQY.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人所属省份"] = ucSSQY.SelectedItem[0];
                        hashTableInfor["经纪人个人所属地市"] = ucSSQY.SelectedItem[1];
                        hashTableInfor["经纪人个人所属区县"] = ucSSQY.SelectedItem[2];
                    }

                    if (this.txtXXDZ.Text == "")
                    {
                        this.labRemXXDZ.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemXXDZ.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人详细地址"] = this.txtXXDZ.Text;
                    }

                    if (this.txtLXRXM.Text == "")
                    {
                        this.labRemLXRXM.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRXM.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人联系人姓名"] = this.txtLXRXM.Text;
                    }

                    if (this.txtLXRSJH.Text == "")
                    {
                        this.labRemLXRSJH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else if (!Support.ValStr.isPhone(this.txtLXRSJH.Text))
                    {
                        this.labRemLXRSJH.Visible = true;
                        this.labRemLXRSJH.Text = "请填写正确的联系人手机号！";
                        JJRDWTag = "不完整";
                        tag += 1;

                    }
                    else
                    {
                        this.labRemLXRSJH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人联系人手机号"] = this.txtLXRSJH.Text;
                    }
                    //if (this.txtKHYH.Text.Trim() == "")
                    //{
                    //    this.labRemKHYH.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemKHYH.Visible = false;
                    //    JJRDWTag = "完整";
                    //    hashTableInfor["经纪人个人开户银行"] = this.txtKHYH.Text;
                    //}
                    if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                    {
                        this.labRemKHYH.Visible = true;
                        JJRDWTag = "不完整";
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHYH.Visible = false;
                        JJRDWTag = "完整";
                        hashTableInfor["经纪人个人开户银行"] = this.cbxKHYH.Text;
                    }


                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    this.labRemYHZH.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemYHZH.Visible = false;
                    //    JJRDWTag = "完整";
                        hashTableInfor["经纪人个人银行账号"] = this.txtYHZH.Text.Trim();
                    //}
                    //if (this.cbxGLJG.Text == "请选择业务管理部门")
                    //{
                    //    this.labRemGLJG.Visible = true;
                    //    JJRDWTag = "不完整";
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemGLJG.Visible = false;
                    //    JJRDWTag = "完整";
                    //    hashTableInfor["经纪人个人平台管理机构"] = this.cbxGLJG.Text;
                    //    if (hashTableInfor["经纪人个人平台管理机构"].ToString() == HTuser["原平台管理机构"].ToString())
                    //    {
                    //        hashTableInfor["经纪人个人是否更换平台管理机构"] = "否";
                    //    }
                    //    else
                    //    {
                    //        hashTableInfor["经纪人个人是否更换平台管理机构"] = "是";
                    //    }
                    //}

                        if (this.radFGS.Checked == true)//选择分公司
                        {
                            if (this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString() == "请选择业务管理部门")
                            {
                                this.labRemGLJG.Visible = true;
                                tag += 1;
                            }
                            else
                            {
                                this.labRemGLJG.Visible = false;
                                hashTableInfor["经纪人个人平台管理机构"] = this.cbxGLJG.Items[this.cbxGLJG.SelectedIndex].ToString();
                                hashTableInfor["经纪人个人业务管理部门分类"] = "分公司";
                                if (hashTableInfor["经纪人个人平台管理机构"].ToString() == HTuser["原平台管理机构"].ToString())
                                {
                                    hashTableInfor["经纪人个人是否更换平台管理机构"] = "否";
                                }
                                else
                                {
                                    hashTableInfor["经纪人个人是否更换平台管理机构"] = "是";
                                }
                            }
                        }

                        if (this.radYWTZB.Checked == true)//平台总部
                        {
                            hashTableInfor["经纪人个人平台管理机构"] = "平台总部";
                            hashTableInfor["经纪人个人业务管理部门分类"] = "平台总部";
                            if (hashTableInfor["经纪人个人平台管理机构"].ToString() == HTuser["原平台管理机构"].ToString())
                            {
                                hashTableInfor["经纪人个人是否更换平台管理机构"] = "否";
                            }
                            else
                            {
                                hashTableInfor["经纪人个人是否更换平台管理机构"] = "是";
                            }
                        }

                        if (this.radGXTW.Checked == true)//高校团委
                        {
                            if (String.IsNullOrEmpty(this.labGXTW_ZhangHao.Text))
                            {
                                this.labRemGXTW.Visible = true;
                                tag += 1;
                            }
                            else
                            {
                                this.labRemGXTW.Visible = false;
                            }
                            //所在院系
                            if (String.IsNullOrEmpty(this.txtSZYX.Text))
                            {
                                this.labRemXZYX.Visible = true;
                                tag += 1;

                            }
                            else
                            {
                                this.labRemXZYX.Visible = false;
                                hashTableInfor["经纪人个人平台管理机构"] = this.labTWMC.Text.Trim();
                                hashTableInfor["经纪人个人院系编号"] = txtSZYX.Text.Trim();
                                hashTableInfor["经纪人个人业务管理部门分类"] = "高校团委";
                                if (hashTableInfor["经纪人个人平台管理机构"].ToString() == HTuser["原平台管理机构"].ToString())
                                {
                                    hashTableInfor["经纪人个人是否更换平台管理机构"] = "否";
                                }
                                else
                                {
                                    hashTableInfor["经纪人个人是否更换平台管理机构"] = "是";
                                }
                            }
                        }
                    if (tag == 0)
                    {
                        //禁用提交区域并开启进度
                        flowLayoutPanel1.Enabled = false;
                        PBload.Visible = true;

                        //提交账户信息
                        SRT_UpdateJYZHXX_Run();
                    }
                    else
                    {
                        ScrollToPoint();
                        return;
                    }

                }

            }
            #endregion
            #region//买卖家交易账户
            else if (this.radMMJ.Checked == true)//买卖家交易账户
            {
                if (this.radFW_JJR.Checked == true || this.radFW_Bank.Checked == true)//一般经纪人
                {
                    bool b= IsValidZSBH();
                    if (!b)
                        return;
                }
                #region//单位注册
                if (this.radDW.Checked == true)
                {
                    int tag = 0;
                    hashTableInfor["买卖家单位交易账户类别"] = "";
                    hashTableInfor["买卖家单位交易注册类别"] = "";
                    hashTableInfor["买卖家单位交易方名称"] = "";
                    hashTableInfor["买卖家单位营业执照注册号"] = "";
                    hashTableInfor["买卖家单位营业执照扫描件"] = "";
                    hashTableInfor["买卖家单位组织机构代码证代码证"] = "";
                    hashTableInfor["买卖家单位组织机构代码证代码证扫描件"] = "";
                    hashTableInfor["买卖家单位税务登记证税号"] = "";
                    hashTableInfor["买卖家单位税务登记证扫描件"] = "";
                    hashTableInfor["买卖家单位一般纳税人资格证扫描件"] = "";
                    hashTableInfor["买卖家单位开户许可证号"] = "";
                    hashTableInfor["买卖家单位开户许扫描件"] = "";
                    hashTableInfor["买卖家单位预留印鉴卡扫描件"] = "";
                    hashTableInfor["买卖家单位法定代表人姓名"] = "";
                    hashTableInfor["买卖家单位法定代表人身份证号"] = "";
                    hashTableInfor["买卖家单位法定代表人身份证扫描件"] = "";
                    hashTableInfor["买卖家单位法定代表人身份证反面扫描件"] = "";
                    hashTableInfor["买卖家单位法定代表人授权书"] = "";
                    hashTableInfor["买卖家单位交易方联系电话"] = "";
                    hashTableInfor["买卖家单位所属省份"] = "";
                    hashTableInfor["买卖家单位所属地市"] = "";
                    hashTableInfor["买卖家单位所属区县"] = "";
                    hashTableInfor["买卖家单位详细地址"] = "";
                    hashTableInfor["买卖家单位联系人姓名"] = "";
                    hashTableInfor["买卖家单位联系人手机号"] = "";
                    hashTableInfor["买卖家单位开户银行"] = "";
                    hashTableInfor["买卖家单位银行账号"] = "";
                    hashTableInfor["买卖家单位经纪人资格证书编号"] = "";
                    hashTableInfor["买卖家单位经纪人经纪人名称"] = "";
                    hashTableInfor["买卖家单位经纪人联系电话"] = "";
                    hashTableInfor["买卖家单位登录邮箱"] = HTuser["DLYX"].ToString();
                    hashTableInfor["买卖家单位用户名"] = HTuser["YHM"].ToString();
                    hashTableInfor["买卖家单位登录_Number"] = HTuser["登录_Number"].ToString();
                    hashTableInfor["买卖家单位关联_Number"] = HTuser["关联_Number"].ToString();
                    hashTableInfor["买卖家单位卖家角色编号"] = HTuser["原卖家角色编号"].ToString();
                    hashTableInfor["买卖家单位买家角色编号"] = HTuser["原买家角色编号"].ToString();
                    hashTableInfor["买卖家单位交易账户类别"] = "买家卖家交易账户";
                    hashTableInfor["买卖家单位交易注册类别"] = "单位";
                    hashTableInfor["买卖家单位是否更换注册类别"] = "否";
                    hashTableInfor["买卖家单位是否更换经纪人资格证书编号"] = "否";
                    hashTableInfor["方法类别"] = "买卖家单位";//这里是为了方便选择处理方法
                    hashTableInfor["买卖家单位是否已被经纪人审核通过"] = hashTableVirify["经纪人是否审核通过"].ToString();
                    hashTableInfor["买卖家单位是否已被分公司审核通过"] = hashTableVirify["分公司是否审核通过"].ToString();

                    hashTableInfor["买卖家单位证券资金密码"] = "";
                    hashTableInfor["买卖家单位银行工作人员工号"] = "";
                    hashTableInfor["买卖家单位关联银行"] = "";
                    hashTableInfor["买卖家单位业务服务部门"] = "";
                    hashTableInfor["买卖家单位买家卖家与经纪人关联表Number"] = "";

                    if (hashTableInfor["买卖家单位交易注册类别"].ToString() == HTuser["原注册类别"].ToString())
                    {
                        hashTableInfor["买卖家单位是否更换注册类别"] = "否";
                    }
                    else
                    {
                        hashTableInfor["买卖家单位是否更换注册类别"] = "是";
                    }

                    if (lblMMJGLB_Number.Text.Trim() != "记录关联表Number" && lblMMJGLB_Number.Text.Trim()!="")
                    {
                        hashTableInfor["买卖家单位买家卖家与经纪人关联表Number"] = lblMMJGLB_Number.Text.Trim();
                    }
                

                    if (this.txtJJFMC.Text.Trim() == "")
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = false;
                   
                        hashTableInfor["买卖家单位交易方名称"] = this.txtJJFMC.Text.Trim();

                    }
                    //营业执照
                    if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemYYZZ.Text = "请填写营业执照注册号，并上传扫描件！";
                        this.labRemYYZZ.Visible = true;
                      
                        tag += 1;
                    }
                    else if (this.txtYYZZ.Text.Trim() != "" && (this.pan_SC_YYZZ.UpItem == null || this.pan_SC_YYZZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemYYZZ.Text = "请上传营业执照扫描件！";
                        this.labRemYYZZ.Visible = true;
                      
                        tag += 1;
                    }
                    else if (this.txtYYZZ.Text.Trim() == "" && (this.pan_SC_YYZZ.UpItem != null && this.pan_SC_YYZZ.UpItem.Items.Count > 0))
                    {
                        this.labRemYYZZ.Text = "请填写营业执照注册号！";
                        this.labRemYYZZ.Visible = true;
                 
                        tag += 1;
                    }
                    else
                    {
                        this.labRemYYZZ.Visible = false;
                   
                        hashTableInfor["买卖家单位营业执照注册号"] = this.txtYYZZ.Text;
                        hashTableInfor["买卖家单位营业执照扫描件"] = this.pan_SC_YYZZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //组织机构代码证
                    if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码，并上传扫描件！";
                        this.labRemZZJGDMZ.Visible = true;
                      
                        tag += 1;
                    }
                    else if (this.txtZZJGDMZ.Text.Trim() != "" && (this.pan_SC_ZZJGDMZ.UpItem == null || this.pan_SC_ZZJGDMZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemZZJGDMZ.Text = "请上传组织机构代码证扫描件！";
                        this.labRemZZJGDMZ.Visible = true;
                   
                        tag += 1;
                    }
                    else if (this.txtZZJGDMZ.Text.Trim() == "" && (this.pan_SC_ZZJGDMZ.UpItem != null && this.pan_SC_ZZJGDMZ.UpItem.Items.Count > 0))
                    {
                        this.labRemZZJGDMZ.Text = "请填写组织机构代码证代码！";
                        this.labRemZZJGDMZ.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        this.labRemZZJGDMZ.Visible = false;
                     
                        hashTableInfor["买卖家单位组织机构代码证代码证"] = this.txtZZJGDMZ.Text;
                        hashTableInfor["买卖家单位组织机构代码证代码证扫描件"] = this.pan_SC_ZZJGDMZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //税务登记证
                    if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSWDJZ.Text = "请填写税务登记证税号，并上传扫描件！";
                        this.labRemSWDJZ.Visible = true;
                   
                        tag += 1;
                    }
                    else if (this.txtSWDJZ.Text.Trim() != "" && (this.pan_SC_SWDJZ.UpItem == null || this.pan_SC_SWDJZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSWDJZ.Text = "请上传税务登记证扫描件！";
                        this.labRemSWDJZ.Visible = true;
                    
                        tag += 1;
                    }
                    else if (this.txtSWDJZ.Text.Trim() == "" && (this.pan_SC_SWDJZ.UpItem != null && this.pan_SC_SWDJZ.UpItem.Items.Count > 0))
                    {
                        this.labRemSWDJZ.Text = "请填写税务登记证号！";
                        this.labRemSWDJZ.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        this.labRemSWDJZ.Visible = false;
                     
                        hashTableInfor["买卖家单位税务登记证税号"] = this.txtSWDJZ.Text;
                        hashTableInfor["买卖家单位税务登记证扫描件"] = this.pan_SC_SWDJZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //纳税人资格证
                    if (this.pan_SC_NSRZGZ.UpItem == null || this.pan_SC_NSRZGZ.UpItem.Items.Count <= 0)
                    {
                        this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                        this.labRemNSRZGZ.Visible = true;
                    
                        tag += 1;
                    }
                    else if (this.pan_SC_NSRZGZ.UpItem != null && this.pan_SC_NSRZGZ.UpItem.Items.Count > 0)
                    {
                        this.labRemNSRZGZ.Text = "请上传一般纳税人资格证明扫描件！";
                        this.labRemNSRZGZ.Visible = false;
                     
                        hashTableInfor["买卖家单位一般纳税人资格证扫描件"] = this.pan_SC_NSRZGZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //开户许可证
                    if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemKHXKZ.Text = "请填写开户许可证号，并上传开户许可证扫描件！";
                        this.labRemKHXKZ.Visible = true;
                     
                        tag += 1;
                    }
                    else if (this.txtKHXKZ.Text.Trim() != "" && (this.pan_SC_KHXKZ.UpItem == null || this.pan_SC_KHXKZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemKHXKZ.Text = "请上传开户许可证扫描件！";
                        this.labRemKHXKZ.Visible = true;
                      
                        tag += 1;
                    }
                    else if (this.txtKHXKZ.Text.Trim() == "" && (this.pan_SC_KHXKZ.UpItem != null && this.pan_SC_KHXKZ.UpItem.Items.Count > 0))
                    {
                        this.labRemKHXKZ.Text = "请填写开户许可证号！";
                        this.labRemKHXKZ.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHXKZ.Visible = false;
                    
                        hashTableInfor["买卖家单位开户许可证号"] = this.txtKHXKZ.Text;
                        hashTableInfor["买卖家单位开户许扫描件"] = this.pan_SC_KHXKZ.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //预留印鉴卡扫描件
                    if (this.pan_SC_YLYJK.UpItem == null || this.pan_SC_YLYJK.UpItem.Items.Count <= 0)
                    {
                        this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                        this.labRemYLYJK.Visible = true;

                        tag += 1;
                    }
                    else if (this.pan_SC_YLYJK.UpItem != null && this.pan_SC_YLYJK.UpItem.Items.Count > 0)
                    {
                        this.labRemYLYJK.Text = "请上传预留印鉴表扫描件！";
                        this.labRemYLYJK.Visible = false;

                        hashTableInfor["买卖家单位预留印鉴卡扫描件"] = this.pan_SC_YLYJK.UpItem.Items[0].SubItems[1].Text.Trim();
                    }
                    //法定代表人姓名
                    if (this.txtFDDBR.Text.Trim() == "")
                    {
                        this.labRemFDDBR.Text = "请填写法定代表人姓名！";
                        this.labRemFDDBR.Visible = true;
                    
                        tag += 1;
                    }
                    else
                    {
                        this.labRemFDDBR.Text = "";
                        this.labRemFDDBR.Visible = false;
                      
                        hashTableInfor["买卖家单位法定代表人姓名"] = this.txtFDDBR.Text;
                    }
                    //法定代表人身份证号扫描件
                    if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                    {
                        this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号，并上传正面扫描件！";
                        this.labRemFDDBRSHZH.Visible = true;
                        //this.TipFRSFZ.Visible = false;
                       
                        tag += 1;
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem == null || this.pan_SC_FDDBRSHZH.UpItem.Items.Count <= 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                        {
                            this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                            this.labRemFDDBRSHZH.Visible = true;
                            //this.TipFRSFZ.Visible = false;
                         
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemFDDBRSHZH.Text = strIdValueInfor;
                        //        this.labRemFDDBRSHZH.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            this.labRemFDDBRSHZH.Text = "请上传法定代表人身份证正面扫描件！";
                            this.labRemFDDBRSHZH.Visible = true;
                            //this.TipFRSFZ.Visible = false;
                          
                            tag += 1;
                        }
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() == "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                    {
                        this.labRemFDDBRSHZH.Text = "请填写法定代表人身份证号！";
                        this.labRemFDDBRSHZH.Visible = true;
                        //this.TipFRSFZ.Visible = false;
                      
                        tag += 1;
                    }
                    else if (this.txtFDDBRSHZH.Text.Trim() != "" && (this.pan_SC_FDDBRSHZH.UpItem != null && this.pan_SC_FDDBRSHZH.UpItem.Items.Count > 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtFDDBRSHZH.Text.Trim()))
                        {
                            this.labRemFDDBRSHZH.Text = "请填写正确的法定代表人身份证号！";
                            this.labRemFDDBRSHZH.Visible = true;
                            //this.TipFRSFZ.Visible = false;

                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtFDDBRSHZH.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemFDDBRSHZH.Text = strIdValueInfor;
                        //        this.labRemFDDBRSHZH.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            this.labRemFDDBRSHZH.Visible = false;
                            //this.TipFRSFZ.Visible = true;
                         
                            hashTableInfor["买卖家单位法定代表人身份证号"] = this.txtFDDBRSHZH.Text;
                            hashTableInfor["买卖家单位法定代表人身份证扫描件"] = this.pan_SC_FDDBRSHZH.UpItem.Items[0].SubItems[1].Text.Trim();
                        }
                    }
                    if (this.pan_SC_FDDBRSHZH_FM.UpItem == null || this.pan_SC_FDDBRSHZH_FM.UpItem.Items.Count <= 0)
                    {
                        this.labRemFDDBRSHZH_FM.Visible = true;
                        tag += 1;
                    }
                    else
                    {

                        this.labRemFDDBRSHZH_FM.Visible = false;
                        hashTableInfor["买卖家单位法定代表人身份证反面扫描件"] = this.pan_SC_FDDBRSHZH_FM.UpItem.Items[0].SubItems[1].Text.Trim();
                    }

                    //法定带代表人授权书
                    if (this.pan_SC_FDDBRSQS.UpItem == null || this.pan_SC_FDDBRSQS.UpItem.Items.Count <= 0)
                    {
                        this.labRemFDDBRSQS.Text = "请上传法定代表人授权书扫描件！";
                        this.labRemFDDBRSQS.Visible = true;
                    
                        tag += 1;
                    }
                    else if (this.pan_SC_FDDBRSQS.UpItem != null && this.pan_SC_FDDBRSQS.UpItem.Items.Count > 0)
                    {
                        this.labRemFDDBRSQS.Text = "";
                        this.labRemFDDBRSQS.Visible = false;
                   
                        hashTableInfor["买卖家单位法定代表人授权书"] = this.pan_SC_FDDBRSQS.UpItem.Items[0].SubItems[1].Text.Trim();
                    }


                    if (this.txtJYFLXDH.Text.Trim() == "")
                    {
                        labRemJYFLXDH.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        labRemJYFLXDH.Visible = false;
                    
                        hashTableInfor["买卖家单位交易方联系电话"] = this.txtJYFLXDH.Text;
                    }

                    if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                    {
                        labRemSSQY.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        labRemSSQY.Visible = false;
                    
                        hashTableInfor["买卖家单位所属省份"] = ucSSQY.SelectedItem[0];
                        hashTableInfor["买卖家单位所属地市"] = ucSSQY.SelectedItem[1];
                        hashTableInfor["买卖家单位所属区县"] = ucSSQY.SelectedItem[2];
                    }

                    if (this.txtXXDZ.Text == "")
                    {
                        this.labRemXXDZ.Visible = true;
                      
                        tag += 1;

                    }
                    else
                    {
                        this.labRemXXDZ.Visible = false;
                     
                        hashTableInfor["买卖家单位详细地址"] = this.txtXXDZ.Text;
                    }

                    if (this.txtLXRXM.Text == "")
                    {
                        this.labRemLXRXM.Visible = true;
                     
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRXM.Visible = false;
                   
                        hashTableInfor["买卖家单位联系人姓名"] = this.txtLXRXM.Text;
                    }

                    if (this.txtLXRSJH.Text == "")
                    {
                        this.labRemLXRSJH.Visible = true;
                    
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRSJH.Visible = false;
                     
                        hashTableInfor["买卖家单位联系人手机号"] = this.txtLXRSJH.Text;
                    }
                    //if (this.txtKHYH.Text.Trim() == "")
                    //{
                    //    this.labRemKHYH.Visible = true;
                      
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemKHYH.Visible = false;
                     
                    //    hashTableInfor["买卖家单位开户银行"] = this.txtKHYH.Text;
                    //}

                    if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                    {
                        this.labRemKHYH.Visible = true;
                        
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHYH.Visible = false;
                        
                        hashTableInfor["买卖家单位开户银行"] = this.cbxKHYH.Text;
                    }

                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    this.labRemYHZH.Visible = true;
                    
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemYHZH.Visible = false;
                      
                        hashTableInfor["买卖家单位银行账号"] = this.txtYHZH.Text.Trim();
                    //}
                        if (this.radFW_JJR.Checked == true)//选择一般经纪人
                        {


                            if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                            {
                                this.lablRemJJRZGZS.Visible = true;

                                tag += 1;
                            }
                            else
                            {
                                this.lablRemJJRZGZS.Visible = false;

                            }

                            if (this.txtJJRZGZS.Text.Trim() == "")
                            {
                                this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                                this.lablRemJJRZGZS.Visible = true;

                                tag += 1;
                            }
                            else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                            {
                                this.lablRemJJRZGZS.Visible = false;

                                hashTableInfor["买卖家单位经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                                hashTableInfor["买卖家单位经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                                hashTableInfor["买卖家单位经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                                hashTableInfor["买卖家单位业务服务部门"] = lblJJRFL.Text.Trim();
                                if (hashTableInfor["买卖家单位经纪人资格证书编号"].ToString() == HTuser["原经纪人资格证书编号"].ToString())
                                {
                                    hashTableInfor["买卖家单位是否更换经纪人资格证书编号"] = "否";
                                }
                                else
                                {
                                    hashTableInfor["买卖家单位是否更换经纪人资格证书编号"] = "是";
                                }
                            }
                            else
                            {
                                this.lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                this.lablRemJJRZGZS.Visible = true;

                                tag += 1;

                            }
                        }
                        else if (this.radFW_Bank.Checked == true)
                        {
                            if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                            {
                                tag += 1;
                                this.labRemYHMC.Visible = true;
                            }
                            else
                            {
                                hashTableInfor["买卖家单位关联银行"] = this.labYHMC.Text.Trim();
                                this.labRemYHMC.Visible = false;
                            }
                            if (uctextBankYGGH.Enabled == false)
                            {
                                hashTableInfor["买卖家单位银行工作人员工号"] = "无";
                                this.lblBankYGGH.Visible = false;
                            }
                            else
                            {
                                if (this.uctextBankYGGH.Text.Trim() == "")
                                {
                                    tag += 1;
                                    this.lblBankYGGH.Visible = true;
                                }
                                else
                                {
                                    hashTableInfor["买卖家单位银行工作人员工号"] = this.uctextBankYGGH.Text.Trim();
                                    this.lblBankYGGH.Visible = false;
                                }
                            }
                            if (this.txtJJRZGZS.Text.Trim() == "")
                            {
                                //this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                                //this.lablRemJJRZGZS.Visible = true;
                                //tag += 1;
                            }
                            else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                            {
                                this.lablRemJJRZGZS.Visible = false;
                                hashTableInfor["买卖家单位经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                                hashTableInfor["买卖家单位经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                                hashTableInfor["买卖家单位经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                                hashTableInfor["买卖家单位业务服务部门"] = lblJJRFL.Text.Trim();
                                if (hashTableInfor["买卖家单位经纪人资格证书编号"].ToString() == HTuser["原经纪人资格证书编号"].ToString())
                                {
                                    hashTableInfor["买卖家单位是否更换经纪人资格证书编号"] = "否";
                                }
                                else
                                {
                                    hashTableInfor["买卖家单位是否更换经纪人资格证书编号"] = "是";
                                }
                            }
                            else
                            {
                                //this.lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                //this.lablRemJJRZGZS.Visible = true;
                                //tag += 1;

                            }
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                            {
                                tag += 1;
                                this.labRemYHMC.Visible = true;
                            }
                            else
                            {
                                this.labRemYHMC.Visible = false;
                            }

                            if (String.IsNullOrEmpty(this.labZFMC.Text))
                            {
                                tag += 1;
                                this.labRemZFMC.Visible = true;
                            }
                            else
                            {
                                this.labRemZFMC.Visible = false;
                            }

                            if (String.IsNullOrEmpty(this.labHangYeXieHui.Text))
                            {
                                tag += 1;
                                this.labRemHangYeXieHui.Visible = true;
                            }
                            else
                            {
                                this.labRemHangYeXieHui.Visible = false;
                            }

                            if (this.txtJJRZGZS.Text.Trim() == "")
                            {
                                this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                                this.lablRemJJRZGZS.Visible = true;
                             
                                tag += 1;
                            }
                            else
                            {
                                this.lablRemJJRZGZS.Text = "您填写的经纪人资格证书编号无效！";
                                tag += 1;
                                this.lablRemJJRZGZS.Visible = true;
                            }

                        }
                    if (tag == 0)
                    {
                        //禁用提交区域并开启进度
                        flowLayoutPanel1.Enabled = false;
                        PBload.Visible = true;
                        //提交账户信息
                        SRT_UpdateJYZHXX_Run();
                    }
                    else
                    {
                        ScrollToPoint();
                        return;
                    }



                }
                #endregion
                #region//自然人注册
                else if (this.radZRR.Checked == true)
                {
                    int tag = 0;
                    hashTableInfor["买卖家个人交易账户类别"] = "";
                    hashTableInfor["买卖家个人交易注册类别"] = "";
                    hashTableInfor["买卖家个人交易方名称"] = "";
                    hashTableInfor["买卖家个人身份证号"] = "";
                    hashTableInfor["买卖家个人身份证扫描件"] = "";
                    hashTableInfor["买卖家个人身份证反面扫描件"] = "";
                    hashTableInfor["买卖家个人交易方联系电话"] = "";
                    hashTableInfor["买卖家个人所属省份"] = "";
                    hashTableInfor["买卖家个人所属地市"] = "";
                    hashTableInfor["买卖家个人所属区县"] = "";
                    hashTableInfor["买卖家个人详细地址"] = "";
                    hashTableInfor["买卖家个人联系人姓名"] = "";
                    hashTableInfor["买卖家个人联系人手机号"] = "";
                    hashTableInfor["买卖家个人开户银行"] = "";
                    hashTableInfor["买卖家个人银行账号"] = "";
                    hashTableInfor["买卖家个人经纪人资格证书编号"] = "";
                    hashTableInfor["买卖家个人经纪人经纪人名称"] = "";
                    hashTableInfor["买卖家个人经纪人联系电话"] = "";
                    hashTableInfor["买卖家个人登录邮箱"] = HTuser["DLYX"].ToString();
                    hashTableInfor["买卖家个人用户名"] = HTuser["YHM"].ToString();
                    hashTableInfor["买卖家个人登录_Number"] = HTuser["登录_Number"].ToString();
                    hashTableInfor["买卖家个人关联_Number"] = HTuser["关联_Number"].ToString();
                    hashTableInfor["买卖家个人卖家角色编号"] = HTuser["原卖家角色编号"].ToString();
                    hashTableInfor["买卖家个人买家角色编号"] = HTuser["原买家角色编号"].ToString();
                    hashTableInfor["买卖家个人交易账户类别"] = "买家卖家交易账户";
                    hashTableInfor["买卖家个人交易注册类别"] = "自然人";
                    hashTableInfor["买卖家个人是否更换注册类别"] = "否";
                    hashTableInfor["买卖家个人是否更换经纪人资格证书编号"] = "否";
                    hashTableInfor["方法类别"] = "买卖家个人";//这里是为了方便选择处理方法
                    hashTableInfor["买卖家个人是否已被经纪人审核通过"] = hashTableVirify["经纪人是否审核通过"].ToString();
                    hashTableInfor["买卖家个人是否已被分公司审核通过"] = hashTableVirify["分公司是否审核通过"].ToString();

                    hashTableInfor["买卖家个人证券资金密码"] = "";
                    hashTableInfor["买卖家个人关联银行工作人员工号"] = "";
                    hashTableInfor["买卖家个人关联银行"] = "";
                    hashTableInfor["买卖家个人业务服务部门"] = "";
                    hashTableInfor["买卖家个人买家卖家与经纪人关联表Number"] = "";

                    if (hashTableInfor["买卖家个人交易注册类别"].ToString() == HTuser["原注册类别"].ToString())
                    {
                        hashTableInfor["买卖家个人是否更换注册类别"] = "否";
                    }
                    else
                    {
                        hashTableInfor["买卖家个人是否更换注册类别"] = "是";
                    }

                    if (lblMMJGLB_Number.Text.Trim() != "记录关联表Number" && lblMMJGLB_Number.Text.Trim() != "")
                    {
                        hashTableInfor["买卖家个人买家卖家与经纪人关联表Number"] = lblMMJGLB_Number.Text.Trim();
                    }


                    if (this.txtJJFMC.Text.Trim() == "")
                    {
                        this.labRemJJFMC.Text = "请输入真实的自然人姓名！";
                        this.labRemJJFMC.Visible = true;
                      
                        tag += 1;
                    }
                    else
                    {
                        this.labRemJJFMC.Text = "请输入真实的单位名称！";
                        this.labRemJJFMC.Visible = false;
                     
                        hashTableInfor["买卖家个人交易方名称"] = this.txtJJFMC.Text.Trim();

                    }
                    //身份证扫描件
                    if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                    {
                        this.labRemSFZ.Text = "请填写身份证号，并上传身份证正面扫描件！";
                        this.labRemSFZ.Visible = true;
                        //this.TipsSFZZFM.Visible = false;
                      
                        tag += 1;
                    }
                    else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem == null || this.pan_SC_SFZ.UpItem.Items.Count <= 0))
                    {

                        if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                        {
                            this.labRemSFZ.Text = "请填写正确的身份证号！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                          
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemSFZ.Text = strIdValueInfor;
                        //        this.labRemSFZ.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            this.labRemSFZ.Text = "请上传身份证正面扫描件！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                            tag += 1;
                        }
                    }
                    else if (this.txtSFZ.Text.Trim() == "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                    {
                        this.labRemSFZ.Text = "请填写身份证号！";
                        this.labRemSFZ.Visible = true;
                        //this.TipsSFZZFM.Visible = false;
                        tag += 1;
                    }
                    else if (this.txtSFZ.Text.Trim() != "" && (this.pan_SC_SFZ.UpItem != null && this.pan_SC_SFZ.UpItem.Items.Count > 0))
                    {
                        if (!Support.ValStr.isIDCard(this.txtSFZ.Text.Trim()))
                        {
                            this.labRemSFZ.Text = "请填写正确的身份证号！";
                            this.labRemSFZ.Visible = true;
                            //this.TipsSFZZFM.Visible = false;
                            tag += 1;
                        }
                        //else if (!(Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim()) == "身份证有效"))
                        //{
                        //    string strIdValueInfor = Support.ValStr.isTrueIDCard(this.txtSFZ.Text.Trim());
                        //    if (strIdValueInfor == "身份证有效")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        this.labRemSFZ.Text = strIdValueInfor;
                        //        this.labRemSFZ.Visible = true;
                        //        tag += 1;
                        //    }
                        //}
                        else
                        {
                            this.labRemSFZ.Visible = false;
                            //this.TipsSFZZFM.Visible = true;
                          
                            hashTableInfor["买卖家个人身份证号"] = this.txtSFZ.Text.Trim();
                            hashTableInfor["买卖家个人身份证扫描件"] = this.pan_SC_SFZ.UpItem.Items[0].SubItems[1].Text.Trim();
                        }
                    }

                    //身份证反面扫描件
                    if (this.pan_SC_SFZ_FM.UpItem == null || this.pan_SC_SFZ_FM.UpItem.Items.Count <= 0)
                    {
                        this.labRemSFZ_FM.Visible = true;
                        tag += 1;
                    }
                    else
                    {
                        this.labRemSFZ_FM.Visible = false;
                        hashTableInfor["买卖家个人身份证反面扫描件"] = this.pan_SC_SFZ_FM.UpItem.Items[0].SubItems[1].Text.Trim();
                    }



                    if (this.txtJYFLXDH.Text.Trim() == "")
                    {
                        labRemJYFLXDH.Visible = true;
                       
                        tag += 1;
                    }
                    else
                    {
                        labRemJYFLXDH.Visible = false;
                      
                        hashTableInfor["买卖家个人交易方联系电话"] = this.txtJYFLXDH.Text;
                    }

                    if (ucSSQY.SelectedItem[0].ToString().Contains("请选择") || ucSSQY.SelectedItem[1].ToString().Contains("请选择") || ucSSQY.SelectedItem[2].ToString().Contains("请选择"))
                    {
                        labRemSSQY.Visible = true;
                       
                        tag += 1;
                    }
                    else
                    {
                        labRemSSQY.Visible = false;
                      
                        hashTableInfor["买卖家个人所属省份"] = ucSSQY.SelectedItem[0];
                        hashTableInfor["买卖家个人所属地市"] = ucSSQY.SelectedItem[1];
                        hashTableInfor["买卖家个人所属区县"] = ucSSQY.SelectedItem[2];
                    }

                    if (this.txtXXDZ.Text == "")
                    {
                        this.labRemXXDZ.Visible = true;
                       
                        tag += 1;

                    }
                    else
                    {
                        this.labRemXXDZ.Visible = false;
                      
                        hashTableInfor["买卖家个人详细地址"] = this.txtXXDZ.Text;
                    }

                    if (this.txtLXRXM.Text == "")
                    {
                        this.labRemLXRXM.Visible = true;
                      
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRXM.Visible = false;
                      
                        hashTableInfor["买卖家个人联系人姓名"] = this.txtLXRXM.Text;
                    }

                    if (this.txtLXRSJH.Text == "")
                    {
                        this.labRemLXRSJH.Visible = true;
                      
                        tag += 1;
                    }
                    else
                    {
                        this.labRemLXRSJH.Visible = false;
                     
                        hashTableInfor["买卖家个人联系人手机号"] = this.txtLXRSJH.Text;
                    }
                    //if (this.txtKHYH.Text.Trim() == "")
                    //{
                    //    this.labRemKHYH.Visible = true;
                    
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemKHYH.Visible = false;
                      
                    //    hashTableInfor["买卖家个人开户银行"] = this.txtKHYH.Text;
                    //}

                    if (this.cbxKHYH.Text.Trim() == "请选择预指定开户银行")
                    {
                        this.labRemKHYH.Visible = true;
                       
                        tag += 1;
                    }
                    else
                    {
                        this.labRemKHYH.Visible = false;
                       
                        hashTableInfor["买卖家个人开户银行"] = this.cbxKHYH.Text;
                    }
                    //if (this.txtYHZH.Text.Trim() == "")
                    //{
                    //    this.labRemYHZH.Visible = true;
                     
                    //    tag += 1;
                    //}
                    //else
                    //{
                    //    this.labRemYHZH.Visible = false;
                      
                        hashTableInfor["买卖家个人银行账号"] = this.txtYHZH.Text.Trim();
                    //}

                        if (this.radFW_JJR.Checked == true)
                        {
                            if (this.txtJJRZGZS.Text.Trim() == "")//经纪人资格证书
                            {
                                this.lablRemJJRZGZS.Visible = true;

                                tag += 1;
                            }
                            else
                            {
                                this.lablRemJJRZGZS.Visible = false;

                            }

                            if (this.txtJJRZGZS.Text.Trim() == "")
                            {
                                this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                                this.lablRemJJRZGZS.Visible = true;

                                tag += 1;
                            }
                            else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                            {
                                this.lablRemJJRZGZS.Visible = false;

                                hashTableInfor["买卖家个人经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                                hashTableInfor["买卖家个人经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                                hashTableInfor["买卖家个人经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                                hashTableInfor["买卖家个人业务服务部门"] = lblJJRFL.Text.Trim();
                                if (hashTableInfor["买卖家个人经纪人资格证书编号"].ToString() == HTuser["原经纪人资格证书编号"].ToString())
                                {
                                    hashTableInfor["买卖家个人是否更换经纪人资格证书编号"] = "否";
                                }
                                else
                                {
                                    hashTableInfor["买卖家个人是否更换经纪人资格证书编号"] = "是";
                                }
                            }
                            else
                            {
                                this.lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                this.lablRemJJRZGZS.Visible = true;

                                tag += 1;

                            }
                        }
                        else if (this.radFW_Bank.Checked == true)
                        {
                            if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                            {
                                tag += 1;
                                this.labRemYHMC.Visible = true;
                            }
                            else
                            {
                                hashTableInfor["买卖家个人关联银行"] = this.labYHMC.Text.Trim();
                                this.labRemYHMC.Visible = false;
                            }
                            if (uctextBankYGGH.Enabled == false)
                            {
                                hashTableInfor["买卖家个人关联银行工作人员工号"] = "无";
                                this.lblBankYGGH.Visible = false;
                            }
                            else
                            {
                                if (this.uctextBankYGGH.Text.Trim() == "")
                                {
                                    tag += 1;
                                    this.lblBankYGGH.Visible = true;
                                }
                                else
                                {
                                    hashTableInfor["买卖家个人关联银行工作人员工号"] = this.uctextBankYGGH.Text.Trim();
                                    this.lblBankYGGH.Visible = false;
                                }
                            }
                            if (this.txtJJRZGZS.Text.Trim() == "")
                            {
                                //this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                                //this.lablRemJJRZGZS.Visible = true;
                                //tag += 1;
                            }
                            else if (this.txtJJRZGZS.Text.Trim() != "" && this.txtJJRMC.Text.Trim() != "" && this.txtJJRLXDH.Text.Trim() != "")
                            {
                                this.lablRemJJRZGZS.Visible = false;
                                hashTableInfor["买卖家个人经纪人资格证书编号"] = this.txtJJRZGZS.Text.Trim();
                                hashTableInfor["买卖家个人经纪人经纪人名称"] = this.txtJJRMC.Text.Trim();
                                hashTableInfor["买卖家个人经纪人联系电话"] = this.txtJJRLXDH.Text.Trim();
                                hashTableInfor["买卖家个人业务服务部门"] = lblJJRFL.Text.Trim();
                                if (hashTableInfor["买卖家个人经纪人资格证书编号"].ToString() == HTuser["原经纪人资格证书编号"].ToString())
                                {
                                    hashTableInfor["买卖家个人是否更换经纪人资格证书编号"] = "否";
                                }
                                else
                                {
                                    hashTableInfor["买卖家个人是否更换经纪人资格证书编号"] = "是";
                                }
                            }
                            else
                            {
                                //this.lablRemJJRZGZS.Text = "请填写可用的经纪人资格证书编号！";
                                //this.lablRemJJRZGZS.Visible = true;
                                //tag += 1;

                            }
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(this.labYHMC.Text.Trim()))
                            {
                                tag += 1;
                                this.labRemYHMC.Visible = true;
                            }
                            else
                            {
                                this.labRemYHMC.Visible = false;
                            }

                            if (String.IsNullOrEmpty(this.labZFMC.Text))
                            {
                                tag += 1;
                                this.labRemZFMC.Visible = true;
                            }
                            else
                            {
                                this.labRemZFMC.Visible = false;
                            }

                            if (String.IsNullOrEmpty(this.labHangYeXieHui.Text))
                            {
                                tag += 1;
                                this.labRemHangYeXieHui.Visible = true;
                            }
                            else
                            {
                                this.labRemHangYeXieHui.Visible = false;
                            }

                            if (this.txtJJRZGZS.Text.Trim() == "")
                            {
                                this.lablRemJJRZGZS.Text = "请填写您的经纪人资格证书编号！";
                                this.lablRemJJRZGZS.Visible = true;
                          
                                tag += 1;
                            }
                            else
                            {
                                this.lablRemJJRZGZS.Text = "您填写的经纪人资格证书编号无效！";
                                this.lablRemJJRZGZS.Visible = true;
                                tag += 1;
                            }
                        }
                    if (tag == 0)
                    {
                        //禁用提交区域并开启进度
                        flowLayoutPanel1.Enabled = false;
                        PBload.Visible = true;
                        //提交账户信息
                        SRT_UpdateJYZHXX_Run();
                    }
                    else
                    {
                        ScrollToPoint();
                        return;
                    }

                }
                #endregion
            }
            #endregion
        }


        //开启一个测试线程,更新修改的账户数据信息
        private void SRT_UpdateJYZHXX_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTableInfor, new delegateForThread(SRT_UpdateJYZHXX));
            Thread trd = new Thread(new ThreadStart(OTD.UpdateJYZHXX));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_UpdateJYZHXX(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_UpdateJYZHXX_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_UpdateJYZHXX_Invoke(Hashtable OutPutHT)
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
                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showstr);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "", Almsg3);
                    FRSE3.ShowDialog();
                    PublicDS.PublisDsUser = dsreturn;
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
        /// 根据提示信息，自动定位到首个提示的位置
        /// </summary>
        private void ScrollToPoint()
        {
          
            Point scrollPoint = new Point();
            if (this.labRemJYZH.Visible == true)
            {
                scrollPoint.X = this.labRemJJFMC.Parent.Location.X;
                scrollPoint.Y = this.labRemJJFMC.Parent.Location.Y - 28;
            }
            else if (this.labRemZCLB.Visible==true)
            {
                scrollPoint.X = this.labRemZCLB.Parent.Location.X;
                scrollPoint.Y = this.labRemZCLB.Parent.Location.Y - 28;
            }
            else if (this.labRemJJFMC.Visible == true)
            {
                scrollPoint.X = this.labRemJJFMC.Parent.Location.X;
                scrollPoint.Y = this.labRemJJFMC.Parent.Location.Y - 28;
            }
            else if (this.labRemYYZZ.Visible == true)
            {
                scrollPoint.X = this.labRemYYZZ.Parent.Location.X;
                scrollPoint.Y = this.labRemYYZZ.Parent.Location.Y - 28;
            }
            else if (this.labRemSFZ.Visible==true)
            {
                scrollPoint.X = this.labRemSFZ.Parent.Location.X;
                scrollPoint.Y = this.labRemSFZ.Parent.Location.Y - 28;
            }
            else if (this.labRemZZJGDMZ.Visible==true)
            {
                scrollPoint.X = this.labRemZZJGDMZ.Parent.Location.X;
                scrollPoint.Y = this.labRemZZJGDMZ.Parent.Location.Y - 28;
            }
            else if (this.labRemSWDJZ.Visible == true)
            {
                scrollPoint.X = this.labRemSWDJZ.Parent.Location.X;
                scrollPoint.Y = this.labRemSWDJZ.Parent.Location.Y - 28;
            }
            else if (this.labRemNSRZGZ.Visible==true)
            {
                scrollPoint.X = this.labRemNSRZGZ.Parent.Location.X;
                scrollPoint.Y = this.labRemNSRZGZ.Parent.Location.Y - 28;
            }
            else if (this.labRemKHXKZ.Visible == true)
            {
                scrollPoint.X = this.labRemKHXKZ.Parent.Location.X;
                scrollPoint.Y = this.labRemKHXKZ.Parent.Location.Y - 28;
            }
            else if (this.labRemFDDBR.Visible == true)
            {
                scrollPoint.X = this.labRemFDDBR.Parent.Location.X;
                scrollPoint.Y = this.labRemFDDBR.Parent.Location.Y - 28;
            }
            else if (this.labRemFDDBRSHZH.Visible == true)
            {
                scrollPoint.X = this.labRemFDDBRSHZH.Parent.Location.X;
                scrollPoint.Y = this.labRemFDDBRSHZH.Parent.Location.Y - 28;
            }
            else if (this.labRemFDDBRSQS.Visible == true)
            {
                scrollPoint.X = this.labRemFDDBRSQS.Parent.Location.X;
                scrollPoint.Y = this.labRemFDDBRSQS.Parent.Location.Y - 28;
            }
            else if (this.labRemJYFLXDH.Visible == true)
            {
                scrollPoint.X = this.labRemJYFLXDH.Parent.Location.X;
                scrollPoint.Y = this.labRemJYFLXDH.Parent.Location.Y - 28;
            }
            else if (this.labRemSSQY.Visible == true)
            {
                scrollPoint.X = this.labRemSSQY.Parent.Location.X;
                scrollPoint.Y = this.labRemSSQY.Parent.Location.Y - 28;
            }
            else if (this.labRemXXDZ.Visible == true)
            {
                scrollPoint.X = this.labRemXXDZ.Parent.Location.X;
                scrollPoint.Y = this.labRemXXDZ.Parent.Location.Y - 28;
            }
            else if (this.labRemLXRXM.Visible == true)
            {
                scrollPoint.X = this.labRemLXRXM.Parent.Location.X;
                scrollPoint.Y = this.labRemLXRXM.Parent.Location.Y - 28;
            }
            else if (this.labRemLXRSJH.Visible == true)
            {
                scrollPoint.X = this.labRemLXRSJH.Parent.Location.X;
                scrollPoint.Y = this.labRemLXRSJH.Parent.Location.Y - 28;
            }
            else if (this.labRemKHYH.Visible == true)
            {
                scrollPoint.X = this.labRemKHYH.Parent.Location.X;
                scrollPoint.Y = this.labRemKHYH.Parent.Location.Y - 28;
            }
            else if (this.labRemYHZH.Visible == true)
            {
                scrollPoint.X = this.labRemYHZH.Parent.Location.X;
                scrollPoint.Y = this.labRemYHZH.Parent.Location.Y - 28;
            }
            else if (this.labRemGLJG.Visible == true)
            {
                scrollPoint.X = this.labRemGLJG.Parent.Location.X;
                scrollPoint.Y = this.labRemGLJG.Parent.Location.Y - 28;
            }
            else if (this.lablRemJJRZGZS.Visible == true)
            {
                scrollPoint.X = this.lablRemJJRZGZS.Parent.Location.X;
                scrollPoint.Y = this.lablRemJJRZGZS.Parent.Location.Y - 28;
            }
            else if (this.lablRemJJRMC.Visible == true)
            {
                scrollPoint.X = this.lablRemJJRMC.Parent.Location.X;
                scrollPoint.Y = this.lablRemJJRMC.Parent.Location.Y - 28;
            }
            else if (this.lablRemJJRLXDH.Visible == true)
            {
                scrollPoint.X = this.lablRemJJRLXDH.Parent.Location.X;
                scrollPoint.Y = this.lablRemJJRLXDH.Parent.Location.Y - 28;
            }

            this.AutoScrollPosition = scrollPoint;
        
        }










        #endregion

      

       
        /// <summary>
        /// 重置表单
        /// </summary>
        private void reset()
        {
            ucZHZL_B s = new ucZHZL_B();
            s.Dock = DockStyle.Fill;//铺满
            s.AutoScroll = true;//出现滚动条
            s.BackColor = Color.AliceBlue;

            //清理控件
            Panel P = (Panel)this.Parent;
            P.Controls.Clear();
            P.Controls.Add(s); //加入到某一个panel中
            s.Show();//显示出来
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
            cbxKHYH.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
            BindData();
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["证券资金账号"].ToString().Trim() != "")
            { label2.Text = "交易方编号：" + PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["证券资金账号"].ToString().Trim() + ""; }

            //打开开通交易账户申请，用计时器延时打开，防止bug
            timer1.Enabled = true;

            //设置加水印加字样
            this.pan_SC_YYZZ.ManageData = "加水印加字样";
            this.pan_SC_SFZ.ManageData = "加水印加字样";
            this.pan_SC_ZZJGDMZ.ManageData = "加水印加字样";
            this.pan_SC_SWDJZ.ManageData = "加水印加字样";

            this.pan_SC_NSRZGZ.ManageData = "加水印加字样";
            this.pan_SC_KHXKZ.ManageData = "加水印加字样";
            this.pan_SC_YLYJK.ManageData = "加水印加字样";
            this.pan_SC_FDDBRSHZH.ManageData = "加水印加字样";
            this.pan_SC_FDDBRSQS.ManageData = "加水印加字样";

            this.pan_SC_SFZ_FM.ManageData = "加水印加字样";
            this.pan_SC_FDDBRSHZH_FM.ManageData = "加水印加字样";



            //将滚动条置顶
            UPUP();
            //设置进度条的位置，放到按钮旁边
            this.PBload.Location = new Point(this.flowLayoutPanel1.Location.X + this.panTiJiao.Location.X + this.btnSave.Location.X + this.btnSave.Width + 25, this.flowLayoutPanel1.Location.Y + this.panTiJiao.Location.Y + this.btnSave.Location.Y);

            //处理下拉框间距
            cbxGLJG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CB_DrawItem);
        }


        //处理下拉框间距
        private void CB_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox CBthis = (ComboBox)sender;
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(CBthis.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X + 2, e.Bounds.Y + 3);
        }

       /// <summary>
       /// 绑定弹窗处理函数
       /// </summary>
       /// <param name="OutPutHT"></param>
        private void TanChuang_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(TanChuang_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void TanChuang_demo_Invoke(Hashtable OutPutHT)
        {
          

         
        }

        private void LLlishi_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ucZHZL_SHXQ ZHZL = new ucZHZL_SHXQ(null);
            ZHZL.ShowDialog();

        }


        /// <summary>
        /// 系统根据用户选择的所属区域帮助选择，相应的平台管理机构
        /// </summary>
        private void HelpSelectGLJG()
        {
            if (!ucSSQY.SelectedItem[0].ToString().Contains("请选择") && !ucSSQY.SelectedItem[1].ToString().Contains("请选择"))
            {
                if (this.radJJR.Checked)
                {

                    string strSSSF = ucSSQY.SelectedItem[0].ToString();
                    string strSSDS = ucSSQY.SelectedItem[1].ToString();
                    if (strSSSF != strTagSF && strSSDS != strTagDS)
                    {
                        DataSet dataSet = PublicDS.PublisDsData;
                        DataRow[] dataRow = dataSet.Tables["分公司对照表"].Select(" Pname ='" + strSSSF + "' and Cname='" + strSSDS + "'");
                        if (dataRow.Length > 0)
                        {
                            for (int i = 0; i < cbxGLJG.Items.Count; i++)
                            {
                                string strSelectItem = cbxGLJG.Items[i].ToString();
                                string strFGSName = dataRow[0]["FGSname"].ToString();
                                if (strSelectItem == strFGSName)
                                {
                                    cbxGLJG.SelectedIndex = i;
                                }
                            }
                            strTagSF = ucSSQY.SelectedItem[0].ToString();
                            strTagDS = ucSSQY.SelectedItem[1].ToString();
                        }
                    }
                }

            }
        }

        private void timer_PTGLJG_Tick(object sender, EventArgs e)
        {
           //if (ucSSQY.SelectedItem[0].ToString() != strTagSF_Real && ucSSQY.SelectedItem[1].ToString() != strTagDS_Real)
           // {
            HelpSelectGLJG();
            //}
        }

        private void cbxGLJG_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.radJJR.Checked)
            {
                this.timer_PTGLJG.Enabled = false;
            }
        }

        /// <summary>
        /// 隐藏所有提示信息
        /// </summary>
        private void SetRemindInforInVisiable()
        {
            this.labRemJYZH.Visible = false;
            this.labRemZCLB.Visible = false;
            this.labRemJJFMC.Visible = false;
            this.labRemYYZZ.Visible = false;
            this.labRemSFZ.Visible = false;
            this.labRemZZJGDMZ.Visible = false;
            this.labRemSWDJZ.Visible = false;
            this.labRemNSRZGZ.Visible = false;
            this.labRemKHXKZ.Visible = false;
            this.labRemFDDBR.Visible = false;
            this.labRemFDDBRSHZH.Visible = false;
            this.labRemFDDBRSQS.Visible = false;
            this.labRemJYFLXDH.Visible = false;
            this.labRemSSQY.Visible = false;
            this.labRemXXDZ.Visible = false;
            this.labRemLXRXM.Visible = false;
            this.labRemLXRSJH.Visible = false;
            this.labRemKHYH.Visible = false;
            this.labRemYHZH.Visible = false;
            this.labRemGLJG.Visible = false;
            this.lablRemJJRZGZS.Visible = false;
            this.lablRemJJRMC.Visible = false;
            this.lablRemJJRLXDH.Visible = false;
            //this.TipsSFZZFM.Visible = true;
            //this.TipFRSFZ.Visible = true;
        }

        #region//模板下载和协议查看

        /// <summary>
        /// 账户变更申请表模板下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkZHBGSQBMB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/JHJX_File/账户变更申请表模版.zip";
            StringOP.OpenUrl(url);
        }

        /// <summary>
        /// 下载开通须知和账户资料申请表模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkKTXZHKTZLSQB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/JHJX_File/开通须知和开户资料申报表模板.zip";
            StringOP.OpenUrl(url);
        }

        /// <summary>
        /// 查看账户开通协议
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkZHKTXY_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.linkZHKTXY.Text.ToString() == "查看交易方交易账户开通协议")
            {
                int pagenumber = 4;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 1];
                string[] MKarr = new string[pagenumber + 1];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTMJMJZHKTXY.aspx?Action=KT_ZH&Number=" + HTuser["登录_Number"].ToString();
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTZHKTXY";

                }
                CSarr[pagenumber] = "附件";
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTZHKTFJ";
                FormZHKTXY dy = new FormZHKTXY("交易方账户开通协议", CSarr, MKarr, "查看并打印");
                dy.Show();
            }
            else if (this.linkZHKTXY.Text.ToString() == "查看经纪人交易账户开通协议")
            {
                int pagenumber = 3;//预估页数，要大于可能的最大页数
                object[] CSarr = new object[pagenumber + 1];
                string[] MKarr = new string[pagenumber + 1];
                for (int i = 0; i < pagenumber; i++)
                {
                    Hashtable htcs = new Hashtable();
                    htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/FMPTJJRZHKTXY.aspx?Action=KT_ZH&Number=" + HTuser["登录_Number"].ToString();
                    htcs["要传递的参数1"] = "参数1";
                    CSarr[i] = htcs; //模拟参数
                    MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTZHKTXY";

                }
                CSarr[pagenumber] = "附件";
                MKarr[pagenumber] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.MuBan.UCFMPTZHKTFJ";
                FormZHKTXY dy = new FormZHKTXY("经纪人账户开通协议", CSarr, MKarr, "查看并打印");
                dy.Show();

            }
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            //打开开通交易账户申请
            timer1.Enabled = false;
            if (PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["结算账户类型"].ToString() == "")
            {

                Hashtable hts = new Hashtable();
                hts["dlyx"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString();
                hts["dlmm"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录密码"].ToString();
                hts["yhm"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["用户名"].ToString();

                Program.fms = new FormKTJYZH(hts);
                SPopenDialog(Program.fms, true, hts);
            }
        }
        /// <summary>
        /// 交易方名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJJFMC_Leave(object sender, EventArgs e)
        {
            if (this.radZRR.Checked == true)
            {
                if (String.IsNullOrEmpty(this.txtLXRXM.Text.Trim()))
                {
                    this.txtLXRXM.Text = this.txtJJFMC.Text;

                }
            }
        }
        /// <summary>
        /// 交易方联系电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJYFLXDH_Leave(object sender, EventArgs e)
        {
            if (this.radZRR.Checked == true)
            {
                if (String.IsNullOrEmpty(this.txtLXRSJH.Text.Trim()) && txtJYFLXDH.Text.Trim().IndexOf('-') == -1)//没有中划线
                {
                    this.txtLXRSJH.Text = txtJYFLXDH.Text;
                }
            }
        }


        /// <summary>
        /// 选择分公司
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radFGS_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLJG.Visible = false;
                this.panTWGLBM.Visible = false;
                this.panSZYX.Visible = false;

                #region//绑定业务管理部门
                if (!ucSSQY.SelectedItem[0].ToString().Contains("请选择") && !ucSSQY.SelectedItem[1].ToString().Contains("请选择"))
                {
                    if (this.radJJR.Checked)
                    {
                        string strSSSF = ucSSQY.SelectedItem[0].ToString();
                        string strSSDS = ucSSQY.SelectedItem[1].ToString();
                        DataSet dataSet = PublicDS.PublisDsData;
                        DataRow[] dataRow = dataSet.Tables["分公司对照表"].Select(" Pname ='" + strSSSF + "' and Cname='" + strSSDS + "'");
                        if (dataRow.Length > 0)
                        {
                            for (int i = 0; i < cbxGLJG.Items.Count; i++)
                            {
                                string strSelectItem = cbxGLJG.Items[i].ToString();
                                string strFGSName = dataRow[0]["FGSname"].ToString();
                                if (strSelectItem == strFGSName)
                                {
                                    cbxGLJG.SelectedIndex = i;
                                }
                            }
                            strTagSF = ucSSQY.SelectedItem[0].ToString();
                            strTagDS = ucSSQY.SelectedItem[1].ToString();
                        }
                    }
                }
                #endregion

            }

        }

        /// <summary>
        /// 选择业务拓展部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radYWTZB_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLJG.Visible = false;
                this.panTWGLBM.Visible = false;
                this.panSZYX.Visible = false;
            }
        }

        /// <summary>
        /// 选择高校团委
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radGXTW_CheckedChanged(object sender, bool Checked)
        {
            if (Checked == true)
            {
                this.panGLJG.Visible = false;
                this.panTWGLBM.Visible = true;
                this.panSZYX.Visible = true;
                ComboBoxItem cbxItem;
                cbxItem = new ComboBoxItem();
                cbxItem.Text = "请选择院系";
                cbxItem.Value = "请选择院系";
                //cbxSZYX.Items.Insert(0, cbxItem);
                //cbxSZYX.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 选择经纪人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radFW_JJR_CheckedChanged(object sender, bool Checked)
        {
            if (radFW_JJR.Enabled==false)
            {
                return;
            }
            if (Checked == true)
            {
                this.panGLYH.Visible = false;
                this.panBankYGGH.Visible = false;
                this.panGLZFJG.Visible = false;
                this.panHYXH.Visible = false;

                panZGZS.Visible = true;
                panJJRMC.Visible = true;
                panJJRLXDH.Visible = true;

                //清空银行相关数据
                labYHMC.Text = "";
                lblBankDLYX.Text = "";
                uctextBankYGGH.Text = "";

                //清空共同的数据
                lblJJRFL.Text = "";
                txtJJRZGZS.Text = "";
                txtJJRMC.Text = "";
                txtJJRLXDH.Text = "";
            }
        }

        /// <summary>
        /// 选择银行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radFW_Bank_CheckedChanged(object sender, bool Checked)
        {
            if (radFW_Bank.Enabled == false)
            {
                return;
            }
            if (Checked == true)
            {
              
                this.panGLYH.Visible = true;
                this.panBankYGGH.Visible = true;
                this.panGLZFJG.Visible = false;
                this.panHYXH.Visible = false;

                panZGZS.Visible = false;
                panJJRMC.Visible = false;
                panJJRLXDH.Visible = false;

                //清空共同的数据
                txtJJRZGZS.Text = "";
                txtJJRMC.Text = "";
                txtJJRLXDH.Text = "";

            }
        }

        /// <summary>
        /// 选择政府
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Checked"></param>
        private void radFW_ZF_CheckedChanged(object sender, bool Checked)
        {
            if (radFW_ZF.Enabled == false)
            {
                return;
            }
            if (Checked == true)
            {
              
                this.panGLYH.Visible = false;
                this.panBankYGGH.Visible = false;
                this.panGLZFJG.Visible = true;
                this.panHYXH.Visible = false;

                panZGZS.Visible = true;
                panJJRMC.Visible = true;
                panJJRLXDH.Visible = true;

                //清空银行相关数据
                labYHMC.Text = "";
                lblBankDLYX.Text = "";
                uctextBankYGGH.Text = "";

                //清空共同的数据
                lblJJRFL.Text = "";
                txtJJRZGZS.Text = "";
                txtJJRMC.Text = "";
                txtJJRLXDH.Text = "";

            }
        }

        /// <summary>
        ///查找并选择团委名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labCKTWBXZ_Click(object sender, EventArgs e)
        {
            fmSelectGXTW fm = new fmSelectGXTW(new delegateForThread(XZGXTWMC_demo));
            fm.ShowDialog();
        }
        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void XZGXTWMC_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(XZGXTWMC_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void XZGXTWMC_demo_Invoke(Hashtable OutPutHT)
        {
            this.labTWMC.Text = OutPutHT["高校团委名称"].ToString();
            this.labGXTW_ZhangHao.Text = OutPutHT["高校账号"].ToString();
            //HelperSelectYX();
            //this.timer_XZYX.Enabled = true;
        }

        /// <summary>
        /// 选择院系的代码
        /// </summary>
        private void HelperSelectYX()
        {
            if (this.radGXTW.Checked == true)//选择高校团委
            {
                if (!string.IsNullOrEmpty(this.labGXTW_ZhangHao.Text.Trim()))
                {
                    //cbxSZYX.Items.Clear();
                    ComboBoxItem cbxItem;
                    DataSet dataSet = PublicDS.PublisDsData;
                    cbxItem = new ComboBoxItem();
                    cbxItem.Text = "请选择院系";
                    cbxItem.Value = "请选择院系";
                    //cbxSZYX.Items.Insert(0, cbxItem);

                    string[] distinctcols = new string[] { "院系Number", "高校账户", "院系名称" };
                    DataTable dtfd = new DataTable("distinctTable");
                    DataView mydataview = new DataView(dataSet.Tables["院系表"]);
                    dtfd = mydataview.ToTable(true, distinctcols);
                    DataRow[] dataRow = dtfd.Select("高校账户='" + this.labGXTW_ZhangHao.Text.Trim() + "'");
                    foreach (DataRow dr in dataRow)
                    {
                        cbxItem = new ComboBoxItem();
                        cbxItem.Text = dr["院系名称"].ToString();
                        cbxItem.Value = dr["院系Number"].ToString();
                        //cbxSZYX.Items.Add(cbxItem);

                    }
                    //cbxSZYX.SelectedIndex = 0;

                }

            }
        }

        /// <summary>
        /// 关联银行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labCKYHBXZ_Click(object sender, EventArgs e)
        {
            fmSelectBank fm = new fmSelectBank(new delegateForThread(SelectBank_demo));
            fm.ShowDialog();
        }
        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectBank_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SelectBank_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectBank_demo_Invoke(Hashtable OutPutHT)
        {
            this.labYHMC.Text = OutPutHT["银行名称"].ToString();
            lblBankDLYX.Text = OutPutHT["银行登录邮箱"].ToString();

            this.txtJJRZGZS.Text = OutPutHT["经纪人资格证书"].ToString();
            this.txtJJRMC.Text = OutPutHT["交易方名称"].ToString();
            this.txtJJRLXDH.Text = OutPutHT["联系电话"].ToString();
            this.lblJJRFL.Text = OutPutHT["经纪人分类"].ToString();
        }

        /// <summary>
        /// 政府名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labCKZFBXZ_Click(object sender, EventArgs e)
        {
            fmSelectGover fm = new fmSelectGover(new delegateForThread(SelectGover_demo));
            fm.ShowDialog();

        }
        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectGover_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SelectGover_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectGover_demo_Invoke(Hashtable OutPutHT)
        {
            this.labYHMC.Text = OutPutHT["政府名称"].ToString();
        }

        private void radHangYeXieHui_CheckedChanged(object sender, bool Checked)
        {
            if (radHangYeXieHui.Enabled==false)
            {
                return;
            }
            if (Checked == true)
            {
                this.panGLYH.Visible = false;
                this.panBankYGGH.Visible = false;
                this.panGLZFJG.Visible = false;
                this.panHYXH.Visible = true;


                panZGZS.Visible = true;
                panJJRMC.Visible = true;
                panJJRLXDH.Visible = true;

                //清空银行相关数据
                labYHMC.Text = "";
                lblBankDLYX.Text = "";
                uctextBankYGGH.Text = "";

                //清空共同的数据
                lblJJRFL.Text = "";
                txtJJRZGZS.Text = "";
                txtJJRMC.Text = "";
                txtJJRLXDH.Text = "";
            }
        }

        /// <summary>
        ///查看并选择行业协会
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labCKBXZHYXH_Click(object sender, EventArgs e)
        {
            fmSelectHYXH fm = new fmSelectHYXH(new delegateForThread(XZGXTWMC_demo));
            fm.ShowDialog();
        }

        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void HYXH_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(HYXH_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void HYXH_demo_Invoke(Hashtable OutPutHT)
        {
            this.labTWMC.Text = OutPutHT["行业协会名称"].ToString();
            //HelperSelectYX();
            //this.timer_XZYX.Enabled = true;
        }

        #region//选择工号

        private void lblSelectBankYGGH_Click(object sender, EventArgs e)
        {
            fmSelectBankYG bankyg = new fmSelectBankYG(new delegateForThread(SelectBankYGGH_demo), this.lblBankDLYX.Text.Trim());
            bankyg.Show();
        }
        /// <summary>
        /// 绑定弹窗处理函数
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectBankYGGH_demo(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SelectBankYGGH_demo_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }


        /// <summary>
        /// 处理非线程创建的控件
        /// </summary>
        /// <param name="OutPutHT"></param>
        private void SelectBankYGGH_demo_Invoke(Hashtable OutPutHT)
        {
            this.uctextBankYGGH.Text = OutPutHT["员工工号"].ToString();
        }


        #endregion

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/JHJX_File/交易方预留印鉴表.zip";
            StringOP.OpenUrl(url);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/JHJX_File/法定代表人授权书.zip";
            StringOP.OpenUrl(url);
        }
             



    }
}
